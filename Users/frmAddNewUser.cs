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
    public partial class frmAddNewUser : Form
    {
        // This Constructors is used to add a new user
        public frmAddNewUser()
        {
            InitializeComponent();
            ctrlUserInfoWithFilter1.SearchCompletedSend += GetPersonID;
        }

        private int PersonID = -1;
        public void GetPersonID(int ID)
        {
            PersonID = ID;
        }
        // until here

        // this area related to update existing user 
        clsUser User = new clsUser();
        public frmAddNewUser(int UserID)
        {
            InitializeComponent();
            User = clsUser.FindUserByID(UserID);
            clsUser.Mode = clsUser.enMode.Update;
        }
        // Constructors Done


        /* this function is made to ensure that there is person then to ensure that this
         Person does not have a user liked to him if we adding a new user
         then to move forward to the next page if every thing is ok */
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(PersonID != -1) 
            {
                if (clsUser.Mode == clsUser.enMode.AddNew) 
                {
                    if (clsUser.IsUserExistUsingPersonID(PersonID))
                    {
                        MessageBox.Show("Selected Person already has a user, choose another one");
                    }
                    else
                    {
                        tabControl1.SelectedIndex = 1;
                    }
                }
                else
                {
                    tabControl1.SelectedIndex = 1;
                }
            }
            else 
            {
                MessageBox.Show("Please Select a person");
            }

        }
        // Done 

        // close button
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // Done

        // Area of Validation 
        private void CheckValidation(TextBox T, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(T.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(T, "This Field Is Mandatory");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(T, "");
            }
        }

        private void Required(Object T, CancelEventArgs e)
        {
            CheckValidation((TextBox)T, e);
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match password");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }

        }
        // validation work done


        // Event that is fired up on save completion 
        public delegate void SaveCompletedEventHandler(object sender, EventArgs e);
        public event SaveCompletedEventHandler SaveCompleted;
        protected virtual void OnSaveCompleted(EventArgs e)
        {
            SaveCompleted?.Invoke(this, e);
        }
        // Done

        // The Save Function (the most complicated function)
        private void btnSave_Click(object sender, EventArgs e)
        {
       
            if(PersonID != -1 && txtUserName.Text != "" && txtPassword.Text != "" 
                && txtConfirmPassword.Text != "" && 
                txtConfirmPassword.Text == txtPassword.Text) 
            {
                if (clsUser.Mode == clsUser.enMode.AddNew) 
                {
                    if (clsUser.IsUserExistUsingPersonID(PersonID))
                    {
                        MessageBox.Show("Selected Person already has a user, choose another one");
                        return;
                    }
                    else 
                    {
                        User.PersonID = PersonID;
                    }
                }
                

                User.UserName = txtUserName.Text;
                
                User.Password = CurrentUserManager.ComputeHash(txtPassword.Text);

                if (cbIsActive.Checked) 
                {
                    User.IsActive = 1;
                }
                else
                {
                    User.IsActive = 0;
                }


                if (User.Save()) 
                {
                    MessageBox.Show("User Saved Successfully");
                    lblUserID.Text = User.UserID.ToString();
                    OnSaveCompleted(EventArgs.Empty);
                }
                else 
                {
                    MessageBox.Show("Failed Operation, Please try again");
                }


            }
            else 
            {
                MessageBox.Show("Missed Data, Please Complete the Required Data");
            }


        }
        // Saving logic Done

        // Come Back Here
        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            if (clsUser.Mode == clsUser.enMode.AddNew) 
            {
                lblTitle.Text = "Add New User";
            }
            else if(clsUser.Mode == clsUser.enMode.Update) 
            {
                lblTitle.Text = "Update User";
                if(User != null) 
                {
                    ctrlUserInfoWithFilter1.ShowUserDetails(User.PersonID);
                    PersonID = User.PersonID;
                    lblUserID.Text = User.UserID.ToString();
                    txtUserName.Text = User.UserName;
                    txtPassword.Text = User.Password;
                    txtConfirmPassword.Text = User.Password;
                    if(User.IsActive == 1) 
                    {
                        cbIsActive.Checked = true;
                    }
                    else if(User.IsActive == 0)
                    {
                        cbIsActive.Checked = false;
                    }
                }
                else 
                {
                    MessageBox.Show("Error Happened, No User To Show");
                }
            }
        
        }
        // May Need Some Modifications










    }

}
