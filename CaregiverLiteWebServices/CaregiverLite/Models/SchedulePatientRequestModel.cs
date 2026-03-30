using CaregiverLiteWCF.Class;
//using CaregiverLite.CaregiverLiteService;
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
    public class SchedulePatientRequestModel
    {
        public int PatientRequestId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("PatientName")]
        public string PatientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgAddress")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.PatientRequest), ErrorMessageResourceName = "RequiredMsgStreet")]
        [DisplayName("Street")]
        public string Street { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.PatientRequest), ErrorMessageResourceName = "RequiredMsgCity")]
        [DisplayName("City")]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.PatientRequest), ErrorMessageResourceName = "RequiredMsgState")]
        [DisplayName("State")]
        public string State { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgZipCode")]
        [StringLength(100, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgZipCode", MinimumLength = 4)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }
        public string MedicalId { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Status { get; set; }
        public bool IsCancelled { get; set; }

        public string ServiceNames { get; set; }
        public string TimezoneId { get; set; }
        public int TimezoneOffset { get; set; }
        public string TimezonePostfix { get; set; }

        public int OrganisationId { get; set; }

          public string VisitTypeNames { get; set; }
        public string InsertUserId { get; set; }
        public bool IsRepeat { get; set; }
        public string RepeatEvery { get; set; }
        public string RepeatTypeID { get; set; }
        public string RepeatDate { get; set; }

        public string DayOfWeek { get; set; }
        public string DaysOfMonth { get; set; }
        public string Caregiver { get; set; }
        public float MaxDistance { get; set; }
        public int MaxCaregiver { get; set; }

        public int Office { get; set; }
        public string  OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public DateTime date1 { get; set; }
        public string PayerId { get; set; }
        public string PayerProgram { get; set; }
        public string ClientPayerId { get; set; }
        public string ProcedureCode { get; set; }
        public string JurisdictionCode { get; set; }

        public string ProcedureCodes { get; set; }
        public Boolean IsRequiredDriving { get; set; } = true;
    }

    public class SchedulePatientRequestServiceProxy : CaregiverLiteBaseService
    {
        public List<CaregiverLiteWCF.CareGivers> CareGiversList { get; set; }
        
        public List<CaregiverLiteWCF.CareGiverTrackLocation> CareGiverTrackLocationList { get; set; }
        public List<CaregiverLiteWCF.GetDatesList> GetDatesList { get; set; }
        public SchedulePatientRequestList SchedulePatientRequestList { get; set; }
        public SchedulePatientRequest SchedulePatientRequestDetail { get; set; }
        public List<SchedulePatientRequest> SchedulePatientRequestsList { get; set; }
        public Boolean Flag { get; set; }

        public CaregiverLiteWCF.CareGiversList CareGiverList { get; set; }

        public SchedulePatientRequestList SchedulePatientCompleteRequestsList { get; set; }
        public string Result { get; set; }

        public SchedulePatientRequestServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }
        public async Task<CaregiverLiteWCF.CareGiversList> GetAllCareGivers(int pageno, int recordperpage, string search, string sortfield, string sortOrder, string PatientRequestId)
        {

            CaregiverLiteWCF.CareGiversList CareGiversList = new CaregiverLiteWCF.CareGiversList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllCareGivers/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + PatientRequestId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiverList;
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
            return CareGiversList;
        }

        public async Task<List<CaregiverLiteWCF.CareGivers>> GetAllNotifiedCareGiversByRequestId(string PatientRequestId)
        {

            List<CaregiverLiteWCF.CareGivers> CareGiversList = new List<CaregiverLiteWCF.CareGivers>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllNotifiedCareGiversByRequestId/" + PatientRequestId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiversList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiversList;
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

        public async Task<List<CaregiverLiteWCF.CareGiverTrackLocation>> GetTrackLocationByPatientRequestId(string PatientRequestId)
        {

            List<CaregiverLiteWCF.CareGiverTrackLocation> CareGiverTrackLocationList = new List<CaregiverLiteWCF.CareGiverTrackLocation>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetTrackLocationByPatientRequestId/" + PatientRequestId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiverTrackLocationList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiverTrackLocationList;
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
            return CareGiverTrackLocationList;
        }

        public async Task<List<CaregiverLiteWCF.CareGiverTrackLocation>> GetTrackLocationByNurseId(string NurseId, string date)
        {

            List<CaregiverLiteWCF.CareGiverTrackLocation> CareGiverTrackLocationList = new List<CaregiverLiteWCF.CareGiverTrackLocation>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetTrackLocationByNurseId/" + NurseId + "/" + date, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    CareGiverTrackLocationList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiverTrackLocationList;
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
            return CareGiverTrackLocationList;
        }

        public async Task<List<CaregiverLiteWCF.GetDatesList>> GetFilterDates(SchedulePatientRequestModel SchedulePatientRequest)
        {

            List<CaregiverLiteWCF.GetDatesList> GetFilterDatesList = new List<CaregiverLiteWCF.GetDatesList>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetFilterDates", new { SchedulePatientRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    GetFilterDatesList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).GetDatesList;
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
            return GetFilterDatesList;
        }

        public async Task<SchedulePatientRequestList> GetAllSchedulePatientRequest(int pageno, int recordperpage, string search, string sortfield, string sortOrder, int IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterStatus, int FilterOffice, int OrganisationId)
        {

            SchedulePatientRequestList SchedulePatientRequestList = new SchedulePatientRequestList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllSchedulePatientRequest/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + IsAdmin + "/" + LogInUserId + "/" + FromDate + "/" + ToDate + "/" + FilterStatus + "/" + FilterOffice + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SchedulePatientRequestList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).SchedulePatientRequestList;
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
            return SchedulePatientRequestList;
        }

        public async Task<CaregiverLiteWCF.CareGiversList> InsertSchedulePatientRequest(SchedulePatientRequestModel SchedulePatientRequest)
        {
            CaregiverLiteWCF.CareGiversList objCareGiverList = new CaregiverLiteWCF.CareGiversList();
            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertSchedulePatientRequest", new { SchedulePatientRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objCareGiverList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiverList;
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
            return objCareGiverList;
        }

        public async Task<string> InsertSchedulePatientRequestTemp(string PatientRequestId, string NurseId, string IsNotificationSent)
        {
            CaregiverLiteWCF.CareGiversList objCareGiverList = new CaregiverLiteWCF.CareGiversList();
            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertSchedulePatientRequestTemp/" + PatientRequestId + "/" + NurseId + "/" + IsNotificationSent, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).Result;
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

        public async Task<CaregiverLiteWCF.CareGiversList> CancelPatientRequest(string PatientRequestId, string Userid)
        {

            string Result = "";
            CaregiverLiteWCF.CareGiversList objCareGiverList = new CaregiverLiteWCF.CareGiversList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "CancelPatientRequest/" + PatientRequestId + "/" + Userid, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objCareGiverList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiverList;
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
            return objCareGiverList;
        }

        public async Task<SchedulePatientRequest> GetPatientRequestDetailByMedicalIDByOrganisation(string LoginUserId, string MedicalID, string OrganisationId)
        {

            string Result = "";
            SchedulePatientRequest PatientRequestDetails = new SchedulePatientRequest();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientRequestDetailByMedicalIDByOrganisation/" + LoginUserId + "/" + MedicalID + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestDetails = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).SchedulePatientRequestDetail;
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
            return PatientRequestDetails;
        }


        public async Task<SchedulePatientRequest> GetPatientRequestDetailByMedicalID(string LoginUserId, string MedicalID)
        {

            string Result = "";
            SchedulePatientRequest PatientRequestDetails = new SchedulePatientRequest();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientRequestDetailByMedicalID/" + LoginUserId + "/" + MedicalID , this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestDetails = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).SchedulePatientRequestDetail;
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
            return PatientRequestDetails;
        }

        public async Task<string> GetRateByPatientRequestId(string PatientRequestId)
        {
            string rate1 = "";
            List<CaregiverLiteWCF.CareGiverTrackLocation> CareGiverTrackLocationList = new List<CaregiverLiteWCF.CareGiverTrackLocation>();
            try
            {
                var json = "";
                
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetRateByPatientRequestId/" + PatientRequestId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    rate1 = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).Result;
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
            return rate1;
        }

        public async Task<string> SetRateByPatientRequestId(string PatientRequestId,string Rating)
        {
            string rate1 = "";
            List<CaregiverLiteWCF.CareGiverTrackLocation> CareGiverTrackLocationList = new List<CaregiverLiteWCF.CareGiverTrackLocation>();
            try
            {
                var json = "";

                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SetRateByPatientRequestId/" + PatientRequestId +"/"+ Rating, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    rate1 = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).Result;
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
            return rate1;
        }

        public async Task<CareGiversList> RemovePatientRequest(string PatientRequestId, string Userid)
        {

            string Result = "";
            CaregiverLiteWCF.CareGiversList objCareGiverList = new CaregiverLiteWCF.CareGiversList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "RemovePatientRequest/" + PatientRequestId + "/" + Userid, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objCareGiverList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiverList;
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
            return objCareGiverList;
        }

        //QuickBLox
        public async Task<bool> checkDialogId(string GroupName)
        {

            string Result = "";
            Boolean Flag  = false;
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "checkDialogId/" + GroupName, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Flag = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).Flag;
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
            return Flag;
        }
        //

        //16-08-2017 Hardik Masalawala
        public async Task<CareGiversList> GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers(SchedulePatientRequestModel SchedulePatientRequest)
        {
            CareGiversList objCareGiverList = new CareGiversList();
            string Result = "";

            string jsonStringdata = JsonConvert.SerializeObject(SchedulePatientRequest, Newtonsoft.Json.Formatting.Indented);
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers", new { SchedulePatientRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objCareGiverList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).CareGiverList;
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
            return objCareGiverList;
        }
        //16-08-2017

        public async Task<List<Office>> GetAllOffices(string logInUserId)
        {

            List<Office> OfficesList = new List<Office>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAvailableOfficesList/" + logInUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OfficesList = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).lstOffice;
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
            return OfficesList;
        }

        #region CheckInOutBySuperAdmin
        public async Task<string> CheckInOutBySuperAdmin(SchedulePatientRequest SchedulePatientRequest)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "CheckInOutBySuperAdmin/" + SchedulePatientRequest.PatientRequestId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).Result;
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
        #endregion


        public async Task<SchedulePatientRequestList> GetAllPasevaCustomerSchedulePatientRequest(int pageno, int recordperpage, string search, string sortfield, string sortOrder, int IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterStatus, int FilterOffice)
        {

            SchedulePatientRequestList SchedulePatientRequestList = new SchedulePatientRequestList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllPasevaCustomerSchedulePatientRequest/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + IsAdmin + "/" + LogInUserId + "/" + FromDate + "/" + ToDate + "/" + FilterStatus + "/" + FilterOffice, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SchedulePatientRequestList = JsonConvert.DeserializeObject<SchedulePatientRequestServiceProxy>(json).SchedulePatientRequestList;
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
            return SchedulePatientRequestList;
        }
  
    }



    public class RootRegister
    {
        
        public int success { get; set; }
  
        public List<string> data { get; set; }

        public string message { get; set; }
    }


    public class Root
    {

        public SchedulePatientRequest PatientRequest { get; set; }
    }
}