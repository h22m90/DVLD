using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserData
    {
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT Users.UserID as 'User ID', People.PersonID as 'Person ID', Users.UserName as 'Username',
                                (People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName) as 'Fullname',
                                    CASE 
                                        WHEN People.Gender = 0 THEN 'Male'
                                        WHEN People.Gender = 1 THEN 'Female'
                                        ELSE 'Unknown'
                                    END AS Gender,
                                People.Phone, Countries.CountryName as 'Country', Users.IsActive as 'User State'
                                FROM     Users INNER JOIN
                                People ON Users.PersonID = People.PersonID INNER JOIN
                                Countries ON People.NationalityCountryID = Countries.CountryID;";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public static int AddNew(int PersonID, string UserName, string Password, bool IsActive)
        {
            int NewUserID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"INSERT INTO Users(PersonID, UserName, Password, IsActive) 
                                    VALUES(@PersonID, @UserName, @Password, @IsActive)
                                    SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    connection.Open();
                    /*
                     System.Data.SqlClient.SqlException: 
                    'The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Users_People".
                     The conflict occurred in database "DVLD", table "dbo.People", column 'PersonID'.
                     */
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        NewUserID = insertedID;
                    }
                }
            }
            return NewUserID;
        }
        public static bool Update(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            bool IsUpdated = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"UPDATE Users SET PersonID = @PersonID, UserName = @UserName, 
                                    Password = @Password, IsActive = @IsActive
                                    WHERE UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    IsUpdated = rowsAffected > 0;
                }
            }
            return IsUpdated;
        }
        public static bool Delete(int UserID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"DELETE FROM Users WHERE UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return rowsAffected > 0;
        }
        public static bool Delete(string UserName)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"DELETE FROM Users WHERE UserName = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return rowsAffected > 0;
        }
        public static bool GetUserDataByPersonID(int PersonID, ref int UserID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Users WHERE PersonID = @PersonID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                            UserName = reader.GetString(reader.GetOrdinal("UserName"));
                            Password = reader.GetString(reader.GetOrdinal("Password"));
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                        }
                    }
                }
            }
            return IsFound;
        }
        public static bool GetUserData(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Users WHERE UserID = @UserID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                            UserName = reader.GetString(reader.GetOrdinal("UserName"));
                            Password = reader.GetString(reader.GetOrdinal("Password"));
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                        }
                    }
                }
            }
            return IsFound;
        }
        public static bool GetUserData(string UserName, ref int PersonID, ref int UserID, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Users WHERE UserName = @UserName;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                            Password = reader.GetString(reader.GetOrdinal("Password"));
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                        }
                    }
                }
            }
            return IsFound;
        }
        public static bool GetUserData(string UserName, string Password, ref int PersonID, ref int UserID, ref bool IsActive)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                        }
                    }
                }
            }
            return IsFound;
        }
        public static bool IsUserExists(int UserID)
        {
            bool IsExists = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT 1 FROM Users WHERE UserID = @UserID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    IsExists = (result != null);
                }
            }
            return IsExists;
        }
        public static bool IsUserExists(string UserName)
        {
            bool IsExists = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT 1 FROM Users WHERE UserName = @UserName;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    IsExists = (result != null);
                }
            }
            return IsExists;
        }
        public static bool IsUserExistsForPersonID(int PersonID)
        {
            bool IsExists = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT 1 FROM Users WHERE PersonID = @PersonID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    IsExists = (result != null);
                }
            }
            return IsExists;
        }
        public static bool ChangePassword(int UserID, string NewPassword)
        {
            bool IsUpdated = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@NewPassword", NewPassword);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    IsUpdated = rowsAffected > 0;
                }
            }
            return IsUpdated;
        }
    }
}
