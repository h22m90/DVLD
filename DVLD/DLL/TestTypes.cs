using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class TestTypes
    {
        enum enMode { Add = 0, Update = 1 };
        enMode _mode = enMode.Add;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        public TestTypes.enTestType TestTypeID { set; get; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }
        public TestTypes()
        {
            TestTypeID = TestTypes.enTestType.VisionTest;
            TestTypeTitle = string.Empty;
            TestTypeDescription = string.Empty;
            TestTypeFees = 0;
            _mode = enMode.Add;
        }
        public TestTypes(TestTypes.enTestType TestTypeID, string TestTypeTitle, string TestTypeDescription, float TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
            _mode = enMode.Update;
        }
        public static DataTable GetAllTestList()
        {
            return TestTypesData.GetAllTestsList();
        }
        public static TestTypes Find(TestTypes.enTestType TestTypeID)
        {
            string TestTypeTitle = string.Empty;
            string TestTypeDescription = string.Empty;
            float TestTypeFees = 0;
            if (TestTypesData.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
                return new TestTypes(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            else
                return null;
        }
        private bool _UpdateTestType()
        {
            return TestTypesData.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }
        public bool Save()
        {
            return _UpdateTestType();
        }
    }
}
