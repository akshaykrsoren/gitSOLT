using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class ManualEntryModel
    {

        public string PatientRequestManualEntryId { get; set; }

        public string PatientRequestId { get; set; }


        public string ApprovalStatus { get; set; }






        public string PatientRequestTempId { get; set; }


        public string NurseId { get; set; }


        public string Status { get; set; }


        public string StartDrivingDateTime { get; set; }


        public string StopDrivingDateTime { get; set; }


        public string CheckInDateTime { get; set; }


        public string CheckOutDateTime { get; set; }


        public string ReasonForManualEntry { get; set; }


        public string UserId { get; set; }

        public string InsertedDateTime { get; set; }



        public string NurseName { get; set; }

        public string OfficeName { get; set; }

        public string OfficeId { get; set; }

        public string PatientName { get; set; }

        public string AppointmentDate { get; set; }

        public string Comment { get; set; }

        public string Description { get; set; }
        public string Miles { get; set; }
        public string NurseScheduleId { get;  set; }


        public string ImagePath { get; set; }


        public string CheckoutFormReason { get; set; }


        public List<string> ADLs { get; set; } = new List<string>();
        public List<string> IADLs { get; set; } = new List<string>();
    }
}