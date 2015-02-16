using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace WebApplication1
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetInformation ()
        {
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            myCookie = HttpContext.Current.Request.Cookies["MyTestCookie"];

            if (myCookie == null)
                return null;

            // Read the cookie information and display it.
            //String UserName = (String)HttpContext.Current.Session["UserName"];
            String UserName = myCookie.Value;

            if (UserName == null)
                return null;

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

                string stm = @"SELECT UserName, `E-mail`, FirstName, LastName From User WHERE UserName=@Name";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.Parameters.AddWithValue("@Name", UserName);
                rdr = cmd.ExecuteReader();

                rdr.Read();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return " ";
            }

            var toJson = new { UserName = rdr.GetString(0), Email = rdr.GetString(1), FirstName = rdr.GetString(2), LastName = rdr.GetString(3) };
            
            return JsonConvert.SerializeObject(toJson);
        }
    }
}