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
using Excel1 = Microsoft.Office.Interop.Excel;
using System.Reflection;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Configuration;
using DifferenzLibrary;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Office.Word;

namespace CaregiverLite.Controllers
{
     
    [SessionExpire]
    public class AttendanceManagementController : Controller
    {
        // GET: AttendanceManagement
        public ActionResult AttendanceList()
        {
            FillAllOffices();
            FillAllOrganisations();
            return View();
        }


        public ActionResult getAttendanceList(JQueryDataTableParamModel param)
        {

            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            AttendanceDetailsList attendanceList = new AttendanceDetailsList();

            try
            {

                int FilterOfficeId = 0;

                if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);



                if (sortColumnIndex == 0)
                {
                    sortOrder = "Name";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "PatientName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "Date";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "FromTime";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "ToTime";
                }
                //else if (sortColumnIndex == 5)
                //{
                //    sortOrder = " ";
                //}
                else if (sortColumnIndex == 6)
                {
                    sortOrder = "CheckInTotalTime";
                }
                else if (sortColumnIndex == 7)
                {
                    sortOrder = "TotalHours";
                }
                else
                {
                    sortOrder = "PatientRequestId";
                }


                //if (sortColumnIndex == 0)
                //{
                //    sortOrder = "PatientName";
                //}
                //else if (sortColumnIndex == 1)
                //{
                //    sortOrder = "FromTime";
                //}
                //else if (sortColumnIndex == 2)
                //{
                //    sortOrder = "ToTime";
                //}
                //else
                //{
                //    sortOrder = "Name";

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

                int OrganisationId = 0;
                if (!string.IsNullOrEmpty(Request["FilterOrganisationId"]))
                {
                    OrganisationId = Convert.ToInt32(Request["FilterOrganisationId"]);
                }
                else
                {
                    OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                }

                AttendanceManagementServiceProxy AttendanceDetailLiteService = new AttendanceManagementServiceProxy();
                // attendanceList = AttendanceDetailLiteService.GetAllAttendanceDetail(UserID, pageNo, recordPerPage, search).Result;

                //attendanceList = GetAttendanceList24(UserID, Convert.ToString(pageNo), Convert.ToString(recordPerPage), search);
                attendanceList = AttendanceDetailLiteService.GetAllAttendanceDetail(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId, OrganisationId).Result;



            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetCareGiverList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (attendanceList.AttendanceManagemenList != null)
            {
                var result = from C in attendanceList.AttendanceManagemenList select new[] { C, C, C, C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = attendanceList.TotalNumberofRecord,
                    iTotalDisplayRecords = attendanceList.TotalNumberofRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = attendanceList.TotalNumberofRecord,
                    iTotalDisplayRecords = attendanceList.TotalNumberofRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }


        private void FillAllOffices(object SelectedValue = null)
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            try
            {
                string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
                OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();
                var lstOffices = officeServiceProxy.GetAllOffices(logInUserId, OrganisationId.ToString()).Result;
                SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName", SelectedValue);
                ViewBag.lstOffice = officeSelectList;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }


        [HttpGet]
        public ActionResult ExcelReport()
        {
            return PartialView("ExcelReport");
        }



        [HttpGet]
        public ActionResult PayrollExport()
        {
            return PartialView("PayrollExport");
        }

        [HttpGet]
        public ActionResult BillingReportExport()
        {
            return PartialView("BillingReportExport");
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




        [HttpGet]
        public ActionResult ExcelReportPauseResume()
        {
            return PartialView("ExcelReportPauseResume");
        }


        [HttpPost]
        public ActionResult ExcelReportPauseResume(AttendanceManagementDetail objAttendanceManagementDetails)
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




        public ActionResult AttendanceDetail(string PatientRequestID)
        {

            AttendanceManagementDetails objAttendanceManagementDetails = new AttendanceManagementDetails();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAttendanceDetails_Vin", con))
                {
                    string CaregiverName = "";
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
                    string PatientRequestid = "";
                    string CheckoutFormReason = "";

                    List<string> ADLs = new List<string>();
                    List<string> IADLs = new List<string>();


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientRequestID", PatientRequestID);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<AttendanceManagementDetails> AttendanceList = new List<AttendanceManagementDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AttendanceManagementDetails uobj = new AttendanceManagementDetails();
                        uobj.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        PatientRequestid= ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        //uobj.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        uobj.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        uobj.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        uobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        uobj.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        uobj.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStopTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()))
                        {
                            startDrivingDatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStartTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            StopDrivingDatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStopTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');

                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()))
                        {
                            checkInDatetTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckInDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            checkOutDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckOutDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString()))
                        {
                            startDrivingLattitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLatitude"]);
                            stopDrivinglatittude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLatitude"]);
                            startDrivingLongitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLongitude"]);
                            stopDrivingLongitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLongitude"]);
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInLatitude"].ToString()))
                        {
                            checkinLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLatitude"]);
                            checkinLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLongitude"]);

                            checkoutLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLatitude"]);
                            checkoutLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLongitude"]);
                        }

                        CaregiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                        AppointmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("yyyy-MM-dd");
                        IsManaul = Convert.ToString(ds.Tables[0].Rows[i]["ismanual"]);

                        if (Convert.ToInt32(Session["OrganisationId"]) > 0)
                         {
                            if (ds.Tables[0].Columns.Contains("CheckoutFormReason") && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckoutFormReason"].ToString()))
                            {

                                CheckoutFormReason = ds.Tables[0].Rows[i]["CheckoutFormReason"].ToString();
                                ReasonForCompleteRequests response = GetAllReasonForCompleteRequests();

                                string[] Chekoutlist = CheckoutFormReason.Split(';');

                                HashSet<string> checkoutSet = new HashSet<string>();

                                for (int x = 0; x <= Chekoutlist.Length - 1; x++)
                                {
                                    checkoutSet.Add(Chekoutlist[x].TrimStart());
                                }

                                //var checkoutSet = new HashSet<string>(Chekoutlist);
                                
                                foreach (var adl in response.ADLs)
                                {
                                    if (checkoutSet.Contains(adl.CompleteRequestsReason))
                                    {
                                        ADLs.Add(adl.CompleteRequestsReason);
                                    }
                                }

                                foreach (var iadl in response.IADLs)
                                {
                                    if (checkoutSet.Contains(iadl.CompleteRequestsReason))
                                    {
                                        IADLs.Add(iadl.CompleteRequestsReason);
                                    }
                                }

                                //for (int x = 0; x <= Chekoutlist.Length; x++)
                                //{
                                //    for(int y=0; y < response.ADLs.Count; y++)
                                //    {
                                //        if (response.ADLs[y].CompleteRequestsReason == Chekoutlist[x])
                                //        {
                                //            ADLs.Add(Chekoutlist[x]);
                                //        }
                                //    }
                                //    for (int z = 0; z < response.IADLs.Count; z++)
                                //    {
                                //        if (response.IADLs[z].CompleteRequestsReason == Chekoutlist[x])
                                //        {
                                //            IADLs.Add(Chekoutlist[x]);
                                //        }
                                //    }
                                //}
                              
                            }
                        }

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


                        if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString())))
                        {
                            string BaseImagePath = ConfigurationManager.AppSettings["SignatureImageDownload"].ToString();
                            ViewBag.PatientSignature = Path.Combine(BaseImagePath, Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString()));

                        }

                        uobj.TotalTravelTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                        AttendanceList.Add(uobj);
                    }
                    // objAttendanceManagementDetails.AttendanceManagementInfo = AttendanceList;
                    ViewBag.AttendanceDetailList = AttendanceList;
                    ViewBag.CaregiverName = CaregiverName;
                    ViewBag.AppoitnmentDate = AppointmentDate;
                    ViewBag.IsManaul = IsManaul;
                    ViewBag.ADLs = ADLs;
                    ViewBag.IADLs = IADLs;

                    ViewBag.OrgnaisationId = Convert.ToInt32(Session["OrganisationId"]);

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

                    ViewBag.PatientRequestid = PatientRequestid;


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

            return PartialView("AttendanceDetail");
        }


        public ReasonForCompleteRequests GetAllReasonForCompleteRequests()
        {
            // Initialize response object

            ReasonForCompleteRequests response = new ReasonForCompleteRequests();
            // Fetch the data
            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllReasonForCompleteRequests");

            // Validate the dataset
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

            return response;
        }


