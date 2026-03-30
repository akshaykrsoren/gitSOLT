using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using DifferenzLibrary;
using iTextSharp.text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CaregiverLite.Controllers
{

    [SessionExpire]
    public class ClientController : Controller
    {

        public ActionResult Index()
        {
           FillAllOffices();
            return View();
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
       
        public ActionResult GetClientDetailsList(JQueryDataTableParamModel param)
        {
            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            ClientDetailsList ClientDetailsList = new ClientDetailsList();
            try
            {
                int FilterOfficeId = 0;

                if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                string sortOrder = string.Empty;
                 
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                
                var sortDirection = Request["sSortDir_0"]; // asc or desc


                if (sortColumnIndex == 1)
                {
                    sortOrder = "ClientName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "CRNumber";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "Street";
                }
                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "State";
                //}
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "Phone";

                }
                else if (sortColumnIndex == 5)
                {
                    sortOrder = "ZipCode";
                }
                else if (sortColumnIndex == 6)
                {
                    sortOrder = "OfficeName";
                }
                else
                {
                    sortOrder = "InsertDateTime";
                    sortDirection = "desc";
                }


                //if (sortColumnIndex == 1)
                //{
                //    sortOrder = "ClientName";
                //}
                //else if (sortColumnIndex == 2)
                //{
                //    sortOrder = "CRNumber";
                //}
                //else if (sortColumnIndex == 3)
                //{
                //    sortOrder = "Email";
                //}
                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "State";
                //}
                //else if (sortColumnIndex == 5)
                //{
                //    sortOrder = "Phones";

                //}
                //else if (sortColumnIndex == 6)
                //{
                //    sortOrder = "ZipCode";
                //}
                //else  
                //{
                //    sortOrder = "InsertDateTime";
                //    sortDirection = "desc";
                //}                


                string search = "||"; //It's indicate blank filter

                if (!string.IsNullOrEmpty(param.sSearch))
                    search = param.sSearch;
 
                int pageNo = 1;
                int recordPerPage = param.iDisplayLength;

                //Find page number from the logic
                if (param.iDisplayStart > 0)
                {
                    pageNo = (param.iDisplayStart / recordPerPage) + 1;
                }

              //  PatientDetailsServiceProxy PatientDetailLiteService = new PatientDetailsServiceProxy();
                ClientDetailsList = DBGetClientDetailsList(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId);


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "Client_Page";
                log.Methodname = "GetClientDetailsList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (ClientDetailsList.ClientList != null)
            {
                var result = from C in ClientDetailsList.ClientList select new[] { C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ClientDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ClientDetailsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ClientDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ClientDetailsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Client
        public ClientDetailsList DBGetClientDetailsList(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, int OfficeId)
        {
            ClientDetailsList objClientDetails = new ClientDetailsList(); 
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllClientDetails",
                                                        LogInUserId,
                                                        pageno,
                                                        recordperpage,
                                                        sortfield,
                                                        sortOrder,
                                                        OfficeId,
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Client> ClientList = new List<Client>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        Client objClientDetail = new Client();
                        objClientDetail.ClientId= Convert.ToInt32(ds.Tables[0].Rows[i]["Clientid"]);
                        objClientDetail.CRNumber = ds.Tables[0].Rows[i]["CRNumber"].ToString();
                        objClientDetail.ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString();
                       // objClientDetail.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        objClientDetail.Phone = ds.Tables[0].Rows[i]["Phone"].ToString();
                        objClientDetail.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objClientDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objClientDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();                        
                        objClientDetail.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objClientDetail.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        int officeId = 0;
                        Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["OfficeId"]), out officeId);
                        objClientDetail.OfficeId = officeId.ToString();
                        objClientDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();                                           
                        objClientDetail.Address = ds.Tables[0].Rows[i]["Street"].ToString() +" ,"+ ds.Tables[0].Rows[i]["City"].ToString();
                        ClientList.Add(objClientDetail);
                    }
                    objClientDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    objClientDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    objClientDetails.ClientList = ClientList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "ClientPage";
                objErrorlog.Methodname = "GetClientDetailsList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string resError = ErrorLogService.InsertErrorLog(objErrorlog).Result;
            }
            return objClientDetails;
        }


        public JsonResult IsCRNumberExist(string CRNumber)
        {
            var ClientDetail = new Client();
            string InsertUserId = "";

            bool result = true;
            try
            { 
                InsertUserId = Membership.GetUser().ProviderUserKey.ToString();
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "IsClientCRNumberExist", CRNumber); 
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    result = false;//not Exist
                }
                else
                {
                    result = true;//yes Exist
                } 
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ClientController";
                log.Methodname = "IsCRNumberExist";
                log.UserID = InsertUserId;
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            //return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }



        public JsonResult IsPhoneNoExists(string PhoneNo)
        {
            var ClientDetail = new Client();
            string InsertUserId = "";

            bool result = true;
            try
            {
                
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "IsPhoneNumberExist", PhoneNo);
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    result = false;//not Exist
                }
                else
                {
                    result = true;//yes Exist
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ClientController";
                log.Methodname = "IsPhoneNoExist";
                log.UserID = InsertUserId;
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            //return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddClient()
        {
            IEnumerable<SelectListItem> OfficeSelectList = null;
            Client objClient = new Client();
            try
            {
                FillAllOffices();
                objClient.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
                
            }
            catch (Exception ex)
            { 
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "AddClient";
                log.Methodname = "[HttpGet] AddClient";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string resError = ErrorLogService.InsertErrorLog(log).Result;
            }   
            return PartialView(objClient);
        }

        [HttpPost]
        public ActionResult AddClient(Client client)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            int i = 0;
           // string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
            try
            {
                if (ModelState.IsValid)
                {
                  // MembershipCreateStatus status;
                  // Membership.CreateUser(client.UserName, client.Password, client.Email, null, null, true, out status);


                   // Roles.AddUserToRole(client.UserName, "Client");
                  //  client.UserId = Membership.GetUser(client.UserName).ProviderUserKey.ToString();
                  //  client.InsertUserId = client.UserId;


                        //if (client.ProfileImage != "" && client.ProfileImage != null)
                        //{
                        //    int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        //    Image porfilelogoimg = Base64ToImage(client.ProfileImage);
                        //    client.ProfileImage = "Client_" + client.UserId + datetime.ToString() + ".jpeg";
                        //    porfilelogoimg.Save(ProfileImageUploadPath + client.ProfileImage, ImageFormat.Jpeg);
                        //}

                    i = DBInsertClientData(client);
                        if (i > 0)
                        {
                            TempData["Message"] = "Client Details is Added successfully.";
                            return RedirectToAction("Index", "Client");
                        //return RedirectToAction("Index", "Client", new { IsAdded = true });
                    }
                        else
                        {
                            TempData["error"] = true;
                            TempData["Message"] = "Failed to add client"; 
                            return RedirectToAction("Index", "Client");
                           
                        }

                    //if (i > 0)
                    //{
                    //    res["Success"] = true;
                    //    res["Message"] = "Success";

                    //}
                    //else
                    //{
                    //    res["Success"] = false;
                    //    res["Message"] = "Failed";
                    //    Membership.DeleteUser(client.UserName);

                    //}
                    //  }
                    //else if (status == MembershipCreateStatus.DuplicateUserName)
                    //{
                    //    throw new Exception("UserName is already exists");
                    //}
                    //else if (status == MembershipCreateStatus.DuplicateEmail)
                    //{
                    //    throw new Exception("Email is already exists");
                    //}
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
                res["Success"] = false;
                res["Message"] = ex.Message;
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "AddClient";
                log.Methodname = "[HttpPost] AddClient";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string resError = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("Index", "Client", new { IsAdded = false });
            }
        }

        private int DBInsertClientData(Client client)
        {

            int i = 0;
            int TimezoneOffset = Convert.ToInt32(client.TimezoneOffset);
            try
            {
                i = DataAccess.ExecuteNonQuery(Settings.CaregiverLiteDatabase().ToString(), "InsertClientData",                                         
                                           client.CRNumber,
                                           client.ClientName,
                                           client.Email,
                                           //client.UserName,
                                           //client.Password,
                                           client.Phone,                                           
                                           client.ZipCode,
                                           client.CompanyName,
                                           client.Latitude,
                                           client.Longitude,
                                           //new Guid(client.InsertUserId),                                          
                                           //client.Remarks,
                                           client.IsDeleted,
                                          // client.ProfileImage,
                                            client.OfficeId,
                                           client.TimezoneId,
                                           TimezoneOffset,
                                           client.TimezonePostfix,
                                           client.City,
                                           client.State,
                                           client.Street
                                         );

            }
            catch (Exception ex)
            { 
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "AddClient";
                log.Methodname = "DBInsertClientData";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string resError = ErrorLogService.InsertErrorLog(log).Result;
            }
            return i;
        }

        [HttpGet]
        public ActionResult EditClientDetail(string id)
        {
            Client objClientDetail = new Client();
            try
            {
                int officeId = 0;
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientDetailById", id);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objClientDetail.ClientId = Convert.ToInt32(ds.Tables[0].Rows[0]["Clientid"]);
                    objClientDetail.CRNumber = ds.Tables[0].Rows[0]["CRNumber"].ToString();
                    objClientDetail.ClientName = ds.Tables[0].Rows[0]["ClientName"].ToString();
                    objClientDetail.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                    objClientDetail.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    objClientDetail.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    objClientDetail.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    objClientDetail.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objClientDetail.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();

                    objClientDetail.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objClientDetail.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objClientDetail.State = ds.Tables[0].Rows[0]["State"].ToString();
                    

                    Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["OfficeId"]), out officeId);
                    objClientDetail.OfficeId = officeId.ToString();
                    //objClientDetail.OfficeName = ds.Tables[0].Rows[0]["OfficeName"].ToString();
                    FillAllOffices(officeId > 0 ? (object)objClientDetail.OfficeId : null);
                    objClientDetail.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
                }


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ClientController";
                log.Methodname = "[HttpGet] EditClientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objClientDetail);
        }


        [HttpPost]
        public ActionResult EditClientDetail(Client objClient)
        {
           string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

            try
            {
                int TimezoneOffset = Convert.ToInt32(objClient.TimezoneOffset);

                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateClientDetails",
                                                   objClient.ClientId,
                                                   objClient.CRNumber,
                                                   objClient.ClientName,
                                                   objClient.Email,                             
                                                   objClient.Phone,
                                                   objClient.ZipCode,
                                                   objClient.CompanyName,
                                                   objClient.Latitude,
                                                   objClient.Longitude,
                                                   new Guid(InsertedUserID),
                                                   objClient.Remarks,
                                                   objClient.IsDeleted,
                                                   objClient.ProfileImage,
                                                   objClient.OfficeId,
                                                   objClient.TimezoneId,
                                                   TimezoneOffset,
                                                   objClient.TimezonePostfix,
                                                   objClient.City,
                                                   objClient.State,
                                                   objClient.Street
                                                  );

                
                if (i > 0)
                {
                    TempData["Message"] = "Client Details is updated successfully.";
                    return RedirectToAction("Index", "Client", new { IsAdded = true });
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;

                log.StackTrace = e.StackTrace;
                log.Pagename = "ClientsController";
                log.Methodname = "[HttpPost] EditPatientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public JsonResult GetTopCRNumberByOfficeId(int officeId)
        {
            string Result = "";

            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTopCRNumberByOfficeId", officeId);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Result = ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                Result = "";
            }

                return Json(Result,JsonRequestBehavior.AllowGet);
        }


        public string DeleteClientDetail(string ClientId)
        {
            string result = "";
            try
            {
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteClientDetail",
                                                     ClientId,
                                                     new Guid(InsertedUserID)   );

                if (i > 0)
                {
                    TempData["Message"] = "Client deleted successfully.";
                    result = "Success";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ClientController";
                log.Methodname = "DeleteClientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        [HttpGet]
        public ActionResult ClientRegister()
        {
            return PartialView("ClientRegister");
        }


        [HttpPost]
        public ActionResult UploadClientData()
        {
          
            string x = "", myStringWebResource="", RemoteUri = "";

            string filePath = string.Empty;

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
                        RemoteUri = System.Configuration.ConfigurationManager.AppSettings["ClientDownLoadFile"].ToString();
                        string path = Server.MapPath("~/MarketersReportPath/ClientProfileExcel/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filePath);

                        string conString = string.Empty;

                        switch (extension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                                break;
                        }

                        DataTable dt = new DataTable();
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

                                    connExcel.Close();

                                }
                            }
                        }

                       // DataTable dt2 = dt.Copy();

                        DataTable dataset = new DataTable();
                        
                        try
                        {
                            if (!dt.Columns.Contains("Status"))
                            {
                                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json("", JsonRequestBehavior.AllowGet);
                        }

                        #region 
                        //creating excel file in edit in existing file
                        string PathExtension = filePath;

                        FileInfo fileInfo = new FileInfo(PathExtension);
                       
                        ExcelPackage package = new ExcelPackage(fileInfo);

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                   
                      //  ExcelWorksheet worksheet1 = package.Workbook.Worksheets.FirstOrDefault();

                        // get number of rows in the sheet
                        int rows = worksheet.Dimension.Rows; // 10     
                        int column = worksheet.Dimension.Columns + 1;  

                        int y = 1;
                        worksheet.Cells[y, column].Value = "Status";
                        while (y == 1)
                        {
                            worksheet.Cells[1, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[1, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue);
                            worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[1, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                            //   worksheet.Column(9).Style.Font.Bold = true;
                            package.Save();
                            y++;
                        }

                      ///  string RowNeedsToBeDeleted = "";
                      //  string concatstr = "";
                        int RowCounts = dt.Rows.Count;
                        for (int j = 0; j < rows - 1; j++)
                        {
                            string phoneno = dt.Rows[j]["Phone"].ToString();
                            JsonResult xyz = IsPhoneNoExists(phoneno);
                            if (Convert.ToBoolean(xyz.Data))
                            {
                                //  RowNeedsToBeDeleted += j + ",";

                                dt.Rows[j]["status"] = "Y";

                                worksheet.Cells[y, column].Value = "Duplicate Phone Number";
                                worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                            }
                            else
                            {
                                {
                                    //  RowNeedsToBeDeleted += j + ",";

                                   // dt.Rows[j]["status"] = "N";

                                    worksheet.Cells[y, column].Value = "Success";
                                    worksheet.Cells[y, column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[y, column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                                    worksheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[y, column].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                }
                            }
                            y++;
                            
                            //  worksheet.Column(9).Style.Font.Bold = true;    
                            package.Save();
                        }

                     //   RowNeedsToBeDeleted = RowNeedsToBeDeleted.Substring(0, RowNeedsToBeDeleted.Length - 1);
                      //  string[] RowdeltedArray = RowNeedsToBeDeleted.Split(',');

                        int k = 0;
                        int counter = dt.Rows.Count-1;
                        while (k <= counter)
                        {
                                  if (dt.Rows[counter]["Status"].ToString() == "Y")
                                    {
                                        dt.Rows.RemoveAt(counter);      
                                    }
                          counter--;
                        }


                        // DataTable dt1 = dt.Copy();

                      dt.Columns.Remove("Status");

                      if (dt.Rows.Count > 0)
                      {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                        {

                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                {
                                       
                                    #region

                                    // DataTable myTable;
                                    DataRow myNewRow;
                                    // Create a new DataTable.
                                    //  myTable = new DataTable("My Table");
                                    //ADDING DATETIME COLUMN
                                    DataColumn colDateTime = new DataColumn("InsertDateTime");

                                    DataColumn colDateTime1 = new DataColumn("Latitude");
                                    DataColumn colDateTime2 = new DataColumn("Longitude");
                                    DataColumn colDateTime3 = new DataColumn("TimezoneId");
                                    DataColumn colDateTime4 = new DataColumn("TimezoneOffset");
                                    DataColumn colDateTime5 = new DataColumn("TimezonePostfix");
                                    DataColumn colDateTimes6 = new DataColumn("IsDeleted");

                                    dt.Columns.Add(colDateTime);
                                    dt.Columns.Add(colDateTime1);
                                    dt.Columns.Add(colDateTime2);
                                    dt.Columns.Add(colDateTime3);
                                    dt.Columns.Add(colDateTime4);
                                    dt.Columns.Add(colDateTime5);
                                    dt.Columns.Add(colDateTimes6);

                                    int RowCount = dt.Rows.Count;

                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {

                                        string Address = dt.Rows[j]["Street"].ToString() + " " + dt.Rows[j]["City"].ToString() + " " + dt.Rows[j]["State"].ToString();
                                        string ZipCode = dt.Rows[j]["ZipCode"].ToString();

                                        string FullAddress = Address + "," + ZipCode;

                                        var requestUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address=key{0}&key={1}", FullAddress, "AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
                                        using (var client = new WebClient())
                                        {
                                            var result1 = client.DownloadString(requestUrl);
                                            var data1 = JsonConvert.DeserializeObject<JObject>(result1);

                                            var Latitude = data1["results"][0]["geometry"]["location"]["lat"];
                                            var Longitude = data1["results"][0]["geometry"]["location"]["lng"];

                                            Latitude = Convert.ToString(Latitude);
                                            Longitude = Convert.ToString(Longitude);

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

                                            dt.Rows[j]["InsertDateTime"] = System.DateTime.Now;

                                            dt.Rows[j]["Latitude"] = Latitude;
                                            dt.Rows[j]["Longitude"] = Longitude;
                                            dt.Rows[j]["TimezoneId"] = obj.timeZoneId;
                                            dt.Rows[j]["TimezoneOffset"] = obj.rawOffset;
                                            dt.Rows[j]["TimezonePostfix"] = obj.timeZoneName;
                                            dt.Rows[j]["IsDeleted"] = false;
                                        }
                                    }

                                    sqlBulkCopy.DestinationTableName = "dbo.Client";

                                    sqlBulkCopy.ColumnMappings.Add("ClientName", "ClientName");
                                    sqlBulkCopy.ColumnMappings.Add("CRNumber", "CRNumber");
                                    sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                                    sqlBulkCopy.ColumnMappings.Add("Street", "Street");
                                    sqlBulkCopy.ColumnMappings.Add("City", "City");
                                    sqlBulkCopy.ColumnMappings.Add("State", "State");
                                    sqlBulkCopy.ColumnMappings.Add("ZipCode", "ZipCode");
                                    sqlBulkCopy.ColumnMappings.Add("Phone", "Phone");
                                    sqlBulkCopy.ColumnMappings.Add("OfficeId", "OfficeId");
                                    sqlBulkCopy.ColumnMappings.Add("CompanyName", "CompanyName");
                                    sqlBulkCopy.ColumnMappings.Add("InsertDateTime", "InsertDateTime");

                                    sqlBulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                                    sqlBulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                                    sqlBulkCopy.ColumnMappings.Add("TimezoneId", "TimezoneId");
                                    sqlBulkCopy.ColumnMappings.Add("TimezoneOffset", "TimezoneOffset");
                                    sqlBulkCopy.ColumnMappings.Add("TimezonePostfix", "TimezonePostfix");
                                    sqlBulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");

                                    #endregion


                                    con.Open();
                                    try
                                    {
                                        sqlBulkCopy.WriteToServer(dt);
                                        myStringWebResource = RemoteUri + Path.GetFileName(file.FileName);
                                        return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw;
                                    }
                                    con.Close();
                                }
                            }
                        }

                        myStringWebResource = RemoteUri + Path.GetFileName(file.FileName);
                        return Json(myStringWebResource, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            
            return Json(myStringWebResource, JsonRequestBehavior.AllowGet);
        }



        #endregion


    }
}