using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class AppointmentRequestDetails
    {
           
           public string PatientRequestId { get; set; }
           public string StartDrivingDateTime { get; set; }
           public string StopDrivingDateTime { get; set; }
           public string startDrivingLattitude { get; set; }
           public string startDrivingLongitude { get; set; }
           public string stopDrivingLattitude { get; set; }
           public string stopDrivingLongitude { get; set; }
           public string CheckInDateTime { get; set; }
           public string CheckOutDateTime { get; set; }
           public string CheckInLattitude { get; set; }
           public string CheckInLongitude { get; set; }
           public string CheckOutLattitude { get; set; }
           public string CheckOutLongitude { get; set; }

          public string CheckOutFormReason { get; set; }
    }



}
