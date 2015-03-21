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

        [WebMethod]
        public static bool ActivateToggle (String UserID)
        {
            return Database.ActiveToggle(UserID);
        }

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
                            return " ";
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            return result;
        }

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

        [WebMethod]
        public static bool Delete (String ID)
        {
            if (!DeleteFromCustomerTable(ID))
                return false;

            return DeleteFromUserTable(ID);
        }

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

        [WebMethod]
        public static String GetHierarchy ()
        {
            Database.CreateUsersTable(Subscription.GetID());
            return CreateTable.GetReadyTable(false);
        }

        [WebMethod]
        public static String GetParents ()
        {
            return Database.GetParentsJSON(Subscription.GetID().ToString());
        }

        [WebMethod]
        public static String CreateUser (String Username, String FirstName, String LastName, String Email, String Type, String Parent, String Phone, String EmployeeID, String City, String State, String Title)
        {
            return Database.CreateUser(Username, FirstName, LastName, Email, Type, Parent, Phone, EmployeeID, City, State, Title);
        }   

        [WebMethod]
        public static bool HasBoss ()
        {
            if (Subscription.GetBossID() == -2)
                return false;

            if (!CanCreate() && !Subscription.IsSubscribed())
                return false;

            return true;
        }

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