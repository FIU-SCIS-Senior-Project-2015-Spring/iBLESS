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
        private static List<dynamic> vibrationList;

        protected void Page_Load(object sender, EventArgs e)
        {
            BossID = Subscription.GetBossID();
            vibrationList = new List<dynamic>();
        }

        /// <summary>
        /// checks if vibration is duplicated for the given range, if not update it in database (modifying purposes)
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Low"></param>
        /// <param name="High"></param>
        /// <param name="Setting"></param>
        /// <returns></returns>
        [WebMethod]
        public static int UpdateVibration(int ID, double Low, double High, String Setting)
        {
            List<dynamic> copyList = new List<dynamic>();

            foreach (dynamic element in VibrationList)
                if (element.ID != ID)
                    copyList.Add(element);

            if (VibrationList.Count != 0 && !NoDuplication(Low, High, copyList))
                return -1;

            return Database.UpdateVibration(BossID, ID, Low, High, Setting);
        }

        /// <summary>
        /// Converts all vibration patterns under this company into a JSON representing them
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static String GetVibrations()
        {
            var json = new { Vibrations = Vibrations.VibrationList };

            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// checks if vibration is duplicated in given range, if not add it under company's table
        /// </summary>
        /// <param name="Low"></param>
        /// <param name="High"></param>
        /// <param name="Setting"></param>
        /// <returns></returns>
        [WebMethod]
        public static int AddVibration (double Low, double High, String Setting)
        {
            if (VibrationList.Count != 0 && !NoDuplication(Low, High, VibrationList))
                return -1;

            return Database.AddVibration(BossID, Low, High, Setting);
        }

        /// <summary>
        /// Asks database utility class to list all vibration settings for the give company
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static String GetTable ()
        {
            return Database.GetVibrationTable(BossID);
        }

        /// <summary>
        /// deletes vibration setting with id = ID from the company's table
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static int DeleteVibration (int ID)
        {
            return Database.DeleteVibration(BossID, ID);
        }

        /// <summary>
        /// uses binary sort by dBA value in order to quickly check for duplication
        /// </summary>
        /// <param name="Low"></param>
        /// <param name="High"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static bool NoDuplication(double Low, double High, List<dynamic> list)
        {
            int high = list.Count - 1;
            int low = 0;
            int mid = 0;

            while (low <= high)
            {
                mid = (low + high) / 2;

                if (!Database.AreValuesExclusive(list[mid].Low, list[mid].High, Low, High))
                    return false;

                if (High <= list[mid].Low)
                    low = mid + 1;
                else if (Low >= list[mid].High)
                    high = mid - 1;
            }

            return true;
        }

        public static List<dynamic> VibrationList { get { return vibrationList; } set { vibrationList = value; } }
    }
}