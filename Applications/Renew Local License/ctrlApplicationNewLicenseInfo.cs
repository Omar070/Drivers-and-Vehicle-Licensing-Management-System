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
    public partial class ctrlApplicationNewLicenseInfo : UserControl
    {
        public ctrlApplicationNewLicenseInfo()
        {
            InitializeComponent();
        }

        public delegate void SearchCompletedEventHandler(string notes);
        public event SearchCompletedEventHandler SearchCompleted;
        protected virtual void OnSearchCompleted(string notes) 
        {
            SearchCompleted?.Invoke(notes);
        }

        public void Fill(int OldLicenseID, int  NewLicenseID = -1) 
        {
            if(NewLicenseID == -1) 
            {
                clsLicense OldLic = clsLicense.GetLicenseByID(OldLicenseID);
                clsLicensesClasses LClass = clsLicensesClasses.FindLicenseClassByID(OldLic.LicenseClass);
                decimal AppTypeFees = clsApplicationTypes.GetAppTypeFeesByAppTypeID(2);

                lblRLAppicationID.Text = "????";
                lblRenewedLicenseID.Text = "????";

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblIssueDate.Text = DateTime.Now.ToShortDateString();
                lblExpirationDate.Text = DateTime.Now.ToShortDateString();

                lblOldLicenseID.Text = OldLicenseID.ToString();

                lblApplicationFees.Text = AppTypeFees.ToString();
                lblLicenseFees.Text = LClass.ClassFees.ToString();
                lblTotalFees.Text = (AppTypeFees + LClass.ClassFees).ToString();

                lblCreatedBy.Text = CurrentUserManager.CurrentUser.UserName;
            }
            else 
            {
                clsLicense NewLic = clsLicense.GetLicenseByID(NewLicenseID);
                clsApplications MainApp = clsApplications.FindGeneralApplicationByID(NewLic.ApplicationID);
                clsUser User = clsUser.FindUserByID(NewLic.CreatedByUserID);

                lblRLAppicationID.Text = NewLic.ApplicationID.ToString();
                lblRenewedLicenseID.Text = NewLic.LicenseID.ToString();

                lblApplicationDate.Text = MainApp.ApplicationDate.ToShortDateString();
                lblIssueDate.Text = NewLic.IssueDate.ToShortDateString();
                lblExpirationDate.Text = NewLic.ExpirationDate.ToShortDateString();

                lblOldLicenseID.Text = OldLicenseID.ToString();

                lblApplicationFees.Text = MainApp.PaidFees.ToString();
                lblLicenseFees.Text = NewLic.PaidFees.ToString();
                lblTotalFees.Text = (MainApp.PaidFees + NewLic.PaidFees).ToString();

                lblCreatedBy.Text = User.UserName;

                if(NewLic.Notes != "") 
                {
                    textBox1.Text = NewLic.Notes;
                }
            }

            OnSearchCompleted(textBox1.Text);
        }


        

    }
}
