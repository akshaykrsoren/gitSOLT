using CaregiverLiteWCF;
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
    public class RewardPointModel
    {



        public string ProfileImage { get; set; }
        public string CompletedRequestCount;
        public string TotalRewardPoint;
        public string Comment;
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public List<RewardPoint> RewardPointList { get; set; }
        public RewardPoint RewardPointDetail { get; set; }

        public string Office { get; set; }


    }
    public class RewardPointServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public RewardPointsList RewardPointsList { get; set; }
        public RewardPoint RewardPointDetail { get; set; }
        public List<RewardPoint> RewardPointList { get; set; }
        

        public RewardPointServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<RewardPointsList> GetAllRewardPointDetails(int pageno, int recordperpage, string search, string sortfield, string sortOrder ,int FilterOffice,string LogInUserId)
        {
            RewardPointsList RewardPointsList = new RewardPointsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllRewardPointDetail/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + FilterOffice+"/"+ LogInUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    RewardPointsList = JsonConvert.DeserializeObject<RewardPointServiceProxy>(json).RewardPointsList;
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
            return RewardPointsList;
        }

        public async Task<RewardPointsList> GetAllRewardPointAdvanceDetails(int pageno, int recordperpage, string search, string sortfield, string sortOrder , int FilterOffice,string LogInUserId)
        {
            RewardPointsList RewardPointsList = new RewardPointsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllRewardPointAdvanceDetail/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + FilterOffice + "/"+ LogInUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    RewardPointsList = JsonConvert.DeserializeObject<RewardPointServiceProxy>(json).RewardPointsList;
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
            return RewardPointsList;
        }

        public async Task<string> EditComment(string NurseId, string Comment)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditComment/" + NurseId + "/" + Comment, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<SchedulerServiceProxy>(json).Result;
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

        //View Model Code
        public async Task<RewardPointsList> GetAllRewardPointByNurseId(int NurseId)
        {
            RewardPointsList RewardPointsList = new RewardPointsList();
            List<RewardPoint> RewardPointsDetail = new List<RewardPoint>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllRewardPointByNurseId/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    RewardPointsList = JsonConvert.DeserializeObject<RewardPointServiceProxy>(json).RewardPointsList;
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
            return RewardPointsList;
        }

    }

}