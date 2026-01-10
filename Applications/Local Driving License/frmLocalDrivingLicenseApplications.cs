using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace DVLD_Project_Mine_
{
    public partial class frmLocalDrivingLicenseApplications : Form
    {
        public frmLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }


        DataTable dt = clsApplications.GetAllLocalDrivingLicenseApps();
        private void FillTheDGV()
        {
            DataTable dt1 = clsApplications.GetAllLocalDrivingLicenseApps();
            dataGridView1.DataSource = dt1;
            lblRecords.Text = dt1.Rows.Count.ToString();

        }

        private void frmLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            FillTheDGV();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyFilter()
        {
            string filterText = mtxtFilterBy.Text;
            string selectedColumn = cbFilterBy.SelectedItem.ToString();
            DataColumn column = dt.Columns[selectedColumn];

            if (column.DataType == typeof(string))
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("{0} LIKE '%{1}%'", selectedColumn, filterText);
            }
            else if (column.DataType == typeof(int))
            {
                if (int.TryParse(filterText, out int filterInt))
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
                        string.Format("{0} = {1}", selectedColumn, filterInt);
                }
                else
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "1=0"; // No match
                }
            }
            else if (column.DataType == typeof(DateTime))
            {
                if (DateTime.TryParse(filterText, out DateTime filterDate))
                {
                    // this modification is made to compare date parts without time time part
                    string filterExpression = string.Format("{0} >= #{1}# AND {0} < #{2}#",
                    selectedColumn,
                    filterDate.ToString("MM/dd/yyyy"),
                    filterDate.AddDays(1).ToString("MM/dd/yyyy"));

                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filterExpression;

                }
                else
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "1=0"; // No match
                }
            }

            lblRecords.Text = Convert.ToString(dataGridView1.Rows.Count - 1);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                mtxtFilterBy.Visible = false;
                FillTheDGV();
            }
            else
            {
                mtxtFilterBy.Visible = true;
                FillTheDGV();
            }
        }

        private void mtxtFilterBy_TextChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 0:
                    mtxtFilterBy.Visible = false;
                    FillTheDGV();
                    break;
                case 1: // "L.D.L.AppID":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;
                case 2: // "National No":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "CCCCCCC";
                    ApplyFilter();
                    break;
                case 3: //"Full Name":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;
                case 4: // "Status":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;
                default:
                    mtxtFilterBy.Visible = false;
                    break;

            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApplication frm = new frmNewLocalDrivingLicenseApplication();
            frm.SaveCompletedSend += FillTheDGV;
            frm.ShowDialog();
        }


        // All The Context Menu Strip Items  

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo(ID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
   
            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1) 
            {
                if (clsApplications.CancelLocalDrivingLicenseApplication(ID)) 
                {
                    FillTheDGV();
                    MessageBox.Show("Application Cancelled Successfully");
                }
                else 
                {
                    MessageBox.Show("Operation Failed");
                }
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void OpenTestsForms(int TestType) 
        {
            int ID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmVisionAppointment frm = new frmVisionAppointment(ID, TestType);
                frm.JobDone += FillTheDGV;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Person Selected");
            }
        }
        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTestsForms(1);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTestsForms(2);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTestsForms(3);
        }

        // To Manage Enabled vs Disabled Items of the Context Menu Strip 
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            ToolStripMenuItem Tests = (ToolStripMenuItem)contextMenuStrip1.Items[4];

            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (Convert.ToString(dataGridView1.SelectedRows[0].Cells[6].Value) == "New") 
                {
                    if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[5].Value) == 0)
                    {
                        contextMenuStrip1.Items[1].Enabled = true;
                        contextMenuStrip1.Items[2].Enabled = true;
                        contextMenuStrip1.Items[3].Enabled = true;
                        contextMenuStrip1.Items[4].Enabled = true;

                        contextMenuStrip1.Items[7].Enabled = true;

                        contextMenuStrip1.Items[5].Enabled = false;
                        contextMenuStrip1.Items[6].Enabled = false;

                        Tests.DropDownItems[1].Enabled = false;
                        Tests.DropDownItems[2].Enabled = false;

                        Tests.DropDownItems[0].Enabled = true;
                    }
                    else if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[5].Value) == 1)
                    {
                        contextMenuStrip1.Items[1].Enabled = true;
                        contextMenuStrip1.Items[2].Enabled = true;
                        contextMenuStrip1.Items[3].Enabled = true;
                        contextMenuStrip1.Items[4].Enabled = true;

                        contextMenuStrip1.Items[7].Enabled = true;

                        contextMenuStrip1.Items[5].Enabled = false;
                        contextMenuStrip1.Items[6].Enabled = false;

                        Tests.DropDownItems[0].Enabled = false;
                        Tests.DropDownItems[2].Enabled = false;

                        Tests.DropDownItems[1].Enabled = true;
                    }
                    else if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[5].Value) == 2)
                    {
                        contextMenuStrip1.Items[1].Enabled = true;
                        contextMenuStrip1.Items[2].Enabled = true;
                        contextMenuStrip1.Items[3].Enabled = true;
                        contextMenuStrip1.Items[4].Enabled = true;

                        contextMenuStrip1.Items[7].Enabled = true;

                        contextMenuStrip1.Items[5].Enabled = false;
                        contextMenuStrip1.Items[6].Enabled = false;

                        Tests.DropDownItems[0].Enabled = false;
                        Tests.DropDownItems[1].Enabled = false;

                        Tests.DropDownItems[2].Enabled = true;
                    }
                    else if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[5].Value) == 3)
                    {
                        contextMenuStrip1.Items[1].Enabled = true;
                        contextMenuStrip1.Items[2].Enabled = true;
                        contextMenuStrip1.Items[3].Enabled = true;

                        contextMenuStrip1.Items[5].Enabled = true;
                        contextMenuStrip1.Items[7].Enabled = true;

                        contextMenuStrip1.Items[4].Enabled = false;
                        contextMenuStrip1.Items[6].Enabled = false;
                    }
                }
                else if(Convert.ToString(dataGridView1.SelectedRows[0].Cells[6].Value) == "Cancelled") 
                {
                    // May need some modifications 

                    contextMenuStrip1.Items[0].Enabled = true;
                    contextMenuStrip1.Items[2].Enabled = true;
                    contextMenuStrip1.Items[7].Enabled = true;

                    contextMenuStrip1.Items[1].Enabled = false;
                    contextMenuStrip1.Items[3].Enabled = false;
                    contextMenuStrip1.Items[4].Enabled = false;
                    contextMenuStrip1.Items[5].Enabled = false;
                    contextMenuStrip1.Items[6].Enabled = false;
                }
                else if(Convert.ToString(dataGridView1.SelectedRows[0].Cells[6].Value) == "Completed") 
                {
                    contextMenuStrip1.Items[0].Enabled = true;

                    contextMenuStrip1.Items[1].Enabled = false;
                    contextMenuStrip1.Items[2].Enabled = false;
                    contextMenuStrip1.Items[3].Enabled = false;
                    contextMenuStrip1.Items[4].Enabled = false;
                    contextMenuStrip1.Items[5].Enabled = false;

                    contextMenuStrip1.Items[6].Enabled = true;
                    contextMenuStrip1.Items[7].Enabled = true;
                    
                }
              
            }
            
        }

        private void issuingDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmIssueDeriverLicenseForTheFirstTime frm = new frmIssueDeriverLicenseForTheFirstTime(ID);
                frm.SaveCompleted += FillTheDGV;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Person Selected");
            }
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmDeriverLicenseInfo frm = new frmDeriverLicenseInfo(ID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Person Selected");
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                clsLocalDrivingLicenseApplicationInfo LocalApp = clsLocalDrivingLicenseApplicationInfo.GetLocalDrivingLicenseApplicationInfoByLDLAPPID(ID);

                frmLicenseHistory frm = new frmLicenseHistory(LocalApp.ApplicantPersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Person Selected");
            }
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                clsLocalDrivingLicenseApplicationInfo LocalApp = clsLocalDrivingLicenseApplicationInfo.GetLocalDrivingLicenseApplicationInfoByLDLAPPID(ID);

                if(clsLocalDrivingLicenseApplicationInfo.DeleteLocalDrivingLicenseApplication(LocalApp.LocalDrivingLicenseApplicationID, LocalApp.ApplicationID)) 
                {
                    FillTheDGV();
                    MessageBox.Show("Application Deleted Successfully");
                }
                else { MessageBox.Show("Operation Failed"); }
            }
            else
            {
                MessageBox.Show("No Person Selected");
            }
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Function Will Be Implemented Soon");
        }
    }
}
