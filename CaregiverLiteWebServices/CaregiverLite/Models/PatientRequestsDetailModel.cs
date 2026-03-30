using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class PatientRequestsDetailModel
    {
        public string AppointmentDate { get; set; }
        public string AppointmentDuration { get; set; }
        public string CaregiverName { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string Description { get; set; }
        public string DrivingDuration { get; set; }
        public object IsRequiredDriving { get; set; }
        public string JurisdictionCode { get; set; }
        public string MaxDistance { get; set; }
        public string OfficeName { get; set; }
        public string PatientAddress { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PayerId { get; set; }
        public string PayerProgram { get; set; }
        public string ProcedureCode { get; set; }
        public string Speciality { get; set; }
        public string Status { get; set; }
        public string ZipCode { get; set; }

        public string ServiceType {get; set;}


        public string StartDrivingDatetime { get; set; }
        public string StopDrivingDatetime { get; set; }

        public string CheckInDatetTime { get; set; }
        public string CheckOutDateTime { get; set; }

        public string StartDrivingLattitude {get;set;}
        public string StopDrivinglatittude  {get;set;}
        public string StartDrivingLongitude { get; set;}
        public string stopDrivingLongitude { get; set; }

        public string  CheckinLatittude  {get;set;}
        public string  CheckinLongitude  {get;set;}
        public string  CheckoutLongitude {get;set;}
        public string  CheckoutLatittude { get; set;}

        public string VisitTypeName { get; set; }
        public string Rating { get; set; }

        public string CheckoutFormReason { get; set; }

        public string TotalTravelTime { get; set; }
        public string PatientSignature { get; set; }

        public string IsManual { get; set; }
                        
        public string FromTime { get; set; }
        public string ToTime { get; set; }

        public bool TrackMilage { get; set; }

        public string PatientrequestId { get; set; }

        public List<string> ADLs { get; set; } = new List<string>();
        public List<string> IADLs { get; set; } = new List<string>();
        public string ScheduledBy { get;   set; }
    }
}