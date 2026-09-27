using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TestTypesData
    {
        public static DataTable GetAllTestsList()
        {
            DataTable dt = new DataTable();
            using(SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT TestTypeID as 'Test ID',
                                TestTypeTitle as 'Title',
                                TestTypeDescription as 'Test Description',
                                TestTypeFees as 'Fees' FROM TestTypes;";
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public static bool GetTestTypeInfoByID(int TestTypeID,ref string TestTypeTitle,ref string TestTypeDescription,ref float TestTypeFees)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            TestTypeTitle = reader["TestTypeTitle"].ToString();
                            TestTypeDescription = reader["TestTypeDescription"].ToString();
                            TestTypeFees = Convert.ToSingle(reader["TestTypeFees"]);
                        }
                    }
                }
            }
            return IsFound;
        }
        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, float TestTypeFees)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"UPDATE TestTypes
                                SET TestTypeTitle = @TestTypeTitle,
                                    TestTypeDescription = @TestTypeDescription,
                                    TestTypeFees = @TestTypeFees
                                WHERE TestTypeID = @TestTypeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
                    command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
                    command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }
    }
}
