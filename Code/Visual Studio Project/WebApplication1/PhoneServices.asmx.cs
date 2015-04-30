using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Threading;

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
        public Boolean shouldSend = false; // marks whether the user has been in the same location twice.

        [WebMethod]
        public String RegisterSPL(String UserID, double SPL, String GPS, String Weather, double Windspeed, int Winddirection)
        {
            int BossID = Database.GetBossID(UserID); // company id
            string phone; // managers phonen umber
            string message; // message for SMS

            if (WindDirection.WindDoesNotExist(Winddirection))
                return "Error: Wind direction does not exist!";

            if (BossID <= -1)
                return "Error: User is not registered under any company!";

            try { phone = Database.GetManagerPhone(UserID); }
            catch (FormatException ex) { return "Error: Database unavailable!"; }

            message = "User " + UserID + " has exceeded the safe level of dBa; caution is advised. User Information:\n\n" + Database.GetUserInformation(UserID);

            Database.RecordSPL(UserID, BossID, SPL, GPS, Weather, ref shouldSend, Windspeed, Winddirection);

            int companySPL = Database.GetSPL_Type(BossID);

            if (companySPL >= 0 && SPL >= companySPL) // if company has chosen SPL type, and spl has surpassed the limit, send SMS
            {
                new Thread(() => SendToPhone(phone, message)).Start();

                if (shouldSend) // if user in same location twice, send same location message
                    new Thread(() => SendToPhone(phone, "User " + UserID + " has continued to stay in location: " + GPS)).Start();
            }

            // if spl >= companySPL then send fast alert since attention is crucial, otherwise send vibration pattern set by company.
            return SPL >= companySPL ? Vibrations.GetVibrationPattern(Vibrations.VibrationTypes.FastAlert) ?? Database.GetVibrationBasedOnSPL(BossID, SPL) : Database.GetVibrationBasedOnSPL(BossID, SPL);
        }

        [WebMethod]
        public bool ForgotPassword (String Email)
        {
            return Database.SendMail(Email);
        }

        [WebMethod]
        public int ResetPassword (String Username, String OldPassword, String NewPassword)
        {
            return ChangeInformation.UpdateInformationHelper(Username, "", "", OldPassword, NewPassword);
        }

        [WebMethod]
        public bool RegisterUser (String Username, String Firstname, String Lastname, String Email, String Password)
        {
            return Database.RegisterUser(Email, Firstname, Lastname, Password, Username);
        }

        [WebMethod]
        public void SetBLEInformation(String UserID, int IMEI, int MSISDN, int IMSI, String MAC)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.SetBLEInformation(UserID, BossID, IMEI, MSISDN, IMSI, MAC);
        }

        [WebMethod]
        public void SetPhoneInformation (String UserID, int IMEI, int MSISDN, int IMSI, String MAC, String Brand, String PhoneNumber, String PhoneModel,  String Carrier, String PhoneIP)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.SetPhoneInformation(UserID, BossID, IMEI, MSISDN, IMSI, MAC, Brand, PhoneNumber, PhoneModel, Carrier, PhoneIP);
        }

        [WebMethod]
        public String Login (String Username, String Password)
        {
            String errorMessage = "";

            if (Database.ValidateWS(Username, Password, ref errorMessage) != 0)
                return errorMessage;

            return Database.GetTotalInformationJSON(Database.GetID(Username).ToString());
        }

        [WebMethod]
        public void BLEDeviceDisconnected (String UserID)
        {
            int BossID = Database.GetBossID(UserID);
            string phone; // manager's phone
            string message;

            if (BossID <= -1)
                return;

            try { phone = Database.GetManagerPhone(UserID); }
            catch (FormatException ex) { return; }

            message = "User " + UserID + " has disconnected their BLE device for more than 5 minutes. User information:\n\n" + Database.GetUserInformation(UserID);

            SendToPhone(phone, message);
            Database.LogBLEStatus(UserID, BossID, false);
        }

        [WebMethod]
        public void BLEDeviceConnected(String UserID)
        {
            int BossID = Database.GetBossID(UserID);
            string phone; // manager's phone
            string message;

            if (BossID <= -1)
                return;

            try { phone = Database.GetManagerPhone(UserID); }
            catch (FormatException ex) { return; }

            message = "User " + UserID + " has turned on their BLE device.";

            SendToPhone(phone, message);
            Database.LogBLEStatus(UserID, BossID, true);
        }

        [WebMethod]
        public void ExcessiveSessions (String UserID)
        {
            int BossID = Database.GetBossID(UserID);
            string phone; // manager's phone
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

        [WebMethod]
        public void LogNotification (String UserID, String GPS, String Weather, String CellID, String AccessoryStatus, String VibrationSource, String VibrationEffect, int TimeVibrated, String Accelerometer, bool InGeofence)
        {
            int BossID = Database.GetBossID(UserID);

            if (BossID <= -1)
                return;

            Database.LogNotification(UserID, BossID, GPS, Weather, CellID, AccessoryStatus, VibrationSource, VibrationEffect, TimeVibrated, Accelerometer, InGeofence);
        }

        private void SendToPhone(String Phone, String Message)
        {
            string[] carriers = { "@tmomail.net", "@vtext.com", "@txt.att.net", "@messaging.sprintpcs.com", "@email.uscc.net" }; // sends message to most popular USA carriers

            for (int i = 0; i < carriers.Length; i++)
                Mailing.SendMail(Phone + carriers[i], Message, "SPL Limit Exceeded!");
        }
    }
}
