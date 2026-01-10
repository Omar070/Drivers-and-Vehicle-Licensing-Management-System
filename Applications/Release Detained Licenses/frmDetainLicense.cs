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
    public partial class frmDetainLicense : Form
    {
        int LicenseID = -1, PersonID = -1;
        decimal fineFees = -1;
        public frmDetainLicense()
        {
            InitializeComponent();
            ctrlLIcenseInfoWithFilter1.SearchCompleted += GetLicenseID;
        }

        private void GetLicenseID(int ID) 
        {
            if (!clsLicense.IsLicenseExist(ID)) 
            {
                btnDetain.Enabled = false;
                return;
            }

            LicenseID = ID;
            PersonID = clsLicense.GetApplicantPersonIDByLicenseID(LicenseID);

            ctrlDetainInfo1.Fill(LicenseID);

            btnDetain.Enabled = true;

            if (clsDetainedLicense.IsLicenseDetained(LicenseID)) 
            {
                btnDetain.Enabled = false;
                MessageBox.Show("Selected License Is Already Detained, Choose another one");
            }

            if (!clsLicense.IsLicenseActive(LicenseID)) 
            {
                btnDetain.Enabled = false;
                MessageBox.Show("Selected License Is Deactivated, Can't Perform Operations on it");
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (PersonID != -1)
            {
                frmLicenseHistory frm = new frmLicenseHistory(PersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Selected Person");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LicenseID != -1)
            {
                frmDeriverLicenseInfo frm = new frmDeriverLicenseInfo(0, LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Selected Person");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public delegate void DetainDoneEventHandler();
        public event DetainDoneEventHandler DetainDone;
        protected virtual void OnDetainDone() 
        {
            DetainDone?.Invoke();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            clsDetainedLicense DLic = new clsDetainedLicense();

            if (LicenseID == -1) 
            {
                MessageBox.Show("No Selected License");
                return;
            }

            ctrlDetainInfo1.SendFineFees(ref fineFees);

            if(fineFees != -1) 
            {
                DLic.FineFees = fineFees;
            }
            else 
            {
                MessageBox.Show("Invalid Fine Fees");
                return;
            }

            DLic.LicenseID = LicenseID;
            DLic.DetainDate = DateTime.Now;

           
            DLic.CreatedByUserID = CurrentUserManager.CurrentUser.UserID;

            if (DLic.Save()) 
            {
                MessageBox.Show("License Detained Successfully With ID : " + DLic.DetainID);
                
                ctrlDetainInfo1.GetDetainID(DLic.DetainID);

                btnDetain.Enabled = false;
                linkLabel2.Enabled = true;
                OnDetainDone();
            }

        }

        


    }
}
