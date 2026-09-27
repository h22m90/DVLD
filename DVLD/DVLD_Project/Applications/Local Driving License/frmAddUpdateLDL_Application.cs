using DLL;
using DVLD_Project.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using DVLDApplication = DLL.Application;

namespace DVLD_Project.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLDL_Application : Form
    {
        enum enMode { Add = 0, Update = 1 }
        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        private LocalDrivingLicenseApplications _localDrivingLicenseApplications;
        public frmAddUpdateLDL_Application()
        {
            InitializeComponent();
            _Mode = enMode.Add;
        }
        public frmAddUpdateLDL_Application(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }
        private void _LoadClassesIntoComboBox()
        {
            DataTable dtLicenseClasses = LicenseClasses.GetAllLicenseClassesInfo();
            cbLicenseClasses.DataSource = dtLicenseClasses;
            cbLicenseClasses.DisplayMember = "ClassName";
        }
        private void _ResetDefaultValues()
        {
            _LoadClassesIntoComboBox();
            if(_Mode == enMode.Add)
            {
                lblScreenMode.Text = "New Local License Application";
                this.Text = lblScreenMode.Text;
                _localDrivingLicenseApplications = new LocalDrivingLicenseApplications();
                ctrlPersonCardInfoWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;
                cbLicenseClasses.SelectedIndex = 2;
                lblAppFees.Text = ApplicationsTypes.Find((int)DVLDApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblAppDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblScreenMode.Text = "Update Local License Application";
                this.Text = lblScreenMode.Text;
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true; 
            }
        }
        private void _LoadData()
        {
            ctrlPersonCardInfoWithFilter1.FilterEnabled = false;
            _localDrivingLicenseApplications = LocalDrivingLicenseApplications.FindBy_LDL_ID(_LocalDrivingLicenseApplicationID);
            if(_localDrivingLicenseApplications == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            ctrlPersonCardInfoWithFilter1.LoadPersonInfo(_localDrivingLicenseApplications.ApplicantPersonID);
            lblLDL_ID.Text = _localDrivingLicenseApplications.LDL_App_ID.ToString();
            lblAppDate.Text = _localDrivingLicenseApplications.ApplicationDate.ToString();
            cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(LicenseClasses.Find(_localDrivingLicenseApplications.LicenseClassID).ClassName);
            lblAppFees.Text = _localDrivingLicenseApplications.PaidFees.ToString();
            lblCreatedBy.Text = User.Find(_localDrivingLicenseApplications.CreatedByUserID).UserName;
        }
        private void DataBackEvent(object sender, int PersonID)
        {
            _SelectedPersonID = PersonID;
            ctrlPersonCardInfoWithFilter1.LoadPersonInfo(PersonID);
        }
        private void frmAddUpdateLDL_Application_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }
            if(ctrlPersonCardInfoWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardInfoWithFilter1.FilterFocus();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            tcApplicationInfo.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int LicenseClassID = LicenseClasses.Find(cbLicenseClasses.Text).ClassID;
            int ActiveApplicationID = DVLDApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, DVLDApplication.enApplicationType.NewDrivingLicense,LicenseClassID);
            if(ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClasses.Focus();
                return;
            }
            //Create clsLicenseData & clsLicense classes tmrw...............
        }
    }
}
