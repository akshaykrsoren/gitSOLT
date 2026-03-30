using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Disputes
    {
        private int m_DisputeId;
        private int m_AppointmentId;
        private int m_PatientId;
        private int m_NurseId;
        private string m_PatientName;
        private int m_HourRate;
        private string m_Date;
        private string m_FromTime;
        private string m_ToTime;
        private int m_TotalHours;
        private decimal m_TotalAmount;
        private decimal m_PaidAmount;
        private string m_NurseName;
        private string m_Status;
        private string m_InsertUser;
        private string m_DisputeReason;
        private string m_NurseSecretKey;
        private string m_NursePublishableKey;

        [DataMember]
        public int DisputeId
        {
            get { return m_DisputeId; }
            set { m_DisputeId = value; }
        }

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
        public int HourRate
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
        public decimal PaidAmount
        {
            get { return m_PaidAmount; }
            set { m_PaidAmount = value; }
        }

        [DataMember]
        public string NurseName
        {
            get { return m_NurseName; }
            set { m_NurseName = value; }
        }

        [DataMember]
        public string Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUser; }
            set { m_InsertUser = value; }
        }

        [DataMember]
        public string DisputeReason
        {
            get { return m_DisputeReason; }
            set { m_DisputeReason = value; }
        }

        [DataMember]
        public string NurseSecretKey
        {
            get { return m_NurseSecretKey; }
            set { m_NurseSecretKey = value; }
        }

        [DataMember]
        public string NursePublishableKey
        {
            get { return m_NursePublishableKey; }
            set { m_NursePublishableKey = value; }
        }
    }
}