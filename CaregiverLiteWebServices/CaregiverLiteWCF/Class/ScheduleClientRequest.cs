using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class ScheduleClientRequest
    {
        private int m_ClientRequestId;
        private string m_SchedulerName;
        private string m_ClientName;
        private string m_Address;
        private string m_ZipCode;
        private string m_CRNumber;
        private string m_Description;
        private string m_Date;
        private string m_FromTime;
        private string m_ToTime;
        private bool m_IsCancelled;
        private string m_Status;
        private int m_Office;
        private string m_OfficeName;
        private string m_InsertUserId;
        private string m_InsertDateTime;
        private string m_UpdateUserId;
        private string m_UpdateDateTime;
        private string m_MarketersName;

        [DataMember]
        public string VisitTypeNames { get; set; }

        [DataMember]
        public string Marketer { get; set; }
        private string m_ClientSignature { get; set; }
        private string m_ServiceNames { get; set; }
        private string m_TimezoneId { get; set; }
        private int m_TimezoneOffset { get; set; }
        private string m_TimezonePostfix { get; set; }
        private int m_TotalMarketersNotified;
        private string m_DrivingStartTime;
        private string m_CheckInTime;
        private string m_CheckOutTime;
        private string m_Miles;
        private int m_MarketersId;
        private string m_OfficeAddress;

        public bool m_IsRepeat { get; set; }
        public string m_RepeatEvery { get; set; }
        public string m_RepeatTypeID { get; set; }
        public string m_RepeatDate { get; set; }
        public string m_DayOfWeek { get; set; }
        public string m_DaysOfMonth { get; set; }

        public string m_CheckInLatLong { get; set; }
        public string m_CheckOutLatLong { get; set; }
        public string m_CheckInAddress { get; set; }
        public string m_CheckOutAddress { get; set; }

        public float m_MaxDistance { get; set; }
        public int m_MaxCaregiver { get; set; }

        private string m_Street;
        private string m_City;
        private string m_State;


        [DataMember]
        public int TotalMarketersNotified
        {
            get { return m_TotalMarketersNotified; }
            set { m_TotalMarketersNotified = value; }
        }


        [DataMember]
        public string SchedulerName
        {
            get { return m_SchedulerName; }
            set { m_SchedulerName = value; }
        }


        [DataMember]
        public string MarketersName
        {
            get { return m_MarketersName; }
            set { m_MarketersName = value; }
        }


        [DataMember]
        public int ClientRequestId
        {
            get { return m_ClientRequestId; }
            set { m_ClientRequestId = value; }
        }


        [DataMember]
        public string OfficeAddress
        {
            get { return m_OfficeAddress; }
            set { m_OfficeAddress = value; }
        }


        [DataMember]
        public string ClientName
        {
            get { return m_ClientName; }
            set { m_ClientName = value; }
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
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string ZipCode
        {
            get { return m_ZipCode; }
            set { m_ZipCode = value; }
        }

        [DataMember]
        public string CRNumber
        {
            get { return m_CRNumber; }
            set { m_CRNumber = value; }
        }

        [DataMember]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }


        [DataMember]
        public string Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }

        [DataMember]
        public string FromTime
        {
            get { return m_FromTime; }
            set { m_FromTime = value; }
        }

        [DataMember]
        public string ToTime
        {
            get { return m_ToTime; }
            set { m_ToTime = value; }
        }

        [DataMember]
        public bool IsCancelled
        {
            get { return m_IsCancelled; }
            set { m_IsCancelled = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUserId; }
            set { m_InsertUserId = value; }
        }

        [DataMember]
        public string InsertDateTime
        {
            get { return m_InsertDateTime; }
            set { m_InsertDateTime = value; }
        }

        [DataMember]
        public string UpdateUserId
        {
            get { return m_UpdateUserId; }
            set { m_UpdateUserId = value; }
        }

        [DataMember]
        public string UpdateDateTime
        {
            get { return m_UpdateDateTime; }
            set { m_UpdateDateTime = value; }
        }

        [DataMember]
        public string Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }


        [DataMember]
        public int Office
        {
            get { return m_Office; }
            set { m_Office = value; }
        }


        [DataMember]
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }


        [DataMember]
        public string ClientSignature
        {
            get { return m_ClientSignature; }
            set { m_ClientSignature = value; }
        }


        [DataMember]
        public string ServiceNames
        {
            get { return m_ServiceNames; }
            set { m_ServiceNames = value; }
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
        public string DrivingStartTime
        {
            get { return m_DrivingStartTime; }
            set { m_DrivingStartTime = value; }
        }


        [DataMember]
        public string CheckInTime
        {
            get { return m_CheckInTime; }
            set { m_CheckInTime = value; }
        }


        [DataMember]
        public string CheckOutTime
        {
            get { return m_CheckOutTime; }
            set { m_CheckOutTime = value; }
        }


        [DataMember]
        public string Miles
        {
            get { return m_Miles; }
            set { m_Miles = value; }
        }


        [DataMember]
        public int MarketersId
        {
            get { return m_MarketersId; }
            set { m_MarketersId = value; }
        }

        [DataMember]
        public bool IsRepeat
        {
            get { return m_IsRepeat; }
            set { m_IsRepeat = value; }
        }


        [DataMember]
        public string RepeatEvery
        {
            get { return m_RepeatEvery; }
            set { m_RepeatEvery = value; }
        }


        [DataMember]
        public string RepeatTypeID
        {
            get { return m_RepeatTypeID; }
            set { m_RepeatTypeID = value; }
        }


        [DataMember]
        public string RepeatDate
        {
            get { return m_RepeatDate; }
            set { m_RepeatDate = value; }
        }


        [DataMember]
        public string DayOfWeek
        {
            get { return m_DayOfWeek; }
            set { m_DayOfWeek = value; }
        }


        [DataMember]
        public string DaysOfMonth
        {
            get { return m_DaysOfMonth; }
            set { m_DaysOfMonth = value; }
        }


        [DataMember]
        public string CheckInLatLong
        {
            get { return m_CheckInLatLong; }
            set { m_CheckInLatLong = value; }
        }


        [DataMember]
        public string CheckOutLatLong
        {
            get { return m_CheckOutLatLong; }
            set { m_CheckOutLatLong = value; }
        }


        [DataMember]
        public string CheckInAddress
        {
            get { return m_CheckInAddress; }
            set { m_CheckInAddress = value; }
        }


        [DataMember]
        public string CheckOutAddress
        {
            get { return m_CheckOutAddress; }
            set { m_CheckOutAddress = value; }
        }

        //public float m_MaxDistance { get; set; }
        //public int m_MaxCaregiver { get; set; }
        [DataMember]
        public float MaxDistance
        {
            get { return m_MaxDistance; }
            set { m_MaxDistance = value; }
        }
        [DataMember]
        public int MaxCaregiver
        {
            get { return m_MaxCaregiver; }
            set { m_MaxCaregiver = value; }
        }

    }
       [DataContract]
        public class ScheduleClientRequestList
        {
            [DataMember]
            public int TotalNumberofRecord { get; set; }

            [DataMember]
            public int FilteredRecord { get; set; }

            [DataMember]
            public List<ScheduleClientRequest> ScheduleClientRequestsList { get; set; }

            [DataMember]
            public Decimal TotalAmount { get; set; }
        }
    

}