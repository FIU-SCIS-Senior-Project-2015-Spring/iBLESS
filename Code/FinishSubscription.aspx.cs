using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class FinishSubscription : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool Subscribe (int ID)
        {
            int userID = Subscription.GetID();
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = "UPDATE Companies SET `Chargify_ID`=@char_ID WHERE ID=@id";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@char_ID", ID);
                    cmd.Parameters.AddWithValue("@id", userID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            UpdateBossID(userID);
            return true;
        }

        private static bool UpdateBossID(int ID)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = "UPDATE User SET BossID=@id WHERE CustomerNumber=@id";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", ID);

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