using DVLD_Project_Mine_.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project_Mine_
{
    public partial class ctrlLicenseInfo : UserControl
    {
        private static readonly string ImageFolder = Path.Combine(Application.StartupPath, "PeopleImages");
        public ctrlLicenseInfo()
        {
            InitializeComponent();
        }

        clsPersons PersonInfo;
        clsLicense LicenseInfo;
        clsLocalDrivingLicenseApplicationInfo LocalDLInfo;
        private void SetDefaultImage() 
        {
            if (PersonInfo.Gender == 0)
            {
                pictureBox1.Image = Resources.male;
            }
            else
            {
                pictureBox1.Image = Resources.female;
            }
        }

        private void AssignData() 
        {
            lblName.Text = PersonInfo.FirstName + " " + PersonInfo.SecondName + " " +
                PersonInfo.ThirdName + " " + PersonInfo.LastName;

            lblNationalNo.Text = PersonInfo.NationalNo.ToString();
            lblDateOfBirth.Text = PersonInfo.DateOfBirth.ToShortDateString();

            if (PersonInfo.Gender == 0)
            {
                lblGender.Text = "Male";
            }
            else
            {
                lblGender.Text = "Female";
            }

            if (PersonInfo.ImagePath != "")
            {
                try
                {
                    pictureBox1.Image = Image.FromFile(ImageFolder + "\\" + PersonInfo.ImagePath);
                }
                catch
                {
                    MessageBox.Show("Could not find Image Path");
                    SetDefaultImage();

                }
            }
            else
            {
                MessageBox.Show("No Saved Image for this Person!");
                SetDefaultImage();
            }



            lblLicenseID.Text = LicenseInfo.LicenseID.ToString();

            if (LicenseInfo.IsActive)
            {
                lblIsActive.Text = "Yes";
            }
            else
            {
                lblIsActive.Text = "No";
            }

            lblDriverID.Text = LicenseInfo.DriverID.ToString();
            lblIssueDate.Text = LicenseInfo.IssueDate.ToShortDateString();
            lblExpirationDate.Text = LicenseInfo.ExpirationDate.ToShortDateString();

            if (LicenseInfo.IssueReason == 1)
            {
                lblIssueReason.Text = "First Time";
            }
            else if (LicenseInfo.IssueReason == 2)
            {
                lblIssueReason.Text = "Renew";
            }
            else if (LicenseInfo.IssueReason == 3)
            {
                lblIssueReason.Text = "Replaced For Lost";
            }
            else if (LicenseInfo.IssueReason == 4)
            {
                lblIssueReason.Text = "Replaced For Damage";
            }
            else
            {
                lblIssueReason.Text = "Unknown";
            }

            if (LicenseInfo.Notes == "")
            {
                lblNotes.Text = "No Notes";
            }
            else
            {
                lblNotes.Text = LicenseInfo.Notes.ToString();
            }

            if (clsDetainedLicense.IsLicenseDetained(LicenseInfo.LicenseID))
            {
                lblIsDetained.Text = "Yes";
            }
            else
            {
                lblIsDetained.Text = "No";
            }
        }
        public void Fill(int LocalDLID) 
        {
            LocalDLInfo = clsLocalDrivingLicenseApplicationInfo.GetLocalDrivingLicenseApplicationInfoByLDLAPPID(LocalDLID);
            PersonInfo = clsPersons.FindPersonByID(LocalDLInfo.ApplicantPersonID);
            LicenseInfo = clsLicense.GetLicenseByApplicationID(LocalDLInfo.ApplicationID);

            lblClass.Text = LocalDLInfo.Class.ToString();

            AssignData();
        }

        public void FillByLicenseID(int LicenseID) 
        {
            if (!clsLicense.IsLicenseExist(LicenseID)) 
            {
                MessageBox.Show("No License With ID : " + LicenseID);
                return;
            }

            LicenseInfo = clsLicense.GetLicenseByID(LicenseID);
            int PersonID = clsApplications.GetApplicantPersonID(LicenseInfo.ApplicationID);
            PersonInfo = clsPersons.FindPersonByID(PersonID);
            clsLicensesClasses LClass = clsLicensesClasses.FindLicenseClassByID(LicenseInfo.LicenseClass);

            lblClass.Text = LClass.ClassName.ToString();

            AssignData();

        }
  
    }
}
