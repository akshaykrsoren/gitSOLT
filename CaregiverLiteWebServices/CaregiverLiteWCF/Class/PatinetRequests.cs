using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class PatientRequests
    {
        private int m_AppointmentId;
        private int m_PatientId;
        private int m_ServiceId;
        private int m_NurseId;
        private string m_PatientName;
        private string m_FirstName;
        private string m_LastName;

        private string m_PatientEmail;
        private string m_PatientPhone;
        private string m_PatientAddress;
        private string m_ServiceName;
        private decimal m_HourRate;
        private string m_Date;
        private string m_FromTime;
        private string m_ToTime;
        private int m_TotalHours;
        private decimal m_TotalAmount;
        private string m_NurseName;
        private string m_NurseEmail;
        private string m_NursePhone;
        private string m_RescheduleStatus;
        private string m_InsertUser;
        private string m_NurseDeviceType;
        private string m_NurseDeviceToken;
        private int m_ServiceRadius;
        private int m_FromHourRate;
        private int m_ToHourRate;
        private bool m_IsAvailable;
        private bool m_IsBusy;
        private bool m_IsComplete;
        private string m_PatientZipCode;
        private string m_RescheduleType;
        private decimal m_PaidAmount;
        private int m_TravelBufferMinutes;
        private decimal m_DuePayment;
        private string m_PatientUserId;
        private string m_PatientDeviceType;
        private string m_PatientDeviceToken;
        private string m_NurseUserId;
        private decimal m_ChargeToPatientRate;
        private decimal m_CareGiverCharge;
        private decimal m_PayableAmount;
        private decimal m_StripeFee;

        [DataMember]
        public int AppointmentId
        {
            get { return m_AppointmentId; }
            set { m_AppointmentId = value; }
        }

        [DataMember]
        public int PatientId
        {
            get { return m_PatientId; }
            set { m_PatientId = value; }
        }

        [DataMember]
        public int ServiceId
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public string PatientName
        {
            get { return m_PatientName; }
            set { m_PatientName = value; }
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
        public string PatientEmail
        {
            get { return m_PatientEmail; }
            set { m_PatientEmail = value; }
        }

        [DataMember]
        public string PatientPhone
        {
            get { return m_PatientPhone; }
            set { m_PatientPhone = value; }
        }

        [DataMember]
        public string PatientAddress
        {
            get { return m_PatientAddress; }
            set { m_PatientAddress = value; }
        }

        [DataMember]
        public string ServiceName
        {
            get { return m_ServiceName; }
            set { m_ServiceName = value; }
        }

        [DataMember]
        public decimal HourRate
        {
            get { return m_HourRate; }
            set { m_HourRate = value; }
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
        public int TotalHours
        {
            get { return m_TotalHours; }
            set { m_TotalHours = value; }
        }

        [DataMember]
        public decimal TotalAmount
        {
            get { return m_TotalAmount; }
            set { m_TotalAmount = value; }
        }

        [DataMember]
        public string NurseName
        {
            get { return m_NurseName; }
            set { m_NurseName = value; }
        }

        [DataMember]
        public string NurseEmail
        {
            get { return m_NurseEmail; }
            set { m_NurseEmail = value; }
        }

        [DataMember]
        public string NursePhone
        {
            get { return m_NursePhone; }
            set { m_NursePhone = value; }
        }

        [DataMember]
        public string RescheduleStatus
        {
            get { return m_RescheduleStatus; }
            set { m_RescheduleStatus = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUser; }
            set { m_InsertUser = value; }
        }

        [DataMember]
        public string NurseDeviceType
        {
            get { return m_NurseDeviceType; }
            set { m_NurseDeviceType = value; }
        }

        [DataMember]
        public string NurseDeviceToken
        {
            get { return m_NurseDeviceToken; }
            set { m_NurseDeviceToken = value; }
        }

        [DataMember]
        public int ServiceRadius
        {
            get { return m_ServiceRadius; }
            set { m_ServiceRadius = value; }
        }

        [DataMember]
        public int FromHourRate
        {
            get { return m_FromHourRate; }
            set { m_FromHourRate = value; }
        }

        [DataMember]
        public int ToHourRate
        {
            get { return m_ToHourRate; }
            set { m_ToHourRate = value; }
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
        public bool IsComplete
        {
            get { return m_IsComplete; }
            set { m_IsComplete = value; }
        }

        [DataMember]
        public string PatientZipCode
        {
            get { return m_PatientZipCode; }
            set { m_PatientZipCode = value; }
        }

        [DataMember]
        public string RescheduleType
        {
            get { return m_RescheduleType; }
            set { m_RescheduleType = value; }
        }

        [DataMember]
        public decimal PaidAmount
        {
            get { return m_PaidAmount; }
            set { m_PaidAmount = value; }
        }
        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public int TravelBufferMinutes
        {
            get { return m_TravelBufferMinutes; }
            set { m_TravelBufferMinutes = value; }
        }

        [DataMember]
        public decimal DuePayment
        {
            get { return m_DuePayment; }
            set { m_DuePayment = value; }
        }

        [DataMember]
        public string PatientUserId
        {
            get { return m_PatientUserId; }
            set { m_PatientUserId = value; }
        }

        [DataMember]
        public string PatientDeviceToken
        {
            get { return m_PatientDeviceToken; }
            set { m_PatientDeviceToken = value; }
        }

        [DataMember]
        public string PatientDeviceType
        {
            get { return m_PatientDeviceType; }
            set { m_PatientDeviceType = value; }
        }

        [DataMember]
        public string NurseUserId
        {
            get { return m_NurseUserId; }
            set { m_NurseUserId = value; }
        }

        [DataMember]
        public decimal ChargeToPatientRate
        {
            get { return m_ChargeToPatientRate; }
            set { m_ChargeToPatientRate = value; }
        }

        [DataMember]
        public decimal CareGiverCharge
        {
            get { return m_CareGiverCharge; }
            set { m_CareGiverCharge = value; }
        }

        [DataMember]
        public decimal PayableAmount
        {
            get { return m_PayableAmount; }
            set { m_PayableAmount = value; }
        }

        [DataMember]
        public decimal StripeFee
        {
            get { return m_StripeFee; }
            set { m_StripeFee = value; }
        }
    }

    [DataContract]
    public class PatientRequestList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<PatientRequests> PatientRequestsList { get; set; }

        [DataMember]
        public Decimal TotalAmount { get; set; }

    }
}