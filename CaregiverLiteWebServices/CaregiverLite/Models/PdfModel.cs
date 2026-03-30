using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class PdfModel
    {
        public string name { get; set; }
        public string quality { get; set; }
        public string description { get; set; }

        public string PatientName { get; set; }
        public string AppointmentId { get; set; }
        public string ScheduledEmployee { get; set; }
        public string Rescheduledemployee { get; set; }
        public string ReschedulingrequestEmployeeName { get; set; }
        public string ScheduledDate { get; set; }
        public string ReScheduledDate { get; set; }
        public string Fromtime { get; set; }
        public string Totime { get; set; }
        public string Time { get; set; }
        public string Rtime { get; set; }
        public string Reason { get; set; }
        public string TypesOfVisit { get; set; }
    }


    public class RootResponse
    {
        public int success { get; set; }

        public List<Response> data { get; set; }

        public string message { get; set; }
    }


    public class Response
    {
        public string TransactionId { get; set; }
        public string TrasactionStatus { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionStatus { get; set; }

    }


}