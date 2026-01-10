using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project_Mine_
{
    public partial class ctrlDetainInfo : UserControl
    {
        public ctrlDetainInfo()
        {
            InitializeComponent();

            lblCreatedBy.Text = CurrentUserManager.CurrentUser.UserName;
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
        }

        public void Fill(int LicenseID) 
        {
            if (!clsLicense.IsLicenseExist(LicenseID)) { return; }

            lblLicenseID.Text = LicenseID.ToString();
        }

        public void SendFineFees(ref decimal fineFees) 
        {
            if(decimal.TryParse(txtFineFees.Text, out decimal Fine)) 
            {
                fineFees = Fine;
            }
            else 
            {
                return;
            }
        }

        public void GetDetainID(int detainID) 
        {
            lblDetainID.Text = detainID.ToString();
            txtFineFees.Enabled = false;
        }

    }
}
