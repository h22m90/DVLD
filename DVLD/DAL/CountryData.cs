using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CountryData
    {
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Countries";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public static bool GetCountryNameByID(int CountryID, ref string CountryName)
        {
            bool isFound = false;
            using(SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Countries WHERE CountryID = @CountryID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    connection.Open();
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            isFound = true;
                            CountryName = (string)reader["CountryName"];
                        }
                    }
                }
            }
            return isFound;
        }
        public static bool GetCountryByName(string CountryName, ref int CountryID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Countries WHERE CountryName = @CountryName;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            CountryID = (int)reader["CountryID"];
                        }
                    }
                }
            }
            return isFound;
        }
    }
}
