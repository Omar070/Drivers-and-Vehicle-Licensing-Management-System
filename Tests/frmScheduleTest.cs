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
    public partial class frmScheduleTest : Form
    {
        int LocalDLID, TestTypeId, RetakeID = -1, NumberOfPreviousTrials;
        DateTime AppDate = DateTime.Now;
        decimal Fees, RetakeFees;

        clsApplicationTypes AppType = clsApplicationTypes.FindApplicationTypeByID(7);
        clsTestAppointments TestAppointment = new clsTestAppointments();

        public frmScheduleTest( int LDLID, int TestTypeID, int PreviousTrials)
        {
            InitializeComponent();
            LocalDLID = LDLID;
            TestTypeId = TestTypeID;

            NumberOfPreviousTrials = PreviousTrials;

            ctrlScheduleTest1.DateChanged += GetDate;
            ctrlScheduleTest1.UserControlOpened += GetFees;
            

            ctrlScheduleTest1.Fill(LDLID, TestTypeID);

            if(PreviousTrials > 0) 
            {

                gbRetakeTest.Enabled = true;
                RetakeFees = AppType.ApplicationFees;
                lblRAppFees.Text = AppType.ApplicationFees.ToString();
                lblTotalFees.Text = (Fees + RetakeFees).ToString();

            }
            else 
            {
                gbRetakeTest.Enabled = false;
                RetakeFees = 0;
            }
        }

        public frmScheduleTest(int TestAppointID, bool IsLocked = false) 
        {
            InitializeComponent();
            TestAppointment = clsTestAppointments.FindTestAppointmentByID(TestAppointID);
            ctrlScheduleTest1.DateChanged += GetDate;
            ctrlScheduleTest1.Fill(TestAppointment.LocalDrivingLicenseID, TestAppointment.TestTypeID, TestAppointment.TestAppointmentID, IsLocked);
            int PreviousTrials = clsTestAppointments.GetNumberOfTrials(TestAppointment.LocalDrivingLicenseID, TestAppointment.TestTypeID);
            if(PreviousTrials > 0) 
            {
                gbRetakeTest.Enabled = true;
                lblRAppFees.Text = AppType.ApplicationFees.ToString();
                lblTotalFees.Text = TestAppointment.PaidFees.ToString();
              
                if(TestAppointment.RetakeTestAppointmentID != -1) 
                {
                    lblRTestAppID.Text = TestAppointment.RetakeTestAppointmentID.ToString();
                }
            }
            else 
            {
                gbRetakeTest.Enabled = false;
            }

            if (IsLocked) 
            {
                btnSave.Enabled = false;
                lblWhenLocked.Text = "Person already sat for the test, Appointment is Locked";
                label1.Text = "Schedule Retake Test";
            }
        }

        public delegate void SaveCompletedEventHandler();
        public event SaveCompletedEventHandler SaveCompleted;
        protected virtual void OnSaveCompleted() 
        {
            SaveCompleted?.Invoke();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(clsTestAppointments.Mode == clsTestAppointments.enMode.AddNew) 
            {
                if(NumberOfPreviousTrials > 0) 
                {
                    int PersonID = clsApplications.GetApplicantPersonIDUsingLocalDLAID(LocalDLID);
                    clsApplications NewApplication = new clsApplications();

                    NewApplication.ApplicantPersonID = PersonID;
                    NewApplication.ApplicationDate = DateTime.Now;
                    NewApplication.ApplicationTypeID = 7;
                    NewApplication.ApplicationStatus = 1;
                    NewApplication.LastStatusDate = DateTime.Now;
                    NewApplication.PaidFees = AppType.ApplicationFees;
                    NewApplication.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

                    if (NewApplication.SaveApplication()) 
                    {
                        RetakeID = NewApplication.ApplicationID;
                    }
                }

                TestAppointment.TestTypeID = TestTypeId;
                TestAppointment.LocalDrivingLicenseID = LocalDLID;

                TestAppointment.AppointmentDate = AppDate;
                TestAppointment.PaidFees = Fees + RetakeFees;

                TestAppointment.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;
                TestAppointment.IsLocked = false;

                TestAppointment.RetakeTestAppointmentID = RetakeID;

            }
            else if (clsTestAppointments.Mode == clsTestAppointments.enMode.Update) 
            {

                TestAppointment.AppointmentDate = AppDate;
                TestAppointment.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

            }

            if (TestAppointment.Save()) 
            {
                MessageBox.Show("Appointment Saved Successfully with ID " + TestAppointment.TestAppointmentID);
                if(TestAppointment.RetakeTestAppointmentID != -1) 
                {
                    lblRTestAppID.Text = TestAppointment.RetakeTestAppointmentID.ToString();
                }
                OnSaveCompleted();
            }
            else 
            {
                MessageBox.Show("Operation Failed");
            }

        }

        public void GetDate(DateTime AppD) 
        {
            AppDate = AppD;
        }
        public void GetFees(decimal fees) 
        {
            Fees = fees;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
