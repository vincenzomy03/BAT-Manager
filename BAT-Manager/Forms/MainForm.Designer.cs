namespace BatManager.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.btnRunAdmin = new System.Windows.Forms.ToolStripButton();
            this.sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnOpenFolder = new System.Windows.Forms.ToolStripButton();
            this.sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.btnClearSearch = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvScripts = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAdmin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxRun = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxRunAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxShowInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnEdit,
            this.btnDelete,
            this.sep1,
            this.btnRun,
            this.btnRunAdmin,
            this.sep2,
            this.btnRefresh,
            this.btnOpenFolder,
            this.sep3,
            this.lblSearch,
            this.txtSearch,
            this.btnClearSearch});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(940, 35);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "Toolbar";
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(69, 24);
            this.btnNew.Text = "➕ New";
            this.btnNew.ToolTipText = "Create a new .bat script";
            this.btnNew.Click += new System.EventHandler(this.BtnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(74, 24);
            this.btnEdit.Text = "✏️ Edit";
            this.btnEdit.ToolTipText = "Edit selected script";
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 24);
            this.btnDelete.Text = "🗑️ Delete";
            this.btnDelete.ToolTipText = "Delete selected script";
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnRun
            // 
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(73, 24);
            this.btnRun.Text = "▶️ Run";
            this.btnRun.ToolTipText = "Run selected script in cmd.exe";
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // btnRunAdmin
            // 
            this.btnRunAdmin.Enabled = false;
            this.btnRunAdmin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRunAdmin.Name = "btnRunAdmin";
            this.btnRunAdmin.Size = new System.Drawing.Size(126, 24);
            this.btnRunAdmin.Text = "🛡️ Run as Admin";
            this.btnRunAdmin.ToolTipText = "Run script with Administrator privileges (UAC)";
            this.btnRunAdmin.Click += new System.EventHandler(this.BtnRunAdmin_Click);
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(6, 27);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(76, 24);
            this.btnRefresh.Text = "🔄 Refresh";
            this.btnRefresh.ToolTipText = "Reload scripts list (F5)";
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(95, 24);
            this.btnOpenFolder.Text = "📁 Open Folder";
            this.btnOpenFolder.ToolTipText = "Open scripts folder in File Explorer";
            this.btnOpenFolder.Click += new System.EventHandler(this.BtnOpenFolder_Click);
            // 
            // sep3
            // 
            this.sep3.Name = "sep3";
            this.sep3.Size = new System.Drawing.Size(6, 27);
            // 
            // lblSearch
            // 
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 24);
            this.lblSearch.Text = "🔍 Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(160, 27);
            this.txtSearch.ToolTipText = "Type to filter scripts by name, description, file or content...";
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClearSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(23, 24);
            this.btnClearSearch.Text = "✕";
            this.btnClearSearch.ToolTipText = "Clear search filter";
            this.btnClearSearch.Click += new System.EventHandler(this.BtnClearSearch_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusCount,
            this.lblStatusMessage});
            this.statusStrip.Location = new System.Drawing.Point(0, 535);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 14, 0);
            this.statusStrip.Size = new System.Drawing.Size(940, 26);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "Status";
            // 
            // lblStatusCount
            // 
            this.lblStatusCount.Name = "lblStatusCount";
            this.lblStatusCount.Size = new System.Drawing.Size(84, 20);
            this.lblStatusCount.Text = "0 scripts loaded";
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusMessage.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblStatusMessage.Size = new System.Drawing.Size(841, 20);
            this.lblStatusMessage.Spring = true;
            this.lblStatusMessage.Text = "Ready";
            this.lblStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvScripts
            // 
            this.lvScripts.AllowDrop = true;
            this.lvScripts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colAdmin,
            this.colDescription,
            this.colFileName,
            this.colModified});
            this.lvScripts.ContextMenuStrip = this.contextMenu;
            this.lvScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvScripts.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvScripts.FullRowSelect = true;
            this.lvScripts.GridLines = true;
            this.lvScripts.HideSelection = false;
            this.lvScripts.Location = new System.Drawing.Point(0, 35);
            this.lvScripts.MultiSelect = false;
            this.lvScripts.Name = "lvScripts";
            this.lvScripts.ShowItemToolTips = true;
            this.lvScripts.Size = new System.Drawing.Size(940, 500);
            this.lvScripts.TabIndex = 2;
            this.lvScripts.UseCompatibleStateImageBehavior = false;
            this.lvScripts.View = System.Windows.Forms.View.Details;
            this.lvScripts.SelectedIndexChanged += new System.EventHandler(this.LvScripts_SelectedIndexChanged);
            this.lvScripts.DoubleClick += new System.EventHandler(this.LvScripts_DoubleClick);
            this.lvScripts.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LvScripts_ColumnClick);
            this.lvScripts.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.lvScripts.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.lvScripts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvScripts_KeyDown);
            // 
            // colName
            // 
            this.colName.Text = "Script Name ▲";
            this.colName.Width = 240;
            // 
            // colAdmin
            // 
            this.colAdmin.Text = "Admin";
            this.colAdmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colAdmin.Width = 85;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 475;
            // 
            // colFileName
            // 
            this.colFileName.Text = "File";
            this.colFileName.Width = 170;
            // 
            // colModified
            // 
            this.colModified.Text = "Last Modified";
            this.colModified.Width = 150;
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxRun,
            this.ctxRunAdmin,
            this.ctxSep1,
            this.ctxEdit,
            this.ctxDelete,
            this.ctxSep2,
            this.ctxCopyPath,
            this.ctxShowInExplorer});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(222, 160);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // ctxRun
            // 
            this.ctxRun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctxRun.Name = "ctxRun";
            this.ctxRun.Size = new System.Drawing.Size(221, 24);
            this.ctxRun.Text = "▶️ Run";
            this.ctxRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // ctxRunAdmin
            // 
            this.ctxRunAdmin.Name = "ctxRunAdmin";
            this.ctxRunAdmin.Size = new System.Drawing.Size(221, 24);
            this.ctxRunAdmin.Text = "🛡️ Run as Administrator";
            this.ctxRunAdmin.Click += new System.EventHandler(this.BtnRunAdmin_Click);
            // 
            // ctxSep1
            // 
            this.ctxSep1.Name = "ctxSep1";
            this.ctxSep1.Size = new System.Drawing.Size(218, 6);
            // 
            // ctxEdit
            // 
            this.ctxEdit.Name = "ctxEdit";
            this.ctxEdit.Size = new System.Drawing.Size(221, 24);
            this.ctxEdit.Text = "✏️ Edit";
            this.ctxEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // ctxDelete
            // 
            this.ctxDelete.Name = "ctxDelete";
            this.ctxDelete.Size = new System.Drawing.Size(221, 24);
            this.ctxDelete.Text = "🗑️ Delete";
            this.ctxDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // ctxSep2
            // 
            this.ctxSep2.Name = "ctxSep2";
            this.ctxSep2.Size = new System.Drawing.Size(218, 6);
            // 
            // ctxCopyPath
            // 
            this.ctxCopyPath.Name = "ctxCopyPath";
            this.ctxCopyPath.Size = new System.Drawing.Size(221, 24);
            this.ctxCopyPath.Text = "📋 Copy Full Path";
            this.ctxCopyPath.Click += new System.EventHandler(this.CtxCopyPath_Click);
            // 
            // ctxShowInExplorer
            // 
            this.ctxShowInExplorer.Name = "ctxShowInExplorer";
            this.ctxShowInExplorer.Size = new System.Drawing.Size(221, 24);
            this.ctxShowInExplorer.Text = "📁 Show in File Explorer";
            this.ctxShowInExplorer.Click += new System.EventHandler(this.CtxShowInExplorer_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 580);
            this.Controls.Add(this.lvScripts);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BAT-Manager - Batch Script Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.Windows.Forms.ToolStripButton btnRunAdmin;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnOpenFolder;
        private System.Windows.Forms.ToolStripSeparator sep3;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.ToolStripTextBox txtSearch;
        private System.Windows.Forms.ToolStripButton btnClearSearch;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCount;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusMessage;
        private System.Windows.Forms.ListView lvScripts;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colAdmin;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ColumnHeader colFileName;
        private System.Windows.Forms.ColumnHeader colModified;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem ctxRun;
        private System.Windows.Forms.ToolStripMenuItem ctxRunAdmin;
        private System.Windows.Forms.ToolStripSeparator ctxSep1;
        private System.Windows.Forms.ToolStripMenuItem ctxEdit;
        private System.Windows.Forms.ToolStripMenuItem ctxDelete;
        private System.Windows.Forms.ToolStripSeparator ctxSep2;
        private System.Windows.Forms.ToolStripMenuItem ctxCopyPath;
        private System.Windows.Forms.ToolStripMenuItem ctxShowInExplorer;
    }
}
