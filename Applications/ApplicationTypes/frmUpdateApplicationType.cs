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
    public partial class frmUpdateApplicationType : Form
    {

        clsApplicationTypes ThisApplicationType = new clsApplicationTypes();
        public frmUpdateApplicationType(int id)
        {
            InitializeComponent();
            ThisApplicationType = clsApplicationTypes.FindApplicationTypeByID(id);
        }

        private void FillDate() 
        {
            if (ThisApplicationType != null)
            {
                lblID.Text = ThisApplicationType.ApplicationID.ToString();
                txtTitle.Text = ThisApplicationType.ApplicationTitle.ToString();
                txtFees.Text = ThisApplicationType.ApplicationFees.ToString();
            }
            else 
            {
                MessageBox.Show("There is no Object");
            }
            
        }
        
        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
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
            if(txtTitle.Text != null && txtFees.Text != null) 
            {
                if(decimal.TryParse(txtFees.Text, out decimal NewFees))
                {
                    ThisApplicationType.ApplicationFees = NewFees;
                }
                else 
                {
                    MessageBox.Show("Fees Should be Numerical");
                    return;
                }

                ThisApplicationType.ApplicationTitle = txtTitle.Text;


                if (ThisApplicationType.Save()) 
                {
                    MessageBox.Show("Application Type Updated Successfully");
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
