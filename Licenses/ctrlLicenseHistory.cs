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
    public partial class ctrlLicenseHistory : UserControl
    {
        public ctrlLicenseHistory()
        {
            InitializeComponent();
        }

        public void Fill(int PersonID) 
        {
            DataTable dtLocal = clsLocalDrivingLicenseApplicationInfo.GetLocalDrivingLicensesHistoryUsingPersonID(PersonID);
            dgvLocal.DataSource = dtLocal;
            lblLocalRecords.Text = dtLocal.Rows.Count.ToString();

            DataTable dtInternational = clsInternationalLicenses.GetInternationalLicensesHistoryUsingPersonID(PersonID);
            dgvInternational.DataSource = dtInternational;
            lblInternationalRecords.Text = dtInternational.Rows.Count.ToString();
        }

        
        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvLocal.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvLocal.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmDeriverLicenseInfo frm = new frmDeriverLicenseInfo(0, ID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void showLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int IIntLicenseID = -1;
            if (dgvInternational.SelectedRows.Count > 0)
            {
                IIntLicenseID = Convert.ToInt32(dgvInternational.SelectedRows[0].Cells[0].Value);
            }

            if (IIntLicenseID != -1)
            {
                frmInternationalDeriverLicenseInfo frm = new frmInternationalDeriverLicenseInfo(IIntLicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }
    }
}
