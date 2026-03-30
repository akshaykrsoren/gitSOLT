using CaregiverLiteWCF;
using DifferenzLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CaregiverLite.Models
{
    public class EmployeeClientModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    
        public async Task<string> SubmitRequestDat(string request,int OfficeId)
        {

            string result = "";
            string result1 = "";

            string finalresult = "";

            string actheader = "";
            string Token = "";

            string datajson = request;
            try
            {
                var clientGetDialogId = new System.Net.Http.HttpClient();

                //Token = ConfigurationManager.AppSettings["Token"].ToString();

                switch (OfficeId)
                {
                    case 1:
                        Token = ConfigurationManager.AppSettings["TokenForMA"].ToString();
                        actheader = ConfigurationManager.AppSettings["mykeyMA"].ToString();
                        break;
                    case 5:
                        Token = ConfigurationManager.AppSettings["Token"].ToString();
                        actheader = ConfigurationManager.AppSettings["mykey"].ToString();
                        break;
                    case 12:
                        Token = ConfigurationManager.AppSettings["TokenForCASD"].ToString();
                        actheader = ConfigurationManager.AppSettings["mykeyCASD"].ToString();
                        break;
                }

                //if (OfficeId == 5)
                //{
                //    Token = ConfigurationManager.AppSettings["Token"].ToString();
                //    actheader = ConfigurationManager.AppSettings["mykey"].ToString();
                //}
                //else
                //{
                //    Token = ConfigurationManager.AppSettings["TokenForCASD"].ToString();
                //    actheader = ConfigurationManager.AppSettings["mykeyCASD"].ToString();
                //}

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_Client"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_ClientStatus"].ToString();

                clientGetDialogId.BaseAddress = new Uri(API_Url);

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/clients/rest/api/v1.1");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);

                result = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result);

                string ID = data1["id"].Value<string>();
                string UDID= data1["data"]["uuid"].Value<string>();

                insertClientData(ID,UDID,result,request);

                Thread.Sleep(10000);

                var clientGetId = new System.Net.Http.HttpClient();
            
                clientGetId.BaseAddress = new Uri(API_Url_Status + UDID);

                clientGetId.DefaultRequestHeaders.Accept.Clear();
                clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetId.DefaultRequestHeaders.Add("account", actheader);
                // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response1 = await clientGetId.GetAsync("");

                result1 = response1.Content.ReadAsStringAsync().Result;

                var data2 = (JObject)JsonConvert.DeserializeObject(result);
                finalresult = data2["data"]["message"].Value<string>();

                insertClientData(ID, UDID, result1,"");
            }
            catch (Exception ex)
            {
                insertClientData("ClientData", ex.Message,result,"");
                return finalresult = "success";
            }

             return finalresult = "success";
        }


        public async Task<string> SubmitRequestDatAllMed(string request)
        {

            string result = "";
            string result1 = "";

            string finalresult = "";

            string datajson = request;
            try
            {
                var clientGetDialogId = new System.Net.Http.HttpClient();

                string Token = ConfigurationManager.AppSettings["TokenMed"].ToString();
                string actheader = ConfigurationManager.AppSettings["mykeyMed"].ToString();

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_ClientMed"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_ClientStatusMed"].ToString();

                clientGetDialogId.BaseAddress = new Uri(API_Url);

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/clients/rest/api/v1.1");

                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);

                result = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result);

                string ID = data1["id"].Value<string>();
                //string UDID = data1["data"]["TransactionID"].Value<string>();

                insertClientData(ID, ID, result, request);

                Thread.Sleep(10000);

                var clientGetId = new System.Net.Http.HttpClient();

                clientGetId.BaseAddress = new Uri(API_Url_Status + ID);

                clientGetId.DefaultRequestHeaders.Accept.Clear();
                clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetId.DefaultRequestHeaders.Add("account", actheader);
                // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response1 = await clientGetId.GetAsync("");

                result1 = response1.Content.ReadAsStringAsync().Result;

                var data2 = (JObject)JsonConvert.DeserializeObject(result);
                finalresult = data2["data"]["Reason"].Value<string>();

                insertClientData(ID, ID, result1, "");
            }
            catch (Exception ex)
            {
                insertClientData("ClientData", ex.Message, result, "");
                return finalresult = "success";
            }

            return finalresult = "success";
        }

        public async Task<string> SubmitEmployeeRequestData(string request, int OfficeId)
        {

            string result = "";
            string result1 = "";
            string finalresult = "";
            string datajson = request;

            string Token = "";
            string actheader = "";

            try
            {
                var clientGetDialogId = new System.Net.Http.HttpClient();


                switch (OfficeId)
                {
                    case 1:
                        Token = ConfigurationManager.AppSettings["TokenForMA"].ToString();
                        actheader = ConfigurationManager.AppSettings["mykeyMA"].ToString();
                        break;
                    case 5:
                         Token = ConfigurationManager.AppSettings["Token"].ToString();
                         actheader = ConfigurationManager.AppSettings["mykey"].ToString();
                        break;
                    case 12:
                        Token = ConfigurationManager.AppSettings["TokenForCASD"].ToString();
                        actheader = ConfigurationManager.AppSettings["mykeyCASD"].ToString();
                        break;
                }


                //if (OfficeId == 5)
                //{
                //    Token = ConfigurationManager.AppSettings["Token"].ToString();
                //    actheader = ConfigurationManager.AppSettings["mykey"].ToString();
                //}
                //else
                //{
                //    Token = ConfigurationManager.AppSettings["TokenForCASD"].ToString();
                //    actheader = ConfigurationManager.AppSettings["mykeyCASD"].ToString();
                //}

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_Employee"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_EmployeeStatus"].ToString();

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1");

                clientGetDialogId.BaseAddress = new Uri(API_Url);

               // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);

                result = response2.Content.ReadAsStringAsync().Result;

                var data1 = (JObject)JsonConvert.DeserializeObject(result);
                string UDID = data1["data"]["uuid"].Value<string>();

                string ID= data1["id"].Value<string>();

                insertEmployeeData(ID, UDID, result,request);

                Thread.Sleep(10000);

                var clientGetId = new System.Net.Http.HttpClient();

                //   clientGetId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1/status?uuid=" + UDID);
                clientGetId.BaseAddress = new Uri(API_Url_Status + UDID);

                clientGetId.DefaultRequestHeaders.Accept.Clear();
                clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetId.DefaultRequestHeaders.Add("account", actheader);
                // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response1 = await clientGetId.GetAsync("");

                result1 = response1.Content.ReadAsStringAsync().Result;

                var data2 = (JObject)JsonConvert.DeserializeObject(result);
                finalresult = data2["data"]["message"].Value<string>();

                insertEmployeeData(ID, UDID, result1,"");
            }
            catch (Exception ex)
            {
                insertEmployeeData("EmployeeData", ex.Message, result,"");
                return finalresult = "success";
            }

            return finalresult="Success";
        }


        public async Task<string> SubmitEmployeeRequestDataAllMed(string request)
        {

            string result = "";
            string result1 = "";
            string finalresult = "";
            string datajson = request;

            try
            {
                var clientGetDialogId = new System.Net.Http.HttpClient();

                string Token = ConfigurationManager.AppSettings["TokenMed"].ToString();
                string actheader = ConfigurationManager.AppSettings["mykeyMed"].ToString();

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_EmployeeMed"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_EmployeeStatusMed"].ToString();

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1");

                clientGetDialogId.BaseAddress = new Uri(API_Url);

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);

                result = response2.Content.ReadAsStringAsync().Result;

                var data1 = (JObject)JsonConvert.DeserializeObject(result);
               // string UDID = data1["data"]["uuid"].Value<string>();

                string ID = data1["id"].Value<string>();

                insertEmployeeData(ID, ID, result, request);

                Thread.Sleep(10000);

                var clientGetId = new System.Net.Http.HttpClient();

                //   clientGetId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1/status?uuid=" + UDID);
                clientGetId.BaseAddress = new Uri(API_Url_Status + ID);

                clientGetId.DefaultRequestHeaders.Accept.Clear();
                clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetId.DefaultRequestHeaders.Add("account", actheader);
                // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response1 = await clientGetId.GetAsync("");

                result1 = response1.Content.ReadAsStringAsync().Result;

                var data2 = (JObject)JsonConvert.DeserializeObject(result);
              //  finalresult = data2["data"]["message"].Value<string>();

                insertEmployeeData(ID, ID, result1, "");
            }
            catch (Exception ex)
            {
                insertEmployeeData("EmployeeData", ex.Message, result, "");
                return finalresult = "success";
            }

            return finalresult = "Success";
        }

        public async Task<string> SubmitEmployeeVisitRequestData(string request)
        {

            string result = "";

            string result1 = "";

            string finalresult = "";
            string datajson = request;
            try
            {

                var clientGetDialogId = new System.Net.Http.HttpClient();
                string Token = ConfigurationManager.AppSettings["Token"].ToString();
                string actheader = ConfigurationManager.AppSettings["mykey"].ToString();

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetails"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetailsStatus"].ToString();

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1");

                clientGetDialogId.BaseAddress = new Uri(API_Url);

              // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1");

                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);
                result = response2.Content.ReadAsStringAsync().Result;

                var data1 = (JObject)JsonConvert.DeserializeObject(result);
                string UDID = data1["id"].Value<string>();

                insertVisitDetails(UDID, UDID,result,request);

                Thread.Sleep(10000);

                var clientGetId = new System.Net.Http.HttpClient();

               // clientGetId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1/status?uuid="+ UDID);

                clientGetId.BaseAddress = new Uri(API_Url_Status+ UDID);

                clientGetId.DefaultRequestHeaders.Accept.Clear();
                clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetId.DefaultRequestHeaders.Add("account", actheader);
                // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response1 = await clientGetId.GetAsync("");

                result1 = response1.Content.ReadAsStringAsync().Result;

                var data2 = (JObject)JsonConvert.DeserializeObject(result);
                finalresult = data2["data"]["message"].Value<string>();

                insertVisitDetails(UDID, UDID, result1,"");
            }
            catch (Exception ex)
            {
                insertVisitDetails("VisitDetailsData", ex.Message, result,"");
                return finalresult = "success";
                //  throw new Exception(ex.Message);
            }

            return finalresult="success";
        }

        //multiple Data Execution

        public async Task<string> SubmitRequestDatMultiple(string request)
        {

            string result = "";
            string result1 = "";

            string finalresult = "";

            string datajson = request;
            try
            {

                var clientGetDialogId = new System.Net.Http.HttpClient();

                string Token = ConfigurationManager.AppSettings["Token"].ToString();
                string actheader = ConfigurationManager.AppSettings["mykey"].ToString();

                string API_Url= System.Configuration.ConfigurationManager.AppSettings["SandData_Client"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_ClientStatus"].ToString();

                clientGetDialogId.BaseAddress = new Uri(API_Url);
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);

                result = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result);

                string ID = data1["id"].Value<string>();
                string UDID = data1["data"]["uuid"].Value<string>();

                insertClientData(ID, UDID, result, request);

                Thread.Sleep(10000);

                var clientGetId = new System.Net.Http.HttpClient();

                clientGetId.BaseAddress = new Uri(API_Url_Status + UDID);

                clientGetId.DefaultRequestHeaders.Accept.Clear();
                clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetId.DefaultRequestHeaders.Add("account", actheader);
                // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response1 = await clientGetId.GetAsync("");

                result1 = response1.Content.ReadAsStringAsync().Result;

                var data2 = (JObject)JsonConvert.DeserializeObject(result);
                finalresult = data1["status"].Value<string>();
                
                insertClientData(ID, UDID, result1, "");
            }
            catch (Exception ex)
            {
                insertClientData("ClientData", ex.Message, result, "");
                return finalresult = "success";
            }

            return finalresult;
        }

        public async Task<string> SubmitEmployeeRequestDataMultiple(string request)
        {

            string result = "";
            string result1 = "";

            string finalresult = "";

            string datajson = request;
            try
            {

                var clientGetDialogId = new System.Net.Http.HttpClient();

                string Token = ConfigurationManager.AppSettings["Token"].ToString();
                string actheader = ConfigurationManager.AppSettings["mykey"].ToString();

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_Employee"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_EmployeeStatus"].ToString();

               // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1");

                clientGetDialogId.BaseAddress = new Uri(API_Url);
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);

                result = response2.Content.ReadAsStringAsync().Result;

                var data1 = (JObject)JsonConvert.DeserializeObject(result);
                string UDID = data1["data"]["uuid"].Value<string>();

                string ID = data1["id"].Value<string>();

                insertEmployeeData(ID, UDID, result, request);

                finalresult = data1["status"].Value<string>();

                //Thread.Sleep(10000);

                //var clientGetId = new System.Net.Http.HttpClient();

                //clientGetId.BaseAddress = new Uri(" https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1/status?uuid=" + UDID);

                //clientGetId.DefaultRequestHeaders.Accept.Clear();
                //clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                //clientGetId.DefaultRequestHeaders.Add("account", actheader);
                //// var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //var response1 = await clientGetId.GetAsync("");

                //result1 = response1.Content.ReadAsStringAsync().Result;

                //var data2 = (JObject)JsonConvert.DeserializeObject(result);
                //finalresult = data2["data"]["message"].Value<string>();

                //insertEmployeeData(ID, UDID, result1, "");
            }
            catch (Exception ex)
            {
                insertEmployeeData("EmployeeData", ex.Message, result, "");

                return finalresult = "success";
            }

            return finalresult;
        }

        public async Task<string> SubmitEmployeeVisitRequestDataMultiple(string request)
        {

            string result = "";

            string result1 = "";

            string finalresult = "";
            string datajson = request;
            try
            {

                var clientGetDialogId = new System.Net.Http.HttpClient();
                string Token = ConfigurationManager.AppSettings["Token"].ToString();
                string actheader = ConfigurationManager.AppSettings["mykey"].ToString();

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetails"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetailsStatus"].ToString();

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/employees/rest/api/v1.1");

                clientGetDialogId.BaseAddress = new Uri(API_Url);


              //  clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1");

                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);
                result = response2.Content.ReadAsStringAsync().Result;

                var data1 = (JObject)JsonConvert.DeserializeObject(result);
                string UDID = data1["id"].Value<string>();

                insertVisitDetails(UDID, UDID, result, request);

                //Thread.Sleep(10000);

                //var clientGetId = new System.Net.Http.HttpClient();

                //clientGetId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1/status?uuid=" + UDID);

                //clientGetId.DefaultRequestHeaders.Accept.Clear();
                //clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                //clientGetId.DefaultRequestHeaders.Add("account", actheader);
                //// var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //var response1 = await clientGetId.GetAsync("");

                //result1 = response1.Content.ReadAsStringAsync().Result;

                //var data2 = (JObject)JsonConvert.DeserializeObject(result);
                //finalresult = data2["data"]["message"].Value<string>();

                //insertVisitDetails(UDID, UDID, result1, "");

                finalresult = data1["status"].Value<string>();
            }
            catch (Exception ex)
            {
                insertVisitDetails("VisitDetailsData", ex.Message, result, "");
                return finalresult = "success";
                //  throw new Exception(ex.Message);
            }

            return finalresult;

        }

        private void insertClientData(String Id, string UDID,string result,string request)
        {
          //  string result = "Testing";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertSandDataClient", Id, UDID,result,request);

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "InsertScheduleForNurse";
                //objErrorlog.UserID = CareGiverSchedule.UserId;
                //result = InsertErrorLog(objErrorlog);
            }
            //  return result;
        }


        private void insertEmployeeData(String Id, string UDID,string result,string request)
        {
           // string result = "Testing";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertSandDataEmployee", Id, UDID,result, request);

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "InsertScheduleForNurse";
                //objErrorlog.UserID = CareGiverSchedule.UserId;
                //result = InsertErrorLog(objErrorlog);
            }
            //  return result;
        }



        private void insertVisitDetails(String Id, string UDID, string result,string request)
        {
          //  string result = "Testing";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertVisitDetails", Id, UDID,result, request);

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "InsertScheduleForNurse";
                //objErrorlog.UserID = CareGiverSchedule.UserId;
                //result = InsertErrorLog(objErrorlog);
            }
            //  return result;
        }

    }
}