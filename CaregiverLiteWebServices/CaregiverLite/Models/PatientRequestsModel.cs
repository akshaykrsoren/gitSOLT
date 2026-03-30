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

namespace CaregiverLite.Models
{
    public class PatientRequestsModel
    {
        public string AppointmentId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgPatient")]
        public string PatientId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgServices")]
        public string ServiceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgNurse")]
        public string NurseId { get; set; }

        public string PatientName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PatientEmail { get; set; }

        public string PatientPhone { get; set; }

        public string PatientAddress { get; set; }

        public string ServiceName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgHourRate")]
        public string HourRate { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgDate")]
        public string Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgFromTime")]
        public string FromTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgToTime")]
        public string ToTime { get; set; }

        public string TotalHours { get; set; }

        public string TotalAmount { get; set; }

        public string NurseName { get; set; }

        public string NurseEmail { get; set; }

        public string NursePhone { get; set; }

        public string RescheduleStatus { get; set; }

        public string InsertUser { get; set; }

        public string NurseDeviceType { get; set; }

        public string NurseDeviceToken { get; set; }

        public string PatientDeviceToken { get; set; }

        public string PatientDeviceType { get; set; }

        public string NurseUserId { get; set; }

        public string PatientUserId { get; set; }

        public string CareGiverCharge { get; set; }

        public string StripeFee { get; set; }

        public string PaidAmount { get; set; }

        public string ChargeToPatient { get; set; }


        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public int OfficeId { get; set; }
    }

    public class PatientRequestServiceProxy : CaregiverLiteBaseService
    {
        public PatientRequestList PatientRequestList { get; set; }
        public PatientRequests PatientRequestDetail { get; set; }
        public List<PatientRequests> PatientRequestsList { get; set; }

        public PatientRequestList PatientCompleteRequestsList { get; set; }
        public string Result { get; set; }

        public PatientRequestServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

       

        public async Task<PatientRequestList> GetAllPatientRequestsByNurseId(int pageno, int recordperpage, string search, string sortfield, string sortOrder,string NurseId)
        {

            PatientRequestList PatientRequestList = new PatientRequestList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllPatientRequestsByNurseId/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestList;

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
            return PatientRequestList;
        }

        public async Task<PatientRequests> GetAppointmentDetailByAppointmentId(string AppointmentId)
        {

            PatientRequests PatientRequest = new PatientRequests();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAppointmentDetailByAppointmentId/" + AppointmentId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequest = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestDetail;
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
            return PatientRequest;
        }

        public async Task<string> InsertPatientRequestForAdmin(PatientRequests PatientRequest)
        {

            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertPatientRequestForAdmin", new { PatientRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).Result;
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

        public async Task<string> CancelPatientAppointment(string AppointmentId, string UserId)
        {

            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "CancelPatientAppointment/" + AppointmentId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).Result;
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

        public async Task<string> ChangeCareGiver(string AppointmentId, string NurseId, string UserId)
        {
            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "ChangeCareGiver/" + AppointmentId + "/" + NurseId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).Result;
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

        public async Task<PatientRequests> GetRescheduleAppoinmentRequest(string AppointmentId)
        {

            PatientRequests PatientRequest = new PatientRequests();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetRescheduleAppoinmentRequest/" + AppointmentId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequest = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestDetail;
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
            return PatientRequest;
        }

        public async Task<string> RescheduleAppoinment(PatientRequests PatientRequest)
        {

            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "RescheduleAppoinment", new { PatientRequest }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).Result;
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

        public async Task<List<PatientRequests>> GetAppointmentsByDateFilter(string FromDate, string ToDate)
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAppointmentsByDateFilter/" + FromDate + "/" + ToDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
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

        public async Task<List<PatientRequests>> GetPaymentReceived(string NurseId, string PatientId, string ZipCode, string FromDate, string ToDate)
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPaymentReceived/" + NurseId + "/" + PatientId + "/" + ZipCode + "/" + FromDate + "/" + ToDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
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

        public async Task<List<PatientRequests>> GetPaymentReceivedByNurseId(string NurseId, string PatientName, string FromDate, string ToDate)
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPaymentReceivedByNurseId/" + NurseId + "/" + PatientName + "/" + FromDate + "/" + ToDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
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

        public async Task<List<PatientRequests>> GetProjectedPayment(string NurseId, string PatientId, string ZipCode, string FromDate, string ToDate)
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetProjectedPayment/" + NurseId + "/" + PatientId + "/" + ZipCode + "/" + FromDate + "/" + ToDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
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

        public async Task<List<PatientRequests>> GetRescheduleApprovalAlerts()
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetRescheduleApprovalAlerts", this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
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

        
        //cc
        public async Task<PatientRequestList> GetAppointmentsByNurseId(string NurseId, string IsComplete, int pageno, int recordperpage, string search, string sortfield, string sortOrder)
        {

            PatientRequestList PatientRequestsList = new PatientRequestList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAppointmentsByNurseId/" + NurseId + "/" + IsComplete + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestsList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientCompleteRequestsList;
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
            return PatientRequestsList;
        }
        //cc
        public async Task<List<PatientRequests>> GetRescheduleAlertsByNurseId(string NurseId)
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetRescheduleAlertsByNurseId/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
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

        //cc
        public async Task<List<PatientRequests>> GetCancelledAppointmentByPatientForNurseId(string NurseId)
        {

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetCancelledAppointmentByPatientForNurseId/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
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

        //cc
        public async Task<List<PatientRequests>> GetPaymentRequertForMapByNurseId(string NurseId, string NurseName, string FromDate, string ToDate)
        {
            if (NurseName == "") NurseName = "0";

            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPaymentRequertForMapByNurseId/" + NurseId + "/" + NurseName + "/" + FromDate + "/" + ToDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientRequestList = JsonConvert.DeserializeObject<PatientRequestServiceProxy>(json).PatientRequestsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return PatientRequestList;
        }
    }
}