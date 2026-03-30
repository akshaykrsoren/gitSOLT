using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Office
    {
        private int m_OfficeId;
        private string m_OfficeName;
        private string m_AdminName;
        private string m_AdminUserId;
        private string m_Street;
        private string m_City;
        private string m_State;
        private string m_ZipCode;

        private string m_Latitude;
        private string m_Longitude;
        private string m_InsertUserId;
        private string m_TimezoneId;
        private int m_TimezoneOffset;
        private string m_TimezonePostfix;
        public bool m_IsDeleted;
        public string m_UpdateUserId;
        public string m_AssignedZipcodes;

        public string m_AdminEmail;

        public string m_AdminQuickBloxId;

        public string m_Result;


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

        [DataMember]
        public string AdminName
        {
            get { return m_AdminName; }
            set { m_AdminName = value; }
        }



        [DataMember]
        public string AdminUserId
        {
            get { return m_AdminUserId; }
            set { m_AdminUserId = value; }
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
        public string InsertUserId
        {
            get { return m_InsertUserId; }
            set { m_InsertUserId = value; }
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
        public bool IsDeleted
        {
            get { return m_IsDeleted; }
            set { m_IsDeleted = value; }
        }
        [DataMember]
        public string UpdateUserId
        {
            get { return m_UpdateUserId; }
            set { m_UpdateUserId = value; }
        }
        [DataMember]
        public string AssignedZipcodes
        {
            get { return m_AssignedZipcodes; }
            set { m_AssignedZipcodes = value; }
        }

        [DataMember]
        public int OrganisationId { get; set; }

        [DataMember]
        public string AdminEmail
        {
            get { return m_AdminEmail; }
            set { m_AdminEmail = value; }
        }

        [DataMember]
        public string AdminQuickBloxId
        {
            get { return m_AdminQuickBloxId; }
            set { m_AdminQuickBloxId = value; }
        }


        [DataMember]
        public string Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }

    }



    [DataContract]
    public class Organisation
    {
        [DataMember]
        public int OrganisationId { get; set; }
        [DataMember]
        public int OfficeId { get; set; }

        [DataMember]
        public string OfficeName { get; set; }
        [DataMember]
        public string InsertDateTime { get; set; }

        [DataMember]
        public string OrganisationName { get; set; }
        [DataMember]
        public string OrganisationAdminName { get; set; }
        [DataMember]
        public string OrganisationAdminUserId { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string InsertUserId { get; set; }
        [DataMember]
        public string TimezoneId { get; set; }
        [DataMember]
        public int TimezoneOffset { get; set; }
        [DataMember]
        public string TimezonePostfix { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string UpdateUserId { get; set; }
        [DataMember]
        public string AssignedZipcodes { get; set; }

        [DataMember]
        public string AdminEmail { get; set; }

        [DataMember]
        public string AdminQuickBloxId { get; set; }

        [DataMember]
        public string Result { get; set; }

    }



        [DataContract]
    public class OfficesList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<Office> OfficeList { get; set; }
    }



    [DataContract]
    public class OrganisationsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<Organisation> OrganisationList { get; set; }
    }

}