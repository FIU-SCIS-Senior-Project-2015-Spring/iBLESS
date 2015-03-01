using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;
using System.Text;

namespace WebApplication1
{
    public class Subscription
    {
        public static void CreateCookie (String cookieName, String cookieValue)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            myCookie.Value = cookieValue;
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        private static bool GetReply(String ID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://testingtests.chargify.com/customers/" + ID + "/subscriptions.json");
            request.Credentials = new NetworkCredential("YgDv41ACKHJ6x4PVRUwY", "x");
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return true;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                }
                return false;
            }
        }

        public static bool IsSubscribed ()
        {
            int ID = GetID();
            int charID;
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT `Chargify_ID` From Companies WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (!rdr.HasRows)
                            return false;

                        charID = rdr.GetInt32(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return false;
            }

            return GetReply(charID.ToString());
        }

        public static int GetID ()
        {
            int ID;
            String UserName;

            try { UserName = GetCookieValue("MyTestCookie"); }
            catch (HttpException ex) { return -1; }

            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT CustomerNumber From User WHERE UserName=@Name";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", UserName);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (!rdr.HasRows)
                            return -1;

                        ID = rdr.GetInt32(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return -1;
            }

            return ID;
        }

        public static int GetBossID()
        {
            int ID = 0;
            String UserName;

            try { UserName = GetCookieValue("MyTestCookie"); }
            catch (HttpException ex) { return -2; }

            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT BossID From User WHERE UserName=@Name";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", UserName);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (rdr["BossID"] == DBNull.Value)
                            return -2;

                        ID = rdr.GetInt32(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return -1;
            }

            return ID;
        }

        public static String GetCookieValue (String cookieName) 
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];

            if (cookie == null || cookie.Value == null)
                throw new HttpException();

            return cookie.Value;
        }
    }
}