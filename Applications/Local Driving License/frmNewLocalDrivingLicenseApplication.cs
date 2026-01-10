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
    public partial class frmNewLocalDrivingLicenseApplication : Form
    {
        private int PersonID = -1;

        private DataTable dt;
        public frmNewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            ctrlUserInfoWithFilter1.SearchCompletedSend += GetPersonID;

            dt = clsApplications.FillApplicationClasses();
            cbClasses.DataSource = dt;
            cbClasses.DisplayMember = "ClassName";
            cbClasses.ValueMember = "LicenseClassID";

            lblAppDate.Text = DateTime.Now.ToShortDateString();

            lblAppFees.Text = AppType.ApplicationFees.ToString();
            lblUserID.Text = CurrentUserManager.CurrentUser.UserName.ToString();
        }
        public void GetPersonID(int ID) 
        {
            PersonID = ID;
        }

        clsApplicationTypes AppType = clsApplicationTypes.FindApplicationTypeByTitle("New Local Driving License Service");
        
        clsApplications App = new clsApplications();
        

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (PersonID != -1) 
            {
                tabControl1.SelectedIndex = 1;
            }
            else 
            {
                MessageBox.Show("Please Select a Person");
            }
        }

        private void frmNewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AssignData() 
        {
            App.ApplicantPersonID = PersonID;
            App.LastStatusDate = App.ApplicationDate;
            App.ApplicationDate = DateTime.Now;
            App.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;
            App.ApplicationTypeID = AppType.ApplicationID;
            App.PaidFees = AppType.ApplicationFees;
            App.LicenseClassID = Convert.ToInt32(cbClasses.SelectedValue);
            App.ApplicationStatus = 1; // May need modification

        }

        // Don't forget the event that fire after running save function 
        public delegate void SaveCompletedSendEventHandler();
        public event SaveCompletedSendEventHandler SaveCompletedSend;
        protected virtual void OnSaveCompletedSend()
        {
            SaveCompletedSend?.Invoke();
        }

        private bool Check() 
        {
            bool IsFound = false;
            clsPersons Per = clsPersons.FindPersonByID(PersonID);
            int PreviousAppID = -1;
           
            try
            {
                int selectedLicenseClassID = (int)cbClasses.SelectedValue;
                DataRow selectedRow = dt.Select($"LicenseClassID = {selectedLicenseClassID}").FirstOrDefault();
                if (selectedRow != null)
                {
                    string selectedClassName = selectedRow["ClassName"].ToString();
                    PreviousAppID = clsApplications.IsPreviousApplicationExist(Per.NationalNo, selectedClassName, "New");
                    if(PreviousAppID == -1) 
                    {
                        PreviousAppID = clsApplications.IsPreviousApplicationExist(Per.NationalNo, selectedClassName, "Completed");
                    }
                }
            }
            catch { }

            if(PreviousAppID != -1) 
            {
                MessageBox.Show("Choose another license class, Selected Person already have an active application for the same class with id:" + PreviousAppID.ToString());
                IsFound = true;
            }
            return IsFound;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           
            if(PersonID != -1) 
            {
                if (Check()) { return; }

                AssignData();

                if (App.Save()) 
                {
                    lblAppID.Text = App.ApplicationID.ToString();
                    MessageBox.Show("Operation Done Successfully");
                    OnSaveCompletedSend();

                }
                else 
                {
                    MessageBox.Show("Failed to save");
                }
            }
            else 
            {
                MessageBox.Show("Missed info");
            }
        }


    }
}
