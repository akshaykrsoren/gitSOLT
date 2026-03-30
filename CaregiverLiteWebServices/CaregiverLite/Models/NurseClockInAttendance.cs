using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class NurseClockInAttendance
    {
            public int ClockRequestId { get; set; }
  
            public string ClockInDateTime { get; set; }

            public string ClockOutDatetime { get; set; }

            public string ClockInStatus { get; set; }

        public string Name { get; set; }
        public int NurseId { get; set; }

        public string Address { get; set; }

        public string Date { get; set; }

            public string UserId { get; set; }
           public string ServiceTime { get; set; }
    }
}