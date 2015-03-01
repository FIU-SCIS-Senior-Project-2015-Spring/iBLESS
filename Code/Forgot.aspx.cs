using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class Forgot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool SendMail (String Email)
        {
            int counter = 0;
            string[] smtps = { "smtp.gmail.com", "smtp.live.com", "smtp.mail.yahoo.com" };
            string[] accounts = { "ibless.donotreply@gmail.com", "ibless.donotreply@hotmail.com", "ibless.donotreply@yahoo.com" };

            while (true)
            {
                try
                {
                    string smtpAddress = smtps[counter];
                    int portNumber = 587;
                    bool enableSSL = true;

                    string emailFrom = accounts[counter];
                    string password = "marjaime1";
                    string emailTo = Email;
                    string subject = "Password Recovery";
                    String message = "A password recovery request has been sent to this e-mail. If you requested to recover access to your account click on this link:\n";
                    String dbPass = GetPassword();
                    message += "http://ec2-52-0-155-150.compute-1.amazonaws.com/ChangePassword.aspx?Email=" + Email + "&Code=" + dbPass + "\n\n";
                    message += "After clicking on that link your password will change to: " + dbPass;

                    if (!UpdateRecovery(Email, dbPass))
                        return false;

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(emailFrom);
                        mail.To.Add(emailTo);
                        mail.Subject = subject;
                        mail.Body = message;
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            smtp.Credentials = new NetworkCredential(emailFrom, password);
                            smtp.EnableSsl = enableSSL;
                            smtp.Send(mail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    counter++;
                    if (counter >= 3)
                        return false;
                    continue;
                }

                break;
            }

            return true;
        }

        public static string GetPassword ()
        {
            string password = "";
            Random rand = new Random();

            for (int i = 0; i < 10; i++)
                password += (char) (rand.Next(0, 2) == 0 ? rand.Next(48, 58) : rand.Next(65, 91));

            return password;
        }

        private static bool UpdateRecovery (string Email, string Code)
        {
            string myConnectionString = "server=ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com;uid=iBLESS_Trac;" +
                                 "pwd=marjaime1;database=iBLESS;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = "REPLACE INTO Password_Recovery SET `E-mail`=@email, Code=@code";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@code", Code);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}