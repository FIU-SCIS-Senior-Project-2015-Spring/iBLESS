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
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool Register (String Email, String Password, String UserName, String FirstName, String LastName)
        {
            return Database.RegisterUser(Email, FirstName, LastName, Password, UserName);
        }

        public static bool RegisterCreateUser (String Email, String Password, String UserName, String FirstName, String LastName, int ID)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"INSERT INTO User(FirstName,LastName,UserName,Password,`E-mail`,Guid,BossID) VALUES (@FirstName,@LastName,@UserName,@Password,@Email,@Guid,@id)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);
                    Guid userGuid = System.Guid.NewGuid();
                    string hashedPassword = Encryption.HashSHA1(Password + userGuid.ToString());

                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Guid", userGuid.ToString());
                    cmd.Parameters.AddWithValue("@id", ID);

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
    }
}