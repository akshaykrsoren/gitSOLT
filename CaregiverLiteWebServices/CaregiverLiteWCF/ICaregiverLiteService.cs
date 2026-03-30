using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using CaregiverLiteWCF.Class;

namespace CaregiverLiteWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICaregiverLiteService" in both code and config file together.
    [ServiceContract]
    public interface ICaregiverLiteService
    {
        // ERROR LOG 
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertErrorLog")]
        [return: MessageParameter(Name = "Result")]
        string InsertErrorLog(ErrorLog ErrorLog);

        // LOGS
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertLog")]
        [return: MessageParameter(Name = "Result")]
        string InsertLog(Logs objLogs);

        // SERVICES
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllServices/{OrganisationId}")]
        [return: MessageParameter(Name = "ServicesList")]
        List<Services> GetAllServices(String OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertUpdateService")]
        [return: MessageParameter(Name = "Result")]
        string InsertUpdateService(Services Service);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteService/{ServiceId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteService(string ServiceId, string UserId);

        // Care Giver
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllCareGivers/{LoginUserId}/{pageNo}/{recordPerPage}/{IsApprove}/{sortfield}/{sortorder}/{search}/{FilterOffice}/{OrganisationId}")]
        [return: MessageParameter(Name = "CareGiverList")]
        CareGiversList GetAllCareGivers(string LoginUserId,string pageNo, string recordPerPage, string IsApprove, string sortfield, string sortorder, string search, string FilterOffice, string OrganisationId);


        // Care Giver
        [OperationContract]
      
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllCareGiversList/{SchedulerUserId}")]
        [return: MessageParameter(Name = "CareGiverList")]
        CareGiversList GetAllCareGiversList(string SchedulerUserId);
        // Care Giver
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllNotifiedCareGiversByRequestId/{PatientRequestId}")]
        [return: MessageParameter(Name = "CareGiversList")]
        List<CareGivers> GetAllNotifiedCareGiversByRequestId(string PatientRequestId);

        // Care Giver Track Location
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetTrackLocationByPatientRequestId/{PatientRequestId}")]
        [return: MessageParameter(Name = "CareGiverTrackLocationList")]
        List<CareGiverTrackLocation> GetTrackLocationByPatientRequestId(string PatientRequestId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetTrackLocationByNurseId/{NurseId}/{date}")]
        [return: MessageParameter(Name = "CareGiverTrackLocationList")]
        List<CareGiverTrackLocation> GetTrackLocationByNurseId(string NurseId, string date);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertUpdateCareGiverByAdmin")]
        [return: MessageParameter(Name = "Result")]
        string InsertUpdateCareGiverByAdmin(CareGivers CareGiver);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertNurseProfile")]
        [return: MessageParameter(Name = "Result")]
        string InsertNurseProfile(CareGivers CareGiver);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllCareGiverByNurseId/{NurseId}")]
        [return: MessageParameter(Name = "CareGiverDetail")]
        CareGivers GetAllCareGiverByNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteNurse/{NurseId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteNurse(string NurseId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllNursesByDateFilter/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "CareGiversList")]
        List<CareGivers> GetAllNursesByDateFilter(string FromDate, string ToDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ApproveRejectNurse")]
        [return: MessageParameter(Name = "Result")]
        string ApproveRejectNurse(CareGivers CareGiver);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCareGiverByServiceId/{ServiceId}")]
        [return: MessageParameter(Name = "CareGiversList")]
        List<CareGivers> GetCareGiverByServiceId(string ServiceId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCareGiverForPatiantRequest")]
        [return: MessageParameter(Name = "CareGiversList")]
        List<CareGivers> GetCareGiverForPatiantRequest(PatientRequests PatientRequest);

        //PATIENTS
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllUsersForAdmin/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        [return: MessageParameter(Name = "PatientList")]
        PatientsList GetAllUsersForAdmin(string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllPatientsByDateFilter/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "PatientsList")]
        List<Patients> GetAllPatientsByDateFilter(string FromDate, string ToDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientInfoForDashboard/{ZipCode}/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "PatientsList")]
        List<Patients> GetPatientInfoForDashboard(string ZipCode, string FromDate, string ToDate);

        // CAREGIVER TIME SLOTS
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetTimeSlotByNurseId/{NurseId}/{Week}/{Year}")]
        [return: MessageParameter(Name = "TimeSlotsList")]
        List<CareGiverTimeSlots> GetTimeSlotByNurseId(string NurseId, string Week, string Year);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertTimeSlotForNurse")]
        [return: MessageParameter(Name = "Result")]
        string InsertTimeSlotForNurse(CareGiverTimeSlots CareGiverTimeSlot);

        // CAREGIVER SCHEDULE
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetScheduleByTimeSlotId/{TimeSlotId}")]
        [return: MessageParameter(Name = "ScheduleList")]
        List<CareGiverSchedule> GetScheduleByTimeSlotId(string TimeSlotId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertScheduleForNurse")]
        [return: MessageParameter(Name = "Result")]
        string InsertScheduleForNurse(CareGiverSchedule CareGiverSchedule);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteScheduleByTimeSlotId")]
        [return: MessageParameter(Name = "Result")]
        string DeleteScheduleByTimeSlotId(CareGiverSchedule CareGiverSchedule);

        // PATIENTS REQUESTS
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllPatientRequests/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        [return: MessageParameter(Name = "PatientRequestList")]
        PatientRequestList GetAllPatientRequests(string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAppointmentDetailByAppointmentId/{AppointmentId}")]
        [return: MessageParameter(Name = "PatientRequestDetail")]
        PatientRequests GetAppointmentDetailByAppointmentId(string AppointmentId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertPatientRequestForAdmin")]
        [return: MessageParameter(Name = "Result")]
        string InsertPatientRequestForAdmin(PatientRequests PatientRequest);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CancelPatientAppointment/{AppointmentId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string CancelPatientAppointment(string AppointmentId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ChangeCareGiver/{AppointmentId}/{NurseId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string ChangeCareGiver(string AppointmentId, string NurseId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRescheduleAppoinmentRequest/{AppointmentId}")]
        [return: MessageParameter(Name = "PatientRequestDetail")]
        PatientRequests GetRescheduleAppoinmentRequest(string AppointmentId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "RescheduleAppoinment")]
        [return: MessageParameter(Name = "Result")]
        string RescheduleAppoinment(PatientRequests PatientRequest);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAppointmentsByDateFilter/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetAppointmentsByDateFilter(string FromDate, string ToDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPaymentReceived/{NurseId}/{PatientId}/{ZipCode}/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetPaymentReceived(string NurseId, string PatientId, string ZipCode, string FromDate, string ToDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetProjectedPayment/{NurseId}/{PatientId}/{ZipCode}/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetProjectedPayment(string NurseId, string PatientId, string ZipCode, string FromDate, string ToDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRescheduleApprovalAlerts")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetRescheduleApprovalAlerts();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRescheduleAlerts")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetRescheduleAlerts();

        // UNAVAILABILITY REQUEST
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllUnavailabilityRequest/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        [return: MessageParameter(Name = "UnavailabilityRequestsList")]
        UnavailabilityRequestsList GetAllUnavailabilityRequest(string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ApproveRejectUnavailabilityRequest")]
        [return: MessageParameter(Name = "Result")]
        string ApproveRejectUnavailabilityRequest(UnavailabilityRequests UnavailabilityRequest);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAppointmentsByUnavailabilityRequestId/{UnavailabilityRequestId}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetAppointmentsByUnavailabilityRequestId(string UnavailabilityRequestId);

        // DISPUTE
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllDisputes")]
        [return: MessageParameter(Name = "DisputeList")]
        List<Disputes> GetAllDisputes();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InserDisputeComment")]
        [return: MessageParameter(Name = "Result")]
        string InserDisputeComment(Disputes Dispute);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetDisputesDetailByDisputeId/{DisputeId}")]
        [return: MessageParameter(Name = "DisputeDetail")]
        Disputes GetDisputesDetailByDisputeId(string DisputeId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertPaymentTransaction")]
        [return: MessageParameter(Name = "Result")]
        string InsertPaymentTransaction(PaymentTransaction Transaction);

        // Message Queue
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertMessageQueue")]
        [return: MessageParameter(Name = "Result")]
        string InsertMessageQueue(MessageQueue MessageQueue);

        // Permissions
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllPermissionForUser/{UserId}")]
        [return: MessageParameter(Name = "UserPermissionList")]
        List<Permission> GetAllPermissionForUser(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllRolesForUser/{UserId}")]
        [return: MessageParameter(Name = "UserRoleList")]
        List<Role> GetAllRolesForUser(string UserId);

        // Administrator
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertUpdateSuperAdmin")]
        [return: MessageParameter(Name = "Result")]
        string InsertUpdateSuperAdmin(SuperAdmin SuperAdmin);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertUserPermission/{UserId}/{PermissionId}")]
        [return: MessageParameter(Name = "Result")]
        string InsertUserPermission(string UserId, string PermissionId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteUserWithPermissions/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteUserWithPermissions(string UserId);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAdmin/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        //[return: MessageParameter(Name = "AdminList")]
        //AdminsList GetAllAdmin(string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteAdmin/{UserId}")]
        //[return: MessageParameter(Name = "Result")]
        //string DeleteAdmin(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAdminDetailsById/{UserId}")]
        [return: MessageParameter(Name = "SuperAdmin")]
        SuperAdmin GetAdminDetailsById(string UserId);

        // Push notification
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertPushNotification")]
        [return: MessageParameter(Name = "Result")]
        string InsertPushNotification(Notification Notification);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertSentNotification")]
        [return: MessageParameter(Name = "Result")]
        string InsertSentNotification(Notification Notification);
        /* ----------------------new ------------------*/

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCareGiverByUserId/{UserId}")]
        [return: MessageParameter(Name = "CareGiverDetail")]
        CareGivers GetCareGiverByUserId(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllPatientRequestsByNurseId/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{NurseId}")]
        [return: MessageParameter(Name = "PatientRequestList")]
        PatientRequestList GetAllPatientRequestsByNurseId(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ResetPassword")]
        [return: MessageParameter(Name = "Result")]
        string ResetPassword(CareGivers CareGiver);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertNurseCertificate/{NurseId}/{Certificate}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string InsertNurseCertificate(string NurseId, string Certificate, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCareGiverCertiByNurseId/{NurseId}")]
        [return: MessageParameter(Name = "CareGiverListCertificate")]
        List<CareGivers> GetCareGiverCertiByNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPaymentReceivedByNurseId/{NurseId}/{PatientName}/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetPaymentReceivedByNurseId(string NurseId, string PatientName, string FromDate, string ToDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAppointmentsByNurseId/{NurseId}/{IsComplete}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        [return: MessageParameter(Name = "PatientCompleteRequestsList")]
        PatientRequestList GetAppointmentsByNurseId(string NurseId, string IsComplete, string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRescheduleAlertsByNurseId/{NurseId}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetRescheduleAlertsByNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCancelledAppointmentByPatientForNurseId/{NurseId}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetCancelledAppointmentByPatientForNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPaymentRequertForMapByNurseId/{NurseId}/{NurseName}/{FromDate}/{ToDate}")]
        [return: MessageParameter(Name = "PatientRequestsList")]
        List<PatientRequests> GetPaymentRequertForMapByNurseId(string NurseId, string NurseName, string FromDate, string ToDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "UpdateAvailableOrBusyStatus/{NurseId}/{UserId}/{StatusId}/{StatusType}")]
        [return: MessageParameter(Name = "Result")]
        string UpdateAvailableOrBusyStatus(string NurseId, string UserId, string StatusId, string StatusType);

        /*   Stripe Access Token   */

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertStripeAccessToken")]
        [return: MessageParameter(Name = "Result")]
        string InsertStripeAccessToken(StripeAccessToken StripeAccessToken);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetStripeAccessTokenByNurseId/{NurseId}")]
        [return: MessageParameter(Name = "AccessTokenDetail")]
        StripeAccessToken GetStripeAccessTokenByNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "UpdateNurseSchedule")]
        [return: MessageParameter(Name = "Result")]
        string UpdateNurseSchedule(CareGiverMultipleTimeSlots objCareGiverMultipleTimeSlots);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteTimeSlotByTimeSlotId")]
        [return: MessageParameter(Name = "Result")]
        string DeleteTimeSlotByTimeSlotId(CareGiverTimeSlots CareGiverTimeSlot);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ForgotPassword/{Email}")]
        [return: MessageParameter(Name = "Result")]
        string ForgotPassword(string Email);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EmailtoSetPassword/{Email}")]
        [return: MessageParameter(Name = "Result")]
        string EmailtoSetPassword(string Email);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "UpdateCareGiver")]
        [return: MessageParameter(Name = "Result")]
        string UpdateCareGiver(CareGivers CareGiver);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteNurseCertificateById/{UserId}/{NurseCertificateId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteNurseCertificateById(string UserId, string NurseCertificateId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAppointmentByTimeSlotId/{TimeSlotId}/{SlotDate}")]
        [return: MessageParameter(Name = "ScheduleList")]
        List<CareGiverSchedule> GetAppointmentByTimeSlotId(string TimeSlotId, string SlotDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAppointmentsForUnavailability/{TimeSlotId}/{SlotDate}")]
        [return: MessageParameter(Name = "ScheduleList")]
        List<UnavailabilityRequest> GetAppointmentsForUnavailability(string TimeSlotId, string SlotDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertUnavailabilityRequest")]
        [return: MessageParameter(Name = "Result")]
        string InsertUnavailabilityRequest(UnavailabilityRequest UnavailabilityRequest);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetUnavailabilityRequestByTimeSlot/{TimeSlotId}/{SlotDate}")]
        [return: MessageParameter(Name = "Result")]
        string GetUnavailabilityRequestByTimeSlot(string TimeSlotId, string SlotDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertUpdateCareGiverReview")]
        [return: MessageParameter(Name = "Result")]
        string InsertUpdateCareGiverReview(CareGiverReview CareGiverReview);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllCareGiverReview/{PatientId}")]
        [return: MessageParameter(Name = "AllCareGiveReview")]
        CareGiverReviewList GetAllCareGiverReview(string PatientId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCareGiverReview/{NurseId}")]
        [return: MessageParameter(Name = "CareGiveReview")]
        CareGiverReviewList GetCareGiverReview(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCareGiverUserNRate/{NurseId}")]
        [return: MessageParameter(Name = "CareGiverUserNRate")]
        CareGiverUserNRate GetCareGiverUserNRate(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CheckValidAppointmentDate/{AppointmentId}")]
        [return: MessageParameter(Name = "Result")]
        string CheckValidAppointmentDate(string AppointmentId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertSchedulePatientRequest")]
        [return: MessageParameter(Name = "CareGiverList")]
        CareGiversList InsertSchedulePatientRequest(SchedulePatientRequest SchedulePatientRequest);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetFilterDates")]
        [return: MessageParameter(Name = "GetDatesList")]
        List<GetDatesList> GetFilterDates(SchedulePatientRequest SchedulePatientRequest);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllSchedulePatientRequest/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{IsAdmin}/{LogInUserId}/{FromDate}/{ToDate}/{FilterStatus}/{FilterOffice}/{OrganisationId}")]
        [return: MessageParameter(Name = "SchedulePatientRequestList")]
        SchedulePatientRequestList GetAllSchedulePatientRequest(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterStatus, string FilterOffice, string OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertSchedulePatientRequestTemp/{PatientRequestId}/{NurseId}/{IsNotificationSent}")]
        [return: MessageParameter(Name = "Result")]
        string InsertSchedulePatientRequestTemp(string PatientRequestId, string NurseId, string IsNotificationSent);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CancelPatientRequest/{patientid}/{Userid}")]
        [return: MessageParameter(Name = "CareGiverList")]
        CareGiversList CancelPatientRequest(string patientid, string Userid);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCareGiverDetailsByUserId/{UserId}")]
        [return: MessageParameter(Name = "CareGiverDetail")]
        CareGivers GetCareGiverDetailsByUserId(string UserId);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientRequestDetailByMedicalID/{LoginUserId}/{MedicalID}/{OrganisationId}")]
        //[return: MessageParameter(Name = "SchedulePatientRequestDetail")]
        //SchedulePatientRequest GetPatientRequestDetailByMedicalID(string LoginUserId, string MedicalID, string OrganisationId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientRequestDetailByMedicalID/{LoginUserId}/{MedicalID}")]
        [return: MessageParameter(Name = "SchedulePatientRequestDetail")]
        SchedulePatientRequest GetPatientRequestDetailByMedicalID(string LoginUserId, string MedicalID);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientRequestDetailByMedicalID/{LoginUserId}/{MedicalID}/{OrganisationId}")]
        [return: MessageParameter(Name = "SchedulePatientRequestDetail")]
        SchedulePatientRequest GetPatientRequestDetailByMedicalIDByOrganisation(string LoginUserId, string MedicalID, string OrganisationId);

        ///----------------- For Scheduler
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddScheduler")]
        [return: MessageParameter(Name = "Result")]
        string AddScheduler(Scheduler Scheduler);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EditScheduler")]
        [return: MessageParameter(Name = "Result")]
        string EditScheduler(Scheduler Scheduler);
        // Scheduler
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllScheduler/{LoginUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{OfficeId}/{OrganisationId}/{IsActiveStatus}")]
        [return: MessageParameter(Name = "SchedulersList")]
        SchedulersList GetAllScheduler(string LoginUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search,string OfficeId, string OrganisationId,string IsActiveStatus);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllSchedulerList")]
        [return: MessageParameter(Name = "SchedulersList")]
        SchedulersList GetAllSchedulerList();
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteScheduler/{SchedulerId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteScheduler(string SchedulerId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetSchedulerDetailById/{SchedulerId}")]
        [return: MessageParameter(Name = "SchedulerDetail")]
        Scheduler GetSchedulerDetailById(string SchedulerId);

        //Reward Point
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllRewardPointDetail/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{FilterOffice}/{UserId}")]
        [return: MessageParameter(Name = "RewardPointsList")]
        RewardPointsList GetAllRewardPointDetail(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string FilterOffice,string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllRewardPointAdvanceDetail/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{FilterOffice}/{UserId}")]
        [return: MessageParameter(Name = "RewardPointsList")]
        RewardPointsList GetAllRewardPointAdvanceDetail(string pageNo, string recordPerPage, string sortfield, string sortorder, string search ,string FilterOffice,string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EditComment/{NurseId}/{Comment}")]
        [return: MessageParameter(Name = "Result")]
        string EditComment(string NurseId, string Comment);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllRewardPointByNurseId/{NurseId}")]
        [return: MessageParameter(Name = "RewardPointsList")]
        RewardPointsList GetAllRewardPointByNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRateByPatientRequestId/{PatientRequestId}")]
        [return: MessageParameter(Name = "Result")]
        string GetRateByPatientRequestId(string PatientRequestId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SetRateByPatientRequestId/{PatientRequestId}/{Rating}")]
        [return: MessageParameter(Name = "Result")]
        string SetRateByPatientRequestId(string PatientRequestId, string Rating);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllRatingByNurseId/{NurseId}")]
        [return: MessageParameter(Name = "RatingsList")]
        RatingsList GetAllRatingByNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllChattingList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{OfficeId}")]
        [return: MessageParameter(Name = "ChattingsList")]
        ChattingsList GetAllChattingList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search,string OfficeId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetChattingListPatientGroupWise/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{GroupTypeId}")]
        [return: MessageParameter(Name = "ChattingsList")]
        ChattingsList GetChattingListPatientGroupWise(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search,string GroupTypeId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetQuickBloxIdByNurseId/{NurseId}")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting GetQuickBloxIdByNurseId(string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetQuickBloxIdBySchedulerUserId/{SchedulerId}")]
        [return: MessageParameter(Name = "QuickBloxId")]
        string GetQuickBloxIdBySchedulerUserId(string SchedulerId);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetQuickBloxIdByUserId/{UserId}")]
        //[return: MessageParameter(Name = "QuickBloxId")]
        //string GetQuickBloxIdByUserId(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SaveQBId/{UserId}/{QuickBloxId}")]
        [return: MessageParameter(Name = "Result")]
        string SaveQBId(string UserId, string QuickBloxId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SaveDialogId/{NurseUserId}/{SchedulerUserId}/{DialogId}/{UserType}")]
        [return: MessageParameter(Name = "Result")]
        string SaveDialogId(string NurseUserId, string SchedulerUserId, string DialogId, string UserType);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddGroupDialogId/{DialogId}/{GroupName}/{UserId}/{OfficeId}")]
        //[return: MessageParameter(Name = "Result")]
        //string AddGroupDialogId(string DialogId, string GroupName, string UserId,string OfficeId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetDialogId/{NurseUserId}/{SchedulerUserId}")]
        [return: MessageParameter(Name = "Result")]
        string GetDialogId(string NurseUserId, string SchedulerUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetDialogDetail/{Id}")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting GetDialogDetail(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetGroupMemberDetail/{Id}/{OrganisationId}")]
        [return: MessageParameter(Name = "objGroupMemberDetailList")]
        List<ChattingGroupMember> GetGroupMemberDetail(string Id, string OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetSchedulerDetailByUserId/{SchedulerUserId}")]
        [return: MessageParameter(Name = "SchedulerDetail")]
        Scheduler GetSchedulerDetailByUserId(string SchedulerUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllDialogId/{SchedulerUserId}")]
        [return: MessageParameter(Name = "ChattingsList")]
        ChattingsList GetAllDialogId(string SchedulerUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllGroupDialogId/{SchedulerUserId}")]
        [return: MessageParameter(Name = "ChattingsList")]
        ChattingsList GetAllGroupDialogId(string SchedulerUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "RemovePatientRequest/{PatientRequestId}/{UserID}")]
        [return: MessageParameter(Name = "CareGiverList")]
        string RemovePatientRequest(string PatientRequestId, string UserID);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "checkDialogId/{GroupName}")]
        [return: MessageParameter(Name = "Flag")]
        bool checkDialogId(string GroupName);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotifyUserForChatMessage")]
        [return: MessageParameter(Name = "CaregiverList")]
        List<CareGivers> NotifyUserForChatMessage(CareGivers CareGiver);


        ////Check DialogId Exist Or not
        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "checkDialogId/{GroupName}")]
        //[return: MessageParameter(Name = "Flag")]
        //bool checkDialogId(string GroupName);

        //NurseCoordinator
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllNurseCoordinators/{loginUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{officeId}/{search}/{OrganisationId}/{IsActiveStatus}")]
        [return: MessageParameter(Name = "NurseCoordinatorsList")]
        NurseCoordinatorsList GetAllNurseCoordinators(string loginUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string officeId, string search, string OrganisationId,string IsActiveStatus);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddNurseCoordinator")]
        [return: MessageParameter(Name = "Result")]
        string AddNurseCoordinator(NurseCoordinator NurseCoordinator);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteNurseCoordinator/{NurseCoordinatorId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteNurseCoordinator(string NurseCoordinatorId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EditNurseCoordinator")]
        [return: MessageParameter(Name = "Result")]
        string EditNurseCoordinator(NurseCoordinator NurseCoordinator);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetNurseCoordinatorDetailById/{NurseCoordinatorId}")]
        [return: MessageParameter(Name = "NurseCoordinatorDetail")]
        NurseCoordinator GetNurseCoordinatorDetailById(string NurseCoordinatorId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetUnAssignedNurseCoordinatorList/{ChattingGroupId}")]
        [return: MessageParameter(Name = "NurseCoordinatorList")]
        List<NurseCoordinator> GetUnAssignedNurseCoordinatorList(string ChattingGroupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SetNurseCoordinator/{ChattingGroupId}/{NurseCoordinatorId}/{Permission}")]
        [return: MessageParameter(Name = "Result")]
        string SetNurseCoordinator(string ChattingGroupId, string NurseCoordinatorId, string Permission);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SaveQBIdOfNurseCoordinator/{Email}/{QuickBloxId}")]
        [return: MessageParameter(Name = "Result")]//SaveQBId
        string SaveQBIdOfNurseCoordinator(string Email, string QuickBloxId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetNurseCoordinatorDetailByUserId/{NurseCoordinatorUserId}")]
        [return: MessageParameter(Name = "NurseCoordinatorDetail")]
        NurseCoordinator GetNurseCoordinatorDetailByUserId(string NurseCoordinatorUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetNurseCoordinatorPermissionGroupWise/{ChattingGroupId}/{LoginUserUserId}")]
        [return: MessageParameter(Name = "Result")]
        string GetNurseCoordinatorPermissionGroupWise(string ChattingGroupId, string LoginUserUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetNurseCoordinatorPermissionGroupWiseList/{ChattingGroupId}")]
        [return: MessageParameter(Name = "objGroupMemberDetailList")]
        List<ChattingGroupMember> GetNurseCoordinatorPermissionGroupWiseList(string ChattingGroupId);

        //For List of Manuel Caregiver List 16-08-2017
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers")]
        [return: MessageParameter(Name = "CareGiverList")]
        CareGiversList GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers(SchedulePatientRequest SchedulePatientRequest);/*From InsertSchedulePatientRequest*/

        //NurseCoordinator GetNurseCoordinatorDetailByUserId(string NurseCoordinatorUserId);


        //30-08-2017 start
        //Patient ADD EDIT DELETE

        //Add New Patient 
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddPatient")]
        [return: MessageParameter(Name = "Result")]
        string AddPatient(PatientsDetail PatientsDetail);

        //Edit Patient 
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EditPatientDetails")]
        [return: MessageParameter(Name = "Result")]
        string EditPatientDetails(PatientsDetail PatientDetails);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientDetailById/{PatientDetailId}")]
        [return: MessageParameter(Name = "PatientDetail")]
        PatientsDetail GetPatientDetailById(string PatientDetailId);//GetNurseCoordinatorDetailById

        //Delete Patient 
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeletePatientDetail/{PatientDetailId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeletePatientDetail(string PatientDetailId,string UserId);

        //List Patient
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientDetailsList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{OrderId}/{search}/{OrganisationId}/{IsActiveStatus}")]
        [return: MessageParameter(Name = "ListPatientDetails")]
        PatientDetailsList GetPatientDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OrderId, string search, string OrganisationId,string IsActiveStatus);
        //30-08-2017 End


		[OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "IsMedicalIdExist")]
        [return: MessageParameter(Name = "ResultInBool")]
        bool IsMedicalIdExist(PatientsDetail PatientsDetail);
	
        //Added by Pinki 
        //Started on 9th Sept,2017
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllSchedulerbyCaregiverId")]
        [return: MessageParameter(Name = "SchedulerList")]
        List<ScheduleInfo> GetAllSchedulerbyCaregiverId(CareGivers CareGiver);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetQuickBloxDetByUserId/{UserId}")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting GetQuickBloxDetByUserId(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetQuickBloxDetById/{Id}")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting GetQuickBloxDetById(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientChattingGroupList/{UserId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetPatientChattingGroupList(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetUnassignedCaregiverList/{ChattingGroupId}")]
        [return: MessageParameter(Name = "CaregiverList")]
        List<CareGivers> GetUnassignedCaregiverList(string ChattingGroupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAssignedCaregiverListGroupWise/{ChattingGroupId}")]
        [return: MessageParameter(Name = "objGroupMemberDetailList")]
        List<ChattingGroupMember> GetAssignedCaregiverListGroupWise(string ChattingGroupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddCaregiverIntoGroup/{ChattingGroupId}/{NurseId}")]
        [return: MessageParameter(Name = "Result")]
        string AddCaregiverIntoGroup(string ChattingGroupId, string NurseId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "RemoveMemberFromGroupChat/{ChattingGroupId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string RemoveMemberFromGroupChat(string ChattingGroupId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllGroupExceptAssignedGroupByUserId/{UserId}/{LoginUserId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetAllGroupExceptAssignedGroupByUserId(string UserId,string LoginUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAssignedGroupByUserId/{UserId}/{LoginUserId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetAllAssignedGroupByUserId(string UserId,string LoginUserId);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AssignGroupToCaregiver/{ChattingGroupId}/{UserId}")]
        //[return: MessageParameter(Name = "Result")]
        //string AssignGroupToCaregiver(string ChattingGroupId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AssignGroupToUser/{ChattingGroupId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string AssignGroupToUser(string ChattingGroupId, string UserId);

        //Added By Pinki on 26th Oct,2017
        #region Group Chat
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllUnAssignedMemberList/{ChattingGroupId}/{UserId}/{OrganisationId}")]
        [return: MessageParameter(Name = "CaregiverList")]
        List<CareGivers> GetAllUnAssignedMemberList(string ChattingGroupId, string UserId, string OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAssignedMemberListGroupWise/{ChattingGroupId}")]
        [return: MessageParameter(Name = "objGroupMemberDetailList")]
        List<ChattingGroupMember> GetAssignedMemberListGroupWise(string ChattingGroupId);

        #endregion

        //Hardik Masalawala 30-10-2017
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetGroupDetailFromGroupName/{GroupName}")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting GetGroupDetailFromGroupName(string GroupName);

        //01-10-2017
        #region Office

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOfficesList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{OrganisationId}")]
        [return: MessageParameter(Name = "OfficeList")]
        OfficesList GetAllOfficesList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAvailableOfficesList/{LogInUserId}/{OrganisationId}")]
        [return: MessageParameter(Name = "lstOffice")]
        List<Office> GetAllAvailableOfficesList(string LogInUserId, string OrganisationId);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddOffice")]
        //[return: MessageParameter(Name = "OfficeList")]
        //string AddOffice(Office Office);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOfficeDetailByOfficeId/{OfficeId}")]
        [return: MessageParameter(Name = "OfficeDetail")]
        Office GetOfficeDetailByOfficeId(string OfficeId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOrganisationOfficeDetailByOfficeId/{OfficeId}/{OrganisationId}")]
        [return: MessageParameter(Name = "OfficeDetail")]
        Office GetOrganisationOfficeDetailByOfficeId(string OfficeId, string OrganisationId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EditOffice")]
        [return: MessageParameter(Name = "Result")]
        string EditOffice(Office Office);



        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteOfficeByOfficeId/{OfficeId}/{UserId}/{OrganisationId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteOfficeByOfficeId(string OfficeId, string UserId, string OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AssignZipcodesToOfficeByOfficeId/{OfficeId}/{AssignedZipcodes}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string AssignZipcodesToOfficeByOfficeId(string OfficeId,string AssignedZipcodes, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAssignZipcodesToOfficeByOfficeId/{OfficeId}")]
        [return: MessageParameter(Name = "Result")]
        string GetAssignZipcodesToOfficeByOfficeId(string OfficeId);
        #endregion
        /// Added By Krunal on 01-11-2017

        ///----------------- For Office Admin
        #region OfficeAdmin

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddAdmin")]
        [return: MessageParameter(Name = "Result")]
        string AddAdmin(Admin Admin);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOfficeAdmin/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{officeId}/{search}/{OrganisationId}/{IsActiveStatus}")]
        [return: MessageParameter(Name = "AdminsList")]
        AdminsList GetAllOfficeAdmin(string pageNo, string recordPerPage, string sortfield, string sortorder, string officeId, string search, string OrganisationId, string IsActiveStatus);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAdminDetailById/{AdminId}")]
        [return: MessageParameter(Name = "AdminDetail")]
        Admin GetAdminDetailById(string AdminId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EditAdmin")]
        [return: MessageParameter(Name = "Result")]
        string EditAdmin(Admin Admin);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteAdmin/{SchedulerId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteAdmin(string SchedulerId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAdminDetailByUserId/{AdminUserId}")]
        [return: MessageParameter(Name = "AdminDetail")]
        Admin GetAdminDetailByUserId(string AdminUserId);

        #endregion
        // Admin Name List for dropdown
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAdminName/{OrganisationId}")]
        [return: MessageParameter(Name = "AdminList")]
        List<Admin> GetAllAdminName(String OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOfficeExceptAssignedOfficeByUserId/{UserId}/{LoginUserId}")]
        [return: MessageParameter(Name = "OfficesList")]
        List<Office> GetAllOfficeExceptAssignedOfficeByUserId(string UserId,string LoginUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AssignOfficeToUser/{OfficeId}/{UserId}/{LoginUserId}")]
        [return: MessageParameter(Name = "Result")]
        string AssignOfficeToUser(string OfficeId, string UserId,string LoginUserId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAssignedOfficeByUserId/{UserId}")]
        [return: MessageParameter(Name = "OfficesList")]
        List<Office> GetAllAssignedOfficeByUserId(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "RemoveSchedulerFromOffice/{OfficeId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string RemoveSchedulerFromOffice(string OfficeId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SaveQBIdOfAdmin/{Email}/{QuickBloxId}")]
        [return: MessageParameter(Name = "Result")]//SaveQBId
        string SaveQBIdOfAdmin(string Email, string QuickBloxId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientChattingGroupByOfficeId/{OfficeId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetPatientChattingGroupByOfficeId(string OfficeId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddGroupDialogId")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting AddGroupDialogId(GroupChat GroupChat);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddOrganisationGroupDialogId")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting AddOrganisationGroupDialogId(GroupChat GroupChat);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllMemberByOffice/{LoginUserId}/{OfficeId}/{OrganisationId}")]
        [return: MessageParameter(Name = "objGroupMemberDetailList")]
        List<ChattingGroupMember> GetAllMemberByOffice(string LoginUserId,string OfficeId, string OrganisationId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddOffice")]
        [return: MessageParameter(Name = "OfficeDetail")]
        Office AddOffice(Office Office);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOfficeGroupDetailByOfficeId/{OfficeId}/{UserId}")]
        [return: MessageParameter(Name = "objDialogDetail")]
        Chatting GetOfficeGroupDetailByOfficeId(string OfficeId,string UserId);

        #region Delete Group Chat
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteGroupChat/{ChattingGroupId}/{DialogId}/{LoginUserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteGroupChat(string ChattingGroupId, string DialogId, string LoginUserId);
        #endregion

        #region Update Group chat Detail
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "UpdateGroupDetail/{ChattingGroupId}/{DialogId}/{GroupName}/{GroupSubject}/{LoginUserID}")]
        [return: MessageParameter(Name = "Result")]//SaveQBId
        string UpdateGroupDetail(string ChattingGroupId, string DialogId, string GroupName, string GroupSubject, string LoginUserID);
        #endregion


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetChatGroupListByTypeIdForUser/{UserId}/{GroupTypeId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetChatGroupListByTypeIdForUser(string UserId,string GroupTypeId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "IsGroupNameAndSubjectExist/{LoginUserId}")]
        [return: MessageParameter(Name = "ResultInBool")]
        bool IsGroupNameAndSubjectExist(Chatting Chatting, string LoginUserId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ExitMemberFromGroupChat/{ChattingGroupId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string ExitMemberFromGroupChat(string ChattingGroupId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AssignGroupAdminToUser/{ChattingGroupId}/{UserId}/{LoginUserId}")]
        [return: MessageParameter(Name = "Result")]
        string AssignGroupAdminToUser(string ChattingGroupId, string UserId,string LoginUserId);



        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOneToOneChatListByUserId/{UserId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetOneToOneChatListByUserId(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteOneToOneChatByUserId/{DialogId}/{UserId}")]
        [return: MessageParameter(Name = "Result")]
        string DeleteOneToOneChatByUserId(string DialogId, string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetChatGroupListByOfficeIdForUser/{UserId}/{GroupTypeId}/{OfficeId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetChatGroupListByOfficeIdForUser(string UserId, string GroupTypeId,string OfficeId);

        #region AssignPermission
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetGroupMemberListWithPermissionAndRole/{id}")]
        [return: MessageParameter(Name = "objGroupMemberDetailList")]
        List<ChattingGroupMember> GetGroupMemberListWithPermissionAndRole(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SetGroupChatMemberPermission/{ChattingGroupId}/{ChattingGroupMemberId}/{Permission}/{LoginUserId}")]
        [return: MessageParameter(Name = "Result")]
        string SetGroupChatMemberPermission(string ChattingGroupId, string ChattingGroupMemberId, string Permission, string LoginUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetGroupChatMemberPermission/{ChattingGroupId}/{LoginUserId}")]
        [return: MessageParameter(Name = "Result")]
        string GetGroupChatMemberPermission(string ChattingGroupId, string LoginUserId);
        #endregion

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddMemberIntoGroup/{ChattingGroupId}/{QuickBloxId}")]
        //[return: MessageParameter(Name = "Result")]
        //string AddMemberIntoGroup(string ChattingGroupId, string QuickBloxId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetUnassignedSchedulerList/{ChattingGroupId}")]
        [return: MessageParameter(Name = "SchedulerList")]
        List<ScheduleInfo> GetUnassignedSchedulerList(string ChattingGroupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAssignedSchedulerListGroupWise/{ChattingGroupId}")]
        [return: MessageParameter(Name = "objGroupMemberDetailList")]
        List<ChattingGroupMember> GetAssignedSchedulerListGroupWise(string ChattingGroupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddMemberIntoGroup/{ChattingGroupId}/{QuickBloxId}/{LoginUserId}")]
        [return: MessageParameter(Name = "Result")]
        string AddMemberIntoGroup(string ChattingGroupId, string QuickBloxId,string LoginUserId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetChatGroupListForDeleteManually/{UserId}/{GroupTypeId}")]
        [return: MessageParameter(Name = "ChattingList")]
        List<Chatting> GetChatGroupListForDeleteManually(string UserId, string GroupTypeId);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientRoomGroupList/{LogInUserId}")]
        //[return: MessageParameter(Name = "ChattingsList")]
        //ChattingsList GetPatientRoomGroupList(PatientChatModel PatientChatModel, string LogInUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPatientRoomGroupList/{LogInUserId}/{OrganisationId}")]
        [return: MessageParameter(Name = "ChattingGroupList")]
        List<PatientChatList> GetPatientRoomGroupList(PatientChatModel PatientChatModel, string LogInUserId, string OrganisationId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOneToOneChatList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{OfficeId}/{OrganisationId}")]
        [return: MessageParameter(Name = "ChattingsList")]
        ChattingsList GetOneToOneChatList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string OfficeId, string OrganisationId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetALLSuperadminList")]
        [return: MessageParameter(Name = "SchedulerList")]
        List<ScheduleInfo> GetALLSuperadminList();


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CheckInOutBySuperAdmin/{PatientRequestId}")]
        [return: MessageParameter(Name = "Result")]
        string CheckInOutBySuperAdmin(string PatientRequestId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAttendanceDetailsList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{OrderId}/{search}/{OrganisationId}")]
        [return: MessageParameter(Name = "ListAttendanceDetails")]
        AttendanceDetailsList GetAttendanceDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OrderId, string search, string OrganisationId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCornaStatsDetailsList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{OrderId}/{search}")]
        [return: MessageParameter(Name = "ListCornaStatsDetails")]
        CornaStatsDetailsList GetCornaStatsDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OrderId, string search);



        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SetNurseCoordinatorAndOfficeManager/{ChattingGroupId}/{NurseCoordinatorId}/{Permission}")]
        [return: MessageParameter(Name = "Result")]
        string SetNurseCoordinatorAndOfficeManager(string ChattingGroupId, string NurseCoordinatorId, string Permission,string CaregiverQBId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllPasevaCustomerSchedulePatientRequest/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{IsAdmin}/{LogInUserId}/{FromDate}/{ToDate}/{FilterStatus}/{FilterOffice}")]
        [return: MessageParameter(Name = "SchedulePatientRequestList")]
        SchedulePatientRequestList GetAllPasevaCustomerSchedulePatientRequest(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterStatus, string FilterOffice);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AGetAttendanceDetailsList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{OrderId}/{search}")]
        [return: MessageParameter(Name = "ListAttendanceDetails")]
        ReportsDetailsList AGetAttendanceDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OrderId, string search);



        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAttendanceportsOfCaregiver/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}/{IsAdmin}/{LogInUserId}/{FromDate}/{ToDate}/{FilterCaregiver}")]
        //[return: MessageParameter(Name = "ListAttendanceDetails")]
        //ReportsDetailsList GetAllAttendanceportsOfCaregiver(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterCaregiver);


        //#region organisation
        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOrganisationsList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        //[return: MessageParameter(Name = "OrganisationList")]
        //OrganisationsList GetAllOrganisationsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddOrganisation")]
        //[return: MessageParameter(Name = "OrganisationDetail")]
        //Organisation AddOrganisation(Organisation Organisation);


        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOrganisationAdminName")]
        //[return: MessageParameter(Name = "OrganisationAdminList")]
        //List<Admin> GetAllOrganisationAdminName();


        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAvailableOrganisationsList/{LogInUserId}")]
        //[return: MessageParameter(Name = "lstOrganisation")]
        //List<Organisation> GetAllAvailableOrganisationsList(string LogInUserId);


        //#region organisation
        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOrganisationsList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        //[return: MessageParameter(Name = "OrganisationList")]
        //OrganisationsList GetAllOrganisationsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddOrganisation")]
        //[return: MessageParameter(Name = "OrganisationDetail")]
        //Organisation AddOrganisation(Organisation Organisation);


        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOrganisationAdminName/{OrganisationId}")]
        //[return: MessageParameter(Name = "OrganisationAdminList")]
        //List<Admin> GetAllOrganisationAdminName(string OrganisationId);


        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAvailableOrganisationsList/{LogInUserId}")]
        //[return: MessageParameter(Name = "lstOrganisation")]
        //List<Organisation> GetAllAvailableOrganisationsList(string LogInUserId);



        //#endregion

        #region organisation
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOrganisationsList/{LogInUserId}/{pageNo}/{recordPerPage}/{sortfield}/{sortorder}/{search}")]
        [return: MessageParameter(Name = "OrganisationList")]
        OrganisationsList GetAllOrganisationsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddOrganisation")]
        [return: MessageParameter(Name = "OrganisationDetail")]
        Organisation AddOrganisation(Organisation Organisation);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EditOrganisation")]
        [return: MessageParameter(Name = "Result")]
        string EditOrganisation(Organisation organisation);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllOrganisationAdminName/{OrganisationId}")]
        [return: MessageParameter(Name = "OrganisationAdminList")]
        List<Admin> GetAllOrganisationAdminName(string OrganisationId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllAvailableOrganisationsList/{LogInUserId}/{OrganisationId}")]
        [return: MessageParameter(Name = "lstOrganisation")]
        List<Organisation> GetAllAvailableOrganisationsList(string LogInUserId,string OrganisationId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOrganisationDetailByOrganisationId/{OrganisationId}")]
        [return: MessageParameter(Name = "OrganisationDetail")]
        Organisation GetOrganisationDetailByOrganisationId(string OrganisationId);



        #endregion

    }
}
