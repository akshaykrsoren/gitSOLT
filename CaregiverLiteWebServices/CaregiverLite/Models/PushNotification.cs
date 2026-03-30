using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CaregiverLite.Models
{
    public class PushNotificationServiceProxy : CaregiverLiteBaseService
    {
        public PushNotificationServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> InsertPushNotification(Notification Notification)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertPushNotification", new { Notification }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PushNotificationServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }

        public async Task<string> InsertSentNotification(Notification Notification)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertSentNotification", new { Notification }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PushNotificationServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }

        public string Result { get; set; }
    }

    public class PushNotification
    {
        AndroidResponse andrroid_response = new AndroidResponse();
        int successcounter = 0;
        int failedcounter = 0;
        string failed_deviceids = "";

        public string SendPushNotification(string Title, string Msg, string DeviceToken, string DeviceType, string UserId, string AppointmentId, string NurseId, string PatientId, string Type, string InsertUserId,  bool IsCareGiver, string additionalData ="" , int PatientRequestId = 0)
        {
            try
            {
                string UserList = UserId;
                Notification obj = new Notification();
                IosResponse ios_response = new IosResponse();
                AndroidResponse android_response = new AndroidResponse();
                PushNotificationServiceProxy PushNotificationService = new PushNotificationServiceProxy();

                if (!string.IsNullOrEmpty(DeviceToken))
                {
                    string[] deviceids = new string[1];
                    string[] Userids = new string[1];
                    string[] androiddeviceids = new string[1];

                    if (DeviceType.ToLower() == "ios")
                    {
                        deviceids[0] = DeviceToken;
                    }
                    else if (DeviceType.ToLower() == "android")
                    {
                        androiddeviceids[0] = DeviceToken;
                    }


                    obj.UserList = UserList;
                    obj.Title = Title;
                    obj.Message = Msg;
                    obj.InsertUserId = InsertUserId;
                    int NotificationId = int.Parse(PushNotificationService.InsertPushNotification(obj).Result);
                    if (IsCareGiver)
                    {

                        ios_response = Send_IphoneNotification_Multy_CareGiver(deviceids, obj.Title, obj.Message, 0,PatientRequestId, additionalData);
                    }
                    else
                    {
                        ios_response = Send_IphoneNotification_Multy_Patient(deviceids, obj.Title, obj.Message, 0 , PatientRequestId);
                    }
                    android_response = SendNotification_Android_Multy(androiddeviceids.ToArray(), obj.Title, obj.Message, PatientRequestId, additionalData);
                    obj.IosResponse = ios_response;
                    obj.AndroidResponse = android_response;
                    obj.NotificationId = NotificationId;
                    string res = PushNotificationService.InsertSentNotification(obj).Result;
                    return "Success";
                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PushNotification Class";
                log.Methodname = "SendPushNotification";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string Result = ErrorLogService.InsertErrorLog(log).Result;
            }
            return "Fail";
        }

        public AndroidResponse SendNotification_Android_Multy(string[] MobileRegIDs,  string title, string vMsg, int PatientRequestId = 0, string additionalData = "")
        {
            try
            {
                andrroid_response.Response = "";
                andrroid_response.Json = "";

                AndroidResponse andrroid_res = new AndroidResponse();
                string message = vMsg;
                string BrowserAPIKey = ConfigurationManager.AppSettings["BrowserAPIKey"].ToString();
                string deviceRegID = "";

                int numberofDevice = MobileRegIDs.Length;
                int no = numberofDevice / 1000;
                int deviceid = 0;
                for (int i = 0; i <= no && numberofDevice > 0; i++)
                {
                    deviceRegID = "";
                    for (int j = 0; j < 1000 && j < numberofDevice; j++)
                    {
                        deviceRegID = deviceRegID + MobileRegIDs[deviceid] + "\",\"";
                        deviceid++;
                    }
                    deviceRegID = deviceRegID.Substring(0, deviceRegID.Length - 3);

                    andrroid_res = AndroidRequestJson(deviceRegID, title, message, BrowserAPIKey, PatientRequestId, additionalData);

                    andrroid_response.Response = andrroid_response.Response + andrroid_res.Response + "||";
                    andrroid_response.Json = andrroid_response.Json + andrroid_res.Json + "||";

                    numberofDevice = numberofDevice - 1000;
                }

                if (andrroid_response.Response.Contains("error"))
                {
                    andrroid_response.Status = "Failure";
                }
                else
                {
                    andrroid_response.Status = "Success";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PushNotification Class";
                log.Methodname = "SendNotification_Android_Multy";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string Result = ErrorLogService.InsertErrorLog(log).Result;
            }
            return andrroid_response;
        }

        private AndroidResponse AndroidRequestJson(string deviceids, string title, string message, string BrowserAPIKey, int PatientRequestId = 0,  string additionalData = "")
        {
            AndroidResponse androidresponse = new AndroidResponse();
            try
            {
                androidresponse.Json = "";

                androidresponse.Json = androidresponse.Json + "{ \"registration_ids\": [ \"" + deviceids + "\" ], ";
                androidresponse.Json = androidresponse.Json + "  \"data\": {\"description\":\"" + message + "\", ";
                androidresponse.Json = androidresponse.Json + "  \"PatientRequestId\":" + PatientRequestId + ", ";
                androidresponse.Json = androidresponse.Json + additionalData;
                androidresponse.Json = androidresponse.Json + "  \"message\": \"" + title + "\"}}";

                androidresponse.Response = Android_Send(BrowserAPIKey, androidresponse.Json);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PushNotification Class";
                log.Methodname = "AndroidRequestJson";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string Result = ErrorLogService.InsertErrorLog(log).Result;
            }
            return androidresponse;
        }

        private string Android_Send(string apiKey, string postData, string postDataContentType = "application/json")
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //  MESSAGE CONTENT
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //  CREATE REQUEST
            //HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            Request.ContentLength = byteArray.Length;
            Request.UseDefaultCredentials = true;
            Request.PreAuthenticate = true;
            Request.Credentials = CredentialCache.DefaultCredentials;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //  SEND MESSAGE
            try
            {
                WebResponse Response = Request.GetResponse();

                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;

                string text = "";
                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    text = "Unauthorized - need new token";
                }
                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    text = "Response from web service isn't OK";
                }

                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Reader.Close();

                return responseLine;
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "PushNotification Class";
                log.Methodname = "Android_Send";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string Result = ErrorLogService.InsertErrorLog(log).Result;
                return "error :" + ex.Message;
            }
        }

        // new iphone notification 
        public string Send_IphoneNotification(string deviceID, string message, string activity, string propertyid, int BadgeCount = 0)
        {
            //Set Default Values
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["PortNo"].ToString()); ;
            String hostname = ConfigurationManager.AppSettings["HostName"].ToString();
            String certificatePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CertFileName"].ToString());
            String password = ConfigurationManager.AppSettings["CertPassword"].ToString();
            Boolean IsAPNSandBox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAPNSandBox"]);

            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), password, X509KeyStorageFlags.MachineKeySet);

            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);
            SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            string payload = "";
            try
            {
                sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, IsAPNSandBox);
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)32);

                writer.Write(ConvertHexStringToByteArray(deviceID.ToUpper()));
                //String payload1 = "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":" + BadgeCount + ",\"activity\" :\"" + activity + "\", \"sound\":\"default\"}}";
                payload = "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":" + BadgeCount + ",\"propertyid\" :" + propertyid + ",\"storyboardname\" :\"Main\",\"storyboardviewname\" :\"" + activity + "\" ,\"sound\":\"default\"}}";
                //String payload2 = "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":" + BadgeCount + ",\"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
                return payload; ;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
                return ex.Message.ToString(); ;
            }
            catch (Exception e)
            {
                client.Close();
                return e.Message.ToString();
            }
        }

        public IosResponse Send_IphoneNotification_Multy_CareGiver(string[] deviceIDs, string title, string message, int BadgeCount = 0, int PatientRequestId = 0, string additionalData = "")
        {
            IosResponse ios_response = new IosResponse();
            //Set Default Values
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["PortNo"].ToString());
            String hostname = ConfigurationManager.AppSettings["HostName"].ToString();

            String certificatePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CertFileName"].ToString());
            String password = ConfigurationManager.AppSettings["CertPassword"].ToString();
            Boolean IsAPNSandBox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAPNSandBox"]);

            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), password, X509KeyStorageFlags.MachineKeySet);

            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);

            try
            {
                String payload = "";

                for (int i = 0; i < deviceIDs.Length; i++)
                {
                    try
                    {
                        client = new TcpClient(hostname, port);
                        SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                        sslStream.WriteTimeout = 5000;
                        sslStream.ReadTimeout = 5000;
                        sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, IsAPNSandBox);

                        MemoryStream memoryStream = new MemoryStream();
                        BinaryWriter writer = new BinaryWriter(memoryStream);
                        writer.Write((byte)0);
                        writer.Write((byte)0);
                        writer.Write((byte)32);

                        writer.Write(ConvertHexStringToByteArray(deviceIDs[i].ToUpper()));
                        payload = "{\"aps\":{\"alert\":\"" + message + "\"," +
                                    "\"badge\":" + BadgeCount + "," +
                                    "\"Title\" :\"" + title + "\"," +
                                    "\"PatientRequestId\" :\"" + PatientRequestId + "\"," +
                                    additionalData + 
                                    "\"sound\":\"default\"}}";
                        ios_response.Json = payload;
                        writer.Write((byte)0);
                        writer.Write((byte)payload.Length);
                        byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                        writer.Write(b1);
                        writer.Flush();
                        byte[] array = memoryStream.ToArray();
                        sslStream.Write(array);
                        sslStream.Flush();
                        client.Close();

                        successcounter = successcounter + 1;
                    }
                    catch (Exception innerexception)
                    {
                        failed_deviceids += innerexception.Message.ToString() + " " + deviceIDs[i] + "||";
                        failedcounter = failedcounter + 1;
                    }
                }
                if (failed_deviceids.Length > 0)
                {
                    failed_deviceids = failed_deviceids.Substring(0, failed_deviceids.Length - 2);
                }

                client.Close();
                ios_response.Status = "Success : " + successcounter + "||" + "Failed :" + failedcounter;
                ios_response.Response = failed_deviceids;
                return ios_response;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
                ios_response.Response = ex.Message.ToString() + " " + failed_deviceids;
                ios_response.Status = "Failed  at  " + failed_deviceids;
                return ios_response;
            }
            catch (Exception e)
            {
                client.Close();
                ios_response.Response = e.Message.ToString() + " " + failed_deviceids;
                ios_response.Status = "Failed";
                return ios_response;
            }
        }

        public IosResponse Send_IphoneNotification_Multy_Patient(string[] deviceIDs, string title, string message, int BadgeCount = 0, int PatientRequestId = 0)
        {
            IosResponse ios_response = new IosResponse();
            //Set Default Values
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["PortNo"].ToString()); ;
            String hostname = ConfigurationManager.AppSettings["HostName"].ToString();

            String certificatePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PatientCertFileName"].ToString());
            String password = ConfigurationManager.AppSettings["PatientCertPassword"].ToString();
            Boolean IsAPNSandBox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAPNSandBox"]);

            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), password, X509KeyStorageFlags.MachineKeySet);

            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);

            try
            {
                String payload = "";

                for (int i = 0; i < deviceIDs.Length; i++)
                {
                    try
                    {
                        client = new TcpClient(hostname, port);
                        SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                        sslStream.WriteTimeout = 5000;
                        sslStream.ReadTimeout = 5000;
                        sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, IsAPNSandBox);

                        MemoryStream memoryStream = new MemoryStream();
                        BinaryWriter writer = new BinaryWriter(memoryStream);
                        writer.Write((byte)0);
                        writer.Write((byte)0);
                        writer.Write((byte)32);

                        writer.Write(ConvertHexStringToByteArray(deviceIDs[i].ToUpper()));
                        payload = "{\"aps\":{\"alert\":\"" + message + "\"," +
                                    "\"badge\":" + BadgeCount + "," +
                                    "\"Title\" :\"" + title + "\"," +
                                    "\"PatientRequestId\" :\"" + PatientRequestId + "\"," +
                                    "\"sound\":\"default\"}}";
                        ios_response.Json = payload;
                        writer.Write((byte)0);
                        writer.Write((byte)payload.Length);
                        byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                        writer.Write(b1);
                        writer.Flush();
                        byte[] array = memoryStream.ToArray();
                        sslStream.Write(array);
                        sslStream.Flush();
                        client.Close();

                        successcounter = successcounter + 1;
                    }
                    catch (Exception innerexception)
                    {
                        failed_deviceids += innerexception.Message.ToString() + " " + deviceIDs[i] + "||";
                        failedcounter = failedcounter + 1;
                    }
                }
                if (failed_deviceids.Length > 0)
                {
                    failed_deviceids = failed_deviceids.Substring(0, failed_deviceids.Length - 2);
                }

                client.Close();
                ios_response.Status = "Success : " + successcounter + "||" + "Failed :" + failedcounter;
                ios_response.Response = failed_deviceids;
                return ios_response;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
                ios_response.Response = ex.Message.ToString() + " " + failed_deviceids;
                ios_response.Status = "Failed  at  " + failed_deviceids;
                return ios_response;
            }
            catch (Exception e)
            {
                client.Close();
                ios_response.Response = e.Message.ToString() + " " + failed_deviceids;
                ios_response.Status = "Failed";
                return ios_response;
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }


        public IosResponse Send_IphoneNotification_Multy_CareGiver_FORCHATTING(string[] deviceIDs, string message, string title, string GroupName, string DialogId, int BadgeCount = 0, string Permission = null)
        {
            IosResponse ios_response = new IosResponse();
            //Set Default Values
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["PortNo"].ToString()); ;
            String hostname = ConfigurationManager.AppSettings["HostName"].ToString();

            String certificatePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CertFileName"].ToString());
            String password = ConfigurationManager.AppSettings["CertPassword"].ToString();
            Boolean IsAPNSandBox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAPNSandBox"]);

            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), password, X509KeyStorageFlags.MachineKeySet);

            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);

            try
            {
                String payload = "";

                for (int i = 0; i < deviceIDs.Length; i++)
                {
                    try
                    {
                        client = new TcpClient(hostname, port);
                        SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                        sslStream.WriteTimeout = 5000;
                        sslStream.ReadTimeout = 5000;
                        //sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, IsAPNSandBox);
                        sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, IsAPNSandBox);

                        MemoryStream memoryStream = new MemoryStream();
                        BinaryWriter writer = new BinaryWriter(memoryStream);
                        writer.Write((byte)0);
                        writer.Write((byte)0);
                        writer.Write((byte)32);

                        //escapeSequenceChar 
                        var escapeSequenceChar = new string[,] { 
                            //{ "/", @"\\" },
                            { "\\", @"\\" },
                            { "’", @"\'" }, 
                            { "”", @"\'" }, 
                            { "`", @"\'" }, 
                            { "'", @"\\'" },
                            { "\"".ToString(), @"\'" }
                           
                        };
                       
                        for (int v = 0; v < escapeSequenceChar.Length/2 ; v++) 
                        {
                            message = message.Replace(escapeSequenceChar[v,0], escapeSequenceChar[v, 1]);
                            title = title.Replace(escapeSequenceChar[v, 0], escapeSequenceChar[v, 1]);
                            GroupName = GroupName.Replace(escapeSequenceChar[v, 0], escapeSequenceChar[v, 1]);
                        }
                        
                        //escapeSequenceChar 

                        writer.Write(ConvertHexStringToByteArray(deviceIDs[i].ToUpper()));
                        payload = "{\"aps\":{\"alert\":\"" + message + "\"," +
                                    "\"badge\":" + BadgeCount + "," +
                                    "\"Title\" :\"" + title + "\"," +
                                    "\"sound\":\"default\"," +
                                    "\"param\":{\"GroupName\":\"" + GroupName + "\"," + "\"DialogId\":\"" + DialogId + "\"," 
                                    + "\"Permission\":\"" + Permission + "\" "
                                    + "}}}";
                        ios_response.Json = payload;
                        writer.Write((byte)0);
                        writer.Write((byte)payload.Length);
                        byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                        writer.Write(b1);
                        writer.Flush();
                        byte[] array = memoryStream.ToArray();
                        sslStream.Write(array);
                        sslStream.Flush();
                        client.Close();

                        successcounter = successcounter + 1;
                    }
                    catch (Exception innerexception)
                    {
                        failed_deviceids += innerexception.Message.ToString() + " " + deviceIDs[i] + "||";
                        failedcounter = failedcounter + 1;
                    }
                }
                if (failed_deviceids.Length > 0)
                {
                    failed_deviceids = failed_deviceids.Substring(0, failed_deviceids.Length - 2);
                }

                client.Close();
                ios_response.Status = "Success : " + successcounter + "||" + "Failed :" + failedcounter;
                ios_response.Response = failed_deviceids;
                return ios_response;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
                ios_response.Response = ex.Message.ToString() + " " + failed_deviceids;
                ios_response.Status = "Failed  at  " + failed_deviceids;
                return ios_response;
            }
            catch (Exception e)
            {
                client.Close();
                ios_response.Response = e.Message.ToString() + " " + failed_deviceids;
                ios_response.Status = "Failed";
                return ios_response;
            }
        }

        public AndroidResponse SendNotification_Android_Multy_FORCHATTING(string[] MobileRegIDs, string vMsg, string Type, string GroupName, string DialogId, string Permission = null, int BadgeCount = 0)
        {
            andrroid_response.Response = "";
            andrroid_response.Json = "";

            AndroidResponse andrroid_res = new AndroidResponse();
            string message = vMsg;
            string BrowserAPIKey = ConfigurationManager.AppSettings["BrowserAPIKey"].ToString();
            string deviceRegID = "";
            string title = "";

            int numberofDevice = MobileRegIDs.Length;
            int no = numberofDevice / 1000;
            int deviceid = 0;
            for (int i = 0; i <= no && numberofDevice > 0; i++)
            {
                deviceRegID = "";
                for (int j = 0; j < 1000 && j < numberofDevice; j++)
                {
                    deviceRegID = deviceRegID + MobileRegIDs[deviceid] + "\",\"";
                    deviceid++;
                }
                deviceRegID = deviceRegID.Substring(0, deviceRegID.Length - 3);

                if (Type == "2")
                {
                    title = "You have message in " + GroupName + " Group";
                }
                if (Type == "1")
                {
                    title = "You have message in " + GroupName + "";
                }
                andrroid_res = AndroidRequestJson_FORCHATTING(deviceRegID, Type, title, message, GroupName, DialogId, BrowserAPIKey,Permission, BadgeCount);


                andrroid_response.Response = andrroid_response.Response + andrroid_res.Response + "||";
                andrroid_response.Json = andrroid_response.Json + andrroid_res.Json + "||";

                numberofDevice = numberofDevice - 1000;
            }

            if (andrroid_response.Response.Contains("error"))
            {
                andrroid_response.Status = "Failure";
            }
            else
            {
                andrroid_response.Status = "Success";
            }

            return andrroid_response;
        }

        private AndroidResponse AndroidRequestJson_FORCHATTING(string deviceids, string Type, string title, string message, string GroupName, string DialogId, string BrowserAPIKey, string Permission = null, int BadgeCount = 0)
        {
            AndroidResponse androidresponse = new AndroidResponse();
            //escapeSequenceChar 
            var escapeSequenceChar = new string[,] { 
                            //{ "/", @"\\" },
                            { "\\", @"\\" },
                            { "’", @"\'" },
                            { "”", @"\'" },
                            { "`", @"\'" },
                            { "'", @"\\'" },
                            { "\"".ToString(), @"\'" }

                        };

            for (int v = 0; v < escapeSequenceChar.Length / 2; v++)
            {
                message = message.Replace(escapeSequenceChar[v, 0], escapeSequenceChar[v, 1]);
                title = title.Replace(escapeSequenceChar[v, 0], escapeSequenceChar[v, 1]);
                GroupName = GroupName.Replace(escapeSequenceChar[v, 0], escapeSequenceChar[v, 1]);
            }
            //escapeSequenceChar 


            androidresponse.Json = "";

            androidresponse.Json = androidresponse.Json + "{ \"registration_ids\": [ \"" + deviceids + "\" ], ";
            androidresponse.Json = androidresponse.Json + "  \"data\": {\"description\":\"" + message + "\", ";
            androidresponse.Json = androidresponse.Json + "  \"message\": \"" + title + "\",";
            androidresponse.Json = androidresponse.Json + "  \"GroupName\": \"" + GroupName + "\",";
            androidresponse.Json = androidresponse.Json + "  \"DialogId\": \"" + DialogId + "\",";
            androidresponse.Json = androidresponse.Json + "  \"Permission\": \"" + Permission + "\",";
            androidresponse.Json = androidresponse.Json + "  \"BadgeCount\": \"" + BadgeCount + "\",";
            
            androidresponse.Json = androidresponse.Json + "  \"Type\": \"" + Type + "\"}}";


            androidresponse.Response = Android_Send(BrowserAPIKey, androidresponse.Json);

            return androidresponse;
        }


    }
}