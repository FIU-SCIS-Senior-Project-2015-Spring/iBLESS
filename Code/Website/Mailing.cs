using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace WebApplication1
{
    public class Mailing
    {
        public static bool SendMail(String EmailTo, String Message, String Subject)
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

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(emailFrom);
                        mail.To.Add(EmailTo);
                        mail.Subject = Subject;
                        mail.Body = Message;
                        mail.IsBodyHtml = false;

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
    }
}