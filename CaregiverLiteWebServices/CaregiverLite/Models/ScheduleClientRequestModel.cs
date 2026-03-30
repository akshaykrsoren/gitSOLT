using CaregiverLiteWCF.Class;
//using CaregiverLite.CaregiverLiteService;
using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Models
{
    public class ScheduleClientRequestModel
    {

        public int ClientRequestId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("ClientName")]
        public string ClientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgAddress")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.PatientRequest), ErrorMessageResourceName = "RequiredMsgStreet")]
        [DisplayName("Street")]
        public string Street { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.PatientRequest), ErrorMessageResourceName = "RequiredMsgCity")]
        [DisplayName("City")]
        public string City { get; set; }


        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.PatientRequest), ErrorMessageResourceName = "RequiredMsgState")]
        [DisplayName("State")]
        public string State { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgZipCode")]
        [StringLength(100, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgZipCode", MinimumLength = 4)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }
        public string CRNumber { get; set; }
        public string Description { get; set; }

        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Status { get; set; }
        public bool IsCancelled { get; set; }

        public string ServiceNames { get; set; }
        public string TimezoneId { get; set; }
        public int TimezoneOffset { get; set; }
        public string TimezonePostfix { get; set; }

        public string VisitTypeNames { get; set; }
        public string InsertUserId { get; set; }
        public bool IsRepeat { get; set; }
        public string RepeatEvery { get; set; }
        public string RepeatTypeID { get; set; }
        public string RepeatDate { get; set; }

        public string DayOfWeek { get; set; }
        public string DaysOfMonth { get; set; }
        public string Caregiver { get; set; }
        public float MaxDistance { get; set; }
        public int MaxCaregiver { get; set; }

        public int Office { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }

        public DateTime date1 { get; set; }




    }
}