using DLL;
using DVLD_Project.Applications.ApplicationTypes;
using DVLD_Project.Applications.Local_Driving_License;
using DVLD_Project.GlobalClasses;
using DVLD_Project.Login;
using DVLD_Project.People;
using DVLD_Project.Tests;
using DVLD_Project.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmMain : Form
    {
        frmLogin _loginForm;
        public frmMain(frmLogin frm)
        {
            InitializeComponent();
            _loginForm = frm;
        }

        private void peopleListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPeopleList PplList = new frmPeopleList();
            PplList.ShowDialog();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson addPerson = new frmAddUpdatePerson();
            addPerson.ShowDialog();
        }

        private void usersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsersList usersList = new frmUsersList();
            usersList.ShowDialog();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser addNewUser = new frmAddUpdateUser();
            addNewUser.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            changePassword.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            userInfo.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            if (_loginForm != null)
            {
                _loginForm.Show();
                this.Hide();
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void manageApplicationsTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmApplicationsTypesList applicationsTypesList = new frmApplicationsTypesList();
            applicationsTypesList.ShowDialog();
        }

        private void manageTestsTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestTypeslist testTypeslist = new frmTestTypeslist();
            testTypeslist.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLDL_App_List ldlAppList = new frmLDL_App_List(); 
            ldlAppList.ShowDialog();
        }
    }
}
