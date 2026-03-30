using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Scheduler
    {
        private int m_SchedulerId;
        private string m_UserId;
        private string m_Name;
        private string m_FirstName;
        private string m_LastName;
        private string m_Email;
        private string m_UserName;
        private string m_Password;
        private string m_InsertUserId;
        private string m_QuickBloxId;

        private string m_OfficeName;

        private string m_OfficeIds;

      

        [DataMember]
        public int SchedulerId
        {
            get { return m_SchedulerId; }
            set { m_SchedulerId = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string IsActive { get; set; }

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
        public string QuickBloxId
        {
            get { return m_QuickBloxId; }
            set { m_QuickBloxId = value; }
        }

        [DataMember]
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }

        [DataMember]
        public string OfficeIds
        {
            get { return m_OfficeIds; }
            set { m_OfficeIds = value; }
        }

        [DataMember]
        public int OrganisationId { get; set; }

        [DataMember]
        public string OldEmail { get; set; }

    }

    [DataContract]
    public class SchedulersList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<Scheduler> SchedulerList { get; set; }
    }
}