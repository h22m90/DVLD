using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class Person
    {
        public enum enMode { AddNew =  0, Update = 1 }
        public enMode _Mode;
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
            }
        }
        public DateTime DOB { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public Country CountryInfo;
        public string ImagePath { get; set; }
        public Person(int personID, string nationalNo, string firstName, string secondName, string thirdName, string lastName, DateTime dOB,
                        byte gender, string address, string phone, string email, int nationalityCountryID, string imagePath)
        {
            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DOB = dOB;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            NationalityCountryID = nationalityCountryID;
            CountryInfo = Country.Find(nationalityCountryID);
            ImagePath = imagePath;
            _Mode = enMode.Update;
        }
        public Person()
        {
            PersonID = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DOB = DateTime.Now;
            Gender = 0;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            CountryInfo = Country.Find(NationalityCountryID);
            ImagePath = "";
            _Mode = enMode.AddNew;
        }
        public static DataTable GetAllPPL()
        {
            return PersonData.GetAllPPL();
        }
        public static bool DeletePerson(int PersonID)
        {
            return PersonData.DeletePerson(PersonID);
        }
        public static bool IsExist(int PersonID)
        {
            return PersonData.IsPersonExists(PersonID);
        }
        public static bool IsExist(string NationalityNo)
        {
            return PersonData.IsPersonExists(NationalityNo);
        }
        public static Person Find(int PersonID)
        {
            string NationalNo = "", Firstname = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            int NationalityCountryID = 0;
            byte gender = 0;
            DateTime DOB = DateTime.Now;
            bool isFound = PersonData.GetPersonByPersonID(PersonID, ref NationalNo, ref Firstname, ref SecondName, ref ThirdName,
                ref LastName, ref DOB, ref gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);
            if (isFound)
                return new Person(PersonID, NationalNo, Firstname, SecondName, ThirdName, LastName, DOB, gender, Address, Phone, Email,NationalityCountryID, ImagePath);
            else
                return null;
        }
        public static Person Find(string NationalNo)
        {
            string Firstname = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            int NationalityCountryID = 0;
            int PersonID = -1;
            byte gender = 0;
            DateTime DOB = DateTime.Now;
            bool isFound = PersonData.GetPersonByNationalNo(NationalNo, ref PersonID, ref Firstname, ref SecondName, ref ThirdName,
                ref LastName, ref DOB, ref gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);
            if (isFound)
                return new Person(PersonID, NationalNo, Firstname, SecondName, ThirdName, LastName, DOB, gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }
        private bool _AddNew()
        {
            PersonID = PersonData.AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName, DOB, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            if (PersonID != -1)
            {
                _Mode = enMode.Update;
                return true;
            }
            return false;
        }
        private bool _UpdatePerson()
        {
            return PersonData.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DOB, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        }
        public bool Save()
        {
            return _Mode == enMode.AddNew ? _AddNew() : _UpdatePerson();
        }
    }
}
