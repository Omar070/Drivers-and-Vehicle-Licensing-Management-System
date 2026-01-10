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
    public partial class frmDeriverLicenseInfo : Form
    {
        public frmDeriverLicenseInfo(int LocalDLID, int LicenseID = -1)
        {
            InitializeComponent();
            
            if(LicenseID == -1) 
            {
                ctrlLicenseInfo1.Fill(LocalDLID);
            }
            else 
            {
                ctrlLicenseInfo1.FillByLicenseID(LicenseID);
            }

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
