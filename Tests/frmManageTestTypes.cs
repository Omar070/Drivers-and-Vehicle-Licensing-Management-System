using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DVLDBusinessLayer;


namespace DVLD_Project_Mine_
{
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void FillTheDataGridView(object sender, EventArgs e)
        {
            DataTable dt = clsTestTypes.GetAllTestTypes();
            dataGridView1.DataSource = dt;
            lblRecords.Text = dt.Rows.Count.ToString();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            FillTheDataGridView(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmUpdateTestType frm = new frmUpdateTestType(ID);
                frm.SaveCompleted += FillTheDataGridView;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }
    }
}
