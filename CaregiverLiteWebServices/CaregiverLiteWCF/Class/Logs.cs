using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
	public class Logs
	{
        string m_PageName;
        [DataMember]
        public string PageName
        {
            get { return m_PageName; }
            set { m_PageName = value; }
        }

        string m_Message;
        [DataMember]
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        string m_UserID;
        [DataMember]
        public string UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }
	}
}