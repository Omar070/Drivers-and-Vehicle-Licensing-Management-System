using DVLD_Project_Mine_.Properties;
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
using System.IO;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using System.Deployment.Internal;


namespace DVLD_Project_Mine_
{
    public partial class frmAddNewPerson : Form
    {
        private static readonly string ImageFolder = Path.Combine(Application.StartupPath, "PeopleImages");

        // Default Constructor
        public frmAddNewPerson()
        {
            InitializeComponent();

            rbMale.CheckedChanged += new EventHandler(rbMale_CheckedChanged);
            rbFemale.CheckedChanged += new EventHandler(rbFemale_CheckedChanged);
        }

        // Creating the main object
        clsPersons Person = new clsPersons();
        // Done

        // Parametrized Constructor
        public frmAddNewPerson(int iD)
        {
            InitializeComponent();
          
            Person = clsPersons.FindPersonByID(iD);
            clsPersons.Mode = clsPersons.enMode.Update;

            rbMale.CheckedChanged += new EventHandler(rbMale_CheckedChanged);
            rbFemale.CheckedChanged += new EventHandler(rbFemale_CheckedChanged);
        }

        
        // Event Creation
        public delegate void SaveCompletedEventHandler(object sender, EventArgs e);
        public event SaveCompletedEventHandler SaveCompleted;
        //OnSaveCompleted Function is called when we want to fire this event (SaveCompleted)

        // New Event Creation to send the ID
        public delegate void SaveCompletedSendEventHandler(object sender, int ID);
        public event SaveCompletedSendEventHandler SaveCompletedSend;
        protected virtual void OnSaveCompletedSend(int ID)
        {
            SaveCompletedSend?.Invoke(this, ID);
        }
        // until here


        // Members Clarification 
        OpenFileDialog of = new OpenFileDialog();
        private string FirstPath = "";
        private string NewGUIDName = "";
        private string Extension = "";
        private string NewDirection = "";

        bool isDefaultImage;


        // On Form Load
        private void frmAddNewPerson_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            dateTimePicker1.Format = DateTimePickerFormat.Short;

            DataTable dt1 = clsPersons.FillCountriesInComboBox();
            cbCountries.DataSource = dt1;
            cbCountries.DisplayMember = "CountryName";
            cbCountries.ValueMember = "CountryID";

            if (clsPersons.Mode == clsPersons.enMode.AddNew)
            {
                lblMode.Text = "Add New Person";
                rbMale.Checked = true;
                cbCountries.SelectedValue = 51;
                isDefaultImage = true;
            }
            else 
            {
                lblMode.Text = "Update Person";
                if(Person != null) 
                {
                    lblID.Text = Person.PersonID.ToString();

                    txtFirstN.Text = Person.FirstName;
                    txtSecondN.Text = Person.SecondName;
                    txtThirdN.Text = Person.ThirdName;
                    txtLastN.Text = Person.LastName;

                    txtNationalNo.Text = Person.NationalNo;
                    txtEmail.Text = Person.Email;
                    txtPhone.Text = Person.Phone;

                    txtAddress.Text = Person.Address;
                    dateTimePicker1.Value = Person.DateOfBirth;

                    if (Person.Gender == 0)
                    {
                        rbMale.Checked = true;
                    }
                    else if (Person.Gender == 1)
                    {
                        rbFemale.Checked = true;
                    }


                    if (Person.ImagePath != "")
                    {
                        try
                        {
                            pictureBox1.Image = Image.FromFile(ImageFolder + "\\" + Person.ImagePath);
                        }
                        catch
                        {
                            MessageBox.Show("Could not find Image Path");
                        }
                        finally
                        {
                            isDefaultImage = false;
                            llRemoveImage.Visible = true;
                        }

                    }
                    else if (Person.ImagePath == "" && rbMale.Checked == true)
                    {
                        pictureBox1.Image = Resources.male;
                    }
                    else
                    {
                        pictureBox1.Image = Resources.female;
                    }

                    cbCountries.SelectedValue = Person.NationalityCountryID;
                }
                else 
                {
                    MessageBox.Show("No Selected Person");
                    this.Close();
                }
                
            }
        }    
        // Done


