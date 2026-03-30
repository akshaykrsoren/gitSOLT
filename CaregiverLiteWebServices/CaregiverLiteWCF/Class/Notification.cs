using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Notification
    {
        private int m_NotificationId;
        private int m_AppointmentId;
        private int m_NurseId;
        private int m_PatientId;
        private string m_Type;
        private string m_Title;
        private string m_Message;
        private string m_UserList;
        private string m_InsertUserId;
        private IosResponse m_IosResponse;
        private AndroidResponse m_AndroidResponse;

        [DataMember]
        public int NotificationId
        {
            get { return m_NotificationId; }
            set { m_NotificationId = value; }
        }

        [DataMember]
        public int AppointmentId
        {
            get { return m_AppointmentId; }
            set { m_AppointmentId = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public int PatientId
        {
            get { return m_PatientId; }
            set { m_PatientId = value; }
        }

        [DataMember]
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        [DataMember]
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        [DataMember]
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUserId; }
            set { m_InsertUserId = value; }
        }

        [DataMember]
        public IosResponse IosResponse
        {
            get { return m_IosResponse; }
            set { m_IosResponse = value; }
        }

        [DataMember]
        public AndroidResponse AndroidResponse
        {
            get { return m_AndroidResponse; }
            set { m_AndroidResponse = value; }
        }

        [DataMember]
        public string UserList
        {
            get { return m_UserList; }
            set { m_UserList = value; }
        }
    }

    [DataContract]
    public class IosResponse
    {
        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Json { get; set; }
    }

    [DataContract]
    public class AndroidResponse
    {
        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Json { get; set; }
    }
}
