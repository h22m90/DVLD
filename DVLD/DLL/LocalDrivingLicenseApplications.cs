using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class LocalDrivingLicenseApplications : Application
    {
        public int LDL_App_ID { get; set; }
        public int LicenseClassID { set; get; }
        private LicenseClasses _LicenseClassInfo;
        public string FullName
        {
            get
            {
                return base.PersonInfo.FullName;
            }
        }
        public enum enMode { Add = 0, Update = 1 }
        public enMode _Mode = enMode.Add;
        public LocalDrivingLicenseApplications()
        {
            LDL_App_ID = -1;
            ApplicationID = -1;
            _Mode = enMode.Add;
        }
        private LocalDrivingLicenseApplications(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, byte ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID, int LicenseClassID)
        {
            this.LDL_App_ID = LocalDrivingLicenseApplicationID; ;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this._LicenseClassInfo = LicenseClasses.Find(LicenseClassID);
            _Mode = enMode.Update;
        }
        public static DataTable GetAllL_D_L_Applications()
        {
            return LocalDrivingLicenseApplicationsData.GetAllL_D_L_Applications();
        }
        private bool _AddNewLDL()
        {
            this.LDL_App_ID = LocalDrivingLicenseApplicationsData.AddNewLocalLicenseApplication(
                this.ApplicationID, this.LicenseClassID);
            return (this.LDL_App_ID != -1);
        }
        private bool _UpadeteLDL()
        {
            return LocalDrivingLicenseApplicationsData.UpdateLocalLicenseApplication(this.ApplicationID,
                this.LicenseClassID,this.LDL_App_ID);
        }
        public new bool Save()
        {
            base.Mode = (Application.enMode)_Mode;
            if (!base.Save())
                return false;
            return _Mode == enMode.Add ? _AddNewLDL() : _UpadeteLDL();
        }
        public new bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;
            //First we delete the Local Driving License Application
            IsLocalDrivingApplicationDeleted = LocalDrivingLicenseApplicationsData.DeleteLocalLicenseApplication(this.LDL_App_ID);

            if (!IsLocalDrivingApplicationDeleted)
                return false;
            //Then we delete the base Application
            IsBaseApplicationDeleted = base.Delete();
            return IsBaseApplicationDeleted;
        }
        public bool DoesPasstestType(TestTypes.enTestType TestTypeID)
        {
            return LocalDrivingLicenseApplicationsData.DidPassTestType(LDL_App_ID, (int)TestTypeID);
        }
        public bool DoesPassPreviousTest(TestTypes.enTestType CurrentTest)
        {
            switch(CurrentTest)
            {
                case TestTypes.enTestType.VisionTest: return true;
                case TestTypes.enTestType.WrittenTest:
                    return this.DoesPasstestType(TestTypes.enTestType.VisionTest);
                case TestTypes.enTestType.StreetTest:
                    return this.DoesPasstestType(TestTypes.enTestType.WrittenTest);
                default: return false;
            }
        }
        public static bool DoesPassTestType(int LDL_App_ID, TestTypes.enTestType TestTypeID)
        {
            return LocalDrivingLicenseApplicationsData.DidPassTestType(LDL_App_ID, (int)TestTypeID);
        }
        public static LocalDrivingLicenseApplications FindBy_LDL_ID(int LDL_App_ID)
        {
            int ApplicationID = -1, LicenseClassID = -1;
            bool isFound = LocalDrivingLicenseApplicationsData.Get_LDL_By_LicenseID(LDL_App_ID, ref ApplicationID, ref LicenseClassID);
            if (isFound)
            {
                Application BaseApplication = Application.FindBaseApplication(ApplicationID);
                return new LocalDrivingLicenseApplications(LDL_App_ID, BaseApplication.ApplicationID,
                    BaseApplication.ApplicantPersonID,
                                     BaseApplication.ApplicationDate, BaseApplication.ApplicationTypeID,
                                    (enApplicationStatus)BaseApplication.ApplicationStatus, BaseApplication.LastStatusDate,
                                     BaseApplication.PaidFees, BaseApplication.CreatedByUserID, LicenseClassID);
            }
            else
                return null;
        }
        public static LocalDrivingLicenseApplications FindByApplicationID(int ApplicationID)
        {
            int LDL_App_ID = -1, LicenseClassID = -1;
            bool isFound = LocalDrivingLicenseApplicationsData.Get_LDL_By_LicenseID(LDL_App_ID, ref ApplicationID, ref LicenseClassID);
            if (isFound)
            {
                Application BaseApplication = Application.FindBaseApplication(ApplicationID);
                return new LocalDrivingLicenseApplications(LDL_App_ID, BaseApplication.ApplicationID,
                    BaseApplication.ApplicantPersonID,
                                     BaseApplication.ApplicationDate, BaseApplication.ApplicationTypeID,
                                    (enApplicationStatus)BaseApplication.ApplicationStatus, BaseApplication.LastStatusDate,
                                     BaseApplication.PaidFees, BaseApplication.CreatedByUserID, LicenseClassID);
            }
            else
                return null;
        }

    }
}
