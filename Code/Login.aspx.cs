using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Text;
using System.Diagnostics;

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
            String username;
            int id;
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";;

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    
                    string stm = @"SELECT UserName, Password, Guid, CustomerNumber From User WHERE UserName=@Name";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", Email);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        string guid = rdr.GetString(2);
                        string hash = Encryption.HashSHA1(Password + guid);

                        if (!rdr.HasRows || !hash.Equals(rdr.GetString(1)))
                            return false;

                        username = rdr.GetString(0);
                        id = rdr.GetInt32(3);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return false;
            }
            
            Subscription.CreateCookie("MyTestCookie", username);
            Subscription.CreateCookie("ID", id.ToString());

            SetCanCreateCookie(username);

            return true;
        }

        private static void SetCanCreateCookie (String username)
        {
            int UserID = Subscription.GetBossID();
            String canCreate = "true";

            if (UserID == -2)
                canCreate = "false";
            else
            {
                string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                    "pwd=marjaime1;database=iBLESS;";

                try
                {
                    using (MySqlConnection conn = new MySqlConnection())
                    {
                        conn.ConnectionString = myConnectionString;
                        conn.Open();

                        string stm = @"SELECT `Can_Create` From User INNER JOIN " + UserID + "_Users ON User.CustomerNumber=" + UserID + "_Users.ID INNER JOIN " + UserID + "_Arch ON " +
                            UserID + "_Arch.ID=" + UserID + "_Users.Type WHERE UserName=@Name";

                        MySqlCommand cmd = new MySqlCommand(stm, conn);
                        cmd.Parameters.AddWithValue("@Name", username);

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();

                            if (!rdr.HasRows || rdr.GetInt32(0) == 0)
                                canCreate = "false";
                        }
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            Subscription.CreateCookie("canCreate", canCreate);
        }
    }
}