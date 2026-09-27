using DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class ctrlPersonCardInfo : UserControl
    {
        private int _PersonID = -1;
        private Person _Person;
        public int PersonID
        {
            get { return _PersonID; }
        }
        public Person SelectedPersonInfo
        {
            get { return _Person; }
        }
        public ctrlPersonCardInfo()
        {
            InitializeComponent();
        }
        public void LoadPersonInfo(int PersonID)
        {
            _Person = Person.Find(PersonID);
            if (_Person == null)
            {
                ClearPersonData();
                MessageBox.Show("Person not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DisplayPersonData();
        }
        public void LoadPersonInfo(string NationalNo)
        {
            _Person = Person.Find(NationalNo);
            if (_Person == null)
            {
                ClearPersonData();
                MessageBox.Show("Person not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DisplayPersonData();
        }
        private void _DisplayPersonData()
        {
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.FullName;
            lblNationalNo.Text = _Person.NationalNo;
            if (_Person.Gender == 0)
                lblGender.Text = "Male";
            else
                lblGender.Text = "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblAddress.Text = _Person.Address;
            lblDOB.Text = _Person.DOB.ToShortDateString();
            lblCountry.Text = _Person.CountryInfo.CountryName;

            if (!string.IsNullOrEmpty(_Person.ImagePath) &&
                System.IO.File.Exists(_Person.ImagePath))
            {
                pbPersonalImage.Image = Image.FromFile(_Person.ImagePath);
            }
            else
            {
                pbPersonalImage.Image = null;
            }
        }
        public void ClearPersonData()
        {
            _PersonID = -1;
            _Person = null;
            lblPersonID.Text = "N/A";
            lblName.Text = "[....]";
            lblNationalNo.Text = "[....]";
            lblGender.Text = "[....]";
            lblEmail.Text = "[....]";
            lblPhone.Text = "[....]";
            lblAddress.Text = "[....]";
            lblDOB.Text = "[....]";
            lblCountry.Text = "[....]";
            pbPersonalImage.Image = null;
        }

        private void llEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();
            LoadPersonInfo(PersonID);
        }
    }
}
