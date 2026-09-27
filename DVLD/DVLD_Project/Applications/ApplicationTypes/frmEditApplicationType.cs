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

namespace DVLD_Project.Applications.ApplicationTypes
{
    public partial class frmEditApplicationType : Form
    {
        private ApplicationsTypes _applicationType;
        private int _applicationTypeID = -1;
        public frmEditApplicationType(int applicationTypeID)
        {
            InitializeComponent();
            _applicationTypeID = applicationTypeID;
        }
        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblApplicationsTypeID.Text = _applicationTypeID.ToString();
            _applicationType = ApplicationsTypes.Find(_applicationTypeID);
            if (_applicationType != null)
            {
                txtTitle.Text = _applicationType.Title;
                txtFees.Text = _applicationType.Fees.ToString();
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title cannot be empty!");
            }
            else
                errorProvider1.SetError(txtTitle, null);
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
            }
            else if (!float.TryParse(txtFees.Text, out float fee))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Please enter a valid numeric value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, "");
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren()) return;

            if (_applicationType == null)
            {
                MessageBox.Show("Error: Application Type not found.");
                return;
            }

            _applicationType.Title = txtTitle.Text.Trim();
            _applicationType.Fees = Convert.ToSingle(txtFees.Text.Trim());

            if (_applicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