        public ActionResult AttendanceDetailsManaul(string PatientRequestID)
        {

            AttendanceManagementDetails objAttendanceManagementDetails = new AttendanceManagementDetails();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAttendanceDetails_Vin", con))
                {
                    string CaregiverName = "";
                    string AppointmentDate = "";
                    string IsManaul = "";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientRequestID", PatientRequestID);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<AttendanceManagementDetails> AttendanceList = new List<AttendanceManagementDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AttendanceManagementDetails uobj = new AttendanceManagementDetails();
                        uobj.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        //uobj.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        uobj.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        uobj.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        uobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        uobj.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        uobj.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();

                        CaregiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                        AppointmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("yyyy-MM-dd");
                        IsManaul = Convert.ToString(ds.Tables[0].Rows[i]["ismanual"]);



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


                        AttendanceList.Add(uobj);
                    }
                    // objAttendanceManagementDetails.AttendanceManagementInfo = AttendanceList;
                    ViewBag.AttendanceDetailList = AttendanceList;
                    ViewBag.CaregiverName = CaregiverName;
                    ViewBag.AppoitnmentDate = AppointmentDate;
                    ViewBag.IsManaul = IsManaul;

                }
                con.Close();
            }

            return PartialView("AttendanceDetailsManaul");

        }


        public ActionResult AttendanceDetailAllStatus(string PatientRequestID)
        {

            AttendanceManagementDetails objAttendanceManagementDetails = new AttendanceManagementDetails();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAttendanceDetails_Vin", con))
                {
                    string CaregiverName = "";
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
                    string PatientRequestid = "";
                    string PatientSignature = "";



                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientRequestID", PatientRequestID);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<AttendanceManagementDetails> AttendanceList = new List<AttendanceManagementDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AttendanceManagementDetails uobj = new AttendanceManagementDetails();
                        uobj.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        PatientRequestid = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        //uobj.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        uobj.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        uobj.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        uobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        uobj.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        uobj.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStopTime"].ToString())) /*&& !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()))*/
                        {
                            startDrivingDatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStartTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            StopDrivingDatetime  = Convert.ToDateTime(ds.Tables[0].Rows[i]["DrivingStopTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInDateTime"].ToString()) ) /*&& !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString()))*/
                        {
                            checkInDatetTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckInDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/');
                            checkOutDateTime = (ds.Tables[0].Rows[i]["CheckOutDateTime"]).ToString()!=""? Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckOutDateTime"]).ToString("dd/MM/yyyy HH:mm").Replace('-', '/'):"";
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString()))
                        {
                            startDrivingLattitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLatitude"]);
                            stopDrivinglatittude  = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLatitude"]);
                            startDrivingLongitude = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStartLongitude"]);
                            stopDrivingLongitude  = Convert.ToString(ds.Tables[0].Rows[i]["DrivingStopLongitude"]);
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CheckInLatitude"].ToString()))
                        {
                            checkinLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLatitude"]);
                            checkinLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckInLongitude"]);

                            checkoutLatittude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLatitude"]);
                            checkoutLongitude = Convert.ToString(ds.Tables[0].Rows[i]["CheckOutLongitude"]);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString())))
                        {
                            string BaseImagePath = ConfigurationManager.AppSettings["SignatureImageDownload"].ToString();
                           ViewBag.PatientSignature = Path.Combine(BaseImagePath, Convert.ToString(ds.Tables[0].Rows[i]["PatientSignature"].ToString()));
                        }


                        CaregiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                        AppointmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("yyyy-MM-dd");
                        IsManaul = Convert.ToString(ds.Tables[0].Rows[i]["ismanual"]);

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

                        AttendanceList.Add(uobj);
                    }

                    // objAttendanceManagementDetails.AttendanceManagementInfo = AttendanceList;
                    ViewBag.AttendanceDetailList = AttendanceList;
                    ViewBag.CaregiverName = CaregiverName;
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

                    if (!string.IsNullOrEmpty(checkInDatetTime.Split(' ')[0]) /*&& !string.IsNullOrEmpty(checkOutDateTime.Split(' ')[0])*/)
                    {
                        ViewBag.checkInTime = checkInDatetTime.Split(' ')[1];
                        ViewBag.checkOutTime = checkOutDateTime!=""?checkOutDateTime.Split(' ')[1]:"";
                    }

                    ViewBag.startDrivingLattitude = startDrivingLattitude;
                    ViewBag.startDrivingLongitude = startDrivingLongitude;

                    ViewBag.stopDrivinglatittude = stopDrivinglatittude;
                    ViewBag.stopDrivingLongitude = stopDrivingLongitude;

                    ViewBag.checkinLatittude = checkinLatittude;
                    ViewBag.checkinLongitude = checkinLongitude;

                    ViewBag.checkoutLatittude = checkoutLatittude;
                    ViewBag.checkoutLongitude = checkoutLongitude;

                    ViewBag.PatientRequestid = PatientRequestid;

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

            return PartialView("AttendanceDetailAllStatus");

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




        //[HttpPost]
        //public JsonResult TrackLocationByPatientRequestId(int PatientRequestId)
        //{
        //    var origin = "";
        //    var destination = "";

        //    string Miles = "";

        //    List<CareGiverTrackLocation> CareGiverTrackLocationList = new List<CareGiverTrackLocation>();
        //    Dictionary<string, object> res = new Dictionary<string, object>();
        //    try
        //    {


        //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
        //        SqlCommand cmd = new SqlCommand("GetTrackLocationByPatientRequestId", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@PatientRequestId", PatientRequestId);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        DataSet ds = new DataSet();

        //        con.Open();
        //        da.Fill(ds);

        //        int lastindex = ds.Tables[0].Rows.Count;
        //        origin = ds.Tables[0].Rows[0]["LocationLatitude"].ToString() + ',' + ds.Tables[0].Rows[0]["LocationLongitude"].ToString();

        //        destination = ds.Tables[0].Rows[lastindex - 1]["LocationLatitude"].ToString() + ',' + ds.Tables[0].Rows[lastindex - 1]["LocationLongitude"].ToString();

        //        //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTrackLocationByPatientRequestId",
        //        //                                       PatientRequestId);

        //        Miles= GetLatlongDistanceData(origin, destination).Result;

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {

        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                CareGiverTrackLocation objCareGiverTrackLocation = new CareGiverTrackLocation();
        //                objCareGiverTrackLocation.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]);
        //                objCareGiverTrackLocation.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);


        //                //DateTime.Parse(Convert.ToString(item["Date"])).ToString("MM/dd/yyyy");


        //                objCareGiverTrackLocation.LocationDateTime = DateTime.Parse(ds.Tables[0].Rows[i]["LocationDateTime"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
        //                objCareGiverTrackLocation.LocationLatitude = ds.Tables[0].Rows[i]["LocationLatitude"].ToString();
        //                objCareGiverTrackLocation.LocationLongitude = ds.Tables[0].Rows[i]["LocationLongitude"].ToString();
        //                objCareGiverTrackLocation.Status = ds.Tables[0].Rows[i]["Status"].ToString();


        //                //objCareGiverTrackLocation.TotalMiles = ds.Tables[0].Rows[i]["TotalMiles"].ToString();

        //                // objCareGiverTrackLocation.TotalMiles = GetLatlongDistanceData(origin, destination).Result;
        //                objCareGiverTrackLocation.TotalMiles = Miles;
        //                CareGiverTrackLocationList.Add(objCareGiverTrackLocation);
        //            }

        //            if (PatientRequestId > 0)
        //            {

        //                res["Success"] = true;
        //                res["Result"] = CareGiverTrackLocationList;
        //            }

        //            else
        //            {
        //                res["Success"] = false;
        //                res["Result"] = "Patient Request Id not found";
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "PatientRequestController";
        //        log.Methodname = "GetNurseList";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string result = ErrorLogService.InsertErrorLog(log).Result;
        //        res["Success"] = false;
        //        res["Result"] = result;
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}



        //public async Task<string> GetLatlongDistanceData(string origin, string destination)
        //{

        //    double xyz = 0;
        //    string distance = "";
        //    //double jkf =Convert.ToDouble(origin);

        //    //double abc = Convert.ToDouble(destination);

        //    var clientGetDialogId = new System.Net.Http.HttpClient();

        //    var requestUrl = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&key={2}&origins={0}&destinations={1}", origin, destination, "AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");

        //    // clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=41.6880133,-71.1579393&destinations=+&key=AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
        //    // clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&key={2}&origins={0}&destinations={origin}", origin,destination&key=+'AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc');

        //    clientGetDialogId.BaseAddress = new Uri(requestUrl);
        //    clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
        //    var response1 = await clientGetDialogId.GetAsync("");
        //    var result1 = response1.Content.ReadAsStringAsync().Result;
        //    var data = (JObject)JsonConvert.DeserializeObject(result1);

        //    foreach (var row in data["rows"])
        //    {
        //        foreach (var elements in row["elements"])
        //        {
        //            foreach (var dist in elements["distance"])
        //            {

        //                distance = (string)dist;
        //                break;
        //            }

        //        }
        //    }


        //    return distance;



        //}












        public ActionResult TestClientMap()
        {
            return View();
        }



        //public static DataSet GetPatientDetailsListForExcel()
        //{
        //    DataSet Ds = new DataSet();
        //    try
        //    {
        //        DataSet tmpDs = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAttendanceDetailsListForExcel_Vin");

        //        if (tmpDs != null && tmpDs.Tables.Count > 0 && tmpDs.Tables[0].Rows.Count > 0)
        //        {
        //            Ds = tmpDs;
        //            return Ds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetPatientDetailsListForExcel";
        //        //string result = InsertErrorLog(objErrorlog);
        //        return Ds;
        //    }
        //    return Ds;
        //}

        // Export data from database in excel -- Vinod Verma 

        #region ExportDataSetToExcel

        //public string ExportDataSetToExcel(string strOfficeId)
        //{

        //    string result = "";
        //    var ObjStatus = "";
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        //string UserID = Membership.GetUser().ProviderUserKey.ToString();
        //        // int officeId = 0;
        //        // int.TryParse(strOfficeId, out officeId);
        //        // ds = CaregiverLiteService.GetPatientDetailsListForExcel(UserID, officeId);
        //        ds = GetPatientDetailsListForExcel();


        //        if (ds != null)
        //        {   //F:\HardikMasalawala\WorkOn\PaSeva_PGW_Latest\CaregiverLite\CaregiverLite\Content\ExportedFile\
        //            //string ExcelUploadPath = ConfigurationManager.AppSettings["ExportedFilePath"].ToString();// Server.MapPath("")+ "\\Content\\ExportedFile\\";// ConfigurationManager.AppSettings["DealerSchemeSalesUploadPath"].ToString();
        //            string ExcelUploadPath = "C:\\Users\\vinod\\Development\\Master\\CaregiverLite\\CaregiverLite\\Content\\ExportedFile\\";
        //            string ExcelPath = ConfigurationManager.AppSettings["DownlLoadFilePath"].ToString();


        //            var fileName = "AttendanceDetails_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
        //            //File Path
        //             var path = ExcelUploadPath + fileName;
        //            // var path = Path.Combine(Server.MapPath("~/Content"), fileName);
        //            System.IO.File.Create(path);
        //             var files = new DirectoryInfo(ExcelUploadPath).GetFiles();
        //            foreach (var file in files)
        //            {
        //                System.IO.File.Delete(file.FullName);
        //            }


        //            FileInfo newFile = new FileInfo(path);
        //            ExcelPackage workbook = new ExcelPackage(newFile);
        //            var PatientDataSheet = workbook.Workbook.Worksheets.Add("AttendanceDetail");

        //            //For Write Column Name In Excel
        //            PatientDataSheet.Cells[1, 1].Value = "Patient Request Id";
        //            PatientDataSheet.Cells[1, 2].Value = "Date";
        //            PatientDataSheet.Cells[1, 3].Value = "Patient Name";
        //            PatientDataSheet.Cells[1, 4].Value = "Address";
        //            PatientDataSheet.Cells[1, 5].Value = "FromTime";
        //            PatientDataSheet.Cells[1, 6].Value = "ToTime";
        //            PatientDataSheet.Cells[1, 7].Value = "DrivingStartTime";
        //            PatientDataSheet.Cells[1, 8].Value = "DrivingStopTime";
        //           // PatientDataSheet.Cells[1, 9].Value = "Office";

        //            PatientDataSheet.Column(1).Width = 40;
        //            PatientDataSheet.Column(2).Width = 20;
        //            PatientDataSheet.Column(3).Width = 50;
        //            PatientDataSheet.Column(4).Width = 30;
        //            PatientDataSheet.Column(5).Width = 10;
        //            PatientDataSheet.Column(6).Width = 10;
        //            PatientDataSheet.Column(7).Width = 20;
        //            PatientDataSheet.Column(8).Width = 40;
        //           // PatientDataSheet.Column(9).Width = 40;

        //            PatientDataSheet.Row(1).Style.Font.Bold = true;
        //            PatientDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.Red);


        //            int RowNumber = 1;
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                RowNumber = (RowNumber + 1);

        //                PatientDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
        //                PatientDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["Date"];
        //                PatientDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["PatientName"].ToString();
        //                PatientDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Address"].ToString();
        //                PatientDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["FromTime"].ToString();
        //                PatientDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["ToTime"].ToString();
        //                PatientDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
        //                PatientDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
        //               // PatientDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
        //            }

        //            PatientDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            PatientDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            PatientDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            PatientDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


        //            PatientDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //            PatientDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //            PatientDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //            PatientDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

        //            workbook.Save();

        //            string ExcelUrl = ExcelPath + fileName;
        //            ObjStatus = ExcelUrl;


        //        }
        //        else
        //        {

        //        }
        //        return ObjStatus;
        //    }
        //    catch (Exception e)
        //    {
        //        result = "Fail";
        //        string msg = e.Message;
        //    }
        //    return ObjStatus;
        //}

        #endregion


        public ActionResult ExportDataSetToExcel()
        {

            int i = 0;
            int j = 0;
            string sql = null;
            string data = null;
            Excel1.Application xlApp;
            Excel1.Workbook xlWorkBook;
            Excel1.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel1.Application();
            xlApp.Visible = false;
            xlWorkBook = (Excel1.Workbook)(xlApp.Workbooks.Add(Missing.Value));
            xlWorkSheet = (Excel1.Worksheet)xlWorkBook.ActiveSheet;
            string conn = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                var cmd = new SqlCommand("SELECT TOP 0 PatientRequestId, Date, FromTime, ToTime FROM PatientRequest", con);
                var reader = cmd.ExecuteReader();
                int k = 0;
                for (i = 0; i < reader.FieldCount; i++)
                {
                    data = (reader.GetName(i));
                    xlWorkSheet.Cells[1, k + 1] = data;
                    k++;
                }
                char lastColumn = (char)(65 + reader.FieldCount - 1);
                xlWorkSheet.get_Range("A1", lastColumn + "1").Font.Bold = true;
                xlWorkSheet.get_Range("A1",
                lastColumn + "1").VerticalAlignment = Excel1.XlVAlign.xlVAlignCenter;
                reader.Close();

                sql = "SELECT TOP(200) PatientRequestId, Date, FromTime, ToTime FROM PatientRequest";
                SqlDataAdapter dscmd = new SqlDataAdapter(sql, con);
                DataSet ds = new DataSet();
                dscmd.Fill(ds);
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    var newj = 0;
                    for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                    {
                        data = ds.Tables[0].Rows[i].ItemArray[j].ToString();

                        xlWorkSheet.Cells[i + 2, newj + 1] = data;
                        newj++;
                    }
                }
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

            }
            return RedirectToAction("AttendanceList", "AttendanceManagement");
            // return RedirectToAction("Index", "ExportToExcel");

        }


        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
                //MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


        [HttpGet]
        public ActionResult GetMilageReport(string FromDate, string ToDate, int OfficeId)
        {


            string result = "";
            var ObjStatus = "";

            int OrganisationId = 0;
                
                //Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {


                    //SqlCommand cmd = new SqlCommand("GenerateMilageReport", con);

                    //  SqlCommand cmd = new SqlCommand("ORG_GenerateMilageReport_Optimize", con);


                    using (SqlCommand cmd = new SqlCommand("ORG_GenerateMilageReport", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);
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



        [HttpPost]
        public ActionResult GenerateReportAction(string FromDate, string ToDate, int OfficeId)
        {


            string result = "";
            var ObjStatus = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {


                    //SqlCommand cmd = new SqlCommand("GenerateMilageReport", con);

                    //  SqlCommand cmd = new SqlCommand("ORG_GenerateMilageReport_Optimize", con);


                    using (SqlCommand cmd = new SqlCommand("ORG_GenerateMilageReport", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);
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

                string ExcelUploadPath = ConfigurationManager.AppSettings["ExportedFilePath"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["DownlLoadFilePath"].ToString();
                var fileName = "ReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
                var caseFile = ExcelUploadPath + "\\" + fileName;

                // Response.Write(caseFile); 
                FileInfo newFile = new FileInfo(caseFile);

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {

                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

                        ReportDataSheet.Cells["A0:Z1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:Z1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);


                        ReportDataSheet.Cells[1, 1].Value = "NurseName";
                        ReportDataSheet.Cells[1, 2].Value = "PatientName";
                        ReportDataSheet.Cells[1, 3].Value = "Patient ID";
                        ReportDataSheet.Cells[1, 4].Value = "Payroll-Id";
                        ReportDataSheet.Cells[1, 5].Value = "Nurse Address";
                        ReportDataSheet.Cells[1, 6].Value = "Patient Address";
                        ReportDataSheet.Cells[1, 7].Value = "DrivingStartTime";
                        ReportDataSheet.Cells[1, 8].Value = "DrivingStartLatitude";
                        ReportDataSheet.Cells[1, 9].Value = "DrivingStartLongitude";
                        ReportDataSheet.Cells[1, 10].Value = "DrivingStopTime";
                        ReportDataSheet.Cells[1, 11].Value = "DrivingStopLatitude";
                        ReportDataSheet.Cells[1, 12].Value = "DrivingStopLongitude";
                        ReportDataSheet.Cells[1, 13].Value = "Status";
                        ReportDataSheet.Cells[1, 14].Value = "Manual Submission";
                        ReportDataSheet.Cells[1, 15].Value = "TotalTravelTime";
                        ReportDataSheet.Cells[1, 16].Value = "TotalDistance";
                        ReportDataSheet.Cells[1, 17].Value = "ShortestDistance";
                        ReportDataSheet.Cells[1, 18].Value = "CheckInDateTime";
                        ReportDataSheet.Cells[1, 19].Value = "CheckOutDateTime";
                        ReportDataSheet.Cells[1, 20].Value = "TotalCheckInTime";
                        ReportDataSheet.Cells[1, 21].Value = "Office";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        ReportDataSheet.Cells[1, 22].Value = "GoogleShortestTime";
                        // ReportDataSheet.Cells[1, 21].Value = "Pause & Resume";
                        ReportDataSheet.Cells[1, 23].Value = "Request Type";
                        ReportDataSheet.Cells[1, 24].Value = "Visit Type";
                        ReportDataSheet.Cells[1, 25].Value = "NurseId";
                        ReportDataSheet.Cells[1, 26].Value = "Total Driving Time";





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
                        ReportDataSheet.Column(22).Width = 20;
                        ReportDataSheet.Column(23).Width = 50;
                        ReportDataSheet.Column(24).Width = 40;
                        ReportDataSheet.Column(25).Width = 40;

                        int RowNumber = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RowNumber = (RowNumber + 1);

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"];
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["MedicalId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["AxessId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["NurseAddress"].ToString();
                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["Address"];
                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
                            ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
                            ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                            ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                            ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["Status"].ToString();

                            if (ds.Tables[0].Rows[i]["ismanual"].ToString() == "1")
                            {
                                ReportDataSheet.Cells[RowNumber, 14].Value = "true";

                                ReportDataSheet.Cells[RowNumber, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ReportDataSheet.Cells[RowNumber, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                ReportDataSheet.Cells[RowNumber, 14].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                ReportDataSheet.Column(14).Style.Font.Bold = true;

                            }
                            else
                            {
                                ReportDataSheet.Cells[RowNumber, 14].Value = " ";
                                ReportDataSheet.Cells[RowNumber, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ReportDataSheet.Cells[RowNumber, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                ReportDataSheet.Cells[RowNumber, 14].Style.Border.Top.Color.SetColor(System.Drawing.Color.White);
                                ReportDataSheet.Column(14).Style.Font.Bold = true;
                            }

                            string totalchekin = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
                            string totaltrTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();


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


                            ReportDataSheet.Cells[RowNumber, 15].Value = totaltrTime;
                            //ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                            //  ReportDataSheet.Cells[RowNumber, 11].Value = totalTravel(Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]));
                            //ReportDataSheet.Cells[RowNumber, 12].Value = totalTravel(11923);
                            ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
                            ReportDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
                            ReportDataSheet.Cells[RowNumber, 18].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 19].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();

                            ReportDataSheet.Cells[RowNumber, 20].Value = totalchekin;
                            //ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();

                            ReportDataSheet.Cells[RowNumber, 21].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                            //if (Totalworkingtime != "")
                            //{
                            //    ReportDataSheet.Cells[RowNumber, 19].Value = Totalworkingtime;
                            //}
                            ReportDataSheet.Cells[RowNumber, 22].Value = ds.Tables[0].Rows[i]["GoogleShortestTime"].ToString();
                            //   ReportDataSheet.Cells[RowNumber, 21].Value = ds.Tables[0].Rows[i]["PAUSE_RESUME"].ToString();

                            ReportDataSheet.Cells[RowNumber, 23].Value = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 24].Value = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 25].Value = ds.Tables[0].Rows[i]["NurseId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 26].Value = ds.Tables[0].Rows[i]["DrivingTotalTime"].ToString();


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
        public ActionResult GeneratePayrollExport(string FromDate, string ToDate, int OfficeId, string checkboxdata)
        {


            string result = "";
            var ObjStatus = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {

                    //SqlCommand cmd = new SqlCommand("GenerateMilageReport", con);
                    using (SqlCommand cmd = new SqlCommand("ORG_PayrollMilageReport", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);


                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            // var ObjStatus = MilageReportGenerate(ds);
                            //return Json(ObjStatus);  
                            //Json(MilageReportGenerate(ds));
                            ObjStatus = PayrollExportMilage(ds, checkboxdata);
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



        private string PayrollExportMilage(DataSet ds, string checkboxdata)
        {
            var ObjStatus = "";
            string filePath = string.Empty;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string ExcelUploadPath = ConfigurationManager.AppSettings["PayrollExportedFilePath"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["PayrollDownlLoadFilePath"].ToString();
                var fileName = "PayrollExportData_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
                var caseFile = ExcelUploadPath + "\\" + fileName;

                FileInfo newFile = new FileInfo(caseFile);

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {
                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

                        if (checkboxdata == "false")
                        {
                            ReportDataSheet.Cells["A0:J1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ReportDataSheet.Cells["A0:J1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                        }
                        else
                        {
                            ReportDataSheet.Cells["A0:K1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ReportDataSheet.Cells["A0:K1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                        }

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

                        ReportDataSheet.Cells[1, 1].Value = "Employee ID";
                        ReportDataSheet.Cells[1, 2].Value = "First Name";
                        ReportDataSheet.Cells[1, 3].Value = "Last Name";
                        ReportDataSheet.Cells[1, 4].Value = "Email";
                        ReportDataSheet.Cells[1, 5].Value = "Phone";
                        ReportDataSheet.Cells[1, 6].Value = "Office";
                        ReportDataSheet.Cells[1, 7].Value = "Date Start";
                        ReportDataSheet.Cells[1, 8].Value = "Date End";
                        ReportDataSheet.Cells[1, 9].Value = "Total Hours";
                        ReportDataSheet.Cells[1, 10].Value = "Scheduled Hours";

                        if (checkboxdata == "true")
                        {
                            ReportDataSheet.Cells[1, 11].Value = "Total Miles";
                        }

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
                        if (checkboxdata == "true")
                        {
                            ReportDataSheet.Column(11).Width = 40;
                        }

                        int RowNumber = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RowNumber++;

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["FirstName"];
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["LastName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Email"].ToString();
                            ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["phone"];
                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["Fromdate"].ToString();
                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["Todate"].ToString();

                            double totalHours;
                            double.TryParse(ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString(), out totalHours);
                            ReportDataSheet.Cells[RowNumber, 9].Value = totalHours;

                            double scheduledHours;
                            double.TryParse(ds.Tables[0].Rows[i]["TotalScheduledTime"].ToString(), out scheduledHours);
                            ReportDataSheet.Cells[RowNumber, 10].Value = scheduledHours;

                            if (checkboxdata == "true")
                            {
                                double totalMiles;
                                double.TryParse(ds.Tables[0].Rows[i]["drivingTotalMile"].ToString(), out totalMiles);
                                ReportDataSheet.Cells[RowNumber, 11].Value = totalMiles;
                            }
                        }

                        RowNumber++;

                        // Add Excel SUM formula for Total Hours and Scheduled Hours
                        // Add Excel SUM formulas
                        //ReportDataSheet.Cells[RowNumber, 9].Formula = $"SUM(I2:I{RowNumber - 1})";
                        //ReportDataSheet.Cells[RowNumber, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //ReportDataSheet.Cells[RowNumber, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                        //ReportDataSheet.Cells[RowNumber, 9].Style.Font.Bold = true;

                        //ReportDataSheet.Cells[RowNumber, 10].Formula = $"SUM(J2:J{RowNumber - 1})";
                        //ReportDataSheet.Cells[RowNumber, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //ReportDataSheet.Cells[RowNumber, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                        //ReportDataSheet.Cells[RowNumber, 10].Style.Font.Bold = true;

                        RowNumber++;

                        // Add labeled total (formula + text)
                        ReportDataSheet.Cells[RowNumber, 9].Formula = $"\"Actual billed hours: \" & TEXT(SUM(I2:I{RowNumber - 2}),\"0.00\")";
                        ReportDataSheet.Cells[RowNumber, 10].Formula = $"\"Scheduled hours: \" & TEXT(SUM(J2:J{RowNumber - 2}),\"0.00\")";

                        ReportDataSheet.Cells[RowNumber, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells[RowNumber, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);
                        ReportDataSheet.Cells[RowNumber, 9].Style.Font.Bold = true;

                        ReportDataSheet.Cells[RowNumber, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells[RowNumber, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);
                        ReportDataSheet.Cells[RowNumber, 10].Style.Font.Bold = true;



                        ReportDataSheet.Cells[RowNumber, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells[RowNumber, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);
                        ReportDataSheet.Cells[RowNumber, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells[RowNumber, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);

                        // Apply borders
                        ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ReportDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        ReportDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                        excelPackage.SaveAs(newFile);
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




        //previous one

        //private string PayrollExportMilage(DataSet ds, string checkboxdata)
        //{
        //    var ObjStatus = "";
        //    string filePath = string.Empty;


        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

        //        string ExcelUploadPath = ConfigurationManager.AppSettings["PayrollExportedFilePath"].ToString();
        //        string ExcelPath = ConfigurationManager.AppSettings["PayrollDownlLoadFilePath"].ToString();
        //        var fileName = "PayrollExportData_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
        //        var caseFile = ExcelUploadPath + "\\" + fileName;


        //        //string path = Server.MapPath("~/BulkSchedulingExcel/");

        //        //if (!Directory.Exists(path))
        //        //{
        //        //    Directory.CreateDirectory(path);
        //        //}

        //        ////if (!Directory.Exists(path2))
        //        ////{
        //        ////    Directory.CreateDirectory(path2);
        //        ////}

        //        //filePath = path + "PayrollExportData_" + DateTime.Now.ToString("MM.dd.yy hh.mm.ss") + "_" + ".xlsx";
        //        ////string extension = Path.GetExtension(file.FileName);
        //        //file.SaveAs(filePath);



        //        // Response.Write(caseFile); 
        //        FileInfo newFile = new FileInfo(caseFile);

        //        try
        //        {
        //            using (ExcelPackage excelPackage = new ExcelPackage(newFile))
        //            {

        //                var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

        //                if (checkboxdata == "false")
        //                {
        //                    ReportDataSheet.Cells["A0:J1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    ReportDataSheet.Cells["A0:J1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
        //                }
        //                else
        //                {
        //                    ReportDataSheet.Cells["A0:K1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    ReportDataSheet.Cells["A0:K1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

        //                }

        //                ReportDataSheet.Row(1).Style.Font.Bold = true;
        //                ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);


        //                ReportDataSheet.Cells[1, 1].Value = "Employee ID";
        //                ReportDataSheet.Cells[1, 2].Value = "First Name";
        //                ReportDataSheet.Cells[1, 3].Value = "Last Name";
        //                ReportDataSheet.Cells[1, 4].Value = "Email";
        //                ReportDataSheet.Cells[1, 5].Value = "Phone";
        //                ReportDataSheet.Cells[1, 6].Value = "Office";
        //                ReportDataSheet.Cells[1, 7].Value = "Date Start";
        //                ReportDataSheet.Cells[1, 8].Value = "Date End";
        //                ReportDataSheet.Cells[1, 9].Value = "Total Hours";
        //                ReportDataSheet.Cells[1, 10].Value = "scheduled hours";

        //                if (checkboxdata == "true")
        //                {
        //                    ReportDataSheet.Cells[1, 11].Value = "Total Miles";

        //                }


        //                //ReportDataSheet.Cells[1, 11].Value = "DrivingStopLongitude";
        //                //ReportDataSheet.Cells[1, 12].Value = "Status";
        //                //ReportDataSheet.Cells[1, 13].Value = "Manual Submission";
        //                //ReportDataSheet.Cells[1, 14].Value = "TotalTravelTime";
        //                //ReportDataSheet.Cells[1, 15].Value = "TotalDistance";
        //                //ReportDataSheet.Cells[1, 16].Value = "ShortestDistance";
        //                //ReportDataSheet.Cells[1, 17].Value = "CheckInDateTime";
        //                //ReportDataSheet.Cells[1, 18].Value = "CheckOutDateTime";
        //                //ReportDataSheet.Cells[1, 19].Value = "TotalCheckInTime";
        //                //ReportDataSheet.Cells[1, 20].Value = "Office";
        //                ////ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
        //                ////ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
        //                //ReportDataSheet.Cells[1, 21].Value = "GoogleShortestTime";
        //                //// ReportDataSheet.Cells[1, 21].Value = "Pause & Resume";
        //                //ReportDataSheet.Cells[1, 22].Value = "Request Type";
        //                //ReportDataSheet.Cells[1, 23].Value = "Visit Type";
        //                //ReportDataSheet.Cells[1, 24].Value = "NurseId";
        //                //ReportDataSheet.Cells[1, 25].Value = "Total Driving Time";





        //                ReportDataSheet.Column(1).Width = 40;
        //                ReportDataSheet.Column(2).Width = 40;
        //                ReportDataSheet.Column(3).Width = 50;
        //                ReportDataSheet.Column(4).Width = 40;
        //                ReportDataSheet.Column(5).Width = 40;
        //                ReportDataSheet.Column(6).Width = 40;
        //                ReportDataSheet.Column(7).Width = 30;
        //                ReportDataSheet.Column(8).Width = 40;
        //                ReportDataSheet.Column(9).Width = 40;
        //                ReportDataSheet.Column(10).Width = 40;
        //                if (checkboxdata == "true")
        //                {
        //                    ReportDataSheet.Column(11).Width = 40;

        //                }
        //                //ReportDataSheet.Column(11).Width = 40;
        //                //ReportDataSheet.Column(12).Width = 30;
        //                //ReportDataSheet.Column(13).Width = 40;
        //                //ReportDataSheet.Column(14).Width = 40;
        //                //ReportDataSheet.Column(15).Width = 40;
        //                //ReportDataSheet.Column(16).Width = 40;
        //                //ReportDataSheet.Column(17).Width = 40;
        //                //ReportDataSheet.Column(18).Width = 40;
        //                //ReportDataSheet.Column(19).Width = 40;
        //                //ReportDataSheet.Column(20).Width = 40;
        //                //ReportDataSheet.Column(21).Width = 60;
        //                //ReportDataSheet.Column(22).Width = 20;
        //                //ReportDataSheet.Column(23).Width = 50;
        //                //ReportDataSheet.Column(24).Width = 40;
        //                //ReportDataSheet.Column(24).Width = 40;

        //                int RowNumber = 1;
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    RowNumber = (RowNumber + 1);

        //                    ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseId"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["FirstName"];
        //                    ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["LastName"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Email"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["phone"];
        //                    ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["Fromdate"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["Todate"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["TotalScheduledTime"].ToString();

        //                    if (checkboxdata == "true")
        //                    {
        //                        ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["drivingTotalMile"].ToString();

        //                    }     

        //                }

        //                RowNumber += 1;

        //                //ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[0]["TotalDuration"].ToString();

        //                //ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[0]["TotalScheduledDuration"].ToString();

        //                ReportDataSheet.Cells[RowNumber, 9].Value = $"Actual billing (billed) hours: " + (ds.Tables[0].Rows[0]["TotalDuration"] != DBNull.Value
        //                        ? ds.Tables[0].Rows[0]["TotalDuration"].ToString()
        //                        : "0");

        //                //ds.Tables[0].Rows[ds.Tables[0].Rows.Count + 1]["TotalCheckinBillHours"].ToString();


        //                ReportDataSheet.Cells[RowNumber, 10].Value = $"Total Scheduled hours: " + (ds.Tables[0].Rows[0]["Totalscheduledduration"] != DBNull.Value
        //                        ? ds.Tables[0].Rows[0]["Totalscheduledduration"].ToString()
        //                        : "0");

        //                ReportDataSheet.Cells[RowNumber, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

        //                ReportDataSheet.Cells[RowNumber, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);

        //                ReportDataSheet.Cells[RowNumber, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells[RowNumber, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);




        //                ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


        //                ReportDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

        //                excelPackage.SaveAs(newFile);
        //                //  excelPackage.Save(); 

        //                string ExcelUrl = ExcelPath + fileName;
        //                ObjStatus = ExcelUrl;
        //                return ObjStatus;
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            throw (ex);


        //        }
        //    }

        //    else
        //    {
        //        ObjStatus = "";

        //    }



        //    return ObjStatus;



        //}





        [HttpPost]
        public ActionResult GenerateBillingReportExport(string FromDate, string ToDate, int OfficeId)
        {


            string result = "";
            var ObjStatus = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {


                    //SqlCommand cmd = new SqlCommand("GenerateMilageReport", con);
                    using (SqlCommand cmd = new SqlCommand("ORG_BillingReport", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);


                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            // var ObjStatus = MilageReportGenerate(ds);
                            //return Json(ObjStatus);  
                            //Json(MilageReportGenerate(ds));
                            // ObjStatus = PayrollExportMilage(ds, checkboxdata);

                            ObjStatus = BillingExportMilage(ds);
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



        private string BillingExportMilage(DataSet ds)
        {
            string status = "";
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return status;

            var table = ds.Tables[0];

            string exportPath = ConfigurationManager.AppSettings["BillingExportedFilePath"];
            string downloadPath = ConfigurationManager.AppSettings["BillingDownlLoadFilePath"];
            string fileName = "BillingReportData_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
            string fullPath = Path.Combine(exportPath, fileName);

            FileInfo newFile = new FileInfo(fullPath);
            if (newFile.Exists) newFile.IsReadOnly = false;

            using (var package = new ExcelPackage(newFile))
            {
                var sheet = package.Workbook.Worksheets.Add("ReportFromToDate");

                string[] headers = new[] {
            "PatientId", "Patient Name", "Caregiver Name", "Office", "Appointment Date",
            "Check in time", "Check out time", "Duration", "Scheduled start time",
            "Scheduled end time", "Scheduled duration"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    sheet.Cells[1, i + 1].Value = headers[i];
                    sheet.Column(i + 1).Width = 50;
                    sheet.Cells[1, i + 1].Style.Font.Bold = true;
                    sheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    sheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                    sheet.Cells[1, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                var groupedData = table.AsEnumerable()
                    .Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("caregiverName")))
                    .GroupBy(r => r.Field<string>("caregiverName").Trim());

                int rowIndex = 2;
                double grandTotalDuration = 0, grandTotalScheduled = 0;

                foreach (var caregiverGroup in groupedData)
                {
                    double groupDuration = 0, groupScheduled = 0;

                    foreach (var row in caregiverGroup)
                    {
                        sheet.Cells[rowIndex, 1].Value = row["PatientId"] != null ? row["PatientId"].ToString() : "";
                        sheet.Cells[rowIndex, 2].Value = row["PatientName"] != null ? row["PatientName"].ToString() : "";
                        sheet.Cells[rowIndex, 3].Value = row["caregiverName"] != null ? row["caregiverName"].ToString() : "";
                        sheet.Cells[rowIndex, 4].Value = row["officename"] != null ? row["officename"].ToString() : "";

                        DateTime? appointmentDate = row.IsNull("AppointmentDate") ? (DateTime?)null : row.Field<DateTime>("AppointmentDate");
                        sheet.Cells[rowIndex, 5].Value = appointmentDate.HasValue ? appointmentDate.Value.ToString("MM/dd/yyyy") : "";

                        DateTime? checkIn = row.IsNull("CheckIndatetime") ? (DateTime?)null : row.Field<DateTime>("CheckIndatetime");
                        sheet.Cells[rowIndex, 6].Value = checkIn.HasValue ? checkIn.Value.ToString("MM/dd/yyyy hh:mm") : "";

                        DateTime? checkOut = row.IsNull("CheckoutDatetime") ? (DateTime?)null : row.Field<DateTime>("CheckoutDatetime");
                        sheet.Cells[rowIndex, 7].Value = checkOut.HasValue ? checkOut.Value.ToString("MM/dd/yyyy hh:mm") : "";

                        double d = 0;
                        double s = 0;

                        string durationStr = row["Duration"] != null ? row["Duration"].ToString() : "0";
                        string scheduledStr = row["scheduledduration"] != null ? row["scheduledduration"].ToString() : "0";

                        double duration = double.TryParse(durationStr, out d) ? d : 0;
                        double scheduled = double.TryParse(scheduledStr, out s) ? s : 0;

                        sheet.Cells[rowIndex, 8].Value = duration;
                        sheet.Cells[rowIndex, 9].Value = row["scheduledstarttime"] != null ? row["scheduledstarttime"].ToString() : "";
                        sheet.Cells[rowIndex, 10].Value = row["scheduledendtime"] != null ? row["scheduledendtime"].ToString() : "";
                        sheet.Cells[rowIndex, 11].Value = scheduled;

                        groupDuration += duration;
                        groupScheduled += scheduled;

                        rowIndex++;
                    }

                    //sheet.Cells[rowIndex, 8].Value = $"Actual billed hours ({caregiverGroup.Key}): {caregiverTotalDuration:0.00}";

                    // Group total row
                    sheet.Cells[rowIndex, 8].Value = $"Billed Hours Total ({caregiverGroup.Key}): {groupDuration:0.00}";
                    sheet.Cells[rowIndex, 11].Value = $"Scheduled Hours Total ({caregiverGroup.Key}): {groupScheduled:0.00}";


                   sheet.Cells[rowIndex, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                   sheet.Cells[rowIndex, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                   sheet.Cells[rowIndex, 8].Style.Font.Bold = true;
                  
                   sheet.Cells[rowIndex, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                   sheet.Cells[rowIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                   sheet.Cells[rowIndex, 11].Style.Font.Bold = true;
                   
                   sheet.Cells[rowIndex, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                   sheet.Cells[rowIndex, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
               
                    sheet.Cells[rowIndex, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    sheet.Cells[rowIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);


                    rowIndex += 1; // Add spacing row

                    grandTotalDuration += groupDuration;
                    grandTotalScheduled += groupScheduled;
                }


                rowIndex += 2;
                // Grand total row
                sheet.Cells[rowIndex, 8].Value = $"Grand Total Billed Hours: {grandTotalDuration:0.00}";
                sheet.Cells[rowIndex, 11].Value = $"Grand Total Scheduled Hours: {grandTotalScheduled:0.00}";

                sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);
                sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Font.Bold = true;

                // Borders
                using (var range = sheet.Cells[1, 1, rowIndex, 11])
                {
                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                package.Save();
            }

            return Path.Combine(downloadPath, fileName);
        }



        //private string BillingExportMilage(DataSet ds)
        //{
        //    string ObjStatus = "";
        //    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //        return "";

        //    string ExcelUploadPath = ConfigurationManager.AppSettings["BillingExportedFilePath"];
        //    string ExcelPath = ConfigurationManager.AppSettings["BillingDownlLoadFilePath"];
        //    var fileName = "BillingReportData_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
        //    var filePath = Path.Combine(ExcelUploadPath, fileName);

        //    FileInfo newFile = new FileInfo(filePath);
        //    if (newFile.Exists) newFile.IsReadOnly = false;

        //    try
        //    {
        //        using (ExcelPackage excelPackage = new ExcelPackage(newFile))
        //        {
        //            var sheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

        //            // Headers
        //            string[] headers = { "PatientId", "Patient Name", "Caregiver Name", "Office", "Appointment Date", "Check In Time", "Check Out Time", "Duration", "Scheduled Start Time", "Scheduled End Time", "Scheduled Duration" };
        //            for (int i = 0; i < headers.Length; i++)
        //            {
        //                sheet.Cells[1, i + 1].Value = headers[i];
        //                sheet.Column(i + 1).Width = 40;
        //            }

        //            using (var range = sheet.Cells["A1:K1"])
        //            {
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
        //                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
        //                range.Style.Font.Bold = true;
        //            }

        //            int rowIndex = 2;


        //            // Group rows by caregiver name
        //            var groupedData = ds.Tables[0].AsEnumerable()
        //                .GroupBy(row => row["caregiverName"]?.ToString() ?? "Unknown");


        //            foreach (var caregiverGroup in groupedData)
        //            {
        //                double caregiverTotalDuration = 0;
        //                double caregiverScheduledTotal = 0;

        //                foreach (var row in caregiverGroup)
        //                {
        //                    sheet.Cells[rowIndex, 1].Value = row["PatientId"]?.ToString() ?? "";
        //                    sheet.Cells[rowIndex, 2].Value = row["PatientName"]?.ToString() ?? "";
        //                    sheet.Cells[rowIndex, 3].Value = row["caregiverName"]?.ToString() ?? "";
        //                    sheet.Cells[rowIndex, 4].Value = row["officename"]?.ToString() ?? "";
        //                    sheet.Cells[rowIndex, 5].Value = row["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(row["AppointmentDate"]).ToString("MM/dd/yyyy") : "";
        //                    sheet.Cells[rowIndex, 6].Value = row["CheckIndatetime"] != DBNull.Value ? Convert.ToDateTime(row["CheckIndatetime"]).ToString("MM/dd/yyyy hh:mm") : "";
        //                    sheet.Cells[rowIndex, 7].Value = row["CheckoutDatetime"] != DBNull.Value ? Convert.ToDateTime(row["CheckoutDatetime"]).ToString("MM/dd/yyyy hh:mm") : "";

        //                    double duration = 0;
        //                    double.TryParse(row["Duration"]?.ToString(), out duration);
        //                    sheet.Cells[rowIndex, 8].Value = duration;
        //                    caregiverTotalDuration += duration;

        //                    sheet.Cells[rowIndex, 9].Value = row["scheduledstarttime"]?.ToString() ?? "";
        //                    sheet.Cells[rowIndex, 10].Value = row["scheduledendtime"]?.ToString() ?? "";

        //                    double scheduledDuration = 0;
        //                    double.TryParse(row["scheduledduration"]?.ToString(), out scheduledDuration);
        //                    sheet.Cells[rowIndex, 11].Value = scheduledDuration;
        //                    caregiverScheduledTotal += scheduledDuration;

        //                    rowIndex++;
        //                    rowIndex++;

        //                    // Insert subtotal row after this caregiver

        //                }

        //                sheet.Cells[rowIndex, 8].Value = $"Actual billed hours ({caregiverGroup.Key}): {caregiverTotalDuration:0.00}";
        //                sheet.Cells[rowIndex, 11].Value = $"Scheduled hours ({caregiverGroup.Key}): {caregiverScheduledTotal:0.00}";

        //                rowIndex++;
        //                rowIndex++;

        //                //// Insert subtotal row after this caregiver
        //                //sheet.Cells[rowIndex, 8].Value = $"Actual billed hours ({caregiverGroup.Key}): {caregiverTotalDuration:0.00}";
        //                //sheet.Cells[rowIndex, 11].Value = $"Scheduled hours ({caregiverGroup.Key}): {caregiverScheduledTotal:0.00}";

        //                // Style subtotal
        //                var subtotalCells = new[] { sheet.Cells[rowIndex, 8], sheet.Cells[rowIndex, 11] };
        //                foreach (var cell in subtotalCells)
        //                {
        //                    cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    //ExcelFillStyle.Solid;

        //                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
        //                    cell.Style.Font.Bold = true;
        //                }


        //            }
        //                rowIndex += 4; // leave one blank row between groups



        //            sheet.Cells[rowIndex, 8].Formula = $"\"grandTotalDuration: \" & TEXT(SUM(H2:H{rowIndex - 2}),\"0.00\")";
        //            sheet.Cells[rowIndex, 11].Formula = $"\"grandTotalDuration: \" & TEXT(SUM(K2:K{rowIndex - 2}),\"0.00\")";


        //           // sheet.Cells[rowIndex, 8].Value = $"Grand Total Billed Hours: {grandTotalDuration:0.00}";
        //           // sheet.Cells[rowIndex, 11].Value = $"Grand Total Scheduled Hours: {grandTotalScheduled:0.00}";

        //            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
        //            sheet.Cells[rowIndex, 8, rowIndex, 11].Style.Font.Bold = true;

        //            // Borders
        //            var allCells = sheet.Cells[1, 1, rowIndex, 11];
        //            allCells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            allCells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            allCells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            allCells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            allCells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //            allCells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //            allCells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //            allCells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

        //            excelPackage.SaveAs(newFile);
        //            ObjStatus = Path.Combine(ExcelPath, fileName);
        //        }

        //        return ObjStatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        //private string BillingExportMilage(DataSet ds)
        //{
        //    var ObjStatus = "";
        //    string filePath = string.Empty;

        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

        //        string ExcelUploadPath = ConfigurationManager.AppSettings["BillingExportedFilePath"].ToString();
        //        string ExcelPath = ConfigurationManager.AppSettings["BillingDownlLoadFilePath"].ToString();
        //        var fileName = "BillingReportData_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
        //        var caseFile = ExcelUploadPath + "\\" + fileName;


        //        FileInfo newFile = new FileInfo(caseFile);

        //        if (newFile.Exists)
        //        {
        //            newFile.IsReadOnly = false;
        //        }

        //        try
        //        {
        //            using (ExcelPackage excelPackage = new ExcelPackage(newFile))
        //            {

        //                var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

        //                ReportDataSheet.Cells["A0:K1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells["A0:K1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

        //                ReportDataSheet.Row(1).Style.Font.Bold = true;
        //                ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

        //                ReportDataSheet.Cells[1, 1].Value = "PatientId";
        //                ReportDataSheet.Cells[1, 2].Value = "Patient Name";
        //                ReportDataSheet.Cells[1, 3].Value = "caregiver Name";
        //                ReportDataSheet.Cells[1, 4].Value = "Office";
        //                ReportDataSheet.Cells[1, 5].Value = "Appointment Date";
        //                ReportDataSheet.Cells[1, 6].Value = "Check in time";
        //                ReportDataSheet.Cells[1, 7].Value = "Check out time";
        //                ReportDataSheet.Cells[1, 8].Value = "Duration";
        //                ReportDataSheet.Cells[1, 9].Value = "Scheduled start time";
        //                ReportDataSheet.Cells[1, 10].Value = "Scheduled end time";
        //                ReportDataSheet.Cells[1, 11].Value = "Scheduled duration";

        //                ReportDataSheet.Column(1).Width = 40;
        //                ReportDataSheet.Column(2).Width = 40;
        //                ReportDataSheet.Column(3).Width = 50;
        //                ReportDataSheet.Column(4).Width = 40;
        //                ReportDataSheet.Column(5).Width = 40;
        //                ReportDataSheet.Column(6).Width = 50;
        //                ReportDataSheet.Column(7).Width = 50;
        //                ReportDataSheet.Column(8).Width = 50;
        //                ReportDataSheet.Column(9).Width = 40;
        //                ReportDataSheet.Column(10).Width = 40;
        //                ReportDataSheet.Column(11).Width = 50;

        //                int RowNumber = 1;
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    RowNumber = (RowNumber + 1);

        //                    //ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["PatientId"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"];
        //                    //ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["caregiverName"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["officename"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 5].Value = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["AppointmentDate"]).ToString("MM/dd/yyyy"));
        //                    //ReportDataSheet.Cells[RowNumber, 6].Value = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckIndatetime"]).ToString("MM/dd/yyyy hh:mm"));
        //                    //ReportDataSheet.Cells[RowNumber, 7].Value = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckoutDatetime"]).ToString("MM/dd/yyyy hh:mm"));
        //                    //ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["Duration"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["scheduledstarttime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["scheduledendtime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["scheduledduration"].ToString();

        //                    ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["PatientId"] != DBNull.Value ? ds.Tables[0].Rows[i]["PatientId"].ToString() : "";
        //                    ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"] != DBNull.Value ? ds.Tables[0].Rows[i]["PatientName"].ToString() : "";
        //                    ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["caregiverName"] != DBNull.Value ? ds.Tables[0].Rows[i]["caregiverName"].ToString() : "";
        //                    ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["officename"] != DBNull.Value ? ds.Tables[0].Rows[i]["officename"].ToString() : "";

        //                    ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["AppointmentDate"] != DBNull.Value
        //                        ? Convert.ToDateTime(ds.Tables[0].Rows[i]["AppointmentDate"]).ToString("MM/dd/yyyy")
        //                        : "";

        //                    ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["CheckIndatetime"] != DBNull.Value
        //                        ? Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckIndatetime"]).ToString("MM/dd/yyyy hh:mm")
        //                        : "";

        //                    ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["CheckoutDatetime"] != DBNull.Value
        //                        ? Convert.ToDateTime(ds.Tables[0].Rows[i]["CheckoutDatetime"]).ToString("MM/dd/yyyy hh:mm")
        //                        : "";


        //                    //ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["Duration"] != DBNull.Value
        //                    //    ? ds.Tables[0].Rows[i]["Duration"].ToString()
        //                    //    : "";

        //                    double DurationtotalHours = 0;

        //                    // Safely parse "Duration" column to double if it has a valid numeric value
        //                    var durationValue = ds.Tables[0].Rows[i]["Duration"];

        //                    if (durationValue != DBNull.Value && !string.IsNullOrWhiteSpace(durationValue.ToString()))
        //                    {
        //                        double.TryParse(durationValue.ToString(), out DurationtotalHours);
        //                    }
        //                    else
        //                    {
        //                        DurationtotalHours = 0; // or any default fallback
        //                    }

        //                    // Write it to the Excel cell (e.g., as double or string as needed)
        //                    ReportDataSheet.Cells[RowNumber, 8].Value = DurationtotalHours;



        //                    ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["scheduledstarttime"] != DBNull.Value
        //                        ? ds.Tables[0].Rows[i]["scheduledstarttime"].ToString()
        //                        : "";

        //                    ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["scheduledendtime"] != DBNull.Value
        //                        ? ds.Tables[0].Rows[i]["scheduledendtime"].ToString()
        //                        : "";


        //                    double ScheduledurationtotalHours = 0;


        //                    var DurationtotalHoursValue = ds.Tables[0].Rows[i]["scheduledduration"];

        //                    if (DurationtotalHoursValue != DBNull.Value && !string.IsNullOrWhiteSpace(DurationtotalHoursValue.ToString()))
        //                    {
        //                        double.TryParse(DurationtotalHoursValue.ToString(), out ScheduledurationtotalHours);
        //                    }
        //                    else
        //                    {
        //                        ScheduledurationtotalHours = 0; 
        //                    }


        //                    ReportDataSheet.Cells[RowNumber, 11].Value = ScheduledurationtotalHours;



        //                    //ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["scheduledduration"] != DBNull.Value
        //                    //    ? ds.Tables[0].Rows[i]["scheduledduration"].ToString()
        //                    //    : "";

        //                }

        //                RowNumber++;

        //                //ReportDataSheet.Cells[RowNumber, 8].Value = $"Actual billing (billed) hours: " + ( ds.Tables[0].Rows[0]["TotalDuration"] != DBNull.Value
        //                //        ? ds.Tables[0].Rows[0]["TotalDuration"].ToString()
        //                //        : "0");


        //                //ReportDataSheet.Cells[RowNumber, 11].Value = $"Total Scheduled hours: " + ( ds.Tables[0].Rows[0]["Totalscheduledduration"] != DBNull.Value
        //                //        ? ds.Tables[0].Rows[0]["Totalscheduledduration"].ToString()
        //                //        : "0");

        //                RowNumber++;


        //                ReportDataSheet.Cells[RowNumber, 8].Formula = $"\"Actual billed hours: \" & TEXT(SUM(H2:H{RowNumber - 2}),\"0.00\")";
        //                ReportDataSheet.Cells[RowNumber, 11].Formula = $"\"Scheduled hours: \" & TEXT(SUM(K2:K{RowNumber - 2}),\"0.00\")";

        //                ReportDataSheet.Cells[RowNumber, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells[RowNumber, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);
        //                ReportDataSheet.Cells[RowNumber, 8].Style.Font.Bold = true;

        //                ReportDataSheet.Cells[RowNumber, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells[RowNumber, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);
        //                ReportDataSheet.Cells[RowNumber, 11].Style.Font.Bold = true;

        //                ReportDataSheet.Cells[RowNumber, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells[RowNumber, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);

        //                ReportDataSheet.Cells[RowNumber, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells[RowNumber, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.GreenYellow);


        //                ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //                ReportDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

        //                excelPackage.SaveAs(newFile);                      

        //                string ExcelUrl = ExcelPath + fileName;
        //                ObjStatus = ExcelUrl;
        //                return ObjStatus;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw (ex);
        //        }
        //    }
        //    else
        //    {
        //        ObjStatus = "";
        //    }

        //    return ObjStatus;

        //}





        //private string MilageReportGenerate(DataSet ds)
        //{
        //    var ObjStatus = "";
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        //  string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        //        string ExcelUploadPath = ConfigurationManager.AppSettings["ExportedFilePath"].ToString();
        //        string ExcelPath = ConfigurationManager.AppSettings["DownlLoadFilePath"].ToString();

        //        var fileName = "ReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
        //        var caseFile = ExcelUploadPath + "\\" + fileName;
        //        // Response.Write(caseFile);
        //        FileInfo newFile = new FileInfo(caseFile);
        //        try
        //        {
        //            using (ExcelPackage excelPackage = new ExcelPackage(newFile))
        //            {
        //                var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

        //                ReportDataSheet.Cells["A0:P1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells["A0:P1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

        //                ReportDataSheet.Row(1).Style.Font.Bold = true;
        //                ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

        //                ReportDataSheet.Cells[1, 1].Value = "NurseName";
        //                ReportDataSheet.Cells[1, 2].Value = "PatientName";
        //                ReportDataSheet.Cells[1, 3].Value = "Patient Address";
        //                ReportDataSheet.Cells[1, 4].Value = "DrivingStartTime";
        //                ReportDataSheet.Cells[1, 5].Value = "DrivingStartLatitude";
        //                ReportDataSheet.Cells[1, 6].Value = "DrivingStartLongitude";
        //                ReportDataSheet.Cells[1, 7].Value = "DrivingStopTime";
        //                ReportDataSheet.Cells[1, 8].Value = "DrivingStopLatitude";
        //                ReportDataSheet.Cells[1, 9].Value = "DrivingStopLongitude";
        //                ReportDataSheet.Cells[1, 10].Value = "TotalTravelTime";
        //                ReportDataSheet.Cells[1, 11].Value = "TotalDistance";
        //                ReportDataSheet.Cells[1, 12].Value = "ShortestDistance";
        //                ReportDataSheet.Cells[1, 13].Value = "CheckInDateTime";
        //                ReportDataSheet.Cells[1, 14].Value = "CheckOutDateTime";
        //                ReportDataSheet.Cells[1, 15].Value = "TotalCheckInTime";
        //                ReportDataSheet.Cells[1, 16].Value = "Office";

        //                ReportDataSheet.Column(1).Width = 40;
        //                ReportDataSheet.Column(2).Width = 40;
        //                ReportDataSheet.Column(3).Width = 50;
        //                ReportDataSheet.Column(4).Width = 30;
        //                ReportDataSheet.Column(5).Width = 40;
        //                ReportDataSheet.Column(6).Width = 40;
        //                ReportDataSheet.Column(7).Width = 30;
        //                ReportDataSheet.Column(8).Width = 40;
        //                ReportDataSheet.Column(9).Width = 40;
        //                ReportDataSheet.Column(10).Width = 40;

        //                ReportDataSheet.Column(11).Width = 40;
        //                ReportDataSheet.Column(12).Width = 30;
        //                ReportDataSheet.Column(13).Width = 40;
        //                ReportDataSheet.Column(14).Width = 40;
        //                ReportDataSheet.Column(15).Width = 40;
        //                ReportDataSheet.Column(16).Width = 40;

        //                int RowNumber = 1;
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    RowNumber = (RowNumber + 1);
        //                    ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"];
        //                    ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["Address"];
        //                    ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

        //                    ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();

        //                    //ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
        //                }

        //                ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

        //                excelPackage.SaveAs(newFile);
        //                //  excelPackage.Save(); 
        //                string ExcelUrl = ExcelPath + fileName;
        //                ObjStatus = ExcelUrl;
        //                return ObjStatus;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw (ex);
        //        }
        //    }
        //    else
        //    {
        //        ObjStatus = "";
        //    }
        //    return ObjStatus;
        //}  


        [HttpPost]
        public JsonResult ListOffices()
        {
            List<AttendanceManagementDetails> officelist = new List<AttendanceManagementDetails>();

            List<SelectListItem> listItem = new List<SelectListItem>();
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {

                // SqlCommand cmd = new SqlCommand("OnlyOfficesList", con);

                using (SqlCommand cmd = new SqlCommand("ORG_OnlyOfficesList", con))
                {

                    cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);


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


        //  [HttpPost]
        public JsonResult GetCaregiverByOfficeId(int OfficeId)
        {
            List<CareGiverModel> officelist = new List<CareGiverModel>();

            List<SelectListItem> listItem = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {

                using (SqlCommand cmd = new SqlCommand("GetCaregiverByOfficeId", con))
                {
                    cmd.Parameters.AddWithValue("@OfficeId", OfficeId);

                    cmd.CommandType = CommandType.StoredProcedure;


                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    con.Open();

                    //SqlDataReader dr = cmd.ExecuteReader();

                    listItem.Add(new SelectListItem { Text = "Select Caregiver", Value = "0" });

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {

                            CareGiverModel objofficelist = new CareGiverModel();
                            objofficelist.NurseId = Convert.ToInt32(item["NurseId"]);
                            objofficelist.Name = item["Name"].ToString();

                            officelist.Add(objofficelist);
                        }
                    }

                }

            }

            var officeslist = officelist;
            foreach (var offlist in officelist)
            {
                listItem.Add(new SelectListItem
                {
                    Text = offlist.Name,
                    Value = (offlist.NurseId).ToString(),

                });
            }

            return Json(listItem);
        }


        public JsonResult GetFavouritePatientByCaregiver(int NurseId)
        {
            List<PatientDetailsModel> officelist = new List<PatientDetailsModel>();

            List<SelectListItem> listItem = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {

                using (SqlCommand cmd = new SqlCommand("GetFavouritePatientListBasedOnNurseId", con))
                {
                    cmd.Parameters.AddWithValue("@NurseId", Convert.ToString(NurseId));

                    cmd.CommandType = CommandType.StoredProcedure;

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    con.Open();


                    listItem.Add(new SelectListItem { Text = "Select Favourite Patient", Value = "0" });

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {

                            PatientDetailsModel objofficelist = new PatientDetailsModel();
                            objofficelist.PatientId = Convert.ToInt32(item["PatientId"]);
                            objofficelist.PatientName = item["PatientName"].ToString();

                            officelist.Add(objofficelist);
                        }
                    }
                }
            }

            var officeslist = officelist;
            foreach (var offlist in officelist)
            {
                listItem.Add(new SelectListItem
                {
                    Text = offlist.PatientName,
                    Value = (offlist.PatientId).ToString(),

                });
            }

            return Json(listItem);

        }


        [HttpPost]
        public ActionResult GenerateReportActionForPauseAndResume(string FromDate, string ToDate, int OfficeId, int NurseId, int PatientId)
        {

            string result = "";
            var ObjStatus = "";

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))

                {
                    //  SqlCommand cmd = new SqlCommand("Get_Break_GenerateMilageReport", con);

                    using (SqlCommand cmd = new SqlCommand("GenerateMilageReportFIlterWithNuserIdandPatientId", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.Parameters.AddWithValue("@NurseId", NurseId);
                        cmd.Parameters.AddWithValue("@PatientId", PatientId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            // var ObjStatus = MilageReportGenerate(ds);
                            //return Json(ObjStatus);  

                            //Json(MilageReportGenerate(ds));
                            ObjStatus = MilageReportGenerateForPauseAndResume(ds);
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


        private string MilageReportGenerateForPauseAndResume(DataSet ds)
        {
            var ObjStatus = "";


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

                string ExcelUploadPath = ConfigurationManager.AppSettings["ExportedFilePath"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["DownlLoadFilePath"].ToString();

                var fileName = "ReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";

                var caseFile = ExcelUploadPath + "\\" + fileName;

                // Response.Write(caseFile); 

                FileInfo newFile = new FileInfo(caseFile);

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {
                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

                        ReportDataSheet.Cells["A0:K1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:K1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

                        ReportDataSheet.Cells[1, 1].Value = "PatientRequestId";
                        ReportDataSheet.Cells[1, 2].Value = "NurseName";
                        ReportDataSheet.Cells[1, 3].Value = "PatientName";
                        ReportDataSheet.Cells[1, 4].Value = "Nurse Address";

                        // ReportDataSheet.Cells[1, 5].Value = "LocationDateTime";
                        //ReportDataSheet.Cells[1, 4].Value = "Patient Address";
                        //ReportDataSheet.Cells[1, 5].Value = "DrivingStartTime";
                        //ReportDataSheet.Cells[1, 6].Value = "DrivingStartLatitude";
                        //ReportDataSheet.Cells[1, 7].Value = "DrivingStartLongitude";
                        //ReportDataSheet.Cells[1, 8].Value = "DrivingStopTime";
                        //ReportDataSheet.Cells[1, 9].Value = "DrivingStopLatitude";
                        //ReportDataSheet.Cells[1, 10].Value = "DrivingStopLongitude";


                        ReportDataSheet.Cells[1, 5].Value = "Status";
                        ReportDataSheet.Cells[1, 6].Value = "DrivingStopLongitude";
                        ReportDataSheet.Cells[1, 7].Value = "TotalTravelTime";
                        ReportDataSheet.Cells[1, 8].Value = "TotalDistance";
                        ReportDataSheet.Cells[1, 9].Value = "ShortestDistance";
                        //ReportDataSheet.Cells[1, 15].Value = "CheckInDateTime";
                        //ReportDataSheet.Cells[1, 16].Value = "CheckOutDateTime";
                        ReportDataSheet.Cells[1, 10].Value = "TotalCheckInTime";
                        // ReportDataSheet.Cells[1, 11].Value = "Office";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        ReportDataSheet.Cells[1, 11].Value = "GoogleShortestTime";
                        ReportDataSheet.Cells[1, 12].Value = "Pause & Resume";


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
                        //ReportDataSheet.Column(13).Width = 40;
                        //ReportDataSheet.Column(14).Width = 40;
                        //ReportDataSheet.Column(15).Width = 40;
                        //ReportDataSheet.Column(16).Width = 40;
                        //ReportDataSheet.Column(17).Width = 40;
                        //ReportDataSheet.Column(18).Width = 40;
                        //ReportDataSheet.Column(19).Width = 40;
                        //ReportDataSheet.Column(20).Width = 40;

                        //  ReportDataSheet.Column(19).Width = 60;



                        int RowNumber = 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RowNumber = (RowNumber + 1);

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["PatientName"];
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["NurseAddress"].ToString();


                            //ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["Address"];
                            //ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();


                            ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["Status"].ToString();
                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString(); ;



                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();

                            ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();


                            ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();

                            // ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                            // ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();


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



                            //ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

                            //  ReportDataSheet.Cells[RowNumber, 11].Value = totalTravel(Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]));
                            //ReportDataSheet.Cells[RowNumber, 12].Value = totalTravel(11923);


                            //ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();


                            //ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();




                            //if (Totalworkingtime != "")
                            //{
                            //    ReportDataSheet.Cells[RowNumber, 19].Value = Totalworkingtime;
                            //}
                            ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["GoogleShortestTime"].ToString();





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
                            ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["PAUSE_RESUME"].ToString();


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





        //[HttpPost]
        //public async Task<ActionResult> GenerateReportAction(string FromDate, string ToDate, int OfficeId)
        //{

        //    string result = "";

        //    result = await GenerateReportActionDta(FromDate, ToDate, OfficeId);


        //      return Json(result, JsonRequestBehavior.AllowGet);

        //}


        //[HttpPost]
        //public async Task<string> GenerateReportActionDta(string FromDate, string ToDate, int OfficeId)
        //{


        //    string result = "";
        //    var ObjStatus = "";

        //    try
        //    {

        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
        //        {

        //            SqlCommand cmd = new SqlCommand("GenerateMilageReport_resumepausescene", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@FromDate", FromDate);
        //            cmd.Parameters.AddWithValue("@ToDate", ToDate);
        //            cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            DataSet ds = new DataSet();
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);


        //            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                // var ObjStatus = MilageReportGenerate(ds);
        //                //return Json(ObjStatus);  

        //                //Json(MilageReportGenerate(ds));
        //              //  Task.Run(async () => { ObjStatus = MilageReportGenerate(ds); }).Wait();  
        //                ObjStatus= await MilageReportGenerate(ds);
        //              //  return Json(ObjStatus, JsonRequestBehavior.AllowGet);

        //            }
        //            else
        //            {
        //                ObjStatus = "";
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        result = "Fail";
        //        string msg = e.Message;
        //        throw (e);
        //    }

        //    //  return Json(ObjStatus, JsonRequestBehavior.AllowGet);
        //    return ObjStatus;

        //}


        //private async Task<string> MilageReportGenerate(DataSet ds)
        //{
        //    var ObjStatus = "";


        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {

        //        //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

        //        string ExcelUploadPath = ConfigurationManager.AppSettings["ExportedFilePath"].ToString();
        //        string ExcelPath = ConfigurationManager.AppSettings["DownlLoadFilePath"].ToString();

        //        var fileName = "ReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";

        //        var caseFile = ExcelUploadPath + "\\" + fileName;

        //        // Response.Write(caseFile); 


        //        FileInfo newFile = new FileInfo(caseFile);


        //        try
        //        {
        //            using (ExcelPackage excelPackage = new ExcelPackage(newFile))
        //            {

        //                var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

        //                ReportDataSheet.Cells["A0:W1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                ReportDataSheet.Cells["A0:W1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

        //                ReportDataSheet.Row(1).Style.Font.Bold = true;
        //                ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);


        //                ReportDataSheet.Cells[1, 1].Value = "NurseName";
        //                ReportDataSheet.Cells[1, 2].Value = "PatientName";
        //                ReportDataSheet.Cells[1, 3].Value = "Payroll-Id";
        //                ReportDataSheet.Cells[1, 4].Value = "Nurse Address";
        //                ReportDataSheet.Cells[1, 5].Value = "Patient Address";
        //                ReportDataSheet.Cells[1, 6].Value = "DrivingStartTime";
        //                ReportDataSheet.Cells[1, 7].Value = "DrivingStartLatitude";
        //                ReportDataSheet.Cells[1, 8].Value = "DrivingStartLongitude";
        //                ReportDataSheet.Cells[1, 9].Value = "DrivingStopTime";
        //                ReportDataSheet.Cells[1, 10].Value = "DrivingStopLatitude";
        //                ReportDataSheet.Cells[1, 11].Value = "DrivingStopLongitude";
        //                ReportDataSheet.Cells[1, 12].Value = "Status";
        //                ReportDataSheet.Cells[1, 13].Value = "TotalTravelTime";
        //                ReportDataSheet.Cells[1, 14].Value = "TotalDistance";
        //                ReportDataSheet.Cells[1, 15].Value = "ShortestDistance";
        //                ReportDataSheet.Cells[1, 16].Value = "CheckInDateTime";
        //                ReportDataSheet.Cells[1, 17].Value = "CheckOutDateTime";
        //                ReportDataSheet.Cells[1, 18].Value = "TotalCheckInTime";
        //                ReportDataSheet.Cells[1, 19].Value = "Office";
        //                //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
        //                //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
        //                ReportDataSheet.Cells[1, 20].Value = "GoogleShortestTime";
        //                ReportDataSheet.Cells[1, 21].Value = "Pause & Resume";
        //                ReportDataSheet.Cells[1, 22].Value = "Request Type";
        //                ReportDataSheet.Cells[1, 23].Value = "NurseId";



        //                ReportDataSheet.Column(1).Width = 40;
        //                ReportDataSheet.Column(2).Width = 40;
        //                ReportDataSheet.Column(3).Width = 50;
        //                ReportDataSheet.Column(4).Width = 40;
        //                ReportDataSheet.Column(5).Width = 40;
        //                ReportDataSheet.Column(6).Width = 40;
        //                ReportDataSheet.Column(7).Width = 30;
        //                ReportDataSheet.Column(8).Width = 40;
        //                ReportDataSheet.Column(9).Width = 40;
        //                ReportDataSheet.Column(10).Width = 40;

        //                ReportDataSheet.Column(11).Width = 40;
        //                ReportDataSheet.Column(12).Width = 30;
        //                ReportDataSheet.Column(13).Width = 40;
        //                ReportDataSheet.Column(14).Width = 40;
        //                ReportDataSheet.Column(15).Width = 40;
        //                ReportDataSheet.Column(16).Width = 40;
        //                ReportDataSheet.Column(17).Width = 40;
        //                ReportDataSheet.Column(18).Width = 40;
        //                ReportDataSheet.Column(19).Width = 40;
        //                ReportDataSheet.Column(20).Width = 40;
        //                ReportDataSheet.Column(21).Width = 60;
        //                ReportDataSheet.Column(22).Width = 20;
        //                ReportDataSheet.Column(23).Width = 40;



        //                int RowNumber = 1;
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

        //                {
        //                    RowNumber = (RowNumber + 1);

        //                    ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"];
        //                    ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["AxessId"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["NurseAddress"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["Address"];
        //                    ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

        //                    ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["Status"].ToString();



        //                    string totalchekin = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
        //                    string totaltrTime = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();


        //                    //if (totalchekin != "" && totaltrTime != "")
        //                    //{
        //                    //    string[] abcd = totalchekin.Split(':');

        //                    //    string a = abcd[0];
        //                    //    string b = abcd[1];
        //                    //    string c = abcd[2];
        //                    //    string d = abcd[3];

        //                    //    totalchekin = a + "Days" + b + ":" + c + ":" + d;



        //                    //    string[] defg = totaltrTime.Split(':');

        //                    //    string e = defg[0];
        //                    //    string f = defg[1];
        //                    //    string g = defg[2];
        //                    //    string h = defg[3];

        //                    //    totaltraveltime = e + "Days" + f + ":" + g + ":" + h;

        //                    //    int ab = Convert.ToInt32(a) + Convert.ToInt32(e);
        //                    //    int bc = Convert.ToInt32(b) + Convert.ToInt32(f);
        //                    //    int cd = Convert.ToInt32(c) + Convert.ToInt32(g);
        //                    //    int de = Convert.ToInt32(d) + Convert.ToInt32(h);

        //                    //    Totalworkingtime = Convert.ToString(ab) + "Days" + Convert.ToString(bc) + ':' + Convert.ToString(cd) + ':' + Convert.ToString(de);


        //                    //}


        //                    ReportDataSheet.Cells[RowNumber, 13].Value = totaltrTime;
        //                    //ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

        //                    //  ReportDataSheet.Cells[RowNumber, 11].Value = totalTravel(Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]));
        //                    //ReportDataSheet.Cells[RowNumber, 12].Value = totalTravel(11923);
        //                    ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();

        //                    ReportDataSheet.Cells[RowNumber, 18].Value = totalchekin;
        //                    //ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();

        //                    ReportDataSheet.Cells[RowNumber, 19].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();




        //                    //if (Totalworkingtime != "")
        //                    //{
        //                    //    ReportDataSheet.Cells[RowNumber, 19].Value = Totalworkingtime;
        //                    //}
        //                    ReportDataSheet.Cells[RowNumber, 20].Value = ds.Tables[0].Rows[i]["GoogleShortestTime"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 21].Value = ds.Tables[0].Rows[i]["PAUSE_RESUME"].ToString();

        //                    ReportDataSheet.Cells[RowNumber, 22].Value = ds.Tables[0].Rows[i]["ServiceName"].ToString();
        //                    ReportDataSheet.Cells[RowNumber, 23].Value = ds.Tables[0].Rows[i]["NurseId"].ToString();


        //                    //var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ReportFromToDate");

        //                    //ReportDataSheet.Cells["A0:S1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    //ReportDataSheet.Cells["A0:S1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

        //                    //ReportDataSheet.Row(1).Style.Font.Bold = true;
        //                    //ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);


        //                    //ReportDataSheet.Cells[1, 1].Value = "NurseName";
        //                    //ReportDataSheet.Cells[1, 2].Value = "PatientName";
        //                    //ReportDataSheet.Cells[1, 3].Value = "Nurse Address";
        //                    //ReportDataSheet.Cells[1, 4].Value = "Patient Address";
        //                    //ReportDataSheet.Cells[1, 5].Value = "DrivingStartTime";
        //                    //ReportDataSheet.Cells[1, 6].Value = "DrivingStartLatitude";
        //                    //ReportDataSheet.Cells[1, 7].Value = "DrivingStartLongitude";
        //                    //ReportDataSheet.Cells[1, 8].Value = "DrivingStopTime";
        //                    //ReportDataSheet.Cells[1, 9].Value = "DrivingStopLatitude";
        //                    //ReportDataSheet.Cells[1, 10].Value = "DrivingStopLongitude";
        //                    //ReportDataSheet.Cells[1, 11].Value = "Status";
        //                    //ReportDataSheet.Cells[1, 12].Value = "TotalTravelTime";
        //                    //ReportDataSheet.Cells[1, 13].Value = "TotalDistance";
        //                    //ReportDataSheet.Cells[1, 14].Value = "ShortestDistance";
        //                    //ReportDataSheet.Cells[1, 15].Value = "CheckInDateTime";
        //                    //ReportDataSheet.Cells[1, 16].Value = "CheckOutDateTime";
        //                    //ReportDataSheet.Cells[1, 17].Value = "TotalCheckInTime";
        //                    //ReportDataSheet.Cells[1, 18].Value = "Office";
        //                    ////  ReportDataSheet.Cells[1, 19].Value = "Pause & Resume";



        //                    //ReportDataSheet.Column(1).Width = 40;
        //                    //ReportDataSheet.Column(2).Width = 40;
        //                    //ReportDataSheet.Column(3).Width = 50;
        //                    //ReportDataSheet.Column(4).Width = 40;
        //                    //ReportDataSheet.Column(5).Width = 40;
        //                    //ReportDataSheet.Column(6).Width = 40;
        //                    //ReportDataSheet.Column(7).Width = 30;
        //                    //ReportDataSheet.Column(8).Width = 40;
        //                    //ReportDataSheet.Column(9).Width = 40;
        //                    //ReportDataSheet.Column(10).Width = 40;

        //                    //ReportDataSheet.Column(11).Width = 40;
        //                    //ReportDataSheet.Column(12).Width = 30;
        //                    //ReportDataSheet.Column(13).Width = 40;
        //                    //ReportDataSheet.Column(14).Width = 40;
        //                    //ReportDataSheet.Column(15).Width = 40;
        //                    //ReportDataSheet.Column(16).Width = 40;
        //                    //ReportDataSheet.Column(17).Width = 40;
        //                    //ReportDataSheet.Column(18).Width = 40;
        //                    ////  ReportDataSheet.Column(19).Width = 60;



        //                    //int RowNumber = 1;
        //                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

        //                    //{
        //                    //    RowNumber = (RowNumber + 1);

        //                    //    ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"];
        //                    //    ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["NurseAddress"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Address"];
        //                    //    ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

        //                    //    ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["Status"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();

        //                    //    //  ReportDataSheet.Cells[RowNumber, 11].Value = totalTravel(Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]));
        //                    //    //ReportDataSheet.Cells[RowNumber, 12].Value = totalTravel(11923);
        //                    //    ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
        //                    //    ReportDataSheet.Cells[RowNumber, 18].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
        //                    //  ReportDataSheet.Cells[RowNumber, 19].Value = ds.Tables[0].Rows[i]["PAUSE_RESUME"].ToString();


        //                    //ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["CheckInDateTime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["CheckOutDateTime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
        //                    //ReportDataSheet.Cells[RowNumber, 15].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
        //                }

        //                ReportDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                ReportDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


        //                ReportDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //                ReportDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

        //                excelPackage.SaveAs(newFile);
        //                //  excelPackage.Save(); 



        //                string ExcelUrl = ExcelPath + fileName;
        //                ObjStatus = ExcelUrl;
        //                return ObjStatus;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw (ex);


        //        }
        //    }

        //    else
        //    {
        //        ObjStatus = "";

        //    }



        //    return ObjStatus;



        //}




        //[HttpPost]
        //public JsonResult ListOffices()
        //{
        //    List<AttendanceManagementDetails> officelist = new List<AttendanceManagementDetails>();

        //    List<SelectListItem> listItem = new List<SelectListItem>();

        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
        //    {

        //        SqlCommand cmd = new SqlCommand("OnlyOfficesList", con);

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();

        //        SqlDataReader dr = cmd.ExecuteReader();

        //        if (dr.HasRows)
        //        {

        //            while (dr.Read())
        //            {
        //                AttendanceManagementDetails objofficelist = new AttendanceManagementDetails();
        //                objofficelist.OfficeId = Convert.ToInt32(dr["officeId"]);
        //                objofficelist.OfficeName = dr["officeName"].ToString();


        //                officelist.Add(objofficelist);

        //            }

        //        }

        //    }
        //    var officeslist = officelist;
        //    foreach (var offlist in officelist)
        //    {
        //        listItem.Add(new SelectListItem
        //        {
        //            Text = offlist.OfficeName,
        //            Value = (offlist.OfficeId).ToString(),

        //        });
        //    }

        //    return Json(listItem);

        //}



        public string SavePatientRequestCheckInOutData(AppointmentRequestDetails SchedulePatientRequest)
        {
            string result = "";
            string UserID = "";
            UserID = Membership.GetUser().ProviderUserKey.ToString();


            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateIntoCheckInOutPatientRequestData", SchedulePatientRequest.PatientRequestId,
                                                       //DateTime.ParseExact(SchedulePatientRequest.StartDrivingDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       //DateTime.ParseExact(SchedulePatientRequest.StopDrivingDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.CheckInDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.CheckOutDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       UserID
                                                     );


                if (i > 0)
                {

                    result = "success";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return result;
        }


        public string SavePatientRequestManualEntry(AppointmentRequestDetails SchedulePatientRequest)
        {
            string result = "";
            string UserID = "";
            UserID= Membership.GetUser().ProviderUserKey.ToString();

            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateIntoPatientRequestData", SchedulePatientRequest.PatientRequestId,
                                                       DateTime.ParseExact(SchedulePatientRequest.StartDrivingDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.StopDrivingDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.CheckInDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.CheckOutDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       UserID
                                                     );

                if (i > 0)
                {

                    result = "success";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return result;
        }


        public string SavePatientRequestStatusEntry(AppointmentRequestDetails SchedulePatientRequest)
        {
            string result = "";
            string UserID = "";
            UserID = Membership.GetUser().ProviderUserKey.ToString();


            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateIntoPatientRequestData", SchedulePatientRequest.PatientRequestId,
                                                       DateTime.ParseExact(SchedulePatientRequest.StartDrivingDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.StopDrivingDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.CheckInDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       DateTime.ParseExact(SchedulePatientRequest.CheckOutDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                                       UserID
                                                     );


                if (i > 0)
                {

                    result = "success";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return result;
        }






        [HttpGet]
        public ActionResult SendVisitRequestData()
        {
            return PartialView("SendVisitRequestData");
        }
        public JsonResult GetAndPostEmployeeVisitData(string FromDate, String ToDate, String OfficeId)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            string result = "";

            //string ss = "24-Nov-22 11:51:0";
            //DateTime dd1 = Convert.ToDateTime(ss);
            //string foo1 = dd1.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

            //DateTime fromDatetime = Convert.ToDateTime(FromDate);
            ////string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            //string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd'");

            //DateTime ToDatetime = Convert.ToDateTime(ToDate);
            ////string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");
            //string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd'");

            EmployeeClientModel objModel = new EmployeeClientModel();

            List<ClientVisitRequest> clientVisitRequestList = new List<ClientVisitRequest>();
            try
            {


                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeVisitRequestToaddSandDataForDateRange",
                                                                                                            // FromScheduleDateTime,
                                                                                                            // ToScheduleDateTime,
                                                                                                            FromDate,
                                                                                                            ToDate,
                                                                                                       OfficeId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int y = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            ClientVisitRequest clientVisitRequest = new ClientVisitRequest();
                            ClientVisitRequestProviderIdentification objprovider = new ClientVisitRequestProviderIdentification();
                            objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                            objprovider.ProviderQualifier = "MedicaidID";
                            clientVisitRequest.ProviderIdentification = objprovider;

                            clientVisitRequest.EmployeeQualifier = "EmployeeCustomID";
                            clientVisitRequest.EmployeeOtherID = Convert.ToString(item["NurseId"]);
                            clientVisitRequest.EmployeeIdentifier = Convert.ToString(item["NurseId"]);
                            clientVisitRequest.VisitOtherID = Convert.ToString(item["NurseScheduleId"]);

                            clientVisitRequest.SequenceID = Convert.ToString((int)(item["timestampdata"]) + y);
                            //Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
                            clientVisitRequest.GroupCode = null;//(single caregiver visits for multiple clients in same time sapn)
                            clientVisitRequest.ClientIDQualifier = "ClientMedicaidID";

                            #region
                            //string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();

                            //var stringNumber = ChkMedicalId;
                            //int numericValue;
                            //bool isNumber = int.TryParse(stringNumber, out numericValue);

                            //if (!isNumber && Regex.IsMatch(stringNumber, "^[a-zA-Z0-9]*$") && Convert.ToString(item["MedicalId"]).Length >= 8)
                            //{
                            //    string medicadid = string.Empty;

                            //    string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                            //    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, 8), 8);
                            //    int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

                            //    for (int i = jlen; i <= 8; i++)
                            //    {
                            //        if (i == 8)
                            //        {
                            //            // string randomstr = GenerateRandomString();
                            //            medicadid = s.Append(strAppend).ToString();
                            //        }
                            //        else
                            //        {
                            //            medicadid = s.Append('0').ToString();
                            //        }
                            //    }
                            //    clientVisitRequest.ClientID = medicadid;
                            //}
                            //else
                            //{
                            //    if (!isNumber && Regex.IsMatch(stringNumber, "^[a-zA-Z0-9]*$"))
                            //    {
                            //        string medicadid = string.Empty;
                            //        string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                            //        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
                            //        int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

                            //        for (int i = jlen; i <= 8; i++)
                            //        {
                            //            if (i == 8)
                            //            {
                            //                // string randomstr = GenerateRandomString();
                            //                medicadid = s.Append(strAppend).ToString();
                            //            }
                            //            else
                            //            {
                            //                medicadid = s.Append('0').ToString();
                            //            }
                            //        }
                            //        clientVisitRequest.ClientID = medicadid;
                            //    }
                            //    else
                            //    {
                            //        string medicadid = string.Empty;
                            //        string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);

                            //        int jlen = Convert.ToString(item["MedicalId"]).Length;
                            //        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);
                            //        for (int i = jlen; i <= 8; i++)
                            //        {
                            //            if (i == 8)
                            //            {
                            //                // string randomstr = GenerateRandomString();
                            //                medicadid = s.Append(strAppendByOffice).ToString();
                            //            }
                            //            else
                            //            {
                            //                medicadid = s.Append('0').ToString();
                            //            }
                            //        }
                            //        clientVisitRequest.ClientID = medicadid;
                            //    }
                            //}



                            ////if (Convert.ToString(item["MedicalId"]).Length >= 8)
                            ////{
                            ////    clientVisitRequest.ClientID= Convert.ToString(item["MedicalId"]).Substring(0, 7);
                            ////}
                            ////else
                            ////{
                            ////    string medicadid = string.Empty;

                            ////    int jlen = Convert.ToString(item["MedicalId"]).Length;
                            ////    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);
                            ////    for (int i = jlen; i <= 8; i++)
                            ////    {
                            ////        if (i == 8)
                            ////        {
                            ////            // string randomstr = GenerateRandomString();
                            ////            medicadid = s.Append('C').ToString();
                            ////        }
                            ////        else
                            ////        {
                            ////            medicadid = s.Append('0').ToString();
                            ////        }
                            ////    }
                            ////    clientVisitRequest.ClientID = medicadid;
                            ////}

                            //// clientVisitRequest.ClientID = "99999999S"; //Convert.ToString(item["ClientId"]);
                            //clientVisitRequest.ClientOtherID = Convert.ToString(item["PrimaryMD"]); //Convert.ToString(item["MedicalId"]);
                            //clientVisitRequest.VisitCancelledIndicator = "false";
                            //clientVisitRequest.PayerID = Convert.ToString(item["PayerId"]);
                            //clientVisitRequest.PayerProgram = Convert.ToString(item["PayerProgram"]);
                            //clientVisitRequest.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
                            #endregion

                            string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();

                            var stringNumber = ChkMedicalId;
                            int numericValue;
                            bool isNumber = int.TryParse(stringNumber, out numericValue);

                            bool istrue = (Regex.IsMatch(ChkMedicalId, @"[a-zA-Z]") && Regex.IsMatch(ChkMedicalId, @"[0-9]"));

                            if (!isNumber && istrue && Convert.ToString(item["MedicalId"]).Length >= 8)
                            {
                                string medicadid = string.Empty;
                                int dlen = Convert.ToString(item["MedicalId"]).Length;

                                string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 8), 8);

                                int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

                                for (int i = jlen; i <= 8; i++)
                                {
                                    if (i == 8)
                                    {
                                        // string randomstr = GenerateRandomString();
                                        medicadid = s.Append(strAppend).ToString();
                                    }
                                    else
                                    {
                                        medicadid = s.Append('0').ToString();
                                    }
                                }

                                string str7 = medicadid;
                                string str8 = string.Empty;
                                string str9 = string.Empty;
                                int val8 = 0;

                                for (int i = 0; i < str7.Length; i++)
                                {
                                    if (Char.IsLetterOrDigit(str7[i]))
                                        str8 += str7[i];
                                }
                                if (str8.Length > 0)
                                {
                                    str9 = str8.ToString();
                                }
                                clientVisitRequest.ClientID = str9;
                            }
                            else
                            {
                                if (!isNumber && istrue)
                                {
                                    string medicadid = string.Empty;
                                    string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
                                    int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

                                    for (int i = jlen; i <= 8; i++)
                                    {
                                        if (i == 8)
                                        {
                                            // string randomstr = GenerateRandomString();
                                            medicadid = s.Append(strAppend).ToString();
                                        }
                                        else
                                        {
                                            medicadid = s.Append('0').ToString();
                                        }
                                    }
                                    clientVisitRequest.ClientID = medicadid;
                                }
                                else
                                {
                                    string medicadid = string.Empty;
                                    string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);

                                    int jlen = Convert.ToString(item["MedicalId"]).Length;
                                    //StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);

                                    if (jlen >= 8)
                                    {

                                        jlen = Convert.ToString(item["MedicalId"]).Substring(1, 8).Length;
                                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(1, 8));
                                        for (int i = jlen; i <= 8; i++)
                                        {
                                            if (i == 8)
                                            {
                                                // string randomstr = GenerateRandomString();
                                                medicadid = s.Append(strAppendByOffice).ToString();
                                            }
                                            else
                                            {
                                                medicadid = s.Append('0').ToString();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        jlen = Convert.ToString(item["MedicalId"]).Length;
                                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);

                                        for (int i = jlen; i <= 8; i++)
                                        {
                                            if (i == 8)
                                            {
                                                // string randomstr = GenerateRandomString();
                                                medicadid = s.Append(strAppendByOffice).ToString();
                                            }
                                            else
                                            {
                                                medicadid = s.Append('0').ToString();
                                            }
                                        }
                                    }


                                    clientVisitRequest.ClientID = medicadid;
                                }
                            }

                            clientVisitRequest.ClientOtherID = Convert.ToString(item["PrimaryMD"]); //Convert.ToString(item["MedicalId"]);

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

                            clientVisitRequest.VisitCancelledIndicator = "false";

                            if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                            {
                                clientVisitRequest.PayerID = Convert.ToString(item["PayerId"]);
                            }
                            else
                            {
                                clientVisitRequest.PayerID = "CADDS";
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
                            {
                                clientVisitRequest.PayerProgram = Convert.ToString(item["PayerProgram"]);
                            }
                            else
                            {
                                clientVisitRequest.PayerProgram = "PCS";
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                            {
                                clientVisitRequest.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
                            }
                            else
                            {
                                clientVisitRequest.ProcedureCode = "Z9081";
                            }

                            clientVisitRequest.VisitTimeZone = "US/Pacific";
                            clientVisitRequest.ScheduleStartTime = Convert.ToString((Convert.ToDateTime(item["CheckInDateTime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                            clientVisitRequest.ScheduleEndTime = Convert.ToString(Convert.ToDateTime(item["CheckOutDateTime"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                            clientVisitRequest.AdjInDateTime = null;// Convert.ToString(item["InsertDateTime"]);
                            clientVisitRequest.AdjOutDateTime = null;// Convert.ToString(item["InsertDateTime"]); 
                            clientVisitRequest.BillVisit = "true";
                            clientVisitRequest.HoursToBill = "10";
                            clientVisitRequest.HoursToPay = "10";
                            clientVisitRequest.Memo = "This is a memo!";
                            clientVisitRequest.ClientVerifiedTimes = "true";
                            clientVisitRequest.ClientVerifiedTasks = "true";
                            clientVisitRequest.ClientVerifiedService = "true";
                            clientVisitRequest.ClientSignatureAvailable = "true";
                            clientVisitRequest.ClientVoiceRecording = "true";
                            clientVisitRequest.Modifier1 = null;
                            clientVisitRequest.Modifier2 = null;
                            clientVisitRequest.Modifier3 = null;
                            clientVisitRequest.Modifier4 = null;

                            ClientVisitRequestCalls clientVisitRequestCalls = new ClientVisitRequestCalls();
                            List<ClientVisitRequestCalls> clientVisitRequestCallsList = new List<ClientVisitRequestCalls>();
                            clientVisitRequestCalls.CallExternalID = Convert.ToString(item["PatientRequestId"]);
                            clientVisitRequestCalls.CallDateTime = Convert.ToString((Convert.ToDateTime(item["CallDatetime"])).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                            clientVisitRequestCalls.CallAssignment = "Time In";
                            clientVisitRequestCalls.GroupCode = null;
                            clientVisitRequestCalls.CallType = "Mobile";

                            if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                            {
                                clientVisitRequestCalls.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
                            }
                            else{

                                clientVisitRequestCalls.ProcedureCode= "Z9081";
                            }

                            clientVisitRequestCalls.ClientIdentifierOnCall = clientVisitRequest.ClientID;
                            clientVisitRequestCalls.MobileLogin = Convert.ToString(item["username"]);
                            clientVisitRequestCalls.CallLatitude = Convert.ToString(item["Latitude"]);
                            clientVisitRequestCalls.CallLongitude = Convert.ToString(item["Longitude"]);
                            clientVisitRequestCalls.Location = null;
                            clientVisitRequestCalls.TelephonyPIN = null;
                            clientVisitRequestCalls.OriginatingPhoneNumber = null;
                            clientVisitRequestCalls.VisitLocationType = "1";
                            clientVisitRequestCallsList.Add(clientVisitRequestCalls);

                            clientVisitRequest.Calls = clientVisitRequestCallsList;
                            clientVisitRequestList.Add(clientVisitRequest);

                            y = y + 1;
                        }
                    }
                }
                if (clientVisitRequestList.Count <= 0)
                {
                    //throw new Exception("No data Available to send");

                    res["Success"] = false;
                    res["Result"] = "No data Available to send";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                var arraylist = clientVisitRequestList.ToArray();
                List<ClientVisitRequest> request = new List<ClientVisitRequest>();

                foreach (var ReqItem in arraylist)
                {
                    request.Add(ReqItem);
                }
                string x = JsonConvert.SerializeObject(request);
                Task.Run(async () => { result = await objModel.SubmitEmployeeVisitRequestDataMultiple(x); }).Wait();
                res["Success"] = true;
                res["Result"] = result;
            }
            catch (Exception ex)
            {
                res["Success"] = false;
                res["Result"] = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExcelReportClockInClockOut()
        {
            return PartialView();
        }
        public string ExportClockInClockOutDataToExcel(string FromDate, string ToDate, int OfficeId)
        {

            var ObjStatus = "";
           

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);


            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAvailableAttendanceBetweenDates",
                   OfficeId,
                   FromDate,
                   ToDate,
                   OrganisationId 
                  );

          
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               //string ExcelUploadPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); 

                string ExcelUploadPath = ConfigurationManager.AppSettings["ClockInExportedFilePath"].ToString();
                string ExcelPath = ConfigurationManager.AppSettings["ClockInDownlLoadFilePath"].ToString();

                var fileName = "ClockInReportFromToDate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";

                var caseFile = ExcelUploadPath + "\\" + fileName;

                // Response.Write(caseFile); 

                FileInfo newFile = new FileInfo(caseFile);

                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {
                        var ReportDataSheet = excelPackage.Workbook.Worksheets.Add("ClockInReportFromToDate");

                        ReportDataSheet.Cells["A0:F1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:F1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

                        ReportDataSheet.Cells[1, 1].Value = "NurseName";
                        ReportDataSheet.Cells[1, 2].Value = "Date";
                        ReportDataSheet.Cells[1, 3].Value = "ClockInDateTime";
                        ReportDataSheet.Cells[1, 4].Value = "ClockOutDateTime";

                        ReportDataSheet.Cells[1, 5].Value = "ClockInStatus";

                        ReportDataSheet.Cells[1, 6].Value = "ServiceDuration";
                        //ReportDataSheet.Cells[1, 6].Value = "TimezoneId";
                        //ReportDataSheet.Cells[1, 7].Value = "TimezoneOffset";
                        //ReportDataSheet.Cells[1, 8].Value = "TimezonePostfix";


                        ReportDataSheet.Column(1).Width = 40;
                        ReportDataSheet.Column(2).Width = 40;
                        ReportDataSheet.Column(3).Width = 50;
                        ReportDataSheet.Column(4).Width = 50;
                        ReportDataSheet.Column(5).Width = 40;
                        ReportDataSheet.Column(6).Width = 40;

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

                            ReportDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["NurseName"].ToString();
                            ReportDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["Date"].ToString();
                            ReportDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["ClockInDateTime"];
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["ClockOutDateTime"].ToString();


                            ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["ClockInStatus"].ToString();
                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["ServiceTime"].ToString();

                            //ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString(); ;
                            //ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();

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




        private void FillAllOrganisations(object SelectedValue = null)
        {
            SelectedValue = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
                OrganisationServiceProxy OrganisationServiceProxy = new OrganisationServiceProxy();
                //var lstOrganisations = OrganisationServiceProxy.GetAllOrganisations(logInUserId).Result;
                var lstOrganisations = OrganisationServiceProxy.GetAllOrganisations(logInUserId, Convert.ToString(SelectedValue)).Result;
                SelectList OrganisationSelectList = new SelectList(lstOrganisations, "OrganisationId", "OrganisationName", SelectedValue);
                ViewBag.lstOrganisations = OrganisationSelectList;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }



        [HttpPost]
        public ActionResult GenerateReportActionOfOffices(string FromDate, string ToDate, string OfficeIds)
        {
            string result = "";
            var ObjStatus = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {
                     
                    using (SqlCommand cmd = new SqlCommand("ORG_GenerateMilageReportofOffices", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeIds);
                        cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);


                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        { 
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



        [HttpPost]
        public ActionResult GenerateBillingReportExportOfOffices(string FromDate, string ToDate, string OfficeId)
        {


            string result = "";
            var ObjStatus = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {

                    //SqlCommand cmd = new SqlCommand("GenerateMilageReport", con);   
                    // ORG_BillingReportOfOffices
                    //ORGNEW_BillingReportOfOffices

                    using (SqlCommand cmd = new SqlCommand("ORG_BillingReportOfOffices", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);


                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            // var ObjStatus = MilageReportGenerate(ds);
                            //return Json(ObjStatus);  
                            //Json(MilageReportGenerate(ds));
                            // ObjStatus = PayrollExportMilage(ds, checkboxdata);

                            ObjStatus = BillingExportMilage(ds);
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

        [HttpPost]
        public ActionResult GeneratePayrollExportOfOffices(string FromDate, string ToDate, string OfficeId, string checkboxdata)
        { 

            string result = "";
            var ObjStatus = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                { 
                    using (SqlCommand cmd = new SqlCommand("ORG_PayrollMilageReportOfOffices", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                        cmd.Parameters.AddWithValue("@OrganisationId", OrganisationId);
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);


                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            // var ObjStatus = MilageReportGenerate(ds);
                            //return Json(ObjStatus);  
                            //Json(MilageReportGenerate(ds));
                            ObjStatus = PayrollExportMilage(ds, checkboxdata);
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


    }
}





