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
    public partial class ctrlLoginInfo : UserControl
    {
        public ctrlLoginInfo()
        {
            InitializeComponent();
        }

        clsUser User = new clsUser();
        
        public void FillLogInInfo(int UserID) 
        {
            User = clsUser.FindUserByID(UserID);
            if (User != null)
            {
                lblUserName.Text = User.UserName;
                lblUserID.Text = User.UserID.ToString();
                
                if(User.IsActive == 1) 
                {
                    lblIsActive.Text = "Active";
                }
                else 
                {
                    lblIsActive.Text = "InActive";
                }
            }
            
        }
    }
}
