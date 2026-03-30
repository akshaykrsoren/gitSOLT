using CaregiverLite.CaregiverLiteService;

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
    public class SchedulePatientRequest
    {
        public int PatientRequestId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("PatientName")]
        public string PatientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgAddress")]
        [DisplayName("Address")]
        public string Address { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgZipCode")]
        [StringLength(100, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgZipCode", MinimumLength = 5)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }
        public string MedicalId { get; set; }
        public string Description { get; set; }

        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Status { get; set; }
        public bool IsCancelled { get; set; }
    }

    public class SchedulePatientRequestServiceProxy : CaregiverLiteBaseService
    {
        public PatientRequestList SchedulePatientRequestList { get; set; }
        public PatientRequests SchedulePatientRequestDetail { get; set; }
        public List<PatientRequests> SchedulePatientRequestsList { get; set; }

        public PatientRequestList SchedulePatientCompleteRequestsList { get; set; }
        public string Result { get; set; }

        public SchedulePatientRequestServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> InsertSchedulePatientRequest(SchedulePatientRequest SchedulePatientRequest)
        {

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
    }
}