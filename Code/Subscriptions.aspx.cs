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
    public partial class Subscriptions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool IsSubscribed ()
        {
            return Subscription.IsSubscribed();
        }

        [WebMethod]
        public static int MakeSubscription (String Company, String Address, String Type, int Subs)
        {
            if (Database.GetBossID(Subscription.GetID().ToString()) != -2)
                return -1;

            string subscription = "";
            string myConnectionString;
            string date = DateTime.Now.ToString("M/d/yyyy");
            string time = DateTime.Now.ToString("h:mm:ss tt");
            int id = Subscription.GetID();

            myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            if (Subs == 0) subscription = "Bronze";
            else if (Subs == 1) subscription = "Silver";
            else if (Subs == 2) subscription = "Gold";
            else subscription = "Black";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"REPLACE INTO Companies SET ID=@ID, Name=@Name, Date=@Date, Time=@Time, Address=@Address, Type=@Type, Subscription=@Subscription";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", Company);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Time", time);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Type", Type);
                    cmd.Parameters.AddWithValue("@Subscription", subscription);

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
    }
}
