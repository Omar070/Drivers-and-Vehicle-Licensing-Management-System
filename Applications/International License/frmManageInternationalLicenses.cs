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
    public partial class frmManageInternationalLicenses : Form
    {
        DataTable dt = clsInternationalLicenses.GetAllInternationalLicense();
        public frmManageInternationalLicenses()
        {
            InitializeComponent();
            FillDGV();
        }

        private void FillDGV() 
        {
            DataTable dt1 = clsInternationalLicenses.GetAllInternationalLicense();
            dataGridView1.DataSource = dt1;
            lblRecords.Text = dt1.Rows.Count.ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnAddNewInternationalLicense_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseApplication frm = new frmInternationalLicenseApplication();
            frm.IssueCompleted += FillDGV;
            frm.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppID = -1;
            int PersonID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                AppID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if (AppID != -1)
            {
                PersonID = clsApplications.GetApplicantPersonID(AppID);

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
            int IIntLicenseID = -1;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                IIntLicenseID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if(IIntLicenseID != -1) 
            {
                frmInternationalDeriverLicenseInfo frm = new frmInternationalDeriverLicenseInfo(IIntLicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void showPersonLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppID = -1;
            int PersonID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                AppID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if (AppID != -1)
            {
                PersonID = clsApplications.GetApplicantPersonID(AppID);

                frmLicenseHistory frm = new frmLicenseHistory(PersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                mtxtFilterBy.Visible = false;
                FillDGV();
            }
            else
            {
                mtxtFilterBy.Visible = true;
                FillDGV();
            }
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

        private void mtxtFilterBy_TextChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 1:
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 2:
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 3:
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 4:
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                default:
                    mtxtFilterBy.Visible = false;
                    break;

            }
        }


    }
}
