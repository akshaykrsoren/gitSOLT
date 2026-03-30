using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class PatientRequestMaualEntry
    {
        [DataMember]
        public string PatientRequestId { get; set; }

        [DataMember]
        public string PatientRequestTempId { get; set; }

        [DataMember]
        public string NurseId { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string StartDrivingDateTime { get; set; }

        [DataMember]
        public string StopDrivingDateTime { get; set; }

        [DataMember]
        public string CheckInDateTime { get; set; }

        [DataMember]
        public string CheckOutDateTime { get; set; }

        [DataMember]
        public string ReasonForManualEntry { get; set; }

        [DataMember]
        public string UserId { get; set; }



        [DataMember]
        public string InsertedDateTime { get; set; }
        [DataMember]
        public string PatientRequestManualEntryId { get; set; }
        [DataMember]
        public string NurseName { get; set; }
        [DataMember]
        public string OfficeName { get; set; }
        [DataMember]
        public string OfficeId { get; set; }
        [DataMember]
        public string PatientName { get; set; }
        [DataMember]
        public string AppointmentDate { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Miles { get; set; }

        [DataMember]
        public int NurseScheduleId { get; set; }

        [DataMember]
        public int IsResolved { get; set; }
    }

    [DataContract]
    public class RootResponseManualRequest
    {
        [DataMember]
        public List<PatientRequestMaualEntry> Data { get; set; }
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int Success { get; set; }


    }

    [DataContract]
    public class PatientRequestMaualEntryApproval
    {
        [DataMember]
        public string PatientRequestManualEntryId { get; set; }
        [DataMember]
        public string PatientRequestId { get; set; }

        [DataMember]
        public string ApprovalStatus { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public int NurseScheduleId { get; set; }
    }
}