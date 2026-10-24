using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BatManager.Models;

namespace BatManager.Services
{
    /// <summary>
    /// Risultato di una validazione del nome file.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; }

        private ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static ValidationResult Success()
        {
            return new ValidationResult(true, null);
        }

        public static ValidationResult Fail(string message)
        {
            return new ValidationResult(false, message);
        }
    }

    /// <summary>
    /// Repository per la persistenza su disco degli script .bat nella cartella "scripts/".
    /// Gestisce codifica UTF-8/CP850, validazione nomi file Windows e operazioni CRUD.
    /// </summary>
    public class ScriptRepository
    {
        private static readonly HashSet<string> ReservedWindowsNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

        private static readonly char[] ExplicitInvalidChars = new[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

        public string ScriptsDirectory { get; private set; }

        /// <summary>
        /// Codifica predefinita per la scrittura (UTF-8 senza BOM per massima compatibilità).
        /// </summary>
        public Encoding DefaultEncoding { get; set; }

        public ScriptRepository(string customDirectory = null)
        {
            DefaultEncoding = new UTF8Encoding(false);

            if (!string.IsNullOrWhiteSpace(customDirectory))
            {
                ScriptsDirectory = customDirectory;
                EnsureDirectoryExists();
            }
            else
            {
                ScriptsDirectory = ResolveDefaultScriptsDirectory();
                InitializeAndMigrateScripts();
            }
        }

        /// <summary>
        /// Risolve il percorso permanente predefinito in %LOCALAPPDATA%\BAT-Manager\scripts.
        /// Questo garantisce che l'eseguibile, anche se posizionato sul Desktop, salvi gli script
        /// in un'unica cartella centrale permanente senza creare cartelle sul Desktop.
        /// </summary>
        public static string ResolveDefaultScriptsDirectory()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(appData, "BAT-Manager", "scripts");
        }

        private void InitializeAndMigrateScripts()
        {
            EnsureDirectoryExists();

            // 1. Migra automaticamente gli script presenti nella cartella locale dell'eseguibile (se non sul Desktop)
            string localBaseScripts = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");
            CopyMissingScripts(localBaseScripts, ScriptsDirectory);

            // 2. Migra automaticamente gli script dalla cartella Release/Debug del progetto di sviluppo se presente
            string desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string devReleaseScripts = Path.Combine(desktopDir, "BAT-Manager", "BAT-Manager", "bin", "Release", "scripts");
            if (Directory.Exists(devReleaseScripts))
            {
                CopyMissingScripts(devReleaseScripts, ScriptsDirectory);
            }

            string devProjectScripts = Path.Combine(desktopDir, "BAT-Manager", "BAT-Manager", "scripts");
            if (Directory.Exists(devProjectScripts))
            {
                CopyMissingScripts(devProjectScripts, ScriptsDirectory);
            }
        }

        private static void CopyMissingScripts(string sourceDir, string targetDir)
        {
            if (string.IsNullOrWhiteSpace(sourceDir) || !Directory.Exists(sourceDir)) return;
            if (string.Equals(sourceDir.TrimEnd('\\', '/'), targetDir.TrimEnd('\\', '/'), StringComparison.OrdinalIgnoreCase)) return;

            try
            {
                var files = Directory.GetFiles(sourceDir, "*.bat").Concat(Directory.GetFiles(sourceDir, "*.cmd"));
                foreach (var file in files)
                {
                    string dest = Path.Combine(targetDir, Path.GetFileName(file));
                    if (!File.Exists(dest))
                    {
                        File.Copy(file, dest, false);
                    }
                }
            }
            catch { }
        }

        public void EnsureDirectoryExists()
        {
            if (!Directory.Exists(ScriptsDirectory))
            {
                Directory.CreateDirectory(ScriptsDirectory);
            }
        }

        /// <summary>
        /// Carica tutti gli script batch (.bat e .cmd) presenti nella cartella scripts/.
        /// </summary>
        public List<BatScript> LoadAll()
        {
            EnsureDirectoryExists();
            var scripts = new List<BatScript>();

            var files = Directory.GetFiles(ScriptsDirectory, "*.bat")
                .Concat(Directory.GetFiles(ScriptsDirectory, "*.cmd"))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(f => Path.GetFileName(f), StringComparer.CurrentCultureIgnoreCase);

            foreach (var filePath in files)
            {
                try
                {
                    var fileInfo = new FileInfo(filePath);
                    string rawContent = ReadFileWithFallback(filePath);
                    var script = BatParser.Parse(rawContent, fileInfo.Name, fileInfo.FullName);
                    script.LastModified = fileInfo.LastWriteTime;
                    scripts.Add(script);
                }
                catch (Exception ex)
                {
                    scripts.Add(new BatScript(
                        Path.GetFileName(filePath),
                        string.Format("[Read error: {0}]", ex.Message),
                        string.Empty,
                        Path.GetFileName(filePath),
                        filePath
                    ));
                }
            }

            return scripts;
        }

        /// <summary>
        /// Cerca uno script per nome file.
        /// </summary>
        public BatScript GetByFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            string fullPath = Path.Combine(ScriptsDirectory, fileName);
            if (!File.Exists(fullPath)) return null;

            var fileInfo = new FileInfo(fullPath);
            string rawContent = ReadFileWithFallback(fullPath);
            var script = BatParser.Parse(rawContent, fileInfo.Name, fileInfo.FullName);
            script.LastModified = fileInfo.LastWriteTime;
            return script;
        }

        /// <summary>
        /// Salva uno script su disco. Se previousFileName è specificato e diverso da script.FileName,
        /// il vecchio file viene eliminato (rinomina).
        /// </summary>
        public void Save(BatScript script, string previousFileName = null)
        {
            if (script == null) throw new ArgumentNullException("script");

            string targetFileName = EnsureBatExtension(script.FileName);
            script.FileName = targetFileName;

            var validation = ValidateFileName(targetFileName, previousFileName);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(validation.ErrorMessage);
            }

            EnsureDirectoryExists();
            string targetPath = Path.Combine(ScriptsDirectory, targetFileName);

            string fullContent = BatParser.BuildFileContent(script.Name, script.Description, script.Content, script.RunAsAdmin);

            File.WriteAllText(targetPath, fullContent, DefaultEncoding);

            if (!string.IsNullOrWhiteSpace(previousFileName) &&
                !string.Equals(previousFileName, targetFileName, StringComparison.OrdinalIgnoreCase))
            {
                string oldPath = Path.Combine(ScriptsDirectory, previousFileName);
                if (File.Exists(oldPath))
                {
                    try { File.Delete(oldPath); } catch { }
                }
            }

            var fi = new FileInfo(targetPath);
            script.FullPath = fi.FullName;
            script.LastModified = fi.LastWriteTime;
        }

        /// <summary>
        /// Elimina uno script da disco.
        /// </summary>
        public void Delete(BatScript script)
        {
            if (script == null) throw new ArgumentNullException("script");

            string targetPath = !string.IsNullOrWhiteSpace(script.FullPath)
                ? script.FullPath
                : Path.Combine(ScriptsDirectory, script.FileName);

            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }
        }

        /// <summary>
        /// Rinomina uno script esistente.
        /// </summary>
        public void Rename(BatScript script, string newFileName)
        {
            if (script == null) throw new ArgumentNullException("script");
            string previous = script.FileName;
            script.FileName = newFileName;
            Save(script, previous);
        }

        /// <summary>
        /// Valida il nome del file secondo le regole di Windows e controlla la presenza di duplicati.
        /// </summary>
        public ValidationResult ValidateFileName(string fileName, string currentFileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return ValidationResult.Fail("File name cannot be empty.");
            }

            fileName = fileName.Trim();

            char[] invalidChars = Path.GetInvalidFileNameChars().Concat(ExplicitInvalidChars).Distinct().ToArray();
            if (fileName.IndexOfAny(invalidChars) >= 0)
            {
                return ValidationResult.Fail("File name contains invalid characters (\\ / : * ? \" < > |).");
            }

            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            if (ReservedWindowsNames.Contains(nameWithoutExt))
            {
                return ValidationResult.Fail(string.Format("'{0}' is a reserved Windows system name and cannot be used.", nameWithoutExt));
            }

            string targetPath = Path.Combine(ScriptsDirectory, fileName);
            bool isSameFile = !string.IsNullOrWhiteSpace(currentFileName) &&
                              string.Equals(fileName, currentFileName, StringComparison.OrdinalIgnoreCase);

            if (!isSameFile && File.Exists(targetPath))
            {
                return ValidationResult.Fail(string.Format("A file named '{0}' already exists. Please choose a different name.", fileName));
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Genera un nome file valido a partire dal titolo dello script.
        /// </summary>
        public static string GenerateSlugFileName(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return "new_script.bat";
            }

            var sb = new StringBuilder();
            bool lastWasHyphen = false;

            foreach (char c in title)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                    lastWasHyphen = false;
                }
                else if (c == '_' || c == '-')
                {
                    if (!lastWasHyphen)
                    {
                        sb.Append('-');
                        lastWasHyphen = true;
                    }
                }
                else if (char.IsWhiteSpace(c) || ExplicitInvalidChars.Contains(c))
                {
                    if (!lastWasHyphen && sb.Length > 0)
                    {
                        sb.Append('-');
                        lastWasHyphen = true;
                    }
                }
            }

            string slug = sb.ToString().Trim('-');
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = "script";
            }

            if (ReservedWindowsNames.Contains(slug))
            {
                slug = "_" + slug;
            }

            return slug + ".bat";
        }

        private static string EnsureBatExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "script.bat";
            }

            string ext = Path.GetExtension(fileName);
            if (!string.Equals(ext, ".bat", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(ext, ".cmd", StringComparison.OrdinalIgnoreCase))
            {
                return fileName + ".bat";
            }

            return fileName;
        }

        /// <summary>
        /// Restituisce un nome file non in conflitto nella cartella scripts/.
        /// Se il nome desiderato esiste già, aggiunge un suffisso numerico (-1, -2...).
        /// </summary>
        public string GetAvailableFileName(string desiredName)
        {
            if (string.IsNullOrWhiteSpace(desiredName))
            {
                desiredName = "script.bat";
            }

            desiredName = EnsureBatExtension(desiredName);
            if (ValidateFileName(desiredName).IsValid)
            {
                return desiredName;
            }

            string nameWithoutExt = Path.GetFileNameWithoutExtension(desiredName);
            string ext = Path.GetExtension(desiredName);

            int counter = 1;
            while (counter < 1000)
            {
                string candidate = string.Format("{0}-{1}{2}", nameWithoutExt, counter, ext);
                if (ValidateFileName(candidate).IsValid)
                {
                    return candidate;
                }
                counter++;
            }

            return string.Format("{0}-{1}{2}", nameWithoutExt, Guid.NewGuid().ToString("N").Substring(0, 4), ext);
        }

        /// <summary>
        /// Lettura resiliente: tenta UTF-8 stretto; in caso di caratteri non validi ricade su CP850 (OEM DOS/Windows) o ANSI.
        /// </summary>
        public string ReadFileWithFallback(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);

            try
            {
                var strictUtf8 = new UTF8Encoding(false, true);
                return strictUtf8.GetString(bytes);
            }
            catch (DecoderFallbackException)
            {
                try
                {
                    var cp850 = Encoding.GetEncoding(850);
                    return cp850.GetString(bytes);
                }
                catch
                {
                    return Encoding.Default.GetString(bytes);
                }
            }
        }
    }
}
