using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CaregiverLite.Models
{
    public class MessageQueueServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }

        public MessageQueueServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }
        public async Task<string> InsertMessageQueue(MessageQueue MessageQueue)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertMessageQueue", new { MessageQueue }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<MessageQueueServiceProxy>(json).Result;

                    //if (int.Parse(json) > 0)
                    //    result = "success";
                    //else
                    //    return "failed";
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }
    }
}