using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Diagnostics;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for PhoneServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PhoneServices : System.Web.Services.WebService
    {
        private const int OSHA = 90;
        public static bool shouldSend;

        [WebMethod]
        public void RegisterSPL (String UserID, int SPL, String GPS, String Weather)
        {
            int BossID = Database.GetBossID(UserID);
            string phone;
            string message;

            if (BossID <= -1)
                return;

            try { phone = Database.GetManagerPhone(UserID); }
            catch (FormatException ex) { return; }

            message = "User " + UserID + " has exceeded the safe level of dBa; caution is advised. User Information:\n\n" + Database.GetUserInformation(UserID);

            if (SPL > OSHA)
                SendToPhone(phone, message);

            Database.RecordSPL(UserID, BossID, SPL, GPS, Weather);

            if (shouldSend)
                SendToPhone(phone, "User " + UserID + " has continued to stay in location: " + GPS);
        }

        [WebMethod]
        public void BLEDeviceDisconnected (String UserID)
        {
            int BossID = Database.GetBossID(UserID);
            string phone;
            string message;

            if (BossID <= -1)
                return;

            try { phone = Database.GetManagerPhone(UserID); }
            catch (FormatException ex) { return; }

            message = "User " + UserID + " has disconnected their BLE device for more than 5 minutes. User information:\n\n" + Database.GetUserInformation(UserID);

            SendToPhone(phone, message);
        }

        [WebMethod]
        public void ExcessiveSessions (String UserID)
        {
            int BossID = Database.GetBossID(UserID);
            string phone;
            string message;

            if (BossID <= -1)
                return;

            phone = Database.GetPhone(UserID);
            message = "You have exceeded the maximum limit of 5 sessions per hour.";

            SendToPhone(phone, message);
        }

        [WebMethod]
        public void LogError (String UserID, int ErrorID, String GPS, String Weather)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.LogError(UserID, BossID, ErrorID, GPS, Weather);
        }

        private void SendToPhone(String Phone, String Message)
        {
            string[] carriers = { "@tmomail.net", "@vtext.com", "@txt.att.net", "@messaging.sprintpcs.com", "@email.uscc.net" };

            for (int i = 0; i < carriers.Length; i++)
            {
                Mailing.SendMail(Phone + carriers[i], Message, "SPL Limit Exceeded!");
                Debug.WriteLine(Phone + carriers[i]);
            }
        }
    }
}
