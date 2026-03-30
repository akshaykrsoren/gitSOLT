using CaregiverLiteWCF.Class;
using CaregiverLiteWCF;
using CaregiverLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CaregiverLite.Action_Filters;
using System.Globalization;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using Excel;
using OfficeOpenXml;
using System.Configuration;
using CaregiverLite.Constants;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DifferenzLibrary;
using System.Net.Http;
using System.Text;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
using ClosedXML;
using OfficeOpenXml;
using ClosedXML.Excel;
using System.Net.Mail;
using System.Net.Http.Headers;
using WSServiceAddAppointment;
using System.Data.SqlClient;
using System.Threading;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text.RegularExpressions;
//using CaregiverLite.Views.Resources;

namespace CaregiverLite.Controllers
{
   [SessionExpire]
    public class PatientRequestController : Controller
    {
        [AllowAnonymous]
        public JsonResult GetAllSchedulePatientRequest(JQueryDataTableParamModel param)
        {
            SchedulePatientRequestList SchedulePatientRequestList = new SchedulePatientRequestList();
            try
            {


                int recordPerPage = 0;

                int FilterOffice = 0;
                string FilterStatus = "||";
                string FromDate = "1000-01-01";
                string ToDate = "1000-01-01";

                if (Convert.ToInt32(Request["FilterOffice"]) != 0) //Request["FilterOffice"] != null && Request["FilterOffice"] != "" &&
                {
                    FilterOffice = Convert.ToInt32(Request["FilterOffice"]);

                    if (FilterOffice == 0)//if (FilterOffice == "All")
                    {
                        FilterOffice = 0; //FilterOffice = "||";
                    }
                }
                if (Request["FilterStatus"] != null && Request["FilterStatus"] != "")
                {
                    FilterStatus = Request["FilterStatus"];

                    if (FilterStatus == "All")
                    {
                        FilterStatus = "||";
                    }
                }

                if (Request["FromDate"] != null && Request["FromDate"] != "")
                {
                    FromDate = Request["FromDate"];
                }

                if (Request["ToDate"] != null && Request["ToDate"] != "")
                {
                    ToDate = Request["ToDate"];
                }

                int IsAdmin = 0;
                string sortOrder = string.Empty;
                MembershipUser user = Membership.GetUser();
                string LogInUserId = Membership.GetUser().ProviderUserKey.ToString();

                string[] roles = Roles.GetRolesForUser(user.UserName);
                foreach (string role in roles)
                {
                    if (role == "SuperAdmin" || role == "OrgSuperAdmin")
                    {
                        Session["IsSuperAdmin"] = "true";
                        Session["UserId"] = user.ProviderUserKey.ToString();

                        //LogInUserId = Session["UserId"];
                        IsAdmin = 1;
                    }
                }

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                //if (sortColumnIndex == 0)
                //{
                //    sortOrder = "InsertDateTime";
                //}
                //else if (sortColumnIndex == 1)
                //{
                //    sortOrder = "SchedulerName";
                //}
                //else if (sortColumnIndex == 2)
                //{
                //    sortOrder = "PatientName";
                //}
                //else if (sortColumnIndex == 3)
                //{
                //    sortOrder = "Address";
                //}
                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "ZipCode";
                //}
                //else if (sortColumnIndex == 5)
                //{
                //    sortOrder = "MedicalId";
                //}
                //else if (sortColumnIndex == 6)
                //{
                //    sortOrder = "Description";
                //}
                //else if (sortColumnIndex == 7)
                //{
                //    sortOrder = "InsertDateTime";
                //}
                //else if (sortColumnIndex == 8)
                //{
                //    sortOrder = "Date";
                //}
                //else if (sortColumnIndex == 9)
                //{
                //    sortOrder = "FromTime";
                //}
                //else if (sortColumnIndex == 10)
                //{
                //    sortOrder = "ToTime";
                //}
                //else if (sortColumnIndex == 11)
                //{
                //    sortOrder = "CaregiverName";
                //}
                //else if (sortColumnIndex == 12)
                //{
                //    sortOrder = "Status";
                //}
                //else if (sortColumnIndex == 13)
                //{
                //    sortOrder = "Office";
                //}
                //else if (sortColumnIndex == 14)
                //{
                //    sortOrder = "TotalCaregiversNotified";
                //}
                //else if (sortColumnIndex == 15)
                //{
                //    sortOrder = "Check In/Out";
                //}
                //else
                //{
                //    sortOrder = "PatientRequestId";
                //}



                if (sortColumnIndex == 0)
                {
                    sortOrder = "InsertDateTime";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "PatientName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "Address";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "MedicalId";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "InsertDateTime";
                }
                else if (sortColumnIndex == 5)
                {
                    sortOrder = "Date";
                }
                else if (sortColumnIndex == 6)
                {
                    sortOrder = "FromTime";
                }
                else if (sortColumnIndex == 7)
                {
                    sortOrder = "ToTime";
                }
                else if (sortColumnIndex == 9)
                {
                    sortOrder = "Status";
                }
                else if (Convert.ToInt32(Session["OrganisationId"]) > 0)
                {
                    if (sortColumnIndex == 10)
                    {
                        sortOrder = "Office";
                    }
                }
                else if (sortColumnIndex == 11)
                {
                    sortOrder = "Office";
                }
                else
                {
                    sortOrder = "PatientRequestId";
                }


                string search = "||"; //It's indicate blank filter

                if (!string.IsNullOrEmpty(param.sSearch))
                    search = param.sSearch;

                var sortDirection = Request["sSortDir_0"];

                if (sortColumnIndex == 0)
                {
                    sortDirection = "desc";
                }
                /* var sortDirection = Request["sSortDir_0"]; */// asc or desc
                int pageNo = 1;

                //if (param.iDisplayLength == 100)
                //{
                //    //if (param.sEcho == "2")
                //    //{
                //    //    recordPerPage = 95;
                //    //}
                //    //else
                //    //{
                //    recordPerPage = 95;
                //    // }
                //}
                //else
                //{

                recordPerPage = param.iDisplayLength;

                //   }


                //Find page number from the logic

                //if (recordPerPage == 95)
                //{
                //    if (param.iDisplayStart > 0)
                //    {
                //        pageNo = (param.iDisplayStart / 100) + 1;
                //    }
                //}
                //else
                //{
                if (param.iDisplayStart > 0)
                {
                    pageNo = (param.iDisplayStart / recordPerPage) + 1;
                }

                //  }

                int OrganisationId = 0;
                if (!string.IsNullOrEmpty(Request["FilterOrganisationId"]))
                {
                    OrganisationId = Convert.ToInt32(Request["FilterOrganisationId"]);
                }
                else
                {
                    OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                }

                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                SchedulePatientRequestList = SchedulePatientRequestService.GetAllSchedulePatientRequest(pageNo, recordPerPage, search, sortOrder, sortDirection, IsAdmin, LogInUserId, Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"), FilterStatus, FilterOffice, OrganisationId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetAllSchedulePatientRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            if (SchedulePatientRequestList.SchedulePatientRequestsList != null)
            {
                var result = from C in SchedulePatientRequestList.SchedulePatientRequestsList select new[] { C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C };
                var jsonResult= Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = SchedulePatientRequestList.TotalNumberofRecord,
                    iTotalDisplayRecords = SchedulePatientRequestList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;

            }
            else
            {
                var jsonResult= Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = SchedulePatientRequestList.TotalNumberofRecord,
                    iTotalDisplayRecords = SchedulePatientRequestList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }

        }


        //[HttpGet]
        //public JsonResult SearchPatientByName(string term)
        //{
        //    //var patients = _context.Patients
        //    //    .Where(p => p.Name.Contains(term))
        //    //    .Select(p => new { p.Name })
        //    //    .ToList();
        //    return Json(patients, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult PatientRequest()
        {
            FillAllOffices();
            FillAllOrganisations();
            return View();
        }


        public ActionResult PasevaCustomerPatientRequest()
        {

            FillAllOffices();
            return View();
        }


        public JsonResult CancelPatientRequest(string patientrequestid)
        {
            string result = "";
            try
            {
                var userid = Session["UserId"].ToString();
                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                var operationResult = SchedulePatientRequestService.CancelPatientRequest(patientrequestid, userid).Result;

                if (operationResult != null && operationResult.CareGiverList != null)
                {
                    for (int i = 0; i < operationResult.CareGiverList.Count; i++)
                    {
                        var obj = operationResult.CareGiverList[i];
                        PushNotification objNotification = new PushNotification();
                        //string Notificationresult = objNotification.SendPushNotification("Patient Schedule Cancelled", "Patient Schedule Cancelled", obj.DeviceToken, obj.DeviceType, null, null, obj.NurseId.ToString(), null, "Patient Schedule Cancelled", userid, true, "\"Id\" :\"" + patientrequestid + "\",");
                    }
                }

                result = "Success";
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "CancelPatientRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "Nurse")]
        public ActionResult AddPatientRequest()
        {
            FillAllOffices();
            ServicesServiceProxy ServicesService = new ServicesServiceProxy();
            ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(Session["OrganisationId"])).Result;
            ViewBag.VisitTypeList = GetAllVisitType();
            ViewBag.ProcedureCodeList = null;

            ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            ViewBag.OrgSuperAdminName = Convert.ToString(Session["OrgSuperAdminName"]);
            ViewBag.OrgSuperAdminQBId = Convert.ToString(Session["OrgSuperAdminQBId"]);
            ViewBag.OrgSuperAdminEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

            return PartialView();
        }

        [HttpPost]
        public ActionResult AddPatientRequestInfo(SchedulePatientRequestModel objSchedulePatientRequest)
        {
            var PatientRequest = new SchedulePatientRequestModel();
            string caregiver = "";
            string Result2 = "";
            int tempPatientRequestid = 0;

            string[] s1 = objSchedulePatientRequest.Caregiver.ToString().Split(',');

            string jsonString = JsonConvert.SerializeObject(objSchedulePatientRequest, Newtonsoft.Json.Formatting.Indented);

            //ServicesServiceProxy ServicesService = new ServicesServiceProxy();

            //MembershipUser user = Membership.GetUser();
            //string userid =  user.ProviderUserKey.ToString();
            //var uid = Convert.ToString(Session["UserId"]);
            string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
            objSchedulePatientRequest.InsertUserId = InsertedUserID;

            //PatientRequest.InsertUserId = objSchedulePatientRequest.InsertUserId;
            try
            {
                SchedulePatientRequestServiceProxy CareGiverLiteService = new SchedulePatientRequestServiceProxy();

                PatientRequest.PatientRequestId = objSchedulePatientRequest.PatientRequestId;
                PatientRequest.PatientName = objSchedulePatientRequest.PatientName;
                PatientRequest.Address = objSchedulePatientRequest.Address;
                PatientRequest.Street = objSchedulePatientRequest.Street;
                PatientRequest.City = objSchedulePatientRequest.City;
                PatientRequest.State = objSchedulePatientRequest.State;
                PatientRequest.Latitude = objSchedulePatientRequest.Latitude;
                PatientRequest.Longitude = objSchedulePatientRequest.Longitude;
                PatientRequest.ZipCode = objSchedulePatientRequest.ZipCode;
                PatientRequest.MedicalId = objSchedulePatientRequest.MedicalId;
                PatientRequest.Description = objSchedulePatientRequest.Description;
                PatientRequest.Date = objSchedulePatientRequest.Date;
                PatientRequest.Date = DateTime.ParseExact(objSchedulePatientRequest.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString().Split(' ')[0];

                //string formattedDate = date.ToString("yyyy-MM-dd");
                PatientRequest.FromTime = objSchedulePatientRequest.FromTime;
                PatientRequest.ToTime = objSchedulePatientRequest.ToTime;
                PatientRequest.IsCancelled = objSchedulePatientRequest.IsCancelled;
                PatientRequest.InsertUserId = objSchedulePatientRequest.InsertUserId;
                if (objSchedulePatientRequest.ServiceNames != null)
                    PatientRequest.ServiceNames = objSchedulePatientRequest.ServiceNames.Trim(',');

                if (objSchedulePatientRequest.VisitTypeNames != null)
                    PatientRequest.VisitTypeNames = objSchedulePatientRequest.VisitTypeNames.Trim(',');

                PatientRequest.Office = objSchedulePatientRequest.Office;
                PatientRequest.TimezoneId = objSchedulePatientRequest.TimezoneId;
                PatientRequest.TimezoneOffset = objSchedulePatientRequest.TimezoneOffset;
                PatientRequest.TimezonePostfix = objSchedulePatientRequest.TimezonePostfix;

                PatientRequest.IsRepeat = objSchedulePatientRequest.IsRepeat;
                PatientRequest.RepeatEvery = objSchedulePatientRequest.RepeatEvery;
                PatientRequest.RepeatTypeID = objSchedulePatientRequest.RepeatTypeID;
                PatientRequest.RepeatDate = objSchedulePatientRequest.RepeatDate;
                PatientRequest.DayOfWeek = objSchedulePatientRequest.DayOfWeek;
                PatientRequest.DaysOfMonth = objSchedulePatientRequest.DaysOfMonth;
                PatientRequest.IsRequiredDriving = objSchedulePatientRequest.IsRequiredDriving;
                PatientRequest.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                // CaregiverLiteWCF.GetDatesList dates = CareGiverLiteService.GetFilterDates(PatientRequest);

                var FilterDatesList = CareGiverLiteService.GetFilterDates(PatientRequest).Result;

                for (int j = 0; j < FilterDatesList.Count; j++)
                {

                    PatientRequest.Date = FilterDatesList[j].ListDate;

                    CaregiverLiteWCF.CareGiversList result = CareGiverLiteService.InsertSchedulePatientRequest(PatientRequest).Result;
                    tempPatientRequestid = result.CareGiverList[0].PatientRequestId;
                    for (int i = 0; i < result.CareGiverList.Count; i++)
                    {
                        if (s1.Contains("chk" + result.CareGiverList[i].NurseId.ToString()))
                        {

                            //if care giver is matched Start
                            var obj = result.CareGiverList[i];
                            PushNotification objNotification = new PushNotification();
                            var userid = Convert.ToString(Session["UserId"]);

                            var PatientRequestFromTime = DateTime.ParseExact(PatientRequest.FromTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                            var PatientRequestToTime = DateTime.ParseExact(PatientRequest.ToTime, "HH:mm:ss", CultureInfo.InvariantCulture);

                            var ConvertFromTime = PatientRequestFromTime.ToString("hh:mm tt");
                            var ConvertToTime = PatientRequestToTime.ToString("hh:mm tt");
                            var ConvertPatientDate = Convert.ToDateTime(PatientRequest.Date).ToString("dd MMM yyyy");

                            //string Notificationresult = objNotification.SendPushNotification("Patient Schedule Request", "New Patient Schedule Request", obj.DeviceToken, obj.DeviceType, null, null, obj.NurseId.ToString(), null, "Patient Schedule Request", userid, true);
                            
                            string Notificationresult = objNotification.SendPushNotification("Patient Schedule Request", "Patient request added for" + ConvertFromTime + " to " + ConvertToTime + " " + PatientRequest.TimezonePostfix + " on " + ConvertPatientDate, obj.DeviceToken, obj.DeviceType, null, null, obj.NurseId.ToString(), null, "Patient Schedule Request", userid, true, "", obj.PatientRequestId);

                            string resultForCaregiverRequest = CareGiverLiteService.InsertSchedulePatientRequestTemp(obj.PatientRequestId.ToString(), obj.NurseId.ToString(), Notificationresult).Result;

                            try
                            {
                                if ((objSchedulePatientRequest.Office == 5 || objSchedulePatientRequest.Office == 12 || objSchedulePatientRequest.Office==1) && !string.IsNullOrEmpty(objSchedulePatientRequest.PayerId) && !string.IsNullOrEmpty(objSchedulePatientRequest.ProcedureCodes))
                                {
                                    string payerResult = InsertPayerInformationRequest(objSchedulePatientRequest.PayerId, objSchedulePatientRequest.PayerProgram, objSchedulePatientRequest.ProcedureCodes, objSchedulePatientRequest.JurisdictionCode, obj.PatientRequestId.ToString());
                                }
                                else
                                {
                                    if(PatientRequest.OrganisationId > 0 && !string.IsNullOrEmpty(objSchedulePatientRequest.PayerId) && !string.IsNullOrEmpty(objSchedulePatientRequest.ProcedureCodes))
                                    {
                                        string payerResult = InsertPayerInformationRequest(objSchedulePatientRequest.PayerId, objSchedulePatientRequest.PayerProgram, objSchedulePatientRequest.ProcedureCodes, objSchedulePatientRequest.JurisdictionCode, obj.PatientRequestId.ToString());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            // Result2=   GetAndPostEmployeeVisitData(PatientRequest,)
                        }   //if care giver is matched End
                    }
                }

                return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = true, RequestId = tempPatientRequestid });


                
                
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "[HttpPost] AddPatientRequestInfo";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = false });
            }
        }


        public string InsertPayerInformationRequest(string PayerId, string PayerProgram, string ProcedureCode, string JurisdictionCode, string Patientrequestid)
        {
            string result = "";
            int i;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("InsertPayerInformationRequest", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PayerId", PayerId);
                    cmd.Parameters.AddWithValue("@PayerProgram", PayerProgram);
                    cmd.Parameters.AddWithValue("@ProcedureCode", ProcedureCode);
                    cmd.Parameters.AddWithValue("@JurisdictionCode", JurisdictionCode);
                    cmd.Parameters.AddWithValue("@Patientrequestid", Patientrequestid);

                    i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        result = "success";

                    }
                }
            }
            return result;
        }



        //  public JsonResult GetAndPostEmployeeVisitData(string FromDate, String ToDate, String OfficeId)

        //public string GetAndPostEmployeeVisitData()
        // {
        //     Dictionary<string, object> res = new Dictionary<string, object>();
        //     string result = "";
        //     //string ss = "24-Nov-22 11:51:0";
        //     //DateTime dd1 = Convert.ToDateTime(ss);
        //     //string foo1 = dd1.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

        //     //DateTime fromDatetime = Convert.ToDateTime(FromDate);
        //     ////string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
        //     //string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd'");

        //     //DateTime ToDatetime = Convert.ToDateTime(ToDate);
        //     ////string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");
        //     //string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd'");

        //     EmployeeClientModel objModel = new EmployeeClientModel();

        //     List<ClientVisitRequest> clientVisitRequestList = new List<ClientVisitRequest>();
        //     try
        //     {

        //         //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeVisitRequestToaddSandDataForDateRange",
        //         //                                                                                            // FromScheduleDateTime,
        //         //                                                                                            // ToScheduleDateTime,
        //         //                                                                                            FromDate,
        //         //                                                                                            ToDate,
        //         //                                                                                             OfficeId);
        //         //if (ds != null)
        //         //{
        //         //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //         //    {

        //         //        foreach (DataRow item in ds.Tables[0].Rows)
        //         //        {
        //                     ClientVisitRequest clientVisitRequest = new ClientVisitRequest();
        //                     ClientVisitRequestProviderIdentification objprovider = new ClientVisitRequestProviderIdentification();
        //                     objprovider.ProviderID = "000000077";
        //                     objprovider.ProviderQualifier = "MedicaidID";
        //                     clientVisitRequest.ProviderIdentification = objprovider;

        //                     clientVisitRequest.EmployeeQualifier = "EmployeeCustomID";
        //                     clientVisitRequest.EmployeeOtherID = Convert.ToString(item["NurseId"]);
        //                     clientVisitRequest.EmployeeIdentifier = Convert.ToString(item["NurseId"]);
        //                     clientVisitRequest.VisitOtherID = Convert.ToString(item["NurseScheduleId"]);
        //                     clientVisitRequest.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
        //                     clientVisitRequest.GroupCode = null;//(single caregiver visits for multiple clients in same time sapn)
        //                     clientVisitRequest.ClientIDQualifier = "ClientMedicaidID";


        //                     if (Convert.ToString(item["MedicalId"]).Length >= 8)
        //                     {
        //                         clientVisitRequest.ClientID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
        //                     }
        //                     else
        //                     {
        //                         string medicadid = string.Empty;

        //                         int jlen = Convert.ToString(item["MedicalId"]).Length;
        //                         StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);
        //                         for (int i = jlen; i <= 8; i++)
        //                         {
        //                             if (i == 8)
        //                             {
        //                                 // string randomstr = GenerateRandomString();
        //                                 medicadid = s.Append('C').ToString();
        //                             }
        //                             else
        //                             {
        //                                 medicadid = s.Append('0').ToString();
        //                             }
        //                         }

        //                         clientVisitRequest.ClientID = medicadid;


        //                     }

        //                     // clientVisitRequest.ClientID = "99999999S"; //Convert.ToString(item["ClientId"]);
        //                     clientVisitRequest.ClientOtherID = Convert.ToString(item["PrimaryMD"]); //Convert.ToString(item["MedicalId"]);
        //                     clientVisitRequest.VisitCancelledIndicator = "false";
        //                     clientVisitRequest.PayerID = Convert.ToString(item["PayerId"]);
        //                     clientVisitRequest.PayerProgram = Convert.ToString(item["PayerProgram"]);
        //                     clientVisitRequest.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
        //                     clientVisitRequest.VisitTimeZone = "US/Pacific";
        //                     clientVisitRequest.ScheduleStartTime = Convert.ToString((Convert.ToDateTime(item["CheckInDateTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        //                     clientVisitRequest.ScheduleEndTime = Convert.ToString(Convert.ToDateTime(item["CheckOutDateTime"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        //                     clientVisitRequest.AdjInDateTime = null;// Convert.ToString(item["InsertDateTime"]);
        //                     clientVisitRequest.AdjOutDateTime = null;// Convert.ToString(item["InsertDateTime"]); 
        //                     clientVisitRequest.BillVisit = "true";
        //                     clientVisitRequest.HoursToBill = "10";
        //                     clientVisitRequest.HoursToPay = "10";
        //                     clientVisitRequest.Memo = "This is a memo!";
        //                     clientVisitRequest.ClientVerifiedTimes = "true";
        //                     clientVisitRequest.ClientVerifiedTasks = "true";
        //                     clientVisitRequest.ClientVerifiedService = "true";
        //                     clientVisitRequest.ClientSignatureAvailable = "true";
        //                     clientVisitRequest.ClientVoiceRecording = "true";
        //                     clientVisitRequest.Modifier1 = null;
        //                     clientVisitRequest.Modifier2 = null;
        //                     clientVisitRequest.Modifier3 = null;
        //                     clientVisitRequest.Modifier4 = null;

        //                     // ClientVisitRequestCalls clientVisitRequestCalls = new ClientVisitRequestCalls();
        //                     // List<ClientVisitRequestCalls>   clientVisitRequestCallsList = new List<ClientVisitRequestCalls>();
        //                     //clientVisitRequestCalls.CallExternalID= "123456789";
        //                     //clientVisitRequestCalls.CallDateTime = "";
        //                     //clientVisitRequestCalls.CallAssignment = "Time In";
        //                     //clientVisitRequestCalls.GroupCode = null;
        //                     //clientVisitRequestCalls.CallType = "Other";
        //                     //clientVisitRequestCalls.ProcedureCode = "T1000";
        //                     //clientVisitRequestCalls.ClientIdentifierOnCall = "111111111";
        //                     //clientVisitRequestCalls.MobileLogin = null;
        //                     //clientVisitRequestCalls.CallLatitude = Convert.ToString(item["Latitude"]);
        //                     //clientVisitRequestCalls.CallLongitude = Convert.ToString(item["Longitude"]);
        //                     //clientVisitRequestCalls.Location = "";
        //                     //clientVisitRequestCalls.TelephonyPIN = "";
        //                     //clientVisitRequestCalls.OriginatingPhoneNumber = "";
        //                     //clientVisitRequestCallsList.Add(clientVisitRequestCalls);

        //                     clientVisitRequest.Calls = null;// clientVisitRequestCallsList;
        //                     clientVisitRequestList.Add(clientVisitRequest);

        //         //        }
        //         //    }
        //         //}
        //         if (clientVisitRequestList.Count <= 0)
        //         {
        //             throw new Exception("No data Available to send");
        //         }

        //         var arraylist = clientVisitRequestList.ToArray();

        //         List<ClientVisitRequest> request = new List<ClientVisitRequest>();

        //         foreach (var ReqItem in arraylist)
        //         {
        //             request.Add(ReqItem);
        //         }
        //         string x = JsonConvert.SerializeObject(request);
        //         Task.Run(async () => { result = await objModel.SubmitEmployeeVisitRequestData(x); }).Wait();
        //         res["Success"] = true;
        //         res["Result"] = result;

        //     }
        //     catch (Exception ex)
        //     {
        //         res["Success"] = false;
        //         res["Result"] = ex.Message;

        //     }
        //     // return Json(res, JsonRequestBehavior.AllowGet);

        //     return result;
        // }


        //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");     

        [Authorize(Roles = "SuperAdmin,Admin")]
        public string CheckInOutPatientRequest(string PatientRequestId)
        {
            string result = "";
            try
            {
                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                var patientRequests = new SchedulePatientRequest();

                patientRequests.PatientRequestId = Convert.ToInt32(PatientRequestId);
                //patientRequests.InsertUserId = InsertedUserID;

                result = SchedulePatientRequestService.CheckInOutBySuperAdmin(patientRequests).Result;

            }
            catch (Exception ex)
            {

            }


            return result;
        }

        // Added By Vinod Verma for Import excel data
        #region ImportPatientFromExcel
        [HttpPost]
        public ActionResult ImportExcelData(HttpPostedFileBase file)
        {
            var Result = "";// new Status();
            try
            {
                var httpPostedFile = file; //System.Web.HttpContext.Current.Request.Files["UploadedExcel"];
                if (httpPostedFile != null && httpPostedFile.ContentLength > 0)
                {
                    Stream stream = httpPostedFile.InputStream;
                    DataTable data = null;
                    DataSet Exceldata = null;
                    //For Reading Excel
                    IExcelDataReader ExcelReader = null;

                    if (httpPostedFile.FileName.EndsWith(".xls"))
                    {
                        ExcelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        //Create column names from first row
                        ExcelReader.IsFirstRowAsColumnNames = true;
                        Exceldata = ExcelReader.AsDataSet();
                    }
                    else if (httpPostedFile.FileName.EndsWith(".xlsx"))
                    {
                        ExcelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        //Create column names from first row
                        ExcelReader.IsFirstRowAsColumnNames = true;
                        Exceldata = ExcelReader.AsDataSet();
                    }


                    //For Check Excel Data available Or Not
                    if (Exceldata != null)
                    {
                        //for convert dataset to datatable
                        data = Exceldata.Tables[0];
                    }

                    if (data != null)
                    {
                        if (data.Columns.Count < 2)
                        {
                            TempData["Message"] = "No Matching Data Found In Uploaded File";
                            TempData["error"] = true;
                            return RedirectToAction("PatientRequest", "PatientRequest");
                            //ObjStatus.IsSuccess = false;
                            //ObjStatus.Message = "No Matching Data Found In Uploaded File";
                            //Give Error message 
                        }
                        else
                        {
                            if (data.Columns[0].ToString().ToLower().Trim() == "agency"
                                && data.Columns[1].ToString().ToLower().Trim() == "patientname"
                                 && data.Columns[2].ToString().ToLower().Trim() == "mrn"
                                  && data.Columns[3].ToString().ToLower().Trim() == "user"
                                  && data.Columns[4].ToString().ToLower().Trim() == "discipline"
                                  && data.Columns[5].ToString().ToLower().Trim() == "date"
                                  && data.Columns[6].ToString().ToLower().Trim() == "time"
                                  && data.Columns[7].ToString().ToLower().Trim() == "set"
                                  && data.Columns[8].ToString().ToLower().Trim() == "schedulestatus"
                                  && data.Columns[9].ToString().ToLower().Trim() == "visitstatus"
                               )
                            {
                                ConstantVarForImportExcel constantVarForImportExcel = new ConstantVarForImportExcel();

                                int BlankRowCount = 0;
                                DataTable UserDataTable = new DataTable();
                                UserDataTable.Columns.Add("Agency", typeof(string));
                                UserDataTable.Columns.Add("PatientName", typeof(string));
                                UserDataTable.Columns.Add("MRN", typeof(string));
                                UserDataTable.Columns.Add("User", typeof(string));
                                UserDataTable.Columns.Add("Discipline", typeof(string));
                                UserDataTable.Columns.Add("Date", typeof(string));
                                UserDataTable.Columns.Add("Time", typeof(string));
                                UserDataTable.Columns.Add("Set", typeof(string));
                                UserDataTable.Columns.Add("ScheduleStatus", typeof(string));
                                UserDataTable.Columns.Add("VisitStatus", typeof(string));

                                for (int i = 0; i < data.Rows.Count; i++)
                                {
                                    if (data.Rows[i][0].ToString() != "" && data.Rows[i][1].ToString() != "" && data.Rows[i][2].ToString() != "" && data.Rows[i][3].ToString() != "")
                                    {

                                        DataRow DataRow = UserDataTable.NewRow();
                                        DataRow["Agency"] = data.Rows[i][0].ToString();
                                        DataRow["PatientName"] = data.Rows[i][1].ToString();
                                        DataRow["MRN"] = data.Rows[i][2].ToString();
                                        DataRow["User"] = data.Rows[i][3].ToString();
                                        DataRow["Discipline"] = data.Rows[i][4].ToString();
                                        DataRow["Date"] = data.Rows[i][5].ToString();
                                        DataRow["Time"] = data.Rows[i][6].ToString();
                                        DataRow["Set"] = data.Rows[i][7].ToString();
                                        DataRow["ScheduleStatus"] = data.Rows[i][8].ToString();
                                        DataRow["VisitStatus"] = data.Rows[i][9].ToString();
                                        UserDataTable.Rows.Add(DataRow);
                                        BlankRowCount = 0;
                                    }
                                    else
                                    {
                                        BlankRowCount++;
                                        //if (BlankRowCount == 5)
                                        //{
                                        //    break;
                                        //}
                                    }
                                }
                                if (UserDataTable.Rows.Count > 0)
                                {
                                    Result = CaregiverLiteService.AddPatientAppointmentFromExcel(UserDataTable, Membership.GetUser().ProviderUserKey.ToString());
                                    if (Result == "Success")
                                    {
                                        TempData["Message"] = "Patients' Details Uploaded Successfully";
                                        return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = true });
                                    }
                                    else
                                    {
                                        //TempData["Message"] = "No Matching Data Found In Uploaded File; " + Result;
                                        TempData["Message"] = "No Matching Data Found In Uploaded File, specified Office was not found";
                                        TempData["error"] = true;
                                        return RedirectToAction("PatientRequest", "PatientRequest");
                                    }
                                }
                                else
                                {
                                    TempData["Message"] = "No Matching Data Found In Uploaded File";
                                    TempData["error"] = true;
                                    return RedirectToAction("PatientRequest", "PatientRequest");

                                    //ObjStatus.Message = "No Matching Data Found In Uploaded File";
                                }
                            }
                            else
                            {
                                TempData["Message"] = "No Matching Data Found In Uploaded File";
                                TempData["error"] = true;
                                return RedirectToAction("PatientRequest", "PatientRequest");

                                //ObjStatus.Message = "No Matching Data Found In Uploaded File";
                                //"No Matching Data Found In Uploaded File";
                            }
                        }
                    }
                    else
                    {
                        TempData["Message"] = "No Data Found In Selected File";
                        TempData["error"] = true;
                        return RedirectToAction("PatientRequest", "PatientRequest");
                        //ObjStatus.IsSuccess = false;
                        //ObjStatus.Message = "No Data Found In Uploaded File";
                    }
                }
                else
                {
                    TempData["Message"] = "Please Select File For Import Patient Data";
                    TempData["error"] = true;
                    return RedirectToAction("PatientRequest", "PatientRequest");
                    // ObjStatus.IsSuccess = false;
                    //ObjStatus.Message = "No Excel File Found.";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "ImportExcelData";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                TempData["Message"] = "No Matching Data Found In Uploaded File";// e.Message + " in Selected file";
                TempData["error"] = true;
                return RedirectToAction("PatientRequest", "PatientRequest");

            }
            return RedirectToAction("PatientRequest", "PatientRequest");
        }
        #endregion
        // End for import excel data


        public ActionResult GetCareGiverList(JQueryDataTableParamModel param)
        {
            CareGiversList CareGiverList = new CareGiversList();
            try
            {
                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortOrderArr = new string[] {"Name",
                                                "ProfileImage",
                                                "ServiceName",
                                                "HourlyRate",
                                                "UserName",
                                                "Password",
                                                "Email",
                                                "Phone",
                                                "Address",
                                                "IsApprove"};

                sortOrder = sortOrderArr[sortColumnIndex];

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

                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                CareGiverList = SchedulePatientRequestService.GetAllCareGivers(pageNo, recordPerPage, search, sortOrder, sortDirection, "").Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetCareGiverList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (CareGiverList.CareGiverList != null)
            {
                var result = from C in CareGiverList.CareGiverList select new[] { C, C, C, C, C, C, C, C, C, C, C };

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = CareGiverList.TotalNumberofRecord,
                    iTotalDisplayRecords = CareGiverList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = CareGiverList.TotalNumberofRecord,
                    iTotalDisplayRecords = CareGiverList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult GetNurseList()
        {
            try
            {
                List<CareGivers> CareGiverList = new List<CareGivers>();
                string PatientRequestId = "0";

                if (Request.QueryString["PatientRequestId"] != null)
                {
                    PatientRequestId = Request.QueryString["PatientRequestId"].ToString();
                }

                List<CareGivers> CareGiversList = new List<CareGivers>();

                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                CareGiverList = SchedulePatientRequestService.GetAllNotifiedCareGiversByRequestId(PatientRequestId).Result;


                ViewBag.CareGiverList = CareGiverList;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetNurseList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public JsonResult GetPatientRequestDetailByMedicalID(string MedicalID)
        {
            SchedulePatientRequest obj = new SchedulePatientRequest();
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                obj = SchedulePatientRequestService.GetPatientRequestDetailByMedicalID(LoginUserId, MedicalID).Result;

                //  obj = SchedulePatientRequestService.GetPatientRequestDetailByMedicalID(LoginUserId, MedicalID).Result;

                ViewBag.ServiceList = new ServicesServiceProxy().GetAllServices(Convert.ToString(OrganisationId)).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "CancelPatientRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPatientRequestDetailByMedicalIDByOrganisation(string MedicalID)
        {
            SchedulePatientRequest obj = new SchedulePatientRequest();
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                obj = SchedulePatientRequestService.GetPatientRequestDetailByMedicalIDByOrganisation(LoginUserId, MedicalID, Convert.ToString(OrganisationId)).Result;

                //  obj = SchedulePatientRequestService.GetPatientRequestDetailByMedicalID(LoginUserId, MedicalID).Result;

                ViewBag.ServiceList = new ServicesServiceProxy().GetAllServices(Convert.ToString(OrganisationId)).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "CancelPatientRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult GetNurseListForMap(int PatientRequestId)
        {
            List<CareGivers> CareGiverList = new List<CareGivers>();
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                //string PatientRequestId = "0";
                //if (Request.QueryString["PatientRequestId"] != null)
                //{
                //    PatientRequestId = Request.QueryString["PatientRequestId"].ToString();
                //}

                if (PatientRequestId > 0)
                {
                    List<CareGivers> CareGiversList = new List<CareGivers>();

                    SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                    CareGiverList = SchedulePatientRequestService.GetAllNotifiedCareGiversByRequestId(Convert.ToString(PatientRequestId)).Result;

                    res["Success"] = true;
                    res["Result"] = CareGiverList;
                }
                else
                {
                    res["Success"] = false;
                    res["Result"] = "Patient Request Id not found";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetNurseList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string result = ErrorLogService.InsertErrorLog(log).Result;
                res["Success"] = false;
                res["Result"] = result;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TrackLocationByPatientRequestId(int PatientRequestId)
        {
            List<CareGiverTrackLocation> CareGiverTrackLocationList = new List<CareGiverTrackLocation>();
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                if (PatientRequestId > 0)
                {
                    List<CareGiverTrackLocation> CareGiversTrackLocationList = new List<CareGiverTrackLocation>();

                    SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                    CareGiverTrackLocationList = SchedulePatientRequestService.GetTrackLocationByPatientRequestId(Convert.ToString(PatientRequestId)).Result;

                    res["Success"] = true;
                    res["Result"] = CareGiverTrackLocationList;
                }
                else
                {
                    res["Success"] = false;
                    res["Result"] = "Patient Request Id not found";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetNurseList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string result = ErrorLogService.InsertErrorLog(log).Result;
                res["Success"] = false;
                res["Result"] = result;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRatingByPatientRequestId(int PatientRequestId)
        {
            string rating = "";
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                if (PatientRequestId > 0)
                {
                    List<CareGiverTrackLocation> CareGiversTrackLocationList = new List<CareGiverTrackLocation>();

                    SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                    rating = SchedulePatientRequestService.GetRateByPatientRequestId(Convert.ToString(PatientRequestId)).Result;

                    res["Success"] = true;
                    res["Result"] = rating;
                }
                else
                {
                    res["Success"] = false;
                    res["Result"] = "Patient Request Id not found";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetNurseList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string result = ErrorLogService.InsertErrorLog(log).Result;
                res["Success"] = false;
                res["Result"] = result;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetRatingByPatientRequestId(int PatientRequestId, string Rating)
        {
            string rating = "";
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {

                if (PatientRequestId > 0)
                {
                    SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                    rating = SchedulePatientRequestService.SetRateByPatientRequestId(Convert.ToString(PatientRequestId), Rating).Result;

                    res["Success"] = true;
                    res["Result"] = rating;
                }
                else
                {
                    res["Success"] = false;
                    res["Result"] = "Patient Request Id not found";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetNurseList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string result = ErrorLogService.InsertErrorLog(log).Result;
                res["Success"] = false;
                res["Result"] = result;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemovePatientRequest(string[] PatientRequestId)
        {
            string result = "";
            try
            {
                var userid = Session["UserId"].ToString();
                foreach (string PatientID in PatientRequestId)
                {
                    SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                    var operationResult = SchedulePatientRequestService.RemovePatientRequest(PatientID, userid).Result;

                    if (operationResult != null && operationResult.CareGiverList != null)
                    {
                        for (int i = 0; i < operationResult.CareGiverList.Count; i++)
                        {
                            var obj = operationResult.CareGiverList[i];
                            PushNotification objNotification = new PushNotification();
                            string Notificationresult = objNotification.SendPushNotification("Patient Appointment removed", "Patient Appointment removed", obj.DeviceToken, obj.DeviceType, null, null, obj.NurseId.ToString(), null, "Patient Appointment removed", userid, true, "\"Id\" :\"" + PatientID + "\",");

                        }
                    }
                }

                result = "Success";
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "RemovePatientRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //QuickBlox

        public JsonResult checkDialogId(string GroupName)
        {
            Boolean flag = false;
            try
            {
                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                flag = SchedulePatientRequestService.checkDialogId(GroupName).Result;

                //ViewBag.ServiceList = new ServicesServiceProxy().GetAllServices().Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "checkDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        //

        /*16-08-2020*/
        [HttpPost]
        public JsonResult getManuelCaregiver(SchedulePatientRequestModel RequestModel, string MaxCaregiver, string MaxDistance)
        {

            RequestModel.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            string jsonStringdata = JsonConvert.SerializeObject(RequestModel, Newtonsoft.Json.Formatting.Indented);

            CareGiversList CareGiverList = new CareGiversList();
            Dictionary<string, object> res = new Dictionary<string, object>();

            try
            {


                if (Convert.ToInt16(MaxCaregiver) > 0 && Convert.ToInt16(MaxDistance) > 0)
                {
                    List<CareGivers> CareGiversList = new List<CareGivers>();

                    SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                    CareGiverList = SchedulePatientRequestService.GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers(RequestModel).Result;


                    // CareGiverList = GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers(RequestModel);

                    res["Success"] = true;
                    res["Result"] = CareGiverList.CareGiverList;
                }
                else
                {
                    res["Success"] = false;
                    res["Result"] = "Patient Request Id not found";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetNurseList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string result = ErrorLogService.InsertErrorLog(log).Result;
                res["Success"] = false;
                res["Result"] = result;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        //public CareGiversList GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers(SchedulePatientRequestModel SchedulePatientRequest)/*From InsertSchedulePatientRequest*/
        //{
        //    var origin = "";
        //    var destination = "";

        //    CareGiversList objList = new CareGiversList();
        //    List<CareGivers> objCareGiverListing = new List<CareGivers>();
        //    string result = "";

        //    SchedulePatientRequest.date1 = DateTime.Parse(SchedulePatientRequest.Date);

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(SchedulePatientRequest.ServiceNames) && SchedulePatientRequest.ServiceNames.EndsWith(","))
        //        {
        //            SchedulePatientRequest.ServiceNames = SchedulePatientRequest.ServiceNames.TrimEnd(SchedulePatientRequest.ServiceNames[SchedulePatientRequest.ServiceNames.Length - 1]);
        //        }
        //        DataSet ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers_V1_test",
        //                                                SchedulePatientRequest.PatientName,
        //                                                SchedulePatientRequest.Address,
        //                                                SchedulePatientRequest.Latitude,
        //                                                SchedulePatientRequest.Longitude,
        //                                                SchedulePatientRequest.ZipCode,
        //                                                SchedulePatientRequest.MedicalId,
        //                                                SchedulePatientRequest.Description,
        //                                                SchedulePatientRequest.date1,
        //                                                SchedulePatientRequest.FromTime,
        //                                                SchedulePatientRequest.ToTime,
        //                                                SchedulePatientRequest.IsCancelled,
        //                                                SchedulePatientRequest.ServiceNames,
        //                                                SchedulePatientRequest.TimezoneId,
        //                                                SchedulePatientRequest.TimezoneOffset,
        //                                                SchedulePatientRequest.TimezonePostfix,
        //                                                //Guid.Parse(Membership.GetUser().ProviderUserKey.ToString()),
        //                                                SchedulePatientRequest.MaxDistance,
        //                                                SchedulePatientRequest.MaxCaregiver,
        //                                                Convert.ToInt32(SchedulePatientRequest.Office));


        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {

        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                CareGivers objCareGivers = new CareGivers();
        //                objCareGivers.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
        //                objCareGivers.Name = ds.Tables[0].Rows[i]["Name"].ToString();
        //                objCareGivers.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
        //                objCareGivers.Password = ds.Tables[0].Rows[i]["Password"].ToString();
        //                objCareGivers.Email = ds.Tables[0].Rows[i]["Email"].ToString();
        //                objCareGivers.Address = ds.Tables[0].Rows[i]["address"].ToString();
        //                objCareGivers.Street = ds.Tables[0].Rows[i]["Street"].ToString();
        //                objCareGivers.City = ds.Tables[0].Rows[i]["City"].ToString();
        //                objCareGivers.State = ds.Tables[0].Rows[i]["State"].ToString();
        //                objCareGivers.DeviceToken = ds.Tables[0].Rows[i]["DeviceToken"].ToString();
        //                objCareGivers.DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
        //                objCareGivers.Latitude = ds.Tables[0].Rows[i]["latitude"].ToString();
        //                objCareGivers.Longitude = ds.Tables[0].Rows[i]["longitude"].ToString();
        //                objCareGivers.CurrentLatitude = ds.Tables[0].Rows[i]["CurrentLatitude"].ToString();
        //                objCareGivers.CurrentLongitude = ds.Tables[0].Rows[i]["CurrentLongitude"].ToString();
        //                objCareGivers.DistanceUnit = ds.Tables[0].Rows[i]["Distance"].ToString().Substring(0, 5);
        //                objCareGivers.IsNurseBusy = ds.Tables[0].Rows[i]["NurseBusy"].ToString();
        //                objCareGivers.Office = ds.Tables[0].Rows[i]["Office"].ToString();


        //                //origin = (SchedulePatientRequest.Latitude +','+
        //                //                                SchedulePatientRequest.Longitude);

        //                //destination = ds.Tables[0].Rows[i]["latitude"].ToString() + ',' + ds.Tables[0].Rows[i]["longitude"].ToString();

        //                // objCareGivers.DistanceUnit = GetLatlongDistanceData((SchedulePatientRequest.Address+','+SchedulePatientRequest.ZipCode), ds.Tables[0].Rows[i]["address"].ToString()).Result; 

        //                //objCareGivers.DistanceUnit = GetLatlongDistanceData(origin,destination).Result;


        //                objCareGiverListing.Add(objCareGivers);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers";
        //        objErrorlog.UserID = SchedulePatientRequest.InsertUserId;
        //       // result = InsertErrorLog(objErrorlog);
        //    }
        //    objList.CareGiverList = objCareGiverListing;
        //    return objList;
        //}

        public List<VisitType> GetAllVisitType()
        {
            List<VisitType> VisitTypeList = new List<VisitType>();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllVisitType", OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        VisitType objService = new VisitType();
                        objService.VisitTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["VisitTypeId"]);
                        objService.VisitTypeName = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();

                        VisitTypeList.Add(objService);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllServices";

            }
            return VisitTypeList;
        }



        public async Task<string> GetLatlongDistanceData(string origin, string destination)
        {

            double xyz = 0;
            string distance = "";
            //double jkf =Convert.ToDouble(origin);

            //double abc = Convert.ToDouble(destination);

            var clientGetDialogId = new System.Net.Http.HttpClient();
            

            //  var requestUrl = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&key={2}&origins={0}&destinations={1}", origin, destination, "AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");

            clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + origin + "&destinations=" + destination + "&key=AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
            // clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/directions/json?origin=" + origin +"&destinations="+ destination +"&key=AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw");

            // clientGetDialogId.BaseAddress = new Uri(requestUrl);
            clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            var response1 = await clientGetDialogId.GetAsync("");
            var result1 = response1.Content.ReadAsStringAsync().Result;
            var data = (JObject)JsonConvert.DeserializeObject(result1);

            foreach (var row in data["rows"])
            {

                foreach (var elements in row["elements"])
                {
                    foreach (var dist in elements["distance"])
                    {

                        distance = (string)dist;
                        break;
                    }
                }
            }

            return distance;
        }

        private void FillAllOffices()
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try 
            { 
            string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
            OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();

            var lstOffices = officeServiceProxy.GetAllOffices(logInUserId, OrganisationId.ToString()).Result;
            lstOffices.Insert(0, new Office() { OfficeId = 0, OfficeName = "All" });
            SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName");
            ViewBag.lstOffice = officeSelectList;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }

        private void FillAllOrganisations(object SelectedValue = null)
        {
            SelectedValue = Convert.ToInt32(Session["OrganisationId"]);

            try
            { 
            string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
            OrganisationServiceProxy OrganisationServiceProxy = new OrganisationServiceProxy();

            var lstOrganisations = OrganisationServiceProxy.GetAllOrganisations(logInUserId, Convert.ToString(SelectedValue)).Result;
            SelectList OrganisationSelectList = new SelectList(lstOrganisations, "OrganisationId", "OrganisationName", SelectedValue);
            ViewBag.lstOrganisations = OrganisationSelectList;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }



        [AllowAnonymous]
        public JsonResult GetAllPasevaCustomerSchedulePatientRequest(JQueryDataTableParamModel param)
        {
            SchedulePatientRequestList SchedulePatientRequestList = new SchedulePatientRequestList();
            try
            {
                int recordPerPage = 0;

                int FilterOffice = 0;
                string FilterStatus = "||";
                string FromDate = "1000-01-01";
                string ToDate = "1000-01-01";

                if (Convert.ToInt32(Request["FilterOffice"]) != 0) //Request["FilterOffice"] != null && Request["FilterOffice"] != "" &&
                {
                    FilterOffice = Convert.ToInt32(Request["FilterOffice"]);

                    if (FilterOffice == 0)//if (FilterOffice == "All")
                    {
                        FilterOffice = 0; //FilterOffice = "||";
                    }
                }
                if (Request["FilterStatus"] != null && Request["FilterStatus"] != "")
                {
                    FilterStatus = Request["FilterStatus"];

                    if (FilterStatus == "All")
                    {
                        FilterStatus = "||";
                    }
                }

                if (Request["FromDate"] != null && Request["FromDate"] != "")
                {
                    FromDate = Request["FromDate"];
                }

                if (Request["ToDate"] != null && Request["ToDate"] != "")
                {
                    ToDate = Request["ToDate"];
                }

                int IsAdmin = 0;
                string sortOrder = string.Empty;
                MembershipUser user = Membership.GetUser();
                string LogInUserId = Membership.GetUser().ProviderUserKey.ToString();


                string[] roles = Roles.GetRolesForUser(user.UserName);
                foreach (string role in roles)
                {
                    if (role == "SuperAdmin")
                    {
                        Session["IsSuperAdmin"] = "true";
                        Session["UserId"] = user.ProviderUserKey.ToString();
                        //LogInUserId = Session["UserId"];
                        IsAdmin = 1;
                    }

                }

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "InsertDateTime";
                }

                else if (sortColumnIndex == 1)
                {
                    sortOrder = "SchedulerName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "PatientName";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "Address";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "ZipCode";
                }
                else if (sortColumnIndex == 5)
                {
                    sortOrder = "MedicalId";
                }
                else if (sortColumnIndex == 6)
                {
                    sortOrder = "Description";
                }
                else if (sortColumnIndex == 7)
                {
                    sortOrder = "InsertDateTime";
                }
                else if (sortColumnIndex == 8)
                {
                    sortOrder = "Date";
                }
                else if (sortColumnIndex == 9)
                {
                    sortOrder = "FromTime";
                }
                else if (sortColumnIndex == 10)
                {
                    sortOrder = "ToTime";
                }
                else if (sortColumnIndex == 11)
                {
                    sortOrder = "CaregiverName";
                }
                else if (sortColumnIndex == 12)
                {
                    sortOrder = "Status";
                }
                else if (sortColumnIndex == 13)
                {
                    sortOrder = "Office";
                }
                else if (sortColumnIndex == 14)
                {
                    sortOrder = "TotalCaregiversNotified";
                }
                else if (sortColumnIndex == 15)
                {
                    sortOrder = "Check In/Out";
                }
                else
                {
                    sortOrder = "PatientRequestId";
                }

                string search = "||"; //It's indicate blank filter
                if (!string.IsNullOrEmpty(param.sSearch))
                    search = param.sSearch;

                var sortDirection = Request["sSortDir_0"];

                if (sortColumnIndex == 0)
                {
                    sortDirection = "desc";
                }
                /* var sortDirection = Request["sSortDir_0"]; */// asc or desc
                int pageNo = 1;


                if (param.iDisplayLength == 100)
                {
                    //if (param.sEcho == "2")
                    //{
                    //    recordPerPage = 95;
                    //}
                    //else
                    //{
                    recordPerPage = 95;
                    // }
                }
                else
                {
                    recordPerPage = param.iDisplayLength;
                }

                //Find page number from the logic

                if (recordPerPage == 95)
                {
                    if (param.iDisplayStart > 0)
                    {
                        pageNo = (param.iDisplayStart / 100) + 1;
                    }
                }
                else
                {
                    if (param.iDisplayStart > 0)
                    {
                        pageNo = (param.iDisplayStart / recordPerPage) + 1;
                    }
                }

                SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                SchedulePatientRequestList = SchedulePatientRequestService.GetAllPasevaCustomerSchedulePatientRequest(pageNo, recordPerPage, search, sortOrder, sortDirection, IsAdmin, LogInUserId, Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"), FilterStatus, FilterOffice).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetAllSchedulePatientRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            if (SchedulePatientRequestList.SchedulePatientRequestsList != null)
            {
                var result = from C in SchedulePatientRequestList.SchedulePatientRequestsList select new[] { C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = SchedulePatientRequestList.TotalNumberofRecord,
                    iTotalDisplayRecords = SchedulePatientRequestList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = SchedulePatientRequestList.TotalNumberofRecord,
                    iTotalDisplayRecords = SchedulePatientRequestList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        public async Task<string> AddPatientScheduleRequestInfo(SchedulePatientRequest objSchedulePatientRequest)
        {

            var PatientRequest = new WSServiceAddAppointment.Model.SchedulePatientRequest();
            // var PatientRequest = new SchedulePatientRequest();
            string caregiver = "";


            string[] s1 = objSchedulePatientRequest.Caregiver.ToString().Split(',');

            // string s1 = objSchedulePatientRequest.Caregiver.ToString();
            string jsonStringdata = "";
            var resultdata = "";

            //string jsonString = JsonConvert.SerializeObject(objSchedulePatientRequest, Newtonsoft.Json.Formatting.Indented);

            //ServicesServiceProxy ServicesService = new ServicesServiceProxy();

            //MembershipUser user = Membership.GetUser();
            //string userid =  user.ProviderUserKey.ToString();
            //var uid = Convert.ToString(Session["UserId"]);
            // string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
            // objSchedulePatientRequest.InsertUserId = InsertedUserID;

            //PatientRequest.InsertUserId = objSchedulePatientRequest.InsertUserId;

            try
            {
                SchedulePatientRequestServiceProxy CareGiverLiteService = new SchedulePatientRequestServiceProxy();

                PatientRequest.PatientRequestId = objSchedulePatientRequest.PatientRequestId;
                PatientRequest.PatientName = objSchedulePatientRequest.PatientName;
                PatientRequest.Address = objSchedulePatientRequest.Address;
                PatientRequest.Street = objSchedulePatientRequest.Street;
                PatientRequest.City = objSchedulePatientRequest.City;
                PatientRequest.State = objSchedulePatientRequest.State;
                PatientRequest.Latitude = objSchedulePatientRequest.Latitude;
                PatientRequest.Longitude = objSchedulePatientRequest.Longitude;
                PatientRequest.ZipCode = objSchedulePatientRequest.ZipCode;
                PatientRequest.MedicalId = objSchedulePatientRequest.MedicalId;
                PatientRequest.Description = objSchedulePatientRequest.Description;
                PatientRequest.Date = objSchedulePatientRequest.Date.ToString().Split(' ')[0];
                //PatientRequest.Date = DateTime.ParseExact(objSchedulePatientRequest.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString().Split(' ')[0];

                //string formattedDate = date.ToString("yyyy-MM-dd");
                PatientRequest.FromTime = objSchedulePatientRequest.FromTime;
                PatientRequest.ToTime = objSchedulePatientRequest.ToTime;
                PatientRequest.IsCancelled = objSchedulePatientRequest.IsCancelled;
                PatientRequest.InsertUserId = objSchedulePatientRequest.InsertUserId;
                if (objSchedulePatientRequest.ServiceNames != null)
                    PatientRequest.ServiceNames = objSchedulePatientRequest.ServiceNames.Trim(',');
                if (objSchedulePatientRequest.VisitTypeNames != null)
                    PatientRequest.VisitTypeNames = objSchedulePatientRequest.VisitTypeNames;
                //.Trim(',');

                PatientRequest.Office = objSchedulePatientRequest.Office;
                PatientRequest.TimezoneId = objSchedulePatientRequest.TimezoneId;
                PatientRequest.TimezoneOffset = objSchedulePatientRequest.TimezoneOffset;
                PatientRequest.TimezonePostfix = objSchedulePatientRequest.TimezonePostfix;

                PatientRequest.IsRepeat = objSchedulePatientRequest.IsRepeat;
                PatientRequest.RepeatEvery = objSchedulePatientRequest.RepeatEvery;
                PatientRequest.RepeatTypeID = objSchedulePatientRequest.RepeatTypeID;
                PatientRequest.RepeatDate = objSchedulePatientRequest.RepeatDate;
                PatientRequest.DayOfWeek = objSchedulePatientRequest.DayOfWeek;
                PatientRequest.DaysOfMonth = objSchedulePatientRequest.DaysOfMonth;
                PatientRequest.Caregiver = objSchedulePatientRequest.Caregiver;



                PatientRequest.PayerId= objSchedulePatientRequest.PayerId;
                PatientRequest.PayerProgram= objSchedulePatientRequest.PayerProgram;
                PatientRequest.ProcedureCode = objSchedulePatientRequest.ProcedureCode;
                PatientRequest.JurisdictionCode = objSchedulePatientRequest.JurisdictionCode;


                // CaregiverLiteWCF.GetDatesList dates = CareGiverLiteService.GetFilterDates(PatientRequest);

                //this code is commented 
                Root objss = new Root();
                // objss.PatientRequest = PatientRequest;
                //these are commmented code if not called ws api application


                WSS objssgg = new WSS();

                jsonStringdata = JsonConvert.SerializeObject(PatientRequest, Newtonsoft.Json.Formatting.Indented);

                WSServiceAddAppointment.Model.RootRegister newdata = objssgg.AddAppointmentsNew(PatientRequest);

                resultdata = newdata.message;

                if (resultdata == "Success")
                {
                    var GroupName1 = PatientRequest.PatientName.Trim() + "(" + PatientRequest.MedicalId.Trim() + ")";

                    var chkData = checkDialogId(GroupName1);

                    var chkGroupNme = checkGroupName(PatientRequest.MedicalId.Trim());

                    if (!checkGroupName(PatientRequest.MedicalId.Trim()))
                    {

                        if (chkData.Data.Equals(true))
                        {
                            return resultdata;
                        }
                        else
                        {
                            var SuperAdminQBID = "32132455";

                            Task.Run(async () =>
                            {
                                ChattingController obj = new ChattingController();
                                string rsultnew = await obj.CreatePatientGroupChatOnQuickBloxRestAPI(GroupName1, SuperAdminQBID, PatientRequest.Office, PatientRequest.InsertUserId);
                            }).Wait();
                        }
                    }
                    else
                    {
                        return resultdata;
                    }



                }


                // call WellsKyapplication like third party api,  and replaces model with WSServiceAddAppointment.Model.SchedulePatientRequest()


                //   DataTable dt = new DataTable();


                //jsonStringdata = JsonConvert.SerializeObject(objss, Newtonsoft.Json.Formatting.Indented);


                //resultdata = await Task.Run(() => WSAppointmentApi(jsonStringdata, PatientRequest.PatientName, PatientRequest.MedicalId, PatientRequest.Office, PatientRequest.InsertUserId));




                // Task.Run(async() => {resultdata=  Convert.ToString(WSAppointmentApi(jsonStringdata, PatientRequest.PatientName, PatientRequest.MedicalId, PatientRequest.Office, PatientRequest.InsertUserId)); }).Wait();







                //var FilterDatesList = CareGiverLiteService.GetFilterDates(PatientRequest).Result;


                //for (int j = 0; j < FilterDatesList.Count; j++)

                //{

                //    PatientRequest.Date = FilterDatesList[j].ListDate;

                //    CaregiverLiteWCF.CareGiversList result = CareGiverLiteService.InsertSchedulePatientRequest(PatientRequest).Result;

                //    for (int i = 0; i < result.CareGiverList.Count; i++)
                //    {
                //        if (s1.Contains(result.CareGiverList[i].NurseId.ToString()))
                //        {//if care giver is matched Start
                //            var obj = result.CareGiverList[i];
                //            PushNotification objNotification = new PushNotification();
                //            var userid = Convert.ToString(Session["UserId"]);

                //            var PatientRequestFromTime = DateTime.ParseExact(PatientRequest.FromTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                //            var PatientRequestToTime = DateTime.ParseExact(PatientRequest.ToTime, "HH:mm:ss", CultureInfo.InvariantCulture);

                //            var ConvertFromTime = PatientRequestFromTime.ToString("hh:mm tt");
                //            var ConvertToTime = PatientRequestToTime.ToString("hh:mm tt");
                //            var ConvertPatientDate = Convert.ToDateTime(PatientRequest.Date).ToString("dd MMM yyyy");
                //            //string Notificationresult = objNotification.SendPushNotification("Patient Schedule Request", "New Patient Schedule Request", obj.DeviceToken, obj.DeviceType, null, null, obj.NurseId.ToString(), null, "Patient Schedule Request", userid, true);
                //            string Notificationresult = objNotification.SendPushNotification("Patient Schedule Request", "Patient request added for " + ConvertFromTime + " to " + ConvertToTime + " " + PatientRequest.TimezonePostfix + " on " + ConvertPatientDate, obj.DeviceToken, obj.DeviceType, null, null, obj.NurseId.ToString(), null, "Patient Schedule Request", userid, true, "", obj.PatientRequestId);

                //            string resultForCaregiverRequest = CareGiverLiteService.InsertSchedulePatientRequestTemp(obj.PatientRequestId.ToString(), obj.NurseId.ToString(), Notificationresult).Result;
                //        }//if care giver is matched End
                //    }
                //}

                //  return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = true });
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "[HttpPost] AddPatientRequestInfo";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                // return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = false });
            }
            return resultdata;
        }


        private async Task<string> WSAppointmentApi(string jsonStringdata, string PatientName, string MedicalId, int OfficeId, string AdminUseId)
        {
            string resultset = "Success";


            HttpClient client = new HttpClient();

            string WellskyUrl = ConfigurationManager.AppSettings["WellskyUrl"].ToString();

            var content = new StringContent(jsonStringdata, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // var response = await client.PostAsync("http://localhost:62496/WSS.svc/AddAppointmentsnew", content);
            var response = await client.PostAsync(WellskyUrl, content);
            var result = response.Content.ReadAsStringAsync().Result;

            var ResultResponse = (JObject)JsonConvert.DeserializeObject(result);

            string isSuccess = ResultResponse["AddAppointmentsNewResult"]["message"].Value<string>();

            //  DataTable dt = new DataTable();

            if (isSuccess == "Success")
            {
                var GroupName1 = PatientName.Trim() + "(" + MedicalId.Trim() + ")";

                var chkData = checkDialogId(GroupName1);

                if (chkData.Data.Equals(true))
                {

                    return isSuccess;
                }
                else
                {
                    var SuperAdminQBID = "32132455";



                    Task.Run(() => { ChattingController obj = new ChattingController();
                        obj.CreatePatientGroupChatOnQuickBloxRestAPI(GroupName1, SuperAdminQBID, OfficeId, AdminUseId); }).Wait(); ;
                }



            }

            if (isSuccess == Convert.ToString(resultset))
            {
                return isSuccess;
            }

            return isSuccess;
        }



        [HttpGet]
        public ActionResult GetReadExcelScheduledList()
        {
            return PartialView("ImportScheduledAppointmentReader");
        }


        [HttpGet]
        public ActionResult GetReadExcelScheduledListWithWellsky()
        {
            return PartialView("ImportScheduleWithWellskyData");
        }


        //Live Updated code
        #region
        [HttpPost]

        public ActionResult ImportScheduledAppointmentReader()
        {
            return PartialView();
        }


        [HttpPost]

        public ActionResult ImportScheduleWithWellskyData()
        {
            return PartialView();
        }



        [HttpPost]
        public async Task<ActionResult> ImportScheduledExcelDataset()
        {
            string empty = string.Empty;
            int Scheduled = 0;
            int error = 0;
            int y = 2;
            string remoteUri = ConfigurationManager.AppSettings["BulkScheduling"].ToString();
            string myStringWebResource = "";
            try
            {
                if (this.Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = this.Request.Files;
                    for (int i = 0; i < files.Count; ++i)
                    {
                        HttpPostedFileBase httpPostedFileBase = files[0];
                        if (this.Request.Browser.Browser.ToUpper() == "IE" || this.Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] strArray = httpPostedFileBase.FileName.Split('\\');
                            string str = strArray[strArray.Length - 1];
                        }
                        else
                        {
                            string fileName = httpPostedFileBase.FileName;
                        }
                        if (httpPostedFileBase != null)
                        {
                            string path = this.Server.MapPath("~/Uploads/");
                            string str1 = this.Server.MapPath("~/BulkSchedulingExcel/");
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            if (!Directory.Exists(str1))
                                Directory.CreateDirectory(str1);
                            string filename = path + "AppointmentScheduled_" + DateTime.Now.ToString("MM.dd.yy hh.mm.ss") + "_" + Path.GetFileName(httpPostedFileBase.FileName);
                            string extension = Path.GetExtension(httpPostedFileBase.FileName);
                            httpPostedFileBase.SaveAs(filename);
                            string FileName = "AppointmentScheduled_" + DateTime.Now.ToString("MM.dd.yy hh.mm") + Path.GetFileName(httpPostedFileBase.FileName);
                            string filePath2 = Path.Combine(str1, FileName);
                            Path.GetExtension(httpPostedFileBase.FileName);
                            httpPostedFileBase.SaveAs(filePath2);
                            string connectionString = string.Empty;
                            switch (extension)
                            {
                                case ".xls":
                                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=YES'";
                                    break;
                                case ".xlsx":
                                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=YES'";
                                    break;
                            }
                            DataTable dt = new DataTable();
                            DataTable dataTable1 = new DataTable();
                            using (OleDbConnection connExcel = new OleDbConnection(connectionString))
                            {
                                using (OleDbCommand cmdExcel = new OleDbCommand())
                                {
                                    using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter())
                                    {
                                        cmdExcel.Connection = connExcel;
                                        connExcel.Open();
                                        string str2 = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, (object[])null).Rows[0]["TABLE_NAME"].ToString();
                                        connExcel.Close();
                                        connExcel.Open();
                                        cmdExcel.CommandText = "SELECT * From [" + str2 + "]";
                                        oleDbDataAdapter.SelectCommand = cmdExcel;
                                        oleDbDataAdapter.Fill(dt);
                                    }
                                    DataTable dataTable2 = new DataTable();
                                    dt.Columns.Add(new DataColumn("Comments", typeof(string)));
                                    ExcelPackage package = new ExcelPackage(new FileInfo(filePath2));
                                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                                    ((IEnumerable<ExcelWorksheet>)package.Workbook.Worksheets).FirstOrDefault<ExcelWorksheet>();
                                    int num = 1;
                                    int columnIndex = 1;
                                    while (!string.IsNullOrEmpty(((ExcelRangeBase)worksheet.Cells[num, columnIndex]).Text))
                                        ++columnIndex;
                                    ((ExcelRangeBase)worksheet.Cells[num, columnIndex]).Value = (object)"Comments";
                                    ExcelRange cell = worksheet.Cells[num, columnIndex];
                                    ((ExcelRangeBase)cell).Style.Fill.PatternType = (ExcelFillStyle)1;
                                    ((ExcelRangeBase)cell).Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
                                    ((ExcelRangeBase)cell).Style.Border.Top.Style = (ExcelBorderStyle)4;
                                    ((ExcelRangeBase)cell).Style.Border.Top.Color.SetColor(Color.Black);
                                    ((ExcelRangeBase)cell).Style.Font.Bold = true;
                                    worksheet.Column(columnIndex).Style.Font.Bold = true;
                                    foreach (DataColumn column in (InternalDataCollectionBase)dt.Columns)
                                        column.ColumnName = column.ColumnName.ToUpper();
                                    foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                                    {
                                        DataRow item = row;
                                        if (!(item["MRN"].ToString() == ""))
                                        {
                                            if (!string.IsNullOrEmpty(item["MRN"].ToString()))
                                            {
                                                SchedulePatientRequest schedulePatient = new SchedulePatientRequest();
                                                schedulePatient.MedicalId = item["MRN"].ToString();
                                                schedulePatient.CaregiverName = item["EMPLOYEE"].ToString();
                                                 string chkProcedureCode = item["CODE"].ToString();
                                                schedulePatient.InsertUserId = ConfigurationManager.AppSettings["AutomatedAppointementUserIdFromIndia"].ToString().ToString();
                                                Task.Run((Func<Task>)(async () => schedulePatient = this.GetPatientRequestDetailByMedicalIDs(schedulePatient.InsertUserId, schedulePatient.MedicalId, schedulePatient.CaregiverName,chkProcedureCode))).Wait();
                                                schedulePatient.MaxCaregiver = Convert.ToInt32(100);
                                                schedulePatient.Street = schedulePatient.Street;
                                                schedulePatient.ServiceNames = schedulePatient.ServiceNames;
                                                schedulePatient.TimezoneOffset = Convert.ToInt32(schedulePatient.TimezoneOffset);
                                                schedulePatient.City = schedulePatient.City;
                                                schedulePatient.Address = schedulePatient.Address;
                                                schedulePatient.TimezoneId = schedulePatient.TimezoneId;
                                                schedulePatient.IsCancelled = Convert.ToBoolean(false);
                                                schedulePatient.State = schedulePatient.State;
                                                schedulePatient.TimezonePostfix = schedulePatient.TimezonePostfix;
                                                schedulePatient.FromTime = "02:00:00";
                                                schedulePatient.PatientRequestId = schedulePatient.PatientRequestId;
                                                schedulePatient.ToTime = "20:00:00";
                                                schedulePatient.Date = item["SCHEDULE DATE"].ToString();
                                                schedulePatient.Office = schedulePatient.Office;
                                                schedulePatient.ZipCode = schedulePatient.ZipCode;
                                                schedulePatient.Description = "";
                                                schedulePatient.VisitTypeNames = item["TASK"].ToString();
                                                schedulePatient.RepeatEvery = "";
                                                schedulePatient.RepeatTypeID = "";
                                                if (dt.Columns.Contains("CODE") && !string.IsNullOrEmpty(schedulePatient.PayerId))
                                                {
                                                    schedulePatient.PayerId = schedulePatient.PayerId;
                                                    schedulePatient.PayerProgram = schedulePatient.PayerProgram;
                                                    schedulePatient.ProcedureCode = item["CODE"].ToString();
                                                    schedulePatient.JurisdictionCode = schedulePatient.JurisdictionCode;
                                                }
                                                if (!string.IsNullOrEmpty(schedulePatient.Latitude) && !string.IsNullOrEmpty(schedulePatient.Longitude))
                                                {
                                                    schedulePatient.Latitude = schedulePatient.Latitude;
                                                    schedulePatient.Longitude = schedulePatient.Longitude;
                                                }
                                                else if (string.Compare(item["MRN"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0)
                                                {
                                                    string address = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address=key{0}&key={1}", (object)(schedulePatient.Address + "," + schedulePatient.ZipCode), (object)"AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw");
                                                    using (WebClient webClient = new WebClient())
                                                    {
                                                        JObject jobject = JsonConvert.DeserializeObject<JObject>(webClient.DownloadString(address));
                                                        JToken jtoken1 = jobject["results"][(object)0][(object)"geometry"][(object)"location"][(object)"lat"];
                                                        JToken jtoken2 = jobject["results"][(object)0][(object)"geometry"][(object)"location"][(object)"lng"];
                                                        schedulePatient.Latitude = Convert.ToString((object)jtoken1);
                                                        schedulePatient.Longitude = Convert.ToString((object)jtoken2);
                                                    }
                                                }
                                                if (string.Compare(item["MRN"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0 && schedulePatient.MedicalId != "" && schedulePatient.Caregiver != "" && schedulePatient.Caregiver != null && schedulePatient.MedicalId != null && !string.IsNullOrEmpty(schedulePatient.Caregiver) && !string.IsNullOrEmpty(schedulePatient.MedicalId))
                                                {
                                                    string str3 = await this.AddPatientScheduleRequestInfo(schedulePatient);
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Value = (object)str3;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.PatternType = (ExcelFillStyle)1;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.BackgroundColor.SetColor(Color.White);
                                                    ((ExcelRangeBase)worksheet.Cells).Style.Border.Top.Style = (ExcelBorderStyle)4;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Border.Top.Color.SetColor(Color.Black);
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Font.Bold = true;
                                                    ++Scheduled;
                                                    ++y;
                                                }
                                                else if (item["MRN"].ToString().Trim().ToLower() != schedulePatient.MedicalId.ToString().Trim().ToLower() && schedulePatient.MedicalId == null && string.IsNullOrEmpty(schedulePatient.MedicalId))
                                                {
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Value = (object)"ERROR: MedicalId(MRN) Not Found On Paseva";
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.PatternType = (ExcelFillStyle)1;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.BackgroundColor.SetColor(Color.White);
                                                    ((ExcelRangeBase)worksheet.Cells).Style.Border.Top.Style = (ExcelBorderStyle)4;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Border.Top.Color.SetColor(Color.Black);
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Font.Bold = true;
                                                    ++error;
                                                    ++y;
                                                }
                                                else if (schedulePatient.Caregiver == null && string.IsNullOrEmpty(schedulePatient.Caregiver))
                                                {
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Value = (object)"ERROR: Employee Not Found On Paseva";
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.PatternType = (ExcelFillStyle)1;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.BackgroundColor.SetColor(Color.White);
                                                    ((ExcelRangeBase)worksheet.Cells).Style.Border.Top.Style = (ExcelBorderStyle)4;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Border.Top.Color.SetColor(Color.Black);
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Font.Bold = true;
                                                    ++error;
                                                    ++y;
                                                }
                                                else if (string.Compare(item["MRN"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0)
                                                {
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Value = (object)"ERROR: Patient & MedicalId(MRN) Data Mismatch Problem On Paseva";
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.PatternType = (ExcelFillStyle)1;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.BackgroundColor.SetColor(Color.White);
                                                    ((ExcelRangeBase)worksheet.Cells).Style.Border.Top.Style = (ExcelBorderStyle)4;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Border.Top.Color.SetColor(Color.Black);
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Font.Bold = true;
                                                    ++error;
                                                    ++y;
                                                }
                                                else
                                                {
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Value = (object)"ERROR: Fail";
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.PatternType = (ExcelFillStyle)1;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Fill.BackgroundColor.SetColor(Color.White);
                                                    ((ExcelRangeBase)worksheet.Cells).Style.Border.Top.Style = (ExcelBorderStyle)4;
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Border.Top.Color.SetColor(Color.Black);
                                                    ((ExcelRangeBase)worksheet.Cells[y, columnIndex]).Style.Font.Bold = true;
                                                    ++error;
                                                    ++y;
                                                }
                                                package.Save();
                                                item = (DataRow)null;
                                            }
                                            else
                                                break;
                                        }
                                        else
                                            break;
                                    }
                                    myStringWebResource = remoteUri + FileName;
                                    this.SendUpdatedExcelToEmail(filePath2, FileName, Scheduled, error);
                                    myStringWebResource = remoteUri + FileName;
                                    return (ActionResult)this.Json((object)myStringWebResource, (JsonRequestBehavior)0);
                                }
                            }
                        }
                    }
                    files = (HttpFileCollectionBase)null;
                }
            }
            catch (Exception ex)
            {
            }
            return (ActionResult)this.Json((object)myStringWebResource, (JsonRequestBehavior)0);
        }


        [HttpPost]
        // public async Task<ActionResult> ImportScheduledExcelData(HttpPostedFileBase file)
        public async Task<ActionResult> ImportScheduledExcelData()
        {
            string x = "";

            string finalresult = "";

            string FullAddress = "";

            string filePath = string.Empty;

            string ObjStatus = "";
            string filePath2 = "";

            string FileName = "";

            int Scheduled = 0;
            int error = 0;
            int y = 2;

            string remoteUri = ConfigurationManager.AppSettings["BulkScheduling"].ToString(), myStringWebResource = "";
            try
            {
            if (Request.Files.Count > 0)
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null)
                    {
                        string path = Server.MapPath("~/Uploads/");
                        string path2 = Server.MapPath("~/BulkSchedulingExcel/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }

                        filePath = path + "AppointmentScheduled_" + DateTime.Now.ToString("MM.dd.yy hh.mm.ss") + "_" + Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filePath);

                        FileName = "AppointmentScheduled_" + DateTime.Now.ToString("MM.dd.yy hh.mm") + Path.GetFileName(file.FileName);
                        filePath2 = System.IO.Path.Combine(path2, FileName);
                        string extension2 = Path.GetExtension(file.FileName);

                        file.SaveAs(filePath2);

                        string conString = string.Empty;


                            //switch (extension)
                            //{
                            //    case ".xls":
                            //        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            //        break;
                            //    case ".xlsx":
                            //        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            //        break;
                            //}

                            switch (extension)
                            {
                                case ".xls":
                                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                                    break;
                                case ".xlsx":
                                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                                    //";Extended Properties=Excel 8.0;";
                                    // conString = "Provider=Microsoft.ACE.OLEDB.4.0;Data Source='" + filePath +" ';Extended Properties='Excel 9.0;HDR=YES'";
                                    break;
                            }


                            DataTable dt = new DataTable();

                        DataTable dt1 = new DataTable();
                        // conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);

                                    //odaExcel.Fill(1, 3, dt);
                                }

                                DataTable dataset = new DataTable();
                                // dt.Columns.Add("Status");

                                dt.Columns.Add(new DataColumn("Comments", typeof(string)));

                                //  DataRow row = dt.NewRow();


                                #region 
                                //creating excel file in edit in existing file


                                string PathExtension = filePath2;

                                //  FileStream fileInfo = new FileStream(PathExtension, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                FileInfo fileInfo = new FileInfo(PathExtension);


                                 ExcelPackage package = new ExcelPackage(fileInfo);
                                 ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                                 ExcelWorksheet worksheet1 = package.Workbook.Worksheets.FirstOrDefault();


                                    //using (var package1 = new ExcelPackage(fileInfo))
                                    //{
                                    //    foreach (var worksheet4 in package1.Workbook.Worksheets)
                                    //    {
                                    //        if (worksheet4.Hidden != eWorkSheetHidden.Visible)
                                    //        {
                                    //            worksheet4.Hidden = eWorkSheetHidden.Visible;
                                    //            Console.WriteLine($"Unhidden: {worksheet4.Name}");
                                    //        }
                                    //    }
                                    //}


                                    //  ExcelPackage package = new ExcelPackage(fileInfo);
                                    //// ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                                    //ExcelWorksheet worksheet1 = package.Workbook.Worksheets.FirstOrDefault();

                                    //    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                                    //    if (worksheet == null)
                                    //    {
                                    //        worksheet = package.Workbook.Worksheets[1];
                                    //    }


                                    // get number of rows in the sheet
                                    //   int rows = worksheet.Dimension.Rows; // 10

                                    //   worksheet.Cells[1, 7].Value = "Comments";

                                    int headerRow = 1;
                                int columnIndex = 1;

                                // Find the first empty cell in the header row
                                while (!string.IsNullOrEmpty(worksheet.Cells[headerRow, columnIndex].Text))
                                {
                                    columnIndex++;
                                }

                                worksheet.Cells[headerRow, columnIndex].Value = "Comments";

                                // Apply styles
                                var cell = worksheet.Cells[headerRow, columnIndex];
                                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.OrangeRed);
                                cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                cell.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                cell.Style.Font.Bold = true;

                                // Optional: bold entire column
                                worksheet.Column(columnIndex).Style.Font.Bold = true;
                                    // package.Save();
                                    //while (y == 1)
                                    //{

                                    //    worksheet.Cells[1, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //    worksheet.Cells[1, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.OrangeRed);
                                    //    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    //    worksheet.Cells[1, 7].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);

                                    //    worksheet.Column(7).Style.Font.Bold = true;

                                    //    package.Save();
                                    //    y++;
                                    //}

                                    // loop through the worksheet rows           
                                    //end of excel file existance

                                    #endregion
                                   foreach (DataColumn column in dt.Columns)
                                   {
                                       column.ColumnName = column.ColumnName.Trim().ToUpper();
                                   }

                                   foreach (DataRow item in dt.Rows)
                                   {

                                    if (item["MRN"].ToString() == "" || string.IsNullOrEmpty(item["MRN"].ToString()))
                                    {
                                        break;
                                    }

                                    SchedulePatientRequest schedulePatient = new SchedulePatientRequest();

                                   schedulePatient.MedicalId = item["MRN"].ToString();
                                   schedulePatient.CaregiverName = item["EMPLOYEE"].ToString();


                                        string chkProcedureCode = null;

                                        if (dt.Columns.Cast<DataColumn>().Any(c => c.ColumnName.Trim().ToUpper() == "CODE"))
                                        {
                                            var colName = dt.Columns.Cast<DataColumn>()
                                                                    .First(c => c.ColumnName.Trim().ToUpper() == "CODE")
                                                                    .ColumnName;

                                            if (item[colName] != DBNull.Value)
                                            {
                                                chkProcedureCode = item[colName].ToString();
                                            }
                                        }

                                   string InsertedUserID = ConfigurationManager.AppSettings["AutomatedAppointementUserIdFromIndia"].ToString();

                                   schedulePatient.InsertUserId = InsertedUserID.ToString();
                                   Task.Run(async () => { schedulePatient = GetPatientRequestDetailByMedicalIDs(schedulePatient.InsertUserId, schedulePatient.MedicalId, schedulePatient.CaregiverName, chkProcedureCode); }).Wait();
                                    
                                    schedulePatient.MaxCaregiver = Convert.ToInt32(100);
                                    schedulePatient.Street = schedulePatient.Street;
                                    schedulePatient.ServiceNames = schedulePatient.ServiceNames;

                                    schedulePatient.TimezoneOffset = Convert.ToInt32(schedulePatient.TimezoneOffset);
                                    schedulePatient.City = schedulePatient.City;
                                    schedulePatient.Address = schedulePatient.Address;

                                    schedulePatient.TimezoneId = schedulePatient.TimezoneId;
                                    schedulePatient.IsCancelled = Convert.ToBoolean(false);
                                    schedulePatient.State = schedulePatient.State;
                                    schedulePatient.TimezonePostfix = schedulePatient.TimezonePostfix;
                                    schedulePatient.FromTime = "02:00:00";

                                    schedulePatient.PatientRequestId = schedulePatient.PatientRequestId;
                                    schedulePatient.ToTime = "20:00:00";
                                    schedulePatient.Date = item["SCHEDULE DATE"].ToString();
                                    schedulePatient.Office = schedulePatient.Office;

                                    schedulePatient.ZipCode = schedulePatient.ZipCode;
                                    schedulePatient.Description = "";
                                    schedulePatient.VisitTypeNames = item["TASK"].ToString();
                                    schedulePatient.RepeatEvery = "";
                                    schedulePatient.RepeatTypeID = "";

                                        // if (dt.Columns.Contains("CODE"))
                                        // {
                                        //    if (!string.IsNullOrEmpty(schedulePatient.PayerId))
                                        //    {
                                        //        schedulePatient.PayerId = schedulePatient.PayerId;
                                        //        schedulePatient.PayerProgram = schedulePatient.PayerProgram;
                                        //        schedulePatient.ProcedureCode = item["CODE"].ToString(); 
                                        //        schedulePatient.JurisdictionCode = schedulePatient.JurisdictionCode;
                                        //    }
                                        //} 

                                        if (!string.IsNullOrEmpty(schedulePatient.PayerId))
                                        {
                                            schedulePatient.PayerId = schedulePatient.PayerId;
                                            schedulePatient.PayerProgram = schedulePatient.PayerProgram;
                                            schedulePatient.JurisdictionCode = schedulePatient.JurisdictionCode;

                                            schedulePatient.ProcedureCode = dt.Columns.Contains("CODE")
                                                ? item["CODE"].ToString()
                                                : null; // or a default value
                                        }


                                    if (!string.IsNullOrEmpty(schedulePatient.Latitude) && !string.IsNullOrEmpty(schedulePatient.Longitude))
                                    {
                                        schedulePatient.Latitude = schedulePatient.Latitude;
                                        schedulePatient.Longitude = schedulePatient.Longitude;
                                    }
                                    else
                                    {
                                        //  FullAddress = schedulePatient.Street + ',' + schedulePatient.City + ',' + schedulePatient.State + ',' + schedulePatient.ZipCode;
                                        // if (Convert.ToString(item["MRN"]) == Convert.ToString(schedulePatient.MedicalId))
                                        //   if(String.Equals(item["MRN"].ToString(), schedulePatient.MedicalId))
                                        if (string.Compare(item["MRN"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0)
                                        {
                                            FullAddress = schedulePatient.Address + "," + schedulePatient.ZipCode;

                                            var requestUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address=key{0}&key={1}", FullAddress, "AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw");
                                            using (var client = new WebClient())
                                            {
                                                var result1 = client.DownloadString(requestUrl);
                                                var data1 = JsonConvert.DeserializeObject<JObject>(result1);

                                                var Latitude = data1["results"][0]["geometry"]["location"]["lat"];
                                                var Longitude = data1["results"][0]["geometry"]["location"]["lng"];

                                                schedulePatient.Latitude = Convert.ToString(Latitude);
                                                schedulePatient.Longitude = Convert.ToString(Longitude);
                                            }
                                        }
                                    }

                                        //if (worksheet.Cells[1, 7].Value.ToString() == "Comments")
                                        //{
                                        // if (item["Patient"].ToString().Trim().ToLower() == schedulePatient.PatientName.Trim().ToLower() && string.Compare(item["MRN"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0 && schedulePatient.MedicalId != "" && schedulePatient.Caregiver != "" && schedulePatient.Caregiver != null && schedulePatient.MedicalId != null && !string.IsNullOrEmpty(schedulePatient.Caregiver) && !string.IsNullOrEmpty(schedulePatient.MedicalId))
                                        if (string.Compare(item["MRN"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0 && schedulePatient.MedicalId != "" && schedulePatient.Caregiver != "" && schedulePatient.Caregiver != null && schedulePatient.MedicalId != null && !string.IsNullOrEmpty(schedulePatient.Caregiver) && !string.IsNullOrEmpty(schedulePatient.MedicalId))
                                        {
                                           
                                            finalresult = await AddPatientScheduleRequestInfo(schedulePatient);
                                        
                                            worksheet.Cells[y, columnIndex].Value = finalresult;
                                            worksheet.Cells[y, columnIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            worksheet.Cells[y, columnIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                            worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                            worksheet.Cells[y, columnIndex].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                            worksheet.Cells[y, columnIndex].Style.Font.Bold = true; 
                                            Scheduled = Scheduled + 1;
                                            y = y + 1;
                                        }
                                        else
                                        {
                                            if (item["MRN"].ToString().Trim().ToLower() != schedulePatient.MedicalId.ToString().Trim().ToLower() && schedulePatient.MedicalId == null && string.IsNullOrEmpty(schedulePatient.MedicalId))
                                            {

                                                worksheet.Cells[y, columnIndex].Value = "ERROR: MedicalId(MRN) Not Found On Paseva";
                                                worksheet.Cells[y, columnIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                worksheet.Cells[y, columnIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                worksheet.Cells[y, columnIndex].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                worksheet.Cells[y, columnIndex].Style.Font.Bold = true; 
                                                error = error + 1;
                                                y = y + 1;
                                            }
                                            else
                                            {
                                                if (schedulePatient.Caregiver == null && string.IsNullOrEmpty(schedulePatient.Caregiver))
                                                {
                                                    worksheet.Cells[y, columnIndex].Value = "ERROR: Employee Not Found On Paseva";
                                                    worksheet.Cells[y, columnIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                    worksheet.Cells[y, columnIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                    worksheet.Cells[y, columnIndex].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                    worksheet.Cells[y, columnIndex].Style.Font.Bold = true; 
                                                    error = error + 1;
                                                    y = y + 1;
                                                }
                                                else
                                                {
                                                   // if (item["Patient"].ToString().Trim().ToLower() != schedulePatient.PatientName.Trim().ToLower())
                                                   if(string.Compare(item["MRN"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0)
                                                    {
                                                        worksheet.Cells[y, columnIndex].Value = "ERROR: Patient & MedicalId(MRN) Data Mismatch Problem On Paseva";
                                                        worksheet.Cells[y, columnIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                        worksheet.Cells[y, columnIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                        worksheet.Cells[y, columnIndex].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                        worksheet.Cells[y, columnIndex].Style.Font.Bold = true; 
                                                        error = error + 1;
                                                        y = y + 1;
                                                    }
                                                    else
                                                    {

                                                        worksheet.Cells[y, columnIndex].Value = "ERROR: Fail";
                                                        worksheet.Cells[y, columnIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                        worksheet.Cells[y, columnIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                        worksheet.Cells[y, columnIndex].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                        worksheet.Cells[y, columnIndex].Style.Font.Bold = true; 
                                                        error = error + 1;
                                                        y = y + 1;
                                                    }
                                                }

                                            }

                                        }
                                        package.Save();

                                  //  }


                                    // replace occurences
                                    //if (worksheet.Cells[1, 7].Value.ToString() == "Comments")
                                    //{
                                    //    if (finalresult == "Success")
                                    //            {

                                    //        worksheet.Cells[y, 7].Value = finalresult;
                                    //        worksheet.Cells[y, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //        worksheet.Cells[y, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                    //        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    //        worksheet.Cells[y, 7].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                    //        worksheet.Column(7).Style.Font.Bold = true;

                                    //               item["Comments"] = finalresult;
                                    //                 Scheduled = Scheduled + 1;
                                    //                 y = y + 1;
                                    //            }
                                    //            else
                                    //            {
                                    //    worksheet.Cells[y, 7].Value = "Fail";
                                    //    worksheet.Cells[y, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //    worksheet.Cells[y, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                    //    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    //    worksheet.Cells[y, 7].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                    //    worksheet.Column(7).Style.Font.Bold = true;

                                    //    item["Comments"] = "Fail";
                                    //                error = error + 1;
                                    //                y = y + 1;
                                    //            }

                                    //     }    

                                }
                                     myStringWebResource = remoteUri + FileName;
                                    if (SendUpdatedExcelToEmail(filePath2, FileName, Scheduled, error)) ;
                                    {
                                        //return RedirectToAction("PatientRequest", "PatientRequest");
                                        myStringWebResource = remoteUri + FileName;

                                        return Json(myStringWebResource, JsonRequestBehavior.AllowGet);

                                    }
                                   // exportToExcel(dt, Scheduled, error);
                                }
                        }
                    }
                }
            }
           }
            catch (Exception ex)
            {

                insertdata(ex.Message);

                myStringWebResource = remoteUri + FileName;
                if (SendUpdatedExcelToEmail(filePath2, FileName, Scheduled, error)) ;
                {
                    //return RedirectToAction("PatientRequest", "PatientRequest");
                    myStringWebResource = remoteUri + FileName;

                    return Json(myStringWebResource, JsonRequestBehavior.AllowGet);

                }
            }

            return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
            //  return RedirectToAction("PatientRequest", "PatientRequest");
        }

        #endregion




        //Testing Code for Emerging Data

        [HttpPost]
        // public async Task<ActionResult> ImportScheduledExcelData(HttpPostedFileBase file)
        public async Task<ActionResult> ImportScheduledExcelDataWithWellsKy()
        {
            string x = "";

            string finalresult = "";

            string FullAddress = "";

            string filePath = string.Empty;

            string ObjStatus = "";

            int Scheduled = 0;
            int error = 0;
            int y = 1;
            string PatientName = "";

            string remoteUri = ConfigurationManager.AppSettings["WellskyBulkScheduling"].ToString(), myStringWebResource = "";

            if (Request.Files.Count > 0)
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null)
                    {
                        string path = Server.MapPath("~/Uploads/");
                        string path2 = Server.MapPath("~/Uploads/WellskyBatchScheduling/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }


                        filePath = path + "AppointmentScheduled_" + DateTime.Now.ToString("MM.dd.yy hh.mm.ss") + "_" + Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filePath);
             
                        string FileName = "AppointmentScheduled_" + DateTime.Now.ToString("MM.dd.yy hh.mm") + Path.GetFileName(file.FileName);
                        string filePath2 = System.IO.Path.Combine(path2, FileName);
                        string extension2 = Path.GetExtension(file.FileName);

                        file.SaveAs(filePath2);

                        string conString = string.Empty;

                        switch (extension)
                        {
                            case ".xls":
                                conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                                break;
                            case ".xlsx":
                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                                break;
                        }

                        DataTable dt = new DataTable();

                        DataTable dt1 = new DataTable();
                        // conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);

                                    //odaExcel.Fill(1, 3, dt);
                                }

                                DataTable dataset = new DataTable();
                                // dt.Columns.Add("Status");

                                dt.Columns.Add(new DataColumn("Comments", typeof(string)));
                                //  DataRow row = dt.NewRow();

                                #region 
                                //creating excel file in edit in existing file


                                string PathExtension = filePath2;

                                //  FileStream fileInfo = new FileStream(PathExtension, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                FileInfo fileInfo = new FileInfo(PathExtension);


                                ExcelPackage package = new ExcelPackage(fileInfo);
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                                ExcelWorksheet worksheet1 = package.Workbook.Worksheets.FirstOrDefault();

                                // get number of rows in the sheet
                                int rows = worksheet.Dimension.Rows; // 10

                                worksheet.Cells[1, 25].Value = "Comments";
                                while (y == 1)
                                {

                                    worksheet.Cells[1, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[1, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.OrangeRed);
                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[1, 25].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);

                                    worksheet.Column(25).Style.Font.Bold = true;

                                    package.Save();
                                    y++;
                                }

                                // loop through the worksheet rows           
                                //end of excel file existance
                                #endregion

                                foreach (DataRow item in dt.Rows)
                                {
                                    if (item["Medical Record Number"].ToString() == "" || string.IsNullOrEmpty(item["Medical Record Number"].ToString()))
                                    {
                                        break;
                                    }

                                    PatientName = item["Patient First"].ToString();
                                    //'+","+ item["Patient Last"].ToString();
                                    SchedulePatientRequest schedulePatient = new SchedulePatientRequest();

                                    schedulePatient.MedicalId = item["Medical Record Number"].ToString();
                                    schedulePatient.CaregiverName = item["Provider First"].ToString() + " " + item["Provider Last"].ToString() + " " + item["Task Type"].ToString();
                                    string InsertedUserID = ConfigurationManager.AppSettings["AutomatedAppointementUserIdFromIndia"].ToString();

                                    schedulePatient.InsertUserId = InsertedUserID.ToString();
                                    Task.Run(async () => { schedulePatient = GetPatientRequestDetailByMedicalIDsWithCareGiverName(schedulePatient.InsertUserId, schedulePatient.MedicalId, schedulePatient.CaregiverName); }).Wait();

                                    schedulePatient.MaxCaregiver = Convert.ToInt32(100);
                                    schedulePatient.Street = schedulePatient.Street;
                                    schedulePatient.ServiceNames = schedulePatient.ServiceNames;

                                    schedulePatient.TimezoneOffset = Convert.ToInt32(schedulePatient.TimezoneOffset);
                                    schedulePatient.City = schedulePatient.City;
                                    schedulePatient.Address = schedulePatient.Address;

                                    schedulePatient.TimezoneId = schedulePatient.TimezoneId;
                                    schedulePatient.IsCancelled = Convert.ToBoolean(false);
                                    schedulePatient.State = schedulePatient.State;
                                    schedulePatient.TimezonePostfix = schedulePatient.TimezonePostfix;
                                    schedulePatient.FromTime = "02:00:00";

                                    schedulePatient.PatientRequestId = schedulePatient.PatientRequestId;
                                    //schedulePatient.PatientName = item["Patient"].ToString();

                                    schedulePatient.ToTime = "20:00:00";
                                    schedulePatient.Date = item["Target Date"].ToString();
                                    schedulePatient.Office = schedulePatient.Office;

                                    schedulePatient.ZipCode = schedulePatient.ZipCode;
                                    schedulePatient.Description = "";
                                    schedulePatient.VisitTypeNames = item["Task Name"].ToString();
                                    schedulePatient.RepeatEvery = "";
                                    schedulePatient.RepeatTypeID = "";

                                    if (!string.IsNullOrEmpty(schedulePatient.Latitude) && !string.IsNullOrEmpty(schedulePatient.Longitude))
                                    {
                                        schedulePatient.Latitude = schedulePatient.Latitude;
                                        schedulePatient.Longitude = schedulePatient.Longitude;
                                    }
                                    else
                                    {
                                        //  FullAddress = schedulePatient.Street + ',' + schedulePatient.City + ',' + schedulePatient.State + ',' + schedulePatient.ZipCode;
                                        // if (Convert.ToString(item["MRN"]) == Convert.ToString(schedulePatient.MedicalId))
                                        //   if(String.Equals(item["MRN"].ToString(), schedulePatient.MedicalId))
                                        if (string.Compare(item["Medical Record Number"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0)
                                        {
                                            FullAddress = schedulePatient.Address + "," + schedulePatient.ZipCode;

                                            var requestUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address=key{0}&key={1}", FullAddress, "AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw");
                                            using (var client = new WebClient())
                                            {
                                                var result1 = client.DownloadString(requestUrl);
                                                var data1 = JsonConvert.DeserializeObject<JObject>(result1);

                                                var Latitude = data1["results"][0]["geometry"]["location"]["lat"];
                                                var Longitude = data1["results"][0]["geometry"]["location"]["lng"];

                                                schedulePatient.Latitude = Convert.ToString(Latitude);
                                                schedulePatient.Longitude = Convert.ToString(Longitude);
                                            }
                                        }
                                    }

                                    if (worksheet.Cells[1, 25].Value.ToString() == "Comments")
                                    {
                                        if (schedulePatient.PatientName.Contains(PatientName) && string.Compare(item["Medical Record Number"].ToString().Trim().ToLower(), schedulePatient.MedicalId.ToString().Trim().ToLower()) == 0 && schedulePatient.MedicalId != "" && schedulePatient.Caregiver != "" && schedulePatient.Caregiver != null && schedulePatient.MedicalId != null && !string.IsNullOrEmpty(schedulePatient.Caregiver) && !string.IsNullOrEmpty(schedulePatient.MedicalId))
                                        {
                                            // schedulePatient.PatientName = item["Patient"].ToString();
                                            finalresult = await AddPatientScheduleRequestInfo(schedulePatient);

                                            worksheet.Cells[y, 25].Value = finalresult;
                                            worksheet.Cells[y, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            worksheet.Cells[y, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                            worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                            worksheet.Cells[y, 25].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                            worksheet.Column(25).Style.Font.Bold = true;

                                            item["Comments"] = finalresult;
                                            Scheduled = Scheduled + 1;
                                            y = y + 1;

                                        }
                                        else
                                        {
                                            if (item["Medical Record Number"].ToString().Trim().ToLower() != schedulePatient.MedicalId.ToString().Trim().ToLower() && schedulePatient.MedicalId == null && string.IsNullOrEmpty(schedulePatient.MedicalId))
                                            {

                                                worksheet.Cells[y, 25].Value = "ERROR: MedicalId(MRN) Not Found On Paseva";
                                                worksheet.Cells[y, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                worksheet.Cells[y, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                worksheet.Cells[y, 25].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                worksheet.Column(25).Style.Font.Bold = true;

                                                item["Comments"] = "ERROR: MedicalId(MRN) Not Found On Paseva";
                                                error = error + 1;
                                                y = y + 1;

                                            }
                                            else
                                            {
                                                if (schedulePatient.Caregiver == null && string.IsNullOrEmpty(schedulePatient.Caregiver))
                                                {
                                                    worksheet.Cells[y, 25].Value = "ERROR: Employee Not Found On Paseva";
                                                    worksheet.Cells[y, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                    worksheet.Cells[y, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                    worksheet.Cells[y, 25].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                    worksheet.Column(25).Style.Font.Bold = true;

                                                    item["Comments"] = "ERROR: Employee Not Found On Paseva";
                                                    error = error + 1;
                                                    y = y + 1;

                                                }
                                                else
                                                {
                                                    if (!schedulePatient.PatientName.Contains(PatientName))
                                                    {

                                                        worksheet.Cells[y, 25].Value = "ERROR: Patient & MedicalId(MRN) Data Mismatch Problem On Paseva";
                                                        worksheet.Cells[y, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                        worksheet.Cells[y, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                        worksheet.Cells[y, 25].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                        worksheet.Column(25).Style.Font.Bold = true;

                                                        item["Comments"] = "Patient & MedicalId(MRN) Mismatch Problem On Paseva";
                                                        error = error + 1;
                                                        y = y + 1;
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[y, 25].Value = "ERROR: Fail";
                                                        worksheet.Cells[y, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                        worksheet.Cells[y, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                        worksheet.Cells[y, 25].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                        worksheet.Column(25).Style.Font.Bold = true;

                                                        item["Comments"] = "ERROR: Fail";
                                                        error = error + 1;
                                                        y = y + 1;
                                                    }
                                                }
                                            }
                                        }
                                        package.Save();
                                    }


                                    // replace occurences
                                    //if (worksheet.Cells[1, 7].Value.ToString() == "Comments")
                                    //{
                                    //    if (finalresult == "Success")
                                    //            {

                                    //        worksheet.Cells[y, 7].Value = finalresult;
                                    //        worksheet.Cells[y, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //        worksheet.Cells[y, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                    //        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    //        worksheet.Cells[y, 7].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                    //        worksheet.Column(7).Style.Font.Bold = true;

                                    //               item["Comments"] = finalresult;
                                    //                 Scheduled = Scheduled + 1;
                                    //                 y = y + 1;
                                    //            }
                                    //            else
                                    //            {
                                    //    worksheet.Cells[y, 7].Value = "Fail";
                                    //    worksheet.Cells[y, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //    worksheet.Cells[y, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                    //    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    //    worksheet.Cells[y, 7].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                    //    worksheet.Column(7).Style.Font.Bold = true;

                                    //    item["Comments"] = "Fail";
                                    //                error = error + 1;
                                    //                y = y + 1;
                                    //            }

                                    //     }    

                                }

                                if (SendUpdatedExcelToEmail(filePath2, FileName, Scheduled, error)) ;
                                {
                                    //return RedirectToAction("PatientRequest", "PatientRequest");
                                    myStringWebResource = remoteUri + FileName;

                                    return Json(myStringWebResource, JsonRequestBehavior.AllowGet);

                                }
                                //exportToExcel(dt,Scheduled,error);
                            }
                        }
                    }
                }
            }

            return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
            //   return RedirectToAction("PatientRequest", "PatientRequest");

        }



        private void CopyDataTable(DataSet ds)
        {
            // Create an object variable for the copy.
            //  DataTable copyDataTable = new DataTable();

            // copyDataTable = table.Copy();

            //    copyDataTable.Columns.Add(new DataColumn("Done", typeof(string)));
            //  DataRow dr = copyDataTable.NewRow();
            //  dr["Done"] = result;
            // for (int i = i; i < copyDataTable.Rows.Count) ;
            // copyDataTable.Rows.Add(dr);
            // Insert code to work with the copy.




            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=EmployeeonHoldList.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);

                    Response.Flush();
                    Response.End();
                }
            }



            //  return copyDataTable;
        }



        private void exportToExcel(DataTable dt, int Scheduled, int Error)
        {
            // insertdata();


            string FileName = "AppointmentScheduleDat_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx", myStringWebResource = null;


            /*Set up work book, work sheets, and excel application*/
            Microsoft.Office.Interop.Excel.Application oexcel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                //  insertdata();

                string path = AppDomain.CurrentDomain.BaseDirectory;
                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Workbook obook = oexcel.Workbooks.Add(misValue);
                Microsoft.Office.Interop.Excel.Worksheet osheet = new Microsoft.Office.Interop.Excel.Worksheet();


                //  obook.Worksheets.Add(misValue);

                osheet = (Microsoft.Office.Interop.Excel.Worksheet)obook.Sheets["Sheet1"];
                int colIndex = 0;
                int rowIndex = 1;

                foreach (DataColumn dc in dt.Columns)
                {
                    colIndex++;
                    osheet.Cells[1, colIndex] = dc.ColumnName;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    rowIndex++;
                    colIndex = 0;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        colIndex++;
                        osheet.Cells[rowIndex, colIndex] = dr[dc.ColumnName];
                    }
                }

                osheet.Columns.AutoFit();

                string filepath = ConfigurationManager.AppSettings["LocalBulkScheduling"].ToString() + "\\" + FileName;

                //Release and terminate excel

                obook.SaveAs(filepath);
                obook.Close();
                oexcel.Quit();
                // releaseObject(osheet);

                //  releaseObject(obook);

                // releaseObject(oexcel);

                string remoteUri = ConfigurationManager.AppSettings["BulkScheduling"].ToString();
                //string fileName = "ms-banner.gif", 
                string msg = string.Empty;
                string AttachmentFileName = filepath;
                string subject = "Paseva Schedule" + " " + DateTime.Now.ToString("MM.dd.yyyy").ToString();

                bool IsFileAttachment = true;

                msg += "<b>Scheduled:</b> " + Scheduled + "<br>";
                msg += "<b>Error:</b> " + Error + "<br><br>";

                msg += "Warm Regards,<br>";
                msg += "Team PaSeva Automation";

                string body = msg;
                string Result1 = "";

                string toAddress = ConfigurationManager.AppSettings["AppointmentMailScheduling"].ToString();
                //string CCMailID = ConfigurationManager.AppSettings["AppointmentCCMailScheduling"].ToString();
                string CCMailID = "";

                bool isBodyHtml = true;

                if (sendEmailWithAttachment(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
                {
                    // Create a new WebClient instance.
                    using (WebClient myWebClient = new WebClient())
                    {
                        //myStringWebResource = remoteUri + FileName;
                        ////  Download the Web resource and save it into the current filesystem folder.
                        //myWebClient.DownloadFile(myStringWebResource, FileName);
                    }

                    //  GC.Collect();
                }
            }
            catch (Exception ex)
            {
                oexcel.Quit();
                // log.AddToErrorLog(ex, this.Name);
            }

            // return myStringWebResource;
        }


        public bool SendUpdatedExcelToEmail(string filepath, string FileName, int Scheduled, int Error)
        {
            // string filepaths = ConfigurationManager.AppSettings["LocalBulkScheduling"].ToString() + "\\" + FileName;

            string remoteUri = ConfigurationManager.AppSettings["BulkScheduling"].ToString(), myStringWebResource = "";
            //string fileName = "ms-banner.gif", 
            string msg = string.Empty;
            string AttachmentFileName = filepath;
            string subject = "Paseva Schedule" + " " + DateTime.Now.ToString("MM.dd.yyyy").ToString();

            bool IsFileAttachment = true;

            msg += "<b>Scheduled:</b> " + Scheduled + "<br>";
            msg += "<b>Error:</b> " + Error + "<br><br>";

            msg += "Warm Regards,<br>";
            msg += "Team PaSeva Automation";

            string body = msg;

            string Result1 = "";

            string toAddress = ConfigurationManager.AppSettings["AppointmentMailScheduling"].ToString();
            string CCMailID = ConfigurationManager.AppSettings["AppointmentCCMailScheduling"].ToString();

            bool isBodyHtml = true;

            if (sendEmailWithAttachment(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
            {
                // Create a new WebClient instance.
                //using (WebClient myWebClient = new WebClient())
                //{
                //    myStringWebResource = remoteUri + FileName;
                //    // Download the Web resource and save it into the current filesystem folder.
                //    myWebClient.DownloadFile(myStringWebResource, FileName);
                //}
                return true;
            }
            return false;
        }


        public SchedulePatientRequest GetPatientRequestDetailByMedicalIDs(string LoginUserId, string MedicalID, string CareGiver, string ProcedureCode)
        {
            SchedulePatientRequest objSchedulePatientRequest = new SchedulePatientRequest();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientRequestDetailByMedicalIDWithLatLong", LoginUserId, MedicalID, CareGiver);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objSchedulePatientRequest.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRequestId"]);
                    objSchedulePatientRequest.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objSchedulePatientRequest.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    objSchedulePatientRequest.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objSchedulePatientRequest.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objSchedulePatientRequest.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objSchedulePatientRequest.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objSchedulePatientRequest.MedicalId = ds.Tables[0].Rows[0]["MedicalId"].ToString();
                    objSchedulePatientRequest.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    objSchedulePatientRequest.InsertUserId = LoginUserId;
                    //objSchedulePatientRequest.InsertDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy");
                    //objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //objSchedulePatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    // objSchedulePatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    objSchedulePatientRequest.Caregiver = ds.Tables[1].Rows[0]["NurseId"].ToString();
                    objSchedulePatientRequest.ServiceNames = ds.Tables[2].Rows[0]["ServiceNames"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Latitude"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Longitude"].ToString()))
                    {
                        objSchedulePatientRequest.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                        objSchedulePatientRequest.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    }
                    else
                    {
                        objSchedulePatientRequest.Latitude = ds.Tables[0].Rows[0]["Latitudes"].ToString();
                        objSchedulePatientRequest.Longitude = ds.Tables[0].Rows[0]["Longitudes"].ToString();
                    }


                    objSchedulePatientRequest.OfficeName = ds.Tables[0].Rows[0]["Office"].ToString();
                    objSchedulePatientRequest.Office = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneId"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString()))
                    {
                        objSchedulePatientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneId"].ToString();
                        objSchedulePatientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString());
                        objSchedulePatientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfix"].ToString();
                        // objSchedulePatientRequest.OfficeAddress = ds.Tables[0].Rows[0]["OfficeAddress"].ToString();
                    }
                    else
                    {
                        objSchedulePatientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneIds"].ToString();
                        objSchedulePatientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffsets"].ToString());
                        objSchedulePatientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfixs"].ToString();
                    }

                    if (!string.IsNullOrEmpty(ProcedureCode))
                    {
                        objSchedulePatientRequest.PayerId = ds.Tables[0].Rows[0]["PayerId"].ToString();
                        objSchedulePatientRequest.PayerProgram = ds.Tables[0].Rows[0]["PayerProgram"].ToString();

                        objSchedulePatientRequest.ProcedureCode = ds.Tables[0].Rows[0]["ProcedureCode"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(code => code.Trim()).Where(code => !string.IsNullOrEmpty(code) && code==ProcedureCode).FirstOrDefault();

                        objSchedulePatientRequest.JurisdictionCode = ds.Tables[0].Rows[0]["JurisdictionCode"].ToString();
                    }
		
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiversDetailsByUserId";
                // string result = InsertErrorLog(objErrorlog);
                return objSchedulePatientRequest;
            }
            return objSchedulePatientRequest;
        }



        public SchedulePatientRequest GetPatientRequestDetailByMedicalIDsWithCareGiverName(string LoginUserId, string MedicalID, string CareGiver)
        {
            SchedulePatientRequest objSchedulePatientRequest = new SchedulePatientRequest();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientRequestDetailByMedicalIDWithLatLongLikeCaregiver", LoginUserId, MedicalID, CareGiver);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objSchedulePatientRequest.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRequestId"]);
                    objSchedulePatientRequest.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objSchedulePatientRequest.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    objSchedulePatientRequest.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objSchedulePatientRequest.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objSchedulePatientRequest.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objSchedulePatientRequest.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objSchedulePatientRequest.MedicalId = ds.Tables[0].Rows[0]["MedicalId"].ToString();
                    objSchedulePatientRequest.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    objSchedulePatientRequest.InsertUserId = LoginUserId;
                    //objSchedulePatientRequest.InsertDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy");
                    //objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //objSchedulePatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    // objSchedulePatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    objSchedulePatientRequest.Caregiver = ds.Tables[1].Rows[0]["NurseId"].ToString();
                    objSchedulePatientRequest.ServiceNames = ds.Tables[2].Rows[0]["ServiceNames"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Latitude"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Longitude"].ToString()))
                    {
                        objSchedulePatientRequest.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                        objSchedulePatientRequest.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    }
                    else
                    {
                        objSchedulePatientRequest.Latitude = ds.Tables[0].Rows[0]["Latitudes"].ToString();
                        objSchedulePatientRequest.Longitude = ds.Tables[0].Rows[0]["Longitudes"].ToString();
                    }


                    objSchedulePatientRequest.OfficeName = ds.Tables[0].Rows[0]["Office"].ToString();
                    objSchedulePatientRequest.Office = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneId"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString()))
                    {
                        objSchedulePatientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneId"].ToString();
                        objSchedulePatientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString());
                        objSchedulePatientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfix"].ToString();
                        // objSchedulePatientRequest.OfficeAddress = ds.Tables[0].Rows[0]["OfficeAddress"].ToString();
                    }
                    else
                    {
                        objSchedulePatientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneIds"].ToString();
                        objSchedulePatientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffsets"].ToString());
                        objSchedulePatientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfixs"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiversDetailsByUserId";
                // string result = InsertErrorLog(objErrorlog);
                return objSchedulePatientRequest;
            }
            return objSchedulePatientRequest;
        }


        private bool sendEmailWithAttachment(string toAddress, string subject, string body, bool IsFileAttachment, string AttachmentFileName, string CCMailID, bool isBodyHtml = true)
        {
            try
            {

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var mailMessage = new MailMessage();
                // MailAddress bccAddress = new MailAddress(ConfigurationManager.AppSettings["BCCEMAILADDRESS"]);

                mailMessage.To.Add(toAddress);

                // mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"]);
                if (!(string.IsNullOrEmpty(CCMailID)))
                {
                    mailMessage.CC.Add(CCMailID);
                }

                //  MailAddress ma = new MailAddress("pramendrasingh400@gmail.com", "singhtripty");
                // MailAddress ma = new MailAddress("pramendrasingh400@gmail.com");

                MailAddress ma = new MailAddress(ConfigurationManager.AppSettings["AppointmentOutLookMail"].ToString());
                mailMessage.From = ma;

                mailMessage.Subject = subject;

                //LinkedResource Signature = null;

                if (IsFileAttachment == true)
                {
                    if (!string.IsNullOrEmpty(AttachmentFileName))
                    {
                        Attachment attachFile = new Attachment(AttachmentFileName);
                        mailMessage.Attachments.Add(attachFile);
                    }
                }

                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                var smtpClient = new SmtpClient { EnableSsl = false };
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Host = ConfigurationManager.AppSettings["SMTPHost"];

                //smtpClient.Host ="smtp.live.com";

                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.UseDefaultCredentials = false;

                // mailMessage.Bcc.Add(bccAddress);
                // smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                // smtpClient.Credentials = new NetworkCredential("pksingh@solifetec.com", "Password22");

                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["AppointmentOutLookMail"].ToString(), ConfigurationManager.AppSettings["AppointmentOutlookPassword"].ToString());
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                //  ErrorLog objErrorlog = new ErrorLog();
                //  objErrorlog.Errormessage = ex.Message;
                //  objErrorlog.StackTrace = ex.StackTrace;
                //  objErrorlog.Pagename = "CareGiverSuperAdminService";
                //  objErrorlog.Methodname = "sendEmail";
                ////  string result = InsertErrorLog(objErrorlog);
                return false;

            }
            return false;
        }



        //private void insertdata()
        //{
        //    string result = "Testing";
        //    try
        //    {
        //        int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "insertdatatocheck", result);

        //        if (i > 0)
        //        {
        //            result = "Success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog objErrorlog = new ErrorLog();
        //        //objErrorlog.Errormessage = ex.Message;
        //        //objErrorlog.StackTrace = ex.StackTrace;
        //        //objErrorlog.Pagename = "CareGiverLiteService";
        //        //objErrorlog.Methodname = "InsertScheduleForNurse";
        //        //objErrorlog.UserID = CareGiverSchedule.UserId;
        //        //result = InsertErrorLog(objErrorlog);
        //    }
        //  //  return result;
        //}


        public ActionResult PatientRequestManualEntry()
        {
            return View();
        }
        //List<PatientRequestMaualEntry>
        public JsonResult GetAllPatientRequestManualEntry(JQueryDataTableParamModel param)
        {
            RootResponseManualRequest Response = new RootResponseManualRequest();
            List<PatientRequestMaualEntry> lstManualRequest = new List<PatientRequestMaualEntry>();
            int TotalCount = 0;
            int FilteredRecord = 0;

            string LogInUserId = "";
            string FilterStatus = "||";

            try
            {
                LogInUserId = Membership.GetUser().ProviderUserKey.ToString();

                // UserId = GetUserIDFromAccessToken();
                if (LogInUserId == "" || LogInUserId == null)
                {
                    throw new Exception("UserId is NULL");
                }

                //int FilterOfficeId = 0;

                //if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                //    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "PatientName";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "NurseName";
                }
                else if (sortColumnIndex == 2)
                {
                    //sortOrder = "OfficeName";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "InsertedDatetime";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "AppointmentDate";
                }

                if (Request["FilterStatus"] != null && Request["FilterStatus"] != "")
                {
                    FilterStatus = Request["FilterStatus"];

                    if (FilterStatus == "All")
                    {
                        FilterStatus = "||";
                    }
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

                MembershipUser user = Membership.GetUser(new Guid(LogInUserId));
                string[] role = Roles.GetRolesForUser(user.UserName);


                #region Admin(Office Manager)
                if (role[0] == "Admin" || role[0] == "SuperAdmin" || role[0] == "OrgSuperAdmin")
                {
                   

                        // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetALLPatientRequestManualEntryData_PasevaWeb",

                        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetALLPatientRequestManualEntryData_PasevaWeb",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortOrder,
                                                        sortDirection,
                                                        search,
                                                        FilterStatus);

                    if (ds != null)
                    {
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //ds.Tables.[0].Rpws[0]["OfficeName"]

                            foreach (DataRow item in ds.Tables[0].Rows)
                            {


                                PatientRequestMaualEntry objManualEntryRequest = new PatientRequestMaualEntry();
                                objManualEntryRequest.PatientRequestManualEntryId = item["ManualEntryId"].ToString();
                                objManualEntryRequest.PatientRequestId = item["PatientRequestId"].ToString();
                                objManualEntryRequest.PatientRequestTempId = item["PatientRequestTempId"].ToString();
                                objManualEntryRequest.NurseId = item["NurseId"].ToString();
                                objManualEntryRequest.StartDrivingDateTime = item["StartDrivingDateTime"].ToString();
                                objManualEntryRequest.StopDrivingDateTime = item["StopDrivingDateTime"].ToString();
                                objManualEntryRequest.CheckInDateTime = item["CheckInDateTime"].ToString();
                                objManualEntryRequest.CheckOutDateTime = item["CheckOutDateTime"].ToString();
                                objManualEntryRequest.ReasonForManualEntry = item["ReasonForManualEntry"].ToString();
                                objManualEntryRequest.Status = item["ManualStatus"].ToString();
                                objManualEntryRequest.Comment = "";
                                // objManualEntryRequest.InsertedDateTime =item["InsertedDateTime"].ToString();
                                objManualEntryRequest.InsertedDateTime = item["InsertedDateTime"].ToString();


                                objManualEntryRequest.Description = item["Description"].ToString();
                                objManualEntryRequest.NurseName = item["NurseName"].ToString();
                                objManualEntryRequest.OfficeId = item["OfficeId"].ToString();
                                objManualEntryRequest.OfficeName = item["officeName"].ToString();
                                objManualEntryRequest.PatientName = item["patientName"].ToString();
                                objManualEntryRequest.AppointmentDate = item["AppointmentDate"].ToString();
                                objManualEntryRequest.Miles = item["Miles"].ToString();
                                lstManualRequest.Add(objManualEntryRequest);
                            }
                            TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                            FilteredRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

                            //Response.Data = lstManualRequest;
                            //Response.Message = "Data Available";
                            //Response.Success = 1;
                            //lstManualRequest = lstManualRequest;
                            //  TempData["Message"] = "Data Available";
                            //  TempData["error"] = "False";
                            Response.Message = "Data Available";
                            Response.Success = 1;

                            //Session["OfficeName"] = objNurseCoordinator.IsAllowForPatientChatRoom;
                        }
                        else
                        {
                            // TempData["error"] = "True";
                            // TempData["Message"] = "No ManualRequest Form Data Available";
                            Response.Message = "No ManualRequest Form Data Available";
                            Response.Success = 0;
                        }
                    }
                }
                #endregion
                else
                {
                    Response.Message = "Process denied !! User Not Authorised. ";
                    Response.Success = 0;

                }


            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteWebServices";
                objErrorlog.Methodname = "GetAllPatientRequestManualEntry";
                //    result = InsertErrorLog(objErrorlog);
                Response.Message = ex.Message;
                Response.Success = 0;

            }

            if (lstManualRequest != null)
            {
                var result = from C in lstManualRequest select new[] { C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = TotalCount,
                    iTotalDisplayRecords = FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = TotalCount,
                    iTotalDisplayRecords = FilteredRecord,
                }, JsonRequestBehavior.AllowGet);
            }


        }


        public ActionResult ShowManualEntryDetail(string ManualEntryId)
        {
            ManualEntryModel objModel = new ManualEntryModel();
            string LogInUserId = "";
            try
            {
                LogInUserId = Membership.GetUser().ProviderUserKey.ToString();
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientRequestManualEntryData_ById_PasevaWeb",
                                                    LogInUserId,
                                                    ManualEntryId);

                if (ds != null)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        objModel.PatientRequestManualEntryId = ds.Tables[0].Rows[0]["ManualEntryId"].ToString();
                        objModel.PatientRequestId = ds.Tables[0].Rows[0]["PatientRequestId"].ToString();
                        objModel.PatientRequestTempId = ds.Tables[0].Rows[0]["PatientRequestTempId"].ToString();
                        objModel.NurseId = ds.Tables[0].Rows[0]["NurseId"].ToString();
                        objModel.StartDrivingDateTime = ds.Tables[0].Rows[0]["StartDrivingDateTime"].ToString();
                        objModel.StopDrivingDateTime = ds.Tables[0].Rows[0]["StopDrivingDateTime"].ToString();
                        objModel.CheckInDateTime = ds.Tables[0].Rows[0]["CheckInDateTime"].ToString();
                        objModel.CheckOutDateTime = ds.Tables[0].Rows[0]["CheckOutDateTime"].ToString();
                        objModel.ReasonForManualEntry = ds.Tables[0].Rows[0]["ReasonForManualEntry"].ToString();
                        objModel.Status = ds.Tables[0].Rows[0]["ManualStatus"].ToString();
                        objModel.Comment = "";
                        objModel.InsertedDateTime = ds.Tables[0].Rows[0]["InsertedDateTime"].ToString();

                        ViewBag.Status = ds.Tables[0].Rows[0]["ManualStatus"].ToString();
                        objModel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                        objModel.NurseName = ds.Tables[0].Rows[0]["NurseName"].ToString();
                        objModel.OfficeId = ds.Tables[0].Rows[0]["OfficeId"].ToString();
                        objModel.OfficeName = ds.Tables[0].Rows[0]["officeName"].ToString();
                        objModel.PatientName = ds.Tables[0].Rows[0]["patientName"].ToString();
                        objModel.AppointmentDate = ds.Tables[0].Rows[0]["AppointmentDate"].ToString();
                        objModel.Miles = ds.Tables[0].Rows[0]["Miles"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PatientSignature"].ToString()) && ds.Tables[0].Rows[0]["PatientSignature"].ToString().EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                        {
                            string BaseImagePath = ConfigurationManager.AppSettings["SignatureImageDownload"].ToString();
                            objModel.ImagePath = Path.Combine(BaseImagePath, ds.Tables[0].Rows[0]["PatientSignature"].ToString());
                        }

                        ViewBag.NurseName = ds.Tables[0].Rows[0]["NurseName"].ToString();

                        ViewBag.PatientRequestManualEntryId = ds.Tables[0].Rows[0]["ManualEntryId"].ToString();

                        if (Convert.ToInt32(Session["OrganisationId"]) > 0)
                        {
                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CheckoutFormReason"].ToString()))
                            {
                                objModel.CheckoutFormReason = ds.Tables[0].Rows[0]["CheckoutFormReason"].ToString();
                                AttendanceManagementController managementController = new AttendanceManagementController();
                                ReasonForCompleteRequests response = managementController.GetAllReasonForCompleteRequests();

                                string[] Chekoutlist = objModel.CheckoutFormReason.Split(';');
                                HashSet<string> checkoutSet = new HashSet<string>();

                                for (int x = 0; x <= Chekoutlist.Length - 1; x++)
                                {
                                    checkoutSet.Add(Chekoutlist[x].TrimStart());
                                }

                                objModel.ADLs = new List<string>();
                                foreach (var adl in response.ADLs)
                                {
                                    if (checkoutSet.Contains(adl.CompleteRequestsReason))
                                    {
                                        objModel.ADLs.Add(adl.CompleteRequestsReason);
                                    }
                                }
                                objModel.IADLs = new List<string>();
                                foreach (var iadl in response.IADLs)
                                {
                                    if (checkoutSet.Contains(iadl.CompleteRequestsReason))
                                    {
                                        objModel.IADLs.Add(iadl.CompleteRequestsReason);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "EditPatientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }


        public ActionResult ShowManualDetails(string ManualEntryId)
        {
            ManualEntryModel objModel = new ManualEntryModel();
            string LogInUserId = "";
            try
            {
                LogInUserId = Membership.GetUser().ProviderUserKey.ToString();
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientRequestManualEntryData_ById_PasevaWeb",
                                                    LogInUserId,
                                                    ManualEntryId);

                if (ds != null)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        objModel.PatientRequestManualEntryId = ds.Tables[0].Rows[0]["ManualEntryId"].ToString();
                        objModel.PatientRequestId = ds.Tables[0].Rows[0]["PatientRequestId"].ToString();
                        objModel.PatientRequestTempId = ds.Tables[0].Rows[0]["PatientRequestTempId"].ToString();
                        objModel.NurseId = ds.Tables[0].Rows[0]["NurseId"].ToString();
                        objModel.StartDrivingDateTime = ds.Tables[0].Rows[0]["StartDrivingDateTime"].ToString();
                        objModel.StopDrivingDateTime = ds.Tables[0].Rows[0]["StopDrivingDateTime"].ToString();
                        objModel.CheckInDateTime = ds.Tables[0].Rows[0]["CheckInDateTime"].ToString();
                        objModel.CheckOutDateTime = ds.Tables[0].Rows[0]["CheckOutDateTime"].ToString();
                        objModel.ReasonForManualEntry = ds.Tables[0].Rows[0]["ReasonForManualEntry"].ToString();
                        objModel.Status = ds.Tables[0].Rows[0]["ManualStatus"].ToString();
                        objModel.Comment = "";
                        objModel.InsertedDateTime = ds.Tables[0].Rows[0]["InsertedDateTime"].ToString();

                        ViewBag.Status = ds.Tables[0].Rows[0]["ManualStatus"].ToString();
                        objModel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                        objModel.NurseName = ds.Tables[0].Rows[0]["NurseName"].ToString();
                        objModel.OfficeId = ds.Tables[0].Rows[0]["OfficeId"].ToString();
                        objModel.OfficeName = ds.Tables[0].Rows[0]["officeName"].ToString();
                        objModel.PatientName = ds.Tables[0].Rows[0]["patientName"].ToString();
                        objModel.AppointmentDate = ds.Tables[0].Rows[0]["AppointmentDate"].ToString();
                        objModel.Miles = ds.Tables[0].Rows[0]["Miles"].ToString();
                        ViewBag.NurseName = ds.Tables[0].Rows[0]["NurseName"].ToString();
                        ViewBag.PatientRequestManualEntryId = ds.Tables[0].Rows[0]["ManualEntryId"].ToString();


                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PatientSignature"].ToString()) && ds.Tables[0].Rows[0]["PatientSignature"].ToString().EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                        {
                            string BaseImagePath = ConfigurationManager.AppSettings["SignatureImageDownload"].ToString();
                            objModel.ImagePath = Path.Combine(BaseImagePath, ds.Tables[0].Rows[0]["PatientSignature"].ToString());
                        }


                        if (Convert.ToInt32(Session["OrganisationId"]) > 0)
                        {
                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CheckoutFormReason"].ToString()))
                            {
                                objModel.CheckoutFormReason = ds.Tables[0].Rows[0]["CheckoutFormReason"].ToString();
                                AttendanceManagementController managementController = new AttendanceManagementController();
                                ReasonForCompleteRequests response = managementController.GetAllReasonForCompleteRequests();

                                string[] Chekoutlist = objModel.CheckoutFormReason.Split(';');
                                HashSet<string> checkoutSet = new HashSet<string>();

                                for (int x = 0; x <= Chekoutlist.Length - 1; x++)
                                {
                                    checkoutSet.Add(Chekoutlist[x].TrimStart());
                                }

                                objModel.ADLs = new List<string>();
                                foreach (var adl in response.ADLs)
                                {
                                    if (checkoutSet.Contains(adl.CompleteRequestsReason))
                                    {
                                        objModel.ADLs.Add(adl.CompleteRequestsReason);
                                    }
                                }
                                objModel.IADLs = new List<string>();
                                foreach (var iadl in response.IADLs)
                                {
                                    if (checkoutSet.Contains(iadl.CompleteRequestsReason))
                                    {
                                        objModel.IADLs.Add(iadl.CompleteRequestsReason);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "EditPatientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }



        public JsonResult PatientRequestManualEntryApproval(PatientRequestMaualEntryApproval PatientRequestApproval)
        {
            RootResponseManualRequest Response = new RootResponseManualRequest();
            string result = "";
            string UserId = "";
            string ApprovalStatus = "";
            Dictionary<string, object> res = new Dictionary<string, object>();
            int OrganisationId= Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                UserId = Membership.GetUser().ProviderUserKey.ToString();
                if (UserId == "" || UserId == null)
                {
                    throw new Exception("UserId is NULL");
                }
                if (PatientRequestApproval.ApprovalStatus == "" || PatientRequestApproval.ApprovalStatus == null)
                {
                    throw new Exception("Approval process is Null");
                }

                if (PatientRequestApproval.ApprovalStatus == "A")
                {
                    ApprovalStatus = "Approved";
                }
                else if (PatientRequestApproval.ApprovalStatus == "R")
                {
                    ApprovalStatus = "Rejected";
                }
                else
                {
                    throw new Exception("Incorrect approval process");
                }

                MembershipUser user = Membership.GetUser(new Guid(UserId));
                string[] role = Roles.GetRolesForUser(user.UserName);

                #region Admin(Office Manager)

                if (role[0] == "Admin" || role[0] == "SuperAdmin" || role[0] == "OrgSuperAdmin")
                {

                    //Admin objAdmin = GetAdminDetailByUserId(Membership.GetUser(user.UserName).ProviderUserKey.ToString());
                    //if (string.IsNullOrEmpty(objAdmin.Name))
                    //{
                    //    throw new Exception("User Name is Null");
                    //}


                    int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "PatientRequestManualEntryApproval",
                                                         PatientRequestApproval.PatientRequestManualEntryId,
                                                         UserId,
                                                         ApprovalStatus,
                                                         PatientRequestApproval.Comment);

                    if (i > 0)
                    {


                        if (PatientRequestApproval.ApprovalStatus == "A")
                        {

                            if (OrganisationId > 0 && OrganisationId == 3)
                            {
                                string visitRequest = GetAndPostEmployeeVisitDataAllMed(Convert.ToInt32(PatientRequestApproval.PatientRequestManualEntryId));
                            }

                            res["Success"] = true;
                            res["Message"] = "Visit Approved Successfully";
                        }

                        if (PatientRequestApproval.ApprovalStatus == "R")
                        {
                            res["Success"] = true;
                            res["Message"] = "Visit Rejected Successfully";
                        }
                    }
                    else
                    {
                        res["Success"] = false;
                        res["Message"] = "Visit Approval process failed ";

                    }
                }
                else
                {
                    res["Success"] = false;
                    res["Message"] = "Process denied !! User Not Authorised. ";

                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteMobileServices";
                objErrorlog.Methodname = "PatientRequestManualEntryApproval";
                //result = InsertErrorLog(objErrorlog);

                res["Success"] = false;
                res["Message"] = ex.Message;

            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public bool checkGroupName(string MedicalId)
        {
            try
            {
                //string result = "";

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "checkGroupName", MedicalId);

                if (ds.Tables[0].Rows[0][0].ToString() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "checkDialogId";
                // string result = InsertErrorLog(objErrorlog);
            }
            return false;
        }


        [HttpGet]
        public ActionResult ExcelReport()
        {
            return PartialView("ExcelReport");
        }

        [HttpPost]
        public ActionResult ExcelReport(AttendanceManagementDetail objAttendanceManagementDetails)
        {
            try
            {

                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAttendanceDetails_Vin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", objAttendanceManagementDetails.FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", objAttendanceManagementDetails.ToDate);
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //da.Fill(ds);
                        //List<AttendanceManagementDetails> AttendanceList = new List<AttendanceManagementDetails>();
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        //    AttendanceManagementDetails uobj = new AttendanceManagementDetails();
                        //    uobj.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        //    uobj.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        //    uobj.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        //    uobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        //    uobj.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        //    uobj.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();                           

                        //    uobj.TotalTravelTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();


                        //    AttendanceList.Add(uobj);
                        //}

                        //ViewBag.AttendanceDetailList = AttendanceList;
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult GenerateReportAction(string FromDate, string ToDate, int OfficeId)
        {

            string result = "";
            var ObjStatus = "";

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))

                {

                    using (SqlCommand cmd = new SqlCommand("GenerateMilageReportNotCompletedRequest", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);


                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            // var ObjStatus = MilageReportGenerate(ds);
                            //return Json(ObjStatus);  

                            //Json(MilageReportGenerate(ds));
                            ObjStatus = MilageReportGenerate(ds);
                            return Json(ObjStatus, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            ObjStatus = "";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = "Fail";
                string msg = e.Message;
                throw (e);
            }

            return Json(ObjStatus, JsonRequestBehavior.AllowGet);

        }

        private string MilageReportGenerate(DataSet ds)
        {
            var ObjStatus = "";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

                string ExcelUploadPath = ConfigurationManager.AppSettings["ExportNotCompletedRequest"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["DownlLoadNotCompletedRequest"].ToString();

                var fileName = "ReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";

                var caseFile = ExcelUploadPath + "\\" + fileName;

                // Response.Write(caseFile); 


                FileInfo newFile = new FileInfo(caseFile);


                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {

                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

                        ReportDataSheet.Cells["A0:I1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:I1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

                        ReportDataSheet.Cells[1, 1].Value = "NurseName";
                        ReportDataSheet.Cells[1, 2].Value = "PatientName";
                        // ReportDataSheet.Cells[1, 3].Value = "Payroll-Id";
                        ReportDataSheet.Cells[1, 3].Value = "Nurse Address";
                        ReportDataSheet.Cells[1, 4].Value = "Patient Address";
                        ReportDataSheet.Cells[1, 5].Value = "Date";
                        //ReportDataSheet.Cells[1, 6].Value = "DrivingStartTime";
                        //ReportDataSheet.Cells[1, 7].Value = "DrivingStartLatitude";
                        //ReportDataSheet.Cells[1, 8].Value = "DrivingStartLongitude";
                        //ReportDataSheet.Cells[1, 9].Value = "DrivingStopTime";
                        //ReportDataSheet.Cells[1, 10].Value = "DrivingStopLatitude";
                        //ReportDataSheet.Cells[1, 11].Value = "DrivingStopLongitude";
                        ReportDataSheet.Cells[1, 6].Value = "Status";
                        //ReportDataSheet.Cells[1, 13].Value = "Manual Submission";
                        //ReportDataSheet.Cells[1, 14].Value = "TotalTravelTime";
                        //ReportDataSheet.Cells[1, 15].Value = "TotalDistance";
                        //ReportDataSheet.Cells[1, 16].Value = "ShortestDistance";
                        //ReportDataSheet.Cells[1, 17].Value = "CheckInDateTime";
                        //ReportDataSheet.Cells[1, 18].Value = "CheckOutDateTime";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalCheckInTime";
                        ReportDataSheet.Cells[1, 7].Value = "Office";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        //ReportDataSheet.Cells[1, 21].Value = "GoogleShortestTime";
                        // ReportDataSheet.Cells[1, 21].Value = "Pause & Resume";
                        ReportDataSheet.Cells[1, 8].Value = "Request Type";
                        ReportDataSheet.Cells[1, 9].Value = "Visit Type";
                        ReportDataSheet.Cells[1, 10].Value = "NurseId";
                        //ReportDataSheet.Cells[1, 25].Value = "Total Driving Time";

                        ReportDataSheet.Column(1).Width = 40;
                        ReportDataSheet.Column(2).Width = 40;
                        ReportDataSheet.Column(3).Width = 50;
                        ReportDataSheet.Column(4).Width = 40;
                        ReportDataSheet.Column(5).Width = 40;
                        ReportDataSheet.Column(6).Width = 40;
                        ReportDataSheet.Column(7).Width = 30;
                        ReportDataSheet.Column(8).Width = 40;
                        ReportDataSheet.Column(9).Width = 40;
                        ReportDataSheet.Column(10).Width = 40;

                        //ReportDataSheet.Column(10).Width = 40;
                        //ReportDataSheet.Column(11).Width = 40;
                        //ReportDataSheet.Column(12).Width = 30;
                        //ReportDataSheet.Column(13).Width = 40;
                        //ReportDataSheet.Column(14).Width = 40;
                        //ReportDataSheet.Column(15).Width = 40;
                        //ReportDataSheet.Column(16).Width = 40;
                        //ReportDataSheet.Column(17).Width = 40;
                        //ReportDataSheet.Column(18).Width = 40;
                        //ReportDataSheet.Column(19).Width = 40;
                        //ReportDataSheet.Column(20).Width = 40;
                        //ReportDataSheet.Column(21).Width = 60;
                        //ReportDataSheet.Column(22).Width = 20;
                        //ReportDataSheet.Column(23).Width = 50;
                        // ReportDataSheet.Column(24).Width = 40;
                        //ReportDataSheet.Column(24).Width = 40;

                        int RowNumber = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RowNumber = (RowNumber + 1);

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"];
                            //  ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["AxessId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["NurseAddress"].ToString();
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Address"];
                            ReportDataSheet.Cells[RowNumber, 5].Value = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateOfVisit"]).ToString("MM-dd-yyyy");

                            //ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["Status"].ToString();

                            //if (ds.Tables[0].Rows[i]["ismanual"].ToString() == "1")
                            //{
                            //    ReportDataSheet.Cells[RowNumber, 13].Value = "true";
                            //    ReportDataSheet.Cells[RowNumber, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            //    ReportDataSheet.Cells[RowNumber, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            //    ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            //    ReportDataSheet.Cells[RowNumber, 13].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                            //    ReportDataSheet.Column(13).Style.Font.Bold = true;
                            //}
                            //else
                            //{
                            //    ReportDataSheet.Cells[RowNumber, 13].Value = " ";
                            //    ReportDataSheet.Cells[RowNumber, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            //    ReportDataSheet.Cells[RowNumber, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            //    ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            //    ReportDataSheet.Cells[RowNumber, 13].Style.Border.Top.Color.SetColor(System.Drawing.Color.White);
                            //    ReportDataSheet.Column(13).Style.Font.Bold = true;
                            //}

                            //string totalchekin = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
                            //string totaltrTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                            //if (totalchekin != "" && totaltrTime != "")
                            //{
                            //    string[] abcd = totalchekin.Split(':');
                            //    string a = abcd[0];
                            //    string b = abcd[1];
                            //    string c = abcd[2];
                            //    string d = abcd[3];
                            //    totalchekin = a + "Days" + b + ":" + c + ":" + d;
                            //    string[] defg = totaltrTime.Split(':');
                            //    string e = defg[0];
                            //    string f = defg[1];
                            //    string g = defg[2];
                            //    string h = defg[3];

                            //    totaltraveltime = e + "Days" + f + ":" + g + ":" + h;
                            //    int ab = Convert.ToInt32(a) + Convert.ToInt32(e);
                            //    int bc = Convert.ToInt32(b) + Convert.ToInt32(f);
                            //    int cd = Convert.ToInt32(c) + Convert.ToInt32(g);
                            //    int de = Convert.ToInt32(d) + Convert.ToInt32(h);
                            //    Totalworkingtime = Convert.ToString(ab) + "Days" + Convert.ToString(bc) + ':' + Convert.ToString(cd) + ':' + Convert.ToString(de);

                            //}

                            //  ReportDataSheet.Cells[RowNumber, 14].Value = totaltrTime;
                            //ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                            //  ReportDataSheet.Cells[RowNumber, 11].Value = totalTravel(Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]));
                            //ReportDataSheet.Cells[RowNumber, 12].Value = totalTravel(11923);
                            //ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 18].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();

                            //  ReportDataSheet.Cells[RowNumber, 19].Value = totalchekin;
                            //ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();

                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                            //if (Totalworkingtime != "")
                            //{
                            //    ReportDataSheet.Cells[RowNumber, 19].Value = Totalworkingtime;
                            //}
                            // ReportDataSheet.Cells[RowNumber, 21].Value = ds.Tables[0].Rows[i]["GoogleShortestTime"].ToString();
                            // ReportDataSheet.Cells[RowNumber, 21].Value = ds.Tables[0].Rows[i]["PAUSE_RESUME"].ToString();

                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["NurseId"].ToString();

                            //ReportDataSheet.Cells[RowNumber, 25].Value = ds.Tables[0].Rows[i]["DrivingTotalTime"].ToString();

                            //var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

                            //ReportDataSheet.Cells["A0:S1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            //ReportDataSheet.Cells["A0:S1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                            //ReportDataSheet.Row(1).Style.Font.Bold = true;
                            //ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

                            //ReportDataSheet.Cells[1, 1].Value = "NurseName";
                            //ReportDataSheet.Cells[1, 2].Value = "PatientName";
                            //ReportDataSheet.Cells[1, 3].Value = "Nurse Address";
                            //ReportDataSheet.Cells[1, 4].Value = "Patient Address";
                            //ReportDataSheet.Cells[1, 5].Value = "DrivingStartTime";
                            //ReportDataSheet.Cells[1, 6].Value = "DrivingStartLatitude";
                            //ReportDataSheet.Cells[1, 7].Value = "DrivingStartLongitude";
                            //ReportDataSheet.Cells[1, 8].Value = "DrivingStopTime";
                            //ReportDataSheet.Cells[1, 9].Value = "DrivingStopLatitude";
                            //ReportDataSheet.Cells[1, 10].Value = "DrivingStopLongitude";
                            //ReportDataSheet.Cells[1, 11].Value = "Status";
                            //ReportDataSheet.Cells[1, 12].Value = "TotalTravelTime";
                            //ReportDataSheet.Cells[1, 13].Value = "TotalDistance";
                            //ReportDataSheet.Cells[1, 14].Value = "ShortestDistance";
                            //ReportDataSheet.Cells[1, 15].Value = "CheckInDateTime";
                            //ReportDataSheet.Cells[1, 16].Value = "CheckOutDateTime";
                            //ReportDataSheet.Cells[1, 17].Value = "TotalCheckInTime";
                            //ReportDataSheet.Cells[1, 18].Value = "Office";
                            ////  ReportDataSheet.Cells[1, 19].Value = "Pause & Resume";

                            //ReportDataSheet.Column(1).Width = 40;
                            //ReportDataSheet.Column(2).Width = 40;
                            //ReportDataSheet.Column(3).Width = 50;
                            //ReportDataSheet.Column(4).Width = 40;
                            //ReportDataSheet.Column(5).Width = 40;
                            //ReportDataSheet.Column(6).Width = 40;
                            //ReportDataSheet.Column(7).Width = 30;
                            //ReportDataSheet.Column(8).Width = 40;
                            //ReportDataSheet.Column(9).Width = 40;
                            //ReportDataSheet.Column(10).Width = 40;

                            //ReportDataSheet.Column(11).Width = 40;
                            //ReportDataSheet.Column(12).Width = 30;
                            //ReportDataSheet.Column(13).Width = 40;
                            //ReportDataSheet.Column(14).Width = 40;
                            //ReportDataSheet.Column(15).Width = 40;
                            //ReportDataSheet.Column(16).Width = 40;
                            //ReportDataSheet.Column(17).Width = 40;
                            //ReportDataSheet.Column(18).Width = 40;
                            ////  ReportDataSheet.Column(19).Width = 60;

                            //int RowNumber = 1;
                            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                            //{
                            //    RowNumber = (RowNumber + 1);

                            //    ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"];
                            //    ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["NurseAddress"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Address"];
                            //    ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                            //    ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["Status"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                            //    //  ReportDataSheet.Cells[RowNumber, 11].Value = totalTravel(Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]));
                            //    //ReportDataSheet.Cells[RowNumber, 12].Value = totalTravel(11923);
                            //    ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
                            //    ReportDataSheet.Cells[RowNumber, 18].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                            //  ReportDataSheet.Cells[RowNumber, 19].Value = ds.Tables[0].Rows[i]["PAUSE_RESUME"].ToString();

                            //ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        }

                        ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        ReportDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                        excelPackage.SaveAs(newFile);
                        // excelPackage.Save(); 

                        string ExcelUrl = ExcelPath + fileName;
                        ObjStatus = ExcelUrl;
                        return ObjStatus;
                    }
                }

                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else
            {
                ObjStatus = "";

            }

            return ObjStatus;
        }

        public ActionResult PatientRequestManualEntryCompleteApproval()
        {
            return View();
        }

        //List<PatientRequestMaualEntry>
        public JsonResult GetAllRequestCompleteApproval(JQueryDataTableParamModel param)
        {
            RootResponseManualRequest Response = new RootResponseManualRequest();
            List<PatientRequestMaualEntry> lstManualRequest = new List<PatientRequestMaualEntry>();
            int TotalCount = 0;
            int FilteredRecord = 0;

            string LogInUserId = "";
            string FilterStatus = "||";

            try
            {
                LogInUserId = Membership.GetUser().ProviderUserKey.ToString();

                //if (LogInUserId == "" || LogInUserId == null)
                //{
                //    throw new Exception("UserId is NULL");
                //}

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "PatientName";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "NurseName";
                }
                else if (sortColumnIndex == 2)
                {
                    //sortOrder = "OfficeName";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "Date";
                }

                //if (Request["FilterStatus"] != null && Request["FilterStatus"] != "")
                //{
                //    FilterStatus = Request["FilterStatus"];

                //    if (FilterStatus == "All")
                //    {
                //        FilterStatus = "||";
                //    }
                //}

                //  FilterStatus = "||";

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

                MembershipUser user = Membership.GetUser(new Guid(LogInUserId));
                string[] role = Roles.GetRolesForUser(user.UserName);

                #region Admin(Office Manager)
                if (role[0] == "Admin" || role[0] == "SuperAdmin" || role[0] == "OrgSuperAdmin")
                {
                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllApprovalForCompleteRequests",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortOrder,
                                                        sortDirection,
                                                        search,
                                                        FilterStatus);


                    if (ds != null)
                    {
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //ds.Tables.[0].Rpws[0]["OfficeName"]
                            int i = 0;
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {


                                PatientRequestMaualEntry objManualEntryRequest = new PatientRequestMaualEntry();
                                //  objManualEntryRequest.PatientRequestManualEntryId = item["ManualEntryId"].ToString();
                                objManualEntryRequest.NurseScheduleId = Convert.ToInt32(item["NurseScheduleId"]);
                                //objManualEntryRequest.PatientRequestTempId = item["PatientRequestTempId"].ToString();
                                //objManualEntryRequest.NurseId = item["NurseId"].ToString();
                                // objManualEntryRequest.StartDrivingDateTime = item["StartDrivingDateTime"].ToString();
                                //objManualEntryRequest.StopDrivingDateTime = item["StopDrivingDateTime"].ToString();
                                objManualEntryRequest.CheckInDateTime = item["CheckInDateTime"].ToString();
                                objManualEntryRequest.CheckOutDateTime = item["CheckOutDateTime"].ToString();
                                //objManualEntryRequest.ReasonForManualEntry = item["ReasonForManualEntry"].ToString();
                                // objManualEntryRequest.Status = item["ManualStatus"].ToString();
                                //objManualEntryRequest.Comment = "";

                                //  objManualEntryRequest.InsertedDateTime = item["InsertedDateTime"].ToString();
                                // objManualEntryRequest.Description = item["Description"].ToString();
                                objManualEntryRequest.NurseName = item["Name"].ToString();
                                //objManualEntryRequest.OfficeId = item["OfficeId"].ToString();
                                objManualEntryRequest.OfficeName = item["OfficeName"].ToString();
                                objManualEntryRequest.PatientName = item["PatientName"].ToString();
                                objManualEntryRequest.AppointmentDate = item["Date"].ToString();
                                //objManualEntryRequest.Miles = item["Miles"].ToString();
                                objManualEntryRequest.IsResolved = Convert.ToInt32(item["IsResolve"]);

                                lstManualRequest.Add(objManualEntryRequest);
                            }

                            TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                            FilteredRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

                            Response.Message = "Data Available";
                            Response.Success = 1;
                        }
                        else
                        {

                            Response.Message = "No ManualRequest Form Data Available";
                            Response.Success = 0;
                        }
                    }
                }
                #endregion
                else
                {
                    Response.Message = "Process denied !! User Not Authorised. ";
                    Response.Success = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteWebServices";
                objErrorlog.Methodname = "GetAllPatientRequestManualEntry_CompletApproval";
                //    result = InsertErrorLog(objErrorlog);

                Response.Message = ex.Message;
                Response.Success = 0;
            }

            if (lstManualRequest != null)
            {
                var result = from C in lstManualRequest select new[] { C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = TotalCount,
                    iTotalDisplayRecords = FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = TotalCount,
                    iTotalDisplayRecords = FilteredRecord,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public void insertdata(string data)
        {
            string result = "Testing for new request";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "insertdatatocheck", data);

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "InsertScheduleForNurse";
                //objErrorlog.UserID = CareGiverSchedule.UserId;
                //result = InsertErrorLog(objErrorlog);
            }
            //  return result;
        }

        public ActionResult ShowManualEntryCompleteApprovalDetail(string NurseScheduleId)
        {
            ManualEntryModel objModel = new ManualEntryModel();

            try
            {
                // LogInUserId = Membership.GetUser().ProviderUserKey.ToString();
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllResonForCompleterequestsDetails",
                                                    NurseScheduleId);

                if (ds != null)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        objModel.NurseScheduleId = ds.Tables[0].Rows[0]["NurseScheduleId"].ToString();
                        // objModel.PatientRequestId = ds.Tables[0].Rows[0]["PatientRequestId"].ToString();
                        // objModel.PatientRequestTempId = ds.Tables[0].Rows[0]["PatientRequestTempId"].ToString();
                        //  objModel.NurseId = ds.Tables[0].Rows[0]["NurseId"].ToString();
                        objModel.StartDrivingDateTime = ds.Tables[0].Rows[0]["DrivingStartTime"].ToString();
                        objModel.StopDrivingDateTime = ds.Tables[0].Rows[0]["DrivingStopTime"].ToString();
                        objModel.CheckInDateTime = ds.Tables[0].Rows[0]["CheckInDateTime"].ToString();
                        objModel.CheckOutDateTime = ds.Tables[0].Rows[0]["CheckoutDateTime"].ToString();

                        objModel.ReasonForManualEntry = ds.Tables[0].Rows[0]["reason"].ToString();

                        //objModel.Status = ds.Tables[0].Rows[0]["ManualStatus"].ToString();
                        objModel.ReasonForManualEntry = ds.Tables[0].Rows[0]["AddititonalComment"].ToString();
                        objModel.InsertedDateTime = ds.Tables[0].Rows[0]["InsertDateTime"].ToString();
                        ViewBag.Status = "";// ds.Tables[0].Rows[0]["ManualStatus"].ToString();
                                            // objModel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                        objModel.NurseName = ds.Tables[0].Rows[0]["Name"].ToString();
                        //  objModel.OfficeId = ds.Tables[0].Rows[0]["OfficeId"].ToString();
                        objModel.OfficeName = ds.Tables[0].Rows[0]["OfficeName"].ToString();
                        objModel.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                        //  objModel.AppointmentDate = ds.Tables[0].Rows[0]["AppointmentDate"].ToString();
                        objModel.Miles = ds.Tables[0].Rows[0]["DrivingTotalDistance"].ToString();
                        ViewBag.NurseName = objModel.NurseName;

                        ViewBag.NurseScheduleId = objModel.NurseScheduleId;
                        ViewBag.IsResolved = Convert.ToInt32(ds.Tables[0].Rows[0]["IsResolve"]);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "EditPatientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }

        public JsonResult PatientRequestManualEntryCompleteApprovals(PatientRequestMaualEntryApproval PatientRequestApproval)
        {
            RootResponseManualRequest Response = new RootResponseManualRequest();
            int IsResolve = 0;
            string UserId = "";
            string ApprovalStatus = "";
            Dictionary<string, object> res = new Dictionary<string, object>();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                UserId = Membership.GetUser().ProviderUserKey.ToString();
                if (UserId == "" || UserId == null)
                {
                    throw new Exception("UserId is NULL");
                }
                if (PatientRequestApproval.ApprovalStatus == "" || PatientRequestApproval.ApprovalStatus == null)
                {
                    throw new Exception("Approval process is Null");
                }

                if (PatientRequestApproval.ApprovalStatus == "A")
                {
                    ApprovalStatus = "Approved";
                    IsResolve = 1;
                }
                else if (PatientRequestApproval.ApprovalStatus == "R")
                {
                    ApprovalStatus = "Rejected";
                }
                else
                {
                    throw new Exception("Incorrect approval process");
                }

                MembershipUser user = Membership.GetUser(new Guid(UserId));
                string[] role = Roles.GetRolesForUser(user.UserName);

                #region Admin(Office Manager)

                if (role[0] == "Admin" || role[0] == "SuperAdmin" || role[0] == "OrgSuperAdmin")
                {

                    int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateApprovalForCompleteRequests",
                                                         PatientRequestApproval.NurseScheduleId,
                                                          PatientRequestApproval.Comment,
                                                          IsResolve,
                                                          UserId);

                    if (i > 0)
                    {


                        if (PatientRequestApproval.ApprovalStatus == "A")
                        {

                            res["Success"] = true;
                            res["Message"] = "Visit Approved Successfully";
                        }
                        if (PatientRequestApproval.ApprovalStatus == "R")
                        {
                            res["Success"] = true;
                            res["Message"] = "Visit Rejected Successfully";
                        }
                    }
                    else
                    {
                        res["Success"] = false;
                        res["Message"] = " Visit Approval process failed ";
                    }
                }
                else
                {
                    res["Success"] = false;
                    res["Message"] = "Process denied !! User Not Authorised. ";
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteMobileServices";
                objErrorlog.Methodname = "PatientRequestManualEntryApproval";
                //result = InsertErrorLog(objErrorlog);

                res["Success"] = false;
                res["Message"] = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public string GetAndPostEmployeeVisitDataAllMed(int PatientRequestManualEntryId)
        {

            Dictionary<string, object> res = new Dictionary<string, object>();
            string result = "";
            string EmployeeSSN = "00001";

            #region
            //string ss = "24-Nov-22 11:51:0";
            //DateTime dd1 = Convert.ToDateTime(ss);
            //string foo1 = dd1.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

            //DateTime fromDatetime = Convert.ToDateTime(FromDate);
            ////string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            //string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd'");

            //DateTime ToDatetime = Convert.ToDateTime(ToDate);
            ////string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");
            //string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd'");

            // EmployeeClientModel objModel = new EmployeeClientModel();

            #endregion


            List<ClientVisitRequest> clientVisitRequestList = new List<ClientVisitRequest>();
            try
            {
                string SandDataOfficeId = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeVisitRequestToaddSandDataForApprovedRequestAllMed",
                                                                                                          PatientRequestManualEntryId
                                                                                                         );


                if (ds != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {

                            if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                            {

                                ClientVisitRequest clientVisitRequest = new ClientVisitRequest();
                                ClientVisitRequestProviderIdentification objprovider = new ClientVisitRequestProviderIdentification();
                                objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDMed"].ToString();
                                objprovider.ProviderQualifier = "NPI";
                                clientVisitRequest.ProviderIdentification = objprovider;

                                clientVisitRequest.EmployeeQualifier = "EmployeeSSN";
  
                                clientVisitRequest.EmployeeOtherID = Convert.ToString(item["EEID"]);
                                clientVisitRequest.EmployeeIdentifier = EmployeeSSN + Convert.ToString(item["NurseId"]);

                                clientVisitRequest.VisitOtherID = Convert.ToString(item["NurseScheduleId"]);
                                clientVisitRequest.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
                                clientVisitRequest.GroupCode = null;
                                clientVisitRequest.ClientIDQualifier = "ClientMedicaidID";


                                string ChkMedicalId = string.Empty;
                                string strChkMedical = Convert.ToString(item["MedicalId"]).ToString();
                                string strChkMedicalFInal = string.Empty;

                                clientVisitRequest.ClientID = Convert.ToString(item["MedicalId"]).ToString();
                                clientVisitRequest.ClientIdentifier = clientVisitRequest.ClientID;

                               
                                clientVisitRequest.ClientOtherID = Convert.ToString(item["PrimaryMD"]); //Convert.ToString(item["MedicalId"]);
                                //clientVisitRequest.ContingencyPlan = "CP01";

                                string str3 = Convert.ToString(item["PrimaryMD"]);
                                string str4 = string.Empty;
                                int val = 0;

                                for (int i = 0; i < str3.Length; i++)
                                {
                                    if (Char.IsLetterOrDigit(str3[i]))
                                        str4 += str3[i];
                                }
                                if (str4.Length > 0)
                                {
                                    clientVisitRequest.ClientOtherID = str4.ToString();
                                }


                                clientVisitRequest.VisitCancelledIndicator = "False";


                                if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                                {
                                    clientVisitRequest.PayerID = Convert.ToString(item["PayerId"]);
                                }
                                //else
                                //{
                                //    clientVisitRequest.PayerID = "CADDS";
                                //}

                                if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
                                {
                                    clientVisitRequest.PayerProgram = Convert.ToString(item["PayerProgram"]);
                                }
                                //else
                                //{
                                //    clientVisitRequest.PayerProgram = "PCS"; 
                                //}

                                string procedureArrayCode = Convert.ToString(item["ProcedureCode"]).ToString();
                                string ProcedureCodeReuse = "";

                                if (procedureArrayCode.Contains(','))
                                {

                                    int ProcedureData = Convert.ToString(item["ProcedureCode"]).Split(',').Length;
                                    string[] ProcedureArray = Convert.ToString(item["ProcedureCode"]).Split(',');

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        ProcedureCodeReuse = Convert.ToString(ProcedureArray[0]);
                                        clientVisitRequest.ProcedureCode = ProcedureCodeReuse;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        ProcedureCodeReuse = Convert.ToString(item["ProcedureCode"]).ToString();
                                        clientVisitRequest.ProcedureCode = ProcedureCodeReuse;
                                    }

                                }
                                //else
                                //{
                                //    clientVisitRequest.ProcedureCode = "Z9081";
                                //}

                                clientVisitRequest.VisitTimeZone = "US/Eastern";
                                // clientVisitRequest.ScheduleStartTime = Convert.ToString((Convert.ToDateTime(item["FromTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                // clientVisitRequest.ScheduleEndTime = Convert.ToString((Convert.ToDateTime(item["ToTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                clientVisitRequest.Reschedule = "No";
                                clientVisitRequest.AdjInDateTime = null;// Convert.ToString(item["InsertDateTime"]);
                                clientVisitRequest.AdjOutDateTime = null;// Convert.ToString(item["InsertDateTime"]); 
                                clientVisitRequest.BillVisit = "true";
                                clientVisitRequest.HoursToBill = "1";
                                clientVisitRequest.HoursToPay = "1";
                                clientVisitRequest.Memo = "This is a memo!";
                                clientVisitRequest.ClientVerifiedTimes = "true";
                                clientVisitRequest.ClientVerifiedTasks = "true";
                                clientVisitRequest.ClientVerifiedService = "true";
                                clientVisitRequest.ClientSignatureAvailable = "true";
                                clientVisitRequest.ClientVoiceRecording = "true";
                                //clientVisitRequest.Modifier1 = null;
                                //clientVisitRequest.Modifier2 = null;
                                //clientVisitRequest.Modifier3 = null;
                                //clientVisitRequest.Modifier4 = null;

                                // ClientVisitRequestCalls clientVisitRequestCalls = new ClientVisitRequestCalls();
                                List<ClientVisitRequestCalls> clientVisitRequestCallsList = new List<ClientVisitRequestCalls>();
                                {
                                    ClientVisitRequestCalls clientVisitRequestCalls = new ClientVisitRequestCalls();
                                    clientVisitRequestCalls.CallExternalID = Convert.ToString(item["PatientRequestId"]) + (Convert.ToString(item["NurseId"]));
                                    clientVisitRequestCalls.CallDateTime = Convert.ToString((Convert.ToDateTime(item["CheckInDateTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                    clientVisitRequestCalls.CallAssignment = "Time In";
                                    clientVisitRequestCalls.GroupCode = null;
                                    clientVisitRequestCalls.CallType = "Mobile";

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        clientVisitRequestCalls.ProcedureCode = ProcedureCodeReuse;
                                    }
                                    //else
                                    //{
                                    //    clientVisitRequestCalls.ProcedureCode = "Z9081"; 
                                    //}

                                    clientVisitRequestCalls.ClientIdentifierOnCall = clientVisitRequest.ClientID;
                                    clientVisitRequestCalls.MobileLogin = Convert.ToString(item["username"]);
                                    clientVisitRequestCalls.CallLatitude = Convert.ToString(item["Latitude"]);
                                    clientVisitRequestCalls.CallLongitude = Convert.ToString(item["Longitude"]);
                                    //clientVisitRequestCalls.Location = null;
                                    clientVisitRequestCalls.TelephonyPIN = null;
                                    clientVisitRequestCalls.OriginatingPhoneNumber = null;
                                    clientVisitRequestCalls.VisitLocationType = "1";
                                    clientVisitRequestCallsList.Add(clientVisitRequestCalls);
                                }

                                {
                                    ClientVisitRequestCalls clientVisitRequestCallss = new ClientVisitRequestCalls();
                                    clientVisitRequestCallss.CallExternalID = Convert.ToString(item["PatientRequestId"]) + (Convert.ToString(item["NurseId"])) + "1";
                                    clientVisitRequestCallss.CallDateTime = Convert.ToString((Convert.ToDateTime(item["CheckOutDateTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                    clientVisitRequestCallss.CallAssignment = "Time Out";
                                    clientVisitRequestCallss.GroupCode = null;
                                    clientVisitRequestCallss.CallType = "Mobile";

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        clientVisitRequestCallss.ProcedureCode = ProcedureCodeReuse;
                                    }
                                    //else
                                    //{
                                    //    clientVisitRequestCalls.ProcedureCode = "Z9081"; 
                                    //}

                                    clientVisitRequestCallss.ClientIdentifierOnCall = clientVisitRequest.ClientID;
                                    clientVisitRequestCallss.MobileLogin = Convert.ToString(item["username"]);
                                    clientVisitRequestCallss.CallLatitude = Convert.ToString(item["Latitude"]);
                                    clientVisitRequestCallss.CallLongitude = Convert.ToString(item["Longitude"]);
                                    // clientVisitRequestCallss.Location = null;
                                    clientVisitRequestCallss.TelephonyPIN = null;
                                    clientVisitRequestCallss.OriginatingPhoneNumber = null;
                                    clientVisitRequestCallss.VisitLocationType = "1";
                                    clientVisitRequestCallsList.Add(clientVisitRequestCallss);
                                }


                                clientVisitRequest.Calls = clientVisitRequestCallsList;
                                clientVisitRequestList.Add(clientVisitRequest);
                            }
                            else
                            {
                                return result = "";
                            }
                        }
                    }
                    else
                    {
                        return result = "";
                    }
                }
                else
                {
                    return result = "";
                }
                if (clientVisitRequestList.Count <= 0)
                {
                    throw new Exception("No data Available to send");
                }

                var arraylist = clientVisitRequestList.ToArray();
                List<ClientVisitRequest> request = new List<ClientVisitRequest>();

                foreach (var ReqItem in arraylist)
                {
                    request.Add(ReqItem);
                }
                string x = JsonConvert.SerializeObject(request);
                Task.Run(async () => { result = await SubmitEmployeeVisitRequestDataAllMed(x); }).Wait();
                //  res["Success"] = true;
                // res["Result"] = result;
            }


            catch (Exception ex)
            {
                //res["Success"] = false;
                //res["Result"] = ex.Message;
                result = ex.Message;
            }
            // return Json(res, JsonRequestBehavior.AllowGet);
            return result;
        }


        //public async Task<string> SubmitEmployeeVisitRequestDataAllMed(string request)
        //{

        //    string result = "";
        //    string result1 = "";
        //    string finalresult = "";

        //    string datajson = request;
        //    try
        //    {

        //        string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetailsMed"].ToString();
        //        string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetailsStatusMed"].ToString();

        //        var clientGetDialogId = new System.Net.Http.HttpClient();
        //        string Token = ConfigurationManager.AppSettings["TokenMed"].ToString();
        //        string actheader = ConfigurationManager.AppSettings["mykeyMed"].ToString();

        //        // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1");

        //        clientGetDialogId.BaseAddress = new Uri(API_Url);

        //        clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
        //        clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
        //        clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
        //        var content2 = new StringContent(request, Encoding.UTF8, "application/json");
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //        var response2 = await clientGetDialogId.PostAsync("", content2);
        //        result = response2.Content.ReadAsStringAsync().Result;

        //        var data1 = (JObject)JsonConvert.DeserializeObject(result);
        //        string UDID = data1["id"].Value<string>();

        //        insertVisitDetails(UDID, UDID, result, request);

        //        Thread.Sleep(10000);

        //        var clientGetId = new System.Net.Http.HttpClient();

        //        //clientGetId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1/status?uuid=" + UDID);

        //        clientGetId.BaseAddress = new Uri(API_Url_Status + UDID);

        //        clientGetId.DefaultRequestHeaders.Accept.Clear();
        //        clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
        //        clientGetId.DefaultRequestHeaders.Add("account", actheader);
        //        // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //        var response1 = await clientGetId.GetAsync("");

        //        result1 = response1.Content.ReadAsStringAsync().Result;

        //        var data2 = (JObject)JsonConvert.DeserializeObject(result);
        //        //  finalresult = data2["data"]["message"].Value<string>();

        //        insertVisitDetails(UDID, UDID, result1, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        insertVisitDetails("VisitDetailsData", ex.Message, result, "");
        //        //  throw new Exception(ex.Message);
        //    }

        //    return finalresult;

        //}


        private void insertVisitDetails(String Id, string UDID, string result, string request)
        {
            //  string result = "Testing";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertVisitDetails", Id, UDID, result, request);

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                //ErrorLog objErrorlog = new ErrorLog();
                //objErrorlog.Errormessage = ex.Message;
                //objErrorlog.StackTrace = ex.StackTrace;
                //objErrorlog.Pagename = "CareGiverLiteService";
                //objErrorlog.Methodname = "InsertScheduleForNurse";
                //objErrorlog.UserID = CareGiverSchedule.UserId;
                //result = InsertErrorLog(objErrorlog);
            }
            //  return result;
        }

        [HttpGet]
        public ActionResult GenerateManualReport()
        {
            return PartialView();
        }

        public string ExportGenerateManualReport(string FromDate, string ToDate)
        {

            var ObjStatus = "";

            string LogInUserId = Membership.GetUser().ProviderUserKey.ToString();

            //int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            int OfficeId = Convert.ToInt32(Session["OfficeId"]);

            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetManualPendingAvailableBetweenDates",
                   OfficeId,
                   FromDate,
                   ToDate
                  );


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

                string ExcelUploadPath = ConfigurationManager.AppSettings["ManaulExportedFilePath"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["ManualDownlLoadFilePath"].ToString();

                var fileName = "ManaulReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";

                var caseFile = ExcelUploadPath + "\\" + fileName;

                // Response.Write(caseFile); 

                FileInfo newFile = new FileInfo(caseFile);

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {
                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ManaulReportFromToDate");

                        ReportDataSheet.Cells["A0:M1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:M1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

                        ReportDataSheet.Cells[1, 1].Value = "Patient Name";
                        ReportDataSheet.Cells[1, 2].Value = "Caregiver Name";
                        ReportDataSheet.Cells[1, 3].Value = "Office";
                        ReportDataSheet.Cells[1, 4].Value = "Submission Date";

                        ReportDataSheet.Cells[1, 5].Value = "Appointment Date";

                        ReportDataSheet.Cells[1, 6].Value = "Status";
                        ReportDataSheet.Cells[1, 7].Value = "StartDriving DateTime";
                        ReportDataSheet.Cells[1, 8].Value = "StopDriving DateTime";
                        ReportDataSheet.Cells[1, 9].Value = "Total Traveling Distance";
                        ReportDataSheet.Cells[1, 10].Value = "Check-In DateTime";
                        ReportDataSheet.Cells[1, 11].Value = "Check-Out DateTime";
                        ReportDataSheet.Cells[1, 12].Value = "Reason of manual entry";
                        ReportDataSheet.Cells[1, 13].Value = "Comments";

                        ReportDataSheet.Column(1).Width = 40;
                        ReportDataSheet.Column(2).Width = 40;
                        ReportDataSheet.Column(3).Width = 50;
                        ReportDataSheet.Column(4).Width = 50;
                        ReportDataSheet.Column(5).Width = 40;
                        ReportDataSheet.Column(6).Width = 40;

                        ReportDataSheet.Column(7).Width = 40;
                        ReportDataSheet.Column(8).Width = 40;
                        ReportDataSheet.Column(9).Width = 50;
                        ReportDataSheet.Column(10).Width = 50;
                        ReportDataSheet.Column(11).Width = 40;
                        ReportDataSheet.Column(12).Width = 40;
                        ReportDataSheet.Column(13).Width = 40;

                        //ReportDataSheet.Column(6).Width = 40;
                        //ReportDataSheet.Column(7).Width = 30;
                        //ReportDataSheet.Column(8).Width = 40;

                        //ReportDataSheet.Column(9).Width = 40;
                        //ReportDataSheet.Column(10).Width = 40;

                        //ReportDataSheet.Column(11).Width = 40;
                        //ReportDataSheet.Column(12).Width = 30;

                        int RowNumber = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RowNumber = (RowNumber + 1);

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["PatientName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["CareGiverName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["Office"];
                            ReportDataSheet.Cells[RowNumber, 4].Value = Convert.ToDateTime(ds.Tables[0].Rows[i]["InsertedDateTime"].ToString()).ToString("MM/dd/yyyy");

                            ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["AppointmentDate"].ToString();
                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["Status"].ToString();

                            //ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString(); ;
                            //ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();

                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["StartDrivingDateTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["StopDrivingDateTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["TotalTravelingDistance"];
                            ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();

                            ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["Reasonofmanualentry"].ToString();
                            ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["Comment"].ToString();

                        }

                        ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        ReportDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                        excelPackage.SaveAs(newFile);
                        //  excelPackage.Save(); 

                        string ExcelUrl = ExcelPath + fileName;
                        ObjStatus = ExcelUrl;

                        return ObjStatus;
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else
            {
                ObjStatus = "";
            }
            return ObjStatus;
        }

        public JsonResult GetPatientRequestDetailByFilter(string FilterBy, string FilterData)
        {

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            List<PatientDetailsModel> lstSchedulePatientRequest = new List<PatientDetailsModel>();
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientRequestDetailBYFilter",
                                                      FilterBy,
                                                      FilterData,
                                                      OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientDetailsModel objSchedulePatientRequest = new PatientDetailsModel();
                        objSchedulePatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objSchedulePatientRequest.MedicalId = ds.Tables[0].Rows[i]["MedicalId"].ToString();
                        objSchedulePatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();

                        lstSchedulePatientRequest.Add(objSchedulePatientRequest);
                    }
                    res["Success"] = true;
                    res["Result"] = lstSchedulePatientRequest;
                }
                else
                {
                    res["Success"] = false;
                    res["Result"] = lstSchedulePatientRequest;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "PatientRequest";
                objErrorlog.Methodname = "GetPatientRequestDetailByFilter";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllPatientRequestDetails(string PatientRequestId)
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            List<PatientRequestsDetailModel> listPatientsDetails = new List<PatientRequestsDetailModel>();

            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetPatientRequestDetailsByPatientRequestId", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PatientRequestID", PatientRequestId);
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PatientRequestsDetailModel uobj = new PatientRequestsDetailModel();
                            uobj.PatientrequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                            // PatientRequestid = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                            //uobj.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                            uobj.AppointmentDate = ds.Tables[0].Rows[i]["Date"].ToString();
                            uobj.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                            uobj.PatientAddress = ds.Tables[0].Rows[i]["Address"].ToString();
                            uobj.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                            uobj.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                            uobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                            uobj.VisitTypeName = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();
                            uobj.PatientID = ds.Tables[0].Rows[i]["Medicalid"].ToString();
                            uobj.Rating = ds.Tables[0].Rows[i]["Rating"].ToString();

                            uobj.ServiceType = ds.Tables[0].Rows[i]["ServiceName"].ToString();

                            if (!String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Description"].ToString()))
                            {
                                uobj.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                            }
                            else
                            {
                                uobj.Description = "";
                            }

                            uobj.CaregiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                            uobj.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                            uobj.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();                         
                            uobj.TrackMilage = Convert.ToBoolean(ds.Tables[0].Rows[i]["drivingEnabled"].ToString());

                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["payerId"].ToString()))
                            {
                                uobj.PayerId = ds.Tables[0].Rows[i]["payerId"].ToString();
                                uobj.PayerProgram = ds.Tables[0].Rows[i]["PayerProgram"].ToString();
                                uobj.ProcedureCode = ds.Tables[0].Rows[i]["ProcedureCode"].ToString();
                                uobj.JurisdictionCode = ds.Tables[0].Rows[i]["Jurisdictioncode"].ToString();
                            }

                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStopTime"].ToString())) /*&& !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()))*/
                            {
                                uobj.StartDrivingDatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStartTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                                uobj.StartDrivingDatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStopTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            }

                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString())) /*&& !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()))*/
                            {
                                uobj.CheckInDatetTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckInDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                                uobj.CheckOutDateTime = (ds.Tables[0].Rows[i]["CheckOutDateTime"]).ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckOutDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/') : "";


                            }

                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString()))
                            {
                                uobj.StartDrivingLattitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLatitude"]);
                                uobj.StopDrivinglatittude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLatitude"]);
                                uobj.StartDrivingLongitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLongitude"]);
                                uobj.stopDrivingLongitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLongitude"]);
                            }

                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInLatitude"].ToString()))
                            {
                                uobj.CheckinLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLatitude"]);
                                uobj.CheckinLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLongitude"]);

                                uobj.CheckoutLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLatitude"]);
                                uobj.CheckoutLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLongitude"]);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString())))
                            {
                                string BaseImagePath = ConfigurationManager.AppSettings["SignatureImageDownload"].ToString();
                                ViewBag.PatientSignature = Path.Combine(BaseImagePath, Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString()));
                            }

                            uobj.CaregiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                            uobj.AppointmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("yyyy-MM-dd");
                            uobj.IsManual = Convert.ToString(ds.Tables[0].Rows[i]["ismanual"]);

                            if (Convert.ToInt32(Session["OrganisationId"]) > 0)
                            {
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckoutFormReason"].ToString()))
                                {
                                    uobj.CheckoutFormReason = ds.Tables[0].Rows[i]["CheckoutFormReason"].ToString();
                                    AttendanceManagementController managementController = new AttendanceManagementController();
                                    ReasonForCompleteRequests response = managementController.GetAllReasonForCompleteRequests();

                                    string[] Chekoutlist = uobj.CheckoutFormReason.Split(';');
                                    HashSet<string> checkoutSet = new HashSet<string>();

                                    for (int x = 0; x <= Chekoutlist.Length - 1; x++)
                                    {
                                        checkoutSet.Add(Chekoutlist[x].TrimStart());
                                    }

                                    uobj.ADLs = new List<string>();
                                    foreach (var adl in response.ADLs)
                                    {
                                        if (checkoutSet.Contains(adl.CompleteRequestsReason))
                                        {
                                            uobj.ADLs.Add(adl.CompleteRequestsReason);
                                        }
                                    }
                                    uobj.IADLs = new List<string>();
                                    foreach (var iadl in response.IADLs)
                                    {
                                        if (checkoutSet.Contains(iadl.CompleteRequestsReason))
                                        {
                                            uobj.IADLs.Add(iadl.CompleteRequestsReason);
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString())))
                            {
                                string BaseImagePath = ConfigurationManager.AppSettings["SignatureImageDownload"].ToString();
                                uobj.PatientSignature = Path.Combine(BaseImagePath, Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString()));
                            }

                            uobj.TotalTravelTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                            listPatientsDetails.Add(uobj);
                        }

                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "PatientRequest";
                objErrorlog.Methodname = "GetAllPatientRequestDetails";

            }

            return Json(listPatientsDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPatienRequestData(string PatientRequestId)
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            
            PatientRequestsDetailModel uobj = new PatientRequestsDetailModel();
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetPatientRequestDetailsByPatientRequestId", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PatientRequestID", PatientRequestId);
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                           
                            uobj.PatientrequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();                           
                            uobj.AppointmentDate = ds.Tables[0].Rows[i]["Date"].ToString();
                            uobj.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                            uobj.ScheduledBy = ds.Tables[0].Rows[i]["FirstName"].ToString() +" "+ ds.Tables[0].Rows[i]["LastName"].ToString();
                            uobj.PatientAddress = ds.Tables[0].Rows[i]["Address"].ToString();
                            uobj.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                            uobj.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                            uobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                            uobj.VisitTypeName = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();
                            uobj.PatientID = ds.Tables[0].Rows[i]["Medicalid"].ToString();
                            uobj.Rating = ds.Tables[0].Rows[i]["Rating"].ToString(); 
                            uobj.ServiceType = ds.Tables[0].Rows[i]["ServiceName"].ToString();

                            if (!String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Description"].ToString()))
                            {
                                uobj.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                            }
                            else
                            {
                                uobj.Description = "";
                            }

                            uobj.CaregiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                            uobj.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                            uobj.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                            uobj.TrackMilage = Convert.ToBoolean(ds.Tables[0].Rows[i]["drivingEnabled"].ToString());

                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["payerId"].ToString()))
                            {
                                uobj.PayerId = ds.Tables[0].Rows[i]["payerId"].ToString();
                                uobj.PayerProgram = ds.Tables[0].Rows[i]["PayerProgram"].ToString();
                                uobj.ProcedureCode = ds.Tables[0].Rows[i]["ProcedureCode"].ToString();
                                uobj.JurisdictionCode = ds.Tables[0].Rows[i]["Jurisdictioncode"].ToString();
                            }
                            uobj.CaregiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                            uobj.AppointmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("yyyy-MM-dd");
                            uobj.IsManual = Convert.ToString(ds.Tables[0].Rows[i]["ismanual"]);
                             
                           
                        }

                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "PatientRequest";
                objErrorlog.Methodname = "GetAllPatientRequestDetails";

            } 
            return PartialView(uobj);
        }
     
        public JsonResult GetAllReasonForCompleteRequests()
        {
            ReasonForCompleteRequests response = new ReasonForCompleteRequests();
            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllReasonForCompleteRequests");

            if (ds?.Tables.Count > 0 && ds.Tables[0]?.Rows.Count > 0)
            {
                DataTable table = ds.Tables[0];

                if (table != null)
                {
                    IEnumerable<DataRow> rows = table.Rows.Cast<DataRow>();

                    var groupedData = rows.GroupBy(row => row["MainTypeReason"].ToString());

                    foreach (var group in groupedData)
                    {

                        if (group.Key.Equals("ADLs", StringComparison.OrdinalIgnoreCase))
                        {
                            response.ADLs = group.Select(row => new ReasonCodeType
                            {
                                CompleteRequestsReason = row["Reasons"].ToString(),
                                ReasonId = Convert.ToInt32(row["ReasonRequestsId"])
                            }).ToList();
                        }
                        else if (group.Key.Equals("IADLs", StringComparison.OrdinalIgnoreCase))
                        {
                            response.IADLs = group.Select(row => new ReasonCodeType
                            {
                                CompleteRequestsReason = row["Reasons"].ToString(),
                                ReasonId = Convert.ToInt32(row["ReasonRequestsId"])
                            }).ToList();
                        }
                        else if (group.Key.Equals("Acknowledgement", StringComparison.OrdinalIgnoreCase))
                        {
                            response.Acknowledgement = group.Select(row => new ReasonCodeType
                            {
                                CompleteRequestsReason = row["Reasons"].ToString(),
                                ReasonId = Convert.ToInt32(row["ReasonRequestsId"])
                            }).ToList();
                        }
                    }
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CompletePatientRequestDetails(AppointmentRequestDetails RequestDetails)
        {
            string result = "";
            string UserID = "";
            UserID = Membership.GetUser().ProviderUserKey.ToString();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

              DateTime checlin = DateTime.ParseExact(RequestDetails.CheckInDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime chekout = DateTime.ParseExact(RequestDetails.CheckOutDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "CompletePatientRequestData", RequestDetails.PatientRequestId,
                                                       DateTime.ParseExact(RequestDetails.CheckInDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(RequestDetails.CheckOutDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       UserID,
                                                       RequestDetails.CheckOutFormReason
                                                     );

                if (i > 0)
                {

                    if (OrganisationId > 0 && OrganisationId == 3)
                    {
                       string visitRequest = GetAndPostEmployeeVisitDataAllMedByPatientRequestId(Convert.ToInt32(RequestDetails.PatientRequestId));
                    }

                    result = "success";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]

        //public JsonResult MarkRequestsComplete(List<AppointmentRequestDetails> requestList) //MultiApointments

        //{

        //    int i = 0;

        //    string result = "";

        //    if (requestList == null || !requestList.Any())

        //    {

        //        return Json(new { status = "error", message = "No data provided." });

        //    }

        //    string userId = Membership.GetUser().ProviderUserKey.ToString();

        //    var dt = new DataTable();

        //    dt.Columns.Add("PatientRequestId", typeof(int));

        //    dt.Columns.Add("CheckInDateTime", typeof(DateTime));

        //    dt.Columns.Add("CheckOutDateTime", typeof(DateTime));

        //    dt.Columns.Add("UserID", typeof(string));

        //    dt.Columns.Add("CheckOutFormReason", typeof(string));

        //    foreach (var req in requestList)

        //    {

        //        dt.Rows.Add(req.PatientRequestId, DateTime.ParseExact(req.CheckInDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture), DateTime.ParseExact(req.CheckOutDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture), userId, req.CheckOutFormReason);

        //    }

        //    try

        //    {

        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))

        //        {

        //            con.Open();

        //            using (SqlCommand cmd = new SqlCommand("MarkAsCompleteRequestList", con))

        //            {

        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.AddWithValue("@MarkAsCompleteRequestList", dt);

        //                i = cmd.ExecuteNonQuery();

        //                if (i > 0)

        //                {

        //                    result = "Success";

        //                }

        //                else
        //                {

        //                    result = "failed";

        //                }

        //            }

        //        }

        //        return Json(result, JsonRequestBehavior.AllowGet);


        //    }

        //    catch (Exception ex)

        //    {

        //        // Log the exception here instead of throwing

        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);

        //    }

        //}


        [HttpPost]
        public JsonResult MarkRequestsComplete(List<AppointmentRequestDetails> requestList) //MultiApointments
        {
            int i = 0;
            string result = "";

            string RequestId = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            if (requestList == null || !requestList.Any())
            {
                return Json(new { status = "error", message = "No data provided." });
            }

            string userId = Membership.GetUser().ProviderUserKey.ToString();

            var dt = new DataTable();
            dt.Columns.Add("PatientRequestId", typeof(int));
            dt.Columns.Add("CheckInDateTime", typeof(DateTime));
            dt.Columns.Add("CheckOutDateTime", typeof(DateTime));
            dt.Columns.Add("UserID", typeof(string));
            dt.Columns.Add("CheckOutFormReason", typeof(string));


            StringBuilder requestIdBuilder = new StringBuilder();

            foreach (var req in requestList)
            {
                dt.Rows.Add(req.PatientRequestId, DateTime.ParseExact(req.CheckInDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture), DateTime.ParseExact(req.CheckOutDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture), userId, req.CheckOutFormReason);

                requestIdBuilder.Append(req.PatientRequestId).Append(",");
            }

             RequestId = requestIdBuilder.Length > 0 ? requestIdBuilder.ToString(0, requestIdBuilder.Length - 1):string.Empty;

         
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("MarkAsCompleteRequestList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MarkAsCompleteRequestList", dt);
                        i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            if (OrganisationId > 0 && OrganisationId == 3)
                            {
                              string visitRequest = GetAndPostEmployeeVisitDataAllMedByPatientRequestIds(RequestId);
                            }

                            result = "Success";
                        }
                        else
                        {
                            result = "failed";
                        }
                    }
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception here instead of throwing
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public string GetAndPostEmployeeVisitDataAllMedByPatientRequestIds(string patientRequestIds)
        {
            string result = "";
            string employeeSSN = "00001";
            List<ClientVisitRequest> clientVisitRequestList = new List<ClientVisitRequest>();

            try
            {
                string officeId = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();


                DataTable patientRequestIdTable = new DataTable();
                patientRequestIdTable.Columns.Add("PatientRequestId", typeof(int));


                var ids = patientRequestIds.Split(',')
                                        .Select(id => id.Trim())
                                        .Where(id => !string.IsNullOrEmpty(id))
                                        .ToList();

                foreach (var id in ids)
                {
                    patientRequestIdTable.Rows.Add(id);
                }

                using (SqlConnection conn = new SqlConnection(Settings.CareGiverSuperAdminDatabase().ToString()))
                using (SqlCommand cmd = new SqlCommand("GetEmployeeVisitRequestToaddSandDataAllMedByPatientRequestIdList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlParameter tvpParam = cmd.Parameters.AddWithValue("@PatientRequestIds", patientRequestIdTable);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.PatientRequestIdTableType";

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                            continue;

                        ClientVisitRequest visit = new ClientVisitRequest
                        {
                            ProviderIdentification = new ClientVisitRequestProviderIdentification
                            {
                                ProviderID = ConfigurationManager.AppSettings["Provider_IDMed"].ToString(),
                                ProviderQualifier = "NPI"
                            },
                            EmployeeQualifier = "EmployeeSSN",
                            EmployeeOtherID = Convert.ToString(item["EEID"]),
                            EmployeeIdentifier = employeeSSN + Convert.ToString(item["NurseId"]),
                            VisitOtherID = Convert.ToString(item["NurseScheduleId"]),
                            SequenceID = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString(),
                            ClientIDQualifier = "ClientMedicaidID",
                            ClientID = Convert.ToString(item["MedicalId"]),
                            ClientIdentifier = Convert.ToString(item["MedicalId"]),
                            ClientOtherID = new string(Convert.ToString(item["PrimaryMD"]).Where(char.IsLetterOrDigit).ToArray()),
                            VisitCancelledIndicator = "False",
                            PayerID = Convert.ToString(item["PayerId"]),
                            PayerProgram = Convert.ToString(item["PayerProgram"]),
                            ProcedureCode = Convert.ToString(item["ProcedureCode"])?.Split(',')?.FirstOrDefault(),
                            VisitTimeZone = "US/Eastern",
                            Reschedule = "No",
                            AdjInDateTime = null,
                            AdjOutDateTime = null,
                            BillVisit = "true",
                            HoursToBill = "1",
                            HoursToPay = "1",
                            Memo = "This is a memo!",
                            ClientVerifiedTimes = "true",
                            ClientVerifiedTasks = "true",
                            ClientVerifiedService = "true",
                            ClientSignatureAvailable = "true",
                            ClientVoiceRecording = "true",
                        };

                        var procedureCode = visit.ProcedureCode;

                        List<ClientVisitRequestCalls> callList = new List<ClientVisitRequestCalls>
                {
                    new ClientVisitRequestCalls
                    {
                        CallExternalID = Convert.ToString(item["PatientRequestId"]) + Convert.ToString(item["NurseId"]),
                        CallDateTime = Convert.ToDateTime(item["CheckInDateTime"]).ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                        CallAssignment = "Time In",
                        CallType = "Mobile",
                        ProcedureCode = procedureCode,
                        ClientIdentifierOnCall = visit.ClientID,
                        MobileLogin = Convert.ToString(item["username"]),
                        CallLatitude = Convert.ToString(item["Latitude"]),
                        CallLongitude = Convert.ToString(item["Longitude"]),
                        VisitLocationType = "1"
                    },
                    new ClientVisitRequestCalls
                    {
                        CallExternalID = Convert.ToString(item["PatientRequestId"]) + Convert.ToString(item["NurseId"]) + "1",
                        CallDateTime = Convert.ToDateTime(item["CheckOutDateTime"]).ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                        CallAssignment = "Time Out",
                        CallType = "Mobile",
                        ProcedureCode = procedureCode,
                        ClientIdentifierOnCall = visit.ClientID,
                        MobileLogin = Convert.ToString(item["username"]),
                        CallLatitude = Convert.ToString(item["Latitude"]),
                        CallLongitude = Convert.ToString(item["Longitude"]),
                        VisitLocationType = "1"
                    }
                };

                        visit.Calls = callList;
                        clientVisitRequestList.Add(visit);
                    }
                }

                if (!clientVisitRequestList.Any())
                    throw new Exception("No data available to send");

                string jsonPayload = JsonConvert.SerializeObject(clientVisitRequestList);
                Task.Run(async () => { result = await SubmitEmployeeVisitRequestDataAllMed(jsonPayload); }).Wait();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }



        public string GetAndPostEmployeeVisitDataAllMedByPatientRequestId(int PatientRequestId)
        {

            Dictionary<string, object> res = new Dictionary<string, object>();
            string result = "";
            string EmployeeSSN = "00001";

            List<ClientVisitRequest> clientVisitRequestList = new List<ClientVisitRequest>();
            try
            {
                string SandDataOfficeId = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeVisitRequestToaddSandDataRequestAllMedByPatientRequestId",
                                                                                                          PatientRequestId
                                                                                                         );

                if (ds != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {

                            if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                            {

                                ClientVisitRequest clientVisitRequest = new ClientVisitRequest();
                                ClientVisitRequestProviderIdentification objprovider = new ClientVisitRequestProviderIdentification();
                                objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDMed"].ToString();
                                objprovider.ProviderQualifier = "NPI";
                                clientVisitRequest.ProviderIdentification = objprovider;

                                clientVisitRequest.EmployeeQualifier = "EmployeeSSN";

                                clientVisitRequest.EmployeeOtherID = Convert.ToString(item["EEID"]);
                                clientVisitRequest.EmployeeIdentifier = EmployeeSSN + Convert.ToString(item["NurseId"]);

                                clientVisitRequest.VisitOtherID = Convert.ToString(item["NurseScheduleId"]);
                                clientVisitRequest.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
                                clientVisitRequest.GroupCode = null;
                                clientVisitRequest.ClientIDQualifier = "ClientMedicaidID";

                                string ChkMedicalId = string.Empty;
                                string strChkMedical = Convert.ToString(item["MedicalId"]).ToString();
                                string strChkMedicalFInal = string.Empty;

                                clientVisitRequest.ClientID = Convert.ToString(item["MedicalId"]).ToString();
                                clientVisitRequest.ClientIdentifier = clientVisitRequest.ClientID;


                                clientVisitRequest.ClientOtherID = Convert.ToString(item["PrimaryMD"]); //Convert.ToString(item["MedicalId"]);
                                //clientVisitRequest.ContingencyPlan = "CP01";

                                string str3 = Convert.ToString(item["PrimaryMD"]);
                                string str4 = string.Empty;
                                int val = 0;

                                for (int i = 0; i < str3.Length; i++)
                                {
                                    if (Char.IsLetterOrDigit(str3[i]))
                                        str4 += str3[i];
                                }
                                if (str4.Length > 0)
                                {
                                    clientVisitRequest.ClientOtherID = str4.ToString();
                                }


                                clientVisitRequest.VisitCancelledIndicator = "False";


                                if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                                {
                                    clientVisitRequest.PayerID = Convert.ToString(item["PayerId"]);
                                }
                                //else
                                //{
                                //    clientVisitRequest.PayerID = "CADDS";
                                //}

                                if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
                                {
                                    clientVisitRequest.PayerProgram = Convert.ToString(item["PayerProgram"]);
                                }
                                //else
                                //{
                                //    clientVisitRequest.PayerProgram = "PCS"; 
                                //}

                                string procedureArrayCode = Convert.ToString(item["ProcedureCode"]).ToString();
                                string ProcedureCodeReuse = "";

                                if (procedureArrayCode.Contains(','))
                                {

                                    int ProcedureData = Convert.ToString(item["ProcedureCode"]).Split(',').Length;
                                    string[] ProcedureArray = Convert.ToString(item["ProcedureCode"]).Split(',');

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        ProcedureCodeReuse = Convert.ToString(ProcedureArray[0]);
                                        clientVisitRequest.ProcedureCode = ProcedureCodeReuse;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        ProcedureCodeReuse = Convert.ToString(item["ProcedureCode"]).ToString();
                                        clientVisitRequest.ProcedureCode = ProcedureCodeReuse;
                                    }

                                }
                                //else
                                //{
                                //    clientVisitRequest.ProcedureCode = "Z9081";
                                //}

                                clientVisitRequest.VisitTimeZone = "US/Eastern";
                                // clientVisitRequest.ScheduleStartTime = Convert.ToString((Convert.ToDateTime(item["FromTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                // clientVisitRequest.ScheduleEndTime = Convert.ToString((Convert.ToDateTime(item["ToTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                clientVisitRequest.Reschedule = "No";
                                clientVisitRequest.AdjInDateTime = null;// Convert.ToString(item["InsertDateTime"]);
                                clientVisitRequest.AdjOutDateTime = null;// Convert.ToString(item["InsertDateTime"]); 
                                clientVisitRequest.BillVisit = "true";
                                clientVisitRequest.HoursToBill = "1";
                                clientVisitRequest.HoursToPay = "1";
                                clientVisitRequest.Memo = "This is a memo!";
                                clientVisitRequest.ClientVerifiedTimes = "true";
                                clientVisitRequest.ClientVerifiedTasks = "true";
                                clientVisitRequest.ClientVerifiedService = "true";
                                clientVisitRequest.ClientSignatureAvailable = "true";
                                clientVisitRequest.ClientVoiceRecording = "true";

                                //clientVisitRequest.Modifier1 = null;
                                //clientVisitRequest.Modifier2 = null;
                                //clientVisitRequest.Modifier3 = null;
                                //clientVisitRequest.Modifier4 = null;
                                // ClientVisitRequestCalls clientVisitRequestCalls = new ClientVisitRequestCalls();

                                List<ClientVisitRequestCalls> clientVisitRequestCallsList = new List<ClientVisitRequestCalls>();
                                {
                                    ClientVisitRequestCalls clientVisitRequestCalls = new ClientVisitRequestCalls();
                                    clientVisitRequestCalls.CallExternalID = Convert.ToString(item["PatientRequestId"]) + (Convert.ToString(item["NurseId"]));
                                    clientVisitRequestCalls.CallDateTime = Convert.ToString((Convert.ToDateTime(item["CheckInDateTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                    clientVisitRequestCalls.CallAssignment = "Time In";
                                    clientVisitRequestCalls.GroupCode = null;
                                    clientVisitRequestCalls.CallType = "Mobile";

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        clientVisitRequestCalls.ProcedureCode = ProcedureCodeReuse;
                                    }
                                    //else
                                    //{
                                    //    clientVisitRequestCalls.ProcedureCode = "Z9081"; 
                                    //}

                                    clientVisitRequestCalls.ClientIdentifierOnCall = clientVisitRequest.ClientID;
                                    clientVisitRequestCalls.MobileLogin = Convert.ToString(item["username"]);
                                    clientVisitRequestCalls.CallLatitude = Convert.ToString(item["Latitude"]);
                                    clientVisitRequestCalls.CallLongitude = Convert.ToString(item["Longitude"]);
                                    //clientVisitRequestCalls.Location = null;
                                    clientVisitRequestCalls.TelephonyPIN = null;
                                    clientVisitRequestCalls.OriginatingPhoneNumber = null;
                                    clientVisitRequestCalls.VisitLocationType = "1";
                                    clientVisitRequestCallsList.Add(clientVisitRequestCalls);
                                }

                                {
                                    ClientVisitRequestCalls clientVisitRequestCallss = new ClientVisitRequestCalls();
                                    clientVisitRequestCallss.CallExternalID = Convert.ToString(item["PatientRequestId"]) + (Convert.ToString(item["NurseId"])) + "1";
                                    clientVisitRequestCallss.CallDateTime = Convert.ToString((Convert.ToDateTime(item["CheckOutDateTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                                    clientVisitRequestCallss.CallAssignment = "Time Out";
                                    clientVisitRequestCallss.GroupCode = null;
                                    clientVisitRequestCallss.CallType = "Mobile";

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    {
                                        clientVisitRequestCallss.ProcedureCode = ProcedureCodeReuse;
                                    }
                                    //else
                                    //{
                                    //    clientVisitRequestCalls.ProcedureCode = "Z9081"; 
                                    //}

                                    clientVisitRequestCallss.ClientIdentifierOnCall = clientVisitRequest.ClientID;
                                    clientVisitRequestCallss.MobileLogin = Convert.ToString(item["username"]);
                                    clientVisitRequestCallss.CallLatitude = Convert.ToString(item["Latitude"]);
                                    clientVisitRequestCallss.CallLongitude = Convert.ToString(item["Longitude"]);
                                    // clientVisitRequestCallss.Location = null;
                                    clientVisitRequestCallss.TelephonyPIN = null;
                                    clientVisitRequestCallss.OriginatingPhoneNumber = null;
                                    clientVisitRequestCallss.VisitLocationType = "1";
                                    clientVisitRequestCallsList.Add(clientVisitRequestCallss);
                                }


                                clientVisitRequest.Calls = clientVisitRequestCallsList;
                                clientVisitRequestList.Add(clientVisitRequest);
                            }
                            else
                            {
                                return result = "";
                            }
                        }
                    }
                    else
                    {
                        return result = "";
                    }
                }
                else
                {
                    return result = "";
                }
                if (clientVisitRequestList.Count <= 0)
                {
                    throw new Exception("No data Available to send");
                }

                var arraylist = clientVisitRequestList.ToArray();
                List<ClientVisitRequest> request = new List<ClientVisitRequest>();

                foreach (var ReqItem in arraylist)
                {
                    request.Add(ReqItem);
                }
                string x = JsonConvert.SerializeObject(request);
                Task.Run(async () => { result = await SubmitEmployeeVisitRequestDataAllMed(x); }).Wait();
                //  res["Success"] = true;
                // res["Result"] = result;
            }


            catch (Exception ex)
            {
                //res["Success"] = false;
                //res["Result"] = ex.Message;
                result = ex.Message;
            }
            // return Json(res, JsonRequestBehavior.AllowGet);
            return result;
        }

        public async Task<string> SubmitEmployeeVisitRequestDataAllMed(string request)
        {

            string result = "";
            string result1 = "";
            string finalresult = "";

            string datajson = request;
            try
            {

                string API_Url = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetailsMed"].ToString();
                string API_Url_Status = System.Configuration.ConfigurationManager.AppSettings["SandData_VisitDetailsStatusMed"].ToString();

                var clientGetDialogId = new System.Net.Http.HttpClient();
                string Token = ConfigurationManager.AppSettings["TokenMed"].ToString();
                string actheader = ConfigurationManager.AppSettings["mykeyMed"].ToString();

                // clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1");

                clientGetDialogId.BaseAddress = new Uri(API_Url);

                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
                var content2 = new StringContent(request, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response2 = await clientGetDialogId.PostAsync("", content2);
                result = response2.Content.ReadAsStringAsync().Result;

                var data1 = (JObject)JsonConvert.DeserializeObject(result);
                string UDID = data1["id"].Value<string>();

                insertVisitDetails(UDID, UDID, result, request);

                Thread.Sleep(10000);

                var clientGetId = new System.Net.Http.HttpClient();

                //clientGetId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/visits/rest/api/v1.1/status?uuid=" + UDID);

                clientGetId.BaseAddress = new Uri(API_Url_Status + UDID);

                clientGetId.DefaultRequestHeaders.Accept.Clear();
                clientGetId.DefaultRequestHeaders.Add("Authorization", Token);
                clientGetId.DefaultRequestHeaders.Add("account", actheader);
                // var content2 = new StringContent(request, Encoding.UTF8, "application/json");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response1 = await clientGetId.GetAsync("");

                result1 = response1.Content.ReadAsStringAsync().Result;

                var data2 = (JObject)JsonConvert.DeserializeObject(result);
                //  finalresult = data2["data"]["message"].Value<string>();

                insertVisitDetails(UDID, UDID, result1, "");
            }
            catch (Exception ex)
            {
                insertVisitDetails("VisitDetailsData", ex.Message, result, "");
                //  throw new Exception(ex.Message);
            }

            return finalresult;

        }

        [HttpPost]
        public ActionResult UpdatePatientRequestInfo(SchedulePatientRequestModel objSchedulePatientRequest)
        {
            string result = "";

            SchedulePatientRequestModel PatientRequest = new SchedulePatientRequestModel();
            try
            {
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();


                if (objSchedulePatientRequest.PatientRequestId == 0)
                {
                    throw new Exception("PatientRequestId not Found");
                }

                PatientRequest.PatientRequestId = objSchedulePatientRequest.PatientRequestId;
                PatientRequest.Date = objSchedulePatientRequest.Date; 
                PatientRequest.FromTime = objSchedulePatientRequest.FromTime;
                PatientRequest.ToTime = objSchedulePatientRequest.ToTime;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdatePatientRequest", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PatientRequestId", PatientRequest.PatientRequestId);
                        cmd.Parameters.AddWithValue("@Date", PatientRequest.Date);
                        cmd.Parameters.AddWithValue("@FromTime", PatientRequest.FromTime);
                        cmd.Parameters.AddWithValue("@ToTime", PatientRequest.ToTime);
                        cmd.Parameters.AddWithValue("@UserId", InsertedUserID);


                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            int status = Convert.ToInt32(reader["StatusCode"]);
                            string message = reader["Message"].ToString();

                            if (status == 1)
                            {
                                result = "success";
                            }
                            else
                            {
                                result = "PatientRequestId not found.";
                            }
                        }

                        reader.Close();
                    }
                    con.Close();
                } 
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL-related exceptions
                 result = "SQL Error: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            } 

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult CheckCaregiverAllowedHours(int nurseid)  
        {
            string msg = "";
           // string NurseID = Regex.Replace(nurseid, @"\D", "");
            try
            {
                AccountController obj = new AccountController();
                /*  Code Added for sent mail to the scheduler and officemanager if she taken more than the allowed time  */
                DataSet dt = obj.GetAccessHoursTakenByNurse(nurseid);
                if (dt.Tables[0].Rows[0]["Name"].ToString() != "" && dt.Tables[0].Rows[0]["Email"].ToString() != "")
                {
                    var nurseName = dt.Tables[0].Rows[0]["Name"].ToString();
                    var allowedHours = dt.Tables[0].Rows[0]["AllowedHours"].ToString();
                    var usedHours = dt.Tables[0].Rows[0]["TotalHours"].ToString();
                    var OrganisationName = "Paseva.com";
                   
                    //string EmailBody = string.Format(
                    //     "Dear {0},\n\nYou have exceeded your allowed working hours for this week.\nAllowed: {1} hrs\nUsed: {2} hrs\n\nPlease ensure your hours stay within the limit.\n\nThank you,\n{3}",
                    //     dt.Tables[0].Rows[0]["Name"].ToString(),
                    //     dt.Tables[0].Rows[0]["AllowedHours"].ToString(),
                    //     dt.Tables[0].Rows[0]["TotalHours"].ToString(),
                    //     OrganisationName
                    //     );
                    //string emailBody = string.Format(
                    //                                 @"Dear {0},<br/><br/>
                    //                                 You have exceeded your allowed working hours for this week.<br/><br/>
                    //                                 <b>Allowed hours:</b> {1} hrs<br/>
                    //                                 <b>Used hours:</b> {2} hrs<br/><br/>
                    //                                 Please ensure your hours stay within the allowed limit.<br/><br/>
                    //                                 Thank you,<br/>
                    //                                 {3}",
                    //                                 nurseName, allowedHours, usedHours, OrganisationName
                    //                                );
                   // sendEmail(dt.Tables[2].Rows[0]["SchedulerEmail"].ToString(), subject, emailBody, false, "", dt.Tables[3].Rows[0]["OfficeEmail"].ToString());
                }

                /* END  */

                msg = JsonConvert.SerializeObject(dt); //
               // conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

     /*   public void sendEmail(string SchedulerEmail,string AttachmentFileName, string CCMailID,string NurseName,string AllowedHours,string TotalHours,string OfficeEmail)
        {
            try
            {
                bool IsFileAttachment = false;
                string OrganisationName = "Paseva.com";
                string subject = "You have exceeded your allowed access time";
                string body = string.Format(
                                            @"Dear {0},<br/><br/>
                                            You have exceeded your allowed working hours for this week.<br/><br/>
                                            <b>Allowed hours:</b> {1} hrs<br/>
                                            <b>Used hours:</b> {2} hrs<br/><br/>
                                            Please ensure your hours stay within the allowed limit.<br/><br/>
                                            Thank you,<br/>
                                            {3}",
                                            NurseName, AllowedHours, TotalHours, OrganisationName
                                           );
                var mailMessage = new MailMessage();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                mailMessage.To.Add(SchedulerEmail);
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"]);
                if (!(string.IsNullOrEmpty(CCMailID)))
                    mailMessage.CC.Add(CCMailID);
                mailMessage.Subject = subject;
                if (IsFileAttachment == true)
                {
                    string[] ach = AttachmentFileName.Split(';');
                    for (int i = 0; i < ach.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(ach[i]))
                        {
                            Attachment attachFile = new Attachment(ach[i]);
                            mailMessage.Attachments.Add(attachFile);
                        }
                    }
                }
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                var smtpClient = new SmtpClient { EnableSsl = false };
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Host = "smtp.office365.com";
                //ConfigurationManager.AppSettings["SMTPHost"];
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.UseDefaultCredentials = false;
                // mailMessage.Bcc.Add(bccAddress);
                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
               // return true;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "sendEmail";
                // string result = InsertErrorLog(objErrorlog);
            }
            //return false;
        } */

        [HttpPost]
        public JsonResult SendEmail(string SchedulerEmail, string CCMailID, string NurseName, string AllowedHours, string TotalHours, string OfficeEmail)
        {
            try
            {
                string AttachmentFileName = "";
                bool IsFileAttachment = false;
                string OrganisationName = "Paseva.com";
                string subject = "You have exceeded your allowed access time";
                string body = string.Format(
                                            @"Dear {0},<br/><br/>
                                            You have exceeded your allowed working hours.<br/><br/>
                                            <b>Allowed hours:</b> {1} hrs<br/>
                                            <b>Scheduling hours:</b> {2} hrs<br/><br/>
                                            Please ensure your hours stay within the allowed limit.<br/><br/>
                                            Thank you,<br/>
                                            {3}",
                                            NurseName, AllowedHours, TotalHours, OrganisationName
                                           );
                var mailMessage = new MailMessage();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                mailMessage.To.Add(SchedulerEmail);
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"]);
                if (!(string.IsNullOrEmpty(CCMailID)))
                    mailMessage.CC.Add(CCMailID);
                mailMessage.Subject = subject;
                if (IsFileAttachment == true)
                {
                    string[] ach = AttachmentFileName.Split(';');
                    for (int i = 0; i < ach.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(ach[i]))
                        {
                            Attachment attachFile = new Attachment(ach[i]);
                            mailMessage.Attachments.Add(attachFile);
                        }
                    }
                }
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                var smtpClient = new SmtpClient { EnableSsl = false };
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Host = "smtp.office365.com";
                //ConfigurationManager.AppSettings["SMTPHost"];
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.UseDefaultCredentials = false;
                // mailMessage.Bcc.Add(bccAddress);
                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
                return Json(new { success = true, message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}