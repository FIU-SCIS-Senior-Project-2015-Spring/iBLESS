using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class CreateTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private static bool Create ()
        {
            int ID = Subscription.GetID();
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + ID + "_Arch(ID int NOT NULL,Type char(50) UNIQUE,Can_Create boolean,PRIMARY KEY (ID))";
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

        [WebMethod]
        public static bool CheckSubscription ()
        {
            return Subscription.IsSubscribed();
        }

        [WebMethod]
        public static String InsertAfter (int row_ID, bool CanCreate, String NewType)
        {
            int ID = Subscription.GetID();
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";
            string result = "";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"Select ID, Type, `Can_Create` FROM " + ID + "_Arch WHERE ID>@id ORDER BY ID DESC";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", row_ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        IncrementResults(rdr, ID);
                        Insert(NewType, CanCreate, row_ID + 1, ID);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            GetUsers(row_ID);
            result = GetReadyTable(false);
            return result;
        }

        private static bool Insert (String Type, bool CanCreate, int RowID, int TableID)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"INSERT INTO " + TableID + "_Arch (ID,Type,`Can_Create`) VALUES (@id,@type,@create)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", RowID);
                    cmd.Parameters.AddWithValue("@type", Type);
                    cmd.Parameters.AddWithValue("@create", CanCreate);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return false;
            }

            return true;
        }

        private static bool GetUsers (int row_ID)
        {
            int ID = Subscription.GetID();
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"Select Type, ID FROM " + ID + "_Users WHERE Type>@id ORDER BY Type DESC";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", row_ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        IncrementResultsUsers(rdr, ID);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private static bool IncrementResultsUsers (MySqlDataReader rdr, int ID)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    while (rdr.Read())
                    {
                        string commandText = "UPDATE " + ID + "_Users SET `Type`=@id WHERE ID=@currentID";
                        MySqlCommand cmd = new MySqlCommand(commandText, conn);

                        int RowID = rdr.GetInt32(0);
                        Debug.WriteLine(RowID);
                        cmd.Parameters.AddWithValue("@id", RowID + 1);
                        cmd.Parameters.AddWithValue("@currentID", rdr.GetString(1));

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private static bool IncrementResults (MySqlDataReader rdr, int ID)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    while (rdr.Read())
                    {
                        string commandText = "UPDATE " + ID + "_Arch SET `ID`=@id, Type=@type, `Can_Create`=@create WHERE ID=@currentID";
                        MySqlCommand cmd = new MySqlCommand(commandText, conn);

                        int RowID = rdr.GetInt32(0);

                        cmd.Parameters.AddWithValue("@id", RowID + 1);
                        cmd.Parameters.AddWithValue("@type", rdr.GetString(1));
                        cmd.Parameters.AddWithValue("@create", rdr.GetBoolean(2));
                        cmd.Parameters.AddWithValue("@currentID", RowID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
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
        public static String Delete (String Type, int TypeID)
        {
            int ID = Subscription.GetID();
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";
            string result = "";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"DELETE FROM " + ID + "_Arch WHERE Type=@type";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@type", Type);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            DeleteFromUsers (TypeID, ID);
            result = GetReadyTable(false);
            return result;
        }

        private static bool DeleteFromUsers (int TypeID, int ID)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT ID FROM " + ID + "_Users WHERE Type=@type";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@type", TypeID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            CreateUsers.Delete(rdr.GetInt32(0).ToString());
                }

                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"DELETE FROM " + ID + "_Users WHERE Type=@type";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@type", TypeID);
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
        public static String GetReadyTable (bool create)
        {
            if (create)
                Create();

            int ID = Subscription.GetBossID();
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";
            string result = "";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT ID, Type, Can_Create From " + ID + "_Arch";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (!populateResult(ref result, rdr))
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

        private static bool populateResult (ref String result, MySqlDataReader rdr)
        {
            result = "{\"result\": [";
            bool infoInside = false;

            while (rdr.Read())
            {
                infoInside = true;
                result += "{\"ID\":" + "\"" + rdr.GetInt32(0) + "\", ";
                result += "\"Type\":" + "\"" + rdr.GetString(1) + "\", ";
                result += "\"Can_Create\":" + "\"" + rdr.GetBoolean(2) + "\"},";
            }

            result = result.Substring(0, result.Length - 1);
            result += "]}";

            return infoInside;
        }
    }
}