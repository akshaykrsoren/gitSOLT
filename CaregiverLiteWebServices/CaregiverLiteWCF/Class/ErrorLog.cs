using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class ErrorLog
    {
        [DataMember]
        public string Methodname { get; set; }

        [DataMember]
        public string Pagename { get; set; }


        [DataMember]
        public string Errormessage { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public string UserID { get; set; }
    }
}