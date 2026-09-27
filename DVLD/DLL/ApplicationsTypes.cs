using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace DLL
{
    public class ApplicationsTypes
    {
        private enum enMode {Add = 0, Update = 1};
        private enMode _mode;
        public string Title { get; set; }
        public float Fees { get; set; }
        public int ApplicationID { get; set; }
        public ApplicationsTypes()
        {
            this.ApplicationID = -1;
            this.Title = string.Empty;
            this.Fees = 0;
            this._mode = enMode.Add;
        }
        public ApplicationsTypes(int ApplicationID, string ApplicationTitle, float ApplicationFees)
        {
            this.ApplicationID = ApplicationID;
            this.Title = ApplicationTitle;
            this.Fees = ApplicationFees;
            this._mode = enMode.Update;
        }
        public static DataTable GetAllApplicationsTypes()
        {
            return ApplicationsTypesData.GetAllApplicationsTypes();
        }
        public static ApplicationsTypes Find(int ID)
        {
            string Title = "";
            float Fees = 0;
            if(ApplicationsTypesData.GetApplicationTypeInfoByID(ID,ref Title,ref Fees))
                return new ApplicationsTypes(ID, Title, Fees);
            else
                return null;
        }
        private bool _UpdateApplicationType()
        {
            return ApplicationsTypesData.UpdateApplicationType(this.ApplicationID, this.Title, this.Fees);
        }
        public bool Save()
        {
            return _UpdateApplicationType();
        }
    }
}
