namespace OSLCConsumer
{
    partial class ShowRequirementsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.Artifact_foldersListbox = new System.Windows.Forms.ListBox();
            this.informLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Requirements_listView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Artifact_foldersListbox);
            this.panel1.Location = new System.Drawing.Point(12, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 171);
            this.panel1.TabIndex = 0;
            // 
            // Artifact_foldersListbox
            // 
            this.Artifact_foldersListbox.FormattingEnabled = true;
            this.Artifact_foldersListbox.ItemHeight = 16;
            this.Artifact_foldersListbox.Location = new System.Drawing.Point(3, 3);
            this.Artifact_foldersListbox.Name = "Artifact_foldersListbox";
            this.Artifact_foldersListbox.Size = new System.Drawing.Size(192, 164);
            this.Artifact_foldersListbox.TabIndex = 0;
            this.Artifact_foldersListbox.Click += new System.EventHandler(this.Artifact_foldersListbox_Click);
            // 
            // informLabel
            // 
            this.informLabel.AutoSize = true;
            this.informLabel.Location = new System.Drawing.Point(271, 13);
            this.informLabel.Name = "informLabel";
            this.informLabel.Size = new System.Drawing.Size(0, 17);
            this.informLabel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(103, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Artifact Folders";
            // 
            // Requirements_listView
            // 
            this.Requirements_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.Requirements_listView.FullRowSelect = true;
            this.Requirements_listView.GridLines = true;
            this.Requirements_listView.HideSelection = false;
            this.Requirements_listView.Location = new System.Drawing.Point(274, 56);
            this.Requirements_listView.Name = "Requirements_listView";
            this.Requirements_listView.Size = new System.Drawing.Size(946, 276);
            this.Requirements_listView.TabIndex = 3;
            this.Requirements_listView.UseCompatibleStateImageBehavior = false;
            this.Requirements_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Title";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Created";
            this.columnHeader3.Width = 178;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Modified";
            this.columnHeader4.Width = 153;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Creator";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            // 
            // ShowRequirementsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 473);
            this.Controls.Add(this.Requirements_listView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.informLabel);
            this.Controls.Add(this.panel1);
            this.Name = "ShowRequirementsForm";
            this.Text = "OSLC Consumer - Artifacts";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShowRequirementsForm_FormClosed);
            this.Load += new System.EventHandler(this.ShowRequirementsForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox Artifact_foldersListbox;
        private System.Windows.Forms.Label informLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView Requirements_listView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}