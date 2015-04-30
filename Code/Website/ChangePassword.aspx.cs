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

        /// <summary>
        /// Validates that the code is correct for this Email
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool ValidateCode (string Email, string Code)
        {
            return Database.ValidateCode(Email, Code);
        }

        /// <summary>
        /// Changes password for this Email
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool ChangeUserPassword (string Email, string Password)
        {
            return Database.ChangeUserPassword(Email, Password);
        }
    }
}