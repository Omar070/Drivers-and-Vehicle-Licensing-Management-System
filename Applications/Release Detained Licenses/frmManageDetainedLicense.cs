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
    public partial class frmManageDetainedLicense : Form
    {
        DataTable dt = clsDetainedLicense.GetAllDetainedLicenses();
        public frmManageDetainedLicense()
        {
            InitializeComponent();
            FillDGV();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ReleaseDone += FillDGV;
            frm.ShowDialog();
        }

        private void btnDetaine_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.DetainDone += FillDGV;
            frm.ShowDialog();
        }

        private void FillDGV() 
        {
            DataTable dt1 = clsDetainedLicense.GetAllDetainedLicenses();
            dataGridView1.DataSource = dt1;
            lblRecords.Text = dt1.Rows.Count.ToString();
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
           

            lblRecords.Text = (Convert.ToInt32(dataGridView1.RowCount - 1)).ToString();

        }

        private void ApplyReleaseFilter()
        {
            string filterExpression = "";

            switch (cbIsReleased.SelectedIndex)
            {
                case 1: // Active
                    filterExpression = "IsReleased = true";
                    break;
                case 2: // Inactive
                    filterExpression = "IsReleased = false";
                    break;
                default: // All
                    break;
            }

            // Apply the filter to the DataView
            if (!string.IsNullOrEmpty(filterExpression))
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "";
            }

            lblRecords.Text = (Convert.ToInt32(dataGridView1.RowCount - 1)).ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                mtxtFilterBy.Visible = false;
                cbIsReleased.Visible = false;
                FillDGV();
            }
            else if (cbFilterBy.SelectedIndex == 2)
            {
                mtxtFilterBy.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.SelectedIndex = 0;
                FillDGV();
            }
            else
            {
                mtxtFilterBy.Visible = true;
                cbIsReleased.Visible = false;
                FillDGV();
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyReleaseFilter();
        }

        private void mtxtFilterBy_TextChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 1: // "DetainID":
                    mtxtFilterBy.Visible = true;
                    cbIsReleased.Visible = false;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 2: // "IsReleased":
                    cbIsReleased.Visible = true;
                    cbIsReleased.SelectedIndex = 0;
                    mtxtFilterBy.Visible = false;
                    ApplyReleaseFilter();
                    break;

                case 3: // "NationalNO":
                    mtxtFilterBy.Visible = true;
                    cbIsReleased.Visible = false;
                    mtxtFilterBy.Mask = "CCCCCCCCCC";
                    ApplyFilter();
                    break;

                case 4: // "Full Name":
                    mtxtFilterBy.Visible = true;
                    cbIsReleased.Visible = false;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 5: // "ReleaseAppID":
                    mtxtFilterBy.Visible = true;
                    cbIsReleased.Visible = false;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                default:
                    mtxtFilterBy.Visible = false;
                    cbIsReleased.Visible = false;
                    break;

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0) 
            {
                if (Convert.ToBoolean(dataGridView1.SelectedRows[0].Cells[3].Value) == true) 
                {
                    contextMenuStrip1.Items[3].Enabled = false;   
                }
                else 
                {
                    contextMenuStrip1.Items[3].Enabled = true;
                }
            }
            
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            int PersonID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                LicenseID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if (LicenseID != -1)
            {
                PersonID = clsLicense.GetApplicantPersonIDByLicenseID(LicenseID);

                frmPersonDetails frm = new frmPersonDetails(PersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                LicenseID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if (LicenseID != -1)
            {
                frmDeriverLicenseInfo frm = new frmDeriverLicenseInfo(0, LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            int PersonID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                LicenseID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if (LicenseID != -1)
            {
                PersonID = clsLicense.GetApplicantPersonIDByLicenseID(LicenseID);

                frmLicenseHistory frm = new frmLicenseHistory(PersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                LicenseID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if (LicenseID != -1)
            {
                frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(LicenseID);
                frm.ReleaseDone += FillDGV;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }
    }

}
