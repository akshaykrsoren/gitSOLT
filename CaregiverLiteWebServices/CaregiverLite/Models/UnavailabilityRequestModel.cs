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
    public class UnavailabilityRequestModel
    {
        public int NurseId { get; set; }

        public string UserId { get; set; }
    }
    public class UnavailabilityRequestProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public string UnavailabilityRequest { get; set; }


        public UnavailabilityRequestProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> InsertUUnavailabilityRequest(UnavailabilityRequest UnavailabilityRequest)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertUnavailabilityRequest", new { UnavailabilityRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<UnavailabilityRequestProxy>(json).Result;
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

        public async Task<string> GetUnavailabilityRequestByTimeSlot(string TimeSlotId, string SlotDate)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetUnavailabilityRequestByTimeSlot/" + TimeSlotId + "/" + SlotDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<UnavailabilityRequestProxy>(json).Result;
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