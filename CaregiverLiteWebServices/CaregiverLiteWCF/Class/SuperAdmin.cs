using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class SuperAdmin
    {
        private string m_UserId;
        private string m_Name;
        private string m_Password;
        private string m_UserName;
        private string m_Email;

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
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }
        [DataMember]
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }
    }
    [DataContract]
    public class AdminsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<SuperAdmin> AdminList { get; set; }
    }
  
    
}