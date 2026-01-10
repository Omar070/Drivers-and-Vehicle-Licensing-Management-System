using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project_Mine_
{
    public partial class frmReleaseDetainedLicense : Form
    {
        int LicenseID = -1, PersonID = -1;
        decimal AppFees = clsApplicationTypes.GetAppTypeFeesByAppTypeID(5);
        clsDetainedLicense ThisDetainedLicense;

        public frmReleaseDetainedLicense(int LID = -1)
        {
            InitializeComponent();

            if (LID != -1)
            {
                ctrlLIcenseInfoWithFilter1.FillWithoutSearch(LID);
                ctrlLIcenseInfoWithFilter1.DisableFilter();
                GetLicenseID(LID);
            }
            else 
            {
                ctrlLIcenseInfoWithFilter1.SearchCompleted += GetLicenseID;
            }

        }

        private void GetLicenseID(int LID)
        {
            if (!clsLicense.IsLicenseExist(LID)) { return; }

            LicenseID = LID;
            PersonID = clsLicense.GetApplicantPersonIDByLicenseID(LicenseID);

            btnRelease.Enabled = true;
            llblShowLicenseDetails.Enabled = false;

            if (!clsDetainedLicense.IsLicenseDetained(LicenseID))
            {
                MessageBox.Show("This License Is Not Detained");
                btnRelease.Enabled = false;
                return;
            }

            ctrlReleaseDetainedInfo1.Fill(LicenseID);
            ThisDetainedLicense = clsDetainedLicense.GetDetainedLicenseByLicenseID(LicenseID);

        }

        private void llblShowPersonLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (PersonID != -1)
            {
                frmLicenseHistory frm = new frmLicenseHistory(PersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Selected Person");
            }
        }

        private void llblShowLicenseDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LicenseID != -1)
            {
                frmDeriverLicenseInfo frm = new frmDeriverLicenseInfo(0, LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Selected Person");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public delegate void ReleaseDoneEventHandler();
        public event ReleaseDoneEventHandler ReleaseDone;
        protected virtual void OnReleaseDone()
        {
            ReleaseDone?.Invoke();
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            clsApplications NewApp = new clsApplications();

            NewApp.ApplicantPersonID = PersonID;

            NewApp.ApplicationTypeID = 5;
            NewApp.PaidFees = AppFees;

            NewApp.ApplicationDate = DateTime.Now;

            NewApp.ApplicationStatus = 3;
            NewApp.LastStatusDate = DateTime.Now; // may be replaced by ThisDetainedLicense.DetainDate

            NewApp.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

            if (NewApp.SaveApplication())
            {
                ThisDetainedLicense.ReleaseApplicationID = NewApp.ApplicationID;
                ThisDetainedLicense.ReleaseDate = DateTime.Now;
                ThisDetainedLicense.ReleasedByUserID = CurrentUserManager.CurrentUser.UserID;

                if (ThisDetainedLicense.Save())
                {
                    btnRelease.Enabled = false;
                    llblShowLicenseDetails.Enabled = true;

                    ctrlReleaseDetainedInfo1.GetAppID(ThisDetainedLicense.ReleaseApplicationID);
                    MessageBox.Show("License Released Successfully");
                    
                    OnReleaseDone();
                }
                else 
                {
                    MessageBox.Show("Error, Unable To Release The License");
                }

            }
            else 
            {
                MessageBox.Show("Error, Unable To Add a New Application");
            }


        }


    }

}
