using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class SupplyFormData
    {

        public string EmployeeName { get; set; }
        public string OfficeName { get; set; }
        public string PatientGroupName { get; set; }
        public List<PdfDataDescription> SupplyFormDetails { get; set; }
        public string UserName { get; set; }
        public string SupplyFormUrl { get; set; }
        public string ProductNames { get; set; }
        public string Quantitys { get; set; }
        public string Descriptions { get; set; }
        public string CreatedDate { get; set; }


    }


    public class PdfDataDescription
    {
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string PatientGroupName { get; set; }
    }



    public class pdfdata
    {
        public string PdfFile { get; set; }
    }

}