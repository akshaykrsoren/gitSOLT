using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{

    public class MarketerAttendanceModel
    {
        public int MarketersId { get; set; }
        public string ClientRequestId { get; set; }

        public int ReferralId { get; set; }

        public string Review { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string MarketersName { get; set; }

        public string TotalTravelTime { get; set; }

        public string Address { get; set; }

        public string ClientName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string CheckInTotalTime { get; set; }
        public string TotalHours { get; set; }
        public string Dateofvisit { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }


    }

    public class MarketerAttendaceList
    {

        public int TotalNumberofRecord { get; set; }


        public int FilteredRecord { get; set; }


        public List<MarketerAttendanceModel> MarketersAttendanceList { get; set; }
    }

}

