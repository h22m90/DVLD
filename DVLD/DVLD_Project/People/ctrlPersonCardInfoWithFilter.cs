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
    public partial class ctrlPersonCardInfoWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int personId)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(personId);
            }
        }
        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get => _ShowAddPerson;
            set
            {
                _ShowAddPerson = value;
                btnAddNew.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }
        public ctrlPersonCardInfoWithFilter()
        {
            InitializeComponent();
        }

        public int PersonID
        {
            get { return ctrlPersonCardInfo1.PersonID; }
        }
        public Person SelectedPersonInfo
        {
            get { return ctrlPersonCardInfo1.SelectedPersonInfo; }
        }
        public void LoadPersonInfo(int personId)
        {
            cbSearchBy.SelectedIndex = 0;
            txtSearchByValue.Text = personId.ToString();
            _FindNow();
        }
        private void _FindNow() 
        { 
            switch (cbSearchBy.Text) 
            {
                case "Person ID":
                    ctrlPersonCardInfo1.LoadPersonInfo(int.Parse(txtSearchByValue.Text));
                    break; 
                case "National No":
                    ctrlPersonCardInfo1.LoadPersonInfo(txtSearchByValue.Text);
                    break;
                default:
                    break;
            }
            if (OnPersonSelected != null && FilterEnabled) 
                OnPersonSelected(ctrlPersonCardInfo1.PersonID);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Please correct the errors and try again.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FindNow();
        }
        private void cbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearchByValue.Text = "";
            txtSearchByValue.Focus();
        }

        private void ctrlPersonCardInfo1_Load(object sender, EventArgs e)
        {
            cbSearchBy.SelectedIndex = 0;
            cbSearchBy.Focus();
        }

        private void txtSearchByValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchByValue.Text))
            {
                errorProvider1.SetError(txtSearchByValue, "Please enter a value to search.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtSearchByValue, "");
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += DataBackEvent;
            frm.ShowDialog();
        }
        private void DataBackEvent(object Person, int PersonID)
        {
            cbSearchBy.SelectedIndex = 0;
            txtSearchByValue.Text = PersonID.ToString();
            ctrlPersonCardInfo1.LoadPersonInfo(PersonID);
        }
        public void FilterFocus()
        {
            txtSearchByValue.Focus();
        }

        private void txtSearchByValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbSearchBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }
    }
}