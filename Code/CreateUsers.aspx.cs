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

                    string stm = @"SELECT FirstName, LastName, `E-mail`, " + UserID + "_Arch.Type, Parent, " + UserID + "_Users.ID From User INNER JOIN " + UserID + "_Users ON User.CustomerNumber=" + UserID + "_Users.ID INNER JOIN " + UserID + "_Arch ON " +
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
                result += "\"Parent\":" + "\"" + (rdr["Parent"] == DBNull.Value ? "No Parent" : rdr["Parent"]) + "\"},";
            }

            result = result.Substring(0, result.Length - 1);
            result += "]}";

            return infoInside;
        }

        [WebMethod]
        public static String GetHierarchy ()
        {
            Create();
            return CreateTable.GetReadyTable(false);
        }

        [WebMethod]
        public static String GetParents ()
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

                    string stm = @"SELECT ID FROM " + UserID + "_Users";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (!CreateJSON_IDS(ref result, rdr))
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

        private static bool CreateJSON_IDS (ref String result, MySqlDataReader rdr)
        {
            result = "{\"result\": [";
            bool infoInside = false;

            while (rdr.Read())
            {
                infoInside = true;
                result += "{\"ID\":" + "\"" + rdr.GetString(0) + "\"},";
            }

            result = result.Substring(0, result.Length - 1);
            result += "]}";

            return infoInside;
        }

        [WebMethod]
        public static int CreateUser (String Username, String FirstName, String LastName, String Email, String Type, String Parent)
        {
            int UserID = Subscription.GetBossID();

            if (!Registration.RegisterCreateUser(Email, Forgot.GetPassword(), Username, FirstName, LastName, UserID))
                return -1;

            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"INSERT INTO " + UserID + "_Users(ID,Type,Parent,CreatedBy,Date,Time) VALUES (@id,@type,@parent,@by,@date,@time)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", GetID(Username));
                    cmd.Parameters.AddWithValue("@type", Type);
                    cmd.Parameters.AddWithValue("@parent", Parent);
                    cmd.Parameters.AddWithValue("@by", UserID);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("M/d/yyyy"));
                    cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("h:mm:ss tt"));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }

        private static int GetID (String Username)
        {
            int ID;
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";;

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT CustomerNumber From User WHERE UserName=@Name";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", Username);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (!rdr.HasRows)
                            return -2;

                        ID = rdr.GetInt32(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }

            return ID;
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

        private static bool Create()
        {
            int UserID = Subscription.GetBossID();

            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + UserID + "_Users (ID int NOT NULL,Type int, Parent int, CreatedBy int, Date char(100), Time char(100),PRIMARY KEY (ID))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
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
    }
}