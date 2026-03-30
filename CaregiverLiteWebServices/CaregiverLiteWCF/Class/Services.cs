using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Services
    {
        private int m_ServiceId;
        private string m_SeriveName;
        private string m_UserId;

        [DataMember]
        public int ServiceId 
        { 
            get { return m_ServiceId; }
            set { m_ServiceId = value; } 
        }

        [DataMember]
        public string ServiceName
        {
            get { return m_SeriveName; }
            set { m_SeriveName = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public int OrganisationId { get; set; }

        [DataMember]
        public string Description { get;   set; }
    }

    [DataContract]
    public class VisitType
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int VisitTypeId { get; set; }

        [DataMember]
        public string VisitTypeName { get; set; }

    }

    //[DataContract]
    //public class VisitTypeViewModel
    //{
    //    [DataMember]
    //    public List<VisitType> VisitTypeList { get; set; }

    //    [DataMember]
    //    public VisitType VisitsType { get; set; }

    //    [DataMember]
    //    public string ReturnUrl { get; set; }

    //    //[DataMember]
    //    //public string VisitTypeNames { get; set; }
    //}

}