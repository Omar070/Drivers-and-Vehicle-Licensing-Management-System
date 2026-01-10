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
    public partial class frmVisionAppointment : Form
    {
        int LDLID;
        int TestTypeID;
        int PreviousTrials;
        public frmVisionAppointment(int id, int testType)
        {
            InitializeComponent();
            LDLID = id;
            TestTypeID = testType;
            PreviousTrials = clsTestAppointments.GetNumberOfTrials(LDLID, TestTypeID);
            ctrlApplicationDetails1.Fill(LDLID);
            
        }

        public delegate void JobDoneEventHandler();
        public event JobDoneEventHandler JobDone;
        protected virtual void OnJobDone() 
        {
            JobDone?.Invoke();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnJobDone();
            this.Close();
        }


        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            int TestAppointID = -1;
            if (dataGridView1.RowCount > 0) 
            {
                TestAppointID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0].Value);
                bool PassedLastTime = clsTests.IsPassed(TestAppointID);
                if (PassedLastTime)
                {
                    MessageBox.Show("This Person Already Passed This Test Before, You Can Retake Only Failed Tests");
                    return;
                }
            }

            if (clsTestAppointments.IsPreviousAppointmentExist(LDLID, TestTypeID)) 
            {
                MessageBox.Show("This Person Already Has an Active Appointment For This Test, You Can't Add a New Appointment");
            }
            else 
            {
                PreviousTrials = clsTestAppointments.GetNumberOfTrials(LDLID, TestTypeID);
                frmScheduleTest frm = new frmScheduleTest(LDLID, TestTypeID, PreviousTrials);
                frm.SaveCompleted += Fill;
                frm.ShowDialog();
            }

        }
         private void Fill() 
        {
            DataTable dt = clsTestAppointments.GetAppointmentsForLDLID(LDLID, TestTypeID);
            dataGridView1.DataSource = dt;
            lblRecords.Text = dt.Rows.Count.ToString();
        }
        private void frmVisionAppointment_Load(object sender, EventArgs e)
        {
            switch (TestTypeID) 
            {
                case 1:
                    lblTestName.Text = "Vision Test Appointment";
                    break;
                case 2:
                    lblTestName.Text = "Written Test Appointment";
                    break;
                case 3:
                    lblTestName.Text = "Practical Test Appointment";
                    break;
                default:
                    lblTestName.Text = "Vision Test Appointment";
                    break;
            }

            Fill();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            bool IsLocked = false;

            if (dataGridView1.SelectedRows.Count > 0) 
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                IsLocked = Convert.ToBoolean(dataGridView1.SelectedRows[0].Cells[3].Value);

            }

            if (ID != -1) 
            {
                frmScheduleTest frm = new frmScheduleTest(ID, IsLocked);
                frm.SaveCompleted += Fill;
                frm.ShowDialog();
            }
           
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            bool IsLocked = false;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                IsLocked = Convert.ToBoolean(dataGridView1.SelectedRows[0].Cells[3].Value);
            }

            if (IsLocked == false) 
            {
                if (ID != -1)
                {
                    frmTakeTest frm = new frmTakeTest(ID);
                    frm.SaveCompleted += Fill;
                    frm.ShowDialog();
                }
            }
            else 
            {
                MessageBox.Show("This Test Is Locked");
            }
            
        }
    }
}
