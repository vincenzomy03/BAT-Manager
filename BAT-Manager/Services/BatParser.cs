using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BatManager.Models;

namespace BatManager.Services
{
    /// <summary>
    /// Parser per la lettura e scrittura dei metadati nei file .bat.
    /// Riconosce le annotazioni di testa:
    ///   REM @name: ...
    ///   REM @description: ...
    /// Separando i metadati dal corpo effettivo dello script.
    /// </summary>
    public static class BatParser
    {
        // Pattern regex per riconoscere REM @tag: valore oppure :: @tag: valore
        private static readonly Regex MetadataRegex = new Regex(
            @"^\s*(?:REM|::)\s*@(name|description|runasadmin|admin)\s*:\s*(.*)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Analizza il contenuto testuale grezzo di un file .bat ed estrae i metadati e il corpo dello script.
        /// </summary>
        public static BatScript Parse(string rawContent, string fileName, string fullPath = null)
        {
            if (rawContent == null)
            {
                rawContent = string.Empty;
            }

            string name = null;
            string description = null;
            bool runAsAdmin = false;

            var allLines = rawContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var bodyLines = new List<string>();

            bool inHeader = true;
            bool foundMetadata = false;

            for (int i = 0; i < allLines.Length; i++)
            {
                string line = allLines[i];

                if (inHeader)
                {
                    var match = MetadataRegex.Match(line);
                    if (match.Success)
                    {
                        foundMetadata = true;
                        string key = match.Groups[1].Value.ToLowerInvariant();
                        string value = match.Groups[2].Value.Trim();

                        if (key == "name" && string.IsNullOrEmpty(name))
                        {
                            name = value;
                        }
                        else if (key == "description" && string.IsNullOrEmpty(description))
                        {
                            description = value;
                        }
                        else if (key == "runasadmin" || key == "admin")
                        {
                            runAsAdmin = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(value, "1", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(value, "si", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(value, "sì", StringComparison.OrdinalIgnoreCase);
                        }
                        continue;
                    }

                    if (foundMetadata && string.IsNullOrWhiteSpace(line))
                    {
                        inHeader = false;
                        continue;
                    }

                    inHeader = false;
                }

                bodyLines.Add(line);
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = !string.IsNullOrWhiteSpace(fileName)
                    ? Path.GetFileNameWithoutExtension(fileName)
                    : "Untitled Script";
            }

            if (description == null)
            {
                description = string.Empty;
            }

            string content = string.Join(Environment.NewLine, bodyLines.ToArray());

            return new BatScript(name, description, content, fileName, fullPath, runAsAdmin);
        }

        /// <summary>
        /// Genera il testo completo del file .bat combinando l'intestazione con i metadati e il contenuto dei comandi.
        /// </summary>
        public static string BuildFileContent(string name, string description, string content, bool runAsAdmin = false)
        {
            var sb = new StringBuilder();

            string cleanName = (name ?? string.Empty).Trim();
            string cleanDesc = (description ?? string.Empty).Trim();

            sb.AppendLine(string.Format("REM @name: {0}", cleanName));
            sb.AppendLine(string.Format("REM @description: {0}", cleanDesc));
            if (runAsAdmin)
            {
                sb.AppendLine("REM @runAsAdmin: true");
            }
            sb.AppendLine();

            if (!string.IsNullOrEmpty(content))
            {
                sb.Append(content.TrimEnd());
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
