using CaregiverLite.Models;
using CaregiverLiteWCF;
using CaregiverLiteWCF.Class;
using DifferenzLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CaregiverLite.Action_Filters;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class ClientRequestController : Controller
    {

        [AllowAnonymous]
        public JsonResult GetAllScheduleClientRequest(JQueryDataTableParamModel param)
        {
            ScheduleClientRequestList ScheduleClientRequestList = new ScheduleClientRequestList();
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
                    sortOrder = "ClientName";
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
                    sortOrder = "CRNumber";
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
                    sortOrder = "DateOfVisit";
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
                    sortOrder = "MarketersName";
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
                    sortOrder = "TotalMarketersNotified";
                }
                else if (sortColumnIndex == 15)
                {
                    sortOrder = "Check In/Out";
                }
                else
                {
                    sortOrder = "ClientRequestId";
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

              //  ScheduleClientRequestServiceProxy ScheduleClientRequestService = new ScheduleClientRequestServiceProxy();
                ScheduleClientRequestList = GetAllScheduleClientRequestList(Convert.ToString(pageNo), Convert.ToString(recordPerPage), search, sortOrder, sortDirection, Convert.ToString(IsAdmin), LogInUserId, Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"), FilterStatus, Convert.ToString(FilterOffice));
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "GetAllScheduleClientRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            if (ScheduleClientRequestList.ScheduleClientRequestsList != null)
            {
                var result = from C in ScheduleClientRequestList.ScheduleClientRequestsList select new[] { C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ScheduleClientRequestList.TotalNumberofRecord,
                    iTotalDisplayRecords = ScheduleClientRequestList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ScheduleClientRequestList.TotalNumberofRecord,
                    iTotalDisplayRecords = ScheduleClientRequestList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }

        }



        // GET: ClientRequest
        public ActionResult ClientRequest()
        {
            FillAllOffices();
            return View();
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



        public ScheduleClientRequestList GetAllScheduleClientRequestList(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterStatus, string FilterOffice)
        {
            ScheduleClientRequestList ListScheduleClientRequest = new ScheduleClientRequestList();
            try
            {
                if (FromDate == "||" && ToDate == "||")
                {
                    FromDate = "1000-01-01";
                    ToDate = "1000-01-01";
                }
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetALLScheduleClientRequest",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        IsAdmin,
                                                        LogInUserId,
                                                         Convert.ToDateTime(FromDate),
                                                         Convert.ToDateTime(ToDate),
                                                        FilterStatus,
                                                        FilterOffice);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    List<ScheduleClientRequest> ScheduleClientRequestList = new List<ScheduleClientRequest>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    { 
                                              
                        ScheduleClientRequest objScheduleClientRequest = new ScheduleClientRequest();
                        objScheduleClientRequest.ClientRequestId = Convert.ToInt32(ds.Tables[1].Rows[i]["ClientRequestId"]);
                        objScheduleClientRequest.SchedulerName = ds.Tables[1].Rows[i]["SchedulerName"].ToString();
                        objScheduleClientRequest.ClientName = ds.Tables[1].Rows[i]["ClientName"].ToString();
                        objScheduleClientRequest.Address = ds.Tables[1].Rows[i]["Address"].ToString();
                        objScheduleClientRequest.ZipCode = ds.Tables[1].Rows[i]["ZipCode"].ToString();
                        objScheduleClientRequest.CRNumber = ds.Tables[1].Rows[i]["CRNumber"].ToString();
                        objScheduleClientRequest.Description = ds.Tables[1].Rows[i]["Description"].ToString();
                        objScheduleClientRequest.InsertDateTime = ds.Tables[1].Rows[i]["InsertDateTime"].ToString();
                      
                        //DateTime.Parse(ds.Tables[1].Rows[i]["InsertDateTime"].ToString()).ToString();
                        //.ToString("MM/dd/yyyy");
                        //objScheduleClientRequest.Date = DateTime.Parse(ds.Tables[1].Rows[i]["DateOfVisit"].ToString()).ToString("MM/dd/yyyy");
                        
                        objScheduleClientRequest.Date = ((ds.Tables[1].Rows[i]["DateOfVisit"].ToString()) == "") ? "" : DateTime.Parse(ds.Tables[1].Rows[i]["DateOfVisit"].ToString()).ToString("MM/dd/yyyy");
                        objScheduleClientRequest.FromTime = ds.Tables[1].Rows[i]["FromTime"].ToString();
                        objScheduleClientRequest.ToTime = ds.Tables[1].Rows[i]["ToTime"].ToString();
                        objScheduleClientRequest.MarketersName = ds.Tables[1].Rows[i]["MarketersName"].ToString();
                        objScheduleClientRequest.OfficeName = ds.Tables[1].Rows[i]["Office"].ToString();
                        objScheduleClientRequest.TotalMarketersNotified = Convert.ToInt32(ds.Tables[1].Rows[i]["TotalCaregiversNotified"].ToString());
                        objScheduleClientRequest.DrivingStartTime = ds.Tables[1].Rows[i]["DrivingStartTime"].ToString();
                        objScheduleClientRequest.CheckInTime = ds.Tables[1].Rows[i]["CheckInDateTime"].ToString();
                        objScheduleClientRequest.CheckOutTime = ds.Tables[1].Rows[i]["CheckOutDateTime"].ToString();
                        objScheduleClientRequest.Miles = ds.Tables[1].Rows[i]["DrivingTotalDistance"].ToString();
                        objScheduleClientRequest.MarketersId = Convert.ToInt32(ds.Tables[1].Rows[i]["MarketersId"]);
                        
                        // var PatientSignaturePath = ConfigurationManager.AppSettings["MarketersSignature"].ToString();
                        //if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["ClientSignature"].ToString()))
                        //{
                        //    objScheduleClientRequest.ClientSignature = PatientSignaturePath + ds.Tables[1].Rows[i]["ClientSignature"].ToString();
                        //}
                        
                        var Iscancel = ds.Tables[1].Rows[i]["IsCancelled"].ToString();
                        if (Iscancel == "False")
                        {
                            objScheduleClientRequest.IsCancelled = false;
                        }
                        else
                        {
                            objScheduleClientRequest.IsCancelled = true;
                        }

                        objScheduleClientRequest.Status = ds.Tables[1].Rows[i]["Status"].ToString();
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezoneId"].ToString()))
                        {
                            objScheduleClientRequest.TimezoneId = ds.Tables[1].Rows[i]["TimezoneId"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezoneOffset"].ToString()))
                        {
                            objScheduleClientRequest.TimezoneOffset = Convert.ToInt32(ds.Tables[1].Rows[i]["TimezoneOffset"]);
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezonePostfix"].ToString()))
                        {
                            objScheduleClientRequest.TimezonePostfix = ds.Tables[1].Rows[i]["TimezonePostfix"].ToString();
                        }

                        objScheduleClientRequest.CheckInLatLong = ds.Tables[1].Rows[i]["CheckInLatLong"].ToString();
                        objScheduleClientRequest.CheckOutLatLong = ds.Tables[1].Rows[i]["CheckOutLatLong"].ToString();
                        try
                        {
                            objScheduleClientRequest.CheckInAddress = getAddressGoogleAPI(ds.Tables[1].Rows[i]["CheckInLatLong"].ToString());
                            objScheduleClientRequest.CheckOutAddress = getAddressGoogleAPI(ds.Tables[1].Rows[i]["CheckOutLatLong"].ToString());
                        }
                        catch (Exception e)
                        { }

                        ScheduleClientRequestList.Add(objScheduleClientRequest);
                    }

                    ListScheduleClientRequest.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    ListScheduleClientRequest.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListScheduleClientRequest.ScheduleClientRequestsList = ScheduleClientRequestList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetALLScheduleClientRequest";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListScheduleClientRequest;
        }


        public string getAddressGoogleAPI(string LatLong)
        {
            string Address = "";//= new Location();
            using (var w = new WebClient())
            {
                //var url = "http://maps.google.com/maps/api/geocode/json?latlng=" + LatLong + "&sensor=false";
                var url = "https://maps.google.com/maps/api/geocode/json?latlng=" + LatLong + "&key=AIzaSyDCQgMe2fLgmaF9GDvxF61wsIXnktEBKhg";
                var json_data = string.Empty;
                json_data = w.DownloadString(url);
                JObject obj = JsonConvert.DeserializeObject<JObject>(json_data);
                Address = obj["results"][0]["formatted_address"].ToString();
            }
            return Address;
        }


        public string InsertErrorLog(ErrorLog obj)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertErrorLog",
                      obj.Methodname,
                      obj.Pagename,
                      obj.Errormessage,
                      obj.StackTrace,
                      obj.UserID
                     );
                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertErrorLog";
                objErrorlog.UserID = obj.UserID;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public JsonResult RemoveClientRequest(string[] ClientRequestId)
        {
            string result = "";
            try
            {
                var userid = Session["UserId"].ToString();
                foreach (string ClientID in ClientRequestId)
                {
                    var operationResult = RemoveClientRequest(ClientID, userid);
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

        public string RemoveClientRequest(string ClientRequestId, string UserID)
        {
            string result = "";
            try
            {
                Guid UserGUID = Guid.Parse(UserID);
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteClientRequestById",
                                                         Convert.ToInt32(ClientRequestId), UserGUID);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteWebService";
                objErrorlog.Methodname = "RemoveClientRequest";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }



        //public ActionResult AddClientMarektingRequest()
        //{
        //    IEnumerable<SelectListItem> OfficeSelectList = null;
        //    Client objClient = new Client();
        //    try
        //    {
        //        FillAllOffices();
        //        objClient.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = ex.Message;
        //        log.StackTrace = ex.StackTrace;
        //        log.Pagename = "AddClientMarektingRequest";
        //        log.Methodname = "[HttpGet] AddClientMarektingRequest";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string resError = ErrorLogService.InsertErrorLog(log).Result;

        //    }

        //    return PartialView(objClient);
        //}


        public ActionResult AddClientMarektingRequest()
        {
            MarketerDetailsModel objmarketer = new MarketerDetailsModel();
            ClientRequest objClientRequest = new ClientRequest();
            try
            {
                 //  FillAllOffices();
                ////  objClientRequest.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;

                //var lstOffices = officeServiceProxy.GetAllOffices(logInUserId).Result;
                //lstOffices.Insert(0, new Office() { OfficeId = 0, OfficeName = "All" });
                //SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName");
                //ViewBag.lstOffice = officeSelectList;

                List<MarketerDetailsModel> lstmarketer = new List<MarketerDetailsModel>();
                lstmarketer.Insert(0, new MarketerDetailsModel() { MarketersId = 0, MarketersName = "All" });
                SelectList markleterSelectList = new SelectList(lstmarketer, "MarketersId", "MarketersName");
                objClientRequest.marketersSelectList = markleterSelectList;
                ViewBag.marketersSelectList = markleterSelectList;

            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "AddClientMarektingRequest";
                log.Methodname = "[HttpGet] AddClientMarektingRequest";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string resError = ErrorLogService.InsertErrorLog(log).Result;
            }

            return PartialView(objClientRequest);
        }


        public JsonResult GetClientDetailByCRNNumber(string CRNNumber)
        {
            Client obj = new Client();
            ClientRequest ClientResult = new ClientRequest();
            try
            {
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                ClientResult = obj.DBGetClientDetailByCRNNumber(CRNNumber, LoginUserId);

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ClientRequestController";
                log.Methodname = "GetClientDetailByCRNNumber";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return Json(ClientResult, JsonRequestBehavior.AllowGet);
        }




        public JsonResult AddClientRequest()
        {
            string Result = "";



            return Json(Result, JsonRequestBehavior.AllowGet);
        }

    }
}