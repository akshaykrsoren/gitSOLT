using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class ClientAddRequest
    {

        public ProviderIdentification ProviderIdentification { get; set; }
        public string ClientID { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientQualifier { get; set; }
        public string ClientLastName { get; set; }
        // public string ClientAge { get; set; }   
        
        public string ClientMedicaidID { get; set; }
        public string ClientIdentifier { get; set; }
        public string MissingMedicaidID { get; set; }
        public string SequenceID { get; set; }
        public string ClientCustomID { get; set; }
        public string ClientOtherID { get; set; }
        public string ClientTimezone { get; set; }

        public string ClientBirthDate { get; set; }
        public string ProviderAssentContPlan { get; set; }

          //public ClientPayerInformation[] ClientPayerInformation { get; set; }
         //public ClientAddress[] ClientAddress { get; set; }
        //public ClientPhones[] ClientPhone { get; set; }

        public List<ClientPayerInformation> ClientPayerInformation { get; set; }
        public List<ClientAddress> ClientAddress { get; set; }
        public List<ClientPhones> ClientPhone { get; set; }
    }

    public class ClientPayerInformation
    {
        public string PayerID { get; set; }
        public string PayerProgram { get; set; }
        public string ProcedureCode { get; set; }
        public string ClientPayerID { get; set; }
        public string ClientEligibilityDateBegin { get; set; }
        public string ClientEligibilityDateEnd { get; set; }
        public string ClientStatus { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public string JurisdictionID { get; set; }

    }


    public class ClientAddress
    {
        public string ClientAddressType { get; set; }
        public string ClientAddressIsPrimary { get; set; }
        public string ClientAddressLine1 { get; set; }
        public string ClientAddressLine2 { get; set; }
        public string ClientCounty { get; set; }
        public string ClientCity { get; set; }
        public string ClientState { get; set; }
        public string ClientZip { get; set; }
        public string ClientAddressLongitude { get; set; }
        public string ClientAddressLatitude { get; set; }

    }


    public class ClientPhones
    {
        public string ClientPhoneType { get; set; }
        public string ClientPhone { get; set; }

    }
}