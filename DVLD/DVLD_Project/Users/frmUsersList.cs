using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLL;

namespace DVLD_Project.Users
{
    public partial class frmUsersList : Form
    {
        public frmUsersList()
        {
            InitializeComponent();
            dgvUsersList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsersList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        private void _RefreshUsersList()
        {
            dgvUsersList.DataSource = User.GetAllUsers();
        }
        private void frmUsersList_Load(object sender, EventArgs e)
        {
            dgvUsersList.DataSource = User.GetAllUsers();
            cbFilterBy.SelectedIndex = 0;
            cbUsersState.SelectedIndex = 0;
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            /*
                User ID
                Person ID
                Username
                Fullname
                User State
            */
            string FilterColumns = "";
            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumns = "User ID";
                    break;
                case "Person ID":
                    FilterColumns = "Person ID";
                    break;
                case "Username":
                    FilterColumns = "Username";
                    break;
                case "Fullname":
                    FilterColumns = "Fullname";
                    break;
                case "User State":
                    FilterColumns = "User State";
                    break;
                default:
                    FilterColumns = "User ID";
                    break;
            }
            string searchText = txtFilterValue.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                dgvUsersList.DataSource = User.GetAllUsers();
                return;
            }

            var dt = dgvUsersList.DataSource as DataTable;
            if (dt == null)
                return;

            // handle numeric comparison safely for PersonID
            if (FilterColumns == "Person ID" || FilterColumns == "User ID")
            {
                int idValue;
                if (int.TryParse(searchText, out idValue))
                {
                    dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumns, idValue);
                }
                else
                {
                    // invalid numeric input - show no rows
                    dt.DefaultView.RowFilter = "1 = 0";
                }
            }
            else
            {
                // escape single quotes in search text
                string escaped = searchText.Replace("'", "''");
                dt.DefaultView.RowFilter = string.Format("{0} LIKE '%{1}%'", FilterColumns, escaped);
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text ==  "User State")
            {
                txtFilterValue.Visible = false;
                cbUsersState.Visible = true;
            }
            else
            {
                txtFilterValue.Visible = true;
                cbUsersState.Visible = false;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
        }

        private void cbUsersState_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dt = dgvUsersList.DataSource as DataTable;
            if (dt == null)
                return;
            switch(cbUsersState.Text)
            {
                case "All":
                    dt.DefaultView.RowFilter = "";
                    break;

                case "Active":
                    dt.DefaultView.RowFilter = "[User State] = 1";
                    break;

                case "Not Active":
                    dt.DefaultView.RowFilter = "[User State] = 0";
                    break;
            }
        }

        private void dgvUsersList_DoubleClick(object sender, EventArgs e)
        {
            int personID = (int)dgvUsersList.CurrentRow.Cells[0].Value;
            frmUserInfo userInfo = new frmUserInfo(personID);
            userInfo.ShowDialog();
            _RefreshUsersList();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser addUserForm = new frmAddUpdateUser();
            addUserForm.ShowDialog();
            _RefreshUsersList();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser addNewUser = new frmAddUpdateUser();
            addNewUser.ShowDialog();
            _RefreshUsersList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser editUserForm = new frmAddUpdateUser((int)dgvUsersList.CurrentRow.Cells[0].Value);
            editUserForm.ShowDialog();
            _RefreshUsersList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userID = (int)dgvUsersList.CurrentRow.Cells[0].Value;
            if (User.DeleteUser(userID))
            {
                MessageBox.Show("User has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("User is not deleted due to data connected to it.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePasswordStripMenuItem1_Click(object sender, EventArgs e)
        {
            int userID = (int)dgvUsersList.CurrentRow.Cells[0].Value;
            frmChangePassword changePasswordForm = new frmChangePassword(userID);
            changePasswordForm.ShowDialog();
        }
    }
}
