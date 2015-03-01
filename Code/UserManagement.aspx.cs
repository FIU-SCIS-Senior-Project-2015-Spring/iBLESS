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
            String UserName;

            try { UserName = Subscription.GetCookieValue("MyTestCookie"); }
            catch (HttpException ex) { return null; }

            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";

            string result;

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT UserName, `E-mail`, FirstName, LastName From User WHERE UserName=@Name";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", UserName);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    { 
                        rdr.Read();
                        var toJson = new { UserName = rdr.GetString(0), Email = rdr.GetString(1), FirstName = rdr.GetString(2), LastName = rdr.GetString(3) };
                        result = JsonConvert.SerializeObject(toJson);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return " ";
            }

            return result;
        }
    }
}