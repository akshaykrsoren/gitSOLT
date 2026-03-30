using CaregiverLiteWCF;
using CaregiverLiteWCF.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Models
{
    public class RatingModel
    {

        public string m_SchedulerName;
        public string m_AppointmentDate;
        public string m_Time;
        public string m_Point;
        public string m_Rating;
    }

    public class RatingServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public RatingsList RatingsList { get; set; }
        public List<Rating> RatingList { get; set; }


        public RatingServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        

        //View Model Code
        public async Task<RatingsList> GetAllRatingByNurseId(int NurseId)
        {
            RatingsList RatingsList = new RatingsList();
            List<Rating> RatingsDetail = new List<Rating>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllRatingByNurseId/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    RatingsList = JsonConvert.DeserializeObject<RatingServiceProxy>(json).RatingsList;
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
            return RatingsList;
        }

    }
}