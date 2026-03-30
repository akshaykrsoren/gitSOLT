using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Test1
    { 
        
    }
    [DataContract]
    public class MessageQueue
    {
        string m_Subject;
        string m_Message;
        string m_UserID;
        string m_EmailID;
        string m_MobileNumber;
        string m_IsFileAttachment;
        string m_AttachmentFileName;

        [DataMember]
        public string Subject { get { return m_Subject; } set { m_Subject = value; } }

        [DataMember]
        public string Message { get { return m_Message; } set { m_Message = value; } }

        [DataMember]
        public string UserID { get { return m_UserID; } set { m_UserID = value; } }

        [DataMember]
        public string EmailID { get { return m_EmailID; } set { m_EmailID = value; } }

        [DataMember]
        public string MobileNumber { get { return m_MobileNumber; } set { m_MobileNumber = value; } }

        [DataMember]
        public string IsFileAttachment { get { return m_IsFileAttachment; } set { m_IsFileAttachment = value; } }

        [DataMember]
        public string AttachmentFileName { get { return m_AttachmentFileName; } set { m_AttachmentFileName = value; } }
    }
}