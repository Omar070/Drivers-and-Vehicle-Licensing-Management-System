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
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        private static readonly string ImageFolder = Path.Combine(Application.StartupPath, "PeopleImages");
        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        clsInternationalLicenses IntLicense;
        clsPersons Person;
        int PersonID = -1;
        
        private void SetDefaultImage()
        {
            if (Person.Gender == 0)
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
            lblName.Text = Person.FirstName + " " + Person.SecondName
               + " " + Person.ThirdName + " " + Person.LastName;

            lblIntLicenseID.Text = IntLicense.InternationalLicenseID.ToString();
            lblApplicationID.Text = IntLicense.ApplicationID.ToString();
            lblLicenseID.Text = IntLicense.IssuedUsingLocalLicenseID.ToString();

            if (IntLicense.IsActive == true)
            {
                lblIsActive.Text = "Active";
            }
            else { lblIsActive.Text = "Deactivated"; }

            lblNationalNo.Text = Person.NationalNo.ToString();
            lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();

            if (Person.Gender == 0)
            {
                lblGender.Text = "Male";
            }
            else { lblGender.Text = "Female"; }

            lblDeriverID.Text = IntLicense.DriverID.ToString();

            lblIssueDate.Text = IntLicense.IssueDate.ToShortDateString();
            lblExpirationDate.Text = IntLicense.ExpirationDate.ToShortDateString();

            if (Person.ImagePath != "")
            {
                try
                {
                    pictureBox1.Image = Image.FromFile(ImageFolder + "\\" + Person.ImagePath);
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
        }
        public void Fill(int IntLicenseID) 
        {
            IntLicense = clsInternationalLicenses.GetInternationalLicenseByID(IntLicenseID);
            PersonID = clsApplications.GetApplicantPersonID(IntLicense.ApplicationID);

            if (PersonID != -1) 
            {
                Person = clsPersons.FindPersonByID(PersonID);
            }
            
            if(Person == null || IntLicense == null) 
            { 
                MessageBox.Show("Missed Data");
                return;
            }

            AssignData();
        }

    }
}
