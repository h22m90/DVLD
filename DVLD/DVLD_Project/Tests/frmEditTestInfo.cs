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

namespace DVLD_Project.Tests
{
    public partial class frmEditTestInfo : Form
    {
        private TestTypes.enTestType _TestTypeID = TestTypes.enTestType.VisionTest;
        private TestTypes _TestType;
        public frmEditTestInfo(TestTypes.enTestType testID)
        {
            InitializeComponent();
            _TestTypeID = testID;
        }

        private void frmEditTestInfo_Load(object sender, EventArgs e)
        {
            lblTestID.Text = ((int)_TestTypeID).ToString();
            _TestType = TestTypes.Find(_TestTypeID);
            if (_TestType != null)
            {
                txtTestTitle.Text = _TestType.TestTypeTitle;
                txtTestDescription.Text = _TestType.TestTypeDescription;
                txtTestFees.Text = _TestType.TestTypeFees.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren()) return;
            if (_TestType == null)
            {
                MessageBox.Show("Loading test data failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _TestType.TestTypeTitle = txtTestTitle.Text.Trim();
            _TestType.TestTypeDescription = txtTestDescription.Text.Trim();
            _TestType.TestTypeFees = Convert.ToSingle(txtTestFees.Text.Trim());
            if(_TestType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error saving data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTestTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTitle, "Title cannot be empty!");
            }
            else
                errorProvider1.SetError(txtTestTitle, null);
        }

        private void txtTestDescription_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtTestDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestDescription, "Description cannot be empty!");
            }
            else
                errorProvider1.SetError(txtTestDescription, null);
        }

        private void txtTestFees_KeyPress(object sender, KeyPressEventArgs e)
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
