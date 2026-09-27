using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ApplicationsTypesData
    {
        public static DataTable GetAllApplicationsTypes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT ApplicationTypeID as 'ID',
                                        ApplicationTypeTitle as 'Application Name',
                                        ApplicationFees as 'Fees'
                                        FROM     ApplicationTypes";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public static bool GetApplicationTypeInfoByID(int ApplicationID,ref string ApplicationTitle,ref float ApplicationFees)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationID";
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    connection.Open();
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            IsFound = true;
                            ApplicationTitle = reader["ApplicationTypeTitle"].ToString();
                            ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                        }
                    }
                }
            }
            return IsFound;
        }
        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationFees)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"UPDATE ApplicationTypes
                        SET ApplicationTypeTitle = @ApplicationTypeTitle,
                        ApplicationFees = @ApplicationFees
                        WHERE ApplicationTypeID = @ApplicationTypeID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                    command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }
    }
}
