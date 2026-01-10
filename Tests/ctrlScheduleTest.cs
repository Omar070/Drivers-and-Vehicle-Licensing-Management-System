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
    public partial class ctrlScheduleTest : UserControl
    {
        clsLocalDrivingLicenseApplicationInfo LDLInfo;
        clsTestTypes TestTypesInfo;
        clsTestAppointments TestAppoint;
        int TrialsNo;
        public ctrlScheduleTest()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = DateTime.Now.AddHours(-10);
            dateTimePicker1.Format = DateTimePickerFormat.Short;
        }

        public delegate void UserControlOpenedEventHandler(decimal PaidFees);
        public event UserControlOpenedEventHandler UserControlOpened;
        protected virtual void OnUserControlOpened(decimal PaidFees)
        {
            UserControlOpened?.Invoke(PaidFees);
        }

        public delegate void DateChangedEventHandler(DateTime AppointmentDate);
        public event DateChangedEventHandler DateChanged;
        protected virtual void OnDateChange(DateTime AppointmentDate)
        {
            DateChanged?.Invoke(AppointmentDate);
        }
        public void Fill(int LDLID, int TestID, int TESTAppID = -1, bool IsLocked = false) 
        {
            LDLInfo = clsLocalDrivingLicenseApplicationInfo.GetLocalDrivingLicenseApplicationInfoByLDLAPPID(LDLID);
            TestTypesInfo = clsTestTypes.FindTestTypeByID(TestID);
            TrialsNo = clsTestAppointments.GetNumberOfTrials(LDLInfo.LocalDrivingLicenseApplicationID, TestTypesInfo.TestTypeID);

            lblDLAppID.Text = LDLInfo.LocalDrivingLicenseApplicationID.ToString();
            lblDClass.Text = LDLInfo.Class.ToString();
            lblName.Text = LDLInfo.ApplicantName.ToString();
            lblTrial.Text = TrialsNo.ToString();
            lblFees.Text = TestTypesInfo.TestTypeFees.ToString();

            OnUserControlOpened(TestTypesInfo.TestTypeFees);

            if(TESTAppID != -1) 
            {
                TestAppoint = clsTestAppointments.FindTestAppointmentByID(TESTAppID);
                dateTimePicker1.Value = TestAppoint.AppointmentDate;
            }

            if (IsLocked) 
            {
                dateTimePicker1.Enabled = false;
            }
        }

 

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            OnDateChange(dateTimePicker1.Value);
        }
    }
}
