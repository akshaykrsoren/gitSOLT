using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class RescheduleFormData
    {

            public string OfficeId { get; set; }

            public string OfficeName { get; set; }

            public List<PdfModel> PdfDetail { get; set; }

    }



    public class ReScheduledFrom
    {

        public string EmployeeName { get; set; }
        public string OfficeName { get; set; }
        public string PatientGroupName { get; set; }
        public List<PdfDataDescription1> ReScheduledFromDetails { get; set; }
        public string ScheduledDate { get; set; }
        public string ScheduledTime { get; set; }
        public string ReScheduledDate { get; set; }
        public string ReScheduledTime { get; set; }
        public string ReschedulingFormUrl { get; set; }
        public string Reason { get; set; }
        public string TypesOfVisit { get; set; }
        public string CreatedDate { get; set; }
        public string patientname { get; set; }


        public List<PdfDataDescription1> ReshuduleDataList { get; set; }
    }


    public class PdfDataDescription1
    {

        public string CreatedDate { get; set; }
        public string PatientGroupName { get; set; }
        public string ScheduledDate { get; set; }
        public string ScheduledTime { get; set; }
        public string ReScheduledDate { get; set; }
        public string ReScheduledTime { get; set; }
        public string ReschedulingFormUrl { get; set; }
        public string Reason { get; set; }
        public string TypesOfVisit { get; set; }

        public string patientname { get; set; }
    }

    
}