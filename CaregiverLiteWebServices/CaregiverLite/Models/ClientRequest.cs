using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Models
{
    public class ClientRequest
    {

        public int ClientId { get; set; }

        public string ClientName { get; set; }
        public string CRNumber { get; set; }

        public bool IsRepeat { get; set; }
        public string RepeatDate { get; set; }
    
        public string RepeatEvery { get; set; }
    
        public string RepeatTypeID { get; set; }

        public string DayOfWeek { get; set; }
  
        public string DaysOfMonth { get; set; }

        public string MarketersName { get; set; }

        public string Date { get; set; }

        public int ClientRequestId { get; set; }

        public int Office { get; set; }

        public string Description { get; set; }

        public string MarketersId { get; set; }


        public string Address { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string ZipCode { get; set; }

        public string InsertUserId { get; set; }

        public string UpdateUserId { get; set; }

        public string TimezoneId { get; set; }

        public int TimezoneOffset { get; set; }

        public string TimezonePostfix { get; set; }

        public string Fromtime { get; set; }
        public string Totime { get; set; }
        public string Time { get; set; }

        public string TimeOfVist { set; get; }
        public string NameOfPractice { get; set; }

        public string DischargePlaneerName { get; set; }

        public string FirstVisit { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }


        public string UserName { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }


        public string DeviceType { get; set; }


        public string DeviceToken { get; set; }


        public string AccessToken { get; set; }


        public bool IsApprove { get; set; }


        public bool IsAvailable { get; set; }


        public string QuickbloxId { get; set; }



        public string AccessCode { get; set; }



        public string Active { get; set; }



        public string CreatedDate { get; set; }


        public bool AutoLogin { get; set; }

        public string ProfileImage { get; set; }

        public bool IsDeleted { get; set; }

        public bool CanAdminEdit { get; set; }


        public string Remarks { get; set; }

        public string Company { get; set; }
        public string OfficeId { get; set; }

        public string OfficeName { get; set; }
         public IEnumerable<SelectListItem> marketersSelectList { get; set; }



    }

    public class ClientREferrral
    {
        public string patientRequestid { get; set; }
        public string Marketersid { get; set; }
        public string ClientName { get; set; }
        public string VisitDate { get; set; }
        public string AdmissionStatus { get; set; }
        public string Reason { get; set; }
        public string Insurance { get; set; }
        public string PhoneNo { get; set; }
    }

    public class TimeZoneJSON
    {
        public string dstOffset { get; set; }
        public string rawOffset { get; set; }
        public string status { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
    }
    }