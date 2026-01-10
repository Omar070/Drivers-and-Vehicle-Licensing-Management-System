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
    public partial class frmRenewLicenseApplication : Form
    {
        int LicenseID = -1, NewLicenseID = -1;
        
        int PersonId = -1, Validity = -1;
        decimal AppFees = 0;
        string Notes = "";
        

        clsLicense OldLicense;
        public frmRenewLicenseApplication()
        {
            InitializeComponent();

            ctrlLIcenseInfoWithFilter1.SearchCompleted += GetLicenseID;
            ctrlApplicationNewLicenseInfo1.SearchCompleted += GetNotes;
        }
        
        private void GetLicenseID(int id) 
        {
            LicenseID = id;


            if (!clsLicense.IsLicenseExist(LicenseID)) { return; }


            ctrlApplicationNewLicenseInfo1.Fill(LicenseID);
                

            OldLicense = clsLicense.GetLicenseByID(LicenseID);

            btnRenew.Enabled = true;
            if (OldLicense.ExpirationDate > DateTime.Now)
            {
                MessageBox.Show("Selected License Isn't Yet Expired, It Will Expire on " + OldLicense.ExpirationDate.ToShortDateString());
                btnRenew.Enabled = false;
                llblShowNewLicenseInfo.Enabled = false;
            }

            PersonId = clsApplications.GetApplicantPersonID(OldLicense.ApplicationID);
            Validity = clsLicensesClasses.GetClassValidityByClassID(OldLicense.LicenseClass);
            AppFees = clsApplicationTypes.GetAppTypeFeesByAppTypeID(2);
        }
        private void GetNotes(string notes) 
        {
            Notes = notes;
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

        private void llblShowLIcenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void btnRenew_Click(object sender, EventArgs e)
        {
            clsApplications NewApp = new clsApplications();
            clsLicense NewLicense = new clsLicense();

            NewApp.ApplicantPersonID = PersonId;

            NewApp.ApplicationDate = DateTime.Now;
            NewApp.ApplicationTypeID = 2;

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

                NewLicense.Notes = Notes;
                NewLicense.PaidFees = OldLicense.PaidFees;

                NewLicense.IsActive = true;
                NewLicense.IssueReason = 2;

                NewLicense.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

                if (NewLicense.Save()) 
                {
                    NewLicenseID = NewLicense.LicenseID;
                    OldLicense.IsActive = false;

                    if (OldLicense.Save()) 
                    {
                        clsInternationalLicenses OldIntLicense = clsInternationalLicenses.GetInternationalLicenseUsingLocalLicenseID(OldLicense.LicenseID); 
                        if(OldIntLicense != null) 
                        {
                            OldIntLicense.IsActive = false;
                            OldIntLicense.Save();
                        }

                        
                        ctrlApplicationNewLicenseInfo1.Fill(OldLicense.LicenseID, NewLicense.LicenseID);
                        ctrlLIcenseInfoWithFilter1.DisableFilter();


                        llblShowNewLicenseInfo.Enabled = true;
                        btnRenew.Enabled = false;

                        MessageBox.Show("License Renewed Successfully With ID : " + NewLicense.LicenseID);
                    }


                }
            }

        }


    }


}
