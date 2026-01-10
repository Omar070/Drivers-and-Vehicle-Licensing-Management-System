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
    public partial class ctrlApplicationInfoForReplacement : UserControl
    {
        public ctrlApplicationInfoForReplacement()
        {
            InitializeComponent();
        }

        clsLicense NewLicense;
        public void Fill(int OldLicenseID = -1, int LicenseID = -1, decimal AppFees = -1) 
        {
            if (LicenseID != -1) 
            {
                NewLicense = clsLicense.GetLicenseByID(LicenseID);

                lblLRApplicationID.Text = NewLicense.ApplicationID.ToString();
                lblReplacedLicenseID.Text = NewLicense.LicenseID.ToString();

                lblApplicationDate.Text = NewLicense.IssueDate.ToShortDateString();
                lblOldLicenseID.Text = OldLicenseID.ToString();

                lblApplicationFees.Text = AppFees.ToString();
                lblCreatedBy.Text = CurrentUserManager.CurrentUser.UserName;
            }
            else 
            {
                lblLRApplicationID.Text = "????";
                lblReplacedLicenseID.Text = "????";

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblOldLicenseID.Text = OldLicenseID.ToString();

                lblApplicationFees.Text = AppFees.ToString();
                lblCreatedBy.Text = CurrentUserManager.CurrentUser.UserName;
            }
            


        }


    }
}
