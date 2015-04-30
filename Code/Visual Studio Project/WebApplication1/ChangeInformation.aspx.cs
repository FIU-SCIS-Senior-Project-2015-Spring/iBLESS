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

        /// <summary>
        /// Updates user information with new first name, last name, and new password.
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        [WebMethod]
        public static int UpdateInformation(string FirstName, string LastName, string OldPassword, string NewPassword)
        {
            string UserName;

            try { UserName = Subscription.GetCookieValue("MyTestCookie"); }
            catch (HttpException ex) { return -1; }

            return UpdateInformationHelper(UserName, FirstName, LastName, OldPassword, NewPassword);
        }

        /// <summary>
        /// Helper of previous method. Main logic occurs here.
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        public static int UpdateInformationHelper (String UserName, String FirstName, String LastName, String OldPassword, String NewPassword)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                     "pwd=marjaime1;database=iBLESS;";
            string hashedPassword = "";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT Password, Guid From User WHERE (`E-mail`=@mail OR UserName=@username)";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@username", UserName);
                    cmd.Parameters.AddWithValue("@mail", UserName);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        string oldPass = rdr.GetString(0);
                        string guid = rdr.GetString(1);

                        if (!OldPassword.Equals("") && !oldPass.Equals(Encryption.HashSHA1(OldPassword + guid)))
                            return 2;
                    }

                    string commandText = "UPDATE User SET " + (NewPassword == "" ? "" : "Password=@password, Guid=@guid, ") + (FirstName == "" ? "" : "FirstName=@firstName, ") + (LastName == "" ? "" : "LastName=@lastName, ");
                    commandText = commandText.Substring(0, commandText.Length - 2) + " WHERE (`E-mail`=@mail OR UserName=@username)";
                    cmd = new MySqlCommand(commandText, conn);
                    Guid userGuid = System.Guid.NewGuid();

                    if (!NewPassword.Equals(""))
                        hashedPassword = Encryption.HashSHA1(NewPassword + userGuid.ToString());

                    cmd.Parameters.AddWithValue("@username", UserName);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@firstName", FirstName);
                    cmd.Parameters.AddWithValue("@lastName", LastName);
                    cmd.Parameters.AddWithValue("@guid", userGuid);
                    cmd.Parameters.AddWithValue("@mail", UserName);

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