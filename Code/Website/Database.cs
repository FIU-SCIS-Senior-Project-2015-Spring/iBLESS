using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WebApplication1
{
    /// <summary>
    /// This class is the foundation block for all iBLESS database interaction. Here there are a bunch of static methods with which to query and update the database.
    /// Also, the connection string is hidden as a property.
    /// </summary>
    public class Database
    {
        private static string myConnectionString = Properties.Settings.Default.ConnectionString;

        /// <summary>
        /// <c>RegisterUser</c> returns true if the user was successfully registed into the system or false if there was a problem.
        /// In order to store the password securely, <c>RegisterUser</c> uses SHA1 hashing algorithm with a unique Guid.
        /// </summary>
        /// <param name="Email">E-mail of the user</param>
        /// <param name="Firstname">Firstname of the user</param>
        /// <param name="Lastname">Lastname of the user</param>
        /// <param name="Password">Password of the user</param>
        /// <param name="Username">Username of the user</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RegisterUser(String Email, String Firstname, String Lastname, String Password, String Username)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"INSERT INTO User(FirstName,LastName,UserName,Password,`E-mail`,Guid) VALUES (@FirstName,@LastName,@UserName,@Password,@Email,@Guid)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);
                    Guid userGuid = System.Guid.NewGuid();
                    string hashedPassword = Encryption.HashSHA1(Password + userGuid.ToString());

                    cmd.Parameters.AddWithValue("@FirstName", Firstname);
                    cmd.Parameters.AddWithValue("@LastName", Lastname);
                    cmd.Parameters.AddWithValue("@UserName", Username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Guid", userGuid.ToString());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// <c>ActiveToggle</c> returns true if the user's status was changed; otherwise, it returns false.
        /// </summary>
        /// <param name="UserID">User ID of the user which status needs to be changed.</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool ActiveToggle(String UserID)
        {
            bool inactive = GetInactiveValue(UserID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"UPDATE User SET IsInactive=@inactive WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", UserID);
                    cmd.Parameters.AddWithValue("@inactive", !inactive);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }

            return !inactive;
        }

        /// <summary>
        /// Gets the user status.
        /// </summary>
        /// <param name="UserID">User ID of the user to check status of.</param>
        /// <returns>True if status successfully retrieved, or false if error.</returns>
        private static bool GetInactiveValue(String UserID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT IsInactive From User WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", UserID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        return rdr.GetBoolean(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Replaces the phone's information of the user with ID = UserID.
        /// </summary>
        /// <param name="UserID">ID of user.</param>
        /// <param name="BossID">ID of user's boss.</param>
        /// <param name="IMEI">IMEI of phone.</param>
        /// <param name="MSISDN">MSISDN of phone.</param>
        /// <param name="IMSI">IMSI of phone.</param>
        /// <param name="MAC">MAC of phone.</param>
        /// <param name="Brand">Brand of phone.</param>
        /// <param name="PhoneNumber">Phone number of phone.</param>
        /// <param name="PhoneModel">Phone model of phone.</param>
        /// <param name="Carrier">Carrier of phone.</param>
        /// <param name="PhoneIP">Phone IP.</param>
        public static void SetPhoneInformation(String UserID, int BossID, int IMEI, int MSISDN, int IMSI, String MAC, String Brand, String PhoneNumber, String PhoneModel, String Carrier, String PhoneIP)
        {
            CreatePhoneTable(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"REPLACE INTO " + BossID + "_PhoneInformation SET ID=@ID, IMEI=@imei, MSISDN=@msisdn, IMSI=@imsi, MAC=@mac"
                        + ", BRAND=@brand, PhoneNumber=@phoneNumber, PhoneModel=@phoneModel, Carrier=@carrier, PhoneIP=@ip";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@ID", UserID);
                    cmd.Parameters.AddWithValue("@imei", IMEI);
                    cmd.Parameters.AddWithValue("@msisdn", MSISDN);
                    cmd.Parameters.AddWithValue("@imsi", IMSI);
                    cmd.Parameters.AddWithValue("@mac", MAC);
                    cmd.Parameters.AddWithValue("@phoneNumber", PhoneNumber);
                    cmd.Parameters.AddWithValue("@brand", Brand);
                    cmd.Parameters.AddWithValue("@phoneModel", PhoneModel);
                    cmd.Parameters.AddWithValue("@carrier", Carrier);
                    cmd.Parameters.AddWithValue("@ip", PhoneIP);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Creates Phone Information table for company with ID = BossID
        /// </summary>
        /// <param name="BossID">ID of user's boss</param>
        /// <returns>True if table was created successfully. False if not.</returns>
        private static bool CreatePhoneTable(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_PhoneInformation (ID int NOT NULL, IMEI int NOT NULL, MSISDN int NOT NULL, IMSI int NOT NULL, MAC char(100) NOT NULL, BRAND char(100), PhoneNumber char(100), PhoneModel char(100), Carrier char(100), PhoneIP char(100), PRIMARY KEY (ID))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates user with Username/E-mail = Username and Password = Password.
        /// </summary>
        /// <param name="Username">User's username</param>
        /// <param name="Password">User's password</param>
        /// <returns>0 if valid. 1 if user does not exist. 2 if user is inactive.</returns>
        public static int Validate(String Username, String Password)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT UserName, Password, Guid, CustomerNumber, IsInactive From User WHERE (`E-mail`=@mail OR UserName=@Name)";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", Username);
                    cmd.Parameters.AddWithValue("@mail", Username);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        string guid = rdr.GetString(2);
                        string hash = Encryption.HashSHA1(Password + guid);

                        if (!rdr.HasRows || !hash.Equals(rdr.GetString(1)))
                            return 1;

                        if (rdr.GetBoolean(4))
                            return 2;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Webservice version of Validate
        /// </summary>
        /// <param name="Username">User's username</param>
        /// <param name="Password">User's password</param>
        /// <param name="ErrorMessage">Error message to populate in case of errors</param>
        /// <returns>0 if valid. 1 if user does not exist. 2 if user is inactive.</returns>
        public static int ValidateWS(String Username, String Password, ref String ErrorMessage)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT Password, Guid, IsInactive From User WHERE (`E-mail`=@mail OR UserName=@Name)";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", Username);
                    cmd.Parameters.AddWithValue("@mail", Username);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.HasRows)
                        {
                            ErrorMessage = "Error: User does not exist in DB";
                            return 1;
                        }

                        rdr.Read();
                        string guid = rdr.GetString(1);
                        string hash = Encryption.HashSHA1(Password + guid);    

                        if (!hash.Equals(rdr.GetString(0)))
                        {
                            ErrorMessage = "Error: Password is incorrect!";
                            return 1;
                        }

                        if (rdr.GetBoolean(2))
                        {
                            ErrorMessage = "Error: User is inactive";
                            return 2;
                        }
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Creates a cookie which indicates wether the user's role allows they to create other users under the company's ID.
        /// </summary>
        /// <param name="username">User's username for which to create the cookie.</param>
        public static void SetCanCreateCookie(String username)
        {
            int UserID = Subscription.GetBossID();
            String canCreate = "true";

            if (UserID == -2)
                canCreate = "false";
            else
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection())
                    {
                        conn.ConnectionString = myConnectionString;
                        conn.Open();

                        string stm = @"SELECT `Can_Create` From User INNER JOIN " + UserID + "_Users ON User.CustomerNumber=" + UserID + "_Users.ID INNER JOIN " + UserID + "_Arch ON " +
                            UserID + "_Arch.ID=" + UserID + "_Users.Type WHERE (`E-mail`=@mail OR UserName=@Name)";

                        MySqlCommand cmd = new MySqlCommand(stm, conn);
                        cmd.Parameters.AddWithValue("@Name", username);
                        cmd.Parameters.AddWithValue("@mail", username);

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();

                            if (!rdr.HasRows || rdr.GetInt32(0) == 0)
                                canCreate = "false";
                        }
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            Subscription.CreateCookie("canCreate", canCreate);
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>User's information</returns>
        public static string GetUserInformation(String ID)
        {
            String information = "";
            if (GetBossID(ID) > 0)
                information = GetComplexInformation(ID);
            else
                information = GetSimpleInformation(ID);

            return information;
        }

        /// <summary>
        /// Gets the manager's phone of user with id = ID
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>Manager's phone</returns>
        public static string GetManagerPhone(String ID)
        {
            int BossID = GetBossID(ID);
            int ParentID;

            try { ParentID = GetParentID(ID, BossID); }
            catch (Exception ex) { throw; }

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT Phone FROM " + BossID + "_Users WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("id", ParentID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        return rdr["Phone"].ToString();
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Gets parent's id of user with id = ID.
        /// The parent is the user under which THIS user has been created.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <param name="BossID">ID of user's boss</param>
        /// <returns>Parent's ID</returns>
        private static int GetParentID(String ID, int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT Parent, ID FROM " + BossID + "_Users WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.HasRows)
                            throw new FormatException();

                        rdr.Read();

                        if (rdr["Parent"] == DBNull.Value)
                            return rdr.GetInt32(1);

                        return rdr.GetInt32(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets user's phone number.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>User's phonen number</returns>
        public static string GetPhone(String ID)
        {
            int BossID = GetBossID(ID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT Phone FROM " + BossID + "_Users WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        return rdr["Phone"].ToString();
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Validates if DB's code is equal to method's code
        /// </summary>
        /// <param name="Email">E-mail that is assigned to the code. Serves as primary key.</param>
        /// <param name="Code">Code to check.</param>
        /// <returns>True if valid. False if not.</returns>
        public static bool ValidateCode (string Email, string Code)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    
                    string stm = @"SELECT Code From Password_Recovery WHERE `E-mail`=@email";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@email", Email);
                    conn.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (rdr.GetString(0) == null || !rdr.GetString(0).Equals(Code))
                            return false;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Changes user's password with e-mail = Email. 
        /// </summary>
        /// <param name="Email">User's e-mail address.</param>
        /// <param name="Password">User's password</param>
        /// <returns>True if password changed successfully. False if not.</returns>
        public static bool ChangeUserPassword (string Email, string Password)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = "UPDATE User SET Password=@password, Guid=@guid WHERE `E-mail`=@email";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);
                    Guid userGuid = System.Guid.NewGuid();
                    string hashedPassword = Encryption.HashSHA1(Password + userGuid.ToString());

                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@guid", userGuid);
                    cmd.Parameters.AddWithValue("@email", Email);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Send an e-mail to Email address with information on how to recover account's password.
        /// dbPass is used in order to make sure that only the person who received the e-mail can change the user's password
        /// </summary>
        /// <param name="Email">E-mail address to send e-mail to.</param>
        /// <returns>True if sent successfully. False if not.</returns>
        public static bool SendMail(String Email)
        {
            String Message = "A password recovery request has been sent to this e-mail. If you requested to recover access to your account click on this link:\n";
            String dbPass = GetPassword();
            Message += "http://ec2-52-0-155-150.compute-1.amazonaws.com/ChangePassword.aspx?Email=" + Email + "&Code=" + dbPass + "\n\n";

            if (!Database.UpdateRecovery(Email, dbPass))
                return false;

            return Mailing.SendMail(Email, Message, "Password Recovery");
        }

        /// <summary>
        /// Creates a randomly generated password.
        /// </summary>
        /// <returns>The password</returns>
        private static string GetPassword()
        {
            string password = "";
            Random rand = new Random();

            for (int i = 0; i < 10; i++)
                password += (char)(rand.Next(0, 2) == 0 ? rand.Next(48, 58) : rand.Next(65, 91));

            return password;
        }

        /// <summary>
        /// Links the database with the new code to validate user when recovering password.
        /// </summary>
        /// <param name="Email">User's e-mail address.</param>
        /// <param name="Code">Code to which link the e-mail address.</param>
        /// <returns>True if successful. False if not.</returns>
        private static bool UpdateRecovery(string Email, string Code)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = "REPLACE INTO Password_Recovery SET `E-mail`=@email, Code=@code";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@code", Code);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to get information of Emmployee type accounts.
        /// </summary>
        /// <param name="ID">User's ID.</param>
        /// <returns>Employee's information.</returns>
        private static string GetComplexInformation(String ID)
        {
            string informantion = "";
            int BossID = GetBossID(ID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT FirstName, LastName, `E-mail`, " + BossID + "_Arch.Type, Parent, Phone, City, State, Title From User INNER JOIN " + BossID + "_Users ON User.CustomerNumber=" + BossID + "_Users.ID INNER JOIN " + BossID + "_Arch ON " +
                        BossID + "_Arch.ID=" + BossID + "_Users.Type WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        informantion = "ID: " + ID + "\nFirst Name: " + rdr["FirstName"] + "\nLast Name: " + rdr["LastName"] + "\nE-mail: " + rdr["E-mail"] +
                            "\nType: " + rdr["Type"] + "\nParent: " + ((rdr["Parent"] == DBNull.Value) ? "No Parent" : rdr["Parent"]) + "\nPhone: " +
                            rdr["Phone"] + "\nCity: " + rdr["City"] + "\nState: " + rdr["State"] + "\nTitle: " + rdr["Title"];
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }

            return informantion;
        }

        /// <summary>
        /// Gets user's information in JSON format.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>User's information</returns>
        public static string GetTotalInformationJSON(String ID)
        {
            String information = GetInformationJSON(ID);

            if (information != " ")
                return information;

            return GetSimpleInformationJSON(ID);
        }

        /// <summary>
        /// Gets administrator information in JSON format.
        /// </summary>
        /// <param name="ID">Administator's ID.</param>
        /// <returns>Administator's information.</returns>
        public static string GetAdminInformationJSON(String ID)
        {
            string information;

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT CustomerNumber, Username, FirstName, LastName, `E-mail`, Name, Date, Address, Type, Subscription, TagLine, Website, Description, `Chargify_ID` FROM User INNER JOIN Companies ON User.CustomerNumber=Companies.ID WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        var json = new
                        {
                            Username = rdr["Username"],
                            FirstName = rdr["FirstName"],
                            LastName = rdr["LastName"],
                            Email = rdr["E-mail"],
                            ID = rdr["CustomerNumber"],
                            SPL_Type = Database.GetSPL_TypeString(GetBossID(ID)),
                            Date = rdr["Date"],
                            Name = rdr["Name"],
                            Address = rdr["Address"],
                            Type = rdr["Type"],
                            Subscription = rdr["Subscription"],
                            Tagline = rdr["TagLine"],
                            Website = rdr["Website"],
                            Description = rdr["Description"],
                            Chargify = rdr["Chargify_ID"]
                        };

                        information = JsonConvert.SerializeObject(json);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            return information;
        }

        /// <summary>
        /// Gets information for basic and administrator type of accounts.
        /// </summary>
        /// <param name="ID">User's ID.</param>
        /// <returns>Information in JSON format.</returns>
        private static string GetSimpleInformationJSON(String ID)
        {
            string information;
            int BossID = GetBossID(ID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT CustomerNumber, Username, FirstName, LastName, `E-mail` FROM User WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        var json = new
                        {
                            Username = rdr["Username"],
                            FirstName = rdr["FirstName"],
                            LastName = rdr["LastName"],
                            Email = rdr["E-mail"],
                            ID = rdr["CustomerNumber"],
                            SPL_Type = Database.GetSPL_TypeString(GetBossID(ID))
                        };

                        information = JsonConvert.SerializeObject(json);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            return information;
        }

        /// <summary>
        /// Creates a JSON where the navigation bar settings are listed.
        /// E.g:
        /// 
        /// if change information shouldn't be visible, then changeInformation will be set to false. 
        /// Then the programmer can easily check that property and set the navigation's bar visibility accordingly.
        /// </summary>
        /// <param name="Username">User's username</param>
        /// <returns>Navigation bar settings in JSON format.</returns>
        public static String GetNavigationSettings (String Username)
        {
            int userID = GetID(Username);
            int bossID = GetBossID(userID.ToString());
            bool canCreateUsers = Subscription.GetCookieValue("canCreate").Equals("true") ? true : false;

            if (Subscription.IsSubscribed())
                return JsonConvert.SerializeObject(new { changeInformation = true, subscriptions = true, createUser = IsHierarchyCreated(bossID), createTable = true, vibrationPattern = true, checkSPL = true });

            if (IsBasicUser(userID))
                return JsonConvert.SerializeObject(new { changeInformation = true, subscriptions = true, createUser = false, createTable = false, vibrationPattern = false, checkSPL = false });

            return JsonConvert.SerializeObject(new { changeInformation = true, subscriptions = false, createUser = canCreateUsers && IsHierarchyCreated(bossID), createTable = false, vibrationPattern = false, checkSPL = false });
        }

        /// <summary>
        /// Checks if company account has created a hierarchy.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>True if hierarchy created. False otherwise.</returns>
        private static bool IsHierarchyCreated (int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT ID, Type, Can_Create From " + BossID + "_Arch";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (rdr.HasRows)
                            return true;

                    return false;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets user's type.
        /// 
        /// E.g: Administrator.
        /// </summary>
        /// <param name="userID">User's ID</param>
        /// <returns>User type as a string.</returns>
        public static String GetUserType (int userID)
        {
            if (Subscription.IsSubscribed())
                return "Administrator";
            else if (IsBasicUser(userID))
                return "Basic User";

            return "Employee";
        }

        /// <summary>
        /// Checks if user is basic.
        /// </summary>
        /// <param name="userID">User's ID</param>
        /// <returns>True if basic. False if not.</returns>
        private static bool IsBasicUser (int userID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "SELECT BossID FROM User WHERE CustomerNumber=@id";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", userID);

                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (rdr["BossID"] == DBNull.Value)
                            return true;

                        return false;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates a table under company with ID = BossID.
        /// This table will hold information with the company SPL settings.
        /// 
        /// E.g:
        /// 
        /// Company wants vibration pattern to be overriden when SPL is between 50 to 60.
        /// Set LOW = 50, and HIGH = 60.
        /// Set String = vibration pattern.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>True if successfully created. False if not.</returns>
        private static bool CreateVibrationSettings(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_VibrationSettings (ID int NOT NULL AUTO_INCREMENT,Low DOUBLE NOT NULL, High DOUBLE NOT NULL, String char(100) NOT NULL, PRIMARY KEY (ID))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a vibration pattern to the company's vibration settings table.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <param name="Low">Low SPL boundary.</param>
        /// <param name="High">High SPL boundary.</param>
        /// <param name="Setting">Vibration pattern.</param>
        /// <returns>0 if success. -2 if SQL error.</returns>
        public static int AddVibration(int BossID, double Low, double High, String Setting)
        {
            CreateVibrationSettings(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "INSERT INTO " + BossID + "_VibrationSettings (Low, High,String) VALUES (@low,@high,@string)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@low", Low);
                    cmd.Parameters.AddWithValue("@high", High);
                    cmd.Parameters.AddWithValue("@string", Setting);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return -2;
            }

            return 0;
        }

        /// <summary>
        /// Gets vibration pattern based on SPL
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <param name="SPL">SPL value to check.</param>
        /// <returns>Vibration pattern if exists.</returns>
        public static String GetVibrationBasedOnSPL (int BossID, double SPL)
        {
            CreateVibrationSettings(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "SELECT Low, High, String FROM " + BossID + "_VibrationSettings";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            if (SPLInside(rdr.GetDouble(0), rdr.GetDouble(1), SPL))
                                return rdr.GetString(2);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return "Error with DB";
            }

            return " ";
        }

        /// <summary>
        /// Updates vibration pattern with Setting. 
        /// This vibration pattern belongs to the range between low and high.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <param name="ID">Vibration pattern's ID. Serves as primary key.</param>
        /// <param name="Low">Low SPL boundary.</param>
        /// <param name="High">High SPL boundary.</param>
        /// <param name="Setting">New vibration pattern.</param>
        /// <returns>0 if success. -2 if SQL error.</returns>
        public static int UpdateVibration (int BossID, int ID, double Low, double High, String Setting)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "UPDATE " + BossID + "_VibrationSettings SET Low=@low, High=@high, String=@setting WHERE ID=@id";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@low", Low);
                    cmd.Parameters.AddWithValue("@high", High);
                    cmd.Parameters.AddWithValue("@setting", Setting);
                    cmd.Parameters.AddWithValue("@id", ID);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return -2;
            }

            return 0;
        }

        /// <summary>
        /// Sets VibrationPattern.VibratrionList equal to the database's vibration table.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>Vibration list serialized into JSON.</returns>
        public static String GetVibrationTable(int BossID)
        {
            CreateVibrationSettings(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "SELECT Low, High, String, ID FROM " + BossID + "_VibrationSettings ORDER BY `Low` DESC";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        VibrationPattern.VibrationList.Clear();

                        while (rdr.Read())
                        {
                            String setting = rdr.GetString(2);
                            VibrationPattern.VibrationList.Add(new { Name = setting.Substring(0, setting.IndexOf(";")), Low = rdr.GetDouble(0), High = rdr.GetDouble(1), ID = rdr.GetInt32(3), Setting = setting });
                        }
                    }

                    var json = new { Vibrations = VibrationPattern.VibrationList };

                    return JsonConvert.SerializeObject(json);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }
        }

        /// <summary>
        /// Deletes vibration from company's vibration pattern table.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <param name="ID">Vibration's ID</param>
        /// <returns>0 if deleted. -1 if SQL error.</returns>
        public static int DeleteVibration(int BossID, int ID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"DELETE FROM " + BossID + "_VibrationSettings WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }

        // Checks if at least one of target's values are inside the range from SourceLow to SourceHigh.
        private static bool IsInsideRange(double SourceLow, double SourceHigh, double TargetLow, double TargetHigh)
        {
            return (TargetLow < SourceHigh && TargetLow > SourceLow) || (TargetHigh < SourceHigh && TargetHigh > SourceLow);
        }

        // Checks if SPL is inside the range from Low to High.
        private static bool SPLInside (double Low, double High, double SPL)
        {
            return (Low <= SPL && SPL <= High);
        }

        // Checks if range from TargetLow to TargetHigh is exclusive to range from SourceLow to SourceHigh.
        public static bool AreValuesExclusive(double SourceLow, double SourceHigh, double TargetLow, double TargetHigh)
        {
            if (SourceLow == TargetLow && SourceHigh == TargetHigh) return false;

            return !(IsInsideRange(SourceLow, SourceHigh, TargetLow, TargetHigh) || IsInsideRange(TargetLow, TargetHigh, SourceLow, SourceHigh));
        }

        /// <summary>
        /// Gets employee information for user with id = ID.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>Information</returns>
        public static string GetInformationJSON(String ID)
        {
            string information;
            int BossID = GetBossID(ID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT CustomerNumber, Username, FirstName, LastName, `E-mail`, " + BossID + "_Arch.Type, Parent, Phone, City, State, Title, EmployeeID From User INNER JOIN " + BossID + "_Users ON User.CustomerNumber=" + BossID + "_Users.ID INNER JOIN " + BossID + "_Arch ON " +
                        BossID + "_Arch.ID=" + BossID + "_Users.Type WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        var json = new
                        {
                            Username = rdr["Username"],
                            EmployeeID = rdr["EmployeeID"],
                            FirstName = rdr["FirstName"],
                            LastName = rdr["LastName"],
                            Email = rdr["E-mail"],
                            Phone = rdr["Phone"],
                            Type = rdr["Type"],
                            Parent = ((rdr["Parent"] == DBNull.Value) ? "No Parent" : rdr["Parent"]),
                            City = rdr["City"],
                            State = rdr["State"],
                            Title = rdr["Title"],
                            ID = rdr["CustomerNUmber"],
                            SPL_Type = Database.GetSPL_TypeString(GetBossID(ID))
                        };
                
                        information = JsonConvert.SerializeObject(json);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            return information;
        }

        /// <summary>
        /// Same as GetInformationJSON but with the addition of role's ID as a property under the name TypeID so that the pop up box for modify info can work correctly.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>User's information</returns>
        public static string GetInformationForCreateUsers (String ID)
        {
            string information;
            int BossID = GetBossID(ID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT CustomerNumber, Username, FirstName, LastName, `E-mail`, " + BossID + "_Arch.ID, Parent, Phone, City, State, Title, EmployeeID From User INNER JOIN " + BossID + "_Users ON User.CustomerNumber=" + BossID + "_Users.ID INNER JOIN " + BossID + "_Arch ON " +
                        BossID + "_Arch.ID=" + BossID + "_Users.Type WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        var json = new
                        {
                            Username = rdr["Username"],
                            EmployeeID = rdr["EmployeeID"],
                            FirstName = rdr["FirstName"],
                            LastName = rdr["LastName"],
                            Email = rdr["E-mail"],
                            Phone = rdr["Phone"],
                            TypeID = rdr["ID"],
                            Parent = ((rdr["Parent"] == DBNull.Value) ? "No Parent" : rdr["Parent"]),
                            City = rdr["City"],
                            State = rdr["State"],
                            Title = rdr["Title"],
                            ID = rdr["CustomerNUmber"],
                            SPL_Type = Database.GetSPL_TypeString(GetBossID(ID))
                        };

                        information = JsonConvert.SerializeObject(json);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            return information;
        }

        /// <summary>
        /// Records SPl value for user with ID = UserID under company with ID = BossID
        /// </summary>
        /// <param name="UserID">User's ID</param>
        /// <param name="BossID">Company's ID</param>
        /// <param name="SPL">SPL Value</param>
        /// <param name="GPS">GPS Location</param>
        /// <param name="Weather">Weather</param>
        /// <param name="shouldSend">Variable that indicates wether the user has already reported being on this location</param>
        /// <param name="Windspeed">Wind speed</param>
        /// <param name="Winddirection">Wind direction (bit-map, check WindDirection.cs)</param>
        /// <returns></returns>
        public static bool RecordSPL(String UserID, int BossID, double SPL, String GPS, String Weather, ref Boolean shouldSend, double Windspeed, int Winddirection)
        {
            CreateSPlTable(BossID);
            return RecordSPLHelper(UserID, BossID, SPL, GPS, Weather, ref shouldSend, Windspeed, Winddirection);
        }

        /// <summary>
        /// Records SPl value for user with ID = UserID under company with ID = BossID
        /// </summary>
        /// <param name="UserID">User's ID</param>
        /// <param name="BossID">Company's ID</param>
        /// <param name="SPL">SPL Value</param>
        /// <param name="GPS">GPS Location</param>
        /// <param name="Weather">Weather</param>
        /// <param name="shouldSend">Variable that indicates wether the user has already reported being on this location</param>
        /// <param name="Windspeed">Wind speed</param>
        /// <param name="Winddirection">Wind direction (bit-map, check WindDirection.cs)</param>
        /// <returns></returns>
        private static bool RecordSPLHelper(String UserID, int BossID, double SPL, String GPS, String Weather, ref Boolean shouldSend, double Windspeed, int Winddirection)
        {
            IsUserInSameLocation(UserID, GPS, BossID, ref shouldSend);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string commandText = @"INSERT INTO " + BossID + "_SPL (ID,Date,Time,Weather,Phone,`GPS_Loc`,SPL,WindSpeed,WindDirection) VALUES (@id,@date,@time,@weather,@phone,@gps,@spl,@speed,@direction)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", UserID);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/M/dd"));
                    cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("h:mm:ss tt"));
                    cmd.Parameters.AddWithValue("@weather", Weather);
                    cmd.Parameters.AddWithValue("@phone", GetPhone(UserID));
                    cmd.Parameters.AddWithValue("@gps", GPS);
                    cmd.Parameters.AddWithValue("@spl", SPL);
                    cmd.Parameters.AddWithValue("@speed", Windspeed);
                    cmd.Parameters.AddWithValue("@direction", WindDirection.GetName(Winddirection));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets company SPL value given the selection of the SPL regulation.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>-2 if no regulation select. -1 if SQL error. 90 if OSHA. 85 if NIOSH</returns>
        public static int GetSPL_Type (int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string command = @"SELECT `SPL_Type` FROM Companies WHERE ID=@bossID";
                    MySqlCommand cmd = new MySqlCommand(command, conn);
                    cmd.Parameters.AddWithValue("@bossID", BossID);
                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            return rdr.GetString(0).Equals("OSHA") ? 90 : 85;
                        }
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }

            return -2;
        }

        /// <summary>
        /// Gets SPL regulation's name.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>SPL regulation's name</returns>
        public static String GetSPL_TypeString (int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string command = @"SELECT `SPL_Type` FROM Companies WHERE ID=@bossID";
                    MySqlCommand cmd = new MySqlCommand(command, conn);
                    cmd.Parameters.AddWithValue("@bossID", BossID);
                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            return rdr.GetString(0).Equals("OSHA") ? "OSHA" : "NIOSH";
                        }
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }

            return "";
        }

        /// <summary>
        /// Creates a List containing information about the last entries per user under company with id = BossID.
        /// This information contains: firstname, lastname, location, and dBA.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>List with last entries</returns>
        public static List<SPL> GetSPLValues (int BossID)
        {
            CreateSPlTable(BossID);
            List<SPL> information = new List<SPL>();

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string commandText = @"SELECT FirstName, LastName, `GPS_Loc`, SPL, ID FROM " + BossID + "_SPL INNER JOIN User ON User.CustomerNumber=" + BossID + "_SPL.ID ORDER BY Date ASC, Time ASC";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            {
                                String name = rdr.GetString(0) + " " + rdr.GetString(1);
                                String location = rdr.GetString(2);
                                int spl = rdr.GetInt32(3);
                                int id = rdr.GetInt32(4);

                                int index = information.FindIndex(x => x.Name == name);

                                if (index == -1)
                                    information.Add(new SPL(name, location, spl, id));
                                else
                                    information[index] = new SPL(name, location, spl, id);
                            }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

            return information;
        }

        /// <summary>
        /// Assigns true to shouldSend if user has been before and in the same day in the location indicated by GPS.
        /// </summary>
        /// <param name="UserID">User's ID</param>
        /// <param name="GPS">Location</param>
        /// <param name="BossID">Company's ID</param>
        /// <param name="shouldSend">boolean value to populate with true if user has visited location, or false if not.</param>
        private static void IsUserInSameLocation(String UserID, String GPS, int BossID, ref Boolean shouldSend)
        {
            String date = DateTime.Now.ToString("yyyy/M/dd");

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string commandText = @"SELECT Date FROM " + BossID + "_SPL WHERE ID=@id AND `GPS_Loc`=@gps AND Date=@date";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", UserID);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@gps", GPS);

                    conn.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (rdr.HasRows)
                            shouldSend = true;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Creates SPL table for company with is = BossID.
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>True if successful. False if not</returns>
        private static bool CreateSPlTable(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_SPL (ID int NOT NULL,Date date NOT NULL,Time time(0) NOT NULL,Weather text,Phone char(100),GPS_Loc char(100),SPL DOUBLE NOT NULL, WindSpeed DOUBLE NOT NULL, WindDirection char(100) NOT NULL, PRIMARY KEY (Date, Time))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logs an error in the databse under company with id = BossID
        /// </summary>
        /// <param name="UserID">User who submits the error</param>
        /// <param name="BossID">Company's ID</param>
        /// <param name="ErrorID">Error's ID</param>
        /// <param name="GPS">Location where error was captureed</param>
        /// <param name="Weather">Weather where error was captured</param>
        public static void LogError(String UserID, int BossID, int ErrorID, String GPS, String Weather)
        {
            CreateErrorTable(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string commandText = @"INSERT INTO " + BossID + "_Errors (ID,Date,Time,Weather,Phone,`GPS_Loc`,ErrorID) VALUES (@id,@date,@time,@weather,@phone,@gps,@err)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", UserID);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/M/dd"));
                    cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("h:mm:ss tt"));
                    cmd.Parameters.AddWithValue("@weather", Weather);
                    cmd.Parameters.AddWithValue("@phone", GetPhone(UserID));
                    cmd.Parameters.AddWithValue("@gps", GPS);
                    cmd.Parameters.AddWithValue("@err", ErrorID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Creates error table for company with id = BossID
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>True if success. False if error.</returns>
        private static bool CreateErrorTable(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_Errors (ID int NOT NULL,Date char(100) NOT NULL,Time char(100) NOT NULL,Weather text,Phone char(100),GPS_Loc char(100),ErrorID int NOT NULL,PRIMARY KEY (Date, Time))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logs BLE device status (on or off).
        /// </summary>
        /// <param name="UserID">User who is logging his/her status.</param>
        /// <param name="BossID">Company's ID</param>
        /// <param name="IsOn">True or false</param>
        public static void LogBLEStatus(String UserID, int BossID, bool IsOn)
        {
            CreateBLETable(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;

                    string commandText = @"REPLACE INTO " + BossID + "_BLEInformation SET ID=@ID, IsOn=@isOn";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@ID", UserID);
                    cmd.Parameters.AddWithValue("@isOn", IsOn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Replaces information for BLE device of user with id = UserID
        /// </summary>
        /// <param name="UserID">User's ID</param>
        /// <param name="BossID">Company's ID</param>
        /// <param name="IMEI">BLE's IMEI</param>
        /// <param name="IMSI">BLE's IMSI</param>
        /// <param name="MSISDN">BLE's MSISDN</param>
        /// <param name="MAC">BLE's MAC</param>
        public static void SetBLEInformation(String UserID, int BossID, int IMEI, int IMSI, int MSISDN, String MAC)
        {
            CreateBLETable(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "REPLACE INTO " + BossID + "_BLEInformation SET ID=@id, IMEI=@imei, IMSI=@imsi, MSISDN=@msisdn, MAC=@mac";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", UserID);
                    cmd.Parameters.AddWithValue("@imsi", IMSI);
                    cmd.Parameters.AddWithValue("@@imei", IMEI);
                    cmd.Parameters.AddWithValue("@msisdn", MSISDN);
                    cmd.Parameters.AddWithValue("@mac", MAC);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Creates a BLE information table under company with id = BossIDD
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>True if success. False if error</returns>
        private static bool CreateBLETable(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_BLEInformation (ID int NOT NULL,IsOn BOOLEAN NOT NULL, IMEI int NOT NULL, MAC char(100) NOT NULL, MSISDN int NOT NULL, IMSI int NOT NULL, PRIMARY KEY (ID))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logs a notification under primary key = UserID. This notification belongs to the notifications table for company with company's id = BossID
        /// </summary>
        /// <param name="UserID">User's ID</param>
        /// <param name="BossID">Company's ID</param>
        /// <param name="GPS">Location</param>
        /// <param name="Weather">Weather</param>
        /// <param name="CellID">Cellular ID</param>
        /// <param name="AccessoryStatus">Accessory status</param>
        /// <param name="VibrationSource">Vibration source</param>
        /// <param name="VibrationEffect">Vibration pattern</param>
        /// <param name="TimeVibrated">Time device vibrated</param>
        /// <param name="Accelerometer">True if moving. False if not.</param>
        /// <param name="InGeofence">True or false.</param>
        public static void LogNotification(String UserID, int BossID, String GPS, String Weather, String CellID, String AccessoryStatus, String VibrationSource, String VibrationEffect, int TimeVibrated, String Accelerometer, bool InGeofence)
        {
            CreateNotificationsTable(BossID);

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string commandText = @"INSERT INTO " + BossID + "_Notifications (ID,Date,Time,Weather,Phone,`GPS_Loc`,CellID,AccessoryStatus,VibrationSource,VibrationEffect,TimeVibrated,Accelerometer,InGeofence) VALUES (@id,@date,@time,@weather,@phone,@gps,@cellID,@accStatus,@source,@effect,@timeVibrated,@accelerometer,@geofence)";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    cmd.Parameters.AddWithValue("@id", UserID);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/M/dd"));
                    cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("h:mm:ss tt"));
                    cmd.Parameters.AddWithValue("@weather", Weather);
                    cmd.Parameters.AddWithValue("@phone", GetPhone(UserID));
                    cmd.Parameters.AddWithValue("@gps", GPS);
                    cmd.Parameters.AddWithValue("@cellID", CellID);
                    cmd.Parameters.AddWithValue("@accStatus", AccessoryStatus);
                    cmd.Parameters.AddWithValue("@source", VibrationSource);
                    cmd.Parameters.AddWithValue("@effect", VibrationEffect);
                    cmd.Parameters.AddWithValue("@timeVibrated", TimeVibrated);
                    cmd.Parameters.AddWithValue("@accelerometer", Accelerometer);
                    cmd.Parameters.AddWithValue("@geofence", InGeofence);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Creates the notiication table under company with id = BossID
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>True if successful. False if not.</returns>
        private static bool CreateNotificationsTable(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_Notifications (ID int NOT NULL,Date char(100) NOT NULL,Time char(100) NOT NULL,Weather text,Phone char(100),GPS_Loc char(100),CellID char(100) NOT NULL, AccessoryStatus char(100) NOT NULL, VibrationSource char(100) NOT NULL, VibrationEffect char(100) NOT NULL, TimeVibrated int NOT NULL, Accelerometer char(100) NOT NULL, InGeofence BOOLEAN NOT NULL, PRIMARY KEY (Date, Time))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the company's ID for a user with id = ID
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>Company's ID</returns>
        public static int GetBossID(String ID)
        {
            int id = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT BossID From User WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (rdr["BossID"] == DBNull.Value)
                            return -2;

                        id = rdr.GetInt32(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return -1;
            }

            return id;
        }

        /// <summary>
        /// Gets basic user or information.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>Information</returns>
        private static string GetSimpleInformation(String ID)
        {
            string informantion = "";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT FirstName, LastName, `E-mail` From User WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        informantion = "ID: " + ID + "\nFirst Name: " + rdr["FirstName"] + "\nLast Name: " + rdr["LastName"] + "\nE-mail: " + rdr["E-mail"];
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return "";
            }

            return informantion;
        }

        /// <summary>
        /// Uses a MySQLDataReader in order to populate a string passed as a parameter with information.
        /// The string is set in JSON format.
        /// </summary>
        /// <param name="result">String to populate</param>
        /// <param name="rdr">DataReader from where to get information</param>
        /// <returns>True if information was added to result. False if nothing was added.</returns>
        private static bool populateResult(ref String result, MySqlDataReader rdr)
        {
            result = "{\"result\": [";
            bool infoInside = false;

            while (rdr.Read())
            {
                infoInside = true;
                result += "{\"ID\":" + "\"" + rdr.GetInt32(0) + "\", ";
                result += "\"Type\":" + "\"" + rdr.GetString(1) + "\", ";
                result += "\"Can_Create\":" + "\"" + rdr.GetBoolean(2) + "\"},";
            }

            result = result.Substring(0, result.Length - 1);
            result += "]}";

            return infoInside;
        }

        /// <summary>
        /// Gets all team leaders (parents) under user's company's users table.
        /// </summary>
        /// <param name="ID">User's ID</param>
        /// <returns>Parents as JSON</returns>
        public static String GetParentsJSON(String ID)
        {
            String result = "";
            int UserID = GetBossID(ID);

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT ID, Parent FROM " + UserID + "_Users";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (!CreateJSON_IDS(ref result, rdr))
                            return " ";
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return " ";
            }

            return result;
        }

        /// <summary>
        /// Uses a MySQLDataReader in order to populate a string with its information.
        /// </summary>
        /// <param name="result">String to populate</param>
        /// <param name="rdr">DataReader from where to get the information</param>
        /// <returns>True if information was added to result. False if not.</returns>
        private static bool CreateJSON_IDS(ref String result, MySqlDataReader rdr)
        {
            result = "{\"result\": [";
            bool infoInside = false;

            while (rdr.Read())
            {
                if (rdr["Parent"] == DBNull.Value)
                {
                    infoInside = true;
                    result += "{\"ID\":" + "\"" + rdr.GetString(0) + "\"},";
                }
            }

            result = result.Substring(0, result.Length - 1);
            result += "]}";

            return infoInside;
        }

        /// <summary>
        /// Updates user's information
        /// </summary>
        /// <param name="Username">User's username</param>
        /// <param name="FirstName">User's firstname</param>
        /// <param name="LastName">User's lastname</param>
        /// <param name="Email">User's e-mail address</param>
        /// <param name="Phone">User's phone</param>
        /// <param name="EmployeeID">User's employee id</param>
        /// <param name="City">User's city</param>
        /// <param name="Title">User's job title</param>
        /// <param name="State">User's state</param>
        /// <param name="Role">User's role</param>
        /// <param name="Parent">User's team leader</param>
        /// <param name="ID">User's ID</param>
        /// <returns>0 if no error.</returns>
        public static int UpdateEmployeeInfo(String Username, String FirstName, String LastName, String Email, String Phone, String EmployeeID, String City, String Title, String State, String Role, String Parent, String ID)
        {
            if (!UpdateUsertable(Username, FirstName, LastName, Email, ID))
                return -1;

            return UpdateCompanyUsertable(Phone, EmployeeID, City, Title, State, Role, Parent, ID) == false ? -2 : 0;
        }

        // Helper method for UpdateEmployeeInfo
        private static bool UpdateCompanyUsertable(String Phone, String EmployeeID, String City, String Title, String State, String Role, String Parent, String ID)
        {
            int BossID = GetBossID(ID);

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"UPDATE " + BossID + "_Users SET Phone=@phone, EmployeeID=@empID, Type=@role, Parent=@parent, City=@city, State=@state, Title=@title WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.Parameters.AddWithValue("@phone", Phone);
                    cmd.Parameters.AddWithValue("@empID", EmployeeID);
                    cmd.Parameters.AddWithValue("@role", Role);
                    cmd.Parameters.AddWithValue("@parent", Parent);
                    cmd.Parameters.AddWithValue("@city", City);
                    cmd.Parameters.AddWithValue("@state", State);
                    cmd.Parameters.AddWithValue("@title", Title);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        //Helper method for UpdateEmployeeInfo
        private static bool UpdateUsertable(String Username, String FirstName, String LastName, String Email, String ID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"UPDATE User SET UserName=@name, FirstName=@first, LastName=@last, `E-mail`=@email WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.Parameters.AddWithValue("@first", FirstName);
                    cmd.Parameters.AddWithValue("@last", LastName);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@name", Username);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets id for user with username = Username
        /// </summary>
        /// <param name="Username">User's username</param>
        /// <returns>returns ID</returns>
        public static int GetID(String Username)
        {
            int ID;

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT CustomerNumber From User WHERE (`E-mail`=@mail OR UserName=@Name)";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@Name", Username);
                    cmd.Parameters.AddWithValue("@mail", Username);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        if (!rdr.HasRows)
                            return -2;

                        ID = rdr.GetInt32(0);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }

            return ID;
        }

        /// <summary>
        /// Creates a user into the system
        /// </summary>
        /// <param name="Username">New user's username</param>
        /// <param name="FirstName">New user's firstname</param>
        /// <param name="LastName">New user's lastname</param>
        /// <param name="Email">New user's e-mail</param>
        /// <param name="Type">New user's role</param>
        /// <param name="Parent">New user's parent</param>
        /// <param name="Phone">New user's phone number</param>
        /// <param name="EmployeeID">New user's employee ID</param>
        /// <param name="City">New user's city</param>
        /// <param name="State">New user's state</param>
        /// <param name="Title">New user's job title</param>
        /// <returns>"" if success, or error messages</returns>
        public static string CreateUser(String Username, String FirstName, String LastName, String Email, String Type, String Parent, String Phone, String EmployeeID, String City, String State, String Title)
        {
            int UserID = Subscription.GetBossID();
            string Password = GetPassword();

            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = myConnectionString;
                conn.Open();

                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int ID = RegisterUserWithBossID(Email, Password, Username, FirstName, LastName, UserID, conn, transaction);
                        RegisterUnderCompany(ID.ToString(), Type, Parent, Phone, EmployeeID, City, State, Title, conn, transaction);
                        transaction.Commit();
                    }
                    catch (MySqlException ex)
                    {
                        Debug.WriteLine(ex.HelpLink);
                        transaction.Rollback();
                        return ex.HelpLink;
                    }
                }
            }

            return Mailing.SendMail(Email, "Welcome to iBLESS. You have been registered under user " + UserID + "\nYou password is: " + Password, "Welcome to iBLESS!") ? "" : "Error sending E-mail";
        }

        /// <summary>
        /// Creates the users table for company with id = BossID
        /// </summary>
        /// <param name="BossID">Company's ID</param>
        /// <returns>True if success. False if not</returns>
        public static bool CreateUsersTable(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_Users (ID int NOT NULL,Type int, Parent int, CreatedBy int, Date char(100), Time char(100), Phone char(100) UNIQUE, EmployeeID int UNIQUE, City char(100), State char(100), Title char(100), PRIMARY KEY (ID))";
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a user inside users table of company with id = UserID
        /// </summary>
        /// <param name="ID">New user's id</param>
        /// <param name="Type">New user's role</param>
        /// <param name="Parent">New user's parent</param>
        /// <param name="Phone">New user's phone number</param>
        /// <param name="EmployeeID">New user's employee id</param>
        /// <param name="City">New user's city</param>
        /// <param name="State">New user's state</param>
        /// <param name="Title">New user's job title</param>
        /// <param name="conn">Connection string</param>
        /// <param name="transaction">MySQL transaction (used for atomic operation)</param>
        /// <returns>True if no error. False if error.</returns>
        private static bool RegisterUnderCompany(String ID, String Type, String Parent, String Phone, String EmployeeID, String City, String State, String Title, MySqlConnection conn, MySqlTransaction transaction)
        {
            int UserID = Subscription.GetBossID();

            try
            {
                string commandText = @"INSERT INTO " + UserID + "_Users(ID,Type,Parent,CreatedBy,Date,Time,Phone,EmployeeID,City,State,Title) VALUES (@id,@type,@parent,@by,@date,@time,@phone,@employee,@city,@state,@title)";
                MySqlCommand cmd = new MySqlCommand(commandText, conn, transaction);

                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@type", Type);
                cmd.Parameters.AddWithValue("@parent", Parent);
                cmd.Parameters.AddWithValue("@by", UserID);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/M/dd"));
                cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("h:mm:ss tt"));
                cmd.Parameters.AddWithValue("@phone", Phone);
                cmd.Parameters.AddWithValue("@employee", EmployeeID);
                cmd.Parameters.AddWithValue("@city", City);
                cmd.Parameters.AddWithValue("@state", State);
                cmd.Parameters.AddWithValue("@title", Title);

                cmd.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                ex.HelpLink = getKey(ex.Message);
                throw ex;
            }

            return true;
        }

        //Extracts error name from error messages.
        private static string getKey(String Error)
        {
            String key = "";

            for (int i = Error.Length - 2; i >= 0 && Error[i] != '\''; i--)
                key = Error[i] + key;

            return key;
        }

        // Creates a user in the system that contains a boss id indicating that this user belongs to a company
        private static int RegisterUserWithBossID(String Email, String Password, String UserName, String FirstName, String LastName, int ID, MySqlConnection conn, MySqlTransaction transaction)
        {
            int LastID = -1;

            try
            {
                string commandText = @"INSERT INTO User(FirstName,LastName,UserName,Password,`E-mail`,Guid,BossID) VALUES (@FirstName,@LastName,@UserName,@Password,@Email,@Guid,@id); SELECT LAST_INSERT_ID()";
                MySqlCommand cmd = new MySqlCommand(commandText, conn, transaction);
                Guid userGuid = System.Guid.NewGuid();
                string hashedPassword = Encryption.HashSHA1(Password + userGuid.ToString());

                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Guid", userGuid.ToString());
                cmd.Parameters.AddWithValue("@id", ID);

                cmd.ExecuteNonQuery();
                LastID = (int)cmd.LastInsertedId;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                ex.HelpLink = getKey(ex.Message);
                throw ex;
            }

            return LastID;
        }
    }
}