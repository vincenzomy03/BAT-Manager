using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BatManager.Models;
using BatManager.Services;

namespace BatManager.Forms
{
    public partial class MainForm : Form
    {
        private readonly ScriptRepository _repository;
        private List<BatScript> _allScripts = new List<BatScript>();
        private int _sortColumnIndex = 0; // 0 = Nome
        private bool _sortAscending = true;

        public MainForm()
        {
            InitializeComponent();
            LoadApplicationIcon();
            _repository = new ScriptRepository();
        }

        private void LoadApplicationIcon()
        {
            try
            {
                using (var stream = typeof(MainForm).Assembly.GetManifestResourceStream("BatManager.Resources.app.ico"))
                {
                    if (stream != null)
                    {
                        this.Icon = new Icon(stream);
                        return;
                    }
                }

                string icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "app.ico");
                if (File.Exists(icoPath))
                {
                    this.Icon = new Icon(icoPath);
                }
            }
            catch { }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadScripts();
        }

        /// <summary>
        /// Carica o ricarica l'elenco degli script dal disco e aggiorna la visualizzazione.
        /// </summary>
        private void LoadScripts(string scriptToSelect = null)
        {
            try
            {
                _allScripts = _repository.LoadAll();
                ApplyFilter(scriptToSelect);
                SetStatusMessage("Scripts loaded. Drag and drop .bat or .cmd files here to import them!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("An error occurred while loading scripts:\n\n{0}", ex.Message),
                    "Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                SetStatusMessage("Error while loading scripts.");
            }
        }

        /// <summary>
        /// Applica il filtro di ricerca testuale alla lista di script.
        /// </summary>
        private void ApplyFilter(string scriptToSelect = null)
        {
            string query = txtSearch.Text.Trim();
            IEnumerable<BatScript> filtered = _allScripts;

            if (!string.IsNullOrEmpty(query))
            {
                filtered = _allScripts.Where(s =>
                    (s.Name != null && s.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (s.Description != null && s.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (s.FileName != null && s.FileName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (s.Content != null && s.Content.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            // Ordinamento in base alla colonna cliccata
            switch (_sortColumnIndex)
            {
                case 0: // Nome
                    filtered = _sortAscending
                        ? filtered.OrderBy(s => s.Name, StringComparer.CurrentCultureIgnoreCase)
                        : filtered.OrderByDescending(s => s.Name, StringComparer.CurrentCultureIgnoreCase);
                    break;
                case 1: // Admin
                    filtered = _sortAscending
                        ? filtered.OrderBy(s => s.RunAsAdmin).ThenBy(s => s.Name, StringComparer.CurrentCultureIgnoreCase)
                        : filtered.OrderByDescending(s => s.RunAsAdmin).ThenBy(s => s.Name, StringComparer.CurrentCultureIgnoreCase);
                    break;
                case 2: // Descrizione
                    filtered = _sortAscending
                        ? filtered.OrderBy(s => s.Description, StringComparer.CurrentCultureIgnoreCase)
                        : filtered.OrderByDescending(s => s.Description, StringComparer.CurrentCultureIgnoreCase);
                    break;
                case 3: // File
                    filtered = _sortAscending
                        ? filtered.OrderBy(s => s.FileName, StringComparer.CurrentCultureIgnoreCase)
                        : filtered.OrderByDescending(s => s.FileName, StringComparer.CurrentCultureIgnoreCase);
                    break;
                case 4: // Ultima Modifica
                    filtered = _sortAscending
                        ? filtered.OrderBy(s => s.LastModified)
                        : filtered.OrderByDescending(s => s.LastModified);
                    break;
            }

            lvScripts.BeginUpdate();
            lvScripts.Items.Clear();

            ListViewItem itemToSelect = null;

            foreach (var s in filtered)
            {
                var item = new ListViewItem(s.Name);
                item.SubItems.Add(s.RunAsAdmin ? "🛡️ Yes" : "—");
                item.SubItems.Add(string.IsNullOrWhiteSpace(s.Description) ? "—" : s.Description);
                item.SubItems.Add(s.FileName);
                item.SubItems.Add(s.LastModified.ToString("g"));
                item.Tag = s;

                lvScripts.Items.Add(item);

                if (scriptToSelect != null && string.Equals(s.FileName, scriptToSelect, StringComparison.OrdinalIgnoreCase))
                {
                    itemToSelect = item;
                }
            }

            lvScripts.EndUpdate();

            if (itemToSelect != null)
            {
                itemToSelect.Selected = true;
                itemToSelect.EnsureVisible();
            }

            lblStatusCount.Text = string.Format("{0} of {1} scripts displayed", lvScripts.Items.Count, _allScripts.Count);
            UpdateToolbarButtonsState();
        }

        private BatScript GetSelectedScript()
        {
            if (lvScripts.SelectedItems.Count > 0)
            {
                return lvScripts.SelectedItems[0].Tag as BatScript;
            }
            return null;
        }

        private void UpdateToolbarButtonsState()
        {
            bool hasSelection = lvScripts.SelectedItems.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnRun.Enabled = hasSelection;
            btnRunAdmin.Enabled = hasSelection;
        }

        private void SetStatusMessage(string msg)
        {
            lblStatusMessage.Text = msg;
        }

        #region Event Handlers

        private void BtnNew_Click(object sender, EventArgs e)
        {
            using (var editor = new EditorForm(null, _repository))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    string selectedName = editor.SavedScript != null ? editor.SavedScript.FileName : null;
                    LoadScripts(selectedName);
                    string displayName = editor.SavedScript != null ? editor.SavedScript.Name : "";
                    SetStatusMessage(string.Format("New script '{0}' created.", displayName));
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedScript();
            if (selected == null) return;

            using (var editor = new EditorForm(selected, _repository))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    string selectedName = editor.SavedScript != null ? editor.SavedScript.FileName : null;
                    LoadScripts(selectedName);
                    string displayName = editor.SavedScript != null ? editor.SavedScript.Name : "";
                    SetStatusMessage(string.Format("Script '{0}' saved.", displayName));
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedScript();
            if (selected == null) return;

            var confirm = MessageBox.Show(
                string.Format("Are you sure you want to delete the script:\n\n\"{0}\" ({1})?\n\nThis action cannot be undone.", selected.Name, selected.FileName),
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _repository.Delete(selected);
                    LoadScripts();
                    SetStatusMessage(string.Format("Script '{0}' deleted successfully.", selected.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format("Error while deleting file:\n\n{0}", ex.Message),
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedScript();
            if (selected == null) return;

            try
            {
                bool asAdmin = selected.RunAsAdmin;
                ScriptRunner.Run(selected, asAdmin, true);

                if (asAdmin)
                {
                    SetStatusMessage(string.Format("Execution started as Administrator (configured in script): {0}", selected.FileName));
                }
                else
                {
                    SetStatusMessage(string.Format("Execution started: {0}", selected.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Cannot execute script:\n\n{0}", ex.Message),
                    "Execution Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnRunAdmin_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedScript();
            if (selected == null) return;

            try
            {
                ScriptRunner.Run(selected, true, true);
                SetStatusMessage(string.Format("Execution started as Administrator: {0}", selected.FileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Cannot execute with elevated privileges:\n\n{0}", ex.Message),
                    "Administrator Execution Cancelled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedScript();
            LoadScripts(selected != null ? selected.FileName : null);
        }

        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptRunner.OpenInExplorer(_repository.ScriptsDirectory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Cannot open scripts folder:\n\n{0}", ex.Message),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
        }

        private void LvScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateToolbarButtonsState();
        }

        private void LvScripts_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        private void LvScripts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (_sortColumnIndex == e.Column)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortColumnIndex = e.Column;
                // Per la colonna Admin (1) o Ultima Modifica (4), al primo clic mostra prima quelli con permessi o i più recenti
                if (e.Column == 1 || e.Column == 4)
                {
                    _sortAscending = false;
                }
                else
                {
                    _sortAscending = true;
                }
            }

            UpdateColumnHeaderSortIndicators();
            var selected = GetSelectedScript();
            ApplyFilter(selected != null ? selected.FileName : null);
        }

        private void UpdateColumnHeaderSortIndicators()
        {
            string arrow = _sortAscending ? " ▲" : " ▼";
            colName.Text = "Script Name" + (_sortColumnIndex == 0 ? arrow : "");
            colAdmin.Text = "Admin" + (_sortColumnIndex == 1 ? arrow : "");
            colDescription.Text = "Description" + (_sortColumnIndex == 2 ? arrow : "");
            colFileName.Text = "File" + (_sortColumnIndex == 3 ? arrow : "");
            colModified.Text = "Last Modified" + (_sortColumnIndex == 4 ? arrow : "");
        }

        private void LvScripts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                BtnRun_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                BtnDelete_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                BtnRefresh_Click(sender, e);
            }
        }

        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasSelection = lvScripts.SelectedItems.Count > 0;
            ctxRun.Enabled = hasSelection;
            ctxRunAdmin.Enabled = hasSelection;
            ctxEdit.Enabled = hasSelection;
            ctxDelete.Enabled = hasSelection;
            ctxCopyPath.Enabled = hasSelection;
            ctxShowInExplorer.Enabled = hasSelection;
        }

        private void CtxCopyPath_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedScript();
            if (selected != null && !string.IsNullOrWhiteSpace(selected.FullPath))
            {
                Clipboard.SetText(selected.FullPath);
                SetStatusMessage("Path copied to clipboard.");
            }
        }

        private void CtxShowInExplorer_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedScript();
            if (selected != null)
            {
                ScriptRunner.OpenInExplorer(selected.FullPath);
            }
        }

        #endregion

        #region Drag and Drop Import

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Any(f => IsValidBatchFile(f)))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0) return;

            foreach (string file in files)
            {
                if (IsValidBatchFile(file))
                {
                    ImportScriptFromFile(file);
                }
            }
        }

        private void ImportScriptFromFile(string filePath)
        {
            try
            {
                string rawContent = _repository.ReadFileWithFallback(filePath);
                string fileName = Path.GetFileName(filePath);
                var imported = BatParser.Parse(rawContent, fileName, filePath);

                using (var editor = EditorForm.CreateForImport(imported, _repository))
                {
                    if (editor.ShowDialog(this) == DialogResult.OK)
                    {
                        string savedFile = editor.SavedScript != null ? editor.SavedScript.FileName : null;
                        LoadScripts(savedFile);
                        string displayName = editor.SavedScript != null ? editor.SavedScript.Name : "";
                        SetStatusMessage(string.Format("Script '{0}' imported and saved successfully.", displayName));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Cannot import script from '{0}':\n\n{1}", filePath, ex.Message),
                    "Import Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static bool IsValidBatchFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return false;
            string ext = Path.GetExtension(path);
            return string.Equals(ext, ".bat", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".cmd", StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
