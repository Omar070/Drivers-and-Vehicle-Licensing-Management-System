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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DVLD_Project_Mine_
{
   
    public partial class frmPeopleManagement : Form
    {
        private static readonly string ImageFolder = Path.Combine(Application.StartupPath, "PeopleImages");
        public frmPeopleManagement()
        {
            InitializeComponent();
        }

        DataTable dt = clsPersons.GetAllPeople();
        private void frmPeopleManagement_Load(object sender, EventArgs e)
        {
            RefreshTheDGV(sender, e);
            cbSortBy.SelectedIndex = 0;
            cbFilterBy.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson();
            frm.SaveCompleted += RefreshTheDGV;
            frm.ShowDialog();
        }

        // Context Menu Strip
        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson();
            frm.SaveCompleted += RefreshTheDGV;
            frm.ShowDialog();
        }
      
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvPeople.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvPeople.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmAddNewPerson frm = new frmAddNewPerson(ID);
                frm.SaveCompleted += RefreshTheDGV;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvPeople.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvPeople.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1) 
            {
                clsPersons Person = clsPersons.FindPersonByID(ID);
                string ImagePath = Person.ImagePath;

                if (clsPersons.DeletePerson(ID))
                {
                    try { File.Delete(ImageFolder + "\\" + ImagePath); }
                    catch { MessageBox.Show("Their is no old pic to delete!"); }

                    MessageBox.Show("Person Deleted Successfully");
                }
                else 
                {
                    MessageBox.Show("Person was not deleted because there is data liked to him");
                }
                RefreshTheDGV(sender, e);
            }
            else
            {
                MessageBox.Show("Error, No Selected Person!!");
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Function will be available soon En Shaa' Allah");
        }

        private void callPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Function will be available soon En Shaa' Allah");
        }
         
        // Context Menu Strip Done
        public void RefreshTheDGV(object sender, EventArgs e)
        {
            DataTable dt1 = clsPersons.GetAllPeople();
            dgvPeople.DataSource = dt1;
            dgvPeople.Refresh();
            lblNumberOfRecords.Text = (Convert.ToInt32(dgvPeople.RowCount - 1)).ToString();
        }

        private void cbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbSortBy.SelectedIndex) 
            {
                case 1: // "PersonID":
                    dgvPeople.Sort(dgvPeople.Columns["PersonID"], ListSortDirection.Ascending);
                    break;
                case 2: // "National No":
                    dgvPeople.Sort(dgvPeople.Columns["NationalNo"], ListSortDirection.Ascending);
                    break;
                case 3: //"First Name":
                    dgvPeople.Sort(dgvPeople.Columns["FirstName"], ListSortDirection.Ascending);
                    break;
                case 4: // "Second Name":
                    dgvPeople.Sort(dgvPeople.Columns["SecondName"], ListSortDirection.Ascending);
                    break;
                case 5: //  "Third Name":
                    dgvPeople.Sort(dgvPeople.Columns["ThirdName"], ListSortDirection.Ascending);
                    break;
                case 6://  "Last Name":
                    dgvPeople.Sort(dgvPeople.Columns["LastName"], ListSortDirection.Ascending);
                    break;
                case 7: // "Gender":
                    dgvPeople.Sort(dgvPeople.Columns["Gendor"], ListSortDirection.Ascending);
                    break;
                case 8: // "DateOfBirth":
                    dgvPeople.Sort(dgvPeople.Columns["DateOfBirth"], ListSortDirection.Ascending);
                    break;
                case 9: // "Nationality":
                    dgvPeople.Sort(dgvPeople.Columns["CountryName"], ListSortDirection.Ascending);
                    break;
                case 10: // "Phone":
                    dgvPeople.Sort(dgvPeople.Columns["Phone"], ListSortDirection.Ascending);
                    break;
                case 11: // "Email":
                    dgvPeople.Sort(dgvPeople.Columns["Email"], ListSortDirection.Ascending);
                    break;
                default:
                    dgvPeople.Sort(dgvPeople.Columns["PersonID"], ListSortDirection.Ascending);
                    break;

            }
        }


        private void cbFilterBy_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0) 
            {
                mtxtFilterBy.Visible = false;
                RefreshTheDGV(sender, e);
            }
            else 
            {
                mtxtFilterBy.Visible = true;
                RefreshTheDGV(sender, e);
            }
        }

        
        private void mtxtFilterBy_TextChanged(object sender, EventArgs e)
        {

            switch (cbFilterBy.SelectedIndex)
            {
                case 1: // "PersonID":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99999999";
                    ApplyFilter();
                    break;

                case 2: // "National No":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "CCCCCCC";
                    ApplyFilter();
                    break;

                case 3: //"First Name":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 4: // "Second Name":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 5: //  "Third Name":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 6://  "Last Name":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 7: // "Gender":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "9";
                    ApplyFilter();
                    break;

                case 8: // "DateOfBirth":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "99" + '/' + "99" + '/' + "9999";
                    ApplyFilter();
                    break;

                case 9: // "Nationality":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "???????????????????????????????";
                    ApplyFilter();
                    break;

                case 10: // "Phone":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "9999999999999";
                    ApplyFilter();
                    break;

                case 11: // "Email":
                    mtxtFilterBy.Visible = true;
                    mtxtFilterBy.Mask = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    ApplyFilter();
                    break;


                default:
                    mtxtFilterBy.Visible = false;
                    break;

            }


        }


        // ChatGPT 
        private void ApplyFilter()
        {
            string filterText = mtxtFilterBy.Text;
            string selectedColumn = cbFilterBy.SelectedItem.ToString();
            DataColumn column = dt.Columns[selectedColumn];

            if (column.DataType == typeof(string))
            {
                (dgvPeople.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("{0} LIKE '%{1}%'", selectedColumn, filterText);
            }
            else if (column.DataType == typeof(int))
            {
                if (int.TryParse(filterText, out int filterInt))
                {
                    (dgvPeople.DataSource as DataTable).DefaultView.RowFilter =
                        string.Format("{0} = {1}", selectedColumn, filterInt);
                }
                else
                {
                    (dgvPeople.DataSource as DataTable).DefaultView.RowFilter = "1=0"; // No match
                }
            }
            else if (column.DataType == typeof(DateTime))
            {
                if (DateTime.TryParse(filterText, out DateTime filterDate))
                {
                 // this modification is made to compare date parts without time time part
                    string filterExpression = string.Format("{0} >= #{1}# AND {0} < #{2}#",
                    selectedColumn,
                    filterDate.ToString("MM/dd/yyyy"),
                    filterDate.AddDays(1).ToString("MM/dd/yyyy"));

                    (dgvPeople.DataSource as DataTable).DefaultView.RowFilter = filterExpression;

                }
                else
                {
                    (dgvPeople.DataSource as DataTable).DefaultView.RowFilter = "1=0"; // No match
                }
            }

            lblNumberOfRecords.Text = (Convert.ToInt32(dgvPeople.RowCount - 1)).ToString();

        }
        // Done 
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = -1;
            if (dgvPeople.SelectedRows.Count > 0)
            {
                ID = Convert.ToInt32(dgvPeople.SelectedRows[0].Cells[0].Value);
            }

            if (ID != -1)
            {
                frmPersonDetails frm = new frmPersonDetails(ID);
                frm.ShowDialog();
                RefreshTheDGV(sender, e);
            }
            else
            {
                MessageBox.Show("Error, No Selected Person!!");
            }
        }

        







    }
}
