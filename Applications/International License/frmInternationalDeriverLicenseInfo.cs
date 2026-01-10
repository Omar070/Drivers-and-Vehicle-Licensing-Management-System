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
    public partial class frmInternationalDeriverLicenseInfo : Form
    {
        public frmInternationalDeriverLicenseInfo(int IntLicenseID)
        {
            InitializeComponent();
            ctrlInternationalLicenseInfo1.Fill(IntLicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
