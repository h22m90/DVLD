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
    public partial class frmTestTypeslist : Form
    {
        public frmTestTypeslist()
        {
            InitializeComponent();
            dgvTestsList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTestsList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        private void _RefreshTestsTypesList()
        {
            dgvTestsList.DataSource = TestTypes.GetAllTestList();
        }

        private void frmTestTypeslist_Load(object sender, EventArgs e)
        {
            dgvTestsList.DataSource = TestTypes.GetAllTestList();
        }

        private void editTestInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestInfo editTestInfo = new frmEditTestInfo((TestTypes.enTestType)(dgvTestsList.CurrentRow.Cells[0].Value));
            editTestInfo.ShowDialog();
            _RefreshTestsTypesList();
        }
    }
}
