using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PersonData
    {
        public static DataTable GetAllPPL()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT 
                                    People.PersonID, 
                                    People.NationalNo,
                                    People.FirstName, 
                                    People.SecondName, 
                                    People.ThirdName,
                                    People.LastName, 
                                    People.DateOfBirth, 
                                    CASE 
                                        WHEN People.Gender = 0 THEN 'Male'
                                        WHEN People.Gender = 1 THEN 'Female'
                                        ELSE 'Unknown'
                                    END AS Gender,
                                    Countries.CountryName, 
                                    People.Address,
                                    People.Phone, 
                                    People.Email, 
                                    People.ImagePath
                                FROM People
                                INNER JOIN Countries 
                                    ON People.NationalityCountryID = Countries.CountryID;";
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
        public static int AddNewPerson(string NationalNo, string Firstname, string Secondname, string Thirdname, string Lastname,
            DateTime DOB, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int NewPersonID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"INSERT INTO People
                                (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                                    Gender, Address, Phone, Email, NationalityCountryID, ImagePath)
                                VALUES
                                (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth,
                                    @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath)
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", Firstname);
                    command.Parameters.AddWithValue("@SecondName", Secondname);
                    command.Parameters.AddWithValue("@ThirdName", Thirdname);
                    command.Parameters.AddWithValue("@LastName", Lastname);
                    command.Parameters.AddWithValue("@DateOfBirth", DOB);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath", (object)ImagePath ?? DBNull.Value);
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                        NewPersonID = (int)result;
                }
            }
            return NewPersonID;
        }
        public static bool UpdatePerson(int PersonID, string NationalNo, string Firstname, string Secondname,
            string Thirdname, string Lastname, DateTime DOB, byte Gender, string Address, string Phone,
            string Email, int NationalityCountryID, string ImagePath)
        {
            int affectedRows = 0;
            using(SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"UPDATE People
                                    SET NationalNo = @NationalNo,
                                        FirstName = @Firstname,
                                        SecondName = @Secondname,
                                        ThirdName = @Thirdname,
                                        LastName = @Lastname,
                                        DateOfBirth = @DOB,
                                        Gender = @Gender,
                                        Address = @Address,
                                        Phone = @Phone,
                                        Email = @Email,
                                        NationalityCountryID = @NationalityCountryID,
                                        ImagePath = @ImagePath
                                        WHERE PersonID = @PersonID";
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@Firstname", Firstname);
                    command.Parameters.AddWithValue("@Secondname", Secondname);
                    command.Parameters.AddWithValue("@Thirdname", (object)Thirdname ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Lastname", Lastname);
                    command.Parameters.AddWithValue("@DOB", DOB);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", (object)Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath", (object)ImagePath ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
            }
            return affectedRows > 0;
        }
        public static bool DeletePerson(int PersonID)
        {
            int affectedRows = 0;
            using(SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"DELETE People
                                WHERE PersonID = @PersonID";
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
            }
            return affectedRows > 0;
        }
        public static bool GetPersonByPersonID(
                                                int PersonID,
                                                ref string NationalNo,
                                                ref string Firstname,
                                                ref string Secondname,
                                                ref string Thirdname,
                                                ref string Lastname,
                                                ref DateTime DOB,
                                                ref byte Gender,
                                                ref string Address,
                                                ref string Phone,
                                                ref string Email,
                                                ref int NationalityCountryID,
                                                ref string ImagePath)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT 
                            PersonID,
                            NationalNo,
                            FirstName,
                            SecondName,
                            ThirdName,
                            LastName,
                            DateOfBirth,
                            Gender,
                            Address,
                            Phone,
                            Email,
                            NationalityCountryID,
                            ImagePath
                         FROM People
                         WHERE PersonID = @PersonID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            NationalNo = reader["NationalNo"].ToString();
                            Firstname = reader["FirstName"].ToString();
                            Secondname = reader["SecondName"].ToString();
                            Thirdname = reader["ThirdName"] == DBNull.Value
                                        ? null
                                        : reader["ThirdName"].ToString();
                            Lastname = reader["LastName"].ToString();
                            DOB = (DateTime)reader["DateOfBirth"];
                            Gender = (byte)reader["Gender"];
                            Address = reader["Address"].ToString();
                            Phone = reader["Phone"].ToString();
                            Email = reader["Email"] == DBNull.Value
                                    ? null
                                    : reader["Email"].ToString();
                            NationalityCountryID = (int)reader["NationalityCountryID"];
                            ImagePath = reader["ImagePath"] == DBNull.Value
                                        ? null
                                        : reader["ImagePath"].ToString();
                        }
                    }
                }
            }
            return isFound;
        }
        public static bool GetPersonByNationalNo(
                                                string NationalNo,
                                                ref int PersonID,
                                                ref string Firstname,
                                                ref string Secondname,
                                                ref string Thirdname,
                                                ref string Lastname,
                                                ref DateTime DOB,
                                                ref byte Gender,
                                                ref string Address,
                                                ref string Phone,
                                                ref string Email,
                                                ref int NationalityCountryID,
                                                ref string ImagePath)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT
                            PersonID,
                            NationalNo,
                            FirstName,
                            SecondName,
                            ThirdName,
                            LastName,
                            DateOfBirth,
                            Gender,
                            Address,
                            Phone,
                            Email,
                            NationalityCountryID,
                            ImagePath
                         FROM People
                         WHERE NationalNo = @NationalNo;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            PersonID = (int)reader["PersonID"];
                            Firstname = reader["FirstName"].ToString();
                            Secondname = reader["SecondName"].ToString();

                            Thirdname = reader["ThirdName"] == DBNull.Value
                                        ? null
                                        : reader["ThirdName"].ToString();

                            Lastname = reader["LastName"].ToString();
                            DOB = (DateTime)reader["DateOfBirth"];
                            Gender = (byte)reader["Gender"];
                            Address = reader["Address"].ToString();
                            Phone = reader["Phone"].ToString();

                            Email = reader["Email"] == DBNull.Value
                                    ? null
                                    : reader["Email"].ToString();

                            NationalityCountryID = (int)reader["NationalityCountryID"];

                            ImagePath = reader["ImagePath"] == DBNull.Value
                                        ? null
                                        : reader["ImagePath"].ToString();
                        }
                    }
                }
            }
            return isFound;
        }
        public static bool IsPersonExists(int personID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT 1 FROM People WHERE PersonID = @PersonID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    isFound = (result != null);
                }
            }
            return isFound;
        }
        public static bool IsPersonExists(string NationalNo)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT 1 FROM People WHERE NationalNo = @NationalNo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    isFound = (result != null);
                }
            }
            return isFound;
        }
    }
}