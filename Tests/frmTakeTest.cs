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
    public partial class frmTakeTest : Form
    {
        clsTestAppointments TestAppoint;
        clsTests Test = new clsTests();
        public frmTakeTest(int testAppointment)
        {
            InitializeComponent();
            TestAppoint = clsTestAppointments.FindTestAppointmentByID(testAppointment);

            ctrlScheduleTest1.Fill(TestAppoint.LocalDrivingLicenseID, TestAppoint.TestTypeID, TestAppoint.TestAppointmentID);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public delegate void SaveCompletedEventHandler();
        public event SaveCompletedEventHandler SaveCompleted;
        protected virtual void OnSaveCompleted() 
        {
            SaveCompleted?.Invoke();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Test.TestAppointmentID = TestAppoint.TestAppointmentID;

            if (rbPass.Checked)
            {
                Test.Result = true;
            }
            else { Test.Result = false; }

            Test.Notes = textBox1.Text;
            Test.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;


            // MessageBox.Show("Are You Sure You Want To Save? .. After Saving You Will Not Be Able To Change The Result");

            // May need to involve the following logic inside the message box result


            if (Test.AddNewTest()) 
            {
                MessageBox.Show("Data Saved Successfully");
                lblTestID.Text = Test.TestID.ToString();
            }
            else { MessageBox.Show("Operation Failed"); }

            OnSaveCompleted();

        }
    }
}
