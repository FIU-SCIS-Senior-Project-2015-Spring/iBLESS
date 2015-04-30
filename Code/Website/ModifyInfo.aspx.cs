using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class ModifyInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetInfo (String ID)
        {
            return Database.GetInformationForCreateUsers(ID);
        }

        [WebMethod]
        public static string GetRolesInfo (String ID)
        {
            return Database.GetRolesJSON(ID);
        }

        [WebMethod]
        public static string GetParentsInfo (String ID)
        {
            return Database.GetParentsJSON(ID);
        }

        [WebMethod]
        public static int UpdateInfo (String Username, String ID, String FirstName, String LastName, String Email, String City, String State, String Title, String Phone, String Type, String Parent, String EmployeeID)
        {
            return Database.UpdateEmployeeInfo(Username, FirstName, LastName, Email, Phone, EmployeeID, City, Title, State, Type, Parent, ID);
        }
    }
}