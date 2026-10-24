using System;

namespace BatManager.Models
{
    /// <summary>
    /// Rappresenta uno script .bat gestito dall'applicazione.
    /// Contiene i metadati estratti dall'intestazione e il contenuto dei comandi.
    /// </summary>
    public class BatScript
    {
        /// <summary>
        /// Nome del file su disco (es. "backup.bat").
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Percorso assoluto completo del file (es. "C:\...\scripts\backup.bat").
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Nome descrittivo dello script (estratto da REM @name: o ricavato dal nome file).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descrizione dell'utilità dello script (estratta da REM @description:).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Contenuto effettivo dei comandi batch, separato dall'intestazione dei metadati.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Indica se lo script deve essere eseguito sempre con privilegi elevati (UAC Amministratore).
        /// </summary>
        public bool RunAsAdmin { get; set; }

        /// <summary>
        /// Data e ora dell'ultima modifica del file.
        /// </summary>
        public DateTime LastModified { get; set; }

        public BatScript()
        {
            Name = string.Empty;
            Description = string.Empty;
            Content = string.Empty;
            FileName = string.Empty;
            FullPath = string.Empty;
            RunAsAdmin = false;
            LastModified = DateTime.Now;
        }

        public BatScript(string name, string description, string content, string fileName = null, string fullPath = null, bool runAsAdmin = false)
        {
            Name = name ?? string.Empty;
            Description = description ?? string.Empty;
            Content = content ?? string.Empty;
            FileName = fileName ?? string.Empty;
            FullPath = fullPath ?? string.Empty;
            RunAsAdmin = runAsAdmin;
            LastModified = DateTime.Now;
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? FileName : Name;
        }
    }
}
