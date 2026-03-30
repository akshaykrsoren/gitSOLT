using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF
{
    public class PatientsDetail
    {
        private int m_PatientId { get; set; }
        private string m_PatientName { get; set; }

        private string m_FirstName { get; set; }
        private string m_LastName { get; set; }

        private string m_MedicalId { get; set; }
        private string m_Address { get; set; }
        private string m_PhoneNo { get; set; }
        private string m_Email { get; set; }
        private string m_Latitude { get; set; }
        private string m_Longitude { get; set; }
        private string m_ZipCode { get; set; }
        private string m_InsertUserId { get; set; }
        private string m_UpdateUserId { get; set; }
        private string m_TimezoneId { get; set; }
        private string m_TimezoneOffset { get; set; }
        private string m_TimezonePostfix { get; set; }

        private string m_Street { get; set; }
        private string m_City { get; set; }
        private string m_State { get; set; }
        private string m_PrimaryMD { get; set; }
        private int m_OfficeId;
        private string m_OfficeName;

        private string m_Password;
        private string m_UserName;

        [DataMember]
        public string DateOfBirth { get; set; }

        [DataMember]
        public string PayerId { get; set; }
        [DataMember]
        public string PayerProgram { get; set; }
        [DataMember]
        public string ClientPayerId { get; set; }
        [DataMember]
        public string ProcedureCode { get; set; }

        [DataMember]
        public int OrganisationId { get; set; }

        [DataMember]
        public string JurisdictionCode { get; set; }

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
        public int PatientId
        {
            get { return m_PatientId; }
            set { m_PatientId = value; }
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
        public string MedicalId
        {
            get { return m_MedicalId; }
            set { m_MedicalId = value; }
        }

        [DataMember]
        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        [DataMember]
        public string PhoneNo
        {
            get { return m_PhoneNo; }
            set { m_PhoneNo = value; }
        }

        [DataMember]
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
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
        public string ZipCode
        {
            get { return m_ZipCode; }
            set { m_ZipCode = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUserId; }
            set { m_InsertUserId = value; }
        }

        [DataMember]
        public string UpdateUserId
        {
            get { return m_UpdateUserId; }
            set { m_UpdateUserId = value; }
        }

        [DataMember]
        public string TimezoneId
        {
            get { return m_TimezoneId; }
            set { m_TimezoneId = value; }
        }

        [DataMember]
        public string TimezoneOffset
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
        public string PrimaryMD
        {
            get { return m_PrimaryMD; }
            set { m_PrimaryMD = value; }
        }

        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }

        [DataMember]
        public string IsActive
        {

            get;set;
        }

        [DataMember]
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }


        //[DataMember]
        //public bool IsActive { get; set; }
        [DataMember]
        public string ReferredBy { get; set; }
        [DataMember]
        public string primarymd { get; set; }
        [DataMember]
        public string jurisdictioncode { get; set; }
        [DataMember]
        public string payerprogram { get; set; }
        [DataMember]
        public string procedurecode { get; set; }
    }
    [DataContract]
    public class PatientDetailsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<PatientsDetail> PatientList { get; set; }
    }
}