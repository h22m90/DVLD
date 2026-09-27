using DLL;
using DVLD_Project.GlobalClasses;
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

namespace DVLD_Project.Login
{
    public partial class frmLogin : Form
    {
        private bool _showPassword = false;

        public frmLogin()
        {
            InitializeComponent();
        }

        private bool ShowPassword
        {
            get => _showPassword;
            set
            {
                _showPassword = value;
                txtPassword.UseSystemPasswordChar = !_showPassword;
                btnShowHidePassword.Image = _showPassword ? Resources.ClosedEye : Resources.OpenedEye;
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            ShowPassword = false;
            txtUserName.Focus();
            string UserName = string.Empty;
            string Password = string.Empty;
            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;
        }

        private void btnShowHidePassword_Click(object sender, EventArgs e)
        {
            ShowPassword = !ShowPassword;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User user = User.Find(txtUserName.Text.Trim(), txtPassword.Text.Trim());
            if (user != null)
            {
                if (chkRememberMe.Checked)
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                else
                    clsGlobal.RememberUsernameAndPassword(string.Empty, string.Empty);
                if (!user.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your account is inactive. Please contact the administrator.", "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                clsGlobal.CurrentUser = user;
                this.Hide();
                frmMain mainForm = new frmMain(this);
                mainForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}