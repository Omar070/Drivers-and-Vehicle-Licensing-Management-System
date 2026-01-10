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
    public partial class ctrlReleaseDetainedInfo : UserControl
    {
        public ctrlReleaseDetainedInfo()
        {
            InitializeComponent();
        }

        clsDetainedLicense Detained;
        decimal AppFees = clsApplicationTypes.GetAppTypeFeesByAppTypeID(5);
        public void Fill(int LicenseID) 
        {
            if (!clsLicense.IsLicenseExist(LicenseID)) { return; }

            Detained = clsDetainedLicense.GetDetainedLicenseByLicenseID(LicenseID);

            lblDetainID.Text = Detained.DetainID.ToString();
            lblLicenseID.Text = Detained.LicenseID.ToString();

            lblDetaineDate.Text = Detained.DetainDate.ToShortDateString();
            lblCreatedBy.Text = Detained.CreatedByUserID.ToString();
           
            lblApplicationFees.Text = AppFees.ToString();
            lblFineFees.Text = Detained.FineFees.ToString();
            lblTotalFees.Text = (AppFees + Detained.FineFees).ToString();

        
            lblApplicationID.Text = "????";

        }

        public void GetAppID(int ReleaseAppID) 
        {
            lblApplicationID.Text = ReleaseAppID.ToString();
        }

    }
}
