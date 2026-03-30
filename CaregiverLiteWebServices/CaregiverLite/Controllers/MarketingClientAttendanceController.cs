using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using DifferenzLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class MarketingClientAttendanceController : Controller
    {
        // GET: MarketingClient
        public ActionResult Index()
        {
            return View();
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
        public ActionResult MarketingAttendanceList()
        {
            //FillAllOffices();
            return View();
        }
        public ActionResult GetMarketingAttendanceList(JQueryDataTableParamModel param)
        {
            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            MarketerAttendaceList objMarketingAttendaceList = new MarketerAttendaceList();
            try
            {
                int FilterOfficeId = 0;

                if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


                if (sortColumnIndex == 0)
                {
                    sortOrder = "MarketersName";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "ClientName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "Dateofvisit";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "CheckInTotalTime";
                }
                else
                {
                    sortOrder = "ClientRequestId";
                }


                //if (sortColumnIndex == 0)
                //{
                //    sortOrder = "MarketersName";
                //}
                //else if (sortColumnIndex == 1)
                //{
                //    sortOrder = "MarketersId";
                //}
                //else if (sortColumnIndex == 2)
                //{
                //    sortOrder = "ClientName";
                //}
                //else
                //{
                //    sortOrder = "Dateofvisit";
                //}






                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "TotalHour";
                //}
                //else if (sortColumnIndex == 5)
                //{
                //    sortOrder = "FromTime";
                //}
                //else if (sortColumnIndex == 6)
                //{
                //    sortOrder = "ToTime";
                //}
                //else if (sortColumnIndex == 7)
                //{
                //    sortOrder = "ToTime";
                //}
                //else if (sortColumnIndex == 9)
                //{
                //    sortOrder = "Latitude";
                //}
                //else
                //{
                //    sortOrder = "Longitude";

                //}

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

                objMarketingAttendaceList = DBGetMarketingAttendanceList(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId);

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetMarketingAttandanceList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (objMarketingAttendaceList.MarketersAttendanceList != null)
            {
                var result = from C in objMarketingAttendaceList.MarketersAttendanceList select new[] { C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = objMarketingAttendaceList.TotalNumberofRecord,
                    iTotalDisplayRecords = objMarketingAttendaceList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = objMarketingAttendaceList.TotalNumberofRecord,
                    iTotalDisplayRecords = objMarketingAttendaceList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public MarketerAttendaceList DBGetMarketingAttendanceList(string LogInUserId, int pageNo, int recordPerPage, string search, string sortfield, string sortorder, int OfficeId)
        {

            MarketerAttendaceList MarketingsDetailsListResponse = new MarketerAttendaceList();


            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllMarketersAttendanceList",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        Convert.ToInt32(OfficeId),
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<MarketerAttendanceModel> MarketersAttendanceDetailsList = new List<MarketerAttendanceModel>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        MarketerAttendanceModel objMarketerAttendance = new MarketerAttendanceModel();
                        objMarketerAttendance.MarketersId = Convert.ToInt32(ds.Tables[0].Rows[i]["MarketersId"]);
                        objMarketerAttendance.ClientRequestId = Convert.ToString(ds.Tables[0].Rows[i]["ClientRequestId"]);
                        objMarketerAttendance.MarketersName = ds.Tables[0].Rows[i]["MarketersName"].ToString();
                        objMarketerAttendance.ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString();
                        objMarketerAttendance.Dateofvisit = Convert.ToDateTime(ds.Tables[0].Rows[i]["Dateofvisit"]).ToString("yyyy-MM-dd");
                        objMarketerAttendance.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objMarketerAttendance.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objMarketerAttendance.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objMarketerAttendance.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();
                        objMarketerAttendance.TotalHours = ds.Tables[0].Rows[i]["TotalHours"].ToString();
                        objMarketerAttendance.CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                        objMarketerAttendance.Review = (ds.Tables[0].Rows[i]["review"] == null) ? "" : ds.Tables[0].Rows[i]["review"].ToString();
                        objMarketerAttendance.ReferralId = (ds.Tables[0].Rows[i]["ReferralId"].ToString() == "") ?0 : Convert.ToInt32(ds.Tables[0].Rows[i]["ReferralId"].ToString());

                        // MarketerAttendance.PrimaryMD = ds.Tables[0].Rows[i]["PrimaryMD"].ToString();                         
                        MarketersAttendanceDetailsList.Add(objMarketerAttendance);
                    }
                    MarketingsDetailsListResponse.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    MarketingsDetailsListResponse.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    MarketingsDetailsListResponse.MarketersAttendanceList = MarketersAttendanceDetailsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "DBGetMarketingAttendanceList";
                string result = InsertErrorLog(objErrorlog);
            }
            return MarketingsDetailsListResponse;
        }


        public ActionResult ClientAttendanceDetail(string ClientRequestId)
        {

            MarketerAttendanceModel objAttendanceManagementDetails = new MarketerAttendanceModel();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetMarketersAttendanceDetails_Vin", con))
                {
                    string MarketersName = "";
                    string AppointmentDate = "";
                    string IsManaul = "";
                    string startDrivingDatetime = "";
                    string StopDrivingDatetime = "";
                    string startDrivingLattitude = "";
                    string startDrivingLongitude = "";
                    string stopDrivinglatittude = "";
                    string stopDrivingLongitude = "";
                    string checkinLatittude = "";
                    string checkinLongitude = "";
                    string checkoutLatittude = "";
                    string checkoutLongitude = "";
                    string checkInDatetTime = "";
                    string checkOutDateTime = "";
                    string ClientRequestid = "";



                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientRequestID", ClientRequestId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<MarketerAttendanceModel> AttendanceList = new List<MarketerAttendanceModel>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        MarketerAttendanceModel uobj = new MarketerAttendanceModel();
                        uobj.ClientRequestId = ds.Tables[0].Rows[i]["ClientRequestId"].ToString();
                        ClientRequestid = ds.Tables[0].Rows[i]["ClientRequestId"].ToString();
                        //uobj.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        uobj.Dateofvisit = ds.Tables[0].Rows[i]["DateOfVisit"].ToString();
                        uobj.ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString();
                        uobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        uobj.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        uobj.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStopTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()))
                        {
                            startDrivingDatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStartTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            StopDrivingDatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStopTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            checkInDatetTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckInDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            checkOutDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckOutDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString()))
                        {
                            startDrivingLattitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLatitude"]);
                            stopDrivinglatittude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLatitude"]);
                            startDrivingLongitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLongitude"]);
                            stopDrivingLongitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLongitude"]);

                            checkinLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLatitude"]);
                            checkinLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLongitude"]);

                            checkoutLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLatitude"]);
                            checkoutLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLongitude"]);
                        }

                        MarketersName = ds.Tables[0].Rows[i]["MarketersName"].ToString();
                        AppointmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateOfVisit"]).ToString("yyyy-MM-dd");
                       // IsManaul = Convert.ToString(ds.Tables[0].Rows[i]["ismanual"]);

                        //if (ds.Tables[0].Rows[i]["DrivingStartTime"] != DBNull.Value)
                        //{
                        //    uobj.DrivingStartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStartTime"]);
                        //}
                        //else if (ds.Tables[0].Rows[i]["DrivingStartTime"] == DBNull.Value)
                        //{
                        //    uobj.DrivingStartTime = null;
                        //}
                        //if (ds.Tables[0].Rows[i]["DrivingStopTime"] != DBNull.Value)
                        //{
                        //    uobj.DrivingStopTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStopTime"]);
                        //}
                        //else if (ds.Tables[0].Rows[i]["DrivingStopTime"] == DBNull.Value)
                        //{
                        //    uobj.DrivingStopTime = null;
                        //}

                        uobj.TotalTravelTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                        ViewBag.ClientRequestId = ClientRequestId;
                        string tempRating = ds.Tables[0].Rows[i]["Rating"].ToString();
                        string tempReview = ds.Tables[0].Rows[i]["Review"].ToString();
                        ViewBag.Rating = (tempRating == "") ? 0 : Convert.ToInt32(tempRating);
                        ViewBag.Review = tempReview;


                        AttendanceList.Add(uobj);
                    }
                    // objAttendanceManagementDetails.AttendanceManagementInfo = AttendanceList;
                    ViewBag.AttendanceDetailList = AttendanceList;
                    ViewBag.MarketersName = MarketersName;
                    ViewBag.AppoitnmentDate = AppointmentDate;
                    ViewBag.IsManaul = IsManaul;

                    //ViewBag.startDrivingDatetime = startDrivingDatetime;
                    //ViewBag.StopDrivingDatetime = StopDrivingDatetime;


                    ViewBag.startDrivingDate = startDrivingDatetime.Split(' ')[0];
                    ViewBag.StopDrivingDate = StopDrivingDatetime.Split(' ')[0];

                    if (!string.IsNullOrEmpty(startDrivingDatetime.Split(' ')[0]) && !string.IsNullOrEmpty(StopDrivingDatetime.Split(' ')[0]))
                    {
                        ViewBag.startDrivingtime = startDrivingDatetime.Split(' ')[1];
                        ViewBag.StopDrivingtime = StopDrivingDatetime.Split(' ')[1];
                    }

                    ViewBag.checkInDate = checkInDatetTime.Split(' ')[0];
                    ViewBag.checkOutDate = checkOutDateTime.Split(' ')[0];

                    if (!string.IsNullOrEmpty(checkInDatetTime.Split(' ')[0]) && !string.IsNullOrEmpty(checkOutDateTime.Split(' ')[0]))
                    {
                        ViewBag.checkInTime = checkInDatetTime.Split(' ')[1];
                        ViewBag.checkOutTime = checkOutDateTime.Split(' ')[1];
                    }

                    ViewBag.startDrivingLattitude = startDrivingLattitude;
                    ViewBag.startDrivingLongitude = startDrivingLongitude;

                    ViewBag.stopDrivinglatittude = stopDrivinglatittude;
                    ViewBag.stopDrivingLongitude = stopDrivingLongitude;

                    ViewBag.checkinLatittude = checkinLatittude;
                    ViewBag.checkinLongitude = checkinLongitude;

                    ViewBag.checkoutLatittude = checkoutLatittude;
                    ViewBag.checkoutLongitude = checkoutLongitude;

                    ViewBag.ClientRequestId = ClientRequestId;

                    //  ViewBag.startDrivingDatetime = startDrivingDatetime +" - "+ StopDrivingDatetime;
                    ////  ViewBag.StopDrivingDatetime = StopDrivingDatetime;
                    //  ViewBag.startDrivingLattitude = startDrivingLattitude +" , "+ startDrivingLongitude;
                    //  // ViewBag.startDrivingLongitude = startDrivingLongitude;
                    //  ViewBag.stopDrivinglatittude = stopDrivinglatittude + " , " + stopDrivingLongitude;
                    // // ViewBag.stopDrivingLongitude = stopDrivingLongitude;
                    //  ViewBag.checkinLatittude = checkoutLatittude +" , "+ checkinLongitude;
                    // // ViewBag.checkinLongitude = checkinLongitude;
                    //  ViewBag.checkoutLatittude = checkoutLatittude +" , "+ checkoutLongitude;
                    ////  ViewBag.checkoutLongitude = checkoutLongitude;

                    //  ViewBag.checkInDatetTime = checkInDatetTime +" - "+ checkOutDateTime;
                    ////  ViewBag.checkOutDateTime = checkOutDateTime;

                }
                con.Close();
            }

            return PartialView("ClientAttendanceDetail");
        }



      //  [HttpGet]
        public ActionResult MarketersExcelReport()
        {
            return PartialView("MarketersExcelReport");
        }


        public ActionResult MarketersReferralsReport()
        {
            return PartialView("MarketersReferralsReport");
        }

        //[HttpPost]
        //public ActionResult ExcelReport()
        //{        
        //    return PartialView();
        //}


        public ActionResult ReferrralDetail(string ClientRequestId)
        {
            //string ss = "4311";
            MarketerAttendanceModel objAttendanceManagementDetails = new MarketerAttendanceModel();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetClientReferralListbyClientRequestId", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientRequestID", ClientRequestId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<ClientREferrral> ReferralList = new List<ClientREferrral>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ClientREferrral uobj = new ClientREferrral();
                        uobj.patientRequestid = ds.Tables[0].Rows[i]["patientRequestid"].ToString();
                        uobj.Insurance = ds.Tables[0].Rows[i]["Insurance"].ToString();
                        uobj.ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString();
                        uobj.Marketersid = ds.Tables[0].Rows[i]["Marketersid"].ToString();
                        uobj.PhoneNo = ds.Tables[0].Rows[i]["PhoneNo"].ToString();
                        uobj.AdmissionStatus = ds.Tables[0].Rows[i]["AdmissionStatus"].ToString();
                        uobj.Reason = ds.Tables[0].Rows[i]["Reason"].ToString();

                        ReferralList.Add(uobj); 
                    } 
                    ViewBag.ReferralDetailList = ReferralList;
                }
                con.Close();
            }

            return PartialView("REferralDetail");
        }



        [HttpPost]
        public ActionResult GenerateMarketersReportAction(string FromDate, string ToDate, int OfficeId)
        {
            string result = "";
            var ObjStatus = "";

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("GenerateMarketersMilageReport", con);
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

              //  string remoteUri = ConfigurationManager.AppSettings["ClientMarketingsBatchScheduling"].ToString(), myStringWebResource = "";

                //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

                string ExcelUploadPath = ConfigurationManager.AppSettings["MarketersExportedFilePath"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["MarketersDownlLoadFilePath"].ToString();

                var fileName = "ReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
                var caseFile = ExcelUploadPath + "\\" + fileName;

                // Response.Write(caseFile); 
                FileInfo newFile = new FileInfo(caseFile);

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {

                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

                        ReportDataSheet.Cells["A0:Y1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:Y1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);


                        ReportDataSheet.Cells[1, 1].Value = "MarketersName";
                        ReportDataSheet.Cells[1, 2].Value = "ClientName";
                       // ReportDataSheet.Cells[1, 3].Value = "Payroll-Id";
                        ReportDataSheet.Cells[1, 3].Value = "Marketer Address";
                        ReportDataSheet.Cells[1, 4].Value = "Client Address";
                        ReportDataSheet.Cells[1, 5].Value = "Visits Date";
                        ReportDataSheet.Cells[1, 6].Value = "DrivingStartTime";
                        ReportDataSheet.Cells[1, 7].Value = "DrivingStartLatitude";
                        ReportDataSheet.Cells[1, 8].Value = "DrivingStartLongitude";
                        ReportDataSheet.Cells[1, 9].Value = "DrivingStopTime";
                        ReportDataSheet.Cells[1, 10].Value = "DrivingStopLatitude";
                        ReportDataSheet.Cells[1, 11].Value = "DrivingStopLongitude";
                        ReportDataSheet.Cells[1, 12].Value = "Status";
                       // ReportDataSheet.Cells[1, 13].Value = "Manual Submission";
                        ReportDataSheet.Cells[1, 13].Value = "TotalTravelTime";
                        ReportDataSheet.Cells[1, 14].Value = "TotalDistance";
                        ReportDataSheet.Cells[1, 15].Value = "ShortestDistance";
                        ReportDataSheet.Cells[1, 16].Value = "CheckInDateTime";
                        ReportDataSheet.Cells[1, 17].Value = "CheckOutDateTime";
                        ReportDataSheet.Cells[1, 18].Value = "TotalCheckInTime";
                        ReportDataSheet.Cells[1, 19].Value = "Office";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        ReportDataSheet.Cells[1, 20].Value = "GoogleShortestTime";
                        // ReportDataSheet.Cells[1, 21].Value = "Pause & Resume";
                      //  ReportDataSheet.Cells[1, 20].Value = "Request Type";
                      //  ReportDataSheet.Cells[1, 23].Value = "Visit Type";
                        ReportDataSheet.Cells[1, 21].Value = "MarketersId";
                        ReportDataSheet.Cells[1, 22].Value = "Total Driving Time";
                        ReportDataSheet.Cells[1, 23].Value = "Review";


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

                        ReportDataSheet.Column(11).Width = 40;
                        ReportDataSheet.Column(12).Width = 30;
                        ReportDataSheet.Column(13).Width = 40;
                        ReportDataSheet.Column(14).Width = 40;
                        ReportDataSheet.Column(15).Width = 40;
                        ReportDataSheet.Column(16).Width = 40;
                        ReportDataSheet.Column(17).Width = 40;
                        ReportDataSheet.Column(18).Width = 40;
                        ReportDataSheet.Column(19).Width = 40;
                        ReportDataSheet.Column(20).Width = 40;
                        ReportDataSheet.Column(21).Width = 60;
                        ReportDataSheet.Column(22).Width = 40;
                        ReportDataSheet.Column(23).Width = 40;
                        //  ReportDataSheet.Column(22).Width = 20;
                        //ReportDataSheet.Column(23).Width = 50;
                        //ReportDataSheet.Column(24).Width = 40;
                        //ReportDataSheet.Column(24).Width = 40;

                        int RowNumber = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RowNumber = (RowNumber + 1);

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["MarketersName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["ClientName"];
                            // ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["AxessId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["MarketersAddress"].ToString();
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Address"];
                            ReportDataSheet.Cells[RowNumber, 5].Value = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateOfVisit"]).ToString("yyyy-MM-dd");
                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
                            ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                            ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                            ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["Status"].ToString();

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

                            string totalchekin = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
                            string totaltrTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                            ReportDataSheet.Cells[RowNumber, 13].Value = totaltrTime;
                            ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
                            ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
                            ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();

                            ReportDataSheet.Cells[RowNumber, 18].Value = totalchekin;

                            ReportDataSheet.Cells[RowNumber, 19].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 20].Value = ds.Tables[0].Rows[i]["GoogleShortestTime"].ToString();

                            // ReportDataSheet.Cells[RowNumber, 22].Value = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                            // ReportDataSheet.Cells[RowNumber, 23].Value = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();

                            ReportDataSheet.Cells[RowNumber, 21].Value = ds.Tables[0].Rows[i]["MarketersId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 22].Value = ds.Tables[0].Rows[i]["DrivingTotalTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 23].Value = ds.Tables[0].Rows[i]["Review"].ToString();
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


        [HttpPost]
        public ActionResult GenerateMarketersReferralsReport(string FromDate, string ToDate, int OfficeId)
        {
            string result = "";
            var ObjStatus = "";

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("GenerateMarketersReferralsReport", con);
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
                        ObjStatus = MilageReferralsReportGenerate(ds);
                        return Json(ObjStatus, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        ObjStatus = "";
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


        private string MilageReferralsReportGenerate(DataSet ds)
        {
            var ObjStatus = "";


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                //  string remoteUri = ConfigurationManager.AppSettings["ClientMarketingsBatchScheduling"].ToString(), myStringWebResource = "";

                //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

                string ExcelUploadPath = ConfigurationManager.AppSettings["MarketersReferralsExportedFilePath"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["MarketersReferralsDownlLoadFilePath"].ToString();

                var fileName = "ReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
                var caseFile = ExcelUploadPath + "\\" + fileName;

                // Response.Write(caseFile); 
                FileInfo newFile = new FileInfo(caseFile);

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {

                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

                        ReportDataSheet.Cells["A0:Y1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:Y1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);


                        ReportDataSheet.Cells[1, 1].Value = "MarketersName";
                        ReportDataSheet.Cells[1, 2].Value = "ClientName";
                        // ReportDataSheet.Cells[1, 3].Value = "Payroll-Id";
                        ReportDataSheet.Cells[1, 3].Value = "Marketer Address";
                        ReportDataSheet.Cells[1, 4].Value = "Client Address";
                        ReportDataSheet.Cells[1, 5].Value = "Visits Date";
                        ReportDataSheet.Cells[1, 6].Value = "Status";
                        ReportDataSheet.Cells[1, 7].Value = "ReferralsClientName";
                        ReportDataSheet.Cells[1, 8].Value = "ReferralsVisitDates";
                        ReportDataSheet.Cells[1, 9].Value = "AdmissionStatus";
                        ReportDataSheet.Cells[1, 10].Value = "ReferralsReson";
                        ReportDataSheet.Cells[1, 11].Value = "Insurance";
                        ReportDataSheet.Cells[1, 12].Value = "PhoneNo";
                        ReportDataSheet.Cells[1, 13].Value = "OfficeName";
                        // ReportDataSheet.Cells[1, 13].Value = "Manual Submission";
                        //ReportDataSheet.Cells[1, 13].Value = "TotalTravelTime";
                        //ReportDataSheet.Cells[1, 14].Value = "TotalDistance";
                        //ReportDataSheet.Cells[1, 15].Value = "ShortestDistance";
                        //ReportDataSheet.Cells[1, 16].Value = "CheckInDateTime";
                        //ReportDataSheet.Cells[1, 17].Value = "CheckOutDateTime";
                        //ReportDataSheet.Cells[1, 18].Value = "TotalCheckInTime";
                        //ReportDataSheet.Cells[1, 19].Value = "Office";
                        ////ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        ////ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        //ReportDataSheet.Cells[1, 20].Value = "GoogleShortestTime";
                        //// ReportDataSheet.Cells[1, 21].Value = "Pause & Resume";
                        ////  ReportDataSheet.Cells[1, 20].Value = "Request Type";
                        ////  ReportDataSheet.Cells[1, 23].Value = "Visit Type";
                        //ReportDataSheet.Cells[1, 21].Value = "MarketersId";
                        //ReportDataSheet.Cells[1, 22].Value = "Total Driving Time";
                        ReportDataSheet.Cells[1, 14].Value = "Review";


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

                        ReportDataSheet.Column(11).Width = 40;
                        ReportDataSheet.Column(12).Width = 30;
                        ReportDataSheet.Column(13).Width = 40;
                        ReportDataSheet.Column(14).Width = 40;
                        //ReportDataSheet.Column(14).Width = 40;
                        //ReportDataSheet.Column(15).Width = 40;
                        //ReportDataSheet.Column(16).Width = 40;
                        //ReportDataSheet.Column(17).Width = 40;
                        //ReportDataSheet.Column(18).Width = 40;
                        //ReportDataSheet.Column(19).Width = 40;
                        //ReportDataSheet.Column(20).Width = 40;
                        //ReportDataSheet.Column(21).Width = 60;
                        //ReportDataSheet.Column(22).Width = 40;
                        //ReportDataSheet.Column(23).Width = 40;
                        //  ReportDataSheet.Column(22).Width = 20;
                        //ReportDataSheet.Column(23).Width = 50;
                        //ReportDataSheet.Column(24).Width = 40;
                        //ReportDataSheet.Column(24).Width = 40;

                        int RowNumber = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RowNumber = (RowNumber + 1);

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["MarketersName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["ClientName"];
                            // ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["AxessId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["MarketersAddress"].ToString();
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Address"];
                            ReportDataSheet.Cells[RowNumber, 5].Value = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateOfVisit"]).ToString("yyyy-MM-dd");
                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["Status"].ToString();
                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["ReferralsClientName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["VisitDate"].ToString();
                            ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["AdmissionStatus"].ToString();
                            ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["Reason"].ToString();
                            ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["Insurance"].ToString();
                            ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["PhoneNo"].ToString();
                            ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();



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

                            //ReportDataSheet.Cells[RowNumber, 13].Value = totaltrTime;
                            //ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();

                            //ReportDataSheet.Cells[RowNumber, 18].Value = totalchekin;

                            //ReportDataSheet.Cells[RowNumber, 19].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 20].Value = ds.Tables[0].Rows[i]["GoogleShortestTime"].ToString();

                            //// ReportDataSheet.Cells[RowNumber, 22].Value = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                            //// ReportDataSheet.Cells[RowNumber, 23].Value = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();

                            //ReportDataSheet.Cells[RowNumber, 21].Value = ds.Tables[0].Rows[i]["MarketersId"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 22].Value = ds.Tables[0].Rows[i]["DrivingTotalTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["Review"].ToString();
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



        [HttpPost]
        public JsonResult ListOffices()
        {
            List<AttendanceManagementDetails> officelist = new List<AttendanceManagementDetails>();

            List<SelectListItem> listItem = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {

                SqlCommand cmd = new SqlCommand("OnlyOfficesList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        AttendanceManagementDetails objofficelist = new AttendanceManagementDetails();
                        objofficelist.OfficeId = Convert.ToInt32(dr["officeId"]);
                        objofficelist.OfficeName = dr["officeName"].ToString();

                        officelist.Add(objofficelist);
                    }
                }
            }
            var officeslist = officelist;
            foreach (var offlist in officelist)
            {
                listItem.Add(new SelectListItem
                {
                    Text = offlist.OfficeName,
                    Value = (offlist.OfficeId).ToString(),
                });
            }
            return Json(listItem);
        }


    }
}