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
    public partial class frmReplacementForLostOrDamagedLicense : Form
    {
        int OldLicenseID = -1, NewLicenseID = -1;
        int PersonId = -1, Validity = -1, AppTypeID = -1;
        decimal AppFees = 0;


        clsLicense OldLicense;

        public frmReplacementForLostOrDamagedLicense()
        {
            InitializeComponent();

            ctrlLIcenseInfoWithFilter1.SearchCompleted += GetLicenseID;
           
            rbDamaged.Checked = true;
           
            AppTypeID = 4;
            AppFees = clsApplicationTypes.GetAppTypeFeesByAppTypeID(4);

        }

        private void GetLicenseID(int id)
        {
            if (!clsLicense.IsLicenseExist(id)) { return; }

            OldLicenseID = id;

            ctrlApplicationInfo1.Fill(OldLicenseID, NewLicenseID, AppFees);

            OldLicense = clsLicense.GetLicenseByID(OldLicenseID);

            btnIssueReplacement.Enabled = true;

            if (OldLicense.IsActive == false)
            {
                MessageBox.Show("Selected License Is Inactive, Choose An Active License");

                btnIssueReplacement.Enabled = false;
                llblShowNewLicenseInfo.Enabled = false;

            }

            PersonId = clsApplications.GetApplicantPersonID(OldLicense.ApplicationID);
            Validity = clsLicensesClasses.GetClassValidityByClassID(OldLicense.LicenseClass);

        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            AppTypeID = 4;
            AppFees = clsApplicationTypes.GetAppTypeFeesByAppTypeID(4);

            ctrlApplicationInfo1.Fill(OldLicenseID, NewLicenseID, AppFees);
        }

        private void rbLost_CheckedChanged(object sender, EventArgs e)
        {
            AppTypeID = 3;
            AppFees = clsApplicationTypes.GetAppTypeFeesByAppTypeID(3);

            ctrlApplicationInfo1.Fill(OldLicenseID, NewLicenseID, AppFees);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (NewLicenseID != -1)
            {
                frmDeriverLicenseInfo frm = new frmDeriverLicenseInfo(0, NewLicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Selected Person");
            }
        }

        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (PersonId != -1)
            {
                frmLicenseHistory frm = new frmLicenseHistory(PersonId);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Selected Person");
            }
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            clsApplications NewApp = new clsApplications();
            clsLicense NewLicense = new clsLicense();

            NewApp.ApplicantPersonID = PersonId;

            NewApp.ApplicationDate = DateTime.Now;
            NewApp.ApplicationTypeID = AppTypeID;

            NewApp.ApplicationStatus = 3;
            NewApp.LastStatusDate = DateTime.Now;

            NewApp.PaidFees = AppFees;
            NewApp.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

            if (NewApp.SaveApplication())
            {

                NewLicense.ApplicationID = NewApp.ApplicationID;

                NewLicense.DriverID = OldLicense.DriverID;
                NewLicense.LicenseClass = OldLicense.LicenseClass;

                NewLicense.IssueDate = DateTime.Now;
                NewLicense.ExpirationDate = DateTime.Now.AddYears(Validity);

                NewLicense.Notes = "";
                NewLicense.PaidFees = OldLicense.PaidFees;

                NewLicense.IsActive = true;
                NewLicense.IssueReason = Convert.ToByte(AppTypeID);

                NewLicense.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

                if (NewLicense.Save())
                {
                    NewLicenseID = NewLicense.LicenseID;
                    OldLicense.IsActive = false;

                    if (OldLicense.Save())
                    {
                        clsInternationalLicenses OldIntLicense = clsInternationalLicenses.GetInternationalLicenseUsingLocalLicenseID(OldLicense.LicenseID);
                        if (OldIntLicense != null)
                        {
                            OldIntLicense.IsActive = false;
                            OldIntLicense.Save();
                        }


                        ctrlApplicationInfo1.Fill(OldLicenseID, NewLicenseID, AppFees);
                        ctrlLIcenseInfoWithFilter1.DisableFilter();

                        llblShowNewLicenseInfo.Enabled = true;

                        btnIssueReplacement.Enabled = false;
                        groupBox1.Enabled = false;

                        MessageBox.Show("License Replaced Successfully With ID : " + NewLicense.LicenseID);
                    }


                }
            }
        }

       




    }
}
