using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class ClientVisitRequest
    {
        public ClientVisitRequestProviderIdentification ProviderIdentification { get; set; }
        public string VisitOtherID { get; set; }
        public string SequenceID { get; set; }
        public string EmployeeQualifier { get; set; }
        public string EmployeeOtherID { get; set; }

        public string ClientIdentifier { get; set; }

        public string EmployeeIdentifier { get; set; }
        public string GroupCode { get; set; }
        public string ClientIDQualifier { get; set; }
        public string ClientID { get; set; }
        public string ClientOtherID { get; set; }
        public string VisitCancelledIndicator { get; set; }
        public string PayerID { get; set; }
        public string PayerProgram { get; set; }
        public string ProcedureCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string VisitTimeZone { get; set; }
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        public string ContingencyPlan { get; set; }
        public string Reschedule { get; set; }
        public string AdjInDateTime { get; set; }
        public string AdjOutDateTime { get; set; }
        public string BillVisit { get; set; }
        public string HoursToBill { get; set; }
        public string HoursToPay { get; set; }
        public string Memo { get; set; } 
        public string ClientVerifiedTimes { get; set; }
        public string ClientVerifiedTasks { get; set; }
        public string ClientVerifiedService { get; set; }
        public string ClientSignatureAvailable { get; set; }
        public string ClientVoiceRecording { get; set; }
        public List<ClientVisitRequestCalls> Calls { get; set; }
        
    }

    public class ClientVisitRequestProviderIdentification
    {
        public string ProviderID { get; set; } = "000000077";
        public string ProviderQualifier { get; set; } = "MedicaidID";
       
    }

    public class ClientVisitRequestCalls
    {
        public string CallExternalID { get; set; }
        public string CallDateTime { get; set; }
        public string CallAssignment { get; set; }
        public string GroupCode { get; set; }
        public string CallType { get; set; }
        public string ProcedureCode { get; set; }
        public string ClientIdentifierOnCall { get; set; }
        public string MobileLogin { get; set; }
        public string CallLatitude { get; set; }
        public string CallLongitude { get; set; }
        public string Location { get; set; }
        public string TelephonyPIN { get; set; }
        public string OriginatingPhoneNumber { get; set; }
        public string VisitLocationType { get; set; }



    }
}