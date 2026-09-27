using DLL;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class frmChangePassword : Form
    {
        private bool _showPassword = false;
        private bool ShowPassword
        {
            get => _showPassword;
            set
            {
                _showPassword = value;
                txtNewPwrd.UseSystemPasswordChar = !_showPassword;
                txtConfirmPwrd.UseSystemPasswordChar = !_showPassword;
                btnShowHidePwrd.Image = _showPassword ? Resources.ClosedEye : Resources.OpenedEye;
            }
        }
        private int _userID;
        private User _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _userID = UserID;
        }
        private void _ResetFields()
        {
            txtCurrentPwrd.Text = "";
            txtNewPwrd.Text = "";
            txtConfirmPwrd.Text = "";
            txtCurrentPwrd.Focus();
        }

        private void txtCurrentPwrd_Validating(object sender, CancelEventArgs e)
        {
            if (txtCurrentPwrd.Text.Trim() == "")
            {
                errorProvider1.SetError(txtCurrentPwrd, "Please enter current password");
                e.Cancel = true;
            }
            else if (txtCurrentPwrd.Text.Trim() != _User.Password)
            {
                errorProvider1.SetError(txtCurrentPwrd, "Incorrect password");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError(txtCurrentPwrd, "");
        }

        private void txtNewPwrd_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPwrd.Text))
            {
                errorProvider1.SetError(txtNewPwrd, "Please enter new password");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError(txtNewPwrd, "");
        }

        private void txtConfirmPwrd_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPwrd.Text))
            {
                errorProvider1.SetError(txtConfirmPwrd, "Please confirm new password");
                e.Cancel = true;
            }
            else if (txtConfirmPwrd.Text.Trim() != txtNewPwrd.Text.Trim())
            {
                errorProvider1.SetError(txtConfirmPwrd, "Password does not match");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError(txtConfirmPwrd, "");
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ShowPassword = false;
            _ResetFields();
            _User = User.Find(_userID);
            if (_User == null)
            {
                MessageBox.Show("User not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlUserCard1.LoadUserInfo(_userID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Please correct the errors and try again", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.Password = txtNewPwrd.Text.Trim();
            if (User.ChangePassword(_userID, txtNewPwrd.Text.Trim()))
            {
                MessageBox.Show("Password changed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to change password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShowHidePwrd_Click(object sender, EventArgs e)
        {
            ShowPassword = !ShowPassword;
        }
    }
}
