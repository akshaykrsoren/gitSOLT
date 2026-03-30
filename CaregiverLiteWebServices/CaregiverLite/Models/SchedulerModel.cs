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
    public class SchedulerModel
    {

        public int SchedulerId { get; set; }

        public string UserId { get; set; }

        public string InsertUserId { get; set; }

        public string IsActive { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        //[DisplayName("Name")]
        //public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmail")]
        //[Remote("IsEmailExists", "CareGiver", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmailExists")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsername")]
        // [Remote("IsUserNameExists","CareGiver",AdditionalFields="UserId", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsernameExists")]
        [DisplayName("Username")]
        public string Username { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        //[StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        //[DisplayName("Password")]
        public string Password { get; set; }

        public string OfficeIds { get; set; }

        public string OldEmail { get; set; }

        public int OrganisationId { get; set; }

        public string QuickBloxId { get; set; }

    }

    public class SchedulerServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public SchedulersList SchedulersList { get; set; }
        public Scheduler SchedulerDetail { get; set; }
        public List<Scheduler> SchedulerList { get; set; }


        public SchedulerServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        //public async Task<CareGiversList> GetAllCareGivers(int pageno, int recordperpage, string search, string sortfield, string sortOrder)
        //{

        //    CareGiversList CareGiversList = new CareGiversList();
        //    try
        //    {
        //        var json = "";
        //        // Send request to server
        //        HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllCareGivers/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search, this.cancellationToken).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Parse the response body. Blocking!
        //            json = await response.Content.ReadAsStringAsync();
        //            CareGiversList = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverList;
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog.LogError(ex);
        //    }
        //    return CareGiversList;
        //}

        public async Task<string> AddScheduler(Scheduler Scheduler)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddScheduler", new { Scheduler }).Result;
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


        public async Task<string> EditScheduler(Scheduler Scheduler)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditScheduler", new { Scheduler }).Result;
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

        public async Task<SchedulersList> GetAllSchedulers(string LoginUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder,string OfficeId, int OrganisationId,string IsActiveStatus)
        {

            SchedulersList SchedulersList = new SchedulersList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllScheduler/" + LoginUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + OfficeId + "/" + OrganisationId.ToString()+"/"+ IsActiveStatus, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SchedulersList = JsonConvert.DeserializeObject<SchedulerServiceProxy>(json).SchedulersList;
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
            return SchedulersList;
        }

        public async Task<SchedulersList> GetAllSchedulerList()
        {

            SchedulersList SchedulersList = new SchedulersList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllSchedulerList", this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SchedulersList = JsonConvert.DeserializeObject<SchedulerServiceProxy>(json).SchedulersList;
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
            return SchedulersList;
        }



        public async Task<string> DeleteScheduler(string SchedulerId, string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteScheduler/" + SchedulerId + "/" + UserId, this.cancellationToken).Result;
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


        public async Task<Scheduler> GetSchedulerDetailById(string SchedulerId)
        {

            Scheduler SchedulerDetails = new Scheduler();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetSchedulerDetailById/" + SchedulerId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SchedulerDetails = JsonConvert.DeserializeObject<SchedulerServiceProxy>(json).SchedulerDetail;
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
            return SchedulerDetails;
        }
        public async Task<Scheduler> GetSchedulerDetailByUserId(string SchedulerUserId)
        {

            Scheduler SchedulerDetails = new Scheduler();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetSchedulerDetailByUserId/" + SchedulerUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SchedulerDetails = JsonConvert.DeserializeObject<SchedulerServiceProxy>(json).SchedulerDetail;
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
            return SchedulerDetails;
        }
    }
}