using DLL;
using DVLD_Project.GlobalClasses;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int personID);
        public event DataBackEventHandler DataBack;

        private Person _Person;
        private int _PersonID = -1;

        enum enMode { Add = 0, Update = 1 }
        private enMode _Mode;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.Add;
        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            _Mode = enMode.Update;
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadPersonData();
        }

        private void _ResetDefaultValues()
        {
            _LoadCountriesIntoComboBox();

            if (_Mode == enMode.Add)
            {
                lblScreenMode.Text = "Add New Person";
                this.Text = "Add Person";
                _Person = new Person();
            }
            else
            {
                lblScreenMode.Text = "Update Person Info";
                this.Text = "Update Person";
            }

            lblPersonID.Text = "N/A";

            txtFirstname.Text = "";
            txtSecondname.Text = "";
            txtThirdname.Text = "";
            txtLastname.Text = "";
            txtNationalNo.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            mtbPhone.Text = "";

            dtpDOB.MaxDate = DateTime.Now.AddYears(-18);
            dtpDOB.MinDate = DateTime.Now.AddYears(-100);
            dtpDOB.Value = dtpDOB.MaxDate;

            rbMale.Checked = true;

            cbCountryName.SelectedIndex = cbCountryName.FindString("Turkey");
            pbPersonalImage.Image = null;
        }

        private bool _RemoveImageHelper()
        {
            // consider image location as indicator as well as loaded Image
            return (pbPersonalImage.Image != null) || !string.IsNullOrEmpty(pbPersonalImage.ImageLocation);
        }

        private void _LoadCountriesIntoComboBox()
        {
            DataTable dt = Country.GetAllCountries();

            cbCountryName.DataSource = dt;
            cbCountryName.DisplayMember = "CountryName";
        }

        private void _LoadPersonData()
        {
            _Person = Person.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID,
                    "Person Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                this.Close();
                return;
            }

            lblPersonID.Text = _Person.PersonID.ToString();

            txtFirstname.Text = _Person.FirstName;
            txtSecondname.Text = _Person.SecondName;
            txtThirdname.Text = _Person.ThirdName;
            txtLastname.Text = _Person.LastName;

            txtNationalNo.Text = _Person.NationalNo;

            rbMale.Checked = _Person.Gender == 0;
            rbFemale.Checked = _Person.Gender == 1;

            cbCountryName.Text = _Person.CountryInfo.CountryName;

            txtAddress.Text = _Person.Address;
            txtEmail.Text = _Person.Email;
            mtbPhone.Text = _Person.Phone;

            dtpDOB.Value = _Person.DOB;

            // guard against null and empty values
            if (!string.IsNullOrEmpty(_Person.ImagePath))
            {
                pbPersonalImage.ImageLocation = _Person.ImagePath;
            }
            else
            {
                pbPersonalImage.ImageLocation = null;
                pbPersonalImage.Image = null;
            }

            if (_RemoveImageHelper())
                llRemoveImage.Visible = true;
            else
                llRemoveImage.Visible = false;
        }

        private void _ValidateTextBox(TextBox txt, CancelEventArgs e, string message)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                errorProvider1.SetError(txt, message);
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txt, "");
            }
        }

        private void txtFirstname_Validating(object sender, CancelEventArgs e)
        {
            _ValidateTextBox(txtFirstname, e, "First name is required");
        }

        private void txtSecondname_Validating(object sender, CancelEventArgs e)
        {
            _ValidateTextBox(txtSecondname, e, "Second name is required");
        }

        private void txtLastname_Validating(object sender, CancelEventArgs e)
        {
            _ValidateTextBox(txtLastname, e, "Last name is required");
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "National number is required");
                e.Cancel = true;
                return;
            }

            if (_Mode == enMode.Add && Person.IsExist(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "This National number is used for another person");
                e.Cancel = true;
                return;
            }

            if (_Mode == enMode.Update &&
                txtNationalNo.Text != _Person.NationalNo &&
                Person.IsExist(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "This National number is used for another person");
                e.Cancel = true;
                return;
            }

            errorProvider1.SetError(txtNationalNo, "");
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            _ValidateTextBox(txtAddress, e, "Address is required");
        }

        private void mtbPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void mtbPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mtbPhone.Text))
            {
                errorProvider1.SetError(mtbPhone, "Phone number is required");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(mtbPhone, "");
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            txtEmail.Text = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "");
                return;
            }

            try
            {
                MailAddress email = new MailAddress(txtEmail.Text);
                errorProvider1.SetError(txtEmail, "");
            }
            catch
            {
                errorProvider1.SetError(txtEmail, "Invalid email format");
                e.Cancel = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _ResetDefaultValues();
        }

        private bool _HandlePersonImage()
        {
            // normalize values for safe comparison
            string existingPath = _Person?.ImagePath;
            string newPath = pbPersonalImage.ImageLocation;

            bool existingIsEmpty = string.IsNullOrEmpty(existingPath);
            bool newIsEmpty = string.IsNullOrEmpty(newPath);

            // nothing to do if both are empty or identical
            if (existingIsEmpty && newIsEmpty)
                return true;

            if (!existingIsEmpty && !newIsEmpty &&
                string.Equals(existingPath, newPath, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // delete existing file only if it's a valid path and the file exists
            if (!existingIsEmpty)
            {
                try
                {
                    if (File.Exists(existingPath))
                        File.Delete(existingPath);
                }
                catch (IOException)
                {
                    // swallow or log as needed
                }
                catch (UnauthorizedAccessException)
                {
                    // swallow or log as needed
                }
            }

            // if a new image is set in the picturebox, copy it into project images folder
            if (!newIsEmpty)
            {
                string SourceImageFile = newPath;

                if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                {
                    // update both picturebox and person model with new path
                    pbPersonalImage.ImageLocation = SourceImageFile;
                    _Person.ImagePath = SourceImageFile;
                    return true;
                }
                else
                {
                    MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // If new path is empty but existing was deleted, clear model image path
            _Person.ImagePath = "";
            pbPersonalImage.Image = null;
            pbPersonalImage.ImageLocation = null;

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show(
                    "Some fields are invalid. Hover over the red icons to see the errors.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!_HandlePersonImage())
                return;

            int NationalityCountryID = Country.Find(cbCountryName.Text).CountryID;

            _Person.FirstName = txtFirstname.Text.Trim();
            _Person.SecondName = txtSecondname.Text.Trim();
            _Person.ThirdName = txtThirdname.Text.Trim();
            _Person.LastName = txtLastname.Text.Trim();

            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = mtbPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();

            _Person.DOB = dtpDOB.Value;

            _Person.Gender = (byte)(rbMale.Checked ? 0 : 1);

            _Person.NationalityCountryID = NationalityCountryID;

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();

                _Mode = enMode.Update;

                lblScreenMode.Text = "Update Person Info";

                MessageBox.Show("Data Saved Successfully",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Data was not saved successfully",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonalImage.Load(selectedFilePath);
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonalImage.ImageLocation = null;
            pbPersonalImage.Image = null;
            if (_Person != null)
                _Person.ImagePath = "";
            llRemoveImage.Visible = false;
        }
    }
}