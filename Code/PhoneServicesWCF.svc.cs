using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;

namespace WebApplication1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : PhoneServicesWCF
    {
        private const int OSHA = 90;
        public Boolean shouldSend = false;

        public String Login(String Username, String Password)
        {
            if (Database.Validate(Username, Password) != 0)
                return "Error";

            return Database.GetTotalInformationJSON(Database.GetID(Username).ToString());
        }

        public bool ForgotPassword(String Email)
        {
            return Database.SendMail(Email);
        }

        public bool RegisterUser(String Username, String Firstname, String Lastname, String Email, String Password)
        {
            return Database.RegisterUser(Email, Firstname, Lastname, Password, Username);
        }

        public String RegisterSPL(String UserID, double SPL, String GPS, String Weather, double Windspeed, int Winddirection)
        {
            int BossID = Database.GetBossID(UserID);
            string phone;
            string message;

            if (WindDirection.WindDoesNotExist(Winddirection) || BossID <= -1)
                return "Error";

            try { phone = Database.GetManagerPhone(UserID); }
            catch (FormatException ex) { return "Error"; }

            message = "User " + UserID + " has exceeded the safe level of dBa; caution is advised. User Information:\n\n" + Database.GetUserInformation(UserID);

            Database.RecordSPL(UserID, BossID, SPL, GPS, Weather, ref shouldSend, Windspeed, Winddirection);

            if (SPL > OSHA)
                SendToPhone(phone, message);

            if (shouldSend)
                SendToPhone(phone, "User " + UserID + " has continued to stay in location: " + GPS);

            return Database.GetVibrationBasedOnSPL(BossID, SPL);
        }

        public void SetPhoneInformation(String UserID, int IMEI, int MSISDN, int IMSI, String MAC, String Brand, String PhoneNumber, String PhoneModel, String Carrier, String PhoneIP)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.SetPhoneInformation(UserID, BossID, IMEI, MSISDN, IMSI, MAC, Brand, PhoneNumber, PhoneModel, Carrier, PhoneIP);
        }

        public void SetBLEInformation (String UserID, int IMEI, int MSISDN, int IMSI, String MAC)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.SetBLEInformation(UserID, BossID, IMEI, MSISDN, IMSI, MAC);
        }

        public void BLEDeviceDisconnected(String UserID)
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
            Database.LogBLEStatus(UserID, BossID, false);
        }

        public void BLEDeviceConnected(String UserID)
        {
            int BossID = Database.GetBossID(UserID);
            string phone;
            string message;

            if (BossID <= -1)
                return;

            try { phone = Database.GetManagerPhone(UserID); }
            catch (FormatException ex) { return; }

            message = "User " + UserID + " has turned on their BLE device.";

            SendToPhone(phone, message);
            Database.LogBLEStatus(UserID, BossID, true);
        }

        public void ExcessiveSessions(String UserID)
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

        public void LogError(String UserID, int ErrorID, String GPS, String Weather)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.LogError(UserID, BossID, ErrorID, GPS, Weather);
        }

        public void LogNotification(String UserID, String GPS, String Weather, String CellID, String AccessoryStatus, String VibrationSource, String VibrationEffect, int TimeVibrated, String Accelerometer, bool InGeofence)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.LogNotification(UserID, BossID, GPS, Weather, CellID, AccessoryStatus, VibrationSource, VibrationEffect, TimeVibrated, Accelerometer, InGeofence);
        }

        public static void SendToPhone(String Phone, String Message)
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
