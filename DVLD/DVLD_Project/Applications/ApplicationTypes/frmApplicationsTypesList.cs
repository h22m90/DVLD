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

namespace DVLD_Project.Applications.ApplicationTypes
{
    public partial class frmApplicationsTypesList : Form
    {
        public frmApplicationsTypesList()
        {
            InitializeComponent();
            dgvApplicationsList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvApplicationsList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        private void _RefreshApplicationsTypesList()
        {
            dgvApplicationsList.DataSource = ApplicationsTypes.GetAllApplicationsTypes();
        }
        private void frmApplicationsTypesList_Load(object sender, EventArgs e)
        {
            dgvApplicationsList.DataSource = ApplicationsTypes.GetAllApplicationsTypes();
        }

        private void editAppliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType frmEditApplicationType = new frmEditApplicationType((int)dgvApplicationsList.CurrentRow.Cells[0].Value);
            frmEditApplicationType.ShowDialog();
            _RefreshApplicationsTypesList();
        }
    }
}
