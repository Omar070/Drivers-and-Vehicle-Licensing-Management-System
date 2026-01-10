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
    public partial class frmUpdateTestType : Form
    {
        clsTestTypes ThisTestType = new clsTestTypes();
        public frmUpdateTestType(int id)
        {
            InitializeComponent();
            ThisTestType = clsTestTypes.FindTestTypeByID(id);
        }

        private void FillDate()
        {
            if (ThisTestType != null)
            {
                lblID.Text = ThisTestType.TestTypeID.ToString();
                txtTitle.Text = ThisTestType.TestTypeTitle.ToString();
                txtDescription.Text = ThisTestType.TestTypeDescription.ToString();
                txtFees.Text = ThisTestType.TestTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("There is no Object");
            }

        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            FillDate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public delegate void SaveCompletedEventHandler(object sender, EventArgs e);
        public event SaveCompletedEventHandler SaveCompleted;
        protected virtual void OnSaveCompleted(EventArgs e)
        {
            SaveCompleted?.Invoke(this, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text != null && txtDescription != null && txtFees.Text != null)
            {

                if (decimal.TryParse(txtFees.Text, out decimal NewFees))
                {
                    ThisTestType.TestTypeFees = NewFees;
                }
                else
                {
                    MessageBox.Show("Fees Should be Numerical");
                    return;
                }

                ThisTestType.TestTypeTitle = txtTitle.Text;
                ThisTestType.TestTypeDescription = txtDescription.Text;

                if (ThisTestType.Save())
                {
                    MessageBox.Show("Test Type Updated Successfully");
                    OnSaveCompleted(EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Update Failed");
                }

            }
            else
            {
                MessageBox.Show("Missed Data, The operation cannot be completed");
            }
        }


    }
}
