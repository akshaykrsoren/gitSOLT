using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class PaymentTransaction
    {
        private int m_DisputeId;
        private int m_AppointmentId;
        private int m_PatientId;
        private int m_NurseId;
        private string m_Currency;
        private decimal m_Amount;
        private string m_ChargeId;
        private string m_SecretKey;
        private string m_InsertUser;
        private string m_Description;

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
        public string Currency
        {
            get { return m_Currency; }
            set { m_Currency = value; }
        }

        [DataMember]
        public decimal Amount
        {
            get { return m_Amount; }
            set { m_Amount = value; }
        }

        [DataMember]
        public string ChargeId
        {
            get { return m_ChargeId; }
            set { m_ChargeId = value; }
        }

        [DataMember]
        public string SecretKey
        {
            get { return m_SecretKey; }
            set { m_SecretKey = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUser; }
            set { m_InsertUser = value; }
        }

        [DataMember]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
    }
}