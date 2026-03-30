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
    public class CoronaStatsDetailsModel
    {
        public int BodySymptomsId { get; set; }
        public int NurseId { get; set; }
        public string NurseName { get; set; }
        public string Datetimes { get; set; }
        public string BodyPain { get; set; }
        public string BodyTemperature { get; set; }
        public string Cough { get; set; }
        public string BreathingDifficulty { get; set; }
        public string SoreThroat { get; set; }
        public string OfficeName { get; set; }
        public string ActiveStatus { get; set;}
        public int OfficeId { get; set; }

    }


    public class CoronaStatsDetailsServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public bool ResultInBool { get; set; }
        public List<CornaStatsDetails> CornaStatsDetailsList { get; set; }
        public CornaStatsDetailsList ListCornaStatsDetails { get; set; }
        public CornaStatsDetails CornaStatsDetails { get; set; }


        public CoronaStatsDetailsServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }



        public async Task<CornaStatsDetailsList> GetAllCornaStatsDetails(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, int OfficeId)
        {

            List<CornaStatsDetails> objPatientDetailsList = new List<CornaStatsDetails>();
            CornaStatsDetailsList CornaStatsDetails = new CornaStatsDetailsList();
            try
            {
                var json = "";
                // Send request to server
                //    HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCornaStatsDetailsList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + OfficeId + "/" + search, this.cancellationToken).Result;
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCornaStatsDetailsList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + OfficeId + "/" + search, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CornaStatsDetails = JsonConvert.DeserializeObject<CoronaStatsDetailsServiceProxy>(json).ListCornaStatsDetails;
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
            return CornaStatsDetails;
        }





    }



    }