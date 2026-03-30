using CaregiverLiteWCF.Class;
using CaregiverLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CaregiverLiteWCF;
using DifferenzLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using OfficeOpenXml;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Mail;
using ClosedXML.Excel;
using CaregiverLite.Action_Filters;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class MarketingsController : Controller
    {
        // GET: Marketings
        public ActionResult Marketings()
        {
            FillAllOffices();
            return View();
        }

        public ActionResult GetMarketingsDetailsList(JQueryDataTableParamModel param)
        {
            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            MarketersDetailsList MarketingsDetailsList = new MarketersDetailsList();
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
                    sortOrder = "MarketersName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "MarketersId";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "Street";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "City";
                }
                else if (sortColumnIndex == 5)
                {
                    sortOrder = "State";
                }
                else if (sortColumnIndex == 6)
                {
                    sortOrder = "ZipCode";
                }
                else if (sortColumnIndex == 7)
                {
                    sortOrder = "PhoneNo";
                }
                //else if (sortColumnIndex == 7)
                //{
                //    sortOrder = "PrimaryMD";
                //}
                else if (sortColumnIndex == 8)
                {
                    sortOrder = "OfficeName";
                }
                else
                {
                    sortOrder = "MarketersName";

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
                //    sortOrder = "Street";
                //}
                //else if (sortColumnIndex == 3)
                //{
                //    sortOrder = "City";
                //}
                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "State";
                //}
                //else if (sortColumnIndex == 5)
                //{
                //    sortOrder = "ZipCode";
                //}
                //else if (sortColumnIndex == 6)
                //{
                //    sortOrder = "PhoneNo";
                //}
                ////else if (sortColumnIndex == 7)
                ////{
                ////    sortOrder = "PrimaryMD";
                ////}
                //else if (sortColumnIndex == 9)
                //{
                //    sortOrder = "OfficeName";
                //}
                //else
                //{
                //    sortOrder = "MarketersName";

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

                //MarketingsDetailsServiceProxy MarketingsDetailLiteService = new MarketingsDetailsServiceProxy();
                //MarketingsDetailsList = MarketingsDetailLiteService.GetAllMarketingDetail(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId).Result;

                MarketingsDetailsList = GetMarketingDetailsList(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId);

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
            if (MarketingsDetailsList.MarketersList != null)
            {
                var result = from C in MarketingsDetailsList.MarketersList select new[] { C, C, C, C, C, C, C, C, C, C };
                var jsonResult = Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = MarketingsDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = MarketingsDetailsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }
            else
            {
                var jsonResult = Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = MarketingsDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = MarketingsDetailsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }
        }

        private void FillAllOffices(object SelectedValue = null)
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try 
            { 
            string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
            OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();
            var lstOffices = officeServiceProxy.GetAllOffices(logInUserId,OrganisationId.ToString()).Result;
            SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName", SelectedValue);
            ViewBag.lstOffice = officeSelectList;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }

        public MarketersDetailsList GetMarketingDetailsList(string LogInUserId, int pageNo, int recordPerPage,string search,  string sortfield, string sortorder, int OfficeId)
        {

            MarketersDetailsList ListMarketingsDetails = new MarketersDetailsList();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllMarketingDetails",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        Convert.ToInt32(OfficeId),
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<MarketingsDetail> MarketersDetailsList = new List<MarketingsDetail>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        MarketingsDetail objPatientDetail = new MarketingsDetail();
                        objPatientDetail.MarketersId = Convert.ToInt32(ds.Tables[0].Rows[i]["MarketersId"]);
                        objPatientDetail.MarketersName = ds.Tables[0].Rows[i]["MarketersName"].ToString();
                        //objPatientDetail.MedicalId = ds.Tables[0].Rows[i]["MedicalId"].ToString();
                        objPatientDetail.PhoneNo = ds.Tables[0].Rows[i]["PhoneNo"].ToString();
                        objPatientDetail.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        objPatientDetail.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objPatientDetail.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objPatientDetail.State = ds.Tables[0].Rows[i]["State"].ToString();
                        objPatientDetail.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        //objPatientDetail.PrimaryMD = ds.Tables[0].Rows[i]["PrimaryMD"].ToString();
                        int officeId = 0;
                        Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["OfficeId"]), out officeId);
                        objPatientDetail.OfficeId = officeId;
                        objPatientDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        MarketersDetailsList.Add(objPatientDetail);
                    }
                    ListMarketingsDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListMarketingsDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListMarketingsDetails.MarketersList = MarketersDetailsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientDetailsList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListMarketingsDetails;
        }

        public ActionResult AddMarketers()
        {
            var MarketersDetailsModel = new MarketerDetailsModel();

            //PatientDetailsServiceProxy PatientDetailsService = new PatientDetailsServiceProxy();
            //ViewBag.ServiceList = PatientDetailsService.GetAllServices().Result;
            FillAllOffices();
            MarketersDetailsModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;

            return PartialView(MarketersDetailsModel);
        }

        [HttpPost]
        public ActionResult AddMarketersDetails(MarketerDetailsModel objMarketersDetails)
        {
            var MarketingsDetail = new MarketingsDetail();
            if (objMarketersDetails.MarketersId == 0)
            {
                MembershipCreateStatus status;
                    Membership.CreateUser(objMarketersDetails.UserName, objMarketersDetails.Password, objMarketersDetails.Email, null, null, true, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(objMarketersDetails.UserName, "Marketing");
                }

                else if (status == MembershipCreateStatus.DuplicateEmail || status == MembershipCreateStatus.DuplicateUserName)
                {
                    TempData["error"] = true;
                    TempData["Message"] = "Marketers details not added !! Error : duplicate Username/email found ";
                    return RedirectToAction("Marketings", "Marketings", new { IsAdded = false });
                }
                else
                {
                    TempData["error"] = true;
                    TempData["Message"] = "Marketers  not added !! Error : " + status;
                    return RedirectToAction("Marketings", "Marketings", new { IsAdded = false });
                }
            }
            

            string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString(); 
            try 
            {   
                if (ModelState.IsValid)
                {
                 //  MarketingsDetailsServiceProxy  MarketsDetailsService = new MarketingsDetailsServiceProxy();
                    //SchedulePatientRequestServiceProxy CareGiverLiteService = new SchedulePatientRequestServiceProxy();
                    MarketingsDetail.MarketersId = objMarketersDetails.MarketersId;
                    MarketingsDetail.MarketersName = objMarketersDetails.MarketersName;
                    MarketingsDetail.UserName = objMarketersDetails.UserName;
                    MarketingsDetail.Password = objMarketersDetails.Password;
                    MarketingsDetail.UserId = Membership.GetUser(objMarketersDetails.UserName).ProviderUserKey.ToString();
                    //MarketingsDetail.MedicalId = objMarketersDetails.MedicalId;
                    MarketingsDetail.Email = objMarketersDetails.Email;
                    MarketingsDetail.PhoneNo = objMarketersDetails.PhoneNo;
                    //MarketingsDetail.Address = objMarketersDetails.Address;
                    MarketingsDetail.Street = objMarketersDetails.Street;      
                    MarketingsDetail.City = objMarketersDetails.City;           
                    MarketingsDetail.State = objMarketersDetails.State;         
                    MarketingsDetail.Latitude = objMarketersDetails.Latitude;   
                    MarketingsDetail.Longitude = objMarketersDetails.Longitude;   
                    MarketingsDetail.ZipCode = objMarketersDetails.ZipCode;     
                    MarketingsDetail.InsertUserId = Membership.GetUser().ProviderUserKey.ToString();
                    MarketingsDetail.TimezoneId = objMarketersDetails.TimezoneId; 
                    MarketingsDetail.TimezoneOffset = objMarketersDetails.TimezoneOffset;
                    MarketingsDetail.TimezonePostfix = objMarketersDetails.TimezonePostfix;
                    //MarketingsDetail.PrimaryMD = objMarketersDetails.PrimaryMD;
                    MarketingsDetail.OfficeId = objMarketersDetails.OfficeId;

                    //string result = MarketsDetailsService.AddMarketers(MarketingsDetail).Result;

                    string result = AddMarketerData(MarketingsDetail);

                    if (result == "Success")
                    {
                        TempData["Message"] = "Marketers Details is Added successfully.";
                        return RedirectToAction("Marketings", "Marketings", new { IsAdded = true });
                    }
                    else
                    {
                        TempData["error"] = true;
                        TempData["Message"] = result;// "Patient Details Not Added successfully.";
                        return RedirectToAction("Marketings", "Marketings");
                        //return PartialView("AddPatient", objPatientDetails);
                    }
                }
                else
                {
                    TempData["error"] = true;

                    string messages = string.Join("; ", ModelState.Values
                                        .Where(x => x.Errors.Count > 0)
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    throw new Exception(messages);
                }
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "MarketingsController";
                log.Methodname = "[HttpPost] AddPatientDetails";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                TempData["Message"] = "Marketers Details not Added";
                return RedirectToAction("Marketings", "Marketings", new { IsAdded = false });
                //return PartialView("AddPatient", objPatientDetails);
            }
        }

        public string AddMarketerData(MarketingsDetail MarketingsDetail)
        {

            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddMarketer",
                                                    MarketingsDetail.MarketersId,
                                                    MarketingsDetail.MarketersName,
                                                    MarketingsDetail.UserName,
                                                    MarketingsDetail.Password,
                                                    //MarketingsDetail.MedicalId,
                                                    MarketingsDetail.Email,
                                                    MarketingsDetail.PhoneNo,
                                                    //MarketingsDetail.Address,
                                                    MarketingsDetail.Street,
                                                    MarketingsDetail.City,
                                                    MarketingsDetail.State,
                                                    MarketingsDetail.Latitude,
                                                    MarketingsDetail.Longitude,
                                                    MarketingsDetail.ZipCode,
                                                    Guid.Parse(MarketingsDetail.InsertUserId),
                                                    MarketingsDetail.TimezoneId,
                                                    MarketingsDetail.TimezoneOffset,
                                                    MarketingsDetail.TimezonePostfix,
                                                   // MarketingsDetail.PrimaryMD,
                                                    MarketingsDetail.OfficeId,
                                                    MarketingsDetail.UserId
                                                    
                                                    );

                if (ds.Tables[0].Rows[0][0].ToString() == "Success")
                {
                    result = "Success";
                }
                else
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddPatient";
                objErrorlog.UserID = MarketingsDetail.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        #region
        // Manual Appointment

        [HttpPost]
        public ActionResult WebAddPatientScheduleRequestInfo(ClientRequest objClientRequest)
        {

            RootRespone rootresponse = new RootRespone();

            var ClientRequest = new ClientRequest();

            var resultdata = "";
            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            objClientRequest.InsertUserId = UserID;


            try
            {
                ClientRequest.ClientRequestId = objClientRequest.ClientRequestId;
                ClientRequest.ClientName = objClientRequest.ClientName;
                ClientRequest.Address = objClientRequest.Address;
                ClientRequest.Street = objClientRequest.Street;
                ClientRequest.City = objClientRequest.City;
                ClientRequest.State = objClientRequest.State;
                ClientRequest.Latitude = objClientRequest.Latitude;
                ClientRequest.Longitude = objClientRequest.Longitude;
                ClientRequest.ZipCode = objClientRequest.ZipCode;
                ClientRequest.CRNumber = objClientRequest.CRNumber;
                ClientRequest.Description = objClientRequest.Description;
                ClientRequest.Date = DateTime.ParseExact(objClientRequest.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();

                ClientRequest.Fromtime = objClientRequest.Fromtime;
                ClientRequest.Totime = objClientRequest.Totime;

                ClientRequest.InsertUserId = objClientRequest.InsertUserId;
                ClientRequest.TimeOfVist = objClientRequest.TimeOfVist;
                ClientRequest.NameOfPractice = objClientRequest.NameOfPractice;
                ClientRequest.DischargePlaneerName = objClientRequest.DischargePlaneerName;
                ClientRequest.FirstVisit = objClientRequest.FirstVisit;

                ClientRequest.Office = objClientRequest.Office;
                ClientRequest.TimezoneId = objClientRequest.TimezoneId;
                ClientRequest.TimezoneOffset = objClientRequest.TimezoneOffset;
                ClientRequest.TimezonePostfix = objClientRequest.TimezonePostfix;
                ClientRequest.MarketersId = objClientRequest.MarketersId;

                ClientRequest.IsRepeat = objClientRequest.IsRepeat;
                ClientRequest.RepeatEvery = objClientRequest.RepeatEvery;
                ClientRequest.RepeatTypeID = objClientRequest.RepeatTypeID;
                ClientRequest.RepeatDate = objClientRequest.RepeatDate;
                ClientRequest.DayOfWeek = objClientRequest.DayOfWeek;
                ClientRequest.DaysOfMonth = objClientRequest.DaysOfMonth;

                ClientRequest.MarketersName = objClientRequest.MarketersId;

                RootRegister newdata = AddClientAppointment(ClientRequest);

                resultdata = newdata.message;

                if (resultdata == "Success")
                {
                    rootresponse.message = "Success";
                    rootresponse.success = 1;
                }
                else
                {
                    rootresponse.message = "Falied";
                    rootresponse.success = 0;
                }



            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "MarketingRequestController";
                log.Methodname = "[HttpPost] WebAddPatientScheduleRequestInfo";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                // return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = false });
            }

            return Json(rootresponse, JsonRequestBehavior.AllowGet);
        }


        public RootRegister AddClientAppointment(ClientRequest ClientRequest)
        {
            RootRegister objStatus = new RootRegister();
            string resultForCaregiverRequest = "";

            string tempMarketersId = ClientRequest.MarketersId.ToString();

            try
            {
                MarketingsDetail result = InsertSchedulePatientRequest(ClientRequest);


                if (tempMarketersId.Equals(result.MarketersId.ToString()))
                {

                    var obj = result;
                    PushNotification objNotification = new PushNotification();
                    var userid = ClientRequest.InsertUserId;

                    var ClientRequestFromTime = DateTime.ParseExact(ClientRequest.Fromtime, "HH:mm:ss", CultureInfo.InvariantCulture);
                    var ClientRequestToTime = DateTime.ParseExact(ClientRequest.Totime, "HH:mm:ss", CultureInfo.InvariantCulture);

                    var ConvertFromTime = ClientRequestFromTime.ToString("hh:mm tt");
                    var ConvertToTime = ClientRequestToTime.ToString("hh:mm tt");
                    var ConvertPatientDate = Convert.ToDateTime(ClientRequest.Date).ToString("dd MMM yyyy");
                    string Notificationresult = objNotification.SendPushNotification("Client Schedule Request", "Client request added for " + ConvertFromTime + " to " + ConvertToTime + " " + ClientRequest.TimezonePostfix + " on " + ConvertPatientDate, obj.DeviceToken, obj.DeviceType, null, null, obj.MarketersId.ToString(), null, "Patient Schedule Request", userid, true, "", obj.ClientRequestId);

                    resultForCaregiverRequest = InsertScheduleClientRequestTemp(obj.ClientRequestId.ToString(), obj.MarketersId.ToString(), Notificationresult);
                    if (resultForCaregiverRequest == "Success")
                    {

                        objStatus.message = "Success";
                        objStatus.success = 1;

                    }
                    else
                    {
                        objStatus.message = "Fail";
                        objStatus.success = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "AddClientMarketingAppointment";
                log.Methodname = "[HttpPost] AddClientAppointment";
                return objStatus;

            }

            return objStatus;
        }

        #endregion

        [HttpGet]
        public ActionResult GetReadExcelScheduledList()
        {
            return PartialView("ImportScheduledAppointment");
        }

        #region    By CRN Schedule
        [HttpPost]
        // public async Task<ActionResult> ImportScheduledExcelData(HttpPostedFileBase file)
        public async Task<ActionResult> ImportScheduledExcelData()
        {
            string x = "";

            string finalresult = "";

            string FullAddress = "";

            string filePath = string.Empty;
            string filePath2 = "";
           // string FileName = "";

            string ObjStatus = "";

            int Scheduled = 0;
            int error = 0;
            int y = 1;
            string ClientName = "";

            string remoteUri = ConfigurationManager.AppSettings["ClientMarketingsBatchScheduling"].ToString(), myStringWebResource = "";

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
                        string path = Server.MapPath("~/Uploads/ClientBatchSchedule/");
                        string path2 = Server.MapPath("~/Uploads/ClientMarketingsBatchScheduling/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);

                        }

                        filePath = path + "AppointmentMarketingSchedule_" + DateTime.Now.ToString("MM.dd.yy hh.mm.ss") + "_" + Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filePath);

                        string FileName = "AppointmentMarketing_" + DateTime.Now.ToString("MM.dd.yy hh.mm") + Path.GetFileName(file.FileName);
                        filePath2 = System.IO.Path.Combine(path2, FileName);
                        string extension2 = Path.GetExtension(file.FileName);

                        file.SaveAs(filePath2);

                        string conString = string.Empty;

                        switch (extension)
                        {
                            case ".xls":
                                conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 9.0;HDR=YES'";
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
                                    try
                                    {
                                        connExcel.Open();
                                    }
                                    catch (Exception ex)
                                    {
                                        
                                        throw;
                                    }
                                    //Get the name of First Sheet.
                             
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    try
                                    {
                                        odaExcel.Fill(dt);
                                    }
                                    catch(Exception ex)
                                    {

                                    }

                                    //odaExcel.Fill(1, 3, dt);
                                }

                                DataTable dataset = new DataTable();
                                // dt.Columns.Add("Status");
                                try
                                {
                                    dt.Columns.Add(new DataColumn("Comments", typeof(string)));
                                }
                                catch(Exception ex)
                                {
                                    return Json("", JsonRequestBehavior.AllowGet);
                                }

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
                                int column = worksheet.Dimension.Columns+1;

                                worksheet.Cells[1, column].Value = "Comments";
                                while (y == 1)
                                {
                                    worksheet.Cells[1, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[1, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.OrangeRed);
                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[1, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);

                                 //   worksheet.Column(9).Style.Font.Bold = true;

                                    package.Save();
                                    y++;
                                }

                                // loop through the worksheet rows           
                                //end of excel file existance
                                #endregion

                                foreach (DataRow item in dt.Rows)
                                {
                                    if (item["CRN Number"].ToString() == "" || string.IsNullOrEmpty(item["CRN Number"].ToString()))
                                    {
                                        break;
                                    }

                                    // ClientName = item["Name of practice"].ToString();
                                    // ClientName = item["Discharge planeer name "].ToString();
                                    //'+","+ item["Patient Last"].ToString();

                                    ClientRequest ClientRequest = new ClientRequest();
                                   
                                    ClientRequest.CRNumber = item["CRN Number"].ToString();
                                    ClientRequest.MarketersName = item["MD name"].ToString();
                                    //+ " " + item["Provider Last"].ToString() + " " + item["Task Type"].ToString();
                                    string InsertedUserID = ConfigurationManager.AppSettings["AutomatedAppointementUserIdFromIndia"].ToString();

                                     ClientRequest.InsertUserId = InsertedUserID.ToString();
                                     Task.Run(async () => { ClientRequest = GetClientRequestDetailByCRNNumber(ClientRequest.InsertUserId, ClientRequest.CRNumber, ClientRequest.MarketersName); }).Wait();

                                    //Task.Run(async () => { ClientRequest = GetClientRequestDetailByCRNNumber(ClientRequest.InsertUserId, ClientRequest.Phone, ClientRequest.MarketersName); }).Wait();

                                    if (!string.IsNullOrEmpty(ClientRequest.ClientName))
                                    {

                                        if (!string.IsNullOrEmpty(ClientRequest.MarketersId))
                                        {
                                            ClientRequest.Street = ClientRequest.Street;
                                            // ClientRequest.ServiceNames = ClientRequest.ServiceNames;
                                            ClientRequest.TimezoneOffset = Convert.ToInt32(ClientRequest.TimezoneOffset);
                                            ClientRequest.City = ClientRequest.City;
                                            ClientRequest.Address = ClientRequest.Address;
                                            ClientName = ClientRequest.ClientName;

                                            ClientRequest.TimezoneId = ClientRequest.TimezoneId;

                                            //ClientRequest.IsCancelled = Convert.ToBoolean(false);
                                            ClientRequest.State = ClientRequest.State;
                                            ClientRequest.TimezonePostfix = ClientRequest.TimezonePostfix;
                                            ClientRequest.Fromtime = "02:00:00";

                                            ClientRequest.ClientRequestId = ClientRequest.ClientRequestId;
                                            //ClientRequest.PatientName = item["Patient"].ToString();
                                            ClientRequest.Totime = "20:00:00";
                                            ClientRequest.Date = item["Date"].ToString();
                                            ClientRequest.Office = ClientRequest.Office;

                                            ClientRequest.TimeOfVist = item["timeofvisit"].ToString();
                                            ClientRequest.NameOfPractice = item["Name of practice"].ToString();
                                            ClientRequest.DischargePlaneerName = item["Discharge planner name"].ToString();
                                            ClientRequest.FirstVisit = item["FirstVisit"].ToString();

                                            ClientRequest.ZipCode = ClientRequest.ZipCode;
                                            ClientRequest.Description = "";
                                            // ClientRequest.VisitTypeNames = item["Task Name"].ToString();
                                            ClientRequest.RepeatEvery = "";
                                            ClientRequest.RepeatTypeID = "";

                                            if (!string.IsNullOrEmpty(ClientRequest.Latitude) && !string.IsNullOrEmpty(ClientRequest.Longitude))
                                            {

                                                ClientRequest.Latitude = ClientRequest.Latitude;
                                                ClientRequest.Longitude = ClientRequest.Longitude;

                                            }
                                            else
                                            {

                                                // FullAddress = ClientRequest.Street + ',' + ClientRequest.City + ',' + ClientRequest.State + ',' + ClientRequest.ZipCode;
                                                // if (Convert.ToString(item["MRN"]) == Convert.ToString(ClientRequest.MedicalId))
                                                // if(String.Equals(item["MRN"].ToString(), ClientRequest.MedicalId))

                                                if (string.Compare(item["CRN Number"].ToString().Trim().ToLower(), ClientRequest.CRNumber.ToString().Trim().ToLower()) == 0)
                                                {
                                                    FullAddress = ClientRequest.Address + "," + ClientRequest.ZipCode;
                                                    //AIzaSyCqG0NdAH_5gP1_D9jGhmTGeqNR-9z_afw

                                                    var requestUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address=key{0}&key={1}", FullAddress, "AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
                                                    using (var client = new WebClient())
                                                    {
                                                        var result1 = client.DownloadString(requestUrl);
                                                        var data1 = JsonConvert.DeserializeObject<JObject>(result1);

                                                        var Latitude = data1["results"][0]["geometry"]["location"]["lat"];
                                                        var Longitude = data1["results"][0]["geometry"]["location"]["lng"];

                                                        ClientRequest.Latitude = Convert.ToString(Latitude);
                                                        ClientRequest.Longitude = Convert.ToString(Longitude);

                                                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                                                        TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
                                                        var timestamp = Math.Floor(diff.TotalSeconds);

                                                        var requestUri = string.Format("https://maps.googleapis.com/maps/api/timezone/json?location=" + Latitude + "," + Longitude + "&timestamp=" + timestamp + "&sensor=false &key=AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc&amp;");
                                                        HttpWebRequest request;
                                                        HttpWebResponse response;
                                                        request = (HttpWebRequest)WebRequest.Create(requestUri);
                                                        response = (HttpWebResponse)request.GetResponse();
                                                        TimeZoneJSON obj = null;
                                                        using (var sr = new StreamReader(response.GetResponseStream()))
                                                        {
                                                            obj = JsonConvert.DeserializeObject<TimeZoneJSON>(sr.ReadToEnd());

                                                            // var TimezoneOffset = Convert.ToInt32(obj.rawOffset) / 60;
                                                            var TimezoneOffset = (Convert.ToInt32(obj.dstOffset) + Convert.ToInt32(obj.rawOffset)) / 60;
                                                            obj.rawOffset = Convert.ToString(TimezoneOffset);
                                                            string abbr = string.Empty;
                                                            string[] splitString = obj.timeZoneName.Split(' ');
                                                            foreach (string abbrString in splitString)
                                                            {
                                                                abbr += abbrString[0];
                                                            }
                                                            obj.timeZoneName = abbr;
                                                        }

                                                        UpdateLatLong(ClientRequest.CRNumber, ClientRequest.Latitude, ClientRequest.Longitude, obj.timeZoneId, obj.timeZoneName, obj.rawOffset);
                                                    }
                                                }
                                            }


                                            if (worksheet.Cells[1, column].Value.ToString() == "Comments")
                                            {
                                                if (ClientRequest.ClientName.Contains(ClientName) && string.Compare(item["CRN Number"].ToString().Trim().ToLower(), ClientRequest.CRNumber.ToString().Trim().ToLower()) == 0 && ClientRequest.CRNumber != "" && ClientRequest.MarketersName != "" && ClientRequest.MarketersName != null && ClientRequest.CRNumber != null && !string.IsNullOrEmpty(ClientRequest.MarketersName) && !string.IsNullOrEmpty(ClientRequest.CRNumber))
                                                {
                                                    // schedulePatient.PatientName = item["Patient"].ToString();

                                                    finalresult = await AddPatientScheduleRequestInfo(ClientRequest);

                                                    // finalresult = "Success";

                                                    worksheet.Cells[y, column].Value = finalresult;
                                                    worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                    worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                    worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                    //  worksheet.Column(9).Style.Font.Bold = true;

                                                    item["Comments"] = finalresult;
                                                    Scheduled = Scheduled + 1;
                                                    y = y + 1;

                                                }
                                                else
                                                {
                                                    if (item["CRN Number"].ToString().Trim().ToLower() != ClientRequest.CRNumber.ToString().Trim().ToLower() && ClientRequest.CRNumber == null && string.IsNullOrEmpty(ClientRequest.CRNumber))
                                                    {

                                                        worksheet.Cells[y, column].Value = "ERROR: CRNUMBER(CRN) Not Found On Paseva";
                                                        worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                        worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                        worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                        worksheet.Column(column).Style.Font.Bold = true;

                                                        item["Comments"] = "ERROR: CRNUMBER(CRN) Not Found On Paseva";
                                                        error = error + 1;
                                                        y = y + 1;

                                                    }
                                                    else
                                                    {
                                                        if (ClientRequest.MarketersName == null && string.IsNullOrEmpty(ClientRequest.CRNumber))
                                                        {
                                                            worksheet.Cells[y, column].Value = "ERROR: Marketing Not Found On Paseva";
                                                            worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                            worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                            worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                            worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                            worksheet.Column(column).Style.Font.Bold = true;

                                                            item["Comments"] = "ERROR: Marketing Not Found On Paseva";
                                                            error = error + 1;
                                                            y = y + 1;

                                                        }
                                                        else
                                                        {
                                                            if (!ClientRequest.ClientName.Contains(ClientName))
                                                            {

                                                                worksheet.Cells[y, column].Value = "ERROR: Client & CRNUMBER(CRN) Data Mismatch Problem On Paseva";
                                                                worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                                worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                                worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                                worksheet.Column(column).Style.Font.Bold = true;

                                                                item["Comments"] = "Client & CRNUMBER(CRN) Mismatch Problem On Paseva";
                                                                error = error + 1;
                                                                y = y + 1;

                                                            }
                                                            else
                                                            {

                                                                worksheet.Cells[y, column].Value = "ERROR: Fail";
                                                                worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                                worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                                worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                                worksheet.Column(column).Style.Font.Bold = true;

                                                                item["Comments"] = "ERROR: Fail";
                                                                error = error + 1;
                                                                y = y + 1;

                                                            }
                                                        }

                                                    }

                                                }

                                                package.Save();

                                            }
                                        }
                                        else
                                        {
                                            if (worksheet.Cells[1, column].Value.ToString() == "Comments")
                                            {
                                                worksheet.Cells[y, column].Value = "ERROR: Marketers Not Available";
                                                worksheet.Cells[y, column].Worksheet.Column(column).Width = 35;
                                                worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Blue);
                                                worksheet.Cells[y, column].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                                                worksheet.Column(column).Style.Font.Bold = true;

                                                item["Comments"] = "ERROR: Marketers Not Available";
                                                error = error + 1;
                                                y = y + 1;
                                            }
                                            package.Save();
                                        }
                                }
                                else
                                    {
                                        if (worksheet.Cells[1, column].Value.ToString() == "Comments")
                                        {
                                            worksheet.Cells[y, column].Value = "ERROR: Client Not Availabe";
                                            worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                            worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                            worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Blue);
                                            worksheet.Cells[y, column].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                                            worksheet.Column(column).Style.Font.Bold = true;

                                            item["Comments"] = "ERROR: Client Not Availabe";
                                            error = error + 1;
                                            y = y + 1;
                                        }
                                       
                                    }

                                    package.Save();
                                }

                                if (SendUpdatedExcelToEmail(filePath2, FileName, Scheduled, error)) ;
                                {
                                    //return RedirectToAction("PatientRequest", "PatientRequest");
                                    myStringWebResource = remoteUri + FileName;

                                    return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }

                }
            }

            return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
        }

        #endregion


        // by Phone Number
        [HttpPost]
        // public async Task<ActionResult> ImportScheduledExcelData(HttpPostedFileBase file)
        public async Task<ActionResult> ImportScheduledExcelDataByPhoneNo()
        {
            string x = "";

            string finalresult = "";

            string FullAddress = "";

            string filePath = string.Empty;
            string filePath2 = "";
            // string FileName = "";

            string ObjStatus = "";

            int Scheduled = 0;
            int error = 0;
            int y = 1;
            string ClientName = "";

            string remoteUri = ConfigurationManager.AppSettings["ClientMarketingsBatchScheduling"].ToString(), myStringWebResource = "";

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
                        string path = Server.MapPath("~/Uploads/ClientBatchSchedule/");
                        string path2 = Server.MapPath("~/Uploads/ClientMarketingsBatchScheduling/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);

                        }

                        filePath = path + "AppointmentMarketingSchedule_" + DateTime.Now.ToString("MM.dd.yy hh.mm.ss") + "_" + Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filePath);

                        string FileName = "AppointmentMarketing_" + DateTime.Now.ToString("MM.dd.yy hh.mm") + Path.GetFileName(file.FileName);
                        filePath2 = System.IO.Path.Combine(path2, FileName); 
                        string extension2 = Path.GetExtension(file.FileName);

                        file.SaveAs(filePath2);

                        string conString = string.Empty;

                        switch (extension)
                        {
                            case ".xls":
                                conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 9.0;HDR=YES'";
                                break;
                            case ".xlsx":
                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;";
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
                                    try
                                    {
                                        connExcel.Open();
                                    }
                                    catch (Exception ex)
                                    {

                                        throw;
                                    }
                                    //Get the name of First Sheet.

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
                                try
                                {
                                    dt.Columns.Add(new DataColumn("Comments", typeof(string)));
                                }
                                catch (Exception ex)
                                {
                                    return Json("", JsonRequestBehavior.AllowGet);
                                }

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
                                int column = worksheet.Dimension.Columns + 1;

                                worksheet.Cells[1, column].Value = "Comments";
                                while (y == 1)
                                {
                                    worksheet.Cells[1, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[1, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.OrangeRed);
                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[1, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);

                                    //   worksheet.Column(9).Style.Font.Bold = true;

                                    package.Save();
                                    y++;
                                }

                                // loop through the worksheet rows           
                                //end of excel file existance
                                #endregion

                                foreach (DataRow item in dt.Rows)
                                {
                                    if (item["PhoneNo"].ToString() == "" || string.IsNullOrEmpty(item["PhoneNo"].ToString()))
                                    {
                                        break;
                                    }

                                    // ClientName = item["Name of practice"].ToString();
                                    // ClientName = item["Discharge planeer name "].ToString();
                                    //'+","+ item["Patient Last"].ToString();

                                    ClientRequest ClientRequest = new ClientRequest();
                                    if (item["PhoneNo"].ToString().Contains("-"))
                                    {

                                        ClientRequest.Phone = item["PhoneNo"].ToString().Replace("-", "");
                                    }
                                    else
                                    {
                                        ClientRequest.Phone = item["PhoneNo"].ToString();
                                    }

                                    // ClientRequest.CRNumber = item["CRN Number"].ToString();
                                    ClientRequest.MarketersName = item["MD name"].ToString();
                                    //+ " " + item["Provider Last"].ToString() + " " + item["Task Type"].ToString();
                                    string InsertedUserID = ConfigurationManager.AppSettings["AutomatedAppointementUserIdFromIndia"].ToString();

                                    ClientRequest.InsertUserId = InsertedUserID.ToString();
                                   //  Task.Run(async () => { ClientRequest = GetClientRequestDetailByCRNNumber(ClientRequest.InsertUserId, ClientRequest.CRNumber, ClientRequest.MarketersName); }).Wait();

                                   Task.Run(async () => { ClientRequest = GetClientRequestDetailByPhone(ClientRequest.InsertUserId, ClientRequest.Phone, ClientRequest.MarketersName); }).Wait();


                                    if (!string.IsNullOrEmpty(ClientRequest.ClientName))
                                    {

                                        if (!string.IsNullOrEmpty(ClientRequest.MarketersId))
                                        {
                                            ClientRequest.Street = ClientRequest.Street;
                                            // ClientRequest.ServiceNames = ClientRequest.ServiceNames;
                                            ClientRequest.TimezoneOffset = Convert.ToInt32(ClientRequest.TimezoneOffset);
                                            ClientRequest.City = ClientRequest.City;
                                            ClientRequest.Address = ClientRequest.Address;
                                            ClientName = ClientRequest.ClientName;

                                            ClientRequest.TimezoneId = ClientRequest.TimezoneId;

                                            //ClientRequest.IsCancelled = Convert.ToBoolean(false);
                                            ClientRequest.State = ClientRequest.State;
                                            ClientRequest.TimezonePostfix = ClientRequest.TimezonePostfix;
                                            ClientRequest.Fromtime = "02:00:00";

                                            ClientRequest.ClientRequestId = ClientRequest.ClientRequestId;
                                            //ClientRequest.PatientName = item["Patient"].ToString();
                                            ClientRequest.Totime = "20:00:00";
                                            ClientRequest.Date = item["Date"].ToString();
                                            ClientRequest.Office = ClientRequest.Office;

                                            ClientRequest.TimeOfVist = item["timeofvisit"].ToString();
                                            ClientRequest.NameOfPractice = item["Name of practice"].ToString();
                                            ClientRequest.DischargePlaneerName = item["Discharge planeer name"].ToString();
                                            ClientRequest.FirstVisit = item["FirstVisit"].ToString();

                                            ClientRequest.ZipCode = ClientRequest.ZipCode;
                                            ClientRequest.Description = "";
                                            // ClientRequest.VisitTypeNames = item["Task Name"].ToString();
                                            ClientRequest.RepeatEvery = "";
                                            ClientRequest.RepeatTypeID = "";

                                            if (!string.IsNullOrEmpty(ClientRequest.Latitude) && !string.IsNullOrEmpty(ClientRequest.Longitude))
                                            {

                                                ClientRequest.Latitude = ClientRequest.Latitude;
                                                ClientRequest.Longitude = ClientRequest.Longitude;

                                            }
                                            else
                                            {
                                                // FullAddress = ClientRequest.Street + ',' + ClientRequest.City + ',' + ClientRequest.State + ',' + ClientRequest.ZipCode;
                                                // if (Convert.ToString(item["MRN"]) == Convert.ToString(ClientRequest.MedicalId))
                                                // if(String.Equals(item["MRN"].ToString(), ClientRequest.MedicalId))

                                                if (string.Compare(item["PhoneNo"].ToString().Trim().ToLower(), ClientRequest.Phone.ToString().Trim().ToLower()) == 0)
                                                {
                                                    FullAddress = ClientRequest.Address + "," + ClientRequest.ZipCode;
                                                    //AIzaSyCqG0NdAH_5gP1_D9jGhmTGeqNR-9z_afw

                                                    var requestUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address=key{0}&key={1}", FullAddress, "AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
                                                    using (var client = new WebClient())
                                                    {
                                                        var result1 = client.DownloadString(requestUrl);
                                                        var data1 = JsonConvert.DeserializeObject<JObject>(result1);

                                                        var Latitude = data1["results"][0]["geometry"]["location"]["lat"];
                                                        var Longitude = data1["results"][0]["geometry"]["location"]["lng"];

                                                        ClientRequest.Latitude = Convert.ToString(Latitude);
                                                        ClientRequest.Longitude = Convert.ToString(Longitude);

                                                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                                                        TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
                                                        var timestamp = Math.Floor(diff.TotalSeconds);

                                                        var requestUri = string.Format("https://maps.googleapis.com/maps/api/timezone/json?location=" + Latitude + "," + Longitude + "&timestamp=" + timestamp + "&sensor=false &key=AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc&amp;");
                                                        HttpWebRequest request;
                                                        HttpWebResponse response;
                                                        request = (HttpWebRequest)WebRequest.Create(requestUri);
                                                        response = (HttpWebResponse)request.GetResponse();
                                                        TimeZoneJSON obj = null;
                                                        using (var sr = new StreamReader(response.GetResponseStream()))
                                                        {
                                                            obj = JsonConvert.DeserializeObject<TimeZoneJSON>(sr.ReadToEnd());

                                                            // var TimezoneOffset = Convert.ToInt32(obj.rawOffset) / 60;
                                                            var TimezoneOffset = (Convert.ToInt32(obj.dstOffset) + Convert.ToInt32(obj.rawOffset)) / 60;
                                                            obj.rawOffset = Convert.ToString(TimezoneOffset);
                                                            string abbr = string.Empty;
                                                            string[] splitString = obj.timeZoneName.Split(' ');
                                                            foreach (string abbrString in splitString)
                                                            {
                                                                abbr += abbrString[0];
                                                            }
                                                            obj.timeZoneName = abbr;
                                                        }

                                                        UpdateLatLongByPhoneNo(ClientRequest.Phone, ClientRequest.Latitude, ClientRequest.Longitude, obj.timeZoneId, obj.timeZoneName, obj.rawOffset);
                                                    }
                                                }
                                            }


                                            if (worksheet.Cells[1, column].Value.ToString() == "Comments")
                                            {
                                                if (ClientRequest.ClientName.Contains(ClientName) && string.Compare(item["PhoneNo"].ToString().Trim().ToLower(), ClientRequest.Phone.ToString().Trim().ToLower()) == 0 && ClientRequest.Phone != "" && ClientRequest.MarketersName != "" && ClientRequest.MarketersName != null && ClientRequest.Phone != null && !string.IsNullOrEmpty(ClientRequest.MarketersName) && !string.IsNullOrEmpty(ClientRequest.Phone))
                                                {
                                                    // schedulePatient.PatientName = item["Patient"].ToString();

                                                    finalresult = await AddPatientScheduleRequestInfo(ClientRequest);

                                                    // finalresult = "Success";

                                                    worksheet.Cells[y, column].Value = finalresult;
                                                    worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                    worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                    worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                    //  worksheet.Column(9).Style.Font.Bold = true;

                                                    item["Comments"] = finalresult;
                                                    Scheduled = Scheduled + 1;
                                                    y = y + 1;

                                                }
                                                else
                                                {
                                                    if (item["PhoneNo"].ToString().Trim().ToLower() != ClientRequest.Phone.ToString().Trim().ToLower() && ClientRequest.Phone == null && string.IsNullOrEmpty(ClientRequest.Phone))
                                                    {

                                                        worksheet.Cells[y, column].Value = "ERROR: PhoneNo Not Found On Paseva";
                                                        worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                        worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                        worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                        worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                        worksheet.Column(column).Style.Font.Bold = true;

                                                        item["Comments"] = "ERROR: PhoneNo Not Found On Paseva";
                                                        error = error + 1;
                                                        y = y + 1;

                                                    }
                                                    else
                                                    {
                                                        if (ClientRequest.MarketersName == null && string.IsNullOrEmpty(ClientRequest.Phone))
                                                        {
                                                            worksheet.Cells[y, column].Value = "ERROR: Marketing Not Found On Paseva";
                                                            worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                            worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                            worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                            worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                            worksheet.Column(column).Style.Font.Bold = true;

                                                            item["Comments"] = "ERROR: Marketing Not Found On Paseva";
                                                            error = error + 1;
                                                            y = y + 1;

                                                        }
                                                        else
                                                        {
                                                            if (!ClientRequest.ClientName.Contains(ClientName))
                                                            {

                                                                worksheet.Cells[y, column].Value = "ERROR: Client & PhoneNo Data Mismatch Problem On Paseva";
                                                                worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                                worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                                worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                                worksheet.Column(column).Style.Font.Bold = true;

                                                                item["Comments"] = "Client & PhoneNo Mismatch Problem On Paseva";
                                                                error = error + 1;
                                                                y = y + 1;

                                                            }
                                                            else
                                                            {

                                                                worksheet.Cells[y, column].Value = "ERROR: Fail";
                                                                worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                                worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                                worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                                                worksheet.Column(column).Style.Font.Bold = true;

                                                                item["Comments"] = "ERROR: Fail";
                                                                error = error + 1;
                                                                y = y + 1;

                                                            }
                                                        }

                                                    }

                                                }

                                                package.Save();

                                            }
                                        }
                                        else
                                        {
                                            if (worksheet.Cells[1, column].Value.ToString() == "Comments")
                                            {
                                                worksheet.Cells[y, column].Value = "ERROR: Marketers Not Available";
                                                worksheet.Cells[y, column].Worksheet.Column(column).Width = 35;
                                                worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                                worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                                worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Blue);
                                                worksheet.Cells[y, column].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                                                worksheet.Column(column).Style.Font.Bold = true;

                                                item["Comments"] = "ERROR: Marketers Not Available";
                                                error = error + 1;
                                                y = y + 1;
                                            }
                                            package.Save();
                                        }
                                    }
                                    else
                                    {
                                        if (worksheet.Cells[1, column].Value.ToString() == "Comments")
                                        {
                                            worksheet.Cells[y, column].Value = "ERROR: Client Not Availabe";
                                            worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                            worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                            worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Blue);
                                            worksheet.Cells[y, column].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                                            worksheet.Column(column).Style.Font.Bold = true;

                                            item["Comments"] = "ERROR: Client Not Availabe";
                                            error = error + 1;
                                            y = y + 1;
                                        }

                                    }

                                    package.Save();
                                }

                                if (SendUpdatedExcelToEmail(filePath2, FileName, Scheduled, error)) ;
                                {
                                    //return RedirectToAction("PatientRequest", "PatientRequest");
                                    myStringWebResource = remoteUri + FileName;

                                    return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }

                }
            }

            return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<string> AddPatientScheduleRequestInfo(ClientRequest objClientRequest)
        {

            var ClientRequest = new ClientRequest();
            // var PatientRequest = new SchedulePatientRequest();
            string MarketersName = "";


            string[] s1 = objClientRequest.MarketersName.ToString().Split(',');

            // string s1 = objClientRequest.Caregiver.ToString();
            string jsonStringdata = "";
            var resultdata = "";

            //string jsonString = JsonConvert.SerializeObject(objClientRequest, Newtonsoft.Json.Formatting.Indented);

            //ServicesServiceProxy ServicesService = new ServicesServiceProxy();

            //MembershipUser user = Membership.GetUser();
            //string userid =  user.ProviderUserKey.ToString();
            //var uid = Convert.ToString(Session["UserId"]);
            // string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
            // objClientRequest.InsertUserId = InsertedUserID;

            //PatientRequest.InsertUserId = objClientRequest.InsertUserId;
            try
            {
              //  SchedulePatientRequestServiceProxy CareGiverLiteService = new SchedulePatientRequestServiceProxy();

                ClientRequest.ClientRequestId = objClientRequest.ClientRequestId;
                ClientRequest.ClientName = objClientRequest.ClientName;
                ClientRequest.Address = objClientRequest.Address;
                ClientRequest.Street = objClientRequest.Street;
                ClientRequest.City = objClientRequest.City;
                ClientRequest.State = objClientRequest.State;
                ClientRequest.Latitude = objClientRequest.Latitude;
                ClientRequest.Longitude = objClientRequest.Longitude;
                ClientRequest.ZipCode = objClientRequest.ZipCode;
                ClientRequest.CRNumber = objClientRequest.CRNumber;
                ClientRequest.Description = objClientRequest.Description;
                ClientRequest.Date = objClientRequest.Date.ToString().Split(' ')[0];
                //ClientRequest.Date = DateTime.ParseExact(objClientRequest.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString().Split(' ')[0];

                //string formattedDate = date.ToString("yyyy-MM-dd");
                ClientRequest.Fromtime = objClientRequest.Fromtime;
                ClientRequest.Totime = objClientRequest.Totime;
               // ClientRequest.IsCancelled = objClientRequest.IsCancelled;

                ClientRequest.InsertUserId = objClientRequest.InsertUserId;

                //if (objClientRequest.ServiceNames != null)
                //    ClientRequest.ServiceNames = objClientRequest.ServiceNames.Trim(',');
                //if (objClientRequest.VisitTypeNames != null)
                //    ClientRequest.VisitTypeNames = objClientRequest.VisitTypeNames;
                ////.Trim(',');

                ClientRequest.TimeOfVist = objClientRequest.TimeOfVist;
                ClientRequest.NameOfPractice = objClientRequest.NameOfPractice;
                ClientRequest.DischargePlaneerName = objClientRequest.DischargePlaneerName;
                ClientRequest.FirstVisit = objClientRequest.FirstVisit;

                ClientRequest.Office = objClientRequest.Office;
                ClientRequest.TimezoneId = objClientRequest.TimezoneId;
                ClientRequest.TimezoneOffset = objClientRequest.TimezoneOffset;
                ClientRequest.TimezonePostfix = objClientRequest.TimezonePostfix;

                ClientRequest.IsRepeat = objClientRequest.IsRepeat;
                ClientRequest.RepeatEvery = objClientRequest.RepeatEvery;
                ClientRequest.RepeatTypeID = objClientRequest.RepeatTypeID;
                ClientRequest.RepeatDate = objClientRequest.RepeatDate;
                ClientRequest.DayOfWeek = objClientRequest.DayOfWeek;
                ClientRequest.DaysOfMonth = objClientRequest.DaysOfMonth;

                ClientRequest.MarketersName = objClientRequest.MarketersName;

                // CaregiverLiteWCF.GetDatesList dates = CareGiverLiteService.GetFilterDates(ClientRequest);
                //this code is commented 
                // objss.ClientRequest = ClientRequest;
                //these are commmented code if not called ws api application

                jsonStringdata = JsonConvert.SerializeObject(ClientRequest, Newtonsoft.Json.Formatting.Indented);

               RootRegister newdata = AddAppointmentsNew(ClientRequest);

                resultdata = newdata.message;

                if (resultdata == "Success")
                {

                    return resultdata;
                    //var GroupName1 = PatientRequest.PatientName.Trim() + "(" + PatientRequest.MedicalId.Trim() + ")";

                    //var chkData = checkDialogId(GroupName1);

                    //if (chkData.Data.Equals(true))
                    //{
                    //    return resultdata;
                    //}
                    //else
                    //{
                    //    var SuperAdminQBID = "32132455";

                    //    Task.Run(async () =>
                    //    {
                    //        ChattingController obj = new ChattingController();
                    //        string rsultnew = await obj.CreatePatientGroupChatOnQuickBloxRestAPI(GroupName1, SuperAdminQBID, PatientRequest.Office, PatientRequest.InsertUserId);
                    //    }).Wait();
                    //}



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

        public ClientRequest GetClientRequestDetailByCRNNumber(string LoginUserId, string CRNNumber, string Marketer)
        {
            ClientRequest ClientRequest = new ClientRequest();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientRequestDetailByCRNNumber", LoginUserId, CRNNumber, Marketer);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ClientRequest.ClientRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientRequestId"]);
                    ClientRequest.ClientName = ds.Tables[0].Rows[0]["ClientName"].ToString();
                  //  ClientRequest.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    ClientRequest.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    ClientRequest.City = ds.Tables[0].Rows[0]["City"].ToString();
                    ClientRequest.State = ds.Tables[0].Rows[0]["State"].ToString();
                    ClientRequest.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    ClientRequest.CRNumber = ds.Tables[0].Rows[0]["CRNumber"].ToString();
                    ClientRequest.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    ClientRequest.InsertUserId = LoginUserId;
                    ClientRequest.Address = ClientRequest.Street + " " + ClientRequest.City + " " + ClientRequest.State;
                    //objClientRequest.InsertDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy");
                    //objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //objSchedulePatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    // objSchedulePatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    ClientRequest.MarketersName = ds.Tables[1].Rows[0]["MarketersId"].ToString();
                    ClientRequest.MarketersId= ds.Tables[1].Rows[0]["MarketersId"].ToString();

                    // ClientRequest.ServiceNames = ds.Tables[2].Rows[0]["ServiceNames"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Latitude"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Longitude"].ToString()))
                    {
                        ClientRequest.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                        ClientRequest.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    }
                    else
                    {
                        ClientRequest.Latitude = ds.Tables[0].Rows[0]["Latitudes"].ToString();
                        ClientRequest.Longitude = ds.Tables[0].Rows[0]["Longitudes"].ToString();
                    }

                    ClientRequest.OfficeName = ds.Tables[0].Rows[0]["Office"].ToString();
                    ClientRequest.Office = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneId"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString()))
                    {
                        ClientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneId"].ToString();
                        ClientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString());
                        ClientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfix"].ToString();
                        // objSchedulePatientRequest.OfficeAddress = ds.Tables[0].Rows[0]["OfficeAddress"].ToString();
                    }
                    else
                    {
                        ClientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneIds"].ToString();
                        ClientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffsets"].ToString());
                        ClientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfixs"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "MarketersLiteService";
                objErrorlog.Methodname = "GetMarketersDetailsByUserId";
                // string result = InsertErrorLog(objErrorlog);
                return ClientRequest;
            }
            return ClientRequest;
        }

        public ClientRequest GetClientRequestDetailByPhone(string LoginUserId, string Phone, string Marketer)
        {
            ClientRequest ClientRequest = new ClientRequest();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientRequestDetailByPhoneNo", LoginUserId, Phone, Marketer);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ClientRequest.ClientRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientRequestId"]);
                    ClientRequest.ClientName = ds.Tables[0].Rows[0]["ClientName"].ToString();
                    //  ClientRequest.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    ClientRequest.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    ClientRequest.City = ds.Tables[0].Rows[0]["City"].ToString();
                    ClientRequest.State = ds.Tables[0].Rows[0]["State"].ToString();
                    ClientRequest.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    ClientRequest.CRNumber = ds.Tables[0].Rows[0]["CRNumber"].ToString();
                    ClientRequest.Phone=  ds.Tables[0].Rows[0]["Phone"].ToString();
                    ClientRequest.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    ClientRequest.InsertUserId = LoginUserId;
                    ClientRequest.Address = ClientRequest.Street + " " + ClientRequest.City + " " + ClientRequest.State;
                    //objClientRequest.InsertDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy");
                    //objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //objSchedulePatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    // objSchedulePatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    ClientRequest.MarketersName = ds.Tables[1].Rows[0]["MarketersId"].ToString();
                    ClientRequest.MarketersId = ds.Tables[1].Rows[0]["MarketersId"].ToString();

                    // ClientRequest.ServiceNames = ds.Tables[2].Rows[0]["ServiceNames"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Latitude"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Longitude"].ToString()))
                    {
                        ClientRequest.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                        ClientRequest.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    }
                    else
                    {
                        ClientRequest.Latitude = ds.Tables[0].Rows[0]["Latitudes"].ToString();
                        ClientRequest.Longitude = ds.Tables[0].Rows[0]["Longitudes"].ToString();
                    }

                    ClientRequest.OfficeName = ds.Tables[0].Rows[0]["Office"].ToString();
                    ClientRequest.Office = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneId"].ToString()) && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString()))
                    {
                        ClientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneId"].ToString();
                        ClientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString());
                        ClientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfix"].ToString();
                        // objSchedulePatientRequest.OfficeAddress = ds.Tables[0].Rows[0]["OfficeAddress"].ToString();
                    }
                    else
                    {
                        ClientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneIds"].ToString();
                        ClientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffsets"].ToString());
                        ClientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfixs"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "MarketersLiteService";
                objErrorlog.Methodname = "GetMarketersDetailsByUserId";
                // string result = InsertErrorLog(objErrorlog);
                return ClientRequest;
            }
            return ClientRequest;
        }

        public RootRespone GetMarketingBasedOnInputedMilesAndNumberOfMarketers(MarketingsDetail MarketingDetails)
        {
            RootRespone rootresponse = new RootRespone();

            List<MarketingsDetail> objMarketingListing = new List<MarketingsDetail>();
            string result = "";
            string lattitude = "";
            string longitude = "";

            List<MarketingsDetail> newcaregiverlist = new List<MarketingsDetail>();

            //Geocoder geocoder = new Geocoder("AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
            //var location = geocoder.Geocode(MarketingDetails.Address);
            //foreach (var x in location)
            //{

            //    lattitude =Convert.ToString (x.LatLng.Latitude);
            //    longitude = Convert.ToString(x.LatLng.Longitude);
            //}

            try
            {
                //if (!string.IsNullOrEmpty(MarketingDetails.ServiceNames) && MarketingDetails.ServiceNames.EndsWith(","))
                //{
                //    MarketingDetails.ServiceNames = MarketingDetails.ServiceNames.TrimEnd(MarketingDetails.ServiceNames[MarketingDetails.ServiceNames.Length - 1]);
                //}
                SqlConnection con = new SqlConnection(Settings.CareGiverSuperAdminDatabase().ToString());
                //SqlCommand cmd = new SqlCommand("GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers_PatientMobApp", con);

                SqlCommand cmd = new SqlCommand("GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers_PatientMobAppTOPnew", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PatientName", MarketingDetails.MarketersName);
                //cmd.Parameters.AddWithValue("@Address", MarketingDetails.Address);
                cmd.Parameters.AddWithValue("@Latitude ", MarketingDetails.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", MarketingDetails.Longitude);


                //cmd.Parameters.AddWithValue("@Latitude ", lattitude);
                //cmd.Parameters.AddWithValue("@Longitude", longitude);
                //cmd.Parameters.AddWithValue("@ZipCode", MarketingDetails.ZipCode);
                cmd.Parameters.AddWithValue("@MedicalId", MarketingDetails.MarketersId);
                //cmd.Parameters.AddWithValue("@Description", MarketingDetails.Description);


                //cmd.Parameters.AddWithValue("@Date", MarketingDetails.Date);
                //cmd.Parameters.AddWithValue("@FromTime", MarketingDetails.FromTime);
                //cmd.Parameters.AddWithValue("@ToTime", MarketingDetails.ToTime);


                //cmd.Parameters.AddWithValue("@IsCancelled", MarketingDetails.IsCancelled);
               // cmd.Parameters.AddWithValue("@ServiceNames", MarketingDetails.ServiceNames);
                cmd.Parameters.AddWithValue("@TimezoneId", MarketingDetails.TimezoneId);
                cmd.Parameters.AddWithValue("@TimezoneOffset", MarketingDetails.TimezoneOffset);
                cmd.Parameters.AddWithValue("@TimezonePostfix", MarketingDetails.TimezonePostfix);

               // cmd.Parameters.AddWithValue("@MaxDistance", MarketingDetails.MaxDistance);
                //cmd.Parameters.AddWithValue("@MaxCaregiver", MarketingDetails.MaxCaregiver);
                cmd.Parameters.AddWithValue("@OfficeId", Convert.ToInt32(MarketingDetails.OfficeId));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                con.Open();
                da.Fill(ds);


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        MarketingsDetail objMarketer = new MarketingsDetail();
                        objMarketer.MarketersId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        objMarketer.MarketersName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objMarketer.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objMarketer.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                        objMarketer.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objMarketer.Address = ds.Tables[0].Rows[i]["address"].ToString();
                        objMarketer.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objMarketer.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objMarketer.State = ds.Tables[0].Rows[i]["State"].ToString();
                        //objMarketer.DeviceToken = ds.Tables[0].Rows[i]["DeviceToken"].ToString();
                        //objMarketer.DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
                        var lat = ds.Tables[0].Rows[i]["latitude"].ToString();
                        var lng = ds.Tables[0].Rows[i]["longitude"].ToString();
                        //objMarketer.ProfileImage = ds.Tables[0].Rows[i]["ProfileImage"].ToString();
                        //objMarketer.ServiceId = ds.Tables[0].Rows[i]["Service"].ToString();
                        //objMarketer.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        //objMarketer.QuickbloxId = ds.Tables[0].Rows[i]["QuickbloxId"].ToString();
                        //objMarketer.UserId = ds.Tables[0].Rows[i]["UserID"].ToString();
                        //objMarketer.Education = ds.Tables[0].Rows[i]["Education"].ToString();
                        objMarketer.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"]);

                       // objMarketer.HourlyRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["HourlyRate"].ToString());

                        //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Experience"].ToString()))
                        //{
                        //    objMarketer.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        //}
                        //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Description"].ToString()))
                        //{
                        //    objMarketer.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                        //}


                        //if database does not contain latlong for particular Nurse
                        if (lat != "" && lat != "")
                        {
                            objMarketer.Latitude = (ds.Tables[0].Rows[i]["latitude"].ToString());
                            objMarketer.Longitude = (ds.Tables[0].Rows[i]["longitude"].ToString());
                        }
                        else
                        {
                            objMarketer.Latitude =string.Empty;
                            objMarketer.Longitude =string.Empty;
                        }

                      //  objMarketer.CurrentLatitude = ds.Tables[0].Rows[i]["CurrentLatitude"].ToString();
                      //  objMarketer.CurrentLongitude = ds.Tables[0].Rows[i]["CurrentLongitude"].ToString();
                        if (objMarketer.Latitude != null && objMarketer.Longitude != null)
                        {
                            //objMarketer.DistanceUnit = GetDistanceBetweenTwoLatlong(Convert.ToDouble(lattitude), Convert.ToDouble(longitude), objMarketer.Latitude, objMarketer.Longitude);

                            objMarketer.DistanceUnit = GetDistanceBetweenTwoLatlong(Convert.ToDouble(MarketingDetails.Latitude), Convert.ToDouble(MarketingDetails.Longitude), Convert.ToDouble(objMarketer.Latitude), Convert.ToDouble(objMarketer.Longitude));
                        }
                        else
                        {
                            objMarketer.DistanceUnit = 0;
                        }

                       // objMarketer.IsNurseBusy = ds.Tables[0].Rows[i]["NurseBusy"].ToString();
                       // objMarketer.Office = ds.Tables[0].Rows[i]["Office"].ToString();


                        if (objMarketer.DistanceUnit != 0)
                        {
                            if (objMarketer.DistanceUnit <= MarketingDetails.MaxDistance)
                            {
                                objMarketingListing.Add(objMarketer);
                            }
                        }

                    }
                    // objCareGiverListing.Take(1);

                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }

            //if (MarketingDetails.OfficeId.Length > 0)
            //{
            //    for (int i = 0; i <= MarketingDetails.OfficeId.Length-1; i++)
            //    {

            //        objCareGiverListing.Select(x => x.OfficeId = MarketingDetails.OfficeId[i]);



            //            //.Take(MarketingDetails.MaxCaregiver).ToList();

            //    }

            //    return objCareGiverListing.Take(MarketingDetails.MaxCaregiver).ToList();
            //}
            //else
            //{
            //    //  return 
            //   return  objCareGiverListing.Take(MarketingDetails.MaxCaregiver).ToList();

            //}
            int MaxCaregiver = 1;

            if (objMarketingListing != null)
            {
                rootresponse.success = 1;
                rootresponse.message = "Success";
                rootresponse.data = objMarketingListing.Take(MaxCaregiver).ToList();
            }
            else
            {
                rootresponse.success = 0;
                rootresponse.message = "Fail";
                 rootresponse.data = objMarketingListing.Take(MaxCaregiver).ToList();
            }


            return rootresponse;
        }


        private double GetDistanceBetweenTwoLatlong(double lat1, double long1, double lat2, double long2)
        {
            {
                if ((lat1 == lat2) && (long1 == long2))
                {
                    return 0;
                }
                else
                {
                    long1 = degree2radian(long1);
                    long2 = degree2radian(long2);
                    lat1 = degree2radian(lat1);
                    lat2 = degree2radian(lat2);

                    double dlon = long1 - long2;
                    double dlat = lat2 - lat1;
                    double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                               Math.Cos(lat1) * Math.Cos(lat2) *
                               Math.Pow(Math.Sin(dlon / 2), 2);

                    double c = 2 * Math.Asin(Math.Sqrt(a));

                    double r = 6371 / 1.609344;
                    return (c * r);

                }
            }
        }

        private double degree2radian(double deg)
        {
            return (deg * Math.PI / 190.0);
        }

        public RootRegister AddAppointmentsNew(ClientRequest ClientRequest)
        {
            RootRegister objStatus = new RootRegister();
            string resultForCaregiverRequest = "";
            string caregiver = "";
            string s1 = ClientRequest.MarketersName;

            // CaregiverLiteWCF.GetDatesList dates = CareGiverLiteService.GetFilterDates(PatientRequest);

            try
            {
                MarketingsDetail result = InsertSchedulePatientRequest(ClientRequest);

                //for (int i = 0; i < result.CareGiverList.Count; i++)
                //{
                if (s1.Equals(result.MarketersId.ToString()))
                {//if care giver is matched Start
                    var obj = result;
                    PushNotification objNotification = new PushNotification();
                    var userid = ClientRequest.InsertUserId;

                    var ClientRequestFromTime = DateTime.ParseExact(ClientRequest.Fromtime, "HH:mm:ss", CultureInfo.InvariantCulture);
                    var ClientRequestToTime = DateTime.ParseExact(ClientRequest.Totime, "HH:mm:ss", CultureInfo.InvariantCulture);

                    var ConvertFromTime = ClientRequestFromTime.ToString("hh:mm tt");
                    var ConvertToTime = ClientRequestToTime.ToString("hh:mm tt");
                    var ConvertPatientDate = Convert.ToDateTime(ClientRequest.Date).ToString("dd MMM yyyy");
                    //string Notificationresult = objNotification.SendPushNotification("Patient Schedule Request", "New Patient Schedule Request", obj.DeviceToken, obj.DeviceType, null, null, obj.NurseId.ToString(), null, "Patient Schedule Request", userid, true);
                    string Notificationresult = objNotification.SendPushNotification("Client Schedule Request", "Client request added for " + ConvertFromTime + " to " + ConvertToTime + " " + ClientRequest.TimezonePostfix + " on " + ConvertPatientDate, obj.DeviceToken, obj.DeviceType, null, null, obj.MarketersId.ToString(), null, "Patient Schedule Request", userid, true, "", obj.ClientRequestId);

                    resultForCaregiverRequest = InsertScheduleClientRequestTemp(obj.ClientRequestId.ToString(), obj.MarketersId.ToString(), Notificationresult);
                }//if care giver is matched End
                 // }

                //  return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = true });
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "PatientRequestController";
                log.Methodname = "[HttpPost] AddPatientRequestInfo";
                return objStatus;
                //  ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                // string res = ErrorLogService.InsertErrorLog(log).Result;
                // return RedirectToAction("PatientRequest", "PatientRequest", new { IsAdded = false });
            }
            if (resultForCaregiverRequest == "Success")
            {

                objStatus.message = "Success";
                objStatus.success = 1;

            }
            else
            {
                objStatus.message = "Fail";
                objStatus.success = 0;
            }

            return objStatus;
        }

        public List<GetDatesList> GetFilterDates(ClientRequest ClientRequest)
        {
            List<GetDatesList> GetDatesList = new List<GetDatesList>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetFilterDates",
                                                       ClientRequest.Date,// + " 00:00:00").ToString().Split(' ')[0],
                                                       ClientRequest.IsRepeat,
                                                       ClientRequest.RepeatEvery,
                                                       ClientRequest.RepeatTypeID,
                                                       ClientRequest.RepeatDate,
                                                       ClientRequest.DayOfWeek,
                                                       ClientRequest.DaysOfMonth
                                                       );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GetDatesList objGetDatesList = new GetDatesList();
                        objGetDatesList.ListDate = Convert.ToString(ds.Tables[0].Rows[i]["dates"]);

                        GetDatesList.Add(objGetDatesList);
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllDate";
                string result = InsertErrorLog(objErrorlog);
            }

            return GetDatesList;

        }

        public MarketingsDetail InsertSchedulePatientRequest(ClientRequest ClientRequest)
        {
            //   CareGiversList objList = new CareGiversList();
            //  List<CareGivers> objCareGiverListing = new List<CareGivers>();
            MarketingsDetail objMarketers = new MarketingsDetail();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CaregiverLiteDatabase().ToString(), "InsertScheduleClientRequestBooking",
                                                        ClientRequest.ClientName,
                                                       // ClientRequest.Address,
                                                        ClientRequest.Street,
                                                        ClientRequest.City,
                                                        ClientRequest.State,
                                                        ClientRequest.Latitude,
                                                        ClientRequest.Longitude,
                                                        ClientRequest.ZipCode,
                                                        ClientRequest.CRNumber,
                                                        ClientRequest.Description,
                                                        ClientRequest.Date,
                                                        ClientRequest.Fromtime,
                                                        ClientRequest.Totime,
                                                        ClientRequest.TimeOfVist,
                                                        ClientRequest.NameOfPractice,
                                                        ClientRequest.DischargePlaneerName,
                                                        ClientRequest.FirstVisit,

                                                        //ClientRequest.IsCancelled,
                                                        //ClientRequest.ServiceNames,
                                                       // ClientRequest.VisitTypeNames,

                                                        ClientRequest.TimezoneId,
                                                        ClientRequest.TimezoneOffset,
                                                        ClientRequest.TimezonePostfix,
                                                        Guid.Parse(ClientRequest.InsertUserId),
                                                       // ConfigurationManager.AppSettings["MaxDistance"],
                                                      // 500,
                                                        ClientRequest.Office,
                                                        ClientRequest.MarketersName);
                //SchedulePatientRequest.InsertUserId);
                //new System.Data.SqlTypes.SqlGuid(Guid.NewGuid().ToString())); 

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        // CareGivers objCareGivers = new CareGivers();
                        objMarketers.MarketersId = Convert.ToInt32(ds.Tables[0].Rows[i]["MarketersId"].ToString());
                        objMarketers.ClientRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["ClientRequestId"].ToString());
                       // objMarketers.DeviceToken = ds.Tables[0].Rows[i]["DeviceToken"].ToString();
                       // objMarketers.DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
                        objMarketers.Office = ds.Tables[0].Rows[i]["OfficeId"].ToString();
                        // objCareGiverListing.Add(objCareGivers);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertSchedulePatientRequest";
                objErrorlog.UserID = ClientRequest.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            // objList.CareGiverList = objCareGiverListing;
            // return objList;
            return objMarketers;
        }

        public string InsertScheduleClientRequestTemp(string ClientRequestId, string MarketersId, string IsNotificationSent)
        {
       
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CaregiverLiteDatabase().ToString(), "InsertScheduleClientRequestTemp",
                                                      ClientRequestId,
                                                      MarketersId,
                                                      IsNotificationSent.ToLower() == "success" ? true : false,
                                                      new System.Data.SqlTypes.SqlGuid(Guid.NewGuid().ToString()));



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        result = ds.Tables[0].Rows[i]["Result"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertSchedulePatientRequestTemp";
                objErrorlog.UserID = null;
                result = InsertErrorLog(objErrorlog);
            }
           // objList.CareGiverList = objCareGiverListing;
            return result;
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

        public bool SendUpdatedExcelToEmail(string filepath, string FileName, int Scheduled, int Error)
        {
            // string filepaths = ConfigurationManager.AppSettings["LocalBulkScheduling"].ToString() + "\\" + FileName;


            string myStringWebResource = "";
            //  remoteUri = ConfigurationManager.AppSettings["BulkScheduling"].ToString(),

            //string fileName = "ms-banner.gif", 
            string msg = string.Empty;
            string AttachmentFileName = filepath;
            string subject = "Client Schedule" + " " + DateTime.Now.ToString("MM.dd.yyyy").ToString();

            bool IsFileAttachment = true;

            msg += "<b>Scheduled:</b> " + Scheduled + "<br>";
            msg += "<b>Error:</b> " + Error + "<br><br>";

            msg += "Warm Regards,<br>";
            msg += "Team PaSeva Automation";


            string body = msg;

            string Result1 = "";

            string toAddress = ConfigurationManager.AppSettings["MarketerAppointmentMailScheduling"].ToString();
            string CCMailID = ConfigurationManager.AppSettings["MarketerAppointmentCCMailScheduling"].ToString();


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


                MailAddress ma = new MailAddress(ConfigurationManager.AppSettings["MarketerAppointmentOutLookMail"].ToString());
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


                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MarketerAppointmentOutLookMail"].ToString(), ConfigurationManager.AppSettings["MarketerAppointmentOutlookPassword"].ToString());
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

        [HttpGet]
        public ActionResult EditmarketingDetail(string id)
        {
            MarketerDetailsModel objMarketerDetail = new MarketerDetailsModel();
            try
            {
                int officeId = 0;

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetmarketerDetailById", id);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objMarketerDetail.MarketersId = Convert.ToInt32(ds.Tables[0].Rows[0]["MarketersId"]);
                    objMarketerDetail.MarketersName = ds.Tables[0].Rows[0]["MarketersName"].ToString();
                    objMarketerDetail.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    objMarketerDetail.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    //objMarketerDetail.MedicalId= Convert.ToInt32(ds.Tables[0].Rows[0][""]).ToString();
                    objMarketerDetail.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    objMarketerDetail.PhoneNo = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                    //objMarketerDetail.Address= Convert.ToInt32(ds.Tables[0].Rows[0][""]).ToString();
                    objMarketerDetail.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objMarketerDetail.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objMarketerDetail.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objMarketerDetail.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    objMarketerDetail.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    objMarketerDetail.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objMarketerDetail.TimezoneId = ds.Tables[0].Rows[0]["TimezoneId"].ToString();
                    objMarketerDetail.TimezoneOffset = ds.Tables[0].Rows[0]["TimezoneOffset"].ToString();
                    objMarketerDetail.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfix"].ToString();
                    objMarketerDetail.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"]);
                    Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["OfficeId"]), out officeId);
                    objMarketerDetail.OfficeId = officeId;
                    //objMarketerDetail.OfficeName = ds.Tables[0].Rows[0]["OfficeName"].ToString();
                    FillAllOffices(officeId > 0 ? (object)objMarketerDetail.OfficeId : null);
                    objMarketerDetail.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
                }


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "MarkettingController";
                log.Methodname = "[HttpGet] EditmarketingDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objMarketerDetail);
        }


        
        [HttpPost]
        public ActionResult EditmarketingDetail(MarketerDetailsModel MarketingsDetail)
        {
            string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
             MembershipUser mUser = Membership.GetUser(MarketingsDetail.UserName);
            mUser.ChangePassword(mUser.ResetPassword(), MarketingsDetail.Password);

            
            int i = 0;
            try
            {
                int TimezoneOffset = Convert.ToInt32(MarketingsDetail.TimezoneOffset);
                i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateMarketerDetails",
                                                  MarketingsDetail.MarketersId,
                                                  MarketingsDetail.MarketersName,
                                                  MarketingsDetail.PhoneNo,
                                                  MarketingsDetail.Email,
                                                  MarketingsDetail.Latitude,
                                                  MarketingsDetail.Longitude,
                                                  MarketingsDetail.ZipCode,
                                                  Guid.Parse(InsertedUserID),
                                                  MarketingsDetail.TimezoneId,
                                                  TimezoneOffset,
                                                  MarketingsDetail.TimezonePostfix,
                                                  MarketingsDetail.City,
                                                  MarketingsDetail.State,
                                                  MarketingsDetail.Street,
                                                  MarketingsDetail.OfficeId,
                                                  MarketingsDetail.UserName,
                                                  MarketingsDetail.Password
                                                  );


                if (i > 0)
                {
                    TempData["Message"] = "Marketers Details is updated successfully.";

                }
                else
                {

                    TempData["Message"] = "Marketers Details is not updated ";

                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "MarkettingController";
                log.Methodname = "[HttpPost] EditmarketingDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return RedirectToAction("Marketings", "Marketings");
        }

        public string DeleteMarketingDetail(string MarketerId)
        {
            string result = "";
            try
            {
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteMarketerDetail",
                                                     MarketerId,
                                                     new Guid(InsertedUserID));

                if (i > 0)
                {
                    TempData["Message"] = "Marketer deleted successfully.";
                    result = "Success";
                }
                else
                {
                    TempData["Message"] = "Marketer deletion failed.";
                    result = "Failed";
                }

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "MarketingsController";
                log.Methodname = "DeleteMarketingDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        private void UpdateLatLong(string CRNumber, string Lattitude, string Longitude,string TimeZoneId,string TimeZoneName, string TimeZoneOffset)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateClientLatLong", CRNumber, Lattitude, Longitude,TimeZoneId,TimeZoneName,TimeZoneOffset);

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

        private void UpdateLatLongByPhoneNo(string phone, string Lattitude, string Longitude, string TimeZoneId, string TimeZoneName, string TimeZoneOffset)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateClientLatLongByPhone", phone, Lattitude, Longitude, TimeZoneId, TimeZoneName, TimeZoneOffset);

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

    }
}