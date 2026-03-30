using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class Permission
    {
        private string m_PermissionId;
        private string m_PermissionDescription;

        [DataMember]
        public string PermissionId
        {
            get { return m_PermissionId; }
            set { m_PermissionId = value; }
        }
        [DataMember]
        public string PermissionDescription
        {
            get { return m_PermissionDescription; }
            set { m_PermissionDescription = value; }
        }
       
    }

}