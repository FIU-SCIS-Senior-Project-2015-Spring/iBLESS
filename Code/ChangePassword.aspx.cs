using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Web.Services;

namespace WebApplication1
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool ChangeUserPassword (string Email, string Code)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT Code From Password_Recovery WHERE `E-mail`=@email";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@email", Email);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (rdr.GetString(0) == null || !rdr.GetString(0).Equals(Code))
                            return false;

                        conn.Close();

                        return ChangePassword_Helper(Email, Code);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return false;
            }
        }

        private static bool ChangePassword_Helper (string Email, string Code)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = "UPDATE User SET Password=@password, Guid=@guid WHERE `E-mail`=@email";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);
                    Guid userGuid = System.Guid.NewGuid();
                    string hashedPassword = Encryption.HashSHA1(Code + userGuid.ToString());

                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@guid", userGuid);
                    cmd.Parameters.AddWithValue("@email", Email);

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