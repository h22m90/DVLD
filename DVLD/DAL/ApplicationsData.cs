using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ApplicationsData
    {
        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = "select * from ApplicationsList_View order by ApplicationDate desc";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                return dt;
            }
        }
        public static bool GetApplicationByID(int ApplicationID, ref int ApplicationPersonID,
            ref DateTime ApplicationDate, ref byte ApplicationTypeID, ref byte ApplicationStatus,
            ref DateTime LastStatusDate, ref float ApplicationFees, ref int CreatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = true;
                        ApplicationPersonID = (int)reader["ApplicationPersonID"];
                        ApplicationDate = (DateTime)reader["ApplicationDate"];
                        ApplicationTypeID = (byte)reader["ApplicationTypeID"];
                        ApplicationStatus = (byte)reader["ApplicationStatus"];
                        LastStatusDate = (DateTime)reader["LastStatusDate"];
                        ApplicationFees = (float)reader["ApplicationFees"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                    }
                }
            }
            return isFound;
        }
        public static bool GetApplicationByPersonID(int ApplicationPersonID, ref int ApplicationID,
            ref DateTime ApplicationDate, ref byte ApplicationTypeID,
            ref byte ApplicationStatus, ref DateTime LastStatusDate,
            ref float ApplicationFees, ref int CreatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM Applications WHERE ApplicationPersonID = @ApplicationPersonID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationPersonID", ApplicationPersonID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = true;
                        ApplicationID = (int)reader["ApplicationID"];
                        ApplicationDate = (DateTime)reader["ApplicationDate"];
                        ApplicationTypeID = (byte)reader["ApplicationTypeID"];
                        ApplicationStatus = (byte)reader["ApplicationStatus"];
                        LastStatusDate = (DateTime)reader["LastStatusDate"];
                        ApplicationFees = (float)reader["ApplicationFees"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                    }
                }
            }
            return isFound;
        }
        public static int AddNewApplication(int ApplicationPersonID,
            DateTime ApplicationDate, byte ApplicationTypeID, byte ApplicationStatus,
            DateTime LastStatusDate, float ApplicationFees, int CreatedByUserID)
        {
            int NewApplicationID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"INSERT INTO Applications ( 
                            ApplicantPersonID,ApplicationDate,ApplicationTypeID,
                            ApplicationStatus,LastStatusDate,
                            PaidFees,CreatedByUserID)
                             VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,
                                      @ApplicationStatus,@LastStatusDate,
                                      @PaidFees,   @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationPersonID", ApplicationPersonID);
                    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int newID))
                        {
                            NewApplicationID = newID;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex) as needed
                        NewApplicationID = -1; // Indicate failure
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return NewApplicationID;
            }
        }
        public static bool UpdateApplication(int ApplicationID, int ApplicationPersonID,
            DateTime ApplicationDate, byte ApplicationTypeID, byte ApplicationStatus,
            DateTime LastStatusDate, float ApplicationFees, int CreatedByUserID)
        {
            bool isUpdated = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"UPDATE Applications SET 
                                ApplicantPersonID = @ApplicantPersonID,
                                ApplicationDate = @ApplicationDate,
                                ApplicationTypeID = @ApplicationTypeID,
                                ApplicationStatus = @ApplicationStatus,
                                LastStatusDate = @LastStatusDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID
                                WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicationPersonID);
                    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0; // True if at least one row was updated
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex) as needed
                        isUpdated = false; // Indicate failure
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return isUpdated;
        }
        public static bool DeleteApplication(int ApplicationID)
        {
            bool isDeleted = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"DELETE FROM Applications WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0; // True if at least one row was deleted
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex) as needed
                        isDeleted = false; // Indicate failure
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return isDeleted;
        }
        public static bool IsApplicationExists(int ApplicationID)
        {
            bool exists = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT COUNT(1) FROM Applications WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        exists = count > 0; // True if at least one record exists
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex) as needed
                        exists = false; // Indicate failure
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return exists;
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID, byte ApplicationTypeID)
        {
            return getActiveApplicationID(PersonID, ApplicationTypeID) != -1;
        }
        public static int getActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int activeApplicationID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT ApplicationID FROM Applications 
                                 WHERE ApplicantPersonID = @PersonID 
                                 AND ApplicationTypeID = @ApplicationTypeID 
                                 AND ApplicationStatus = 1"; // Assuming 1 is the status for 'Active'
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int applicationID))
                        {
                            activeApplicationID = applicationID;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex) as needed
                        activeApplicationID = -1; // Indicate failure
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return activeApplicationID;
        }
        public static int getActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int activeApplicationID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
                            From
                            Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            and ApplicationTypeID=@ApplicationTypeID 
							and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            and ApplicationStatus=1"; // Assuming 1 is the status for 'Active'
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int applicationID))
                        {
                            activeApplicationID = applicationID;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex) as needed
                        activeApplicationID = -1; // Indicate failure
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return activeApplicationID;
        }
        public static bool UpdateApplicationStatus(int ApplicationID, byte NewStatus)
        {
            bool isUpdated = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"UPDATE Applications SET 
                                ApplicationStatus = @NewStatus,
                                LastStatusDate = @LastStatusDate
                                WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@NewStatus", NewStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0; // True if at least one row was updated
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex) as needed
                        isUpdated = false; // Indicate failure
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return isUpdated;
        }
    }
}
