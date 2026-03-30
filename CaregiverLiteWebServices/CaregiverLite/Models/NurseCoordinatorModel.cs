using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Models
{



    public class NurseCoordinatorModel
    {

        public int NurseCoordinatorId { get; set; }
        public string UserId { get; set; }
        public string InsertUserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmail")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsername")]
        [DisplayName("Username")]
        public string Username { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        //[StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        //[DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Select Office")]
        [DisplayName("Office")]
        public int OfficeId { get; set; }

        //[Required(ErrorMessage = "Please Select User Is Allow For Patient Chat Room")]
        //[DisplayName("IsAllowForPatientChatRoom")]
        public bool IsAllowForPatientChatRoom  { get; set; }

        public int OldOfficeId { get; set; }

        public string QuickBloxId { get; set; }

        public IEnumerable<SelectListItem> OfficeSelectList { get; set; }

        [DisplayName("JobTitle")]
        public string JobTitle { get; set; }

        public bool IsOfficePermission { get; set; }

        public string IsActive { get; set; }

        public bool IsAllowOneToOneChat { get; set; }

        public bool IsAllowGroupChat { get; set; }

        public bool IsAllowToCreateGroupChat { get; set; }

        public string OldEmail { get; set; }

    
        public int OrganisationId { get; set; }
    }


   
    public class NurseCoordinatorServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public NurseCoordinatorsList NurseCoordinatorsList { get; set; }
        public NurseCoordinator NurseCoordinatorDetail { get; set; }
        public NurseCoordinatorModel NurseCoordinatorModel { get; set; }
        public List<NurseCoordinator> NurseCoordinatorList { get; set; }


        public NurseCoordinatorServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

       

        public async Task<string> AddNurseCoordinator(NurseCoordinator NurseCoordinator)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddNurseCoordinator", new { NurseCoordinator }).Result;
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


       

        public async Task<NurseCoordinatorsList> GetAllNurseCoordinators(string loginUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, int officeId, int OrganisationId, string IsActiveStatus)
        {

            NurseCoordinatorsList NurseCoordinatorsList = new NurseCoordinatorsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllNurseCoordinators/" + loginUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + officeId + "/" + search + "/" + OrganisationId + "/" + IsActiveStatus, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    NurseCoordinatorsList = JsonConvert.DeserializeObject<NurseCoordinatorServiceProxy>(json).NurseCoordinatorsList;
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
            return NurseCoordinatorsList;
        }


        public async Task<string> DeleteNurseCoordinator(string NurseCoordinatorId, string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteNurseCoordinator/" + NurseCoordinatorId + "/" + UserId, this.cancellationToken).Result;
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

        public async Task<string> EditNurseCoordinator(NurseCoordinator NurseCoordinator)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditNurseCoordinator", new { NurseCoordinator }).Result;
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

        public async Task<NurseCoordinator> GetNurseCoordinatorDetailById(string NurseCoordinatorId)
        {

            NurseCoordinator NurseCoordinatorDetails = new NurseCoordinator();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetNurseCoordinatorDetailById/" + NurseCoordinatorId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    NurseCoordinatorDetails = JsonConvert.DeserializeObject<NurseCoordinatorServiceProxy>(json).NurseCoordinatorDetail;
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
            return NurseCoordinatorDetails;
        }

        public async Task<NurseCoordinator> GetNurseCoordinatorDetailByUserId(string NurseCoordinatorId)
        {

            NurseCoordinator NurseCOordinatorDetails = new NurseCoordinator();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetNurseCoordinatorDetailByUserId/" + NurseCoordinatorId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    NurseCOordinatorDetails = JsonConvert.DeserializeObject<NurseCoordinatorServiceProxy>(json).NurseCoordinatorDetail;
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
            return NurseCOordinatorDetails;
        }


    }

}