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
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool ValidateCode (string Email, string Code)
        {
            return Database.ValidateCode(Email, Code);
        }

        [WebMethod]
        public static bool ChangeUserPassword (string Email, string Password)
        {
            return Database.ChangeUserPassword(Email, Password);
        }
    }
}