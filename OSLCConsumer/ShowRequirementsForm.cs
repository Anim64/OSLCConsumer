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
            
            int beg = this.Artifact_foldersListbox.SelectedItem.ToString().IndexOf(",")+1;
            int end = this.Artifact_foldersListbox.SelectedItem.ToString().Length - 1;
            string folderName = this.Artifact_foldersListbox.SelectedItem.ToString().Substring(1, beg - 2);
            string FolderURI = this.Artifact_foldersListbox.SelectedItem.ToString()
                .Substring(beg, end - beg).Trim();
            informLabel.Text = "Loading Artifacts for folder : " + folderName;
            if (this.reqFolders[folderName] == null) { 
                List<Dictionary<string,string>> requirements =  this.oslcManager
                    .GetBodyOfRequirements(this.SERVICE_PROVIDER_URI,
                    this.oslcManager.getRequirementsByFolder(this.SERVICE_PROVIDER_URI, FolderURI)
                    );
                this.reqFolders[folderName] = requirements;
            }

            Requirements_listView.Items.Clear();
            foreach (var item in this.reqFolders[folderName])
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

            informLabel.Text = "Arefacts loaded for folder : " + folderName;




        }

        private void ShowRequirementsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.caller.Close();
        }
    }
}
