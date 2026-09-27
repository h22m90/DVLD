using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class User
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode; 
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Person PersonInfo;
        public User(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            PersonID = personID;
            PersonInfo = Person.Find(personID);
            UserName = userName;
            Password = password;
            IsActive = isActive;
            _Mode = enMode.Update;
        }
        public User()
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            IsActive = false;
            PersonInfo = null;
            _Mode = enMode.AddNew;
        }
        public static DataTable GetAllUsers()
        {
            return UserData.GetAllUsers();
        }
        public static bool DeleteUser(int UserID)
        {
            return UserData.Delete(UserID);
        }
        public static bool DeleteUser(string UserName)
        {
            return UserData.Delete(UserName);
        }
        public static bool isExist(int UserID)
        {
            return UserData.IsUserExists(UserID);
        }
        public static bool isExist(string UserName)
        {
            return UserData.IsUserExists(UserName);
        }
        public static bool IsUserExistsForPersonID(int PersonID)
        {
            return UserData.IsUserExistsForPersonID(PersonID);
        }
        private bool _AddNew()
        {
            UserID = UserData.AddNew(PersonID, UserName, Password, IsActive);
            if (UserID != -1)
            {
                _Mode = enMode.Update;
                return true;
            }
            return false;
        }
        private bool _Update()
        {
            return UserData.Update(UserID, PersonID, UserName, Password, IsActive);
        }
        public bool Save()
        {
            return _Mode == enMode.AddNew ? _AddNew() : _Update();
        }
        public static User Find(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;
            bool isFound = UserData.GetUserData(UserID, ref PersonID, ref UserName, ref Password, ref IsActive);
            if (isFound)
                return new User(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }
        public static User Find(string UserName)
        {
            int PersonID = -1, UserID = -1;
            string Password = "";
            bool IsActive = false;
            bool isFound = UserData.GetUserData(UserName, ref PersonID, ref UserID, ref Password, ref IsActive);
            if (isFound)
            {
                return new User(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }
        public static User Find(string UserName, string Password)
        {
            int PersonID = -1, UserID = -1;
            bool IsActive = false;
            bool isFound = UserData.GetUserData(UserName, Password, ref PersonID, ref UserID, ref IsActive);
            if (isFound)
            {
                return new User(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }
        public static User GetUserDataByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;
            bool isFound = UserData.GetUserDataByPersonID(PersonID, ref UserID, ref UserName, ref Password, ref IsActive);
            if (isFound)
                return new User(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }
        public static bool ChangePassword(int UserID, string NewPassword)
        {
            return UserData.ChangePassword(UserID, NewPassword);
        }
    }
}
