using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Controllers
{
     [SessionExpire]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Dashboard()
        
        {
            PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
            CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
            List<PatientRequests> PaymentReceiveList = PatientRequestService.GetProjectedPayment(Session["Name"].ToString(), "0", "0", "1000-01-01", "1000-01-01").Result;


            var ProjectedPaymentPatient = PaymentReceiveList.Select(e => e.PatientName).Distinct().ToList();
            ViewBag.ProjectedPaymentPatient = ProjectedPaymentPatient;
            CareGivers objCareGivers = CareGiverLiteService.GetAllCareGiverByUserId(Session["UserId"].ToString()).Result;

            ViewBag.IsAvailable = objCareGivers.IsAvailable;
            ViewBag.IsBusy = objCareGivers.IsBusy;

            //PaymentReceiveList = PatientRequestService.GetPaymentReceivedByNurseId(Session["NurseId"].ToString(), "0", "1000-01-01", "1000-01-01").Result;
            ViewBag.PaymentReceivePatientAll = PaymentReceiveList;
            var PaymentReceivePatient = PaymentReceiveList.Select(e => e.PatientName).Distinct().ToList();
            ViewBag.PaymentReceivePatient = PaymentReceivePatient;

            return View();
        }
        public ActionResult GetRescheduleAlerts()
        {
            try
            {
                PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
                ViewBag.RescheduleAlerts = PatientRequestService.GetRescheduleAlertsByNurseId(Session["NurseId"].ToString()).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetRescheduleAlerts";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public ActionResult GetCancelledAppointment()
        {
            try
            {
                PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
                ViewBag.CancelledAppointment = PatientRequestService.GetCancelledAppointmentByPatientForNurseId(Session["NurseId"].ToString()).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetCancelledAppointment";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public ActionResult GetLastWeekPayment()
        {
            try
            {
                PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
                string lastWeekDate = DateTime.UtcNow.Date.AddDays(-7).ToString("yyyy-MM-dd");
                string toDate = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
                List<PatientRequests> PaymentReceiveList = PatientRequestService.GetPaymentReceivedByNurseId(Session["NurseId"].ToString(), "0", lastWeekDate, toDate).Result;
                ViewBag.PaymentReceivePatientAll = PaymentReceiveList;
                var PaymentReceivePatient = PaymentReceiveList.Select(e => e.PatientName).Distinct().ToList();
                ViewBag.PaymentReceivePatient = PaymentReceivePatient;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetLastWeekPayment";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }
        public ActionResult GetFinishedJobs()
        {
            try
            {

            }catch(Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetFinishedJobs";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
       }  

        public ActionResult GetCompleteAppointmentsByNurseId(JQueryDataTableParamModel param)
        {

            PatientRequestList PatientRequestsList = new PatientRequestList();
            try
            {

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                string NurseId = Session["NurseId"].ToString();
                string IsComplete = "1"; //  0-pending 1-complete

                if (sortColumnIndex == 0)
                {
                    sortOrder = "PatientName";
                }
                
                if (sortColumnIndex == 1)
                {
                    sortOrder = "HourRate";
                }
                if (sortColumnIndex == 2)
                {
                    sortOrder = "TotalHours";

                }
                if (sortColumnIndex == 3)
                {
                    sortOrder = "TotalAmount";
                }
                string search = "||"; //It's indicate blank filter

                if (!string.IsNullOrEmpty(param.sSearch))
                    search = param.sSearch;

                var sortDirection = Request["sSortDir_0"]; // asc or desc
                int pageNo = 1;
                int recordPerPage = param.iDisplayLength;

                //Find page number from the logic
                if (param.iDisplayStart > 0)
                {
                    pageNo = (param.iDisplayStart / recordPerPage) + 1;
                }

                PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
                PatientRequestsList = PatientRequestService.GetAppointmentsByNurseId(NurseId, IsComplete, pageNo, recordPerPage, search, sortOrder, sortDirection).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetCompleteAppointmentsByNurseId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (PatientRequestsList.PatientRequestsList != null)
            {
                var result = from C in PatientRequestsList.PatientRequestsList select new[] { C, C, C, C, C, C, C, C, C };

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = PatientRequestsList.TotalNumberofRecord,
                    iTotalDisplayRecords = PatientRequestsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = PatientRequestsList.TotalNumberofRecord,
                    iTotalDisplayRecords = PatientRequestsList.FilteredRecord,
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetPendingJobs()
        {
            try
            {

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetPendingJobs";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public ActionResult GetPendingAppointmentsByNurseId(JQueryDataTableParamModel param)
        {

            PatientRequestList PatientRequestsList = new PatientRequestList();
            try
            {
                decimal PendingTotalAmount = 0;
                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                string NurseId = Session["NurseId"].ToString();
                string IsComplete = "0";

                if (sortColumnIndex == 0)
                {
                    sortOrder = "PatientName";
                }
                if (sortColumnIndex == 1)
                {
                    sortOrder = "PatientAddress";
                }
                if (sortColumnIndex == 2)
                {
                    sortOrder = "HourRate";
                }
                if (sortColumnIndex == 3)
                {
                    sortOrder = "TotalHours";

                }
                if (sortColumnIndex == 4)
                {
                    sortOrder = "TotalAmount";
                }
                string search = "||"; //It's indicate blank filter

                if (!string.IsNullOrEmpty(param.sSearch))
                    search = param.sSearch;

                var sortDirection = Request["sSortDir_0"]; // asc or desc
                int pageNo = 1;
                int recordPerPage = param.iDisplayLength;

                //Find page number from the logic
                if (param.iDisplayStart > 0)
                {
                    pageNo = (param.iDisplayStart / recordPerPage) + 1;
                }

                PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
                PatientRequestsList = PatientRequestService.GetAppointmentsByNurseId(NurseId, IsComplete, pageNo, recordPerPage, search, sortOrder, sortDirection).Result;
                if (PatientRequestsList != null)
                {
                    for (int k = 0; k < PatientRequestsList.PatientRequestsList.Count(); k++)
                    {
                        PendingTotalAmount = PendingTotalAmount + PatientRequestsList.PatientRequestsList[0].TotalAmount;
                    }

                }
                ViewBag.PendingTotalAmount = PendingTotalAmount;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ReportsController";
                log.Methodname = "GetPendingAppointmentsByNurseId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (PatientRequestsList.PatientRequestsList != null)
            {
                var result = from C in PatientRequestsList.PatientRequestsList select new[] { C, C, C, C, C, C, C, C, C };

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = PatientRequestsList.TotalNumberofRecord,
                    iTotalDisplayRecords = PatientRequestsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = PatientRequestsList.TotalNumberofRecord,
                    iTotalDisplayRecords = PatientRequestsList.FilteredRecord,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetProjectedPayment()
        {
            try
            {
                string Zipcode = "0";
                string FromDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                string ToDate = DateTime.Now.ToString("yyyy-MM-dd");
                string PatientId = "0";
                string NurseId = Session["NurseId"].ToString();

                if (Request.QueryString["Zipcode"] != null)
                {
                    Zipcode = Request.QueryString["Zipcode"].ToString();
                }
                if (Request.QueryString["FromDate"] != null)
                {
                    FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy-MM-dd");
                }
                if (Request.QueryString["ToDate"] != null)
                {
                    ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]).ToString("yyyy-MM-dd");
                }
                if (Request.QueryString["PatientId"] != null)
                {
                    PatientId = Request.QueryString["PatientId"].ToString().Replace("$", " ");
                }
                

                PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
                ViewBag.PaymentReceiveList = PatientRequestService.GetProjectedPayment(Session["Name"].ToString(), PatientId, Zipcode, FromDate, ToDate).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetProjectedPayment";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public JsonResult GetPatientData(string day = "",string PatientName="")
        {
            List<Patients> CareGiverList = null;
            List<PatientRequests> PatientRequestsList = new List<PatientRequests>();
            string FromDate = "";
            string ToDate = "";
            try
            {
                if (day == "Today")
                {
                     FromDate = DateTime.Now.ToString("yyyy-MM-dd");
                     ToDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime Day_Today = DateTime.Today;
                    DateTime Day_First = Day_Today.AddDays(1 - Day_Today.DayOfWeek.GetHashCode() - 1);
                    DateTime Day_Last = Day_Today.AddDays(7 - Day_Today.DayOfWeek.GetHashCode() - 1);

                    FromDate = Day_First.ToString("yyyy-MM-dd");
                    ToDate = Day_Last.ToString("yyyy-MM-dd");
                }              

                PatientRequestServiceProxy PatientRequestService = new PatientRequestServiceProxy();
             
                PatientRequestsList = PatientRequestService.GetPaymentRequertForMapByNurseId(Session["NurseId"].ToString(), PatientName, FromDate, ToDate).Result;
               
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "DashboardController";
                log.Methodname = "GetNurseData";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(JsonConvert.SerializeObject(PatientRequestsList));
        }
    }
     public static class DateTimeExtensions
     {
         public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
         {
             int diff = dt.DayOfWeek - startOfWeek;
             if (diff < 0)
             {
                 diff += 7;
             }

             return dt.AddDays(-1 * diff).Date;
         }
     }


}
