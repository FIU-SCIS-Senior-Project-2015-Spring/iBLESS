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

        /// <summary>
        /// Gets navigation bar settings for this user
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [WebMethod]
        public static String GetNavigationSettings (String Username)
        {
            return Database.GetNavigationSettings(Username);
        }

        /// <summary>
        /// Gets the user type (basic, employee, or administrator) for the given username
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [WebMethod]
        public static String GetUserType (String Username)
        {
            int ID = Database.GetID(Username);
            return Database.GetUserType(ID);
        }

        /// <summary>
        /// checks if administator is still subscribed into system
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static bool IsSubscribed ()
        {
            return Subscription.IsSubscribed();
        }

        /// <summary>
        /// Gets username's information
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [WebMethod]
        public static String GetInformation (String Username)
        {
            String json = Database.GetTotalInformationJSON(Database.GetID(Username).ToString());
            return json;
        }

        /// <summary>
        /// Gets administrator's information.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [WebMethod]
        public static String GetInformationAdmin ()
        {
            return Database.GetAdminInformationJSON(Subscription.GetID().ToString());
        }

        /// <summary>
        /// Creates a company entry in the companies table for this given user to signal that he is about to become a paying customer.
        /// The chargify id is used to verify this. At the beginning it will be set to 0, and when purchase has been finished it will be updated
        /// with a unique value in order to identify the user.
        /// </summary>
        /// <param name="Company">Company Name</param>
        /// <param name="Address"></param>
        /// <param name="Type">Company Type</param>
        /// <param name="SubscriptionType">Black, Gold, Silver, Bronze</param>
        /// <param name="SPL_Type">OSHA or NIOSH</param>
        /// <param name="Tagline"></param>
        /// <param name="Description"></param>
        /// <param name="Weblink">Website address</param>
        /// <param name="CharID">Unique ID that will be used by Chargify to verify payment</param>
        /// <returns></returns>
        [WebMethod]
        public static int MakeSubscription (String Company, String Address, String Type, String SubscriptionType, String SPL_Type, String Tagline, String Description, String Weblink, int CharID)
        {
            string myConnectionString;
            string date = DateTime.Now.ToString("yyyy/M/dd"); // important to keep this time format since the system relies on it.
            string time = DateTime.Now.ToString("h:mm:ss tt"); // important to keep this time format since the system relies on it.
            int id = Subscription.GetID();

            myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"REPLACE INTO Companies SET ID=@ID, Name=@Name, Date=@Date, Time=@Time, Address=@Address, Type=@Type, Subscription=@Subscription, `SPL_Type`=@splType," + 
                        "TagLine=@tag, Website=@web, Description=@description, `Chargify_ID`=@charID";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", Company);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Time", time);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Type", Type);
                    cmd.Parameters.AddWithValue("@Subscription", SubscriptionType);
                    cmd.Parameters.AddWithValue("@splType", SPL_Type);
                    cmd.Parameters.AddWithValue("@tag", Tagline);
                    cmd.Parameters.AddWithValue("@web", Weblink);
                    cmd.Parameters.AddWithValue("@description", Description);
                    cmd.Parameters.AddWithValue("@charID", CharID);

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
