using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF.Class
{
    public class CornaStatsDetails
    {
        public int BodySymptomsId { get; set; }
        public int NurseId { get; set; }
        public string NurseName{ get; set; }
        public string Datetimes { get; set; }
        public string BodyPain { get; set; }
        public string BodyTemperature { get; set;}
        public string Cough { get; set; }
        public string BreathingDifficulty { get; set;}
        public string SoreThroat { get; set;}
        public string ActiveStatus { get; set;}
        public string OfficeName { get; set; }
        public int OfficeId { get; set; }
        public string Username { get; set; }
    }

    [DataContract]
    public class CornaStatsDetailsList
    {
        
            [DataMember]
            public int TotalNumberofRecord { get; set; }

            [DataMember]
            public int FilteredRecord { get; set; }

            [DataMember]
            public List<CornaStatsDetails> VitalCornaStatsDetailsList { get; set; }
        
    }



}