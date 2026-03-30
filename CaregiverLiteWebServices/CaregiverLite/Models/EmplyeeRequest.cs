using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class EmplyeeRequest
    {
        public ProviderIdentification ProviderIdentification { get; set; }
        public string EmployeeQualifier { get; set; }
        public string EmployeeIdentifier { get; set; }
        public string EmployeeOtherID { get; set; }
        public string SequenceID { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeSSN { get; set; }
    }


    public class ProviderIdentification
    {
        public string ProviderQualifier { get; set; } = "MedicadID";
        public string ProviderID { get; set; } = "000000077";

    }

}