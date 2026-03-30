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
    public class CareGiverModel
    {
        public int NurseId { get; set; }

        public int OfficeId { get; set; }

        public string UserId { get; set; }

        public string InsertUserId { get; set; }

        public string QuickBloxId { get; set; }

        [DisplayName("NurseFullName")]
        public string NurseFullName { get; set; }

        [DisplayName("PayrollId")]
        public string PayrollId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [DisplayName("Services")]
        public string Services { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgServices")]
        public string ServiceNames { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmail")]
        //[Remote("IsEmailExists", "CareGiver", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmailExists")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsername")]
        // [Remote("IsUserNameExists","CareGiver",AdditionalFields="UserId", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsernameExists")]
        [DisplayName("Username")]
        public string Username { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredOldPassWord")]
        //[Remote("IsOldPasswordValid", "CareGiver", AdditionalFields = "UserId", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredOldPassWordNotMatch")]
        //[DisplayName("Old Password")]
        //public string OldPassword { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        //[Remote("IsValidNewPassword", "CareGiver", AdditionalFields = "OldPassword", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "NewPasswordMatch")]
        //[StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        //[DisplayName("NewPassword")]
        //public string NewPassword { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgReTypePassword")]
        //[DisplayName("Retype Password")]
        //[System.Web.Mvc.Compare("NewPassword", ErrorMessage = "retype password does not match")]
        //[StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        //public string RetypeNewPassword { get; set; }

       // [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgAddress")]
     //   [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgStreet")]
        [DisplayName("Street")]
        public string Street { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgCity")]
        [DisplayName("City")]
        public string City { get; set; }


        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgState")]
        [DisplayName("State")]
        public string State { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgZipCode")]
        [StringLength(100, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgZipCode", MinimumLength = 4)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgHoursRate")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "Please enter valid hour rate")]
        [DisplayName("Hours Rate")]
        [Range(1, 9999.9, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidMsgHoursRate")]
        public string HoursRate { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPhone")]
        [DisplayName("Phone")]
        [StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPhone", MinimumLength = 4)]
        public string Phone { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        //[StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        //[DisplayName("Password")]
        public string Password { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgReTypePassword")]
        //[DisplayName("Retype Password")]
        //[StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        //[System.Web.Mvc.Compare("Password", ErrorMessage = "retype password does not match")]
        //public string RetypePassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgServiceRadius")]
        [DisplayName("ServiceRadius")]
        public string ServiceRadius { get; set; }

        public string DistanceUnit { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEducation")]
        [DisplayName("Education")]
        public string Education { get; set; }

       // [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgProfileImage")]
        [DisplayName("ProfileImage")]
        public string ProfileImage { get; set; }

        public string ProfileVideoThumbnil { get; set; }

        public string ProfileVideo { get; set; }
        public List<string> Certificate { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CareGiverSuperAdmin.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgProfileImage")]
        public HttpPostedFileBase ProfileImageFile { get; set; }

        public HttpPostedFileBase ProfileVideoFile { get; set; }

        public string IsApproved { get; set; }

        public string IsActive { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string OldEmail { get; set; }

        public string IsCertificateApproved { get; set; }

        public string OldHoursRate { get; set; }

        public bool CanAdminEdit { get; set; }

        public HoursRate HoursRateList { get; set; }
        public decimal ChargeToPatient { get; set; }
        public string Timezone { get; set; }
        public string TimezoneId { get; set; }
        public int TimezoneOffset { get; set; }
        public string TimezonePostfix { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string Office { get; set; }

        public int OrganisationId { get; set; }

        // for check when Caregiver data edited that time office changed or not
        public int OldOfficeId { get; set; }

        public int OldOfficeChatGroupId { get; set; }

        public bool IsAllowOneToOneChat { get; set; }

        public bool IsAllowPatientChatRoom { get; set; }

        public bool IsAllowGroupChat { get; set; }

        public bool IsAllowToCreateGroupChat { get; set; }

        public bool IsApprove { get; set; }

        public IEnumerable<SelectListItem> OfficeSelectList { get; set; }
        //public bool IsActive { get;   set; }
    }

    public class CareGiverServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public CareGiversList CareGiverList { get; set; }
        public CareGivers CareGiverDetail { get; set; }
        public List<CareGivers> CareGiversList { get; set; }
        public List<CareGivers> CareGiverListCertificate { get; set; }
        public CareGiverModel CareGiverModelDetail { get; set; }

        public CareGiverServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        //public async Task<CareGiversList> GetAllCareGivers(string LoginUserId,int pageno, int recordperpage,int IsApprove, string search, string sortfield, string sortOrder, int FilterOffice, int OrganisationId,string IsActiveStatus)
        public async Task<CareGiversList> GetAllCareGivers(string LoginUserId, int pageno, int recordperpage, int IsApprove, string search, string sortfield, string sortOrder, int FilterOffice, int OrganisationId)
        {

            CareGiversList CareGiversList = new CareGiversList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllCareGivers/" + LoginUserId + "/" + pageno + "/" + recordperpage + "/" + IsApprove + "/" + sortfield + "/" + sortOrder + "/" + search +"/" + FilterOffice + "/" + OrganisationId.ToString(), this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversList = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverList;
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
            return CareGiversList;
        }


        public async Task<CareGiversList> GetAllCareGiversList(string SchedulerUserId)
        {

            CareGiversList CareGiversList = new CareGiversList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllCareGiversList/" + SchedulerUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversList = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverList;
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
            return CareGiversList;
        }

        public async Task<string> InsertUpdateCareGiverByAdmin(CareGivers CareGiver)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertUpdateCareGiverByAdmin", new { CareGiver }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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

        public async Task<string> UpdateCareGiver(CareGivers CareGiver)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "UpdateCareGiver", new { CareGiver }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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


        public async Task<string> InsertNurseProfile(CareGivers CareGiver)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertNurseProfile", new { CareGiver }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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

        public async Task<CareGivers> GetAllCareGiverByNurseId(string NurseId)
        {

            CareGivers CareGiversDetail = new CareGivers();
            try
            {
                var json = "";
                // Send request to server

                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllCareGiverByNurseId/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversDetail = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverDetail;
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
            return CareGiversDetail;
        }

        public async Task<CareGivers> GetAllCareGiverByUserId(string UserId)
        {

            CareGivers CareGiversDetail = new CareGivers();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCareGiverByUserId/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversDetail = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverDetail;
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
            return CareGiversDetail;
        }
        public async Task<string> ApproveRejectNurse(CareGivers CareGiver)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "ApproveRejectNurse", new { CareGiver }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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

        public async Task<List<CareGivers>> GetAllNursesByDateFilter(string FromDate, string ToDate)
        {

            List<CareGivers> CareGiversList = new List<CareGivers>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllNursesByDateFilter/" + FromDate + "/" + ToDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversList = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiversList;
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
            return CareGiversList;
        }

        public async Task<string> DeleteNurse(string NurseId, string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteNurse/" + NurseId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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

        public async Task<CareGivers> GetCareGiverDetailsByUserId(string UserId)
        {
            CareGivers CareGiversDetail = new CareGivers();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCareGiverByUserId/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversDetail = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverDetail;
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
            return CareGiversDetail;
        }

        public async Task<string> DeleteNurseCertificateById(string UserId, string NurseCertificateId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteNurseCertificateById/" + UserId + "/" + NurseCertificateId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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

        public async Task<string> UpdateAvailableOrBusyStatus(string NurseId, string UserId, string StatusId, string StatusType)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "UpdateAvailableOrBusyStatus/" + NurseId + "/" + UserId + "/" + StatusId + "/" + StatusType, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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

        public async Task<List<CareGivers>> GetCareGiverByServiceId(string ServiceId)
        {

            List<CareGivers> CareGiversList = new List<CareGivers>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCareGiverByServiceId/" + ServiceId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversList = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiversList;
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
            return CareGiversList;
        }

        public async Task<List<CareGivers>> GetCareGiverForPatiantRequest(PatientRequests PatientRequest)
        {

            List<CareGivers> CareGiversList = new List<CareGivers>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCareGiverForPatiantRequest", new { PatientRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversList = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiversList;
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
            return CareGiversList;
        }

        public async Task<string> ResetPassword(CareGivers CareGiver)
        {

            string result = "";
            try
            {
                var json = "";

                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "ResetPassword", new { CareGiver }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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

        public async Task<string> InsertNurseCertificate(string NurseId, string Certificate, string UserId)
        {

            string result = "";
            try
            {
                var json = "";

                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertNurseCertificate/" + NurseId + "/" + Certificate + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).Result;
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


        public async Task<List<CareGivers>> GetCareGiverCertiByNurseId(string NurseId)
        {
            List<CareGivers> CareGiversListobj = new List<CareGivers>();

            string result = "";
            try
            {
                var json = "";

                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCareGiverCertiByNurseId/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversListobj = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverListCertificate;
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
            return CareGiversListobj;
        }
    }

    public class UnavailabilityRequestServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public UnavailabilityRequestsList UnavailabilityRequestsList { get; set; }
        public List<PatientRequests> PatientRequestsList { get; set; }

        public UnavailabilityRequestServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<UnavailabilityRequestsList> GetAllUnavailabilityRequest(int pageno, int recordperpage, string search, string sortfield, string sortOrder)
        {

            UnavailabilityRequestsList UnavailabilityRequest = new UnavailabilityRequestsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllUnavailabilityRequest/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    UnavailabilityRequest = JsonConvert.DeserializeObject<UnavailabilityRequestServiceProxy>(json).UnavailabilityRequestsList;
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
            return UnavailabilityRequest;
        }

        public async Task<string> ApproveRejectUnavailabilityRequest(UnavailabilityRequests UnavailabilityRequest)
        {

            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "ApproveRejectUnavailabilityRequest", new { UnavailabilityRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<UnavailabilityRequestServiceProxy>(json).Result;
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
            return Result;
        }

        public async Task<List<PatientRequests>> GetAppointmentsByUnavailabilityRequestId(string UnavailabilityRequestId)
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAppointmentsByUnavailabilityRequestId/" + UnavailabilityRequestId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<UnavailabilityRequestServiceProxy>(json).PatientRequestsList;
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
            return PatientRequestList;
        }
       
    }


    public class Userprofile
    {
        public CareGiverModel ObjCaregiverModel { get; set; }
        public AdminModel ObjAdminModelModel { get; set; }
        public SchedulerModel ObjSchedulerModel { get; set; }
        public NurseCoordinatorModel ObjNurseCoordinatorModel { get; set; }
        
        


    }
}