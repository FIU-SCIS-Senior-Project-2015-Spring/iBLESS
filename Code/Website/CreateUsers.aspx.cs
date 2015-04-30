using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace WebApplication1
{
    public partial class CreateUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Toggles user status
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool ActivateToggle (String UserID)
        {
            return Database.ActiveToggle(UserID);
        }

        /// <summary>
        /// Gets all user information in order to populate the modify user pop up box
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetInfo(String ID)
        {
            return Database.GetInformationForCreateUsers(ID);
        }

        /// <summary>
        /// Gets all available partes for this user
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetParentsInfo(String ID)
        {
            return Database.GetParentsJSON(ID);
        }

        /// <summary>
        /// Updates user information with the new given values
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="ID"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="Email"></param>
        /// <param name="City"></param>
        /// <param name="State"></param>
        /// <param name="Title"></param>
        /// <param name="Phone"></param>
        /// <param name="Type"></param>
        /// <param name="Parent"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [WebMethod]
        public static int UpdateInfo(String Username, String ID, String FirstName, String LastName, String Email, String City, String State, String Title, String Phone, String Type, String Parent, String EmployeeID)
        {
            return Database.UpdateEmployeeInfo(Username, FirstName, LastName, Email, Phone, EmployeeID, City, Title, State, Type, Parent, ID);
        }

        /// <summary>
        /// Creates a JSON out of the users under the company account of the user that is using the system
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static String PopulateTable ()
        {
            String result = "";
            int UserID = Subscription.GetBossID();

            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT FirstName, LastName, `E-mail`, " + UserID + "_Arch.Type, Parent, " + UserID + "_Users.ID, IsInactive From User INNER JOIN " + UserID + "_Users ON User.CustomerNumber=" + UserID + "_Users.ID INNER JOIN " + UserID + "_Arch ON " +
                        UserID + "_Arch.ID=" + UserID + "_Users.Type";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (!CreateJSON(ref result, rdr))
                            return " "; // no result
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            return result;
        }

        /// <summary>
        /// Deletes user with id = ID from the user table of the company account that is currently using the system
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private static bool DeleteFromUserTable (String ID)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"DELETE FROM User WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes the user from the basic user table
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private static bool DeleteFromCustomerTable (String ID)
        {
            int UserID = Subscription.GetBossID();
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"DELETE FROM " + UserID + "_Users WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete user with id = ID from the system
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool Delete (String ID)
        {
            if (!DeleteFromCustomerTable(ID))
                return false;

            return DeleteFromUserTable(ID);
        }

        /// <summary>
        /// Creates a JSON value. For future JSON creationg please use JSONConvert.Serialize
        /// </summary>
        /// <param name="result"></param>
        /// <param name="rdr"></param>
        /// <returns></returns>
        private static bool CreateJSON (ref String result, MySqlDataReader rdr)
        {
            result = "{\"result\": [";
            bool infoInside = false;

            while (rdr.Read())
            {
                infoInside = true;
                result += "{\"Name\":" + "\"" + rdr.GetString(0) + " " + rdr.GetString(1) + "\", ";
                result += "\"Type\":" + "\"" + rdr.GetString(3) + "\", ";
                result += "\"ID\":" + "\"" + rdr.GetInt32(5) + "\", ";
                result += "\"Email\":" + "\"" + rdr.GetString(2) + "\", ";
                result += "\"Parent\":" + "\"" + (rdr["Parent"] == DBNull.Value ? "No Parent" : rdr["Parent"]) + "\", ";
                result += "\"IsInactive\":" + "\"" + rdr["IsInactive"] + "\"},";
            }

            result = result.Substring(0, result.Length - 1);
            result += "]}";

            return infoInside;
        }

        /// <summary>
        /// Gets roles available under the company account that is being used
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static String GetHierarchy ()
        {
            Database.CreateUsersTable(Subscription.GetID());
            return CreateTable.GetReadyTable(false);
        }

        /// <summary>
        /// Gets parents available under the company account
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static String GetParents ()
        {
            return Database.GetParentsJSON(Subscription.GetID().ToString());
        }

        /// <summary>
        /// Creates a new user under the company account
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="Email"></param>
        /// <param name="Type"></param>
        /// <param name="Parent"></param>
        /// <param name="Phone"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="City"></param>
        /// <param name="State"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [WebMethod]
        public static String CreateUser (String Username, String FirstName, String LastName, String Email, String Type, String Parent, String Phone, String EmployeeID, String City, String State, String Title)
        {
            return Database.CreateUser(Username, FirstName, LastName, Email, Type, Parent, Phone, EmployeeID, City, State, Title);
        }   

        /// <summary>
        /// Checks if user is boss or has boss
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static bool HasBoss ()
        {
            if (Subscription.GetBossID() == -2)
                return false;

            if (!CanCreate() && !Subscription.IsSubscribed())
                return false;

            return true;
        }

        /// <summary>
        /// Checks if user has been given permissions to create new users
        /// </summary>
        /// <returns></returns>
        private static bool CanCreate ()
        {
            String CanCreate;

            try { CanCreate = Subscription.GetCookieValue("CanCreate"); }
            catch (HttpException ex) { return false; }

            if (CanCreate.Equals("false"))
                return false;

            return true;
        }
    }
}