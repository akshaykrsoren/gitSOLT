using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class InTakePatientDetails
    {
        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgPatient")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Please Enter Patient Name")]
        [DisplayName("PatientName")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter PhoneNo")]
        [DisplayName("PhoneNo")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter ZipCode")]
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        [DisplayName("City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [DisplayName("State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Select Office")]
       // [DisplayName("Office")]
        public int OfficeId { get; set; }
        public int OrganisationId { get; set; }

        [DisplayName("PayerId")]
        public string PayerId { get; set; }
        [Required(ErrorMessage = "Please Enter Referred Doctor Name")]
        [DisplayName("Referred By")]
        public string ReferredBy { get; set; }
        public string primarymd { get; set; }
        public string jurisdictioncode { get; set; }
        public string payerprogram { get; set; }
        public string procedurecode { get; set; }
        public string street { get; set; }
        public string InTakePatientid { get; set; }
        public string InTake_PatientId { get; set; }
        public string Latitide { get; set; }
        public string Longitude { get; set; }
        public string TimeZoneId { get; set; }
        public string TimezonePostfix { get; set; }
        public string TimezoneOffset { get; set; }
        public string PatMedicalID { get; set; }

    }

}