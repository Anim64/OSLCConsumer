using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JazzOSLCReqManager;
namespace OSLCConsumer
{
    public partial class ShowRequirementsForm : Form
    {
        OSLCManager oslcManager;
        private string CATALOG_URI = null;
        private string SERVICE_PROVIDER_URI = null;
        private Dictionary<string, List<Dictionary<string, string>>> reqFolders;
        Form caller;
        private string folderName = null;
        
        public ShowRequirementsForm()
        {
            InitializeComponent();
        }
        public ShowRequirementsForm(OSLCManager oslcManager,string CATALOG_URI,string SERVICE_PROVIDER_URI,Form caller)
        {
            InitializeComponent();
            this.oslcManager = oslcManager;
            this.CATALOG_URI = CATALOG_URI;
            this.SERVICE_PROVIDER_URI = SERVICE_PROVIDER_URI;
            reqFolders = null;
            this.caller = caller;
            this.RequirementDetail_panel.Visible = false;
            this.Reload_artifacts_button.Enabled = false;

        }

        private void ShowRequirementsForm_Load(object sender, EventArgs e)
        {
            Dictionary<string,string> Folders = oslcManager.getFoldersContainingArtifacts(this.SERVICE_PROVIDER_URI);
            this.reqFolders = new Dictionary<string, List<Dictionary<string, string>>>();
            foreach (var folder in Folders)
            {
                this.reqFolders.Add(folder.Key, null);
            }
            this.Artifact_foldersListbox.DataSource = new BindingSource(Folders, null);
            this.Artifact_foldersListbox.DisplayMember = "Key";
            this.Artifact_foldersListbox.ValueMember = "Value";

        }

        private void Artifact_foldersListbox_Click(object sender, EventArgs e)
        {
            
            this.Requirements_listView.Items.Clear();
            this.RequirementDetail_panel.Visible = false;
            int beg = this.Artifact_foldersListbox.SelectedItem.ToString().IndexOf(",")+1;
            int end = this.Artifact_foldersListbox.SelectedItem.ToString().Length - 1;
            this.folderName = this.Artifact_foldersListbox.SelectedItem.ToString().Substring(1, beg - 2);
            string FolderURI = this.Artifact_foldersListbox.SelectedItem.ToString()
                .Substring(beg, end - beg).Trim();
            informLabel.Text = "Loading Artifacts for folder : " + folderName;
            if (this.reqFolders[this.folderName] == null) { 
                List<Dictionary<string,string>> requirements =  this.oslcManager
                    .GetBodyOfRequirements(this.SERVICE_PROVIDER_URI,
                    this.oslcManager.getRequirementsByFolder(this.SERVICE_PROVIDER_URI, FolderURI)
                    );
                this.reqFolders[this.folderName] = requirements;
            }

            Requirements_listView.Items.Clear();
            foreach (var item in this.reqFolders[this.folderName])
            {
                Requirements_listView.Items.Add(new ListViewItem(
                    new[] {
                    item["id"],
                    item["title"],
                    item["created"],
                    item["modified"],
                    item["creator"]
                    }));
            }

            informLabel.Text = "Artifacts loaded for folder : " + folderName;
            this.Reload_artifacts_button.Enabled = true;

        }

        private void ShowRequirementsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.caller.Close();
        }

        private void Requirements_listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.reqFolders[this.folderName].OrderBy(p => p["id"]);
        }


        private void Requirements_listView_Click(object sender, EventArgs e)
        {
            this.RequirementDetail_panel.Visible = true;
            var Requirement = this.reqFolders[folderName].Select(x => x)
                .Where(x => x.Values.Contains(this.Requirements_listView.SelectedItems[0].Text))
                .FirstOrDefault();
            this.Title_label.Text = Requirement["title"];
            this.ID_label.Text = Requirement["id"];
            this.Created_label.Text = Requirement["created"];
            this.Modified_label.Text = Requirement["modified"];
            this.Creator_label.Text = Requirement["creator"];

            this.Description_textbox.Text = Requirement["description"];
        }

        private void Reload_artifacts_button_Click(object sender, EventArgs e)
        {
            this.reqFolders[folderName] = null;
            this.Artifact_foldersListbox_Click(sender, e);
        }
    }
}
