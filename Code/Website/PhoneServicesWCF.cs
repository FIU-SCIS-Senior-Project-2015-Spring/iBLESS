using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace WebApplication1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface PhoneServicesWCF
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        String RegisterSPL(String UserID, double SPL, String GPS, String Weather, double Windspeed, int Winddirection);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void SetPhoneInformation(String UserID, int IMEI, int MSISDN, int IMSI, String MAC, String Brand, String PhoneNumber, String PhoneModel, String Carrier, String PhoneIP);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        String Login(String Username, String Password);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void BLEDeviceDisconnected(String UserID);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void BLEDeviceConnected(String UserID);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void ExcessiveSessions(String UserID);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void LogError(String UserID, int ErrorID, String GPS, String Weather);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void LogNotification(String UserID, String GPS, String Weather, String CellID, String AccessoryStatus, String VibrationSource, String VibrationEffect, int TimeVibrated, String Accelerometer, bool InGeofence);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void SetBLEInformation(String UserID, int IMEI, int MSISDN, int IMSI, String MAC);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool ForgotPassword(String Email);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool RegisterUser(String Username, String Firstname, String Lastname, String Email, String Password);
    }
}
