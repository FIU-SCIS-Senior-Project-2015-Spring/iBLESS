using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

namespace WebApplication1
{
    public partial class CheckSPL : System.Web.UI.Page
    {
        private static int BossID;
        private static List<SPL> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            BossID = Subscription.GetBossID();
        }

        [WebMethod]
        public static String GetSPLValues()
        {
            list = Database.GetSPLValues(BossID);
            return JsonConvert.SerializeObject(list);
        }

        [WebMethod]
        public static void AlertSafetyManagers ()
        {
            foreach (SPL person in list)
                if (person.SPLValue >= Database.GetSPL_Type(BossID))
                    Service1.SendToPhone(Database.GetManagerPhone(person.ID.ToString()), "User " + person.ID + " has exceeded the safe level of dBa; caution is advised. User Information:\n\n" + Database.GetUserInformation(person.ID.ToString()));       
        }
    }
}