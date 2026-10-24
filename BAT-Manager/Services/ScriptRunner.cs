using System;
using System.Diagnostics;
using System.IO;
using BatManager.Models;

namespace BatManager.Services
{
    /// <summary>
    /// Servizio responsabile dell'esecuzione dei file .bat e dell'interazione con il sistema operativo.
    /// </summary>
    public static class ScriptRunner
    {
        /// <summary>
        /// Esegue lo script batch in una nuova finestra di cmd.exe.
        /// </summary>
        public static Process Run(BatScript script, bool asAdmin = false, bool keepWindowOpen = true)
        {
            if (script == null) throw new ArgumentNullException("script");
            return RunFile(script.FullPath, asAdmin, keepWindowOpen);
        }

        /// <summary>
        /// Esegue un file batch dato il percorso completo.
        /// </summary>
        public static Process RunFile(string fullPath, bool asAdmin = false, bool keepWindowOpen = true)
        {
            if (string.IsNullOrWhiteSpace(fullPath) || !File.Exists(fullPath))
            {
                throw new FileNotFoundException("The specified file does not exist.", fullPath);
            }

            string workingDir = Path.GetDirectoryName(fullPath);
            string switchCmd = keepWindowOpen ? "/k" : "/c";

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = string.Format("{0} \"\"{1}\"\"", switchCmd, fullPath),
                WorkingDirectory = workingDir,
                UseShellExecute = true
            };

            if (asAdmin)
            {
                psi.Verb = "runas";
            }

            return Process.Start(psi);
        }

        /// <summary>
        /// Apre la cartella specificata in Esplora Risorse (Windows Explorer) selezionando opzionalmente il file.
        /// </summary>
        public static void OpenInExplorer(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) return;

            if (File.Exists(fullPath))
            {
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", fullPath));
            }
            else if (Directory.Exists(fullPath))
            {
                Process.Start("explorer.exe", string.Format("\"{0}\"", fullPath));
            }
        }
    }
}
