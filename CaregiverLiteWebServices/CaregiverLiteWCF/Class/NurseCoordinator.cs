using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class NurseCoordinator
    {
        private int m_NurseCoordinatorId;
        private string m_UserId;
        private string m_Name;
        private string m_FirstName;
        private string m_LastName;
        private string m_Email;
        private string m_UserName;
        private string m_Password;
        private string m_InsertUserId;
        private string m_QBID;
        private int m_OfficeId;
        private string m_OfficeName;
        private bool m_IsAllowForPatientChatRoom;

        private string m_JobTitle;

        private bool m_IsOfficePermission;
        private bool m_IsAllowOneToOneChat;
        private bool m_IsAllowGroupChat;

        private bool m_IsAllowToCreateGroupChat;


        [DataMember]
        public string IsActive { get; set; }

        [DataMember]
        public int OrganisationId { get; set; }

        [DataMember]
        public int NurseCoordinatorId
        {
            get { return m_NurseCoordinatorId; }
            set { m_NurseCoordinatorId = value; }
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
        public string QBID
        {
            get { return m_QBID; }
            set { m_QBID = value; }
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
        [DataMember]
        public bool IsAllowForPatientChatRoom
        {
            get { return m_IsAllowForPatientChatRoom; }
            set { m_IsAllowForPatientChatRoom = value; }

        }

        [DataMember]
        public bool IsAllowOneToOneChat
        {
            get { return m_IsAllowOneToOneChat; }
            set { m_IsAllowOneToOneChat = value; }
        }

        [DataMember]
        public bool IsAllowGroupChat
        {
            get { return m_IsAllowGroupChat; }
            set { m_IsAllowGroupChat = value; }
        }
        [DataMember]
        public string JobTitle
        {
            get { return m_JobTitle; }
            set { m_JobTitle = value; }
        }

        [DataMember]
        public bool IsOfficePermission
        {
            get { return m_IsOfficePermission; }
            set { m_IsOfficePermission = value; }
        }

        [DataMember]
        public bool IsAllowToCreateGroupChat
        {
            get { return m_IsAllowToCreateGroupChat; }
            set { m_IsAllowToCreateGroupChat = value; }
        }



        [DataMember]
        public string OldEmail { get; set; }

    }

    [DataContract]
    public class NurseCoordinatorsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<NurseCoordinator> NurseCoordinatorList { get; set; }
    }
}