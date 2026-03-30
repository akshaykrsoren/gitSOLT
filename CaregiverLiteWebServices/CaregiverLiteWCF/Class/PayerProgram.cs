using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF.Class
{
    [DataContract]
    public class PayerProgram
    {
      [DataMember]
      public int PayerProgramsID { get; set; }

      [DataMember]
      public string PayerID { get; set; }

        [DataMember]
        public string ProgramId { get; set; }

        [DataMember]
        public string HCPProcedureCode { get; set; }

        [DataMember]
        public string JurisdictionEntitiesCode { get; set; }

       
        
    }
}