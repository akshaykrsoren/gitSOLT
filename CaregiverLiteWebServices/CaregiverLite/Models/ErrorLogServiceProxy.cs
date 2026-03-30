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
    public class ErrorLogServiceProxy : CaregiverLiteBaseService
    {
        public ErrorLogServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> InsertErrorLog(ErrorLog ErrorLog)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertErrorLog", new { ErrorLog }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ErrorLogServiceProxy>(json).Result;

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

        public string Result { get; set; }
    }

    public class LogsServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }

        public LogsServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> InsertLog(Logs objLogs)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertLog", new { objLogs }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<LogsServiceProxy>(json).Result;

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