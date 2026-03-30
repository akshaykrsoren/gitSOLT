using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class MarketingsDetail
    {

        private int m_MarketersId { get; set; }
        private string m_MarketersName { get; set; }
        //private string m_MedicalId { get; set; }
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

        private string m_UserId;

        [DataMember]
        public string Office { get; set; }

        [DataMember]
        public string DeviceToken { get; set; }

        [DataMember]
        public string DeviceType { get; set; }

        [DataMember]
        public int ClientRequestId { get; set; }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
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
        public int MarketersId
        {
            get { return m_MarketersId; }
            set { m_MarketersId = value; }
        }

        [DataMember]
        public string MarketersName
        {
            get { return m_MarketersName; }
            set { m_MarketersName = value; }
        }

        [DataMember]
        public double DistanceUnit { get; set; }


        //[DataMember]
        //public string MedicalId
        //{
        //    get { return m_MedicalId; }
        //    set { m_MedicalId = value; }
        //}

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
        public float MaxDistance { get; set; }

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
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }

    }

    [DataContract]
    public class MarketersDetailsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<MarketingsDetail> MarketersList { get; set; }
    }

    public class RootRespone
    {
        [DataMember]
        public int success { get; set; }
        [DataMember]
        public List<MarketingsDetail> data { get; set; }
        [DataMember]
        public string message { get; set; }
    }
}