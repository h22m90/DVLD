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

namespace DVLD_Project.Applications.Local_Driving_License
{
    public partial class frmLDL_App_List : Form
    {
        public frmLDL_App_List()
        {
            InitializeComponent();
            dgvLDLAppList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLDLAppList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void frmLDL_App_List_Load(object sender, EventArgs e)
        {
            dgvLDLAppList.DataSource = LocalDrivingLicenseApplications.GetAllL_D_L_Applications();
            cbFilterBy.SelectedItem = "None";
        }
        private void _refreshLDL_List()
        {
            dgvLDLAppList.DataSource = LocalDrivingLicenseApplications.GetAllL_D_L_Applications();
        }
        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLDL_Application frmAddUpdateLDL_Application = new frmAddUpdateLDL_Application();
            frmAddUpdateLDL_Application.ShowDialog();
            _refreshLDL_List();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumns = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {

                case "License ID":
                    FilterColumns = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilterColumns = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumns = "FullName";
                    break;

                case "Status":
                    FilterColumns = "Status";
                    break;


                default:
                    FilterColumns = "None";
                    break;

            }
            string searchText = txtFilterValue.Text;
            if(string.IsNullOrEmpty(searchText))
            {
                dgvLDLAppList.DataSource = LocalDrivingLicenseApplications.GetAllL_D_L_Applications();
                return;
            }
            var dt = dgvLDLAppList.DataSource as DataTable;
            if (dt == null)
                return;
            if(FilterColumns == "LocalDrivingLicenseApplicationID")
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

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "License ID")
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
        }
    }
}