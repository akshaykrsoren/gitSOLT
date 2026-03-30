using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DifferenzLibrary;
using System.Runtime;
using System.Configuration;
using CaregiverLiteWCF;
using CaregiverLite.Action_Filters;
using Newtonsoft.Json;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class ChartController : Controller
    {
       
            
        //= Convert.ToInt32(Session["OrganisationId"]);

        // GET: Chart
        string constr = "";

     
        public ChartController()
        {
           // constr = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                           | SecurityProtocolType.Tls11
                                           | SecurityProtocolType.Tls12
                                           | SecurityProtocolType.Ssl3;

            
        }

        public ActionResult Index()
        {

            ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            return View();
        }

        [HttpGet]
        public ActionResult ChartDashBoard(string FromDate, string ToDate)
        {
            ChartData obj = new ChartData();
            List<string> OfficeNameList = new List<string>();
            List<int> OfficeCount = new List<int>();

          int  OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceList_ForChart",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAttendanceList_ForChart",
                                                       FromDate,
                                                        ToDate,
                                                        OrganisationId
                                                    );

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        OfficeNameList.Add(ds.Tables[0].Rows[i]["officeName"].ToString());
                        OfficeCount.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["TotalVisit"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErr orlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }

            //  return PartialView("~/Views/Chart/NurseVisitChart.cshtml");
            return RedirectToAction("NurseVisitChart");

            // return RedirectToAction("NurseVisitChart",);


        }
        public ActionResult PatientOfficewise_ChartData(string FromDate, string ToDate)
        {
            List<string> OfficeNameList = new List<string>();
            List<int> OfficeCount = new List<int>();
            ViewBag.OfficeNameList = null;
            ViewBag.OfficeCount = null;
            ViewBag.TotalVisitCount = null;

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPatientListBasedOnOffice_ForChart",
                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllPatientListBasedOnOffice_ForChart",
                                                         FromDate,
                                                         ToDate,
                                                         OrganisationId
                                                     );

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        OfficeNameList.Add(ds.Tables[0].Rows[i]["officeName"].ToString());
                        OfficeCount.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["TotalVisit"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }

            ViewBag.PatientBasedOnOfficeList_VisitChart = OfficeNameList;
            ViewBag.PatientBasedOnOfficeCount_VisitChart = OfficeCount;
            return PartialView();
        }

        public ActionResult TypeOfVisitByPatient_ChartData(string FromDate, string ToDate)
        {
            List<string> OfficeNameList = new List<string>();
            List<int> OfficeCount = new List<int>();
            ViewBag.OfficeNameList = null;
            ViewBag.OfficeCount = null;
            ViewBag.TotalVisitCount = null;

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTypeOfVisitByPatient_ForChart",

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetTypeOfVisitByPatient_ForChart",
                                                         FromDate,
                                                         ToDate,
                                                         OrganisationId
                                                     );

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        OfficeNameList.Add(ds.Tables[0].Rows[i]["VisitType"].ToString());
                        OfficeCount.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["TotalVisit"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }
            ViewBag.TypeOfVisitByPatientList_VisitChart = OfficeNameList;
            ViewBag.TypeOfVisitByPatientCount_VisitChart = OfficeCount;
            return PartialView();
        }

        public ActionResult WeeklyScheduleActivity_ChartData(string FromDate, string ToDate)
        {

            ViewBag.WeeklyScheduleAppoinmentList = null;
            ViewBag.WeeklyScheduleCountStatus = null;


            List<WeeklySchedule> objList = new List<WeeklySchedule>();
            List<WeeklyAppointment> WeekliCountstatus = new List<WeeklyAppointment>();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetWeeklyScheduleActivity_ForChart", OrganisationId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetWeeklyScheduleActivity_ForChart", OrganisationId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        WeeklySchedule obj = new WeeklySchedule();
                        obj.InsertedDatetime = ds.Tables[0].Rows[i]["insertdatetime"].ToString();
                        obj.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                        objList.Add(obj);
                    }
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        WeeklyAppointment obj1 = new WeeklyAppointment();
                        obj1.Status = ds.Tables[1].Rows[i]["status"].ToString();
                        obj1.StatusCount = ds.Tables[1].Rows[i]["StatusCount"].ToString();

                        WeekliCountstatus.Add(obj1);
                    }


                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }
            //ViewBag.WeeklyScheduleAppoinmentList = objList;
            //ViewBag.WeeklyScheduleCountStatus = WeekliCountstatus;
            //return PartialView();

           // ViewBag.WeeklyScheduleAppoinmentList = objList;
           // ViewBag.WeeklyScheduleCountStatus = WeekliCountstatus;


           // var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
           //ViewBag.WeeklyScheduleAppoinmentList = serializer.Serialize(objList);          
            ViewBag.WeeklyScheduleAppoinmentList = JsonConvert.SerializeObject(objList);


            //Second Solutions with Newtonsoft.Json
            //  ViewBag.WeeklyScheduleAppoinmentList = JsonConvert.DeserializeObject<List<WeeklySchedule>>(listDataWeekSchedule);


            //var serializer2 = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
            //ViewBag.WeeklyScheduleCountStatus = serializer2.Serialize(WeekliCountstatus);
            ViewBag.WeeklyScheduleCountStatus = JsonConvert.SerializeObject(WeekliCountstatus); 

            return PartialView();
        }

        public ActionResult ManualAutoScheduling_ChartData(string FromDate, string ToDate)
        {
            List<string> AutoDateList = new List<string>();
            List<int> AutoScheduleCount = new List<int>();

            List<string> ManualDateList = new List<string>();
            List<int> ManualScheduleCount = new List<int>();

            ViewBag.AutoDateList = null;
            ViewBag.AutoScheduleCount = null;

            ViewBag.ManualDateList = null;
            ViewBag.ManualScheduleCount = null;

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllManual_Auto_Schedule_ForChart",
                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllManual_Auto_Schedule_ForChart",
                                                         FromDate,
                                                         ToDate,
                                                         OrganisationId
                                                     );

                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AutoDateList.Add(ds.Tables[0].Rows[i]["ScheduleDate"].ToString());
                        AutoScheduleCount.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["TotalSchedule"].ToString()));
                    }
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        ManualDateList.Add(ds.Tables[1].Rows[j]["ScheduleDate"].ToString());
                        ManualScheduleCount.Add(Convert.ToInt32(ds.Tables[1].Rows[j]["TotalSchedule"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }
            ViewBag.AutoDateList = AutoDateList;
            ViewBag.AutoScheduleCount = AutoScheduleCount;

            ViewBag.ManualDateList = ManualDateList;
            ViewBag.ManualScheduleCount = ManualScheduleCount;
            return PartialView();
        }


        [HttpGet]
        public JsonResult DashBoardRecords_ForChart()
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            List<DashBoardCharRecord> DashBoardCharRecordList = new List<DashBoardCharRecord>();
            try
            {

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetDashBoardRecords_ForChart", OrganisationId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetDashBoardRecords_ForChart", OrganisationId);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DashBoardCharRecord obj = new DashBoardCharRecord();

                        obj.Type = ds.Tables[i].Columns[0].ColumnName;

                        obj.Count = (ds.Tables[i].Rows[0][obj.Type]).ToString();
                        DashBoardCharRecordList.Add(obj);

                    }

                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }

            return Json(DashBoardCharRecordList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult TotalLicenseRelated_ForChart()
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

             List<LicenseRecord> DashBoardLicenseRecordList = new List<LicenseRecord>();
            try
            {

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetDashBoardRecords_ForChart", OrganisationId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetLicenseRecordAsperOrganisation", OrganisationId);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        LicenseRecord obj = new LicenseRecord();

                        obj.TotalActiveLicense = ds.Tables[0].Rows[i]["TotalLicenses"].ToString();

                        obj.UsedActiveLicense = ds.Tables[0].Rows[i]["UsedLicenses"].ToString();

                        DashBoardLicenseRecordList.Add(obj);
                        //DashBoardCharRecordList.Add(obj);
                    }

                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }

            return Json(DashBoardLicenseRecordList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckInHours_ChartData(string FromDate, string ToDate)
        {
            List<string> CheckInDateList = new List<string>();
            List<int> CheckInCount = new List<int>();

            

            ViewBag.CheckInDateList = null;
            ViewBag.CheckInCount = null;

            
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            { 
             
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllCheckInHours_ForChart",
                                                     FromDate,
                                                     ToDate,
                                                     OrganisationId
                                                 );

                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CheckInDateList.Add(ds.Tables[0].Rows[i]["CheckInDate"].ToString());
                        CheckInCount.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["DurationInMinutes"].ToString()));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                //string result = InsertErrorLog(objErrorlog);
            }
            ViewBag.CheckInDateList = CheckInDateList;
            ViewBag.CheckInCount = CheckInCount; 
            return PartialView();
        }

    }


    public class ChartData
    {
        public IEnumerable<string> OfficeNameList { get; set; }
        public IEnumerable<string> OfficeCount { get; set; }

        public string name { get; set; }

    }

    public class DashBoardCharRecord
    {
        public string Type { get; set; }
        public string Count { get; set; }

    }

    public class LicenseRecord
    {
        public string UsedActiveLicense { get; set; }
        public string TotalActiveLicense { get; set; }

    }

    public class WeeklySchedule
    {
        public string InsertedDatetime { get; set; }
        public string Status { get; set; }

    }
    public class WeeklyAppointment
    {
        public string Status { get; set; }
        public string StatusCount { get; set; }

    }
}