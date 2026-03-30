using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Patients
    {
        private int m_PatientId;
        private string m_UserId;
        private string m_Name;

        private string m_FirstName;
        private string m_LastName;
        private string m_Email;
        private string m_Phone;
        private string m_Address;
        private string m_ZipCode;
        private string m_UserName;
        private string m_Password;
        private string m_InsertUserId;
        private string m_Latitude;
        private string m_Longitude;
        private string m_InsertDateTime;

        [DataMember]
        public int PatientId
        {
            get { return m_PatientId; }
            set { m_PatientId = value; }
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
        public string ZipCode
        {
            get { return m_ZipCode; }
            set { m_ZipCode = value; }
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
        public string InsertDateTime
        {
            get { return m_InsertDateTime; }
            set { m_InsertDateTime = value; }
        }
    }

    [DataContract]
    public class PatientsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<Patients> PatientList { get; set; }
    }

    
}