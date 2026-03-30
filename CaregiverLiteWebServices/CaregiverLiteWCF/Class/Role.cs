using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF.Class
{
     [DataContract]
    public class Role
    {
         [DataMember]
        public string RoleId { get; set; }
           [DataMember]
        public string RoleName { get; set; }
           [DataMember]
        public string RoleDescription { get; set; }
           [DataMember]
        public virtual ICollection<Permission> PERMISSIONS { get; set; }
    }
}