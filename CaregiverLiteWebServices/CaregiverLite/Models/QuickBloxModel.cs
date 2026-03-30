using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CaregiverLite.Models
{
    public class QuickBloxModel
    {
    }
    public class QuickBloxServiceProxy : CaregiverLiteBaseService
    {
        public static string Timestamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }
        public static string Hash(string input, string key)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            HMACSHA1 myhmacsha1 = new HMACSHA1(keyByte);
            byte[] byteArray = encoding.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            byte[] hashValue = myhmacsha1.ComputeHash(stream.ToArray());
            return string.Join("", (hashValue.Select(b => b.ToString("x2"))));
        }

        private async void AddUserToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, int CaregiverQBId)
        {

            var client = new System.Net.Http.HttpClient();
             string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();
            client.BaseAddress = new Uri(QuickbloxAPIUrl);
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

            input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
            //Encryption            
            input.signature = Hash(input.signature, System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

            var userData = new Userdata();
            userData.login = SchedulerEmail; //"superadmin@caregiver.com"
            userData.password = "Welcome007!";
            input.user = userData;

            var jData1 = JsonConvert.SerializeObject(input);
            var content1 = new StringContent(jData1);




            var content = new StringContent(jData1, Encoding.UTF8, "application/json");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var response = await client.PostAsync("/session.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            //JObject json = JObject.Parse(result);
            var data = (JObject)JsonConvert.DeserializeObject(result);
            string token = data["session"]["token"].Value<string>();

            ////Sessoin Generated End

            //GetActivePatientRequest All Dialog Detail
            var clientGetDialogId = new System.Net.Http.HttpClient();

            clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
            clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
            var response1 = await clientGetDialogId.GetAsync("");
            var result1 = response1.Content.ReadAsStringAsync().Result;
            // new 
            var MyData = JsonConvert.DeserializeObject<QuickbloxReponse>(result1);
            var tempOccupants_ids = new List<int>();

            var loopCnt = Math.Floor(Convert.ToDecimal(MyData.total_entries / 100) + 1);
            var forlooplnt = 0;
            var forLoopflag = 0;
            for (int p = 0; p < MyData.total_entries; p++)
            {
                if (forLoopflag == 0)
                {
                    if (p < 100)
                    {
                        var currentrow = MyData.items[p];
                        string tempDialogId = currentrow._id;
                        if (tempDialogId == DialogId)// (tempDialogId == "596f0f2da0eb4770e6d705d3") //(tempDialogId == DialogId)
                        {
                            tempOccupants_ids.AddRange(currentrow.occupants_ids);
                            forLoopflag = 1;
                            break;
                        }
                    }
                    else
                    {

                        forlooplnt++;
                        var skip = forlooplnt * 100;
                        var clientGetDialogId2 = new System.Net.Http.HttpClient();
                        clientGetDialogId2.BaseAddress = new Uri(QuickbloxAPIUrl +  "/chat/Dialog.json?limit=100&skip=" + skip);
                        clientGetDialogId2.DefaultRequestHeaders.Accept.Clear();
                        clientGetDialogId2.DefaultRequestHeaders.Add("QB-Token", token);
                        var response2 = await clientGetDialogId2.GetAsync("");
                        var result2 = response2.Content.ReadAsStringAsync().Result;
                        // new 
                        var MyData2 = JsonConvert.DeserializeObject<QuickbloxReponse>(result2);
                        for (int q = 0; q < MyData2.limit; q++)
                        {
                            var currentrow2 = MyData2.items[q];
                            string tempDialogId = currentrow2._id;
                            if (tempDialogId == DialogId)// (tempDialogId == "596f0f2da0eb4770e6d705d3") //(tempDialogId == DialogId)
                            {
                                tempOccupants_ids.AddRange(currentrow2.occupants_ids);
                                forLoopflag = 1;
                                break;
                            }
                        }


                    }
                }
                else { break; }


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

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;

            }






        }

    }
}