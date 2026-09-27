using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLL
{
    public class Application
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public enMode Mode = enMode.AddNew;
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };
        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public Person PersonInfo;
        public string AplicantFullname
        {
            get
            {
                return Person.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { set; get; }
        public byte ApplicationTypeID { set; get; }
        public ApplicationsTypes ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { set; get; }
        public string StatusText
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public User CreatedByUserInfo;
        public Application()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = 0;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }

        private Application(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, byte ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.PersonInfo = Person.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = ApplicationsTypes.Find(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = User.Find(CreatedByUserID);
            Mode = enMode.Update;
        }
        private bool _AddNewApplication()
        {
            this.ApplicationID = ApplicationsData.AddNewApplication(
            this.ApplicantPersonID, this.ApplicationDate,
            this.ApplicationTypeID, (byte)this.ApplicationStatus,
            this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID != -1);
        }
        private bool _UpdateApplication()
        {
            return ApplicationsData.UpdateApplication(ApplicationID, ApplicantPersonID, ApplicationDate,
                ApplicationTypeID, (byte)ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
        }
        public bool Save()
        {
            return Mode == enMode.AddNew ? _AddNewApplication(): _UpdateApplication();
        }
        public bool Delete()
        {
            return ApplicationsData.DeleteApplication(this.ApplicationID);
        }
        public static Application FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            DateTime ApplicationDate = DateTime.Now; byte ApplicationTypeID = 0;
            byte ApplicationStatus = 1; DateTime LastStatusDate = DateTime.Now;
            float PaidFees = 0; int CreatedByUserID = -1;
            bool IsFound = ApplicationsData.GetApplicationByID
                    (
                        ApplicationID, ref ApplicantPersonID,
                        ref ApplicationDate, ref ApplicationTypeID,
                        ref ApplicationStatus, ref LastStatusDate,
                        ref PaidFees, ref CreatedByUserID
                    );
            if (IsFound)
                return new Application(ApplicationID, ApplicantPersonID,
                     ApplicationDate, ApplicationTypeID,
                    (enApplicationStatus)ApplicationStatus, LastStatusDate,
                     PaidFees, CreatedByUserID);
            else
                return null;
        }
        public bool ChangeApplicationStatus(byte NewStatus)
        {
            return ApplicationsData.UpdateApplicationStatus(this.ApplicationID, NewStatus);
        }
        public static bool IsApplicationExist(int ApplicationID)
        {
            return ApplicationsData.IsApplicationExists(ApplicationID);
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID, byte ApplicationTypeID)
        {
            return ApplicationsData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }
        public bool DoesPersonHaveActiveApplication(byte ApplicationTypeID)
        {
            return DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }
        public static int GetActiveApplicationID(int PersonID, Application.enApplicationType ApplicationTypeID)
        {
            return ApplicationsData.getActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }
        public static int GetActiveApplicationIDForLicenseClass(int PersonID, Application.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return ApplicationsData.getActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }
        public int GetActiveApplicationID(Application.enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }
    }
}
