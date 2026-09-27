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
    public partial class frmPeopleList : Form
    {
        public frmPeopleList()
        {
            InitializeComponent();
            dgvPeopleList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPeopleList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        private void _RefreshPeopleList()
        {
            dgvPeopleList.DataSource = Person.GetAllPPL();
        }
        private void frmPeopleList_Load(object sender, EventArgs e)
        {
            dgvPeopleList.DataSource = Person.GetAllPPL();
            if (dgvPeopleList.Columns.Contains("ImagePath"))
                dgvPeopleList.Columns["ImagePath"].Visible = false;
        }

        private void dgvPeopleList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int PersonID = (int)dgvPeopleList.CurrentRow.Cells[0].Value;
            frmPersonDetails personDetails = new frmPersonDetails(PersonID);
            personDetails.ShowDialog();
            _RefreshPeopleList();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson addPerson = new frmAddUpdatePerson();
            addPerson.ShowDialog();
            _RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeopleList.CurrentRow.Cells[0].Value;
            frmAddUpdatePerson addPerson = new frmAddUpdatePerson(PersonID);
            addPerson.ShowDialog();
            _RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeopleList.CurrentRow == null)
                return;

            int PersonID = (int)dgvPeopleList.CurrentRow.Cells[0].Value;
            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Person.DeletePerson(PersonID))
                {
                    MessageBox.Show("Person deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeopleList();
                }
                else
                {
                    MessageBox.Show("Failed to delete the person. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showMoreInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails personDetails = new frmPersonDetails((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            personDetails.ShowDialog();
            _RefreshPeopleList();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson addPerson = new frmAddUpdatePerson();
            addPerson.ShowDialog();
            _RefreshPeopleList();
        }

        private void txtSearchByValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumns = "";
            switch (cbSearchBy.Text)
            {
                case "Person ID":
                    FilterColumns = "PersonID";
                    break;
                case "National No":
                    FilterColumns = "NationalNo";
                    break;
                case "First Name":
                    FilterColumns = "FirstName";
                    break;
                case "Second Name":
                    FilterColumns = "SecondName";
                    break;
                case "Third Name":
                    FilterColumns = "ThirdName";
                    break;
                case "Last Name":
                    FilterColumns = "LastName";
                    break;
                case "Email":
                    FilterColumns = "Email";
                    break;
                case "Phone":
                    FilterColumns = "Phone";
                    break;
                default:
                    FilterColumns = "PersonID";
                    break;
            }

            string searchText = txtSearchByValue.Text.Trim();

            // if search text is empty reset datasource and exit early
            if (string.IsNullOrEmpty(searchText))
            {
                dgvPeopleList.DataSource = Person.GetAllPPL();
                return;
            }

            var dt = dgvPeopleList.DataSource as DataTable;
            if (dt == null)
                return;

            // handle numeric comparison safely for PersonID
            if (FilterColumns == "PersonID")
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

        private void cbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSearchBy.Visible)
            {
                txtSearchByValue.Text = "";
                txtSearchByValue.Focus();
            }
        }

        private void txtSearchByValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbSearchBy.Text == "Person ID")
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
        }
    }
}