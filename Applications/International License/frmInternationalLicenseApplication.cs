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

    public partial class frmInternationalLicenseApplication : Form
    {
        int LicenseID = -1;
        int LocalID = -1;
        int ThisInternationalLicenseID = -1;
        int ApplicantPerson = -1;

        clsLicense LocalLicenseInfo;
        public frmInternationalLicenseApplication()
        {
            InitializeComponent();
            ctrlLIcenseInfoWithFilter1.SearchCompleted += GetLicenseID;

        }

        private void GetLicenseID(int ID)
        {
            LicenseID = ID;
            FillUsingLocalID(LicenseID);

            if (LicenseID != -1)
            {
                LocalLicenseInfo = clsLicense.GetLicenseByID(LicenseID);

                if (LocalLicenseInfo == null)
                {
                    MessageBox.Show("Unable to find local license with id : " + LicenseID);
                    return;
                }
                ApplicantPerson = clsApplications.GetApplicantPersonID(LocalLicenseInfo.ApplicationID);
            }
            else
            {
                MessageBox.Show("No Local License ID");
                return;
            }
        }


        clsInternationalLicenses InternationalLic;
        clsApplications MainApp;
        clsApplicationTypes AppType = clsApplicationTypes.FindApplicationTypeByID(6);
        private void FillUsingLocalID(int LocalLicenseID)
        {
            InternationalLic = clsInternationalLicenses.GetInternationalLicenseUsingLocalLicenseID(LocalLicenseID);

            if (InternationalLic != null && InternationalLic.IsActive == true)
            {
                ThisInternationalLicenseID = InternationalLic.InternationalLicenseID;
                MainApp = clsApplications.FindGeneralApplicationByID(InternationalLic.ApplicationID);

                lblILLicenseID.Text = InternationalLic.InternationalLicenseID.ToString();

                lblILApplicationID.Text = InternationalLic.ApplicationID.ToString();
                lblLocalLicenseID.Text = InternationalLic.IssuedUsingLocalLicenseID.ToString();

                lblIssueDate.Text = InternationalLic.IssueDate.ToShortDateString();
                lblExpirationDate.Text = InternationalLic.ExpirationDate.ToShortDateString();

                clsUser user = clsUser.FindUserByID(InternationalLic.CreatedByUserID);
                if (user != null)
                { lblCreatedBy.Text = user.UserName.ToString(); }
                else { lblCreatedBy.Text = "Unknown"; }

                lblApplicationDate.Text = MainApp.ApplicationDate.ToShortDateString();
                lblFees.Text = MainApp.PaidFees.ToString();


                MessageBox.Show("Person Already Has an Active International License with ID : " + InternationalLic.InternationalLicenseID);
                btnIssue.Enabled = false;
                llblShowLicenseInfo.Enabled = true;
            }
            else
            {
                btnIssue.Enabled = true;
                llblShowLicenseInfo.Enabled = false;
             

                lblILLicenseID.Text = "????";
                lblILApplicationID.Text = "????";

                if (LicenseID != -1)
                {
                    lblLocalLicenseID.Text = LicenseID.ToString();
                }
                else { lblLocalLicenseID.Text = "????"; }

                lblIssueDate.Text = DateTime.Now.ToShortDateString();
                lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();

                lblCreatedBy.Text = CurrentUserManager.CurrentUser.UserName;

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblFees.Text = AppType.ApplicationFees.ToString();

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ApplicantPerson != -1)
            {
                frmLicenseHistory frm = new frmLicenseHistory(ApplicantPerson);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Selected Person");
            }
        }

        public delegate void IssueCompletedEventHandler();
        public event IssueCompletedEventHandler IssueCompleted;
        protected virtual void OnIssueCompleted()
        {
            IssueCompleted?.Invoke();
        }


        private void btnIssue_Click(object sender, EventArgs e)
        {
            
            bool IsPreviousExist = clsInternationalLicenses.IsPreviousInternationalLicenseExist(LicenseID);

            if (IsPreviousExist)
            {
                MessageBox.Show("You Already Have an Active International License");
                return;
            }

            if(LocalLicenseInfo.IsActive == false) 
            {
                MessageBox.Show("Local License Is Deactivated, Unable to perform the operation");
                return;
            }

            if(LocalLicenseInfo.ExpirationDate < DateTime.Now) 
            {
                MessageBox.Show("Local License Is Expired, Unable to perform the operation");
                return;
            }

            if(LocalLicenseInfo.LicenseClass != 3) 
            {
                MessageBox.Show("Invalid Local License Class");
                return;
            }

            clsApplications NewApp = new clsApplications();
            clsInternationalLicenses NewInternational = new clsInternationalLicenses();



            NewApp.ApplicantPersonID = ApplicantPerson;

            if (AppType != null) 
            {
                NewApp.ApplicationTypeID = AppType.ApplicationID;
                NewApp.PaidFees = AppType.ApplicationFees;
            }
            else 
            {
                MessageBox.Show("Missed Info");
                return;
            }
               
            NewApp.ApplicationDate = DateTime.Now; 
            NewApp.ApplicationStatus = 3; 
            NewApp.LastStatusDate = DateTime.Now; 
            NewApp.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

            if (NewApp.SaveApplication()) 
            {
                NewInternational.ApplicationID = NewApp.ApplicationID;
                NewInternational.DriverID = LocalLicenseInfo.DriverID;
                NewInternational.IssuedUsingLocalLicenseID = LocalLicenseInfo.LicenseID;
              
                NewInternational.IssueDate = DateTime.Now;
                NewInternational.ExpirationDate = DateTime.Now.AddYears(1);

                NewInternational.IsActive = true;
                NewInternational.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

                if (NewInternational.Save()) 
                {
                    MessageBox.Show("International License Issued Successfully with ID : " + NewInternational.InternationalLicenseID);
                    FillUsingLocalID(LicenseID);
                    OnIssueCompleted();
                }
                else 
                {
                    MessageBox.Show("Error, Failed to Add a New International License");
                }

            }
            else 
            {
                MessageBox.Show("Error, Failed to Add a New Application");
            }


        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(ThisInternationalLicenseID != -1) 
            {
                frmInternationalDeriverLicenseInfo frm = new frmInternationalDeriverLicenseInfo(ThisInternationalLicenseID);
                frm.ShowDialog();
            }
            else 
            {
                MessageBox.Show("Missed Data, Unable to Perform the Operation");
            }
        }
    }


}
