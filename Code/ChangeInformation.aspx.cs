using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Web.Services;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class ChangeInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static int UpdateInformation(string FirstName, string LastName, string OldPassword, string NewPassword)
        {
            string UserName;
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";
            string hashedPassword = "";

            try { UserName = Subscription.GetCookieValue("MyTestCookie"); }
            catch (HttpException ex) { return -1; }

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT Password, Guid From User WHERE UserName=@Name";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", UserName);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        string oldPass = rdr.GetString(0);
                        string guid = rdr.GetString(1);

                        if (!OldPassword.Equals("") && !oldPass.Equals(Encryption.HashSHA1(OldPassword + guid)))
                            return 2;
                    }

                    string commandText = "UPDATE User SET " + (NewPassword == "" ? "" : "Password=@password, Guid=@guid, ") + (FirstName == "" ? "" : "FirstName=@firstName, ") + (LastName == "" ? "" : "LastName=@lastName, ");
                    commandText = commandText.Substring(0, commandText.Length - 2) + " WHERE `UserName`=@username";
                    cmd = new MySqlCommand(commandText, conn);
                    Guid userGuid = System.Guid.NewGuid();

                    if (!NewPassword.Equals(""))
                        hashedPassword = Encryption.HashSHA1(NewPassword + userGuid.ToString());

                    cmd.Parameters.AddWithValue("@username", UserName);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@firstName", FirstName);
                    cmd.Parameters.AddWithValue("@lastName", LastName);
                    cmd.Parameters.AddWithValue("@guid", userGuid);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }

            return 1;
        }
    }
}