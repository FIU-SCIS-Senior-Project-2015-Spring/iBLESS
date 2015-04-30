using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace WebApplication1
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Gets all user information
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetInformation ()
        {
            return Database.GetTotalInformationJSON(Subscription.GetID().ToString());
        }
    }
}