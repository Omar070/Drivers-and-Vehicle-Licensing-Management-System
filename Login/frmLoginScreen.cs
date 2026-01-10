using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLayer;
using System.IO;

namespace DVLD_Project_Mine_
{
    public partial class frmLoginScreen : Form
    {
        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (clsUser.Authenticate(txtUserName.Text, txtPassword.Text))
            {
                frmMainScreen frm = new frmMainScreen();
                frm.Show();
                this.Hide();
            }
            else 
            {
                MessageBox.Show("Login Failed, Please Check UserName, Password, Or Activity Status");
            }

        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {

        }

    }
}
