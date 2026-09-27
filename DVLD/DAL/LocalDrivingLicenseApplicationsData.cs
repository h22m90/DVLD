using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LocalDrivingLicenseApplicationsData
    {
        public static DataTable GetAllL_D_L_Applications()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT * FROM LocalDrivingLicenseApplications_View 
                                    order by ApplicationDate Desc";
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
        public static int AddNewLocalLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int NewL_LicenseApplicationID = -1;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"INSERT INTO LocalDrivingLicenseApplications ( 
                            ApplicationID,LicenseClassID)
                             VALUES (@ApplicationID,@LicenseClassID);
                             SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        NewL_LicenseApplicationID = insertedID;
                    }
                }
            }
            return NewL_LicenseApplicationID;
        }
        public static bool UpdateLocalLicenseApplication(int ApplicationID, int LicenseClassID, int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"Update  LocalDrivingLicenseApplications  
                            set ApplicationID = @ApplicationID,
                                LicenseClassID = @LicenseClassID
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }
        public static bool DeleteLocalLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"Delete from LocalDrivingLicenseApplications  
                                 where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }
        public static bool Get_LDL_By_LicenseID(int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT ApplicationID, LicenseClassID 
                                 FROM LocalDrivingLicenseApplications
                                 WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            ApplicationID = (int)reader["ApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                        }
                    }
                }
                return isFound;
            }
        }
        public static bool Get_LDL_By_ApplicationID(int ApplicationID, ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT LocalDrivingLicenseApplicationID, LicenseClassID 
                                 FROM LocalDrivingLicenseApplications
                                 WHERE ApplicationID = @ApplicationID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("ApplicationID", ApplicationID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                        }
                    }
                }
                return isFound;
            }
        }

        public static bool DidPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT top 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("TestTypeID", TestTypeID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && bool.TryParse(result.ToString(), out bool testResult))
                    {
                        Result = testResult;
                    }
                }
                return Result;
            }

        }
        public static bool DidAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("TestTypeID", TestTypeID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int found))
                    {
                        Result = found == 1;
                    }
                }
            }
            return Result;
        }
        public static bool TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID, ref int TotalTrials)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT TotalTrialsPerTest = count(TestID)
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("TestTypeID", TestTypeID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int totalTrials))
                    {
                        TotalTrials = totalTrials;
                        isFound = true;
                    }
                }
            }
            return isFound;
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectionStr))
            {
                string query = @"SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                            AND(TestAppointments.TestTypeID = @TestTypeID) and isLocked=0
                            ORDER BY TestAppointments.TestAppointmentID desc";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("TestTypeID", TestTypeID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int found))
                    {
                        Result = found == 1;
                    }
                }
            }
            return Result;
        }
    }
}