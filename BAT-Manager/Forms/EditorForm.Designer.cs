namespace BatManager.Forms
{
    partial class EditorForm
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.chkRunAsAdmin = new System.Windows.Forms.CheckBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnTestRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblContent = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.chkRunAsAdmin);
            this.pnlTop.Controls.Add(this.lblFileName);
            this.pnlTop.Controls.Add(this.txtFileName);
            this.pnlTop.Controls.Add(this.lblDescription);
            this.pnlTop.Controls.Add(this.txtDescription);
            this.pnlTop.Controls.Add(this.lblName);
            this.pnlTop.Controls.Add(this.txtName);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(12, 12);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(760, 195);
            this.pnlTop.TabIndex = 0;
            // 
            // chkRunAsAdmin
            // 
            this.chkRunAsAdmin.AutoSize = true;
            this.chkRunAsAdmin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRunAsAdmin.Location = new System.Drawing.Point(0, 160);
            this.chkRunAsAdmin.Name = "chkRunAsAdmin";
            this.chkRunAsAdmin.Size = new System.Drawing.Size(435, 24);
            this.chkRunAsAdmin.TabIndex = 6;
            this.chkRunAsAdmin.Text = "🛡️ Always run as Administrator (require UAC elevation)";
            this.chkRunAsAdmin.UseVisualStyleBackColor = true;
            // 
            // lblFileName
            // 
            this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFileName.Location = new System.Drawing.Point(510, 4);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(122, 20);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "File Name (.bat):";
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtFileName.Location = new System.Drawing.Point(510, 27);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(250, 29);
            this.txtFileName.TabIndex = 3;
            this.txtFileName.TextChanged += new System.EventHandler(this.TxtFileName_TextChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(0, 68);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(209, 20);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description (@description):";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDescription.Location = new System.Drawing.Point(0, 91);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(760, 56);
            this.txtDescription.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(0, 4);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(158, 20);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Script Name (@name):";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtName.Location = new System.Drawing.Point(0, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(490, 29);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.TxtName_TextChanged);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnTestRun);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(12, 510);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlBottom.Size = new System.Drawing.Size(760, 50);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnTestRun
            // 
            this.btnTestRun.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTestRun.Location = new System.Drawing.Point(0, 10);
            this.btnTestRun.Name = "btnTestRun";
            this.btnTestRun.Size = new System.Drawing.Size(140, 36);
            this.btnTestRun.TabIndex = 0;
            this.btnTestRun.Text = "▶️ Test Script";
            this.btnTestRun.UseVisualStyleBackColor = true;
            this.btnTestRun.Click += new System.EventHandler(this.BtnTestRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.Location = new System.Drawing.Point(645, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 36);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSave.Location = new System.Drawing.Point(515, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 36);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "💾 Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblContent.Location = new System.Drawing.Point(12, 215);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(515, 20);
            this.lblContent.TabIndex = 1;
            this.lblContent.Text = "Batch Commands (REM @... metadata is managed automatically):";
            // 
            // txtContent
            // 
            this.txtContent.AcceptsTab = true;
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtContent.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.txtContent.Location = new System.Drawing.Point(12, 240);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(760, 262);
            this.txtContent.TabIndex = 2;
            this.txtContent.WordWrap = false;
            // 
            // EditorForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 572);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "EditorForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Script Editor";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.CheckBox chkRunAsAdmin;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnTestRun;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox txtContent;
    }
}
