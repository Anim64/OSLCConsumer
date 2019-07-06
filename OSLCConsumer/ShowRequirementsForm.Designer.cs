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
            this.testlabel2 = new System.Windows.Forms.Label();
            this.RequirementDetail_panel = new System.Windows.Forms.Panel();
            this.Title_label = new System.Windows.Forms.Label();
            this.InfoLabel_description = new System.Windows.Forms.Label();
            this.Description_textbox = new System.Windows.Forms.TextBox();
            this.InfoLabel_ID = new System.Windows.Forms.Label();
            this.InfoCreated_label = new System.Windows.Forms.Label();
            this.InfoLabel_modified = new System.Windows.Forms.Label();
            this.InfoLabel_creator = new System.Windows.Forms.Label();
            this.ID_label = new System.Windows.Forms.Label();
            this.Modified_label = new System.Windows.Forms.Label();
            this.Created_label = new System.Windows.Forms.Label();
            this.Creator_label = new System.Windows.Forms.Label();
            this.Reload_artifacts_button = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.RequirementDetail_panel.SuspendLayout();
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
            this.Requirements_listView.Size = new System.Drawing.Size(946, 230);
            this.Requirements_listView.TabIndex = 3;
            this.Requirements_listView.UseCompatibleStateImageBehavior = false;
            this.Requirements_listView.View = System.Windows.Forms.View.Details;
            this.Requirements_listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.Requirements_listView_ColumnClick);
            this.Requirements_listView.Click += new System.EventHandler(this.Requirements_listView_Click);
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
            // testlabel2
            // 
            this.testlabel2.AutoSize = true;
            this.testlabel2.Location = new System.Drawing.Point(16, 238);
            this.testlabel2.Name = "testlabel2";
            this.testlabel2.Size = new System.Drawing.Size(0, 17);
            this.testlabel2.TabIndex = 4;
            // 
            // RequirementDetail_panel
            // 
            this.RequirementDetail_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RequirementDetail_panel.Controls.Add(this.Creator_label);
            this.RequirementDetail_panel.Controls.Add(this.Created_label);
            this.RequirementDetail_panel.Controls.Add(this.Modified_label);
            this.RequirementDetail_panel.Controls.Add(this.ID_label);
            this.RequirementDetail_panel.Controls.Add(this.InfoLabel_creator);
            this.RequirementDetail_panel.Controls.Add(this.InfoLabel_modified);
            this.RequirementDetail_panel.Controls.Add(this.InfoCreated_label);
            this.RequirementDetail_panel.Controls.Add(this.InfoLabel_ID);
            this.RequirementDetail_panel.Controls.Add(this.Description_textbox);
            this.RequirementDetail_panel.Controls.Add(this.InfoLabel_description);
            this.RequirementDetail_panel.Controls.Add(this.Title_label);
            this.RequirementDetail_panel.Location = new System.Drawing.Point(71, 311);
            this.RequirementDetail_panel.Name = "RequirementDetail_panel";
            this.RequirementDetail_panel.Size = new System.Drawing.Size(1109, 380);
            this.RequirementDetail_panel.TabIndex = 5;
            // 
            // Title_label
            // 
            this.Title_label.AutoSize = true;
            this.Title_label.Location = new System.Drawing.Point(475, 17);
            this.Title_label.Name = "Title_label";
            this.Title_label.Size = new System.Drawing.Size(45, 17);
            this.Title_label.TabIndex = 0;
            this.Title_label.Text = "Name";
            // 
            // InfoLabel_description
            // 
            this.InfoLabel_description.AutoSize = true;
            this.InfoLabel_description.Location = new System.Drawing.Point(3, 276);
            this.InfoLabel_description.Name = "InfoLabel_description";
            this.InfoLabel_description.Size = new System.Drawing.Size(87, 17);
            this.InfoLabel_description.TabIndex = 1;
            this.InfoLabel_description.Text = "Description :";
            // 
            // Description_textbox
            // 
            this.Description_textbox.Location = new System.Drawing.Point(97, 276);
            this.Description_textbox.Multiline = true;
            this.Description_textbox.Name = "Description_textbox";
            this.Description_textbox.ReadOnly = true;
            this.Description_textbox.Size = new System.Drawing.Size(181, 84);
            this.Description_textbox.TabIndex = 2;
            // 
            // InfoLabel_ID
            // 
            this.InfoLabel_ID.AutoSize = true;
            this.InfoLabel_ID.Location = new System.Drawing.Point(3, 70);
            this.InfoLabel_ID.Name = "InfoLabel_ID";
            this.InfoLabel_ID.Size = new System.Drawing.Size(27, 17);
            this.InfoLabel_ID.TabIndex = 3;
            this.InfoLabel_ID.Text = "Id :";
            // 
            // InfoCreated_label
            // 
            this.InfoCreated_label.AutoSize = true;
            this.InfoCreated_label.Location = new System.Drawing.Point(3, 100);
            this.InfoCreated_label.Name = "InfoCreated_label";
            this.InfoCreated_label.Size = new System.Drawing.Size(70, 17);
            this.InfoCreated_label.TabIndex = 4;
            this.InfoCreated_label.Text = "Created : ";
            // 
            // InfoLabel_modified
            // 
            this.InfoLabel_modified.AutoSize = true;
            this.InfoLabel_modified.Location = new System.Drawing.Point(3, 130);
            this.InfoLabel_modified.Name = "InfoLabel_modified";
            this.InfoLabel_modified.Size = new System.Drawing.Size(104, 17);
            this.InfoLabel_modified.TabIndex = 5;
            this.InfoLabel_modified.Text = "Last modified : ";
            // 
            // InfoLabel_creator
            // 
            this.InfoLabel_creator.AutoSize = true;
            this.InfoLabel_creator.Location = new System.Drawing.Point(3, 160);
            this.InfoLabel_creator.Name = "InfoLabel_creator";
            this.InfoLabel_creator.Size = new System.Drawing.Size(63, 17);
            this.InfoLabel_creator.TabIndex = 6;
            this.InfoLabel_creator.Text = "Creator :";
            // 
            // ID_label
            // 
            this.ID_label.AutoSize = true;
            this.ID_label.Location = new System.Drawing.Point(113, 70);
            this.ID_label.Name = "ID_label";
            this.ID_label.Size = new System.Drawing.Size(46, 17);
            this.ID_label.TabIndex = 7;
            this.ID_label.Text = "label2";
            // 
            // Modified_label
            // 
            this.Modified_label.AutoSize = true;
            this.Modified_label.Location = new System.Drawing.Point(113, 130);
            this.Modified_label.Name = "Modified_label";
            this.Modified_label.Size = new System.Drawing.Size(46, 17);
            this.Modified_label.TabIndex = 8;
            this.Modified_label.Text = "label3";
            // 
            // Created_label
            // 
            this.Created_label.AutoSize = true;
            this.Created_label.Location = new System.Drawing.Point(113, 100);
            this.Created_label.Name = "Created_label";
            this.Created_label.Size = new System.Drawing.Size(46, 17);
            this.Created_label.TabIndex = 9;
            this.Created_label.Text = "label4";
            // 
            // Creator_label
            // 
            this.Creator_label.AutoSize = true;
            this.Creator_label.Location = new System.Drawing.Point(113, 160);
            this.Creator_label.Name = "Creator_label";
            this.Creator_label.Size = new System.Drawing.Size(46, 17);
            this.Creator_label.TabIndex = 10;
            this.Creator_label.Text = "label5";
            // 
            // Reload_artifacts_button
            // 
            this.Reload_artifacts_button.Location = new System.Drawing.Point(35, 238);
            this.Reload_artifacts_button.Name = "Reload_artifacts_button";
            this.Reload_artifacts_button.Size = new System.Drawing.Size(160, 23);
            this.Reload_artifacts_button.TabIndex = 6;
            this.Reload_artifacts_button.Text = "Reload Artifacts";
            this.Reload_artifacts_button.UseVisualStyleBackColor = true;
            this.Reload_artifacts_button.Click += new System.EventHandler(this.Reload_artifacts_button_Click);
            // 
            // ShowRequirementsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 703);
            this.Controls.Add(this.Reload_artifacts_button);
            this.Controls.Add(this.RequirementDetail_panel);
            this.Controls.Add(this.testlabel2);
            this.Controls.Add(this.Requirements_listView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.informLabel);
            this.Controls.Add(this.panel1);
            this.Name = "ShowRequirementsForm";
            this.Text = "OSLC Consumer - Artifacts";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShowRequirementsForm_FormClosed);
            this.Load += new System.EventHandler(this.ShowRequirementsForm_Load);
            this.panel1.ResumeLayout(false);
            this.RequirementDetail_panel.ResumeLayout(false);
            this.RequirementDetail_panel.PerformLayout();
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
        private System.Windows.Forms.Label testlabel2;
        private System.Windows.Forms.Panel RequirementDetail_panel;
        private System.Windows.Forms.TextBox Description_textbox;
        private System.Windows.Forms.Label InfoLabel_description;
        private System.Windows.Forms.Label Title_label;
        private System.Windows.Forms.Label Creator_label;
        private System.Windows.Forms.Label Created_label;
        private System.Windows.Forms.Label Modified_label;
        private System.Windows.Forms.Label ID_label;
        private System.Windows.Forms.Label InfoLabel_creator;
        private System.Windows.Forms.Label InfoLabel_modified;
        private System.Windows.Forms.Label InfoCreated_label;
        private System.Windows.Forms.Label InfoLabel_ID;
        private System.Windows.Forms.Button Reload_artifacts_button;
    }
}