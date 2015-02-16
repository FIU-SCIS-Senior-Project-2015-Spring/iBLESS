using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Text;

namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession=true)]
        public static bool Validate(String Email, String Password)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";

            MySqlDataReader rdr = null;

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();

                string stm = @"SELECT UserName, Password, Guid From User WHERE UserName=@Name";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.Parameters.AddWithValue("@Name", Email);
                rdr = cmd.ExecuteReader();

                rdr.Read();
                string guid = rdr.GetString(2);
                string hash = Encryption.HashSHA1(Password + guid);

                if (rdr.GetString(0) == null || rdr.GetString(1) == null || !hash.Equals(rdr.GetString(1)))
                    return false;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return false;
            }
            
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            DateTime now = DateTime.Now;

            // Set the cookie value.
            myCookie.Value = rdr.GetString(0);
            // Set the cookie expiration date.
            myCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire

            // Add the cookie.
            HttpContext.Current.Response.Cookies.Add(myCookie);

            //HttpContext.Current.Session["UserName"] = rdr.GetString(0);

            return true;
        }
    }
}