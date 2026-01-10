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
    public partial class frmManageDerivers : Form
    {
        DataTable dt = clsDrivers.GetAllDrivers();
        public frmManageDerivers()
        {
            InitializeComponent();
            cbFilterBy.SelectedIndex = 0;
            FillDGV();
        }

        private void FillDGV() 
        {
            DataTable dt1 = clsDrivers.GetAllDrivers();
            dataGridView1.DataSource = dt1;
            lblRecords.Text = dt1.Rows.Count.ToString();
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

            lblRecords.Text = Convert.ToInt32(dataGridView1.RowCount -1).ToString();
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

        private void mtxtFilterBy_TextChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 1: // "Driver ID":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 2: // "Person ID":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 3: //"National No":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "CCCCCCC";
                    ApplyFilter();
                    break;

                case 4: // "Name":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                default:
                    mtxtFilterBy.Visible = false;
                    break;

            }
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;

            if(dataGridView1.SelectedRows.Count > 0) 
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if(ID != -1) 
            {
                frmPersonDetails frm = new frmPersonDetails(ID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            }

            if (ID != -1)
            {
                frmLicenseHistory frm = new frmLicenseHistory(ID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Function Wil Be Implemented Soon");
        }
    }

}
