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
    public partial class ctrlLIcenseInfoWithFilter : UserControl
    {
        public ctrlLIcenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public delegate void SearchCompletedEventHandler(int LicenseID);
        public event SearchCompletedEventHandler SearchCompleted;
        protected virtual void OnSearchCompleted(int ID) 
        {
            SearchCompleted?.Invoke(ID);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(txtLicenseID.Text != "" && int.TryParse(txtLicenseID.Text, out int InsertedID)) 
            {
                ctrlLicenseInfo1.FillByLicenseID(InsertedID);
                OnSearchCompleted(InsertedID);
            }
            else
            {
                MessageBox.Show("Invalid Data");
            }
        }

        public void FillWithoutSearch(int LID) 
        {
            ctrlLicenseInfo1.FillByLicenseID(LID);
        }


        public void DisableFilter() 
        {
            gbFilter.Enabled = false;
        }
    }
}
