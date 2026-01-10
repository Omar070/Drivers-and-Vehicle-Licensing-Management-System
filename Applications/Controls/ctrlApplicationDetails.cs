using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD_Project_Mine_
{
    public partial class ctrlApplicationDetails : UserControl
    {
        public ctrlApplicationDetails()
        {
            InitializeComponent();

        }

        clsLocalDrivingLicenseApplicationInfo ob;

        public void Fill(int ID) 
        {
            ob = clsLocalDrivingLicenseApplicationInfo.GetLocalDrivingLicenseApplicationInfoByLDLAPPID(ID);

            lblDLAppID.Text = ob.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = ob.Class.ToString();
            lblPassedTests.Text = ob.PassedTestsCount.ToString() + "/3";
            lblID.Text = ob.ApplicationID.ToString();
            lblStatus.Text = ob.Status.ToString();
            lblFees.Text = ob.PaidFees.ToString();
            lblType.Text = ob.ApplicationTypeTitle.ToString();
            lblApplicant.Text = ob.ApplicantName.ToString();
            lblDate.Text = ob.ApplicationDate.ToShortDateString();
            lblStatusDate.Text = ob.LastStatusDate.ToShortDateString();
            lblCreatedBy.Text = ob.CreatedByUserName.ToString();

            if(ob.Status == "Completed") 
            {
                llShowLicenseInfo.Enabled = true;
            }
            
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(ob.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDeriverLicenseInfo frm = new frmDeriverLicenseInfo(ob.LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
        }
    }
}
