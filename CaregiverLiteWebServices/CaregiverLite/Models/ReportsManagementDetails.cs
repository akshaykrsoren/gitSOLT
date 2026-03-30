using CaregiverLiteWCF.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CaregiverLite.Models
{
    public class GenerateReports
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ReportsManagementDetails
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


        public string FromDate1 { get; set; }
        public string ToDate1 { get; set; }

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


    public class ReportsManagementDetailss
    {


        public string status { get; set; }
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

        public string NoOfVists { get; set; }

        public double DrivingTotalDistance { get; set; }

        public double DrivingTotalMilesToPay { get; set; }

        public int NoOfVisitsCompleted { get; set; }

        public int NoOfVisitsCancelled { get; set; }

        public string FromDate1 { get; set; }
        public string ToDate1 { get; set; }

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

   
    public class ReportsDetailsLists
    {
   
        public int TotalNumberofRecord { get; set; }

        public int FilteredRecord { get; set; }

       
        public List<ReportsManagementDetailss> AttendanceManagemenList { get; set; }
    }



    public class AllIndividualsCareGiverReport
    {
       public List<ReportsManagementDetailss> GetAllVisitReportCaregiver { get; set; }

       public List<ReportsManagementDetailss> GetCaregiverVisitsData { get; set; }

    }

    public class ReoortsManagementServiceProxy : CaregiverLiteBaseService
    {


        public string Result { get; set; }
        public bool ResultInBool { get; set; }
        public List<ReportsManagementDetails> ReportsDetailsList { get; set; }
        public ReportsDetailsList ListAttendanceDetails { get; set; }
        public ReportsManagementDetails AttendancetDetail { get; set; }

        // public GenerateReport GenerateReportDetails { get; set; }

        // public List<GenerateReport> GenerateReportList { get; set; }

        public ReoortsManagementServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<ReportsDetailsList> AGetAllAttendanceDetail(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, int FilterNurseId)
        {

            List<ReportsManagementDetails> objPatientDetailsList = new List<ReportsManagementDetails>();
            ReportsDetailsList AttendanceDetail = new ReportsDetailsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AGetAttendanceDetailsList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + FilterNurseId + "/" + search, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AttendanceDetail = JsonConvert.DeserializeObject<ReoortsManagementServiceProxy>(json).ListAttendanceDetails;
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




        // public async Task<ReportsDetailsList> GetAllAttendanceReports(int pageno, int recordperpage, string search, string sortfield, string sortOrder, int IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterCaregiver, int FilterOffice)
        public async Task<ReportsDetailsList> GetAllAttendanceReports(int pageno, int recordperpage, string search, string sortfield, string sortOrder, int IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterCaregiver)
        {

            List<ReportsManagementDetails> objPatientDetailsList = new List<ReportsManagementDetails>();
            ReportsDetailsList AttendanceDetail = new ReportsDetailsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAttendanceportsOfCaregiver/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + IsAdmin + "/" + LogInUserId + "/" + FromDate + "/" + ToDate + "/" + FilterCaregiver, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AttendanceDetail = JsonConvert.DeserializeObject<ReoortsManagementServiceProxy>(json).ListAttendanceDetails;
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
            return AttendanceDetail;
        }







        public async Task<ReportsDetailsList> GetAllcare(string logInUserId)
        {


            List<ReportsManagementDetails> objPatientDetailsList = new List<ReportsManagementDetails>();
            ReportsDetailsList AttendanceDetail = new ReportsDetailsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "ListCaregiver/" + logInUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AttendanceDetail = JsonConvert.DeserializeObject<ReoortsManagementServiceProxy>(json).ListAttendanceDetails;
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

    }
       
    }