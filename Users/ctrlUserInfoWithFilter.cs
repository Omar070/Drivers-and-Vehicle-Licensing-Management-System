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
    public partial class ctrlUserInfoWithFilter : UserControl
    {
        
        public ctrlUserInfoWithFilter()
        {
            InitializeComponent();
        }

        clsPersons Person;

        // Initiation the event to sent the Person ID to the form
        public delegate void SearchCompletedSendEventHandler(int ID);
        public event SearchCompletedSendEventHandler SearchCompletedSend;
        protected virtual void OnSearchCompletedSend(int ID)
        {
            SearchCompletedSend?.Invoke(ID);
        }
        // Event Done


        // recreation of search button logic
        private void FindAndFill() 
        {
            if(cbFilterBy.SelectedIndex == 0) 
            {
                try 
                { 
                    Person = clsPersons.FindPersonByNationalNo(txtSearchBy.Text);
                }
                catch 
                {
                    MessageBox.Show("Incorrect National Number");
                }
                
            }
            else if(cbFilterBy.SelectedIndex == 1) 
            {
                if (int.TryParse(txtSearchBy.Text, out int ID))
                {
                    try 
                    {
                        Person = clsPersons.FindPersonByID(ID);
                    }
                    catch 
                    {
                        MessageBox.Show("Can't find the person!");
                    }
                }
                else 
                {
                    MessageBox.Show("Incorrect ID, Please Check the Format");
                }
            }
            else 
            {
                MessageBox.Show("Unexpected Error");
                return;
            }

            if (Person != null)
            {
                ctrlPersonInfo1.FillPersonInfo(Person.PersonID);
                
                OnSearchCompletedSend(Person.PersonID);
            }
            else 
            {
                MessageBox.Show("The Object Is NULL");
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FindAndFill();
        }
        // Modification until here


        // This function is related to 'Update User' status
        public void ShowUserDetails(int PersonID) 
        {
            ctrlPersonInfo1.FillPersonInfo(PersonID);
            Person = clsPersons.FindPersonByID(PersonID);

            if(cbFilterBy.SelectedIndex == 0) 
            {
                txtSearchBy.Text = Person.NationalNo;
            }
            else if(cbFilterBy.SelectedIndex == 1) 
            {
                txtSearchBy.Text = Person.PersonID.ToString();
            }

            cbFilterBy.Enabled = false;
            txtSearchBy.Enabled = false;
            btnSearch.Enabled = false;
            btnAddNewPerson.Enabled = false;
            groupBox1.Enabled = false;

        }
        // End of the function (that is related to 'Update User')

        // one form load
        private void ctrlUserInfoWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }
        // Done

        // This function is made to fill the data of a new person which is just added
        public void FillTheNewPerson(object sender, int ID) 
        {
            Person = clsPersons.FindPersonByID(ID);
            if(Person != null) 
            {
                ctrlPersonInfo1.FillPersonInfo(Person.PersonID);
                cbFilterBy.SelectedIndex = 1;
                txtSearchBy.Text = Person.PersonID.ToString();

                OnSearchCompletedSend(Person.PersonID);
            }
            else 
            {
                MessageBox.Show("Error, No Saved Person");
            }
        }
        // Done
    
        // Creating new person directly  
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson();
            frm.SaveCompletedSend += FillTheNewPerson;
            frm.ShowDialog();
        }
        // Done

    }

}
