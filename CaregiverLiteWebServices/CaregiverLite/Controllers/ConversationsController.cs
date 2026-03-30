using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class ConversationsController : Controller
    {
        // GET: Conversations
        public ActionResult Conversations()
        {
            //ViewBag.SchedulerList = GetAllSchedulerList();
            //ViewBag.CaregiverList = GetAllCareGiversList();
            return View();
        }



        public JsonResult GetAllSchedulerList()
        {
            string DialogId = "";
            var objSchedulerList = new SchedulersList();
            try
            {
                var SchedulerService = new SchedulerServiceProxy();
                objSchedulerList = SchedulerService.GetAllSchedulerList().Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetAllSchedulerList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(objSchedulerList);
        }

        public JsonResult GetAllCareGiversList(string SchedulerUserId)
        {
            var objCareGiverList = new CareGiversList();
            try
            { 
                var CareGiverLiteService = new CareGiverServiceProxy();
                objCareGiverList = CareGiverLiteService.GetAllCareGiversList(SchedulerUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetAllCareGiversList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(objCareGiverList);
        }

        public async Task<ActionResult> ViewConversation(string ids)
        {
            var loginUserQBID = @Session["FromQBId"];
            AddCareGiverToDialodOnQuickBlox(ids);
            ViewBag.ids = ids;
            return PartialView();
        }

        //public async void AddCareGiverToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, int CaregiverQBId)
        private async void AddCareGiverToDialodOnQuickBlox(string Ids)
        {
            //----
            //var DialogId = "5970c535a0eb47745bd706ed";//"poll@gmail.com_28841699_59452f5fa0eb470270829ff4_superadmin@caregiver.com"
            //var GroupName = "Hardik21(Hardik1)";
            //var SchedulerEmail = "superadmin@caregiver.com";
            //var CaregiverQBId = "29262138";
            ////Sessoin Generated Start

            char d = '_';
            String[] s1 = Ids.Split(d);
            //ToUserQuickBloxId = s1[0];
            //ToUserUserId = s1[2];
            //ToUserEmail = s1[3];

            var DialogId = s1[2];
            var GroupName = s1[0];
            var SchedulerEmail = s1[3];
            var CaregiverQBId = s1[1];

            var client = new System.Net.Http.HttpClient();

            client.BaseAddress = new Uri("https://api.quickblox.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

            Random random = new Random();
            int Vnonce = random.Next(0, 9999);
            string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

            var input = new QuickBloxSession();
            input.application_id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
            input.auth_key = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
            input.nonce = Vnonce.ToString();
            input.timestamp = Timestamp();

            input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=superadmin@caregiver.com&user[password]=Welcome007!";
            //Encryption            
            input.signature = Hash(input.signature, System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

            var userData = new Userdata();
            userData.login = SchedulerEmail; //"superadmin@caregiver.com"
            userData.password = "Welcome007!";
            input.user = userData;

            var jData1 = JsonConvert.SerializeObject(input);
            var content1 = new StringContent(jData1);

            var content = new StringContent(jData1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/session.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            //JObject json = JObject.Parse(result);
            var data = (JObject)JsonConvert.DeserializeObject(result);
            string token = data["session"]["token"].Value<string>();

            ////Sessoin Generated End

            //GetActivePatientRequest All Dialog Detail
            var clientGetDialogId = new System.Net.Http.HttpClient();

            clientGetDialogId.BaseAddress = new Uri("https://api.quickblox.com/chat/Dialog.json");
            clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
            var response1 = await clientGetDialogId.GetAsync("");
            var result1 = response1.Content.ReadAsStringAsync().Result;
            // new 
            var MyData = JsonConvert.DeserializeObject<QuickbloxReponse>(result1);
            var tempOccupants_ids = new List<int>();
            for (int p = 0; p < MyData.total_entries; p++)
            {
                var currentrow = MyData.items[p];
                string tempDialogId = currentrow._id;
                if (tempDialogId == DialogId)// (tempDialogId == "596f0f2da0eb4770e6d705d3") //(tempDialogId == DialogId)
                {
                    tempOccupants_ids.AddRange(currentrow.occupants_ids);
                    break;
                }
            }
            // if() //if occupants_ids not match then Call to Add in group
            bool flag = false;
            for (int k = 0; k < tempOccupants_ids.Count; k++)
            {
                if (Convert.ToInt32(CaregiverQBId) == tempOccupants_ids[k])
                {
                    flag = true;
                    break;
                }
            }
            //Add Member to group
            if (flag == false)
            {

                var objAddDialog = new AddDialog();
                List<int> objoccupants_ids = new List<int>();
                objoccupants_ids.Add(Convert.ToInt32(CaregiverQBId));
                try
                {
                    objAddDialog.name = GroupName;
                    var objPullAll = new PullAll();
                    objPullAll.occupants_ids = objoccupants_ids;

                    objAddDialog.push_all = objPullAll;
                }
                catch (Exception e)
                { }

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri("https://api.quickblox.com/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;
            }
        }


        public string Timestamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }


        public string Hash(string input, string key)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            HMACSHA1 myhmacsha1 = new HMACSHA1(keyByte);
            byte[] byteArray = encoding.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            byte[] hashValue = myhmacsha1.ComputeHash(stream.ToArray());
            return string.Join("", (hashValue.Select(b => b.ToString("x2"))));
        }
    }
}