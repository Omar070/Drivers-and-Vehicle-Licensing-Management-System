using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project_Mine_
{
    public partial class frmUserInfo : Form
    {
        
        clsUser User = new clsUser();
        public frmUserInfo(int id)
        {
            InitializeComponent();
            FillInfo(id);
        }

        private void FillInfo(int ID) 
        {
            User = clsUser.FindUserByID(ID);
            ctrlPersonInfo1.FillPersonInfo(User.PersonID);
            ctrlLoginInfo1.FillLogInInfo(User.UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
