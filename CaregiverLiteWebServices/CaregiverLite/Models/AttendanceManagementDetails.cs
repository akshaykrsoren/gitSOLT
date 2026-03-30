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

    public class AttendanceManagementDetail
    {

        public int id { get; set; }
        public string PatientRequestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NurseId { get; set; }
        // public DateTime Date { get; set; }
        public string Date { get; set; }
        public string RequestedForVisit { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Name { get; set; }
        public string PatientName { get; set; }
        public string Address { get; set; }
        public DateTime? DrivingStartTime { get; set; }
        public DateTime? DrivingStopTime { get; set; }
        public Boolean IsCancelled { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        // Start Added Extra

        public DateTime? CheckInDateTime { get; set; }
        public DateTime? CheckOutDateTime { get; set; }

        public string CheckInTotalTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DrivingStopLatitude { get; set; }
        public string DrivingStopLongitude { get; set; }

        public string TotalTravelTime { get; set; }

        public string RequestedDuration { get; set; }




    }







    public class AttendanceManagementServiceProxy : CaregiverLiteBaseService
    {


        public string Result { get; set; }
        public bool ResultInBool { get; set; }
        public List<AttendanceManagementDetails> AttendanceDetailsList { get; set; }
        public AttendanceDetailsList ListAttendanceDetails { get; set; }
        public AttendanceManagementDetails AttendancetDetail { get; set; }

        // public GenerateReport GenerateReportDetails { get; set; }

        // public List<GenerateReport> GenerateReportList { get; set; }

        public AttendanceManagementServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }



        public async Task<AttendanceDetailsList> GetAllAttendanceDetail(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, int OfficeId, int OrganisationId)
        {

            List<AttendanceManagementDetails> objPatientDetailsList = new List<AttendanceManagementDetails>();
            AttendanceDetailsList AttendanceDetail = new AttendanceDetailsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAttendanceDetailsList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + OfficeId + "/" + search + "/" + OrganisationId.ToString(), this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AttendanceDetail = JsonConvert.DeserializeObject<AttendanceManagementServiceProxy>(json).ListAttendanceDetails;
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
            return AttendanceDetail;
        }







        public async Task<string> ExcelReport(Scheduler Scheduler)
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





    }
}