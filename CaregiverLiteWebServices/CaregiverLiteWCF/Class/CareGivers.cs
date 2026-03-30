using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class CareGivers
    {
        private int m_NurseId;
        private string m_UserId;
        private string m_Name;

        private string m_FirstName;

        private string m_LastName;

        private string m_Email;
        private string m_ServiceId;
        private string m_ServiceName;
        private decimal m_HourlyRate;
        private decimal m_DistanceFromLocation;
        private string m_Phone;
        private string m_Address;
        private string m_Street;
        private string m_City;
        private string m_State;
        private string m_ZipCode;
        private string m_ProfileImage;
        private string m_ProfileVideo;
        private string m_ProfileVideoThumbnil;
        private string m_Certificate;
        private bool m_IsAvailable;
        private bool m_IsBusy;
        private bool m_IsApprove;
        private string m_Latitude;
        private string m_Longitude;
        private string m_DistanceUnit;
        private string m_UserName;
        private string m_Password;
        private string m_InsertUserId;
        private string m_NewPassword;
        private string m_Education;
        private bool m_CanAdminEdit;
        private HoursRate m_HoursRate;
        private decimal m_ChargeToPatient;
        private string m_DeviceToken;
        private string m_DeviceType;
        private int m_PatientRequestId;
        private string m_Office;
        private string m_TimezoneId { get; set; }
        private int m_TimezoneOffset { get; set; }
        private string m_TimezonePostfix { get; set; }
        private List<string> m_CertificateList;
        private string m_CurrentLatitude;
        private string m_CurrentLongitude;
        private string m_DialogId;
        private string m_QuickBloxId;
        private string m_Msg;
        private string m_ChattingType;
        private string m_Permission;
        //  private string m_FilterDate;
		private string m_InsertedOn;
		private string m_IsNurseBusy;
        private int m_BadgeCount;

        private int m_OfficeId;

        private bool m_IsOfficeGroup;
        private bool m_IsGroupAdmin;
        private int m_GroupId;
        private string m_GroupSubject;

        private bool m_IsOfficePermission;
        // OldOfficeChatGroupId

        // New added key for chatting permission 03-Jan-2018 By Krunal
        private bool m_IsAllowOneToOneChat;
        private bool m_IsAllowPatientChatRoom;
        private bool m_IsAllowGroupChat;

        private bool m_IsAllowToCreateGroupChat;

        private string m_PayrollId;
        private string m_NurseFullName;


        [DataMember]
       public string PayrollId
       {
            get { return m_PayrollId; }
            set { m_PayrollId = value; }
       }

        [DataMember]
        public string NurseFullName
        {
            get { return m_NurseFullName; }
            set { m_NurseFullName = value; }
        }

        //[DataMember]
        //public int OrganisationId { get; set; }

        [DataMember]
        public int PatientRequestId
        {
            get { return m_PatientRequestId; }
            set { m_PatientRequestId = value; }
        }

        [DataMember]
        public string IsActive { get; set; }

        [DataMember]
        public string DeviceToken
        {
            get { return m_DeviceToken; }
            set { m_DeviceToken = value; }
        }

        [DataMember]
        public string DeviceType
        {
            get { return m_DeviceType; }
            set { m_DeviceType = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [DataMember]
        public string FirstName
        {
            get { return m_FirstName; }
            set { m_FirstName = value; }
        }

        [DataMember]
        public string LastName
        {
            get { return m_LastName; }
            set { m_LastName = value; }
        }

        [DataMember]
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }

        [DataMember]
        public string ServiceId
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; }
        }

        [DataMember]
        public string ServiceName
        {
            get { return m_ServiceName; }
            set { m_ServiceName = value; }
        }
        [DataMember]
        public string Office
        {
            get { return m_Office; }
            set { m_Office = value; }
        }

        [DataMember]
        public decimal HourlyRate
        {
            get { return m_HourlyRate; }
            set { m_HourlyRate = value; }
        }
        [DataMember]
        public HoursRate HoursRate
        {
            get { return m_HoursRate; }
            set { m_HoursRate = value; }
        }

        [DataMember]
        public decimal DistanceFromLocation
        {
            get { return m_DistanceFromLocation; }
            set { m_DistanceFromLocation = value; }
        }

        [DataMember]
        public string Phone
        {
            get { return m_Phone; }
            set { m_Phone = value; }
        }

        [DataMember]
        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }
        [DataMember]
        public string Street
        {
            get { return m_Street; }
            set { m_Street = value; }
        }

        [DataMember]
        public string City
        {
            get { return m_City; }
            set { m_City = value; }
        }

        [DataMember]
        public string State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        [DataMember]
        public string ZipCode
        {
            get { return m_ZipCode; }
            set { m_ZipCode = value; }
        }

        [DataMember]
        public string ProfileImage
        {
            get { return m_ProfileImage; }
            set { m_ProfileImage = value; }
        }

        [DataMember]
        public string ProfileVideo
        {
            get { return m_ProfileVideo; }
            set { m_ProfileVideo = value; }
        }

        [DataMember]
        public string ProfileVideoThumbnil
        {
            get { return m_ProfileVideoThumbnil; }
            set { m_ProfileVideoThumbnil = value; }
        }

        [DataMember]
        public string Certificate
        {
            get { return m_Certificate; }
            set { m_Certificate = value; }
        }

        [DataMember]
        public List<string> CertificateList
        {
            get { return m_CertificateList; }
            set { m_CertificateList = value; }
        }

        [DataMember]
        public bool IsAvailable
        {
            get { return m_IsAvailable; }
            set { m_IsAvailable = value; }
        }


        [DataMember]
        public bool IsBusy
        {
            get { return m_IsBusy; }
            set { m_IsBusy = value; }
        }


        [DataMember]
        public bool IsApprove
        {
            get { return m_IsApprove; }
            set { m_IsApprove = value; }
        }


        [DataMember]
        public string Latitude
        {
            get { return m_Latitude; }
            set { m_Latitude = value; }
        }

        [DataMember]
        public string Longitude
        {
            get { return m_Longitude; }
            set { m_Longitude = value; }
        }

        [DataMember]
        public string DistanceUnit
        {
            get { return m_DistanceUnit; }
            set { m_DistanceUnit = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        [DataMember]
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUserId; }
            set { m_InsertUserId = value; }
        }

        [DataMember]
        public string NewPassword
        {
            get { return m_NewPassword; }
            set { m_NewPassword = value; }
        }

        [DataMember]
        public string NurseCertificateId { get; set; }

        [DataMember]
        public string IsCertificateApproved { get; set; }

        [DataMember]
        public string Education { get; set; }

        [DataMember]
        public bool CanAdminEdit
        {
            get { return m_CanAdminEdit; }
            set { m_CanAdminEdit = value; }
        }

        [DataMember]
        public decimal ChargeToPatient
        {
            get { return m_ChargeToPatient; }
            set { m_ChargeToPatient = value; }
        }

        [DataMember]
        public string TimezoneId
        {
            get { return m_TimezoneId; }
            set { m_TimezoneId = value; }
        }

        [DataMember]
        public int TimezoneOffset
        {
            get { return m_TimezoneOffset; }
            set { m_TimezoneOffset = value; }
        }

        [DataMember]
        public string TimezonePostfix
        {
            get { return m_TimezonePostfix; }
            set { m_TimezonePostfix = value; }
        }

        [DataMember]
        public string CurrentLatitude
        {
            get { return m_CurrentLatitude; }
            set { m_CurrentLatitude = value; }
        }

        [DataMember]
        public string CurrentLongitude
        {
            get { return m_CurrentLongitude; }
            set { m_CurrentLongitude = value; }
        }

        [DataMember]
        public string DialogId
        {
            get { return m_DialogId; }
            set { m_DialogId = value; }
        }

        [DataMember]
        public string QuickBloxId
        {
            get { return m_QuickBloxId; }
            set { m_QuickBloxId = value; }
        }       
        
        [DataMember]
        public string Msg
        {
            get { return m_Msg; }
            set { m_Msg = value; }
        }
        [DataMember]
        public string ChattingType
        {
            get { return m_ChattingType; }
            set { m_ChattingType = value; }
        }

        //[DataMember]
        //public string FilterDate
        //{
        //    get { return m_FilterDate; }
        //    set { m_FilterDate = value; }
        //}

        [DataMember]
        public string Permission
        {
            get { return m_Permission; }
            set { m_Permission = value; }
        }

        [DataMember]
        public int BadgeCount
        {
            get { return m_BadgeCount; }
            set { m_BadgeCount = value; }
        }

	        [DataMember]
	        public string InsertedOn
	        {
	            get { return m_InsertedOn; }
	            set { m_InsertedOn = value; }
	        }

        [DataMember]
        public string IsNurseBusy
        {
            get { return m_IsNurseBusy; }
            set { m_IsNurseBusy = value; }
        }

        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }

        [DataMember]
        public bool IsOfficePermission
        {
            get { return m_IsOfficePermission; }
            set { m_IsOfficePermission = value; }
        }

        [DataMember]
        public int OrganisationId { get; set; }

        public CaregiverLiteWCF.HoursRate ChargeToPatientHoursRateWithoutStripe { get; set; }

        public CaregiverLiteWCF.HoursRate ChargeToPatientHoursRate { get; set; }

        [DataMember]
        public bool IsOfficeGroup
        {
            get { return m_IsOfficeGroup; }
            set { m_IsOfficeGroup = value; }
        }

        [DataMember]
        public bool IsGroupAdmin
        {
            get { return m_IsGroupAdmin; }
            set { m_IsGroupAdmin = value; }
        }

        [DataMember]
        public int GroupId
        {
            get { return m_GroupId; }
            set { m_GroupId = value; }
        }

        [DataMember]
        public string GroupSubject
        {
            get { return m_GroupSubject; }
            set { m_GroupSubject = value; }
        }


        [DataMember]
        public bool IsAllowOneToOneChat
        {
            get { return m_IsAllowOneToOneChat; }
            set { m_IsAllowOneToOneChat = value; }
        }

        [DataMember]
        public bool IsAllowPatientChatRoom
        {
            get { return m_IsAllowPatientChatRoom; }
            set { m_IsAllowPatientChatRoom = value; }
        }

        [DataMember]
        public bool IsAllowGroupChat
        {
            get { return m_IsAllowGroupChat; }
            set { m_IsAllowGroupChat = value; }
        }

        [DataMember]
        public bool IsAllowToCreateGroupChat
        {
            get { return m_IsAllowToCreateGroupChat; }
            set { m_IsAllowToCreateGroupChat = value; }
        }


        [DataMember]
        public string OldEmail { get; set; }

        //[DataMember]
        //public bool IsActive { get; set; }
    }

    [DataContract]
    public class CareGiversList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<CareGivers> CareGiverList { get; set; }
    }

    [DataContract]
    public class HoursRate
    {
        decimal m_WeekDayRate;
        decimal m_WeekNightRate;
        decimal m_WeekEndDayRate;
        decimal m_WeekEndNightRate;
        decimal m_HolidayRate;

        [DataMember]
        public decimal WeekDayRate
        {
            get { return m_WeekDayRate; }
            set { m_WeekDayRate = value; }
        }

        [DataMember]
        public decimal WeekNightRate
        {
            get { return m_WeekNightRate; }
            set { m_WeekNightRate = value; }
        }

        [DataMember]
        public decimal WeekEndDayRate
        {
            get { return m_WeekEndDayRate; }
            set { m_WeekEndDayRate = value; }
        }
        [DataMember]
        public decimal WeekEndNightRate
        {
            get { return m_WeekEndNightRate; }
            set { m_WeekEndNightRate = value; }
        }

        [DataMember]
        public decimal HolidayRate
        {
            get { return m_HolidayRate; }
            set { m_HolidayRate = value; }
        }
    }


    [DataContract]
    public class GetDatesList
    {

        [DataMember]
        public string ListDate { get; set; }

    }

}