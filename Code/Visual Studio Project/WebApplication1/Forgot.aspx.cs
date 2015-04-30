using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class Forgot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sends an e-mail to Email address with instructions on how to recover account access
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool SendMail (String Email)
        {
            return Database.SendMail(Email);
        }
    }
}