using System;
using System.IO;
using System.Windows.Forms;
using BatManager.Models;
using BatManager.Services;

namespace BatManager.Forms
{
    public partial class EditorForm : Form
    {
        private readonly ScriptRepository _repository;
        private readonly BatScript _originalScript;
        private bool _isUserEditingFileName;

        /// <summary>
        /// Lo script salvato al termine del form (se il risultato è OK).
        /// </summary>
        public BatScript SavedScript { get; private set; }

        public EditorForm(BatScript scriptToEdit, ScriptRepository repository)
        {
            InitializeComponent();
            LoadApplicationIcon();
            _repository = repository ?? new ScriptRepository();
            _originalScript = scriptToEdit;

            if (_originalScript != null)
            {
                Text = string.Format("Edit Script - {0}", _originalScript.Name);
                txtName.Text = _originalScript.Name;
                txtFileName.Text = _originalScript.FileName;
                txtDescription.Text = _originalScript.Description;
                txtContent.Text = _originalScript.Content;
                chkRunAsAdmin.Checked = _originalScript.RunAsAdmin;

                _isUserEditingFileName = true;
            }
            else
            {
                Text = "New Batch Script";
                txtFileName.Text = "new-script.bat";
                txtContent.Text = "@echo off\r\necho Executing script...\r\npause";
                chkRunAsAdmin.Checked = false;
                _isUserEditingFileName = false;
            }
        }

        private void LoadApplicationIcon()
        {
            try
            {
                using (var stream = typeof(EditorForm).Assembly.GetManifestResourceStream("BatManager.Resources.app.ico"))
                {
                    if (stream != null)
                    {
                        this.Icon = new System.Drawing.Icon(stream);
                        return;
                    }
                }

                string icoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "app.ico");
                if (System.IO.File.Exists(icoPath))
                {
                    this.Icon = new System.Drawing.Icon(icoPath);
                }
            }
            catch { }
        }

        /// <summary>
        /// Crea il form configurato per l'importazione di un file trascinato o aperto dall'esterno.
        /// Permette di revisionare e modificare nome, descrizione, nome file e permessi admin prima del salvataggio.
        /// </summary>
        public static EditorForm CreateForImport(BatScript importedScript, ScriptRepository repository)
        {
            if (importedScript == null) throw new ArgumentNullException("importedScript");
            if (repository == null) repository = new ScriptRepository();

            var form = new EditorForm(null, repository);
            form.Text = string.Format("Import Script - {0}", importedScript.Name);
            form.txtName.Text = importedScript.Name;
            form.txtDescription.Text = importedScript.Description;

            // Se il nome file originale è valido e non confligge usalo, altrimenti trova il primo nome libero
            string suggestedName = repository.GetAvailableFileName(importedScript.FileName);
            form.txtFileName.Text = suggestedName;

            form.txtContent.Text = importedScript.Content;
            form.chkRunAsAdmin.Checked = importedScript.RunAsAdmin;
            form._isUserEditingFileName = true;

            return form;
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserEditingFileName && _originalScript == null)
            {
                string slug = ScriptRepository.GenerateSlugFileName(txtName.Text);
                txtFileName.Text = slug;
            }
        }

        private void TxtFileName_TextChanged(object sender, EventArgs e)
        {
            if (txtFileName.Focused)
            {
                _isUserEditingFileName = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string fileName = txtFileName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string content = txtContent.Text;
            bool runAsAdmin = chkRunAsAdmin.Checked;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "Please enter a descriptive name for the script before saving.",
                    "Name Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = ScriptRepository.GenerateSlugFileName(name);
                txtFileName.Text = fileName;
            }

            string currentOriginalName = _originalScript != null ? _originalScript.FileName : null;
            var validation = _repository.ValidateFileName(fileName, currentOriginalName);
            if (!validation.IsValid)
            {
                MessageBox.Show(
                    validation.ErrorMessage,
                    "Invalid File Name",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtFileName.Focus();
                return;
            }

            try
            {
                var targetScript = _originalScript ?? new BatScript();
                targetScript.Name = name;
                targetScript.Description = description;
                targetScript.FileName = fileName;
                targetScript.Content = content;
                targetScript.RunAsAdmin = runAsAdmin;

                _repository.Save(targetScript, currentOriginalName);

                SavedScript = targetScript;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("An error occurred while saving the script:\n\n{0}", ex.Message),
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnTestRun_Click(object sender, EventArgs e)
        {
            try
            {
                string tempFile = Path.Combine(Path.GetTempPath(), string.Format("bat_preview_{0:N}.bat", Guid.NewGuid()));
                string fullContent = BatParser.BuildFileContent(txtName.Text, txtDescription.Text, txtContent.Text, chkRunAsAdmin.Checked);
                File.WriteAllText(tempFile, fullContent, _repository.DefaultEncoding);

                ScriptRunner.RunFile(tempFile, chkRunAsAdmin.Checked, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Cannot run preview test:\n\n{0}", ex.Message),
                    "Execution Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