        // Save and related Functions 
        private void AssignData() 
        {
            Person.FirstName = txtFirstN.Text;
            Person.SecondName = txtSecondN.Text;
            Person.ThirdName = txtThirdN.Text;
            Person.LastName = txtLastN.Text;

            Person.NationalNo = txtNationalNo.Text;
            Person.Email = txtEmail.Text; 
   
            Person.Phone = txtPhone.Text;
            Person.Address = txtAddress.Text;
            Person.DateOfBirth = dateTimePicker1.Value;
            Person.NationalityCountryID = Convert.ToInt32(cbCountries.SelectedValue);

            if (rbMale.Checked)
            { Person.Gender = 0; }
            else
            { Person.Gender = 1; }


            
            if (!isDefaultImage)
            {
                if (FirstPath != "" && NewDirection != "")
                {
                    Directory.CreateDirectory(ImageFolder);
                    File.Copy(FirstPath, NewDirection, overwrite: true);

                    if (clsPersons.Mode == clsPersons.enMode.Update)
                    {
                        try { File.Delete(ImageFolder + "\\" + Person.ImagePath); }
                        catch { MessageBox.Show("Their is no old pic to delete!"); }
                    }

                    Person.ImagePath = NewGUIDName + Extension;

                    return;
                }
                else
                {
                    Person.ImagePath = Person.ImagePath;
                    return;
                }
            }
            else if (isDefaultImage && Person.ImagePath != "")
            {
                string oldPath = Path.Combine(ImageFolder, Person.ImagePath);
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
                else { MessageBox.Show("Their is no old pic to delete!"); }
          

                Person.ImagePath = "";
                return;
            }
            else
            {
                Person.ImagePath = "";
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtFirstN.Text == "" || txtSecondN.Text == "" || txtLastN.Text == "" ||
                txtNationalNo.Text == "" || txtAddress.Text == "" || txtPhone.Text == "") 
            {
                MessageBox.Show("Incomplete Data, Please fill all the mandatory fields");
                return;
            }

            AssignData();

            if (clsPersons.Mode == clsPersons.enMode.AddNew) 
            {
                if (Person.Save())
                {
                    MessageBox.Show("Person Saved Successfully With ID : " + Person.PersonID);
                    lblID.Text = Person.PersonID.ToString();

                    lblMode.Text = "Update Person";
                    clsPersons.Mode = clsPersons.enMode.Update;
                }
                else
                {
                    MessageBox.Show("Error, Failed to Save Person");
                }
            }
            else
            {
                if (Person.Save())
                {
                    MessageBox.Show("Person Updated Successfully");
                }
                else
                {
                    MessageBox.Show("Error, Failed to Update Person");
                }
            }

            OnSaveCompleted(EventArgs.Empty);
            //  this delegate sends the Person ID
            OnSaveCompletedSend(Person.PersonID);
            //
        }
        protected virtual void OnSaveCompleted(EventArgs e)
        {
            SaveCompleted?.Invoke(this, e);
        }
        // Done


        // Defaults
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (isDefaultImage) 
            { pictureBox1.Image = Resources.male; }
            
        }
        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (isDefaultImage)
            { pictureBox1.Image = Resources.female; }
        }
        // Done


        // Set and Remove Image
        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            of.Title = "Open";
            of.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            of.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);;

            if (of.ShowDialog() == DialogResult.OK) 
            {
                FirstPath = of.FileName;
                pictureBox1.Image = Image.FromFile(of.FileName);
                llRemoveImage.Visible = true;
                NewGUIDName = Guid.NewGuid().ToString();
                Extension = Path.GetExtension(of.FileName);
                NewDirection = ImageFolder + "\\" + NewGUIDName + Extension;
                isDefaultImage = false;
            }
            else 
            {
                MessageBox.Show("Didn't Choose a Picture");
                return;
            }
            
        }
        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (rbMale.Checked == true)
            {
                pictureBox1.Image = Resources.male;
            }
            else
            {
                pictureBox1.Image = Resources.female;
            }

            isDefaultImage = true;
            llRemoveImage.Visible = false;
        }
        // Done


        // Validation
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
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtEmail.Text)) 
            {
                if(!txtEmail.Text.Contains("@")) 
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtEmail, "Email Should Be Like that *@*");

                }
                else 
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtEmail, "");
                }
            }
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            CheckValidation((TextBox)sender, e);

            DataTable dt = clsPersons.GetAllPeople();

            if (clsPersons.Mode == clsPersons.enMode.Update && txtNationalNo.Text == Person.NationalNo)
            {
                return;
            }

            foreach (DataRow x in dt.Rows) 
             {
                 if (txtNationalNo.Text == x[1].ToString())
                 {
                     e.Cancel = true;
                     errorProvider1.SetError(txtNationalNo, "This NationalNo Is Used, Please Choose Another One");
                     return;
                 }

             }
            
        }
        // Done
    
    }




}
