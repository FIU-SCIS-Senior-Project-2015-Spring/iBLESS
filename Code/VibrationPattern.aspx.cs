using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace WebApplication1
{
    public partial class VibrationPattern : System.Web.UI.Page
    {
        private static int BossID;
        private static List<Vibrations.Vibration> vibrationList;

        protected void Page_Load(object sender, EventArgs e)
        {
            BossID = Subscription.GetBossID();
            vibrationList = new List<Vibrations.Vibration>();
        }

        [WebMethod]
        public static String GetVibrations()
        {
            var json = new { Vibrations = Vibrations.VibrationList };

            return JsonConvert.SerializeObject(json);
        }

        [WebMethod]
        public static int AddVibration (double Low, double High, String Setting)
        {
            if (VibrationList.Count != 0 && !NoDuplication(Low, High))
                return -1;

            return Database.AddVibration(BossID, Low, High, Setting);
        }

        [WebMethod]
        public static String GetTable ()
        {
            return Database.GetVibrationTable(BossID);
        }

        [WebMethod]
        public static int DeleteVibration (int ID)
        {
            return Database.DeleteVibration(BossID, ID);
        }

        private static bool NoDuplication(double Low, double High)
        {
            int high = VibrationList.Count - 1;
            int low = 0;
            int mid = 0;

            while (low <= high)
            {
                mid = (low + high) / 2;

                if (!Database.AreValuesExclusive(VibrationList[mid].Low, VibrationList[mid].High, Low, High))
                    return false;

                if (High <= VibrationList[mid].Low)
                    low = mid + 1;
                else if (Low >= VibrationList[mid].High)
                    high = mid - 1;
            }

            return true;
        }

        public static List<Vibrations.Vibration> VibrationList { get { return vibrationList; } set { vibrationList = value; } }
    }
}