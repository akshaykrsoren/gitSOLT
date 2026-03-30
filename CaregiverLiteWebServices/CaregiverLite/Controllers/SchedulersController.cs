using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaregiverLite.Models;
using System.Web.Security;
using CaregiverLite.Models.Utility;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using CaregiverLite.Action_Filters;
using CaregiverLiteWCF;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data;
using DifferenzLibrary;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class SchedulersController : Controller
    {
        // GET: Schedulers
        public ActionResult Schedulers()
        {
            //List<Office> OfficeList = new List<Office>();
            //OfficeList = OfficeController.GetOfficeDropDownList().OfficeList;
            //ViewBag.OfficeDropDownList = new SelectList(OfficeList, "OfficeId", "OfficeName");

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            List<Office> OfficeList = new List<Office>();
            OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(),OrganisationId.ToString()).Result;
            SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");
            ViewBag.OfficeDropDownList = officeSelectList;

            FillAllOrganisations();

            return View();
        }

        private void FillAllOrganisations(object SelectedValue = null)
        {
            SelectedValue = Convert.ToInt32(Session["OrganisationId"]);

            try { 
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
                log.Pagename = "SchedulersController";
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
                log.Pagename = "SchedulersController";
                log.Methodname = "IsEmailExists";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSchedulerList(JQueryDataTableParamModel param)
        {
            SchedulersList SchedulersList = new SchedulersList();
            try
            {
                string IsActiveStatus = "";
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                // its for filter OfficeId 
                int OfficeId = 0;

                int.TryParse(Request.Params["ddFilterOffice"], out OfficeId);
                if (Request["FilterActiveStatus"] != null && Request["FilterActiveStatus"] != "")
                {
                    IsActiveStatus = Request["FilterActiveStatus"];
                }

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "FirstName";
                }else  if (sortColumnIndex == 1)
                {
                    sortOrder = "FirstName";
                }
                else if (sortColumnIndex == 2)
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
                    sortOrder = "OfficeName";
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
                //    sortOrder = "OfficeName";
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

                // int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                SchedulerServiceProxy SchedulerLiteService = new SchedulerServiceProxy();
                SchedulersList = SchedulerLiteService.GetAllSchedulers(LoginUserId, pageNo, recordPerPage, search, sortOrder, sortDirection, OfficeId.ToString(), OrganisationId, IsActiveStatus).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "GetSchedulerList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (SchedulersList.SchedulerList != null)
            {
                var result = from C in SchedulersList.SchedulerList select new[] { C, C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = SchedulersList.TotalNumberofRecord,
                    iTotalDisplayRecords = SchedulersList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = SchedulersList.TotalNumberofRecord,
                    iTotalDisplayRecords = SchedulersList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddScheduler()
        {
            //  ServicesServiceProxy ServicesService = new ServicesServiceProxy();
            //ViewBag.ServiceList = ServicesService.GetAllServices().Result;

            return PartialView("AddScheduler");
        }

        //public string DeleteScheduler(string SchedulerId)
        //{
        //    string result = "";
        //    try
        //    {
        //        SchedulerServiceProxy CareGiverLiteService = new SchedulerServiceProxy();
        //        result = CareGiverLiteService.DeleteScheduler(SchedulerId, Membership.GetUser().ProviderUserKey.ToString()).Result;
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "CareGiverController";
        //        log.Methodname = "DeleteScheduler";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return result;
        //}

        public string DeleteScheduler(string SchedulerId, string UserName)
        {
            string result = "";
            try
            {

                MembershipUser mUser = Membership.GetUser(UserName);
                string SchedulerUserId = mUser.ProviderUserKey.ToString();
                Membership.DeleteUser(UserName, true);

                SchedulerServiceProxy CareGiverLiteService = new SchedulerServiceProxy();
                result = CareGiverLiteService.DeleteScheduler(SchedulerId, Membership.GetUser().ProviderUserKey.ToString()).Result;

                if (result == "Success")
                {
                    var QuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(SchedulerUserId).Result;

                    ChattingController ChattingController = new ChattingController();

                    // for delete user one to one chat dialog
                    ChattingController.DeleteOneToOneChatByUserId(SchedulerUserId);

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
                log.Methodname = "DeleteScheduler";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        [HttpPost]
        public ActionResult AddScheduler(SchedulerModel objSchedulerModel)
        {
            try
            {
                SchedulerServiceProxy SchedulerService = new SchedulerServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                //if (ModelState.IsValid)
                //{

                if (objSchedulerModel.SchedulerId == 0)
                {
                    MembershipCreateStatus status;
                    Membership.CreateUser(objSchedulerModel.Username, objSchedulerModel.Password, objSchedulerModel.Email, null, null, true, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(objSchedulerModel.Username, "Scheduler");
                    }
                }

                var Scheduler = new Scheduler();
                Scheduler.SchedulerId = objSchedulerModel.SchedulerId;
                Scheduler.UserId = Membership.GetUser(objSchedulerModel.Username).ProviderUserKey.ToString();
                Scheduler.FirstName = objSchedulerModel.FirstName;
                Scheduler.LastName = objSchedulerModel.LastName;
                Scheduler.Email = objSchedulerModel.Email;
                Scheduler.InsertUserId = InsertedUserID;
                Scheduler.UserName = objSchedulerModel.Username;
                Scheduler.Password = objSchedulerModel.Password;
                Scheduler.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);


                string result = SchedulerService.AddScheduler(Scheduler).Result;

                //if (objSchedulerModel.SchedulerId == 0)
                //{
                //    FormsAuthentication.SetAuthCookie(Membership.GetUser(new Guid(InsertedUserID)).UserName, false);
                //}
                //if (result == "Success")
                //{
                //    return RedirectToAction("Schedulers", "Schedulers", new { IsAdded = true });
                //}

                if (result == "Success")
                {
                    
                    ForgetPasswordServiceProxy ForgetPasswordService = new ForgetPasswordServiceProxy();
                    string result1 = ForgetPasswordService.EmailtoSetPassword(objSchedulerModel.Email).Result;

                    //ForgetPasswordService.ForgotPassword(objSchedulerModel.Email).Result;

                    Task.Run(async () => { await ChattingController.GenerateUserQuickBloxIdRestAPI(Scheduler.UserId, Scheduler.Email, 0, false, Scheduler.OrganisationId, OrganisationEmail); }).Wait();

                    TempData["Message"] = "Scheduler is added successfully.";
                    return RedirectToAction("Schedulers", "Schedulers", new { IsAdded = true });
                }

                //    }

                //  ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                //    ViewBag.ServiceList = ServicesService.GetAllServices().Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "[HttpPost] AddSchedulers";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }


        [HttpPost]
        public ActionResult EditScheduler(SchedulerModel objSchedulerModel)
        {
            try
            {
                SchedulerServiceProxy SchedulerService = new SchedulerServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                //if (ModelState.IsValid)
                //{

                //if (objSchedulerModel.SchedulerId == 0)
                //{
                //    MembershipCreateStatus status;
                //    Membership.CreateUser(objSchedulerModel.Username, objSchedulerModel.Password, objSchedulerModel.Email, null, null, true, out status);

                //    if (status == MembershipCreateStatus.Success)
                //    {
                //        Roles.AddUserToRole(objSchedulerModel.Username, "Scheduler");
                //    }
                //}

                //MembershipUser mUser = Membership.GetUser(objSchedulerModel.Username);
                //mUser.ChangePassword(mUser.ResetPassword(), objSchedulerModel.Password);

                var Scheduler = new Scheduler();
                Scheduler.SchedulerId = objSchedulerModel.SchedulerId;
                // Scheduler.UserId = Membership.GetUser(objSchedulerModel.Username).ProviderUserKey.ToString();
                Scheduler.FirstName = objSchedulerModel.FirstName;
                Scheduler.LastName = objSchedulerModel.LastName;
                Scheduler.Email = objSchedulerModel.Email;
                //Scheduler.Password = objSchedulerModel.Password;
                Scheduler.InsertUserId = InsertedUserID;


                //string result = SchedulerService.EditScheduler(Scheduler).Result;

                //if (result == "Success")
                //{
                //    return RedirectToAction("Schedulers", "Schedulers", new { IsAdded = true });
                //}

                string result = SchedulerService.EditScheduler(Scheduler).Result;
                if (result == "Success")
                {

                    ChattingController chattingController = new ChattingController();

                    // if (objSchedulerModel.OldEmail != objSchedulerModel.Email)

                    if (!string.Equals(objSchedulerModel.OldEmail, objSchedulerModel.Email, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(objSchedulerModel.OldEmail))
                    {
                        
                        Task.Run(async () => { await chattingController.ApiToChangeEmailToQuickblox(objSchedulerModel.OldEmail, objSchedulerModel.Email, objSchedulerModel.QuickBloxId); }).Wait();
                        //chattingController.ApiToChangeEmailToQuickblox(objSchedulerModel.OldEmail, objSchedulerModel.Email, objSchedulerModel.QuickBloxId);
                    }

                    TempData["Message"] = "Scheduler is updated successfully.";
                    return RedirectToAction("Schedulers", "Schedulers", new { IsAdded = true });
                }

                //int n;
                //bool isNumeric = int.TryParse(result, out n);
                //if (isNumeric)
                //{
                //    TempData["Message"] = "Scheduler is updated successfully.";
                //    return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    //ModelState.AddModelError("Error", result);
                //    return Json(new { Result = "Service " + result + " is already in use. You can not remove it. " }, JsonRequestBehavior.AllowGet);
                //}

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "[HttpPost] AddSchedulers";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }


        public ActionResult EditScheduler(string id)
        {
            SchedulerModel objModel = new SchedulerModel();
            try
            {
                objModel = GetSchedulerDetailById(id);

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


        public SchedulerModel GetSchedulerDetailById(string id)
        {
            SchedulerModel objScheduler = new SchedulerModel();
            try
            {
                SchedulerServiceProxy CareGiverLiteService = new SchedulerServiceProxy();
                Scheduler Scheduler = CareGiverLiteService.GetSchedulerDetailById(id).Result;
                objScheduler.SchedulerId = Scheduler.SchedulerId;
                objScheduler.FirstName = Scheduler.FirstName;
                objScheduler.LastName = Scheduler.LastName;
                objScheduler.Username = Scheduler.UserName;
                objScheduler.UserId = Scheduler.UserId;
                objScheduler.Email = Scheduler.Email;
                objScheduler.OldEmail = Scheduler.Email;
                objScheduler.Password = Scheduler.Password;
                objScheduler.IsActive = Scheduler.IsActive;
                objScheduler.QuickBloxId = Scheduler.QuickBloxId;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "GetCareGiverDetailById";
                log.Methodname = "EditCareGiver";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objScheduler;
        }

        //Started by Pinki on 20/09/2017
        #region Superadmin can assign multiple chat groups to scheduler
        public ActionResult AssignGroup(string UserId, string Name, string QuickBloxId, string Email)
        {
            ViewBag.UserId = UserId;
            ViewBag.Name = Name;
            ViewBag.QuickBloxId = QuickBloxId;
            ViewBag.Email = Email;
            ViewBag.UnassignedGroupList = GetAllGroupExceptAssignedGroupByUserId(UserId);
            ViewBag.AssignedGroupList = GetAllAssignedGroupByUserId(UserId);
            return PartialView("AssignGroupToScheduler");
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
                log.Pagename = "SchedulersController";
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
                log.Pagename = "SchedulersController";
                log.Methodname = "GetAllAssignedGroupByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return chattingGroups;
        }
        public string AssignGroupToScheduler(string ChattingGroupIds, string UserId, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var objDialogDetail = new Chatting();
                MembershipUser user = Membership.GetUser();
                //    var SchedulerEmail = user.Email;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {
                    SchedulerEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                }
                else
                {
                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }

              //  string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                var ChattingGroupId = ChattingGroupIds.Split(',');

                foreach (var groupId in ChattingGroupId)
                {
                    result = ChattingService.AssignGroupToUser(groupId, UserId).Result;
                    if (!string.IsNullOrEmpty(result))
                    {
                        objDialogDetail = ChattingService.GetDialogDetail(groupId).Result;

                        Task.Run(async () => { await AddSchedulerToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId); }).Wait();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "AssignGroupToScheduler";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        public JsonResult RemoveSchedulerFromGroupChat(string ChattingGroupId, string UserId, string QuickBloxId)
        {
            string result = "";
            var SchedulerEmail = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();
                // var SchedulerEmail = user.Email;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {
                    SchedulerEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                }
                else
                {
                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }


                Task.Run(async () => { await RemoveSchedulerFromDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId); }).Wait();
                result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "RemoveCaregiverFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            var lstChattingGroup = GetAllGroupExceptAssignedGroupByUserId(UserId);
            return Json(new { result, lstChattingGroup }, JsonRequestBehavior.AllowGet);
        }
        private async Task<int> AddSchedulerToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, string QBId)
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
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var response = await client.PostAsync("/session.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            //JObject json = JObject.Parse(result);
            var data = (JObject)JsonConvert.DeserializeObject(result);
            string token = data["session"]["token"].Value<string>();

            ////Sessoin Generated End

            //GetActivePatientRequest All Dialog Detail
            //var clientGetDialogId = new System.Net.Http.HttpClient();
            //clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
            //clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            //clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
            //var response1 = await clientGetDialogId.GetAsync("");
            //var result1 = response1.Content.ReadAsStringAsync().Result;
            //// new 
            //var MyData = JsonConvert.DeserializeObject<QuickbloxReponse>(result1);
            //var tempOccupants_ids = new List<int>();

            //var loopCnt = Math.Floor(Convert.ToDecimal(MyData.total_entries / 100) + 1);
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
            //// if() //if occupants_ids not match then Call to Add in group
            //bool flag = false;
            //for (int k = 0; k < tempOccupants_ids.Count; k++)
            //{
            //    if (Convert.ToInt32(QBId) == tempOccupants_ids[k])
            //    {
            //        flag = true;
            //        break;
            //    }
            //}
            ////Add Member to group
            //if (flag == false)
            //{

            var objAddDialog = new AddDialog();
            List<int> objoccupants_ids = new List<int>();
            objoccupants_ids.Add(Convert.ToInt32(QBId));
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
            //  }
            return 1;
        }
        private async Task<int> RemoveSchedulerFromDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, string QBId)
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
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var response = await client.PostAsync("/session.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            //JObject json = JObject.Parse(result);
            var data = (JObject)JsonConvert.DeserializeObject(result);
            string token = data["session"]["token"].Value<string>();

            ////Sessoin Generated End

            //GetActivePatientRequest All Dialog Detail
            //var clientGetDialogId = new System.Net.Http.HttpClient();

            //clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
            //clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            //clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
            //var response1 = await clientGetDialogId.GetAsync("");
            //var result1 = response1.Content.ReadAsStringAsync().Result;
            //// new 
            //var MyData = JsonConvert.DeserializeObject<QuickbloxReponse>(result1);
            //var tempOccupants_ids = new List<int>();

            //var loopCnt = Math.Floor(Convert.ToDecimal(MyData.total_entries / 100) + 1);
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


            ////if occupants_ids get match then Call to remove from group
            //bool flag = false;
            //for (int k = 0; k < tempOccupants_ids.Count; k++)
            //{
            //    if (Convert.ToInt32(QBId) == tempOccupants_ids[k])
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
            objoccupants_ids.Add(Convert.ToInt32(QBId));
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
            //  }
            //  #endregion

            return 1;

        }
        #endregion

        #region AssignOffice

        public ActionResult AssignOfficeToScheduler(string UserId, string Name, string Email)
        {
            ViewBag.UserId = UserId;
            ViewBag.Email = Email;
            ViewBag.Name = Name;

            MembershipUser user = Membership.GetUser();
            string LogInUserId = user.ProviderUserKey.ToString();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            if (OrganisationId > 0)
            {
                ViewBag.UnassignedOfficeList = GetAllOrganisationOfficeExceptAssignedOfficeByUserId(UserId, LogInUserId,OrganisationId);
                ViewBag.AssignedOfficeList = GetAllOrganisationAssignedOfficeByUserId(UserId,OrganisationId);
            }
            else
            {
                ViewBag.UnassignedOfficeList = GetAllOfficeExceptAssignedOfficeByUserId(UserId, LogInUserId);
                ViewBag.AssignedOfficeList = GetAllAssignedOfficeByUserId(UserId);
            }


            return PartialView("AssignOfficeToScheduler");
        }


        public List<Office> GetAllOfficeExceptAssignedOfficeByUserId(string UserId, string LoginUserId)
        {
            var OfficeList = new List<Office>();
            try
            {
                OfficeList = new OfficeServiceProxy().GetAllOfficeExceptAssignedOfficeByUserId(UserId, LoginUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "GetAllGroupExceptAssignedGroupByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return OfficeList;
        }


        public string AssignOfficeIdToSchedulers(string OfficeIds, string UserId)
        {
            string result = "";
            try
            {
                var OfficeService = new OfficeServiceProxy();

                var ChattingController = new ChattingController();

                var objDialogDetail = new Chatting();
                MembershipUser user = Membership.GetUser();
                var SchedulerEmail = user.Email;
                var LoginUserId = Session["UserId"];

                 int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                //  var RoleId = Roles.GetRolesForUser(UserId);

                var QuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(UserId).Result;

                var AllOfficeIds = OfficeIds.Split(',');

                foreach (var OfficeId in AllOfficeIds)
                {
                    result = OfficeService.AssignOfficeToUser(OfficeId.ToString(), UserId, LoginUserId.ToString()).Result;

                    // for add memeber in Office Chat group
                    ChattingController.AddMemberIntoOfficeGroup(OfficeId.ToString(), UserId, QuickBloxId,OrganisationId,OrganisationEmail);

                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "AssignOfficeIdToSchedulers";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        public List<Office> GetAllAssignedOfficeByUserId(string UserId)
        {
            var Offices = new List<Office>();
            try
            {
                var OfficeService = new OfficeServiceProxy();
                Offices = new OfficeServiceProxy().GetAllAssignedOfficeByUserId(UserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "GetAllAssignedGroupByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Offices;
        }


        public JsonResult RemoveSchedulerFromOffice(string OfficeId, string UserId)
        {
            string result = "";

            MembershipUser user = Membership.GetUser();
            var ChattingController = new ChattingController();
            string LogInUserId = user.ProviderUserKey.ToString();
            try
            {
                var OfficeService = new OfficeServiceProxy();

                var objOfficeDetail = new Office();

                var ChattingService = new ChattingServiceProxy();
                ChattingsList ChattingsList = new ChattingsList();

                 int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);


                var QuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(UserId).Result;

                result = OfficeService.RemoveSchedulerFromOffice(OfficeId, UserId).Result;

                // for remove memeber from Office Chat group
                ChattingController.RemoveMemberFromOfficeGroup(OfficeId.ToString(), UserId, QuickBloxId,OrganisationId,OrganisationEmail);

                // remove user from all old office group
                List<Chatting> listGroupChatting = ChattingService.GetChatGroupListByOfficeIdForUser(UserId, "2", OfficeId.ToString()).Result;
                ChattingsList.objChattingsList = listGroupChatting;
                foreach (var item in listGroupChatting)
                {
                    ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, UserId, QuickBloxId,OrganisationId,OrganisationEmail);
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "RemoveCaregiverFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            var lstOffice = GetAllOfficeExceptAssignedOfficeByUserId(UserId, LogInUserId);
            return Json(new { result, lstOffice }, JsonRequestBehavior.AllowGet);
        }

        public List<Office> GetAllOrganisationOfficeExceptAssignedOfficeByUserId(string UserId, string LoginUserId, int OrganisationId)
        {
            var OfficesList = new List<Office>();
            string result = "";
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllOrganisationOfficeExceptAssignedOfficeByUserId",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllOrganisationOfficeExceptAssignedOfficeByUserId",
                                                  Guid.Parse(UserId),
                                                   Guid.Parse(LoginUserId),
                                                   Convert.ToInt32(OrganisationId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objOffice = new Office();

                        objOffice.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOffice.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                        OfficesList.Add(objOffice);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverWebLiteService";
                objErrorlog.Methodname = "GetAllOrganisatioOfficeExceptAssignedOfficeByUserId";
               // result = InsertErrorLog(objErrorlog);
            }
            return OfficesList;
        }

        public List<Office> GetAllOrganisationAssignedOfficeByUserId(string UserId, int OrganisationId)
        {
            var objOfficeList = new List<Office>();
            string result = "";
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllOrganisationAssignedOfficeByUserId",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllOrganisationAssignedOfficeByUserId",
                                                  Guid.Parse(UserId),
                                                  Convert.ToInt32(OrganisationId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objOfficeDetail = new Office();

                        objOfficeDetail.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOfficeDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                        objOfficeList.Add(objOfficeDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverWebLiteService";
                objErrorlog.Methodname = "GetAllOrganisationAssignedOfficeByUserId";
                //result = InsertErrorLog(objErrorlog);
            }
            return objOfficeList;
        }


        #endregion
    }
}