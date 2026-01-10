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
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();

            // NB: the Active/InActive filter was not working before adding this subscription
            cbFilterBy.SelectedIndexChanged += new EventHandler(cbFilterBy_SelectedIndexChanged);
            cbIsActive.SelectedIndexChanged += new EventHandler(cbIsActive_SelectedIndexChanged);
         
        }

        DataTable dt = clsUser.GetAllUsers();

        public void RefreshTheDGV(object sender, EventArgs e)
        {
            DataTable dt1 = clsUser.GetAllUsers();
            dgvUsers.DataSource = dt1;
            ConvertLastColumnToCheckBox();
            dgvUsers.Refresh();
            lblNumberOfRecords.Text = (Convert.ToInt32(dgvUsers.RowCount - 1)).ToString();
        }
        
        // ChatGPT
        private void ConvertLastColumnToCheckBox()
        {
            // Convert the last column to a CheckBox column
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "IsActive";
            checkBoxColumn.DataPropertyName = "IsActive";
            checkBoxColumn.Name = "IsActive";
            checkBoxColumn.ReadOnly = true;

            int lastColumnIndex = dgvUsers.Columns.Count - 1;
            dgvUsers.Columns.RemoveAt(lastColumnIndex);
            dgvUsers.Columns.Insert(lastColumnIndex, checkBoxColumn);
        }
        // up until here

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            RefreshTheDGV(sender, e);
            cbFilterBy.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser();
            frm.SaveCompleted += RefreshTheDGV;
            frm.ShowDialog();
        }

        // Filters Part
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                mtxtFilterBy.Visible = false;
                cbIsActive.Visible = false;
                RefreshTheDGV(sender, e);
            }
            else if(cbFilterBy.SelectedIndex == 5) 
            {
                mtxtFilterBy.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
                RefreshTheDGV(sender, e);
            }
            else
            {
                mtxtFilterBy.Visible = true;
                cbIsActive.Visible = false;
                RefreshTheDGV(sender, e);
            }
        }

        private void mtxtFilterBy_TextChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 1: // "UserID":
                    mtxtFilterBy.Visible = true;
                    cbIsActive.Visible = false;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 2: // "PersonID":
                    mtxtFilterBy.Visible = true;
                    cbIsActive.Visible = false;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 3: // "Full Name":
                    mtxtFilterBy.Visible = true;
                    cbIsActive.Visible = false;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 4: // "User Name":
                    mtxtFilterBy.Visible = true;
                    cbIsActive.Visible = false;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 5: // "IsActive":
                    cbIsActive.Visible = true;
                    cbIsActive.SelectedIndex = 0;
                    mtxtFilterBy.Visible = false;
                    ApplyActiveFilter();
                    break;

                default:
                    mtxtFilterBy.Visible = false;
                    cbIsActive.Visible = false;
                    break;

            }
        }

        private void ApplyFilter()
        {
            string filterText = mtxtFilterBy.Text;
            string selectedColumn = cbFilterBy.SelectedItem.ToString();
            DataColumn column = dt.Columns[selectedColumn];

            if (column.DataType == typeof(string))
            {
                (dgvUsers.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("{0} LIKE '%{1}%'", selectedColumn, filterText);
            }
            else if (column.DataType == typeof(int))
            {
                if (int.TryParse(filterText, out int filterInt))
                {
                    (dgvUsers.DataSource as DataTable).DefaultView.RowFilter =
                        string.Format("{0} = {1}", selectedColumn, filterInt);
                }
                else
                {
                    (dgvUsers.DataSource as DataTable).DefaultView.RowFilter = "1=0"; // No match
                }
            }
    

            lblNumberOfRecords.Text = (Convert.ToInt32(dgvUsers.RowCount - 1)).ToString();

        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyActiveFilter();
        }

        private void ApplyActiveFilter()
        {
            string filterExpression = "";

            switch (cbIsActive.SelectedIndex)
            {
                case 1: // Active
                    filterExpression = "IsActive = true";
                    break;
                case 2: // Inactive
                    filterExpression = "IsActive = false";
                    break;
                default: // All
                    break;
            }

            // Apply the filter to the DataView
            if (!string.IsNullOrEmpty(filterExpression))
            {
                (dgvUsers.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            else
            {
                (dgvUsers.DataSource as DataTable).DefaultView.RowFilter = "";
            }

            lblNumberOfRecords.Text = (Convert.ToInt32(dgvUsers.RowCount - 1)).ToString();

        }

        // Filters Done


        // Context Menu Strip 
        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Will Be Available Soon");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Will Be Available Soon");
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser();
            frm.SaveCompleted += RefreshTheDGV;
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvUsers.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmUserInfo frm = new frmUserInfo(ID);
                frm.ShowDialog();
                RefreshTheDGV(sender, e);
            }
            else
            {
                MessageBox.Show("Error, No Selected Person!!");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvUsers.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                clsUser User = clsUser.FindUserByID(ID);

                if (clsUser.DeleteUser(User.UserID))
                {
                    MessageBox.Show("User Deleted Successfully");
                }
                else
                {
                    MessageBox.Show("User was not deleted because there is data liked to him");
                }
                RefreshTheDGV(sender, e);
            }
            else
            {
                MessageBox.Show("Error, No Selected Person!!");
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvUsers.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmAddNewUser frm = new frmAddNewUser(ID);
                frm.SaveCompleted += RefreshTheDGV;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvUsers.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmChangePassword frm = new frmChangePassword(ID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
            
        }

        // Context Menu Strip Done




    }

}
