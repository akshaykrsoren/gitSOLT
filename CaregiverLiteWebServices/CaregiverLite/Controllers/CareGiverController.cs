using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLite.Models.Utility;
using CaregiverLiteWCF;
using DifferenzLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class CareGiverController : Controller
    {
        //
        // GET: /CareGiver/

        [HttpGet]
        public ActionResult CareGiver()
        {
            FillAllOffices();
            FillAllOrganisations();
            return View();
        }


        public ActionResult GetCareGiverList(JQueryDataTableParamModel param)
        {
            CareGiversList CareGiverList = new CareGiversList();

            string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();

            try
            {

                string IsActiveStatus = "";

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                // its for filter IsApprove 
                int IsApprove = 2;
                int FilterOffice = 0;
                int.TryParse(Request.Params["ddFilterStatus"], out IsApprove);

                //int.TryParse(Request.Params["ddFilterOffice"], out IsApprove);

                if (Convert.ToInt32(Request["FilterOffice"]) != 0) //Request["FilterOffice"] != null && Request["FilterOffice"] != "" &&
                {
                    FilterOffice = Convert.ToInt32(Request["FilterOffice"]);

                    if (FilterOffice == 0)//if (FilterOffice == "All")
                    {
                        FilterOffice = 0; //FilterOffice = "||";
                    }
                }

                //if (Request["FilterActiveStatus"] != null && Request["FilterActiveStatus"] != "")
                //{
                //    IsActiveStatus = Request["FilterActiveStatus"];
                //}


                if (sortColumnIndex == 0)
                {
                    sortOrder = "FirstName";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "FirstName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "LastName";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "UserName";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "Email";
                }
                else if (sortColumnIndex == 5)
                {
                    sortOrder = "Phone";
                }
                else if (sortColumnIndex == 6)
                {
                    sortOrder = "ZipCode";
                }
                else if (sortColumnIndex == 7)
                {
                    sortOrder = "Office";
                }
                else if (sortColumnIndex == 8)
                {
                    sortOrder = "PayrollId";
                }
                else if (sortColumnIndex == 9)
                {
                    sortOrder = "InsertDateTime";
                }

                //if (sortColumnIndex == 0)
                //{
                //    sortOrder = "FirstName";
                //}
                //else if (sortColumnIndex == 1)
                //{
                //    sortOrder = "LastName";
                //}
                //else if (sortColumnIndex == 3)
                //{
                //    sortOrder = "UserName";
                //}
                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "Email";
                //}
                //else if (sortColumnIndex == 5)
                //{
                //    sortOrder = "Phone";
                //}
                //else if (sortColumnIndex == 6)
                //{
                //    sortOrder = "ZipCode";
                //}

                //else if (sortColumnIndex == 7)
                //{
                //    sortOrder = "Office";
                //}
                //else if (sortColumnIndex == 8)
                //{
                //    sortOrder = "PayrollId";
                //}
                //else if(sortColumnIndex == 9)
                //{
                //    sortOrder = "InsertDateTime";
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

                //   int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                //CareGiverList = CareGiverLiteService.GetAllCareGivers(LoginUserId, pageNo, recordPerPage, IsApprove, search, sortOrder, sortDirection, FilterOffice, OrganisationId, IsActiveStatus).Result;

                CareGiverList = CareGiverLiteService.GetAllCareGivers(LoginUserId, pageNo, recordPerPage, IsApprove, search, sortOrder, sortDirection, FilterOffice, OrganisationId).Result;
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
            if (CareGiverList.CareGiverList != null)
            {
                var result = from C in CareGiverList.CareGiverList select new[] { C, C, C, C, C, C, C, C, C, C, C, C, C, C, C };
                var jsonResult = Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = CareGiverList.TotalNumberofRecord,
                    iTotalDisplayRecords = CareGiverList.FilteredRecord,
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
                    iTotalRecords = CareGiverList.TotalNumberofRecord,
                    iTotalDisplayRecords = CareGiverList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }
        }


        [Authorize(Roles = "SuperAdmin,Admin,OrgSuperAdmin")]
        public string DeleteNurse(string NurseId, string Username)
        {
            string result = "";

            //MembershipUserCollection user = Membership.DeleteUser(); 
            try
            {
                MembershipUser mUser = Membership.GetUser(Username);
                string CareGiverUserId = mUser.ProviderUserKey.ToString();

                Membership.DeleteUser(Username, true);

                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                result = CareGiverLiteService.DeleteNurse(NurseId, Membership.GetUser().ProviderUserKey.ToString()).Result;

                if (result == "Success")
                {

                    var QuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(CareGiverUserId).Result;

                    ChattingController ChattingController = new ChattingController();

                    // for delete user one to one chat dialog
                    ChattingController.DeleteOneToOneChatByUserId(CareGiverUserId);

                    // for delete user from quickbox
                    Task.Run(async () => { await ChattingController.DeleteUserFromQuickBloxRestAPI(QuickBloxId, mUser.Email); }).Wait();
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "DeleteNurse";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        [HttpPost]
        public string GetNurseName(string id)
        {
            string result = "";
            try
            {
                CareGiverModel objCareGiver = new CareGiverModel();
                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                CareGivers CareGiver = CareGiverLiteService.GetAllCareGiverByNurseId(id).Result;
                result = CareGiver.Name;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "DeleteNurse";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        [HttpPost]
        public JsonResult GetCareGiverDetailsByUserId(string UserId)
        {
            CareGivers CareGiverList = new CareGivers();
            try
            {
                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                CareGiverList = CareGiverLiteService.GetCareGiverDetailsByUserId(UserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetCareGiverDetailsByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(CareGiverList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCareGiver(string id)
        {
            //return PartialView(GetCareGiverDetailById(id));

            CareGiverModel objModel = new CareGiverModel();
            try
            {
                objModel = GetCareGiverDetailById(id);
                List<SelectListItem> li = new List<SelectListItem>();
                for (int i = 5; i <= 75; i += 5)
                {
                    if (i == Convert.ToInt32(objModel.ServiceRadius))
                    {
                        li.Add(new SelectListItem { Selected = true, Text = i + " Miles", Value = i.ToString() });
                    }
                    else
                    {
                        li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
                    }
                }

                ViewData["Radious"] = li;
                int k = objModel.HoursRate.Count();
                string rate = "";
                if (k > 6)
                {
                    rate = String.Format("{0:0.0}", objModel.HoursRate);
                    objModel.HoursRate = Convert.ToDouble(rate).ToString("0.0");
                }


                int OrganisationId = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(objModel.OrganisationId)))
                {

                    OrganisationId = objModel.OrganisationId;
                }
                else
                {
                    OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                }
                 
                ////Office Dropdownlist
                List<Office> OfficeList = new List<Office>();
                OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(), OrganisationId.ToString()).Result;
                SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");
                ViewBag.lstOffice = officeSelectList;

                //  FillAllOffices(objModel.OfficeId > 0 ? (object)objModel.OfficeId : null);
                objModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;

                //FillAllOffices();

                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "EditCareGiver";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);

        }

        public CareGiverModel GetCareGiverDetailById(string id)
        {
            CareGiverModel objCareGiver = new CareGiverModel();

            try
            {
                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                CareGivers CareGiver = CareGiverLiteService.GetAllCareGiverByNurseId(id).Result;
                objCareGiver.NurseId = CareGiver.NurseId;
                objCareGiver.Name = CareGiver.Name;
                objCareGiver.FirstName = CareGiver.FirstName;
                objCareGiver.LastName = CareGiver.LastName;
                //objCareGiver.NurseFullName = CareGiver.NurseFullName;
                objCareGiver.IsActive = CareGiver.IsActive;
                objCareGiver.Services = CareGiver.ServiceId;
                objCareGiver.ServiceNames = CareGiver.ServiceName;
                objCareGiver.OrganisationId = CareGiver.OrganisationId;

                objCareGiver.Email = CareGiver.Email;
                objCareGiver.OldEmail = CareGiver.Email;

                objCareGiver.Username = CareGiver.UserName;
                objCareGiver.Address = CareGiver.Address;
                objCareGiver.Street = CareGiver.Street;
                objCareGiver.City = CareGiver.City;
                objCareGiver.State = CareGiver.State;
                objCareGiver.ZipCode = CareGiver.ZipCode;
                objCareGiver.Office = CareGiver.Office;
                objCareGiver.IsAllowOneToOneChat = CareGiver.IsAllowOneToOneChat;
                objCareGiver.IsAllowPatientChatRoom = CareGiver.IsAllowPatientChatRoom;
                objCareGiver.IsAllowGroupChat = CareGiver.IsAllowGroupChat;
                objCareGiver.IsAllowToCreateGroupChat = CareGiver.IsAllowToCreateGroupChat;
                objCareGiver.IsApprove = CareGiver.IsApprove;

                if (!string.IsNullOrEmpty(CareGiver.ProfileImage))
                {
                    objCareGiver.ProfileImage = CareGiver.ProfileImage;
                }
                else
                {
                    objCareGiver.ProfileImage = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                }

                objCareGiver.ProfileVideo = CareGiver.ProfileVideo;
                objCareGiver.ProfileVideoThumbnil = CareGiver.ProfileVideoThumbnil;
                objCareGiver.HoursRate = CareGiver.HourlyRate.ToString();
                objCareGiver.Phone = CareGiver.Phone;
                //objCareGiver.Password = CareGiver.Password;
                objCareGiver.UserId = CareGiver.UserId;
                objCareGiver.ServiceRadius = CareGiver.DistanceFromLocation.ToString();
                objCareGiver.DistanceUnit = CareGiver.DistanceUnit;

                //objCareGiver.DeviceToken = CareGiver.DeviceToken;
                //objCareGiver.DeviceType = CareGiver.DeviceType;
                //objCareGiver.ChargeToPatient = CareGiver.ChargeToPatient.ToString();

                objCareGiver.Education = CareGiver.Education;
                objCareGiver.CanAdminEdit = CareGiver.CanAdminEdit;
                objCareGiver.Timezone = CareGiver.TimezoneId;
                objCareGiver.OfficeId = CareGiver.OfficeId;

                objCareGiver.OldOfficeId = CareGiver.OfficeId; // for check when Caregiver data edited that time office change or not

                objCareGiver.QuickBloxId = CareGiver.QuickBloxId;
                objCareGiver.PayrollId = CareGiver.PayrollId;

                // objCareGiver.OldOfficeChatGroupId = CareGiver.OldOfficeChatGroupId;

                List<CareGivers> CareCertificate = CareGiverLiteService.GetCareGiverCertiByNurseId(id).Result;
                objCareGiver.Certificate = new List<string>();
                foreach (var item in CareCertificate)
                {
                    objCareGiver.Certificate.Add(item.Certificate);
                }

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetCareGiverDetailById";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objCareGiver;
        }


        public JsonResult IsUserNameExists(string Username, string UserId)
        {
            bool result = true;
            try
            {
                MembershipUserCollection user = Membership.FindUsersByName(Username);

                if (user != null && user.Count != 0)
                {
                    if (user[Username].ProviderUserKey.ToString() != UserId)
                    {
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "IsUserNameExists";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExists(string Email, string UserId)
        {
            bool result = true;
            try
            {
                MembershipUserCollection user = Membership.FindUsersByEmail(Email);

                if (user != null && user.Count != 0)
                {
                    if (user[Membership.GetUserNameByEmail(Email)].ProviderUserKey.ToString() != UserId)
                    {
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "IsEmailExists";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "SuperAdmin,Admin,OrgSuperAdmin")]
        public ActionResult AddCareGiver()
        {
            try
            {
                List<SelectListItem> li = new List<SelectListItem>();
                for (int i = 5; i <= 75; i += 5)
                {
                    li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
                }

                ViewData["Radious"] = li;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                //Office Dropdownlist
                List<Office> OfficeList = new List<Office>();
                //OfficeList = OfficeController.GetOfficeDropDownList().OfficeList;
                //ViewBag.OfficeDropDownList = new SelectList(OfficeList, "OfficeId", "OfficeName");

                OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(), OrganisationId.ToString()).Result;
                //OfficeList.Insert(0, new Office() { OfficeId = 0, OfficeName = "All" });
                SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");

                ViewBag.lstOffice = new SelectList(OfficeList, "OfficeId", "OfficeName");
                //ViewBag.lstOffice = officeSelectList as IEnumerable<SelectListItem>;

                ViewData["OfficeList"] = officeSelectList;

                // FillAllOffices();

                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "AddCareGiver";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }


        [HttpPost]
        public ActionResult AddCareGiver(CareGiverModel objCareGiverModel)
        {
            try
            {
                CareGiverServiceProxy CareGiverService = new CareGiverServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                if (true)
                {
                    if (objCareGiverModel.NurseId == 0)
                    {
                        MembershipCreateStatus status;
                        Membership.CreateUser(objCareGiverModel.Username, objCareGiverModel.Password, objCareGiverModel.Email, null, null, true, out status);

                        if (status == MembershipCreateStatus.Success)
                        {
                            Roles.AddUserToRole(objCareGiverModel.Username, "Nurse");
                        }
                    }

                    var CareGiver = new CareGivers();
                    CareGiver.NurseId = objCareGiverModel.NurseId;
                    CareGiver.UserId = Membership.GetUser(objCareGiverModel.Username).ProviderUserKey.ToString();
                    objCareGiverModel.Name = objCareGiverModel.FirstName + " " + objCareGiverModel.LastName;
                    CareGiver.Name = objCareGiverModel.Name;
                    CareGiver.FirstName = objCareGiverModel.FirstName;
                    CareGiver.LastName = objCareGiverModel.LastName;
                    CareGiver.Email = objCareGiverModel.Email;
                    CareGiver.ServiceId = objCareGiverModel.ServiceNames;
                    CareGiver.HourlyRate = Convert.ToDecimal(objCareGiverModel.HoursRate);
                    CareGiver.DistanceFromLocation = Convert.ToDecimal(objCareGiverModel.ServiceRadius);
                    CareGiver.Phone = objCareGiverModel.Phone;
                    CareGiver.Address = objCareGiverModel.Address;
                    CareGiver.Street = objCareGiverModel.Street;
                    CareGiver.City = objCareGiverModel.City;
                    CareGiver.State = objCareGiverModel.State;
                    CareGiver.ZipCode = objCareGiverModel.ZipCode;
                    CareGiver.ProfileImage = objCareGiverModel.ProfileImage;
                    CareGiver.Latitude = objCareGiverModel.Latitude;
                    CareGiver.Longitude = objCareGiverModel.Longitude;
                    CareGiver.DistanceUnit = "Miles";
                    CareGiver.InsertUserId = InsertedUserID;
                    CareGiver.UserName = objCareGiverModel.Username;
                    CareGiver.Password = objCareGiverModel.Password;
                    CareGiver.TimezoneId = objCareGiverModel.TimezoneId;
                    CareGiver.TimezoneOffset = objCareGiverModel.TimezoneOffset;
                    CareGiver.TimezonePostfix = objCareGiverModel.TimezonePostfix;
                    CareGiver.ChargeToPatient = Convert.ToDecimal(objCareGiverModel.ChargeToPatient);
                    CareGiver.Education = objCareGiverModel.Education;
                    CareGiver.OfficeId = objCareGiverModel.OfficeId;
                    CareGiver.PayrollId = objCareGiverModel.PayrollId;
                    CareGiver.QuickBloxId = objCareGiverModel.QuickBloxId;
                    CareGiver.IsAllowOneToOneChat = objCareGiverModel.IsAllowOneToOneChat;
                    CareGiver.IsAllowPatientChatRoom = objCareGiverModel.IsAllowPatientChatRoom;
                    CareGiver.IsAllowGroupChat = objCareGiverModel.IsAllowGroupChat;
                    CareGiver.IsAllowToCreateGroupChat = objCareGiverModel.IsAllowToCreateGroupChat;
                    CareGiver.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                    string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                    if (objCareGiverModel.ProfileImageFile != null && objCareGiverModel.ProfileImageFile.ContentLength > 0)
                    {
                        int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        CareGiver.ProfileImage = UploadFile.getUploadFile_and_getFileURL(objCareGiverModel.ProfileImageFile, ConfigurationManager.AppSettings["NurseProfileImagePath"].ToString(), "CareGiver_" + CareGiver.UserId + datetime.ToString() + ".jpeg");
                    }
                    else
                    {
                        CareGiver.ProfileImage = !string.IsNullOrEmpty(objCareGiverModel.ProfileImage) ? objCareGiverModel.ProfileImage.Split('/')[objCareGiverModel.ProfileImage.Split('/').Length - 1] : "";
                    }

                    if (objCareGiverModel.ProfileVideoFile != null && objCareGiverModel.ProfileVideoFile.ContentLength > 0)
                    {
                        int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        CareGiver.ProfileVideo = UploadFile.getUploadFile_and_getFileURL(objCareGiverModel.ProfileVideoFile, ConfigurationManager.AppSettings["NurseProfileImagePath"].ToString(), "CareGiver_Video_" + CareGiver.UserId + datetime.ToString() + ".mp4");

                        var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                        ffMpeg.GetVideoThumbnail(ConfigurationManager.AppSettings["NurseProfileImagePath"] + "\\CareGiver_Video_" + CareGiver.UserId + datetime.ToString() + ".mp4", ConfigurationManager.AppSettings["NurseProfileImagePath"] + "\\CareGiver_VideoThumbnil_" + CareGiver.UserId + datetime.ToString() + ".jpeg", 5);
                        CareGiver.ProfileVideoThumbnil = "CareGiver_VideoThumbnil_" + CareGiver.UserId + datetime.ToString() + ".jpeg";
                    }
                    else
                    {
                        CareGiver.ProfileVideo = !string.IsNullOrEmpty(objCareGiverModel.ProfileVideo) ? objCareGiverModel.ProfileVideo.Split('/')[objCareGiverModel.ProfileVideo.Split('/').Length - 1] : "";
                        CareGiver.ProfileVideoThumbnil = !string.IsNullOrEmpty(objCareGiverModel.ProfileVideoThumbnil) ? objCareGiverModel.ProfileVideoThumbnil.Split('/')[objCareGiverModel.ProfileVideoThumbnil.Split('/').Length - 1] : "";
                    }

                    string result = CareGiverService.InsertUpdateCareGiverByAdmin(CareGiver).Result;

                    if (objCareGiverModel.NurseId == 0)
                    {
                        FormsAuthentication.SetAuthCookie(Membership.GetUser(new Guid(InsertedUserID)).UserName, false);

                        // return RedirectToAction("CareGiver", "CareGiver", new { IsAdded = false });
                    }

                    if (result == "Success")
                    {
                        ForgetPasswordServiceProxy ForgetPasswordService = new ForgetPasswordServiceProxy();
                        string result1 = ForgetPasswordService.EmailtoSetPassword(objCareGiverModel.Email).Result;

                        //   ForgetPasswordService.ForgotPassword(objCareGiverModel.Email).Result;

                        Task.Run(async () => { await ChattingController.GenerateUserQuickBloxIdRestAPI(CareGiver.UserId, CareGiver.Email, CareGiver.OfficeId, CareGiver.IsAllowGroupChat, CareGiver.OrganisationId, OrganisationEmail); }).Wait();

                        if (CareGiver.OrganisationId > 0 && CareGiver.OrganisationId == 3)
                        {
                            string results = GetAndPostEmployeeDataAllmedEditAndUpdate(CareGiver.UserId);
                            //GetAndPostEmployeeDataAllmed(CareGiver.UserId);
                        }
                        else
                        {
                            string SandDataOfficeId = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();
                            string[] OfficeIdSandData = SandDataOfficeId.Split(',');

                            if (Array.Exists(OfficeIdSandData, element => element == Convert.ToString(objCareGiverModel.OfficeId)))
                            {
                                string results = GetAndPostEmployeeData(objCareGiverModel.OfficeId, CareGiver.UserId);
                            }
                        }
                        return RedirectToAction("CareGiver", "CareGiver", new { IsAdded = true });
                    }
                }

                List<SelectListItem> li = new List<SelectListItem>();
                for (int i = 5; i <= 75; i += 5)
                {
                    li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
                }

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                ViewData["Radious"] = li;
                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;

                List<Office> OfficeList = new List<Office>();

                OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(), OrganisationId.ToString()).Result;

                SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");

                ViewBag.lstOffice = new SelectList(OfficeList, "OfficeId", "OfficeName");
                //ViewBag.lstOffice = officeSelectList as IEnumerable<SelectListItem>;

                ViewData["OfficeList"] = officeSelectList;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "[HttpPost] AddCareGiver";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("CareGiver", "CareGiver", new { IsAdded = false });
            }
            return PartialView();
        }


        [Authorize(Roles = "SuperAdmin,Admin,OrgSuperAdmin")]
        public ActionResult EditCareGiver(string id)
        {
            CareGiverModel objModel = new CareGiverModel();
            try
            {
                objModel = GetCareGiverDetailById(id);
                List<SelectListItem> li = new List<SelectListItem>();
                for (int i = 5; i <= 75; i += 5)
                {
                    if (i == Convert.ToInt32(objModel.ServiceRadius))
                    {
                        li.Add(new SelectListItem { Selected = true, Text = i + " Miles", Value = i.ToString() });
                    }
                    else
                    {
                        li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
                    }

                }
                ViewData["Radious"] = li;
                int k = objModel.HoursRate.Count();
                string rate = "";
                if (k > 6)
                {
                    rate = String.Format("{0:0.0}", objModel.HoursRate);
                    objModel.HoursRate = Convert.ToDouble(rate).ToString("0.0");
                }

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                ////Office Dropdownlist
                List<Office> OfficeList = new List<Office>();
                OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(), OrganisationId.ToString()).Result;
                SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");
                ViewBag.lstOffice = officeSelectList;

                //  FillAllOffices(objModel.OfficeId > 0 ? (object)objModel.OfficeId : null);
                objModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;

                //FillAllOffices();

                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "EditCareGiver";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }


        [HttpPost]
        public ActionResult EditCareGiver(CareGiverModel objCareGiverModel)
        {
            try
            {
                CareGiverServiceProxy CareGiverService = new CareGiverServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                string json = System.Web.HttpContext.Current.Request["ObjCareGiver"];
                objCareGiverModel = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverModelDetail;

                //string json = System.Web.HttpContext.Current.Request["ObjCareGiver"];
                //objCareGiverModel = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverModelDetail;

                string NurseFullName = System.Web.HttpContext.Current.Request["NurseFullName"];

                var CareGiver = new CareGivers();
                CareGiver.NurseId = objCareGiverModel.NurseId;
                CareGiver.UserId = Membership.GetUser(objCareGiverModel.Username).ProviderUserKey.ToString();
                objCareGiverModel.Name = objCareGiverModel.FirstName + " " + objCareGiverModel.LastName;
                CareGiver.Name = objCareGiverModel.Name;
                CareGiver.NurseFullName = NurseFullName;
                CareGiver.FirstName = objCareGiverModel.FirstName;
                CareGiver.LastName = objCareGiverModel.LastName;
                CareGiver.Email = objCareGiverModel.Email;
                CareGiver.ServiceId = objCareGiverModel.ServiceNames;
                CareGiver.HourlyRate = Convert.ToDecimal(objCareGiverModel.HoursRate);
                CareGiver.DistanceFromLocation = Convert.ToDecimal(objCareGiverModel.ServiceRadius);
                CareGiver.Phone = objCareGiverModel.Phone;
                CareGiver.Address = objCareGiverModel.Address;
                CareGiver.Street = objCareGiverModel.Street;
                CareGiver.City = objCareGiverModel.City;
                CareGiver.State = objCareGiverModel.State;
                CareGiver.ZipCode = objCareGiverModel.ZipCode;
                CareGiver.ProfileImage = objCareGiverModel.ProfileImage;
                CareGiver.Latitude = objCareGiverModel.Latitude;
                CareGiver.Longitude = objCareGiverModel.Longitude;
                CareGiver.DistanceUnit = "Miles";
                CareGiver.InsertUserId = InsertedUserID;
                CareGiver.UserName = objCareGiverModel.Username;

                //CareGiver.Password = objCareGiverModel.Password;

                CareGiver.ChargeToPatient = Convert.ToDecimal(objCareGiverModel.ChargeToPatient);
                CareGiver.Education = objCareGiverModel.Education;
                CareGiver.OfficeId = objCareGiverModel.OfficeId;
                CareGiver.IsAllowOneToOneChat = objCareGiverModel.IsAllowOneToOneChat;
                CareGiver.IsAllowPatientChatRoom = objCareGiverModel.IsAllowPatientChatRoom;
                CareGiver.IsAllowGroupChat = objCareGiverModel.IsAllowGroupChat;
                CareGiver.IsAllowToCreateGroupChat = objCareGiverModel.IsAllowToCreateGroupChat;
                CareGiver.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);
                CareGiver.PayrollId = objCareGiverModel.PayrollId;

                int temppayrolId = 0;
                if (!String.IsNullOrEmpty(objCareGiverModel.PayrollId))
                {
                    temppayrolId = Convert.ToInt32(objCareGiverModel.PayrollId);
                }

                CareGiver.PayrollId = Convert.ToString(temppayrolId);


                // insertdata(json+','+NurseFullName);     
                //MembershipUser mUser = Membership.GetUser(objCareGiverModel.Username);
                //mUser.ChangePassword(mUser.ResetPassword(), objCareGiverModel.Password);

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];

                    if (httpPostedFile != null)
                    {
                        int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        var fileSavePath = Path.Combine(ConfigurationManager.AppSettings["NurseProfileImagePath"].ToString(), "CareGiver_" + CareGiver.UserId + datetime.ToString() + ".jpeg");
                        httpPostedFile.SaveAs(fileSavePath);
                        CareGiver.ProfileImage = "CareGiver_" + CareGiver.UserId + datetime.ToString() + ".jpeg"; //UploadFile.getUploadFile_and_getFileURL(httpPostedFile, ConfigurationManager.AppSettings["NurseProfileImagePath"].ToString(), "CareGiver_" + CareGiver.UserId + datetime.ToString() + ".jpeg");
                    }
                }
                else
                {
                    if (CareGiver.ProfileImage != null)
                    {
                        // CareGiver.ProfileImage = objCareGiverModel.ProfileImage.Split('/')[objCareGiverModel.ProfileImage.Split('/').Length - 1];

                        CareGiver.ProfileImage = !string.IsNullOrEmpty(objCareGiverModel.ProfileImage) ? objCareGiverModel.ProfileImage.Split('/')[objCareGiverModel.ProfileImage.Split('/').Length - 1] : "";

                    }
                }
                if (objCareGiverModel.ProfileVideoFile != null && objCareGiverModel.ProfileVideoFile.ContentLength > 0)
                {
                    int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    CareGiver.ProfileVideo = UploadFile.getUploadFile_and_getFileURL(objCareGiverModel.ProfileVideoFile, ConfigurationManager.AppSettings["NurseProfileImagePath"].ToString(), "CareGiver_Video_" + CareGiver.UserId + datetime.ToString() + ".mp4");

                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                    ffMpeg.GetVideoThumbnail(ConfigurationManager.AppSettings["NurseProfileImagePath"] + "\\CareGiver_Video_" + CareGiver.UserId + datetime.ToString() + ".mp4", ConfigurationManager.AppSettings["NurseProfileImagePath"] + "\\CareGiver_VideoThumbnil_" + CareGiver.UserId + datetime.ToString() + ".jpeg", 5);
                    CareGiver.ProfileVideoThumbnil = "CareGiver_VideoThumbnil_" + CareGiver.UserId + datetime.ToString() + ".jpeg";
                }
                else
                {
                    CareGiver.ProfileVideo = !string.IsNullOrEmpty(objCareGiverModel.ProfileVideo) ? objCareGiverModel.ProfileVideo.Split('/')[objCareGiverModel.ProfileVideo.Split('/').Length - 1] : "";
                    CareGiver.ProfileVideoThumbnil = !string.IsNullOrEmpty(objCareGiverModel.ProfileVideoThumbnil) ? objCareGiverModel.ProfileVideoThumbnil.Split('/')[objCareGiverModel.ProfileVideoThumbnil.Split('/').Length - 1] : "";
                }

                string result = CareGiverService.UpdateCareGiver(CareGiver).Result;

                int n;
                bool isNumeric = int.TryParse(result, out n);
                if (isNumeric)
                {
                    var ChattingController = new ChattingController();

                    var ChattingService = new ChattingServiceProxy();
                    ChattingsList ChattingsList = new ChattingsList();

                    
                    if (!string.Equals(objCareGiverModel.OldEmail, objCareGiverModel.Email, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(objCareGiverModel.OldEmail))
                    {
                        Task.Run(async () => { await ChattingController.ApiToChangeEmailToQuickblox(objCareGiverModel.OldEmail, objCareGiverModel.Email, objCareGiverModel.QuickBloxId); }).Wait();

                    }


                    #region
                    //   If office change at the time of update care giver than Remove &Add care giver from office chat group
                    //if (objCareGiverModel.OldOfficeId != objCareGiverModel.OfficeId)
                    //{
                    //    //var ChattingController = new ChattingController();

                    //    ChattingController.RemoveMemberFromOfficeGroup(objCareGiverModel.OldOfficeId.ToString(), objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);

                    //    ChattingController.AddMemberIntoOfficeGroup(objCareGiverModel.OfficeId.ToString(), objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);

                    //    // ************************************************************************
                    //    // When Office change that time remove user from all existing patient chat 
                    //    List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objCareGiverModel.UserId, "1").Result;
                    //    ChattingsList.objChattingsList = listChatting;
                    //    foreach (var item in listChatting)
                    //    {
                    //        ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);
                    //    }

                    //    // When Office change that time remove user from all existing group chat room
                    //    List<Chatting> listGroupChatting = ChattingService.GetChatGroupListByTypeIdForUser(objCareGiverModel.UserId, "2").Result;
                    //    ChattingsList.objChattingsList = listGroupChatting;
                    //    foreach (var item in listGroupChatting)
                    //    {
                    //        ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);
                    //    }
                    //    // ************************************************************************
                    //}
                    #endregion


                    if (!objCareGiverModel.IsAllowPatientChatRoom)
                    {
                        List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objCareGiverModel.UserId, "1").Result;
                        ChattingsList.objChattingsList = listChatting;

                        foreach (var item in listChatting)
                        {
                            ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);
                        }
                    }


                    if (!objCareGiverModel.IsAllowGroupChat)
                    {
                        List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objCareGiverModel.UserId, "2").Result;
                        ChattingsList.objChattingsList = listChatting;

                        foreach (var item in listChatting)
                        {
                            ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);
                        }

                        ChattingController.RemoveMemberFromOfficeGroup(objCareGiverModel.OldOfficeId.ToString(), objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);
                    }


                    if (objCareGiverModel.IsAllowGroupChat)
                    {
                        List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objCareGiverModel.UserId, "3").Result;
                        ChattingsList.objChattingsList = listChatting;

                        if (listChatting.Count == 0)
                        {
                            ChattingController.AddMemberIntoOfficeGroup(objCareGiverModel.OfficeId.ToString(), objCareGiverModel.UserId, objCareGiverModel.QuickBloxId, CareGiver.OrganisationId, OrganisationEmail);
                        }
                    }
                    

                    //string SandDataOfficeId = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();
                    //string[] OfficeIdSandData = SandDataOfficeId.Split(',');

                    //if (Array.Exists(OfficeIdSandData, element => element == Convert.ToString(objCareGiverModel.OfficeId)))
                    //{
                    //    string results = GetAndPostEmployeeDataByNurseId(objCareGiverModel.NurseId);
                    //}

                    if (CareGiver.OrganisationId > 0 && CareGiver.OrganisationId == 3)
                    {
                        string results = GetAndPostEmployeeDataAllmedEditAndUpdate(CareGiver.UserId);
                    }
                    else
                    {
                        string SandDataOfficeId = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();

                        string[] OfficeIdSandData = SandDataOfficeId.Split(',');

                        if (Array.Exists(OfficeIdSandData, element => element == Convert.ToString(objCareGiverModel.OfficeId)))
                        {
                            string results = GetAndPostEmployeeData(objCareGiverModel.OfficeId,CareGiver.UserId);
                        }

                    }


                    TempData["Message"] = "Care giver is updated successfully.";
                    return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ModelState.AddModelError("Error", result);
                    return Json(new { Result = "Service " + result + " is already in use. You can not remove it. " }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "[HttpPost] EditCareGiver";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            CareGiverModel objModel = new CareGiverModel();
            objModel = GetCareGiverDetailById(objCareGiverModel.NurseId.ToString());
            List<SelectListItem> li = new List<SelectListItem>();
            for (int i = 5; i <= 75; i += 5)
            {
                if (i == Convert.ToInt32(objModel.ServiceRadius))
                {
                    li.Add(new SelectListItem { Selected = true, Text = i + " Miles", Value = i.ToString() });
                }
                else
                {
                    li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
                }
            }
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            ViewData["Radious"] = li;
            ServicesServiceProxy ServicesService = new ServicesServiceProxy();
            ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;

            //return PartialView(objModel);

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }


        public void insertdata(string data)
        {
            string result = "Testing";
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


        // [Authorize(Roles = "SuperAdmin,Admin")]
        //  [RBAC]

        [Authorize(Roles = "SuperAdmin,Admin,OrgSuperAdmin")]
        public ActionResult SendApproval()
        {
            return PartialView();
        }


        //  [Authorize(Roles = "SuperAdmin,Admin")]
        //   [HttpPost]
        // [AllowAnonymous]
        //  [RBAC]


        [Authorize(Roles = "SuperAdmin,Admin,OrgSuperAdmin")]
        [HttpPost]
        public ActionResult SendApproval(CareGiverModel CareGiver)
        {
            try
            {
                CareGiverServiceProxy CareGiverService = new CareGiverServiceProxy();

                //if (CareGiver.IsApproved == "1" && CareGiver.Email != "")
                //{
                //    MembershipUser user = Membership.GetUser(new Guid(CareGiver.UserId));
                //    user.ChangePassword(user.ResetPassword(), CareGiver.Password);
                //}

                CareGivers objCareGiver = new CareGivers();
                objCareGiver.NurseId = CareGiver.NurseId;
                objCareGiver.IsApprove = Convert.ToBoolean(Convert.ToInt32(CareGiver.IsApproved));
                objCareGiver.UserName = CareGiver.Username;
                objCareGiver.InsertUserId = Membership.GetUser().ProviderUserKey.ToString();
                objCareGiver.UserId = Membership.GetUser().ProviderUserKey.ToString();
                objCareGiver.Password = CareGiver.Password;
                objCareGiver.ChargeToPatient = Convert.ToDecimal(CareGiver.ChargeToPatient);
                objCareGiver.IsAllowOneToOneChat = CareGiver.IsAllowOneToOneChat;
                objCareGiver.IsAllowPatientChatRoom = CareGiver.IsAllowPatientChatRoom;
                objCareGiver.IsAllowGroupChat = CareGiver.IsAllowGroupChat;
                objCareGiver.IsAllowToCreateGroupChat = CareGiver.IsAllowToCreateGroupChat;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                if (CareGiver.Email != "" && CareGiver.Email != null)
                {
                    objCareGiver.UserId = CareGiver.UserId;
                }

                var ChattingController = new ChattingController();

                string result = CareGiverService.ApproveRejectNurse(objCareGiver).Result;
                CareGiverModel obj = GetCareGiverDetailById(CareGiver.NurseId.ToString());
                if (result == "Success" && CareGiver.Email == null)
                {
                    //CareGiverModel obj = GetCareGiverDetailById(CareGiver.NurseId.ToString());

                    PushNotification objNotification = new PushNotification();
                    objNotification.SendPushNotification("CareGiver Reject", "You are rejected by CareGiver super Admin", obj.DeviceToken, obj.DeviceType, obj.UserId, "0", obj.NurseId.ToString(), "0", "CareGiver Reject", Membership.GetUser().ProviderUserKey.ToString(), true);

                    //string strMessage = "CareGiver Reject: You have been Rejected by superadmin";
                    //string tophoneNo = obj.Phone;

                    //if (SendSMSService.SendSMS(strMessage, tophoneNo))
                    //{
                    //    result = "Success";
                    //}

                    ChattingController.RemoveMemberFromOfficeGroup(CareGiver.OfficeId.ToString(), CareGiver.UserId, CareGiver.QuickBloxId, OrganisationId, OrganisationEmail);

                    TempData["message"] = "Care giver has been  rejected successfully.";
                    return Json(new { Result = "Success", JsonRequestBehavior.AllowGet });
                }
                else if (result == "Success" && CareGiver.Email != null)
                {
                    //CareGiverModel obj = GetCareGiverDetailById(CareGiver.NurseId.ToString());

                    PushNotification objNotification = new PushNotification();
                    objNotification.SendPushNotification("CareGiver Approved", "You are Approved by CareGiver super Admin", obj.DeviceToken, obj.DeviceType, obj.UserId, "0", obj.NurseId.ToString(), "0", "CareGiver Approveds", Membership.GetUser().ProviderUserKey.ToString(), true);

                    //string strMessage = "CareGiver Approved: You have been apporoved by superadmin";
                    //string tophoneNo = obj.Phone;

                    //if (SendSMSService.SendSMS(strMessage, tophoneNo))
                    //{
                    //    result = "Success";
                    //}

                    //string Message = "";
                    //string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/EmailTemplates/CareGiverApproved.html");
                    //using (StreamReader reader = new StreamReader(filePath))
                    //{
                    //    Message = reader.ReadToEnd();
                    //}

                    //Message = Message.Replace("{CareGiverName}", obj.Name);

                    //MessageQueue MQ = new MessageQueue();
                    //MQ.EmailID = obj.Email;
                    //MQ.Message = Message;
                    //MQ.Subject = "Account activated";
                    //MQ.UserID = Membership.GetUser().ProviderUserKey.ToString();
                    //MQ.MobileNumber = "0";

                    //MessageQueueServiceProxy MessageQueueService = new MessageQueueServiceProxy();
                    //result = MessageQueueService.InsertMessageQueue(MQ).Result;

                    ChattingController.AddMemberIntoOfficeGroup(CareGiver.OfficeId.ToString(), CareGiver.UserId, CareGiver.QuickBloxId, OrganisationId, OrganisationEmail);

                    TempData["message"] = "Care giver has been  approved successfully.";
                    return RedirectToAction("CareGiver", "CareGiver");
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "ApprovedRejectNurse";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }


        [HttpPost]
        public JsonResult TrackLocationByNurseId(int NurseId, string date)
        {
            List<CareGiverTrackLocation> CareGiverTrackLocationList = new List<CareGiverTrackLocation>();
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                if (NurseId > 0)
                {
                    List<CareGiverTrackLocation> CareGiversTrackLocationList = new List<CareGiverTrackLocation>();

                    SchedulePatientRequestServiceProxy SchedulePatientRequestService = new SchedulePatientRequestServiceProxy();
                    CareGiverTrackLocationList = SchedulePatientRequestService.GetTrackLocationByNurseId(Convert.ToString(NurseId), date).Result;

                    res["Success"] = true;
                    res["Result"] = CareGiverTrackLocationList;
                }
                else
                {
                    res["Success"] = false;
                    res["Result"] = "Nurse Request Id not found";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "TrackLocationByNurseId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string result = ErrorLogService.InsertErrorLog(log).Result;
                res["Success"] = false;
                res["Result"] = result;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //Started by Pinki on 18/09/2017
        #region Superadmin can assign multiple chat groups to caregiver
        public ActionResult AssignGroup(string UserId, string CaregiverName, string QuickBloxId, string Email)
        {

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            ViewBag.UserId = UserId;
            ViewBag.CaregiverName = CaregiverName;
            ViewBag.QuickBloxId = QuickBloxId;
            ViewBag.Email = Email;
            ViewBag.UnassignedGroupList = GetAllGroupExceptAssignedGroupByUserId(UserId);
            ViewBag.AssignedGroupList = GetAllAssignedGroupByUserId(UserId);
            return PartialView("AssignGroupToCaregiver");
        }



        public List<Chatting> GetAllGroupExceptAssignedGroupByUserId(string UserId)
        {
            var chattingGroups = new List<Chatting>();
            try
            {
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                chattingGroups = new ChattingServiceProxy().GetAllGroupExceptAssignedGroupByUserId(UserId, LoginUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetAllGroupExceptAssignedGroupByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return chattingGroups;
        }

        public List<Chatting> GetAllAssignedGroupByUserId(string UserId)
        {
            var chattingGroups = new List<Chatting>();
            try
            {
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                var ChattingService = new ChattingServiceProxy();
                chattingGroups = new ChattingServiceProxy().GetAllAssignedGroupByUserId(UserId, LoginUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "GetAllAssignedGroupByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return chattingGroups;
        }

        public string AssignGroupToCaregiver(string ChattingGroupIds, string UserId, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var objDialogDetail = new Chatting();
                MembershipUser user = Membership.GetUser();
                //  var SchedulerEmail = user.Email;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {
                    SchedulerEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                }
                else
                {
                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }

                var ChattingGroupId = ChattingGroupIds.Split(',');

                foreach (var groupId in ChattingGroupId)
                {
                    //result = ChattingService.AssignGroupToCaregiver(groupId, UserId).Result;
                    result = ChattingService.AssignGroupToUser(groupId, UserId).Result;
                    if (!string.IsNullOrEmpty(result))
                    {
                        objDialogDetail = ChattingService.GetDialogDetail(groupId).Result;

                        Task.Run(async () => { await AddCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId); }).Wait();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "AssignGroupToCaregiver";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        public JsonResult RemoveCaregiverFromGroupChat(string ChattingGroupId, string UserId, string QuickBloxId)
        {
            string result = "";
            var SchedulerEmail = "";
            try
            {


                var ChattingService = new ChattingServiceProxy();

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();
                //  var SchedulerEmail = user.Email;


                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {
                    SchedulerEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                }
                else
                {
                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }

                /// var SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId); }).Wait();
                result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "RemoveCaregiverFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            var lstChattingGroup = GetAllGroupExceptAssignedGroupByUserId(UserId);
            return Json(new { result, lstChattingGroup }, JsonRequestBehavior.AllowGet);
        }

        private async Task<int> AddCareGiverToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId)
        {
            string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();
            var client = new System.Net.Http.HttpClient();

            client.BaseAddress = new Uri(QuickbloxAPIUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

            Random random = new Random();
            int Vnonce = random.Next(0, 9999);

            var input = new QuickBloxSession();
            input.application_id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
            input.auth_key = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
            input.nonce = Vnonce.ToString();
            input.timestamp = QuickBloxServiceProxy.Timestamp();

            input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
            //Encryption            
            input.signature = QuickBloxServiceProxy.Hash(input.signature, System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

            var userData = new Userdata();
            userData.login = SchedulerEmail; //"superadmin@caregiver.com"
            userData.password = "Welcome007!";
            input.user = userData;

            var jData1 = JsonConvert.SerializeObject(input);
            var content1 = new StringContent(jData1);

            var content = new StringContent(jData1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/session.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            //JObject json = JObject.Parse(result);
            var data = (JObject)JsonConvert.DeserializeObject(result);
            string token = data["session"]["token"].Value<string>();
            ////Sessoin Generated End

            //GetActivePatientRequest All Dialog Detail
            var clientGetDialogId = new System.Net.Http.HttpClient();
            clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
            clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
            var response1 = await clientGetDialogId.GetAsync("");
            var result1 = response1.Content.ReadAsStringAsync().Result;
            // new 
            var MyData = JsonConvert.DeserializeObject<QuickbloxReponse>(result1);
            var tempOccupants_ids = new List<int>();

            var loopCnt = Math.Floor(Convert.ToDecimal(MyData.total_entries / 100) + 1);
            var forlooplnt = 0;
            var forLoopflag = 0;
            for (int p = 0; p < MyData.total_entries; p++)
            {
                if (forLoopflag == 0)
                {
                    if (p < 100)
                    {
                        var currentrow = MyData.items[p];
                        string tempDialogId = currentrow._id;
                        if (tempDialogId == DialogId)// (tempDialogId == "596f0f2da0eb4770e6d705d3") //(tempDialogId == DialogId)
                        {
                            tempOccupants_ids.AddRange(currentrow.occupants_ids);
                            forLoopflag = 1;
                            break;
                        }
                    }
                    else
                    {
                        forlooplnt++;
                        var skip = forlooplnt * 100;
                        var clientGetDialogId2 = new System.Net.Http.HttpClient();
                        clientGetDialogId2.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json?limit=100&skip=" + skip);
                        clientGetDialogId2.DefaultRequestHeaders.Accept.Clear();
                        clientGetDialogId2.DefaultRequestHeaders.Add("QB-Token", token);
                        var response2 = await clientGetDialogId2.GetAsync("");
                        var result2 = response2.Content.ReadAsStringAsync().Result;
                        // new 
                        var MyData2 = JsonConvert.DeserializeObject<QuickbloxReponse>(result2);
                        for (int q = 0; q < MyData2.limit; q++)
                        {
                            var currentrow2 = MyData2.items[q];
                            string tempDialogId = currentrow2._id;
                            if (tempDialogId == DialogId)// (tempDialogId == "596f0f2da0eb4770e6d705d3") //(tempDialogId == DialogId)
                            {
                                tempOccupants_ids.AddRange(currentrow2.occupants_ids);
                                forLoopflag = 1;
                                break;
                            }
                        }
                    }
                }
                else { break; }
            }
            // if() //if occupants_ids not match then Call to Add in group
            bool flag = false;
            for (int k = 0; k < tempOccupants_ids.Count; k++)
            {
                if (Convert.ToInt32(CaregiverQBId) == tempOccupants_ids[k])
                {
                    flag = true;
                    break;
                }
            }
            //Add Member to group
            if (flag == false)
            {
                var objAddDialog = new AddDialog();
                List<int> objoccupants_ids = new List<int>();
                objoccupants_ids.Add(Convert.ToInt32(CaregiverQBId));
                try
                {
                    objAddDialog.name = GroupName;
                    var objPullAll = new PullAll();
                    objPullAll.occupants_ids = objoccupants_ids;

                    objAddDialog.push_all = objPullAll;
                }
                catch (Exception e)
                { }

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;
            }
            return 1;
        }

        private async Task<int> RemoveCareGiverToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId)
        {
            //----
            string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();
            var client = new System.Net.Http.HttpClient();

            client.BaseAddress = new Uri(QuickbloxAPIUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

            Random random = new Random();
            int Vnonce = random.Next(0, 9999);

            var input = new QuickBloxSession();
            input.application_id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
            input.auth_key = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
            input.nonce = Vnonce.ToString();
            input.timestamp = QuickBloxServiceProxy.Timestamp();

            input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
            //Encryption            
            input.signature = QuickBloxServiceProxy.Hash(input.signature, System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

            var userData = new Userdata();
            userData.login = SchedulerEmail; //"superadmin@caregiver.com"
            userData.password = "Welcome007!";
            input.user = userData;

            var jData1 = JsonConvert.SerializeObject(input);
            var content1 = new StringContent(jData1);

            var content = new StringContent(jData1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/session.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            //JObject json = JObject.Parse(result);
            var data = (JObject)JsonConvert.DeserializeObject(result);
            string token = data["session"]["token"].Value<string>();
            ////Sessoin Generated End

            //GetActivePatientRequest All Dialog Detail
            var clientGetDialogId = new System.Net.Http.HttpClient();

            clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
            clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
            var response1 = await clientGetDialogId.GetAsync("");
            var result1 = response1.Content.ReadAsStringAsync().Result;
            // new 
            var MyData = JsonConvert.DeserializeObject<QuickbloxReponse>(result1);
            var tempOccupants_ids = new List<int>();

            //  var loopCnt = Math.Floor(Convert.ToDecimal(MyData.total_entries / 100) + 1);
            //var forlooplnt = 0;
            //var forLoopflag = 0;
            //for (int p = 0; p < MyData.total_entries; p++)
            //{
            //    if (forLoopflag == 0)
            //    {
            //        if (p < 100)
            //        {
            //            var currentrow = MyData.items[p];
            //            string tempDialogId = currentrow._id;
            //            if (tempDialogId == DialogId)// (tempDialogId == "596f0f2da0eb4770e6d705d3") //(tempDialogId == DialogId)
            //            {
            //                tempOccupants_ids.AddRange(currentrow.occupants_ids);
            //                forLoopflag = 1;
            //                break;
            //            }
            //        }
            //        else
            //        {

            //            forlooplnt++;
            //            var skip = forlooplnt * 100;
            //            var clientGetDialogId2 = new System.Net.Http.HttpClient();
            //            clientGetDialogId2.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json?limit=100&skip=" + skip);
            //            clientGetDialogId2.DefaultRequestHeaders.Accept.Clear();
            //            clientGetDialogId2.DefaultRequestHeaders.Add("QB-Token", token);
            //            var response2 = await clientGetDialogId2.GetAsync("");
            //            var result2 = response2.Content.ReadAsStringAsync().Result;
            //            // new 
            //            var MyData2 = JsonConvert.DeserializeObject<QuickbloxReponse>(result2);
            //            for (int q = 0; q < MyData2.limit; q++)
            //            {
            //                var currentrow2 = MyData2.items[q];
            //                string tempDialogId = currentrow2._id;
            //                if (tempDialogId == DialogId)// (tempDialogId == "596f0f2da0eb4770e6d705d3") //(tempDialogId == DialogId)
            //                {
            //                    tempOccupants_ids.AddRange(currentrow2.occupants_ids);
            //                    forLoopflag = 1;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    else { break; }
            //}


            //if occupants_ids get match then Call to remove from group
            //bool flag = false;
            //for (int k = 0; k < tempOccupants_ids.Count; k++)
            //{
            //    if (Convert.ToInt32(CaregiverQBId) == tempOccupants_ids[k])
            //    {
            //        flag = true;
            //        break;
            //    }
            //}            

            //#region Remove Member from group
            //if (flag == true)
            //{

            var updateDialog = new UpdateDialog();
            List<int> objoccupants_ids = new List<int>();
            objoccupants_ids.Add(Convert.ToInt32(CaregiverQBId));
            try
            {
                updateDialog.name = GroupName;
                var objPullAll = new PullAll();
                objPullAll.occupants_ids = objoccupants_ids;
                updateDialog.pull_all = objPullAll;
            }
            catch (Exception e)
            { }
            var clientAddMember = new System.Net.Http.HttpClient();

            clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
            clientAddMember.DefaultRequestHeaders.Accept.Clear();
            clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

            var jData2 = JsonConvert.SerializeObject(updateDialog);
            var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
            var response2 = await clientAddMember.PutAsync("", content2);
            var result2 = response2.Content.ReadAsStringAsync().Result;
            //}
            //#endregion

            return 1;
        }
        #endregion


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


        [HttpGet]
        public ActionResult GetReadExcelSheet()
        {
            return PartialView("GetReadExcelSheet");
        }


        [HttpPost]
        public ActionResult GetReadExcelSheetUpload()
        {
            
                string x = "";
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {

                    HttpFileCollectionBase files = Request.Files;

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


                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                file.SaveAs(filePath);

                string conString = string.Empty;


                //switch (extension)
                //{
                //    case ".xls": //Excel 97-03.
                //        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                //        break;
                //    case ".xlsx": //Excel 07 and above.
                //        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0 Xml;HDR=YES'";
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
                            }catch(Exception ex)
                            {

                                
                            }

                            #region
                            string ClientPayerId = "";
                            string  medicalId= "";
                            string PayerId = "";
                            string PayerProgram = "";
                            string ProcedureCode = "";

                            int rowsAffected = 0;
                            int affcted = 0;

                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                            {
                                conn.Open();
                                //using (SqlCommand cmd = new SqlCommand("UPDATE patients SET ClientPayerId = @ClientPayerId,  WHERE MedicalId = @MedicalId", conn))
                                //{
                                //    // Add parameters once and reuse
                                //    cmd.Parameters.Add("@ClientPayerId", SqlDbType.VarChar);
                                //    cmd.Parameters.Add("@MedicalId", SqlDbType.VarChar);

                                //    foreach (DataRow item in dt.Rows)
                                //    {
                                //         ClientPayerId = item["Medicaid ID"].ToString();
                                //         medicalId = item["Medical ID"].ToString();
                                //         PayerId = item["PayerID"].ToString();
                                //         PayerProgram = item["PayerProgram"].ToString();
                                //         ProcedureCode = item["ProcedureCode"].ToString();

                                //        cmd.Parameters["@ClientPayerId"].Value = ClientPayerId;
                                //        cmd.Parameters["@MedicalId"].Value = medicalId;
                                //        cmd.Parameters["@PayerID"].Value = ClientPayerId;
                                //        cmd.Parameters["@PayerProgram"].Value = medicalId;
                                //        cmd.Parameters["@ProcedureCode"].Value = ClientPayerId;

                                //        break;
                                //         rowsAffected = cmd.ExecuteNonQuery();

                                //        if (rowsAffected > 0)
                                //        {
                                //            // Your custom logic
                                //        }
                                //    }
                                //}


                                //using (SqlCommand cmd = new SqlCommand("UPDATE patients SET ClientPayerId = @ClientPayerId, PayerID = @PayerID, PayerProgram = @PayerProgram, ProcedureCode = @ProcedureCode, DateOfBirth = @DateOfBirth WHERE MedicalId = @MedicalId", conn))
                                //{
                                //    cmd.Parameters.Add("@ClientPayerId", SqlDbType.VarChar);
                                //    cmd.Parameters.Add("@PayerID", SqlDbType.VarChar);
                                //    cmd.Parameters.Add("@PayerProgram", SqlDbType.VarChar);
                                //    cmd.Parameters.Add("@ProcedureCode", SqlDbType.VarChar);
                                //    cmd.Parameters.Add("@MedicalId", SqlDbType.VarChar);
                                //    cmd.Parameters.Add("@DateOfBirth", SqlDbType.VarChar); // Or SqlDbType.Date if column is date

                                //    foreach (DataRow item in dt.Rows)
                                //    {
                                //        cmd.Parameters["@ClientPayerId"].Value = item["Medicaid ID"].ToString();
                                //        cmd.Parameters["@PayerID"].Value = item["PayerID"].ToString();
                                //        cmd.Parameters["@PayerProgram"].Value = item["PayerProgram"].ToString();
                                //        cmd.Parameters["@ProcedureCode"].Value = item["ProcedureCode"].ToString();
                                //        cmd.Parameters["@MedicalId"].Value = item["Medical ID"].ToString();

                                //        cmd.Parameters["@DateOfBirth"].Value = DateTime.Parse(Convert.ToString(item["DOB"])).ToString("yyyy-MM-dd");

                                //            rowsAffected = cmd.ExecuteNonQuery();

                                //        if (rowsAffected > 0)
                                //        {
                                //            affcted += 1;
                                //            // Optional success handling
                                //        }
                                //    }
                                //}

                            }

                            Console.WriteLine($"{affcted} row(s) inserted.");
                            #endregion

                            #region
                            // Nurse update from Axess

                            //string NurseFullName = "";
                            //string NurseName = "";



                            //int a = 0;
                            //foreach (DataRow item in dt.Rows)
                            //{

                            //    NurseFullName = item["Clinician Name in Axxess"].ToString();
                            //    NurseName = item["Clinician Name in Paseva"].ToString();

                            //   SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
                            //    con1.Open();
                            //    SqlCommand cmd1 = new SqlCommand("InsertNurseFullName", con1);
                            //    cmd1.CommandType = CommandType.StoredProcedure;
                            //    cmd1.Parameters.AddWithValue("@NurseFullname", NurseFullName);
                            //    cmd1.Parameters.AddWithValue("@NurseName", NurseName);
                            //    int k = cmd1.ExecuteNonQuery();

                            //    if (k > 0)
                            //    {
                            //        a = a + 1;
                            //    }

                            //}

                            #endregion

                            #region
                            ////Nurse insertion before Coomented
                            //using (SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                            //{

                            //    SqlCommand cmd2 = new SqlCommand("GetExcessNurseData", con2);
                            //    cmd2.CommandType = CommandType.StoredProcedure;
                            //    //DataSet ds = new DataSet();
                            //    DataTable dt3 = new DataTable();
                            //    SqlDataAdapter da3 = new SqlDataAdapter(cmd2);
                            //    da3.Fill(dt3);

                            //    #region
                            //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                            //    {

                            //        SqlCommand cmd = new SqlCommand("GetCheckCareGiverByName", con);
                            //        cmd.CommandType = CommandType.StoredProcedure;
                            //        DataSet ds = new DataSet();

                            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
                            //        da.Fill(ds);

                            //        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            //        //{

                            //        // J is for axess table 

                            //        //and i for paseva nurse table

                            //        for (int j = 0; j < dt3.Rows.Count; j++)
                            //        {
                            //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            //            {

                            //                x = dt3.Rows[j]["Name"].ToString();
                            //                string[] strarr = x.Split(',');
                            //                string y = strarr[0];
                            //                string[] xy = strarr[1].Split(' ');

                            //                string z = xy[0];

                            //                string last = xy[1];

                            //                string dsfirst = ds.Tables[0].Rows[i]["Name"].ToString();

                            //                // if (y.Contains("PatriciaBellRN")==true /*&& last.Contains("Patricia Bell RN")*/)

                            //                if (dsfirst.Contains(y) && dsfirst.Contains(last))
                            //                {

                            //                    string result = "";

                            //                    string eeids = dt3.Rows[j]["EEID"].ToString();
                            //                    string nurseidds = ds.Tables[0].Rows[i]["NurseId"].ToString();

                            //                    SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
                            //                    con1.Open();
                            //                    SqlCommand cmd1 = new SqlCommand("InsertIntoNurseFromExcel", con1);
                            //                    cmd1.CommandType = CommandType.StoredProcedure;
                            //                    cmd1.Parameters.AddWithValue("@NurseId", ds.Tables[0].Rows[i]["NurseId"].ToString());
                            //                    cmd1.Parameters.AddWithValue("@EEID", dt3.Rows[j]["EEID"].ToString());
                            //                    int k = cmd1.ExecuteNonQuery();
                            //                    if (k == 1)
                            //                    {
                            //                        result = "success";
                            //                        x= "";
                            //                        eeids = "";
                            //                        nurseidds = "";
                            //                        dsfirst = "";

                            //                        break;

                            //                    }

                            //                }


                            //            }
                            //        }
                            //    }

                            //    #endregion


                            //}

                            #endregion

                            #region
                            //Data Added to DataTable with New column and Assigning Value

                            //dt.Columns.Add(new DataColumn("Latitude", typeof(string)));
                            //dt.Columns.Add(new DataColumn("Longitude", typeof(string)));

                            //foreach (DataRow item in dt.Rows)
                            //{

                            //    if (item["MedicalId"].ToString() == "" || string.IsNullOrEmpty(item["MedicalId"].ToString()))
                            //    {
                            //        break;
                            //    }


                            //    string street= item["Street"].ToString();
                            //    string state= item["City"].ToString();
                            //    string city= item["State"].ToString();
                            //    string Zipcode= item["ZipCode"].ToString();

                            //    string FullAddress = street + ", " + city + "," + state + "," + Zipcode;
                            //    // FullAddress = schedulePatient.Address + "," + schedulePatient.ZipCode;

                            //    var requestUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address=key{0}&key={1}", FullAddress, "AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw");
                            //    using (var client = new WebClient())
                            //    {
                            //        var result1 = client.DownloadString(requestUrl);
                            //        var data1 = JsonConvert.DeserializeObject<JObject>(result1);

                            //        var Latitude = data1["results"][0]["geometry"]["location"]["lat"];
                            //        var Longitude = data1["results"][0]["geometry"]["location"]["lng"];

                            //       string Latitudes = Convert.ToString(Latitude);
                            //       string Longitudes = Convert.ToString(Longitude);

                            //        item["Latitude"] = Latitudes;
                            //        item["Longitude"] = Longitudes;
                            //    }


                            //}

                            #endregion

                            connExcel.Close();

                        }
                    }
                }

                DataTable dt1 = dt.Copy();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        #region
                        //Set the database table name.
                        //sqlBulkCopy.DestinationTableName = "dbo.AxessNurse";

                        //// Map the Excel columns with that of the database table, this is optional but good if you do
                        ////
                        //sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                        //sqlBulkCopy.ColumnMappings.Add("Operator", "Operator");
                        //sqlBulkCopy.ColumnMappings.Add("CGI", "CGI");
                        //sqlBulkCopy.ColumnMappings.Add("MCC", "MCC");
                        //sqlBulkCopy.ColumnMappings.Add("MNC", "MNC");
                        //sqlBulkCopy.ColumnMappings.Add("LAC", "LAC");
                        //sqlBulkCopy.ColumnMappings.Add("CELLID", "CELLID");
                        //sqlBulkCopy.ColumnMappings.Add("Lat", "Lat");
                        //sqlBulkCopy.ColumnMappings.Add("Lng", "Lng");
                        //sqlBulkCopy.ColumnMappings.Add("Short Address", "Short Address");
                        //sqlBulkCopy.ColumnMappings.Add("Long Address", "Long Address");
                        //sqlBulkCopy.ColumnMappings.Add("NetType", "NetType");
                        //con.Open();
                        //sqlBulkCopy.WriteToServer(dt);
                        //con.Close();

                        #endregion

                        #region
                        //sqlBulkCopy.DestinationTableName = "dbo.Patients";

                        //// Map the Excel columns with that of the database table, this is optional but good if you do
                        ////
                        //sqlBulkCopy.ColumnMappings.Add("PatientName", "PatientName");
                        //sqlBulkCopy.ColumnMappings.Add("MedicalId", "MedicalId");
                        //// sqlBulkCopy.ColumnMappings.Add("InsertDateTime", "InsertDateTime");
                        //sqlBulkCopy.ColumnMappings.Add("Street", "Street");
                        //sqlBulkCopy.ColumnMappings.Add("City", "City");
                        //sqlBulkCopy.ColumnMappings.Add("State", "State");
                        //sqlBulkCopy.ColumnMappings.Add("ZipCode", "ZipCode");
                        //sqlBulkCopy.ColumnMappings.Add("PhoneNo", "PhoneNo");
                        //sqlBulkCopy.ColumnMappings.Add("OfficeId", "OfficeId");
                        //sqlBulkCopy.ColumnMappings.Add("PrimaryMD", "PrimaryMD");


                        //sqlBulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                        //sqlBulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                        //sqlBulkCopy.ColumnMappings.Add("Short Address", "Short Address");
                        //sqlBulkCopy.ColumnMappings.Add("Long Address", "Long Address");
                        //sqlBulkCopy.ColumnMappings.Add("NetType", "NetType");
                        #endregion

                        #region
                        //sqlBulkCopy.DestinationTableName = "dbo.SanDataClientPayerInformation";

                        //sqlBulkCopy.ColumnMappings.Add("PatientName", "PatientName");
                        //sqlBulkCopy.ColumnMappings.Add("MRN", "MRN");
                        //// sqlBulkCopy.ColumnMappings.Add("InsertDateTime", "InsertDateTime");
                        //sqlBulkCopy.ColumnMappings.Add("EffectiveDate", "EffectiveDate");
                        //sqlBulkCopy.ColumnMappings.Add("ServiceDate", "ServiceDate");
                        //sqlBulkCopy.ColumnMappings.Add("EpisodeStartDate", "EpisodeStartDate");
                        //sqlBulkCopy.ColumnMappings.Add("EpisodeEndDate", "EpisodeEndDate");
                        //sqlBulkCopy.ColumnMappings.Add("Description", "Description");
                        //sqlBulkCopy.ColumnMappings.Add("PayerID", "PayerID");
                        //sqlBulkCopy.ColumnMappings.Add("PayerProgram", "PayerProgram");


                        //sqlBulkCopy.ColumnMappings.Add("ProcedureCode", "ProcedureCode");
                        //sqlBulkCopy.ColumnMappings.Add("JurisdictionCode", "JurisdictionCode");

                        //sqlBulkCopy.ColumnMappings.Add("InsuranceName", "InsuranceName");
                        //sqlBulkCopy.ColumnMappings.Add("Claims", "Claims");

                        //sqlBulkCopy.ColumnMappings.Add("ChargeAmount", "ChargeAmount");
                        //sqlBulkCopy.ColumnMappings.Add("ContractualAdjustment", "ContractualAdjustment");

                        //sqlBulkCopy.ColumnMappings.Add("Allowed", "Allowed");
                        //sqlBulkCopy.ColumnMappings.Add("RemittancePayments", "RemittancePayments");

                        //sqlBulkCopy.ColumnMappings.Add("RemittanceAdjustments", "RemittanceAdjustments");
                        //sqlBulkCopy.ColumnMappings.Add("CorrectionAdjustment", "CorrectionAdjustment");

                        //sqlBulkCopy.ColumnMappings.Add("Balance", "Balance");
                        //  sqlBulkCopy.ColumnMappings.Add("Age", "Age");
                        #endregion

                        #region

                        #region
                        sqlBulkCopy.DestinationTableName = "dbo.Client";

                        //// Map the Excel columns with that of the database table, this is optional but good if you do
                        ////
                        sqlBulkCopy.ColumnMappings.Add("ClientName", "ClientName");
                        sqlBulkCopy.ColumnMappings.Add("CRNumber", "CRN Number");
                        sqlBulkCopy.ColumnMappings.Add("InsertDateTime", DateTime.Now.ToString("yyyy’-‘MM’-‘dd’ HH’:’mm’:’ss"));
                        sqlBulkCopy.ColumnMappings.Add("Street", "Street");
                        sqlBulkCopy.ColumnMappings.Add("City", "City");
                        sqlBulkCopy.ColumnMappings.Add("State", "State");
                        sqlBulkCopy.ColumnMappings.Add("ZipCode", "ZipCode");
                        sqlBulkCopy.ColumnMappings.Add("PhoneNo", "PhoneNo");
                        sqlBulkCopy.ColumnMappings.Add("OfficeId", "OfficeId");
                        sqlBulkCopy.ColumnMappings.Add("CompanyName", "CompanyName");


                        //sqlBulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                        //sqlBulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                        //sqlBulkCopy.ColumnMappings.Add("Short Address", "Short Address");
                        //sqlBulkCopy.ColumnMappings.Add("Long Address", "Long Address");
                        //sqlBulkCopy.ColumnMappings.Add("NetType", "NetType");
                        #endregion

                        #endregion

                        //con.Open();
                        //sqlBulkCopy.WriteToServer(dt);
                        //con.Close();
                    }
                }



            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            ViewBag.Success = "File Imported and excel data saved into database";

            return View();


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


        public string GetAndPostEmployeeData(int OfficeId, string UserId)
        {
            EmployeeClientModel objModel = new EmployeeClientModel();
            string result = "";
            List<EmplyeeRequest> EmployeeAddRequest = new List<EmplyeeRequest>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeToaddSandData", OfficeId,UserId);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            EmplyeeRequest EmplyeeRequestAdd = new EmplyeeRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();

                            switch (OfficeId)
                            {
                                case 1:
                                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDMA"].ToString();
                                    objprovider.ProviderQualifier = "Other";
                                    break;
                                case 5:
                                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                                    objprovider.ProviderQualifier = "MedicaidID";
                                    break;
                                case 12:
                                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDCASD"].ToString();
                                    objprovider.ProviderQualifier = "MedicaidID";
                                    break;
                            }

                            //if (OfficeId == 5)
                            //{
                            //    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                            //}
                            //else
                            //{

                            //    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDCASD"].ToString();
                            //}

                            //objprovider.ProviderQualifier = "MedicaidID";


                            EmplyeeRequestAdd.ProviderIdentification = objprovider;

                            EmplyeeRequestAdd.EmployeeQualifier = "EmployeeCustomID";
                            EmplyeeRequestAdd.EmployeeIdentifier = Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeOtherID = Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeEmail = Convert.ToString(item["Email"]);
                            EmplyeeRequestAdd.EmployeeFirstName = Convert.ToString(item["Name"]);
                            EmplyeeRequestAdd.EmployeeLastName = Convert.ToString(item["ServiceName"]);
                            EmplyeeRequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];

                            EmployeeAddRequest.Add(EmplyeeRequestAdd);
                        }
                    }
            }
            catch (Exception ex)
            {

            }

            var arraylist = EmployeeAddRequest.ToArray();

            List<EmplyeeRequest> request = new List<EmplyeeRequest>();

            foreach (var ReqItem in arraylist)
            {
                request.Add(ReqItem);
            }
        
            var clientGetDialogId = new System.Net.Http.HttpClient();

            string Token = ConfigurationManager.AppSettings["mykey"].ToString();
            string actheader = ConfigurationManager.AppSettings["Token"].ToString();

            string x = JsonConvert.SerializeObject(request);

            Task.Run(async () => { result = await objModel.SubmitEmployeeRequestData(x,OfficeId); }).Wait(); ;

            return result;
        }



        public string GetAndPostEmployeeDataEditAndUpdate(int OfficeId, string UserId)
        {
            EmployeeClientModel objModel = new EmployeeClientModel();
            string result = "";
            List<EmplyeeRequest> EmployeeAddRequest = new List<EmplyeeRequest>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeToaddSandData", OfficeId, UserId);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            EmplyeeRequest EmplyeeRequestAdd = new EmplyeeRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();

                            if (OfficeId == 5)
                            {
                                objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                            }
                            else
                            {

                                objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDCASD"].ToString();
                            }

                            objprovider.ProviderQualifier = "MedicaidID";

                            EmplyeeRequestAdd.ProviderIdentification = objprovider;

                            EmplyeeRequestAdd.EmployeeQualifier = "EmployeeCustomID";
                            EmplyeeRequestAdd.EmployeeIdentifier = Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeOtherID = Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeEmail = Convert.ToString(item["Email"]);
                            EmplyeeRequestAdd.EmployeeFirstName = Convert.ToString(item["Name"]);
                            EmplyeeRequestAdd.EmployeeLastName = Convert.ToString(item["ServiceName"]);
                            EmplyeeRequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];

                            EmployeeAddRequest.Add(EmplyeeRequestAdd);
                        }
                    }
            }
            catch (Exception ex)
            {

            }

            var arraylist = EmployeeAddRequest.ToArray();

            List<EmplyeeRequest> request = new List<EmplyeeRequest>();

            foreach (var ReqItem in arraylist)
            {
                request.Add(ReqItem);
            }

            var clientGetDialogId = new System.Net.Http.HttpClient();

            string x = JsonConvert.SerializeObject(request);

            Task.Run(async () => { result = await objModel.SubmitEmployeeRequestData(x, OfficeId); }).Wait(); ;

            return result;
        }


        public string GetAndPostEmployeeDataAllmed(string UserId)
        {
            EmployeeClientModel objModel = new EmployeeClientModel();
            string EmployeeSSN = "00001";
            string result = "";
            List<EmplyeeRequest> EmployeeAddRequest = new List<EmplyeeRequest>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeToaddSandDataAllMed", UserId);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            EmplyeeRequest EmplyeeRequestAdd = new EmplyeeRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();
                            objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDMed"].ToString();
                            objprovider.ProviderQualifier = "NPI";

                            EmplyeeRequestAdd.ProviderIdentification = objprovider;

                            EmplyeeRequestAdd.EmployeeQualifier = "EmployeeSSN";
                            EmplyeeRequestAdd.EmployeeIdentifier = Convert.ToString(item["EEID"]);
                            EmplyeeRequestAdd.EmployeeOtherID = Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeSSN = EmployeeSSN + Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeEmail = Convert.ToString(item["Email"]);
                            EmplyeeRequestAdd.EmployeeFirstName = Convert.ToString(item["Name"]);
                            EmplyeeRequestAdd.EmployeeLastName = Convert.ToString(item["ServiceName"]);
                            EmplyeeRequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];

                            EmployeeAddRequest.Add(EmplyeeRequestAdd);
                        }
                    }
            }
            catch (Exception ex)
            {
                return result = "false";
            }

            var arraylist = EmployeeAddRequest.ToArray();

            List<EmplyeeRequest> request = new List<EmplyeeRequest>();

            foreach (var ReqItem in arraylist)
            {
                request.Add(ReqItem);
            }

            var clientGetDialogId = new System.Net.Http.HttpClient();

            string Token = ConfigurationManager.AppSettings["mykeyMed"].ToString();
            string actheader = ConfigurationManager.AppSettings["TokenMed"].ToString();

            string x = JsonConvert.SerializeObject(request);

            Task.Run(async () => { result = await objModel.SubmitEmployeeRequestDataAllMed(x); }).Wait(); ;

            return result;
        }



        public string GetAndPostEmployeeDataAllmedEditAndUpdate(string UserId)
        {
            EmployeeClientModel objModel = new EmployeeClientModel();
            string EmployeeSSN = "00001";
            string result = "";
            List<EmplyeeRequest> EmployeeAddRequest = new List<EmplyeeRequest>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeToaddSandDataEditForAllMed", UserId);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            EmplyeeRequest EmplyeeRequestAdd = new EmplyeeRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();
                            objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDMed"].ToString();
                            objprovider.ProviderQualifier = "NPI";

                            EmplyeeRequestAdd.ProviderIdentification = objprovider;

                            EmplyeeRequestAdd.EmployeeQualifier = "EmployeeSSN";
                            EmplyeeRequestAdd.EmployeeIdentifier = Convert.ToString(item["EEID"]);
                            EmplyeeRequestAdd.EmployeeOtherID = Convert.ToString(item["EEID"]);

                            //EmplyeeRequestAdd.EmployeeSSN = Convert.ToString(item["NurseId"]);
                            //EmplyeeRequestAdd.EmployeeQualifier = "EmployeeCustomID";
                            //EmplyeeRequestAdd.EmployeeIdentifier = Convert.ToString(item["NurseId"]);
                            //EmplyeeRequestAdd.EmployeeOtherID = Convert.ToString(item["NurseId"]);

                            EmplyeeRequestAdd.EmployeeSSN = EmployeeSSN + Convert.ToString(item["NurseId"]);

                            EmplyeeRequestAdd.EmployeeEmail = Convert.ToString(item["Email"]);
                            EmplyeeRequestAdd.EmployeeFirstName = Convert.ToString(item["Name"]);
                            EmplyeeRequestAdd.EmployeeLastName = Convert.ToString(item["ServiceName"]);
                            EmplyeeRequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];

                            EmployeeAddRequest.Add(EmplyeeRequestAdd);
                        }
                    }
            }
            catch (Exception ex)
            {
                return result = "false";
            }

            var arraylist = EmployeeAddRequest.ToArray();

            List<EmplyeeRequest> request = new List<EmplyeeRequest>();

            foreach (var ReqItem in arraylist)
            {
                request.Add(ReqItem);
            }

            var clientGetDialogId = new System.Net.Http.HttpClient();

            string Token = ConfigurationManager.AppSettings["mykeyMed"].ToString();
            string actheader = ConfigurationManager.AppSettings["TokenMed"].ToString();

            string x = JsonConvert.SerializeObject(request);

            Task.Run(async () => { result = await objModel.SubmitEmployeeRequestDataAllMed(x); }).Wait(); ;

            return result;
        }


        public string GetAndPostEmployeeDataByNurseId(int NurseId)
        {
            EmployeeClientModel objModel = new EmployeeClientModel();
            string result = "";
            List<EmplyeeRequest> EmployeeAddRequest = new List<EmplyeeRequest>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeToaddSandDataByNurseId", NurseId);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            EmplyeeRequest EmplyeeRequestAdd = new EmplyeeRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();

                            objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();

                            objprovider.ProviderQualifier = "MedicaidID";

                            EmplyeeRequestAdd.ProviderIdentification = objprovider;

                            EmplyeeRequestAdd.EmployeeQualifier = "EmployeeCustomID";
                            EmplyeeRequestAdd.EmployeeIdentifier = Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeOtherID = Convert.ToString(item["NurseId"]);
                            EmplyeeRequestAdd.EmployeeEmail = Convert.ToString(item["Email"]);
                            EmplyeeRequestAdd.EmployeeFirstName = Convert.ToString(item["Name"]);
                            EmplyeeRequestAdd.EmployeeLastName = Convert.ToString(item["ServiceName"]);
                            EmplyeeRequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];

                            EmployeeAddRequest.Add(EmplyeeRequestAdd);
                        }
                    }
            }
            catch (Exception ex)
            {
                return result = "false";
            }
            if (EmployeeAddRequest.Count > 0 && EmployeeAddRequest != null)
            {
                var arraylist = EmployeeAddRequest.ToArray();

                List<EmplyeeRequest> request = new List<EmplyeeRequest>();

                foreach (var ReqItem in arraylist)
                {
                    request.Add(ReqItem);
                }
                var clientGetDialogId = new System.Net.Http.HttpClient();

                string Token = ConfigurationManager.AppSettings["mykey"].ToString();
                string actheader = ConfigurationManager.AppSettings["Token"].ToString();
                string x = JsonConvert.SerializeObject(request);

                int officeid = 0;
                Task.Run(async () => { result = await objModel.SubmitEmployeeRequestData(x,officeid); }).Wait(); ;
            }
            else
            {
                return result = "";
            }
            return result;
        }





        [HttpGet]
        public ActionResult SendEmployeeRequestDataPage()
        {
            return PartialView("SendEmployeeRequestDataPage");
        }
        [HttpPost]
        public JsonResult GetAndPostEmployeeDataByDateRange(string FromDate, String ToDate, String OfficeId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            EmployeeClientModel objModel = new EmployeeClientModel();
            DateTime fromDatetime = Convert.ToDateTime(FromDate);
            string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            string APIresult = "";
            DateTime ToDatetime = Convert.ToDateTime(ToDate);
            string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

            List<EmplyeeRequest> EmployeeAddRequest = new List<EmplyeeRequest>();
            try
            {

                string SandDataOfficeId = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();
                string[] OfficeIdSandData = SandDataOfficeId.Split(',');

                if (Array.Exists(OfficeIdSandData, element => element == Convert.ToString(OfficeId)))
                {

                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetEmployeeToaddSandDataBYDateRange",
                                                                                                             FromScheduleDateTime,
                                                                                                             ToScheduleDateTime,
                                                                                                        OfficeId);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int i = 0;
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                EmplyeeRequest EmplyeeRequestAdd = new EmplyeeRequest();

                                ProviderIdentification objprovider = new ProviderIdentification();
                                objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                                objprovider.ProviderQualifier = "MedicaidID";

                                EmplyeeRequestAdd.ProviderIdentification = objprovider;

                                EmplyeeRequestAdd.EmployeeQualifier = "EmployeeCustomID";
                                EmplyeeRequestAdd.EmployeeIdentifier = Convert.ToString(item["NurseId"]);
                                EmplyeeRequestAdd.EmployeeOtherID = Convert.ToString(item["NurseId"]);
                                EmplyeeRequestAdd.EmployeeEmail = Convert.ToString(item["Email"]);
                                EmplyeeRequestAdd.EmployeeFirstName = Convert.ToString(item["Name"]);
                                EmplyeeRequestAdd.EmployeeLastName = Convert.ToString(item["ServiceName"]);

                                EmplyeeRequestAdd.SequenceID = Convert.ToString((int)(item["timestampdata"]) + i);

                                EmployeeAddRequest.Add(EmplyeeRequestAdd);

                                i = i + 1;
                            }
                        }
                    }
                    if (EmployeeAddRequest.Count <= 0)
                    {
                        result["Success"] = false;
                        result["Message"] = "No data Available to send";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                result["Success"] = false;
                result["Message"] = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var arraylist = EmployeeAddRequest.ToArray();

            List<EmplyeeRequest> request = new List<EmplyeeRequest>();

            foreach (var ReqItem in arraylist)
            {
                request.Add(ReqItem);
            }

            var clientGetDialogId = new System.Net.Http.HttpClient();

            string Token = ConfigurationManager.AppSettings["mykey"].ToString();
            string actheader = ConfigurationManager.AppSettings["Token"].ToString();

            string x = JsonConvert.SerializeObject(request);

            Task.Run(async () => { APIresult = await objModel.SubmitEmployeeRequestDataMultiple(x); }).Wait();
            if (APIresult != "")
            {
                if (APIresult.Contains("FAILED"))
                {
                    result["Success"] = false;
                    result["Message"] = "Data Not Sent";
                }
                else
                {
                    result["Success"] = true;
                    result["Message"] = "Data Sent";
                }
            }
            else
            {
                result["Success"] = false;
                result["Message"] = "Data Not Sent";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //   [HttpPost]

        public ActionResult CareGiverIndiVidualReports()
        {
            return PartialView("CareGiverIndiVidualReports");
        }


        public List<ReportsManagementDetailss> GetCaregiverVisitsData(int NurseId, int day)
        {
            //if(FromDate == "" && ToDate == "")
            //{
            //     FromDate = "1000-01-01";
            //     ToDate = "1000-01-01";
            //}

            List<ReportsManagementDetailss> AttendanceDetailsList = new List<ReportsManagementDetailss>();

            //  ReportsManagementDetailss objAttendanceDetail1 = new ReportsManagementDetailss();
            string TotalTravel = string.Empty;
            string CheckInTotalTime = string.Empty;
            string NewDateVar = string.Empty;

            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllcaregiverDetail",
                                                     NurseId,
                                                     //Convert.ToDateTime(FromDate),
                                                     //Convert.ToDateTime(ToDate)
                                                     day);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

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
                    objAttendanceDetail.status = ds.Tables[0].Rows[i]["Status"].ToString();

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
                //   ViewBag.GetallCareGiverPatientVisitList =

                return AttendanceDetailsList;
            }

            return AttendanceDetailsList;
        }


        public List<ReportsManagementDetailss> GetAllVisitReportCaregiver(int NurseId, int day)
        {


            List<ReportsManagementDetailss> AttendanceDetailsList = new List<ReportsManagementDetailss>();

            // ReportsDetailsLists ListAttendanceDetails = new ReportsDetailsLists();

            string TotalTravel = string.Empty;
            string CheckInTotalTime = string.Empty;
            string NewDateVar = string.Empty;
            try
            {
                //if (FromDate == "||" && ToDate == "||")
                //{
                //    FromDate = "1000-01-01";
                //    ToDate = "1000-01-01";
                //}



                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceList_vinnTestingData",
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceListwithNurseAndPatientNameparam",

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllVisitReportCaregiver",
                                                        NurseId,
                                                        day
                                                       //Convert.ToInt32(pageNo),
                                                       //Convert.ToInt32(recordPerPage),
                                                       //sortfield,
                                                       //sortorder,
                                                       //search,
                                                       //IsAdmin,
                                                       //LogInUserId,
                                                       // Convert.ToDateTime(FromDate),
                                                       // Convert.ToDateTime(ToDate),
                                                       //Filtercaregiver
                                                       );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

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
                    // ListAttendanceDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]) + 3;
                    //  ListAttendanceDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows.Count);
                    //  ListAttendanceDetails.AttendanceManagemenList = AttendanceDetailsList;
                    return AttendanceDetailsList;
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
            return AttendanceDetailsList;
        }


        public JsonResult LoadAllIndividualCareGiverReportData(int NurseId, int day)
        {
            AllIndividualsCareGiverReport ReportData = new AllIndividualsCareGiverReport();

            ReportData.GetAllVisitReportCaregiver = GetAllVisitReportCaregiver(NurseId, day);
            ReportData.GetCaregiverVisitsData = GetCaregiverVisitsData(NurseId, day);

            return Json(ReportData, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Duration = 0)]

        public ActionResult NurseClockInAttendance(String NurseId)
        {
            List<NurseClockInAttendance> ClockInAttendanceList = new List<Models.NurseClockInAttendance>();

            ClockInAttendanceList = GetAvailableClockInRequests(NurseId);

            return PartialView(ClockInAttendanceList);
        }



        public List<NurseClockInAttendance> GetAvailableClockInRequests(string NurseId)
        {
            // string UserId = GetUserIDFromAccessToken();
            List<NurseClockInAttendance> ClockInAttendanceList = new List<NurseClockInAttendance>();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAvailableClockInAttendanceByNurseId", NurseId);

                if (ds != null)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            NurseClockInAttendance ClockInAttendance = new NurseClockInAttendance();

                            ClockInAttendance.ClockRequestId = Convert.ToInt32(item["ClockRequestId"]);
                            ClockInAttendance.ClockInStatus = Convert.ToString(item["ClockInStatus"]);
                            ClockInAttendance.Date = DateTime.Parse(Convert.ToString(item["Date"])).ToString("MM/dd/yyyy");

                            ClockInAttendance.Name = Convert.ToString(item["Name"]);
                            ClockInAttendance.Address = Convert.ToString(item["Address"]);
                            ClockInAttendance.NurseId = Convert.ToInt32(item["NurseId"]);

                            if (!string.IsNullOrEmpty(Convert.ToString(item["ClockInDateTime"])))
                            {
                                ClockInAttendance.ClockInDateTime = Convert.ToString(item["ClockInDateTime"]);
                            }
                            else
                            {
                                ClockInAttendance.ClockInDateTime = "";
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(item["ServiceTime"])))
                            {
                                ClockInAttendance.ServiceTime = Convert.ToString(item["ServiceTime"]);
                            }
                            else
                            {
                                ClockInAttendance.ServiceTime = "";
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(item["ClockOutDatetime"])))
                            {
                                ClockInAttendance.ClockOutDatetime = Convert.ToString(item["ClockOutDatetime"]);
                            }
                            else
                            {
                                ClockInAttendance.ClockOutDatetime = "";
                            }

                            ClockInAttendanceList.Add(ClockInAttendance);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return ClockInAttendanceList;
        }

    }
}
