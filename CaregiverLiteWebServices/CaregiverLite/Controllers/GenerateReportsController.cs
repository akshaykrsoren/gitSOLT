using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using CaregiverLiteWCF.Class;
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
    public class GenerateReportsController : Controller
    {
        // GET: GenerateReports
        public ActionResult GenerateReports()
        {
            FillAllOffices();

            return View();
        }
        public ActionResult getAttendanceList(JQueryDataTableParamModel param)
        {

            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            ReportsDetailsLists attendanceList = new ReportsDetailsLists();

            try
            {

               

                int FilterOffice = 0;
                string FilterCaregiver = "||";
                string FromDate = "1000-01-01";
                string ToDate = "1000-01-01";

                //if (Convert.ToInt32(Request["FilterOffice"]) != 0) //Request["FilterOffice"] != null && Request["FilterOffice"] != "" &&
                //{
                //    FilterOffice = Convert.ToInt32(Request["FilterOffice"]);

                //    if (FilterOffice == 0)//if (FilterOffice == "All")
                //    {
                //        FilterOffice = 0; //FilterOffice = "||";
                //    }
              //  }

                if (Request["FilterCaregiver"] != null && Request["FilterCaregiver"] != "" )
                {
                    FilterCaregiver = Request["FilterCaregiver"];

                    if (FilterCaregiver == "0")
                    {
                        FilterCaregiver = "||";
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


                //string[] roles = Roles.GetRolesForUser(user.UserName);
                //foreach (string role in roles)
                //{
                //    if (role == "SuperAdmin")
                //    {
                //        Session["IsSuperAdmin"] = "true";
                //        Session["UserId"] = user.ProviderUserKey.ToString();
                //        //LogInUserId = Session["UserId"];
                //        IsAdmin = 1;
                //    }

                //}



                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    
                    sortOrder = "Name";
                }
                else if (sortColumnIndex == 1)
                {
                     sortOrder = "Noofvisits";
                }
                //else if (sortColumnIndex == 2)
                //{
                //    sortOrder = "Noofvisits";
                //}
                else
                {
                    sortOrder = "Name";

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

                ReoortsManagementServiceProxy AttendanceDetailLiteService = new ReoortsManagementServiceProxy();
                // attendanceList = AttendanceDetailLiteService.GetAllAttendanceDetail(UserID, pageNo, recordPerPage, search).Result;

                //attendanceList = GetAttendanceList24(UserID, Convert.ToString(pageNo), Convert.ToString(recordPerPage), search);
               //  attendanceList = AttendanceDetailLiteService.AGetAllAttendanceDetail(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOffice).Result;

             //attendanceList = AttendanceDetailLiteService.GetAllAttendanceReports(pageNo, recordPerPage, search, sortOrder, sortDirection, IsAdmin, LogInUserId, Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"),FilterCaregiver).Result;

                attendanceList = GetAllAttendanceportsOfCaregiver(pageNo,recordPerPage, search, sortOrder, sortDirection, IsAdmin, LogInUserId, Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"), FilterCaregiver);


              



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

               // var result = from C in attendanceList.AttendanceManagemenList select new[] { C, C, C, C, C, C, C};
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = attendanceList.TotalNumberofRecord,
                    iTotalDisplayRecords = attendanceList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = attendanceList.TotalNumberofRecord,
                    iTotalDisplayRecords = attendanceList.FilteredRecord

                }, JsonRequestBehavior.AllowGet);
            }
        }

        private void FillAllOffices()
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try { 
            string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
            OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();
            var lstOffices = officeServiceProxy.GetAllOffices(logInUserId,OrganisationId.ToString()).Result;
            lstOffices.Insert(0, new Office() { OfficeId = 0, OfficeName = "All" });
            SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName");
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


        [HttpPost]
        public JsonResult ListCaregiver(string LoginUserId)
        {
            List<AttendanceManagementDetails> Caregiverlist = new List<AttendanceManagementDetails>();

            List<SelectListItem> listItem = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {

                SqlCommand cmd = new SqlCommand("OnlyCaregiverList", con);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                //cmd.Parameters.AddWithValue("@NurseId", LoginUserId);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        AttendanceManagementDetails objCaregiverlist = new AttendanceManagementDetails();
                        objCaregiverlist.NurseId = Convert.ToInt32(dr["NurseId"]);
                        objCaregiverlist.Name = dr["Name"].ToString();


                        Caregiverlist.Add(objCaregiverlist);

                    }

                }

            }
            var caregiverlist = Caregiverlist;
            foreach (var carelist in Caregiverlist)
            {
                listItem.Add(new SelectListItem
                {
                    Text = carelist.Name,
                    Value = (carelist.NurseId).ToString(),

                });
            }

            return Json(listItem);

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



        public JsonResult GetCaregiverByOfficeId(int OfficeId)
        {
            List<CareGiverModel> officelist = new List<CareGiverModel>();

            List<SelectListItem> listItem = new List<SelectListItem>();

            listItem.Clear();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {

                SqlCommand cmd = new SqlCommand("GetCaregiverByOfficeId", con);
                cmd.Parameters.AddWithValue("@OfficeId", OfficeId);

                cmd.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                con.Open();

                //SqlDataReader dr = cmd.ExecuteReader();

                listItem.Add(new SelectListItem { Text = "All", Value = "0" });

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

                SqlCommand cmd = new SqlCommand("GetFavouritePatientListBasedOnNurseId", con);
                cmd.Parameters.AddWithValue("@NurseId", Convert.ToString(NurseId));

                cmd.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                con.Open();


                //SqlDataReader dr = cmd.ExecuteReader();

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
        public ActionResult GenerateReportActionForPauseAndResume(string FromDate, string ToDate, int OfficeId, int NurseId)
        {


            string result = "";
            var ObjStatus = "";

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))

                {

                    //  SqlCommand cmd = new SqlCommand("Get_Break_GenerateMilageReport", con);

                    SqlCommand cmd = new SqlCommand("GenerateMilageReportFIlterWithNuserIdandPatientId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                    cmd.Parameters.AddWithValue("@NurseId", NurseId);
                    //cmd.Parameters.AddWithValue("@PatientId", PatientId);
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

                        ReportDataSheet.Cells["A0:m1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ReportDataSheet.Cells["A0:m1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        ReportDataSheet.Row(1).Style.Font.Bold = true;
                        ReportDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);

                        ReportDataSheet.Cells[1, 1].Value = "PatientRequestId";
                        ReportDataSheet.Cells[1, 2].Value = "NurseName";
                        ReportDataSheet.Cells[1, 3].Value = "PatientName";
                        ReportDataSheet.Cells[1, 4].Value = "PatientName";
                        ReportDataSheet.Cells[1, 5].Value = "Nurse Address";


                        // ReportDataSheet.Cells[1, 5].Value = "LocationDateTime";
                        //ReportDataSheet.Cells[1, 4].Value = "Patient Address";
                        //ReportDataSheet.Cells[1, 5].Value = "DrivingStartTime";
                        //ReportDataSheet.Cells[1, 6].Value = "DrivingStartLatitude";
                        //ReportDataSheet.Cells[1, 7].Value = "DrivingStartLongitude";
                        //ReportDataSheet.Cells[1, 8].Value = "DrivingStopTime";
                        //ReportDataSheet.Cells[1, 9].Value = "DrivingStopLatitude";
                        //ReportDataSheet.Cells[1, 10].Value = "DrivingStopLongitude";


                        ReportDataSheet.Cells[1, 6].Value = "Status";
                        ReportDataSheet.Cells[1, 7].Value = "DrivingStopLongitude";
                        ReportDataSheet.Cells[1, 8].Value = "TotalTravelTime";
                        ReportDataSheet.Cells[1, 9].Value = "TotalDistance";
                        ReportDataSheet.Cells[1, 10].Value = "ShortestDistance";
                        //ReportDataSheet.Cells[1, 15].Value = "CheckInDateTime";
                        //ReportDataSheet.Cells[1, 16].Value = "CheckOutDateTime";
                        ReportDataSheet.Cells[1, 11].Value = "TotalCheckInTime";
                        // ReportDataSheet.Cells[1, 11].Value = "Office";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        //ReportDataSheet.Cells[1, 19].Value = "TotalWorkingTime";
                        ReportDataSheet.Cells[1, 12].Value = "GoogleShortestTime";
                        ReportDataSheet.Cells[1, 13].Value = "Pause & Resume";



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
                            ReportDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["Date"];
                            ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["NurseAddress"].ToString();


                            //ReportDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["Address"];
                            //ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["DrivingStartTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLatitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStartLongitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["DrivingStopTime"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                            //ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();


                            ReportDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["Status"].ToString();
                            ReportDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString(); ;



                            ReportDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["TotalTravelTime"].ToString();
                            ReportDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString();

                            ReportDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["GoogleShortestDistance"].ToString();


                            ReportDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();

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
                            ReportDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["GoogleShortestTime"].ToString();





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
                            ReportDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["PAUSE_RESUME"].ToString();


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




        public ReportsDetailsLists GetAllAttendanceportsOfCaregiver(int pageNo, int recordPerPage, string search, string sortfield, string sortorder, int IsAdmin, string LogInUserId, string FromDate, string ToDate, string Filtercaregiver)
        {
                                                   
            ReportsDetailsLists ListAttendanceDetails = new ReportsDetailsLists();

            string TotalTravel = string.Empty;
            string CheckInTotalTime = string.Empty;
            string NewDateVar = string.Empty;
            try
            {
                if (FromDate == "||" && ToDate == "||")
                {
                    FromDate = "1000-01-01";
                    ToDate = "1000-01-01";
                }


                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceList_vinnTestingData",
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceListwithNurseAndPatientNameparam",

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceListGenerateReports",
                                                       Convert.ToInt32(pageNo),
                                                       Convert.ToInt32(recordPerPage),
                                                       sortfield,
                                                       sortorder,
                                                       search,
                                                       IsAdmin,
                                                       LogInUserId,
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate),
                                                       Filtercaregiver);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<ReportsManagementDetailss> AttendanceDetailsList = new List<ReportsManagementDetailss>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        ReportsManagementDetailss objAttendanceDetail = new ReportsManagementDetailss();
                       // objAttendanceDetail.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        objAttendanceDetail.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                       // objAttendanceDetail.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();

                        objAttendanceDetail.NoOfVists = ds.Tables[0].Rows[i]["Noofvisits"].ToString();


                        CheckInTotalTime = ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();
                        if (CheckInTotalTime == "")
                        {
                            objAttendanceDetail.CheckInTotalTime = "NA";
                        }
                        else
                        {
                            objAttendanceDetail.CheckInTotalTime = CheckInTotalTime;
                        }

                        //  objAttendanceDetail.CheckInTotalTime= ds.Tables[0].Rows[i]["TotalCheckInTime"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString()))
                        {
                            objAttendanceDetail.DrivingTotalDistance = Convert.ToDouble(ds.Tables[0].Rows[i]["DrivingTotalDistance"].ToString());
                        }
                        else
                        {
                            objAttendanceDetail.DrivingTotalDistance = 0;
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DrivingTotalMiles"].ToString()))
                        {
                            objAttendanceDetail.DrivingTotalMilesToPay = Convert.ToDouble(ds.Tables[0].Rows[i]["DrivingTotalMiles"].ToString());
                        }
                        else
                        {
                            objAttendanceDetail.DrivingTotalMilesToPay = 0;
                        }

                        objAttendanceDetail.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());

                        if (Convert.ToInt32(ds.Tables[0].Rows[i]["NoofvisitsCompleted"].ToString()) > 0)
                        {
                            objAttendanceDetail.NoOfVisitsCompleted = Convert.ToInt32(ds.Tables[0].Rows[i]["NoofvisitsCompleted"].ToString());
                        }
                        else
                        {
                            objAttendanceDetail.NoOfVisitsCompleted = 0;
                        }

                        if (Convert.ToInt32(ds.Tables[0].Rows[i]["NoofvisitsCancelled"].ToString()) > 0)
                        {
                            objAttendanceDetail.NoOfVisitsCancelled = Convert.ToInt32(ds.Tables[0].Rows[i]["NoofvisitsCancelled"].ToString());
                        }
                        else
                        {
                            objAttendanceDetail.NoOfVisitsCancelled = 0;
                        }

                        //// objAttendanceDetail.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        //objAttendanceDetail.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        //objAttendanceDetail.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        //objAttendanceDetail.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        //objAttendanceDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        //objAttendanceDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

                        //// objAttendanceDetail.CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                        //CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                        //if (CheckInTotalTime == "")
                        //{
                        //    objAttendanceDetail.CheckInTotalTime = "NA";
                        //}
                        //else
                        //{
                        //    objAttendanceDetail.CheckInTotalTime = CheckInTotalTime;
                        //}
                        //objAttendanceDetail.RequestedDuration = (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("hh")) + " hrs " +
                        //    (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("mm")) + " mins";

                        //TotalTravel = ds.Tables[0].Rows[i]["TotalHours"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalMi"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalSe"].ToString();
                        //if (TotalTravel == " :  : ")
                        //{
                        //    // :  :
                        //    objAttendanceDetail.TotalTravelTime = "No detail available";
                        //}
                        //else
                        //{
                        //    objAttendanceDetail.TotalTravelTime = TotalTravel;

                        //}


                        // objAttendanceDetail.DrivingStopLatitude = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                        // objAttendanceDetail.DrivingStopLongitude = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                        AttendanceDetailsList.Add(objAttendanceDetail);
                    }
                    ListAttendanceDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0])+3;
                    ListAttendanceDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows.Count);
                    ListAttendanceDetails.AttendanceManagemenList = AttendanceDetailsList;  
                                                                                                       
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAttendanceDetailsList";
              //  string result = InsertErrorLog(objErrorlog);
            }
            return ListAttendanceDetails;
        }




        //[HttpGet]
        //public ActionResult OpenDetailsView()
        //{
        //    return PartialView("OpenDetailsView");
        //}




     
        public ActionResult OpenDetailsView(string NurseId, string FromDate, string ToDate)
        {
            if(FromDate == "" && ToDate == "")
            {
                 FromDate = "1000-01-01";
                 ToDate = "1000-01-01";
            }

            ReportsManagementDetailss objAttendanceDetail1 = new ReportsManagementDetailss();
            string TotalTravel = string.Empty;
              string CheckInTotalTime = string.Empty;
               string NewDateVar = string.Empty;

            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllcaregiverVisitDetails",
                                                     Convert.ToString(NurseId),
                                                     Convert.ToDateTime(FromDate),
                                                     Convert.ToDateTime(ToDate));

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<ReportsManagementDetailss> AttendanceDetailsList = new List<ReportsManagementDetailss>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    ReportsManagementDetailss objAttendanceDetail = new ReportsManagementDetailss();
                     objAttendanceDetail.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                    objAttendanceDetail.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    objAttendanceDetail.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                    //objAttendanceDetail.NoOfVists = ds.Tables[0].Rows[i]["NoOfVisits"].ToString();

                    objAttendanceDetail.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());  
                    //// objAttendanceDetail.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    objAttendanceDetail.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                    objAttendanceDetail.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                    objAttendanceDetail.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                    objAttendanceDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                    objAttendanceDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

                    // objAttendanceDetail.CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                    CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                    if (CheckInTotalTime == "")
                    {
                        objAttendanceDetail.CheckInTotalTime = "NA";
                    }
                    else
                    {
                        objAttendanceDetail.CheckInTotalTime = CheckInTotalTime;
                    }
                    //objAttendanceDetail.RequestedDuration = (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("hh")) + " hrs " +
                    //    (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("mm")) + " mins";

                    TotalTravel = ds.Tables[0].Rows[i]["TotalHours"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalMi"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalSe"].ToString();
                    if (TotalTravel == " :  : ")
                    {
                        // :  :
                        objAttendanceDetail.TotalTravelTime = "No detail available";
                    }
                    else
                    {
                        objAttendanceDetail.TotalTravelTime = TotalTravel;

                    }


                    objAttendanceDetail.DrivingStopLatitude = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                    objAttendanceDetail.DrivingStopLongitude = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                    AttendanceDetailsList.Add(objAttendanceDetail);


                   


                }
                //ListAttendanceDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]) + 4;
                //ListAttendanceDetails.FilteredRecord = Convert.ToInt32(ds.Tables[0].Rows.Count);
                //ListAttendanceDetails.AttendanceManagemenList = AttendanceDetailsList;

                ViewBag.GetallCareGiverPatientVisitList = AttendanceDetailsList;


            }






             return PartialView("OpenDetailsView");
           // return View();
        }









    }

}