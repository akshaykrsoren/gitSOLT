using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class GenerateReport
    {
        private string m_FromDate;
        private string m_ToDate;

        [DataMember]
        public string FromDate
        {
            get { return m_FromDate; }
            set { m_FromDate = value; }
        }

        [DataMember]
        public string ToDate
        {
            get { return m_ToDate; }
            set { m_ToDate = value; }
        }



    }
    //[DataContract]
    //public class GenerateReportList
    //{
    //    [DataMember]
    //    public List<GenerateReport> GenerateReportList { get; set; }
    //}

}