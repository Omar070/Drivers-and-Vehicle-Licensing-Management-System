using DVLD_Project_Mine_.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DVLD_Project_Mine_
{
    public partial class ctrlPersonInfo : UserControl
    {
        private static readonly string ImageFolder = Path.Combine(Application.StartupPath, "PeopleImages");
        public ctrlPersonInfo()
        {
            InitializeComponent();
        }

        // Parametrized constructor 
        int CurrentID;
  
        string country;
        clsPersons Person;

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
            lblPersonID.Text = Person.PersonID.ToString();

            lblName.Text = Person.FirstName + " " + Person.SecondName + " " +
                Person.ThirdName + " " + Person.LastName;

            lblNationalNo.Text = Person.NationalNo;

            lblEmail.Text = Person.Email;

            lblAddress.Text = Person.Address;

            if (Person.Gender == 0)
            { lblGender.Text = "Male"; }
            else
            { lblGender.Text = "Female"; }

            lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();

            lblPhone.Text = Person.Phone;

            if (clsPersons.GetCountryName(Person.NationalityCountryID, ref country))
            {
                lblCountry.Text = country;
            }
            else
            {
                lblCountry.Text = "Error";
            }

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

        public void FillPersonInfo(int ID)
        {
            CurrentID = ID;
            Person = clsPersons.FindPersonByID(ID);
           
            if(Person != null) 
            {
                AssignData();
            }
            else
            {
                MessageBox.Show("Invalid ID");
            }
            
        }
        
        public void REFRESH(object sender, EventArgs e) 
        {
            FillPersonInfo(CurrentID);
        }

   
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson(CurrentID);
            frm.SaveCompleted += REFRESH;
            frm.ShowDialog();
        }

   
    }

}
