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

namespace DVLD_Project.Users
{
    public partial class frmAddUpdateUser : Form
    {
        private int _UserID = -1;
        private User _User;
        enum enMode { Add =  0, Update = 1 }
        private enMode _Mode;
        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.Add;
        }
        public frmAddUpdateUser(int userID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _UserID = userID;
        }
        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.Add)
            {
                lblScreenMode.Text = "Add New User";
                this.Text = "Add New User";
                _User = new User();
                tpLoginInfo.Enabled = false;
                ctrlPersonCardInfoWithFilter1.FilterFocus();
            }
            else
            {
                lblScreenMode.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            lblUserID.Text = "[....]";
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtPwrdconfirm.Text = string.Empty;
            cbUserState.Checked = false;
        }
        private void _LoadUserData()
        {
            _User = User.Find(_UserID);
            ctrlPersonCardInfoWithFilter1.FilterEnabled = false;
            if (_User == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblUserID.Text = _User.UserID.ToString();
            txtUsername.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtPwrdconfirm.Text = _User.Password;
            cbUserState.Checked = _User.IsActive;
            ctrlPersonCardInfoWithFilter1.LoadPersonInfo(_User.PersonID);
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadUserData();
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
            _User.PersonID = ctrlPersonCardInfoWithFilter1.PersonID;
            _User.UserName = txtUsername.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = cbUserState.Checked;
            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblScreenMode.Text = "Update User Info";
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfully",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data was not saved successfully",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _ResetDefaultValues();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }

            //incase of add new mode.
            if (ctrlPersonCardInfoWithFilter1.PersonID != -1)
            {

                if (User.IsUserExistsForPersonID(ctrlPersonCardInfoWithFilter1.PersonID))
                {

                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardInfoWithFilter1.FilterFocus();
                }

                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }
            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardInfoWithFilter1.FilterFocus();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            tcUserInfo.SelectedIndex = 0;
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

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            _ValidateTextBox(txtUsername, e, "Username is required.");
            if (_Mode == enMode.Add)
            {

                if (User.isExist(txtUsername.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUsername, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUsername, null);
                }
                ;
            }
            else
            {
                //incase update make sure not to use anothers user name
                if (_User.UserName != txtUsername.Text.Trim())
                {
                    if (User.isExist(txtUsername.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUsername, "username is used by another user");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(txtUsername, null);
                    }
                    ;
                }
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            _ValidateTextBox(txtPassword, e, "Password is required.");
        }

        private void txtPwrdconfirm_Validating(object sender, CancelEventArgs e)
        {
            _ValidateTextBox(txtPwrdconfirm, e, "Password confirmation is required.");
            if (txtPassword.Text != txtPwrdconfirm.Text)
            {
                errorProvider1.SetError(txtPwrdconfirm, "Passwords do not match.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtPwrdconfirm, "");
            }
        }

        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardInfoWithFilter1.FilterFocus();
        }
    }
}