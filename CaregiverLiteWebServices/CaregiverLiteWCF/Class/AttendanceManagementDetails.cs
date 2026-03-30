using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF
{
    public class AttendanceManagementDetails
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

        public int IsEdited { get; set; }

        public int IsForceCheckIn { get; set; }

        public int IsMaual { get; set; }

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
    public class AttendanceDetailsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<AttendanceManagementDetails> AttendanceManagemenList { get; set; }
    }



    [DataContract]
    public class ReasonForCompleteRequests
    {
        //[DataMember]
        //public string MainReasonType { get; set; }

        [DataMember(Order = 2)]

        public List<ReasonCodeType> ADLs { get; set; }


        [DataMember(Order = 3)]
        public List<ReasonCodeType> IADLs { get; set; }


        [DataMember(Order = 1)]
        public List<ReasonCodeType> Acknowledgement { get; set; }
    }


    [DataContract]
    public class ReasonCodeType
    {
        [DataMember]
        public int ReasonId { get; set; }


        [DataMember]
        public string CompleteRequestsReason { get; set; }

    }






}
