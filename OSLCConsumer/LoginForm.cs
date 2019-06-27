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
    public partial class LoginForm : Form
    {
        private OSLCManager oslcManager;
        private string SERVER = "https://158.196.141.113/";
        private string JTS_SERVER = "";
        private string PROJECT_AREA = "Test project";
        private string CATALOG_URI = null;
        private string SERVICE_PROVIDER_URI = null;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void login_button_Click(object sender, EventArgs e)
        {
            login_button.Enabled = false;
            string login = login_textbox.Text;
            string password = password_textbox.Text;

            if(login.Length > 0 && password.Length > 0) {
                oslcManager = new OSLCManager(SERVER, JTS_SERVER, login, password, PROJECT_AREA);
                CATALOG_URI = oslcManager.GetServiceProviderCatalog();

                SERVICE_PROVIDER_URI = oslcManager.GetServiceProvider(CATALOG_URI);
                if (SERVICE_PROVIDER_URI == null)
                {
                    login_warning_label.Text = "Login or Password is incorrect";
                    login_warning_label.Visible = true;
                    login_button.Enabled = true;
                }
                else { 
                    login_warning_label.Text = "Login OK";
                    login_warning_label.Visible = true;
                    login_button.Enabled = true;
                }

            }
            else
            {
                login_warning_label.Text = "Login or Password is empty";
                login_warning_label.Visible = true;
                login_button.Enabled = true;
            }
        }
    }
}
