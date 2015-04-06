using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WebApplication1
{
    public class Database
    {
        private static string myConnectionString = Properties.Settings.Default.ConnectionString;

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
                            UserID + "_Arch.ID=" + UserID + "_Users.Type WHERE UserName=@Name";

                        MySqlCommand cmd = new MySqlCommand(stm, conn);
                        cmd.Parameters.AddWithValue("@Name", username);

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

        public static string GetUserInformation(String ID)
        {
            String information = "";
            if (GetBossID(ID) > 0)
                information = GetComplexInformation(ID);
            else
                information = GetSimpleInformation(ID);

            return information;
        }

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

        public static bool ChangeUserPassword (string Email, string Password)
        {
            Debug.WriteLine(Email);
            Debug.WriteLine(Password);
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

        public static bool SendMail(String Email)
        {
            String Message = "A password recovery request has been sent to this e-mail. If you requested to recover access to your account click on this link:\n";
            String dbPass = GetPassword();
            Message += "http://ec2-52-0-155-150.compute-1.amazonaws.com/ChangePassword.aspx?Email=" + Email + "&Code=" + dbPass + "\n\n";

            if (!Database.UpdateRecovery(Email, dbPass))
                return false;

            return Mailing.SendMail(Email, Message, "Password Recovery");
        }

        private static string GetPassword()
        {
            string password = "";
            Random rand = new Random();

            for (int i = 0; i < 10; i++)
                password += (char)(rand.Next(0, 2) == 0 ? rand.Next(48, 58) : rand.Next(65, 91));

            return password;
        }

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

        public static string GetTotalInformationJSON(String ID)
        {
            String information = GetInformationJSON(ID);

            if (information != " ")
                return information;

            return GetSimpleInformationJSON(ID);
        }

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
                            SPL_Type = Database.GetSPL_Type(GetBossID(ID))
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

        /*
         * Return value of 0 means success
         * Return value of -2 means that there was a problem with SQL
         */
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
                            if (!AreValuesExclusive(rdr.GetDouble(0), rdr.GetDouble(1), SPL, SPL))
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
                        while (rdr.Read())
                        {
                            String setting = rdr.GetString(2);

                            VibrationPattern.VibrationList.Add(new Vibrations.Vibration(setting.Substring(0, setting.IndexOf(";")), rdr.GetDouble(0), rdr.GetDouble(1), rdr.GetInt32(3)));
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

        private static bool IsInsideRange(double SourceLow, double SourceHigh, double TargetLow, double TargetHigh)
        {
            return (TargetLow < SourceHigh && TargetLow > SourceLow) || (TargetHigh < SourceHigh && TargetHigh > SourceLow);
        }

        public static bool AreValuesExclusive(double SourceLow, double SourceHigh, double TargetLow, double TargetHigh)
        {
            if (SourceLow == TargetLow && SourceHigh == TargetHigh) return false;

            return !(IsInsideRange(SourceLow, SourceHigh, TargetLow, TargetHigh) || IsInsideRange(TargetLow, TargetHigh, SourceLow, SourceHigh));
        }

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
                            FirstName = rdr["FirstName"],
                            LastName = rdr["LastName"],
                            Email = rdr["E-mail"],
                            Type = rdr["Type"],
                            Parent = ((rdr["Parent"] == DBNull.Value) ? "No Parent" : rdr["Parent"]),
                            Phone = rdr["Phone"],
                            City = rdr["City"],
                            State = rdr["State"],
                            Title = rdr["Title"],
                            EmployeeID = rdr["EmployeeID"],
                            ID = rdr["CustomerNUmber"],
                            SPL_Type = Database.GetSPL_Type(GetBossID(ID))
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

        public static bool RecordSPL(String UserID, int BossID, double SPL, String GPS, String Weather, ref Boolean shouldSend, double Windspeed, int Winddirection)
        {
            CreateSPlTable(BossID);
            return RecordSPLHelper(UserID, BossID, SPL, GPS, Weather, ref shouldSend, Windspeed, Winddirection);
        }

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
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("M/d/yyyy"));
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
                            Debug.WriteLine(rdr.GetString(0));
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

        public static List<SPL> GetSPLValues (int BossID)
        {
            CreateSPlTable(BossID);
            List<SPL> information = new List<SPL>();

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    string commandText = @"SELECT FirstName, LastName, `GPS_Loc`, SPL, ID FROM " + BossID + "_SPL INNER JOIN User ON User.CustomerNumber=" + BossID + "_SPL.ID";
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

        private static void IsUserInSameLocation(String UserID, String GPS, int BossID, ref Boolean shouldSend)
        {
            String date = DateTime.Now.ToString("M/d/yyyy");

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

        private static bool CreateSPlTable(int BossID)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    String commandText = "CREATE TABLE IF NOT EXISTS " + BossID + "_SPL (ID int NOT NULL,Date char(100) NOT NULL,Time char(100) NOT NULL,Weather text,Phone char(100),GPS_Loc char(100),SPL DOUBLE NOT NULL, WindSpeed DOUBLE NOT NULL, WindDirection char(100) NOT NULL, PRIMARY KEY (Date, Time))";
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
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("M/d/yyyy"));
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
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("M/d/yyyy"));
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

        public static String GetRolesJSON(String UserID)
        {
            int ID = GetBossID(UserID);
            string result = "";

            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"SELECT ID, Type, Can_Create From " + ID + "_Arch";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                        if (!populateResult(ref result, rdr))
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

        public static int UpdateEmployeeInfo(String FirstName, String LastName, String Email, String Phone, String EmployeeID, String City, String Title, String State, String Role, String Parent, String ID)
        {
            if (!UpdateUsertable(FirstName, LastName, Email, ID))
                return -1;

            return UpdateCompanyUsertable(Phone, EmployeeID, City, Title, State, Role, Parent, ID) == false ? -2 : 0;
        }

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

        private static bool UpdateUsertable(String FirstName, String LastName, String Email, String ID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string stm = @"UPDATE User SET FirstName=@first, LastName=@last, `E-mail`=@email WHERE CustomerNumber=@id";

                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.Parameters.AddWithValue("@first", FirstName);
                    cmd.Parameters.AddWithValue("@last", LastName);
                    cmd.Parameters.AddWithValue("@email", Email);
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
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("M/d/yyyy"));
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

        private static string getKey(String Error)
        {
            String key = "";

            for (int i = Error.Length - 2; i >= 0 && Error[i] != '\''; i--)
                key = Error[i] + key;

            return key;
        }

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