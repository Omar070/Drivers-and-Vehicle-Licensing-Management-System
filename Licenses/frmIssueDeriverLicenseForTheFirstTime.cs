using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project_Mine_
{
    public partial class frmIssueDeriverLicenseForTheFirstTime : Form
    {
        clsLocalDrivingLicenseApplicationInfo LocalApp;
        clsApplications MainApp;
        clsLicensesClasses Class;
        clsDrivers Driver;
        public frmIssueDeriverLicenseForTheFirstTime(int id)
        {
            InitializeComponent();

            LocalApp = clsLocalDrivingLicenseApplicationInfo.GetLocalDrivingLicenseApplicationInfoByLDLAPPID(id);
            MainApp = clsApplications.FindGeneralApplicationByID(LocalApp.ApplicationID);
            Class = clsLicensesClasses.FindLicenseClassByName(LocalApp.Class);

            ctrlApplicationDetails1.Fill(LocalApp.LocalDrivingLicenseApplicationID);
        }

        public delegate void SaveCompletedEventHandler();
        public event SaveCompletedEventHandler SaveCompleted;
        protected virtual void OnSaveCompleted() 
        {
            SaveCompleted?.Invoke();
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            int PrevDriverID = clsDrivers.IsPersonAlreadyADriver(LocalApp.ApplicantPersonID);

            if(PrevDriverID == -1) 
            {
                Driver = new clsDrivers();

                Driver.PersonID = LocalApp.ApplicantPersonID;
                Driver.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;
                Driver.CreatedDate = DateTime.Now;
                if (Driver.Save()) 
                {
                    PrevDriverID = Driver.DriverID;
                }
                else
                {
                    MessageBox.Show("Operation Failed, Unable to add the Driver");
                    return;
                }
            }
            

             clsLicense License = new clsLicense();

             License.ApplicationID = MainApp.ApplicationID;
             License.DriverID = PrevDriverID;
            
             License.LicenseClass = Class.LicenseClassID;
            
             License.IssueDate = DateTime.Now;
             License.ExpirationDate = License.IssueDate.AddYears(Class.DefaultValidityLength);
            
             License.Notes = textBox1.Text;

             License.PaidFees = Class.ClassFees;
             License.IsActive = true;
             License.IssueReason = 1;
             License.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

             if (License.Save()) 
             {
                 MainApp.ApplicationStatus = 3;
                 if (MainApp.SaveApplication()) 
                 {
                     MessageBox.Show("License Issued Successfully With License ID : "
                     + License.LicenseID);
                     OnSaveCompleted();
                 }
                 else 
                 {
                     MessageBox.Show("Application Status Did not Updated Successfully");
                 }
             }
             else 
             {
                 MessageBox.Show("Operation Failed,Unable to add the License");
             }

            
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
