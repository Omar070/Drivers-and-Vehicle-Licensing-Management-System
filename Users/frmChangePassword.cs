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
    public partial class frmChangePassword : Form
    {
        clsUser User = new clsUser();
        public frmChangePassword(int id)
        {
            InitializeComponent();
            User = clsUser.FindUserByID(id);
            clsUser.Mode = clsUser.enMode.Update;
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            FillData();
        }
        private void FillData() 
        {
            if (User != null)
            {
                ctrlPersonInfo1.FillPersonInfo(User.PersonID);
                ctrlLoginInfo1.FillLogInInfo(User.UserID);
            }
            else 
            {
                MessageBox.Show("Error, Can't Find The User");
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(User != null && txtCurrentPassword.Text != "" 
                && txtNewPassword.Text != "" && txtConfirmPassword.Text != "" 
                && txtConfirmPassword.Text == txtNewPassword.Text) 
            {
                User.Password = CurrentUserManager.ComputeHash(txtNewPassword.Text);
                if (User.Save()) 
                {
                    MessageBox.Show("Password Changed Successfully");
                }
                else 
                {
                    MessageBox.Show("Error");
                }
            }
            else 
            {
                MessageBox.Show("Missed Data, Please Complete the Required Data");
            }

        }


        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "This Field Is Mandatory");
            }
            else if ( CurrentUserManager.ComputeHash(txtCurrentPassword.Text) != User.Password) 
            {
                errorProvider1.SetError(txtCurrentPassword, "Password Must Match User Password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "This Field Is Mandatory");
            }
            else if (txtConfirmPassword.Text != txtNewPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Must Match The New Password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "This Field Is Mandatory");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, "");
            }
        }

    }
}
