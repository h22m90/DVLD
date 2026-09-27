using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LicenseData
    {
        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Licenses";
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
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
        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass,
                                        DateTime IssueDate, DateTime ExpirationDate, string Notes,
                                        float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr);
            string query = @"
                              INSERT INTO Licenses
                               (ApplicationID,
                                DriverID,
                                LicenseClass,
                                IssueDate,
                                ExpirationDate,
                                Notes,
                                PaidFees,
                                IsActive,IssueReason,
                                CreatedByUserID)
                         VALUES
                               (
                               @ApplicationID,
                               @DriverID,
                               @LicenseClass,
                               @IssueDate,
                               @ExpirationDate,
                               @Notes,
                               @PaidFees,
                               @IsActive,@IssueReason, 
                               @CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);

            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            if (Notes == "")
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out int insertedID))
            {
                LicenseID = insertedID;
            }
            return LicenseID;
        }
        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
                                 DateTime IssueDate, DateTime ExpirationDate, string Notes,
                                 float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"
                            UPDATE Licenses
                            SET ApplicationID   = @ApplicationID,
                                DriverID        = @DriverID,
                                LicenseClass    = @LicenseClass,
                                IssueDate       = @IssueDate,
                                ExpirationDate  = @ExpirationDate,
                                Notes           = @Notes,
                                PaidFees        = @PaidFees,
                                IsActive        = @IsActive,
                                IssueReason     = @IssueReason,
                                CreatedByUserID = @CreatedByUserID
                            WHERE LicenseID     = @LicenseID";

            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                if (string.IsNullOrEmpty(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return (rowsAffected > 0);
        }
        public static bool GetLicenseByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
                                 ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
                                 ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT ApplicationID, DriverID, LicenseClass, IssueDate, 
                            ExpirationDate, Notes, PaidFees, IsActive, 
                            IssueReason, CreatedByUserID 
                         FROM Licenses 
                         WHERE LicenseID = @LicenseID";

            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Record found
                            isFound = true;

                            ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                            DriverID = Convert.ToInt32(reader["DriverID"]);
                            LicenseClass = Convert.ToInt32(reader["LicenseClass"]);
                            IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                            ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);

                            // Handle nullable Notes column cleanly
                            Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : string.Empty;

                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            IsActive = Convert.ToBoolean(reader["IsActive"]);
                            IssueReason = Convert.ToByte(reader["IssueReason"]);
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT     
                        Licenses.LicenseID,
                        ApplicationID,
                        LicenseClasses.ClassName, 
                        Licenses.IssueDate, 
                        Licenses.ExpirationDate, 
                        Licenses.IsActive
                    FROM Licenses 
                    INNER JOIN LicenseClasses 
                        ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                    WHERE DriverID = @DriverID
                    ORDER BY IsActive DESC, ExpirationDate DESC";
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", DriverID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return dt;
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            string query = @"SELECT Licenses.LicenseID
                    FROM Licenses 
                    INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID
                    WHERE Licenses.LicenseClass = @LicenseClass 
                      AND Drivers.PersonID = @PersonID
                      AND Licenses.IsActive = 1;";

            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int foundID))
                    {
                        LicenseID = foundID;
                    }
                }
                catch (Exception ex)
                {
                    // Log exception details here if needed
                    LicenseID = -1;
                }
            }

            return LicenseID;
        }
        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Licenses
                     SET IsActive = 0
                     WHERE LicenseID = @LicenseID";

            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log exception details here if needed
                    rowsAffected = 0;
                }
            }

            return (rowsAffected > 0);
        }
    }
}