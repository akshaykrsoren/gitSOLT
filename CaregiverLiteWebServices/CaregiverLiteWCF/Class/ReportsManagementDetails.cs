using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF.Class
{
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
        public string NoOfVisits { get; set; }


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



        // End Added Extra

        //   public List<AttendanceManagementDetails> AttendanceManagementInfo { get; set; }


    }


    [DataContract]
    public class ReportsDetailsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<ReportsManagementDetails> AttendanceManagemenList { get; set; }
    }

}