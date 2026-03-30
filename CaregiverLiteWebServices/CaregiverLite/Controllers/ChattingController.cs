using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using CaregiverLiteWCF.Class;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using DifferenzLibrary;
using System.Web;

namespace CaregiverLite.Controllers
{
   [SessionExpire]
    public class ChattingController : Controller
    {
        // GET: Chatting
        public ActionResult Chatting()
        {
            //ViewBag.OfficeDropDownList = OfficeController.GetOfficeDropDownList();
            FillAllOffices();
            return View();
        }

        //Modified By Pinki on 11th Sept,2017
        public ActionResult GetChattingList(JQueryDataTableParamModel param)
        {

            FillAllOffices();
            //ViewBag.OfficeDropDownList = OfficeController.GetOfficeDropDownList();


            ChattingsList ChattingsList = new ChattingsList();
            CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
            try
            {
                //
                string OfficeId = "0";


                if (Request["OfficeId"] != null && Request["OfficeId"] != "")
                {
                    OfficeId = Request["OfficeId"];

                    if (OfficeId == "0" || OfficeId.Length == 0)
                    {
                        OfficeId = "0";
                    }
                }
                //


                MembershipUser user = Membership.GetUser();
                string[] roles = Roles.GetRolesForUser(user.UserName);
                string LogInUserId = user.ProviderUserKey.ToString();
                ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
                //if (roles.Length > 0 && roles[0] == "Nurse")
                //{
                //    #region Chatting list for Caregiver
                //    CareGivers obj = CareGiverLiteService.GetCareGiverDetailsByUserId(LogInUserId).Result;

                //    List<ScheduleInfo> lstSchedulers = ChattingLiteService.GetAllSchedulerbyCaregiverId(obj).Result;

                //    List<Chatting> listChatting = lstSchedulers.ConvertAll(x => new Chatting
                //    {
                //        NurseId = x.SchedulerId,
                //        CareGiverName = x.FirstName + ' ' + x.LastName,
                //        QuickBloxDialogId = x.QBDialogId,
                //    });
                //    ChattingsList.objChattingsList = listChatting;
                //    #endregion
                //}
                //else
                //{
                #region Chatting List for Others/SuperAdmin/Scheduler
                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "Name";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "Name";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "Name";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "Name";
                }
                //else if (sortColumnIndex == 3)
                //{
                //    sortOrder = "CompletedReqCount";
                //}
                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "TotalRewardPoint";
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

                ChattingsList = ChattingLiteService.GetAllChattingList(LogInUserId, pageNo, recordPerPage, search, sortOrder, sortDirection, OfficeId).Result;
                #endregion
                //}

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetChattingList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("Login", "Account");
            }
            if (ChattingsList.objChattingsList != null)
            {
                //  var result = from C in ChattingsList.objChattingsList select new[] { C, C, C, C, C, C, C, C, C };
                var result = ChattingsList.objChattingsList;
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ChattingsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ChattingsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ChattingsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ChattingsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //Modified By Pinki on 12th Sept,2017
        public ActionResult GetChattingListPatientGroupWise(JQueryDataTableParamModel param)
        {
            //ViewBag.OfficeDropDownList = OfficeController.GetOfficeDropDownList();
            FillAllOffices();
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                MembershipUser user = Membership.GetUser();
                string[] roles = Roles.GetRolesForUser(user.UserName);
                string LogInUserId = user.ProviderUserKey.ToString();
                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();

                if (roles.Length > 0 && roles[0] == "Nurse")
                {
                    #region Group Chatting list for Caregiver

                    List<Chatting> listChatting = ChattingLiteService.GetPatientChattingGroupList(LogInUserId).Result;
                    ChattingsList.objChattingsList = listChatting;

                    #endregion
                }
                else
                {
                    #region Group Chatting list for Others/SuperAdmin/Scheduler

                    string sortOrder = string.Empty;
                    var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                    if (sortColumnIndex == 0)
                        sortOrder = "GroupName";
                    else if (sortColumnIndex == 1)
                        sortOrder = "GroupName";
                    else if (sortColumnIndex == 2)
                        sortOrder = "GroupName";
                    else if (sortColumnIndex == 3)
                        sortOrder = "GroupName";
                    //else if (sortColumnIndex == 3)
                    //{
                    //    sortOrder = "CompletedReqCount";
                    //}
                    //else if (sortColumnIndex == 4)
                    //{
                    //    sortOrder = "TotalRewardPoint";
                    //}
                    string search = "||"; //It's indicate blank filter
                    if (!string.IsNullOrEmpty(param.sSearch))
                        search = param.sSearch;
                    var sortDirection = Request["sSortDir_0"]; // asc or desc
                    int pageNo = 1;
                    int recordPerPage = param.iDisplayLength;
                    //Find page number from the logic
                    if (param.iDisplayStart > 0)
                        pageNo = (param.iDisplayStart / recordPerPage) + 1;
                    ChattingsList = ChattingLiteService.GetChattingListPatientGroupWise(LogInUserId, pageNo, recordPerPage, search, sortOrder, sortDirection, "1").Result;
                    #endregion
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetChattingListPatientGroupWise";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("Login", "Account");
            }
            if (ChattingsList.objChattingsList != null)
            {
                //   var result = from C in ChattingsList.objChattingsList select new[] { C, C, C, C, C, C, C, C, C };
                var result = ChattingsList.objChattingsList;
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ChattingsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ChattingsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ChattingsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ChattingsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }



        //OpenChatWindow
        public ActionResult OpenChatWindow(int Id, string UserId)
        {
            Session["FromQBId"] = null;
            Session["ToQBId"] = null;
            Session["ToQBUserName"] = null;
            Session["chatDialogId"] = null;

            var LoginUserQuickBloxId = "";
            var ToUserQuickBloxId = "";
            var LoginUserUserId = "";
            var ToUserUserId = "";
            var ToUserEmail = "";
            var FromUserEmail = "";
            var objGroupDetail = new Chatting();

            ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
            Chatting objChattingToUser = new Chatting();
            try
            {
                int IsAdmin = 0;
                string sortOrder = string.Empty;
                string LoginRole = string.Empty;
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();
                LoginUserUserId = LogInUserId;

                string[] roles = Roles.GetRolesForUser(user.UserName);
                foreach (string role in roles)
                {
                    LoginRole = role;
                    if (role == "SuperAdmin")
                    {
                        Session["IsSuperAdmin"] = "true";
                        Session["UserId"] = user.ProviderUserKey.ToString();
                        //LogInUserId = Session["UserId"];
                        IsAdmin = 1;
                    }
                }

                LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxDetByUserId(LogInUserId).Result.QuickBloxId;

                objChattingToUser = ChattingLiteService.GetQuickBloxDetByUserId(UserId).Result;
                // objChattingToUser = ChattingLiteService.GetQuickBloxDetById(Id).Result;

                ToUserQuickBloxId = objChattingToUser.QuickBloxId;
                ToUserUserId = objChattingToUser.UserId;
                ToUserEmail = objChattingToUser.ToEmail;

                Session["FromQBId"] = LoginUserQuickBloxId ?? "0";
                Session["ToQBId"] = ToUserQuickBloxId ?? "0";

                #region Commented
                //string s = ChattingLiteService.GetQuickBloxIdByNurseId(Id).Result;
                //char d = ',';
                //String[] s1 = s.Split(d);
                //ToUserQuickBloxId = s1[0];
                //ToUserUserId = s1[2];
                //ToUserEmail = s1[3];

                //Session["ToQBUserName"] = s1[4];
                //ViewBag.CareGiverName = s1[4];
                //ViewBag.ToQBId = ToUserQuickBloxId;
                //ViewBag.ToQBUserName = s1[1]; 
                #endregion

                Session["ToQBUserName"] = objChattingToUser.CareGiverName;
                ViewBag.CareGiverName = objChattingToUser.CareGiverName;
                ViewBag.ToQBId = ToUserQuickBloxId;
                ViewBag.ToQBUserName = objChattingToUser.CareGiverName;

                Session["LoginUserUserId"] = LoginUserUserId;
                Session["ToUserUserId"] = ToUserUserId;
                Session["ToUserEmail"] = ToUserEmail;
                Session["FromUserEmail"] = user.Email;
                Session["chatDialogId"] = GetDialogId(LoginUserUserId, ToUserUserId);

                if (LoginRole == "SuperAdmin")
                {
                    LoginRole = "Scheduler";
                }
                else if (LoginRole == "Nurse")
                {
                    LoginRole = "CareGiver";
                }
                //else if (LoginRole == "NurseCoordinator")
                //{
                //    LoginRole = "Supervisor/Support";
                //}

                Session["LoginRole"] = LoginRole;
                // Session["chatDialogId"] = roles[0] == "Nurse" ? GetDialogId(LoginUserUserId, ToUserUserId) : GetDialogId(ToUserUserId, LoginUserUserId);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "OpenChatWindow";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public string SaveQBId(string UserId, string QuickBloxId)
        {
            string result = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.SaveQBId(UserId, QuickBloxId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "SaveQBId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        public string SaveDialogId(string NurseUserId, string SchedulerUserId, string DialogId, string UserType)
        {
            string result = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.SaveDialogId(NurseUserId, SchedulerUserId, DialogId, UserType).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "SaveDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        public string GetDialogId(string NurseUserId, string SchedulerUserId)
        {
            string DialogId = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();
                DialogId = ChattingService.GetDialogId(NurseUserId, SchedulerUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return DialogId;
        }

        public Chatting GetDialogDetail(string ID)
        {
            var objDialogDetail = new Chatting();
            try
            {

                var ChattingService = new ChattingServiceProxy();
                objDialogDetail = ChattingService.GetDialogDetail(ID).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetDialogDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objDialogDetail;
        }

        public List<ChattingGroupMember> GetGroupMemberDetail(string ID, string OrganisationId)
        {
            var objGroupMemberDetailListing = new List<ChattingGroupMember>();
            try
            {

                var ChattingService = new ChattingServiceProxy();
                objGroupMemberDetailListing = ChattingService.GetGroupMemberDetail(ID,OrganisationId).Result;


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetGroupMemberDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objGroupMemberDetailListing;
        }

        [HttpPost]
        public JsonResult GetAllDialogId()
        {
            string DialogId = "";
            var objChattingDialogList = new ChattingsList();
            //List<CareGivers> CareGiversList = new List<CareGivers>();
            try
            {
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                var ChattingService = new ChattingServiceProxy();
                objChattingDialogList = ChattingService.GetAllDialogId(LogInUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetAllDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(objChattingDialogList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllGroupDialogId()
        {
            string DialogId = "";
            var objGroupChattingDialogList = new ChattingsList();
            //List<CareGivers> CareGiversList = new List<CareGivers>();
            try
            {
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                var ChattingService = new ChattingServiceProxy();
                objGroupChattingDialogList = ChattingService.GetAllGroupDialogId(LogInUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetAllGroupDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(objGroupChattingDialogList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AttachmentDemo()
        {
            return View();
        }

        public ActionResult PatientGroupChatting()
        {
            //ViewBag.OfficeDropDownList = OfficeController.GetOfficeDropDownList();
            FillAllOffices();
            return View();
        }


        //public string AddGroupDialogId(string DialogId, string GroupName, string UserId, int OfficeId)
        //{
        //    string result = "";
        //    try
        //    {
        //        //UAT
        //        //      string SuperAdminUserId = "D59FF81A-5F78-4D23-BE3B-C8630313E2BB";

        //        string SuperAdminUserId = System.Configuration.ConfigurationManager.AppSettings["SuperAdminUserId"];

        //        //Live & Local
        //        //   string SuperAdminUserId = "D59FF81A-5F78-4D23-BE3B-C8630313E2BA";

        //        var ChattingService = new ChattingServiceProxy();

        //        var GroupChat = new GroupChat();

        //        GroupChat.GroupDialogId = DialogId;
        //        GroupChat.GroupName = GroupName;
        //        GroupChat.LogInUserId = SuperAdminUserId;
        //        GroupChat.OfficeId = OfficeId;
        //        GroupChat.GroupTypeID = 1;

        //        var Chatting = new Chatting();

        //        Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;
        //        //   result = ChattingService.AddGroupDialogId(DialogId, GroupName, UserId, OfficeId.ToString()).Result;

        //        if (Chatting.Result == "Success")
        //        {
        //            MembershipUser user = Membership.GetUser();
        //            string[] roles = Roles.GetRolesForUser(user.UserName);
        //            string LogInUserId = user.ProviderUserKey.ToString();

        //            //if (roles.Length > 0 && roles[0] == "SuperAdmin" || roles[0] == "Scheduler")
        //            //{
        //            OfficeModel objModel = new OfficeModel();

        //            objModel = OfficeController.GetOfficeDetailByOfficeId(OfficeId.ToString());

        //            // string UserIds = "4D51E02D-D16E-4962-A01B-97624A683A64" + ',' + objModel.AdminUserId;

        //            //  string QBIds = "32168516" + ',' + objModel.AdminQuickBloxId;
        //            //   var data =  GetGroupDetailFromGroupName(GroupName);

        //            string UserIds = objModel.AdminUserId;
        //            string QBIds = objModel.AdminQuickBloxId;

        //            var SchedulerList = new List<ScheduleInfo>();

        //            SchedulerList = ChattingService.GetALLSuperadminList().Result;

        //            foreach (var item in SchedulerList)
        //            {
        //                UserIds = UserIds + ',' + item.UserId;

        //                QBIds = QBIds + ',' + item.QuickbloxId;
        //            }

        //            var objDialogDetail = new Chatting();
        //            objDialogDetail = ChattingService.GetGroupDetailFromGroupName(GroupName).Result;

        //            AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), UserIds, QBIds);

        //            if (roles.Length > 0 && roles[0] == "Scheduler")
        //            {
        //                var SchedulerQBID = Session["FromQBId"];
        //                AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, SchedulerQBID.ToString());
        //            }
        //            //   }

        //        }


        //        // check role & result 
        //        // get group ID from  Group name
        //        // get ADMIN userId & QuickboxId from OfficeID
        //        //     AddMemberIntoGroup()


        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "ChattingController";
        //        log.Methodname = "AddGroupDialogId";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return result;
        //}



        //testing for Adding Schedular by default

        public string AddGroupDialogId(string DialogId, string GroupName, string UserId, int OfficeId)
        {
            
            string result = "";
            try
            {

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if(OrganisationId>0)
                {

                    string SuperAdminUserId1 = Session["OrgSuperAdminUserId"].ToString();

                        //System.Configuration.ConfigurationManager.AppSettings["SuperOrgAdminUserId"];


                    GroupChat GroupChat1 = new GroupChat();

                    GroupChat1.GroupDialogId = DialogId;
                    GroupChat1.GroupName = GroupName;
                    GroupChat1.LogInUserId = SuperAdminUserId1;
                    GroupChat1.OfficeId = OfficeId;
                    GroupChat1.GroupTypeID = 1;
                    GroupChat1.OrganisationId = OrganisationId;


                    result = AddOrgGroupDialogId(GroupChat1);

                    return result;

                }

                //UAT
                //string SuperAdminUserId = "6b186dd2-b28c-4baa-8ff7-f68b7e6258fa";

                string SuperAdminUserId = System.Configuration.ConfigurationManager.AppSettings["SuperAdminUserId"];

                //Live & Local
                //   string SuperAdminUserId = "D59FF81A-5F78-4D23-BE3B-C8630313E2BA";

                var ChattingService = new ChattingServiceProxy();

                var GroupChat = new GroupChat();

                GroupChat.GroupDialogId = DialogId;
                GroupChat.GroupName = GroupName;
                GroupChat.LogInUserId = SuperAdminUserId;
                GroupChat.OfficeId = OfficeId;
                GroupChat.GroupTypeID = 1;

                var Chatting = new Chatting();

                Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;
                //   result = ChattingService.AddGroupDialogId(DialogId, GroupName, UserId, OfficeId.ToString()).Result;

                if (Chatting.Result == "Success")
                {
                    MembershipUser user = Membership.GetUser();
                    string[] roles = Roles.GetRolesForUser(user.UserName);
                    string LogInUserId = user.ProviderUserKey.ToString();

                    // string LogInUserId = "6b186dd2-b28c-4baa-8ff7-f68b7e6258fa";


                    //if (roles.Length > 0 && roles[0] == "SuperAdmin" || roles[0] == "Scheduler")
                    //{

                    OfficeModel objModel = new OfficeModel();

                    objModel = OfficeController.GetOfficeDetailByOfficeId(OfficeId.ToString());

                    // string UserIds = "4D51E02D-D16E-4962-A01B-97624A683A64" + ',' + objModel.AdminUserId;

                    //  string QBIds = "32168516" + ',' + objModel.AdminQuickBloxId;
                    //   var data =  GetGroupDetailFromGroupName(GroupName);

                    string UserIds = objModel.AdminUserId;
                    string QBIds = objModel.AdminQuickBloxId;

                    var SchedulerList = new List<ScheduleInfo>();

                    SchedulerList = ChattingService.GetALLSuperadminList().Result;

                    foreach (var item in SchedulerList)
                    {
                        UserIds = UserIds + ',' + item.UserId;

                        QBIds = QBIds + ',' + item.QuickbloxId;
                    }
       
                    var objDialogDetail = new Chatting();
                    objDialogDetail = ChattingService.GetGroupDetailFromGroupName(GroupName).Result;

                    AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), UserIds, QBIds);

                    if (roles.Length > 0 && roles[0] == "Scheduler")
                    {
                        var SchedulerQBID = Session["FromQBId"];
                        AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, SchedulerQBID.ToString());
                    }
                    //else
                    //{

                        if (OfficeId.ToString() == "14" || OfficeId.ToString() == "6" || OfficeId.ToString() == "12" || OfficeId.ToString() == "20" || OfficeId.ToString() == "5")
                        {

                            if (LogInUserId != "441E7AA9-078E-46E6-A8C1-6E7D5A5B71C9" || LogInUserId != "B68C33BB-088B-4338-9FC5-3B3F30A2D76B")
                            {
                                ScheduleInfo schedulerInfo = GetSchedularDetailsAddedTochatRoom(OfficeId.ToString());
                                AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, schedulerInfo.QuickbloxId.ToString());
                            }
                       }          
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddGroupDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        //public string AddOrganisationGroupDialogIdss(string DialogId, string GroupName, string UserId, int OfficeId)
       public string AddOrgGroupDialogId(GroupChat GroupChat1)
        {

            string result = "";
            try
            {

                int  OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                string SuperAdminUserId = Session["OrgSuperAdminUserId"].ToString();
                    //System.Configuration.ConfigurationManager.AppSettings["SuperOrgAdminUserId"];


                var ChattingService = new ChattingServiceProxy();

                var GroupChat = new GroupChat();

                GroupChat.GroupDialogId = GroupChat1.GroupDialogId;
                GroupChat.GroupName = GroupChat1.GroupName;
                GroupChat.LogInUserId = SuperAdminUserId;
                GroupChat.OfficeId = GroupChat1.OfficeId;
                GroupChat.GroupTypeID = 1;
                GroupChat.OrganisationId = OrganisationId;


                var Chatting = new Chatting();

                // Chatting = ChattingService.AddOrganisationGroupDialogId(GroupChat).Result;

                Chatting = AddOrganisationGroupDialogIds(GroupChat);

                //   result = ChattingService.AddGroupDialogId(DialogId, GroupName, UserId, OfficeId.ToString()).Result;

                if (Chatting.Result == "Success")
                {
                    MembershipUser user = Membership.GetUser();
                    string[] roles = Roles.GetRolesForUser(user.UserName);
                    string LogInUserId = user.ProviderUserKey.ToString();

                    // string LogInUserId = "6b186dd2-b28c-4baa-8ff7-f68b7e6258fa";


                    //if (roles.Length > 0 && roles[0] == "SuperAdmin" || roles[0] == "Scheduler")
                    //{

                    OfficeModel objModel = new OfficeModel();

                    objModel = OfficeController.GetOrganisationOfficeDetailByOfficeId(GroupChat1.OfficeId.ToString(), OrganisationId.ToString());

                    // string UserIds = "4D51E02D-D16E-4962-A01B-97624A683A64" + ',' + objModel.AdminUserId;

                    //  string QBIds = "32168516" + ',' + objModel.AdminQuickBloxId;
                    //   var data =  GetGroupDetailFromGroupName(GroupName);

                    string UserIds = objModel.AdminUserId;
                    string QBIds = objModel.AdminQuickBloxId;

                    var SchedulerList = new List<ScheduleInfo>();

                    SchedulerList = GetALLSuperadminListOrganisationBased(GroupChat.OrganisationId);
                        //.Result;

                    foreach (var item in SchedulerList)
                    {
                        UserIds = UserIds + ',' + item.UserId;

                        QBIds = QBIds + ',' + item.QuickbloxId;
                    }

                    var objDialogDetail = new Chatting();
                    objDialogDetail = ChattingService.GetGroupDetailFromGroupName(GroupChat1.GroupName).Result;

                    AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), UserIds, QBIds);

                    if (roles.Length > 0 && roles[0] == "Scheduler")
                    {
                        if (OrganisationId > 0)
                        {
                            var SchedulerQBID = Session["FromQBId"];
                            AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, SchedulerQBID.ToString());
                        }
                    }

                    //else
                    //{
                    //if (OfficeId.ToString() == "14" || OfficeId.ToString() == "6" || OfficeId.ToString() == "12" || OfficeId.ToString() == "20" || OfficeId.ToString() == "5")
                    //{
                    //    if (LogInUserId != "441E7AA9-078E-46E6-A8C1-6E7D5A5B71C9" || LogInUserId != "B68C33BB-088B-4338-9FC5-3B3F30A2D76B")
                    //    {
                    //        ScheduleInfo schedulerInfo = GetSchedularDetailsAddedTochatRoom(OfficeId.ToString());
                    //        AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, schedulerInfo.QuickbloxId.ToString());
                    //    }
                    //}

                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddGroupDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        public List<ScheduleInfo> GetALLSuperadminListOrganisationBased(int OrganisationId)
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetALLSuperadminListOrganisationBased",Convert.ToInt32(OrganisationId));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ScheduleInfo objSchedule = new ScheduleInfo();
                        objSchedule.SchedulerId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdminId"]);
                        objSchedule.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objSchedule.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objSchedule.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objSchedule.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objSchedule.QuickbloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        SchedulerList.Add(objSchedule);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetALLSuperadminList";
                string result = InsertErrorLog(objErrorlog);
            }
            return SchedulerList;
        }



        public Chatting AddOrganisationGroupDialogIds(GroupChat GroupChat)
        {
            string result = "";
            var objDialogDetail = new Chatting();

            try
            {
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddOrganisationGroupDialogId",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_AddOrganisationGroupDialogId",
                                                    GroupChat.GroupDialogId,
                                                    GroupChat.GroupName,
                                                    GroupChat.GroupSubject,
                                                    Convert.ToString(GroupChat.LogInUserId),
                                                    GroupChat.GroupTypeID,
                                                    GroupChat.OfficeId,
                                                    GroupChat.OrganisationId     
                                                    );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    objDialogDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChattingGroupId"]);
                    objDialogDetail.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    objDialogDetail.DialogId = ds.Tables[0].Rows[0]["DialogId"].ToString();

                    objDialogDetail.Result = "Success";

                }

                //    if (i > 0)
                //{
                //    result = "Success";
                //}

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddOrganisationGroupDialogId";
                result = InsertErrorLog(objErrorlog);
            }
            return objDialogDetail;
        }


        private ScheduleInfo GetSchedularDetailsAddedTochatRoom(string OfficeId)
        {
            string result = "";

            ScheduleInfo schedulerinfo = new ScheduleInfo();

            DataSet ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetSchedulerOfficeDetailByOfficeId", OfficeId);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               // schedulerinfo.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["officeId"]);
                schedulerinfo.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
               // schedulerinfo.OfficeName = ds.Tables[0].Rows[0]["officeName"].ToString();
                //schedulerinfo.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                //Office.City = ds.Tables[0].Rows[0]["City"].ToString();
                //Office.State = ds.Tables[0].Rows[0]["State"].ToString();
                //Office.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                schedulerinfo.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                schedulerinfo.QuickbloxId = ds.Tables[0].Rows[0]["QuickbloxId"].ToString();


            }


            return schedulerinfo;
        }
        
         
        //public string AddGroupDialogId(string DialogId, string GroupName, string UserId, int OfficeId)
        //{
        //    string result = "";
        //    try
        //    {
        //        //UAT
        //        //      string SuperAdminUserId = "D59FF81A-5F78-4D23-BE3B-C8630313E2BB";

        //        string SuperAdminUserId = System.Configuration.ConfigurationManager.AppSettings["SuperAdminUserId"];

        //        //Live & Local
        //        //   string SuperAdminUserId = "D59FF81A-5F78-4D23-BE3B-C8630313E2BA";

        //        var ChattingService = new ChattingServiceProxy();

        //        var GroupChat = new GroupChat();

        //        GroupChat.GroupDialogId = DialogId;
        //        GroupChat.GroupName = GroupName;
        //        GroupChat.LogInUserId = SuperAdminUserId;
        //        GroupChat.OfficeId = OfficeId;
        //        GroupChat.GroupTypeID = 1;

        //        var Chatting = new Chatting();

        //        Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;
        //        //   result = ChattingService.AddGroupDialogId(DialogId, GroupName, UserId, OfficeId.ToString()).Result;

        //        if (Chatting.Result == "Success")
        //        {
        //            MembershipUser user = Membership.GetUser();
        //            string[] roles = Roles.GetRolesForUser(user.UserName);
        //            string LogInUserId = user.ProviderUserKey.ToString();

        //            //if (roles.Length > 0 && roles[0] == "SuperAdmin" || roles[0] == "Scheduler")
        //            //{
        //            OfficeModel objModel = new OfficeModel();

        //            objModel = OfficeController.GetOfficeDetailByOfficeId(OfficeId.ToString());

        //            // string UserIds = "4D51E02D-D16E-4962-A01B-97624A683A64" + ',' + objModel.AdminUserId;

        //            //  string QBIds = "32168516" + ',' + objModel.AdminQuickBloxId;
        //            //   var data =  GetGroupDetailFromGroupName(GroupName);

        //            string UserIds = objModel.AdminUserId;
        //            string QBIds = objModel.AdminQuickBloxId;

        //            var SchedulerList = new List<ScheduleInfo>();

        //            SchedulerList = ChattingService.GetALLSuperadminList().Result;



        //            foreach (var item in SchedulerList)
        //            {
        //                UserIds = UserIds + ',' + item.UserId;

        //                QBIds = QBIds + ',' + item.QuickbloxId;
        //            }

        //            var objDialogDetail = new Chatting();
        //            objDialogDetail = ChattingService.GetGroupDetailFromGroupName(GroupName).Result;

        //          //  AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), UserIds, QBIds);



        //            var SchedulerInfoList = new List<SchedulerInfo>();

        //            DataSet ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetSchedulersINFOList", OfficeId);
        //            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    SchedulerInfo objScheduler = new SchedulerInfo();
        //                    objScheduler.SchedulerId = Convert.ToInt32(ds.Tables[0].Rows[i]["SchedulerId"]);
        //                    objScheduler.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
        //                    objScheduler.Name = ds.Tables[0].Rows[i]["Name"].ToString();
        //                    objScheduler.Email = ds.Tables[0].Rows[i]["Email"].ToString();
        //                    objScheduler.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
        //                    objScheduler.QuickbloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
        //                    SchedulerInfoList.Add(objScheduler);
        //                }
        //            }

        //            foreach (var item1 in SchedulerInfoList)
        //            {
        //                // UserIdss = UserIdss + ',' + item1.UserId;

        //                // QBIdss = QBIdss + ',' + item1.QuickbloxId;

        //                UserIds = UserIds + ','+ item1.UserId;

        //                QBIds = QBIds + ',' + item1.QuickbloxId;
        //            }


        //            if (roles.Length > 0 && roles[0] == "Scheduler")
        //            {
        //                var SchedulerQBID = Session["FromQBId"];
        //                AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, SchedulerQBID.ToString());
        //            }
        //            else
        //            {
        //                AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), UserIds, QBIds);
        //                // AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), UserIdss, QBIdss);

        //            }




        //        }

        //        // check role & result 
        //        // get group ID from  Group name
        //        // get ADMIN userId & QuickboxId from OfficeID
        //        //     AddMemberIntoGroup()
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "ChattingController";
        //        log.Methodname = "AddGroupDialogId";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return result;
        //}




        //public ActionResult OpenGroupChatWindow(int Id, int? GroupType)
        //{
        //    int RestrictedGroupId = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupId"].ToString());
        //    string[] RestrictedGroupAccessIds = ConfigurationManager.AppSettings["RestrictedLoginUserUserId"].Split(',');

        //    string SessionUserId = Session["UserId"].ToString();

        //    // For Other Group Chat
        //    int RestrictedGroupIdForMA = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForMA"].ToString());
        //    string[] RestrictedGroupAccessIdsForMA = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForMA"].Split(',');

        //    int RestrictedGroupIdForArizona = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForArizona"].ToString());
        //    string[] RestrictedGroupAccessIdsForArizona = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForArizona"].Split(',');

        //    int RestrictedGroupIdForCASD = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForCASD"].ToString());
        //    string[] RestrictedGroupAccessIdsForCASD = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForCASD"].Split(',');

        //    int RestrictedGroupIdForCAYC = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForCAYC"].ToString());
        //    string[] RestrictedGroupAccessIdsForCAYC = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForCAYC"].Split(',');

        //    int RestrictedGroupIdForFL = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForFL"].ToString());
        //    string[] RestrictedGroupAccessIdsForFL = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForFL"].Split(',');

        //    int RestrictedGroupIdForMN = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForMN"].ToString());
        //    string[] RestrictedGroupAccessIdsForMN = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForMN"].Split(',');

        //    int RestrictedGroupIdForNevada = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForNevada"].ToString());
        //    string[] RestrictedGroupAccessIdsForNevada = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForNevada"].Split(',');

        //    int RestrictedGroupIdForOH = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForOH"].ToString());
        //    string[] RestrictedGroupAccessIdsForOH = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForOH"].Split(',');

        //    int RestrictedGroupIdForIL = Convert.ToInt16(ConfigurationManager.AppSettings["RestrictedGroupIdForIL"].ToString());
        //    string[] RestrictedGroupAccessIdsForIL = ConfigurationManager.AppSettings["RestrictedGroupAccessIdsForIL"].Split(',');


        //    if (Id == RestrictedGroupId && !RestrictedGroupAccessIds.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    } 

        //    // For Other group restriction

        //    else if (Id == RestrictedGroupIdForMA && !RestrictedGroupAccessIdsForMA.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForArizona && !RestrictedGroupAccessIdsForArizona.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForCASD && !RestrictedGroupAccessIdsForCASD.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForCAYC && !RestrictedGroupAccessIdsForCAYC.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForFL && !RestrictedGroupAccessIdsForFL.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForMN && !RestrictedGroupAccessIdsForMN.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForNevada && !RestrictedGroupAccessIdsForNevada.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForOH && !RestrictedGroupAccessIdsForOH.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else if (Id == RestrictedGroupIdForIL && !RestrictedGroupAccessIdsForIL.Any(SessionUserId.Contains))
        //    {
        //        ViewBag.permissionMessage = "You are not authorized to view these group message.";
        //    }

        //    else
        //    {
        //        Session["FromQBId"] = null;
        //        Session["ToQBId"] = null;
        //        Session["ToQBUserName"] = null;
        //        Session["chatDialogId"] = null;

        //        if (GroupType == null)
        //        {
        //            ViewBag.GroupTypeId = 2;
        //        }
        //        else { ViewBag.GroupTypeId = GroupType; }

        //        var LoginUserQuickBloxId = "";
        //        var ToUserQuickBloxId = "";
        //        var LoginUserUserId = "";
        //        var ToUserUserId = "";
        //        var ToUserEmail = "";
        //        var FromUserEmail = "";
        //        var objGroupDetail = new Chatting();
        //        ViewBag.permission = "0";
        //        ViewBag.role = "";

        //        try
        //        {
        //            int IsAdmin = 0;
        //            string sortOrder = string.Empty;
        //            ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
        //            MembershipUser user = Membership.GetUser();
        //            string LogInUserId = user.ProviderUserKey.ToString();
        //            LoginUserUserId = LogInUserId;

        //            string[] roles = Roles.GetRolesForUser(user.UserName);
        //            foreach (string role in roles)
        //            {
        //                if (role == "SuperAdmin")
        //                {
        //                    Session["IsSuperAdmin"] = "true";
        //                    Session["UserId"] = user.ProviderUserKey.ToString();
        //                    //LogInUserId = Session["UserId"];
        //                    IsAdmin = 1;
        //                }
        //                else
        //                {
        //                    ViewBag.role = role;
        //                }
        //            }
        //            //LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxIdBySchedulerUserId(LogInUserId).Result.QuickBloxId;
        //            LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxDetByUserId(LogInUserId).Result.QuickBloxId;
        //            Session["FromQBId"] = LoginUserQuickBloxId ?? "0";
        //            Session["LoginUserUserId"] = LoginUserUserId;
        //            Session["FromUserEmail"] = user.Email;

        //            objGroupDetail = GetDialogDetail(Id.ToString());
        //            ViewBag.GroupName = objGroupDetail.GroupName;
        //            ViewBag.GroupSubject = objGroupDetail.GroupSubject;
        //            ViewBag.GroupDialog = objGroupDetail.DialogId;
        //            ViewBag.GroupDialogId = objGroupDetail.ChattingGroupId;
        //            ViewBag.ChattingGroupId = objGroupDetail.ChattingGroupId;
        //            Session["chatDialogId"] = objGroupDetail.DialogId;

        //            ViewBag.GroupMemberDetail = GetGroupMemberDetail(Id.ToString());
        //            //if (ViewBag.role == "NurseCoordinator")
        //            //{
        //            //    ViewBag.permission = GetNurseCoordinatorPermission(objGroupDetail.ChattingGroupId.ToString(), LoginUserUserId);
        //            //}
        //            ViewBag.permission = GetGroupChatMemberPermission(objGroupDetail.ChattingGroupId.ToString(), LoginUserUserId);

        //        }
        //        catch (Exception e)
        //        {
        //            ErrorLog log = new ErrorLog();
        //            log.Errormessage = e.Message;
        //            log.StackTrace = e.StackTrace;
        //            log.Pagename = "ChattingController";
        //            log.Methodname = "OpenGroupChatWindow";
        //            ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //            string res = ErrorLogService.InsertErrorLog(log).Result;
        //            return RedirectToAction("Login", "Account");
        //        }
        //    }
        //    return View();
        //}

        public ActionResult OpenGroupChatWindow(int Id, int? GroupType)
        {
            Session["FromQBId"] = null;
            Session["ToQBId"] = null;
            Session["ToQBUserName"] = null;
            Session["chatDialogId"] = null;

            if (GroupType == null)
            {
                ViewBag.GroupTypeId = 2;
            }
            else { ViewBag.GroupTypeId = GroupType; }

            var LoginUserQuickBloxId = "";
            var ToUserQuickBloxId = "";
            var LoginUserUserId = "";
            var ToUserUserId = "";
            var ToUserEmail = "";
            var FromUserEmail = "";
            var objGroupDetail = new Chatting();
            ViewBag.permission = "0";
            ViewBag.role = "";

            try
            {
                int IsAdmin = 0;
                string sortOrder = string.Empty;
                ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();
                LoginUserUserId = LogInUserId;

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
                    else
                    {
                        ViewBag.role = role;
                    }
                }

                //LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxIdBySchedulerUserId(LogInUserId).Result.QuickBloxId;
                LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxDetByUserId(LogInUserId).Result.QuickBloxId;
                Session["FromQBId"] = LoginUserQuickBloxId ?? "0";
                Session["LoginUserUserId"] = LoginUserUserId;
                Session["FromUserEmail"] = user.Email;

                objGroupDetail = GetDialogDetail(Id.ToString());
                ViewBag.GroupName = objGroupDetail.GroupName;

                Session["patientgroupname"] = objGroupDetail.GroupName;

                ViewBag.GroupSubject = objGroupDetail.GroupSubject;
                ViewBag.GroupDialog = objGroupDetail.DialogId;
                ViewBag.GroupDialogId = objGroupDetail.ChattingGroupId;
                ViewBag.ChattingGroupId = objGroupDetail.ChattingGroupId;

                Session["chattingGroupId"] = objGroupDetail.ChattingGroupId;

                //code from here
                string ChattingGroupIds = Convert.ToString(Session["chattingGroupId"]);
                List<OfficeModel> listsOffices = GetAddressAndOfficename(ChattingGroupIds);
                ViewBag.listoffclist = listsOffices;

                foreach (var offceslist in listsOffices)
                {
                    Session["OfficeName"] = offceslist.OfficeName;
                    Session["OfficeAddress"] = offceslist.Address;
                }

                Session["chatDialogId"] = objGroupDetail.DialogId;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                ViewBag.OrganisationId = OrganisationId;

                ViewBag.GroupMemberDetail = GetGroupMemberDetail(Id.ToString(),OrganisationId.ToString());

                //if (ViewBag.role == "NurseCoordinator")
                //{
                //    ViewBag.permission = GetNurseCoordinatorPermission(objGroupDetail.ChattingGroupId.ToString(), LoginUserUserId);
                //}

                ViewBag.permission = GetGroupChatMemberPermission(objGroupDetail.ChattingGroupId.ToString(), LoginUserUserId);

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "OpenGroupChatWindow";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("Login", "Account");
            }
            return View();
        }



        //public bool SendChatMessageNotification(CareGivers caregiver)
        //{ }


        [HttpPost]
        public ActionResult UploadFiletoSendToEmail(HttpPostedFileBase UploadedFile, string EmailTo, string vMsg, string UserName, string GroupName)
        {

            if (UploadedFile != null && UploadedFile.ContentLength > 0)
            {
                string path = Server.MapPath("~/Uploads/ChatAttachmentSendToEmail/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fullPath = Path.Combine(path, Path.GetFileName(UploadedFile.FileName));
                UploadedFile.SaveAs(fullPath);

                string toAddress = EmailTo;
               
                bool IsFileAttachment = true;

                string body = " ";

                string CCMailID = " ";

                bool isBodyHtml = true;
                string AttachmentFileName =fullPath ;

                // if (sendEmailWithAttachment(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))

                if (sendEmailWithAttachment(toAddress, GroupName, vMsg, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
                {

                    return Json(new { success = true, filePath = fullPath });

                }



                return Json(new { success = true, filePath = fullPath });
            }

            return Json(new { success = false, message = "No file uploaded" });
        }







        public string NotifyUserForChatMessage(string UserId, string QuickBloxId, string Msg, string DialogId, string Type, string UserName, string GroupName)
        {

            var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedFile"];





            string result = "";
            try
            {
                CareGivers caregiver = new CareGivers();
                caregiver.UserId = UserId;
                caregiver.QuickBloxId = QuickBloxId;
                caregiver.Msg = Msg.Length > 10 ? Msg.Substring(0, 10) + "..." : Msg;
                caregiver.DialogId = DialogId;
                caregiver.ChattingType = Type;//Chatting Type 1=Scheduler Chat,2=Patient Group Chat
                caregiver.UserName = UserName;
                caregiver.Name = GroupName;

                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.NotifyUserForChatMessage(caregiver).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "NotifyUserForChatMessage";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        //Get All Unassigned Nurse Coordinator 

        public List<NurseCoordinator> GetUnAssignedNurseCoordinatorList(string ChattingGroupId)
        {
            string DialogId = "";
            var objNurseCoordinatorList = new List<NurseCoordinator>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                objNurseCoordinatorList = ChattingService.GetUnAssignedNurseCoordinatorList(ChattingGroupId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetUnAssignedNurseCoordinatorList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objNurseCoordinatorList;
        }

        //public string SetNurseCoordinator(string ChattingGroupId, string NurseCoordinatorId, string Permission, string QuickBloxId)
        //{
        //    string result = "";
        //    try
        //    {

        //        var ChattingService = new ChattingServiceProxy();
        //        result = ChattingService.SetNurseCoordinator(ChattingGroupId, NurseCoordinatorId, Permission).Result;

        //        var objDialogDetail = new Chatting();
        //        objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
        //        MembershipUser user = Membership.GetUser();
        //        // var SchedulerEmail = user.Email;
        //        string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];
        //        //AddCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId,objDialogDetail.GroupName, SchedulerEmail,QuickBloxId);

        //        Task.Run(async () => { await AddCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId); }).Wait();

        //        //var task = AddCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId);
        //        //var result = task.WaitAndUnwrapException();
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "ChattingController";
        //        log.Methodname = "SetNurseCoordinator";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return result;
        //}


        public string SetNurseCoordinator(string ChattingGroupId, string NurseCoordinatorId, string Permission, string QuickBloxId)
        {
            string result = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();
                // result = ChattingService.SetNurseCoordinator(ChattingGroupId, NurseCoordinatorId, Permission).Result;

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {

                    string SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();

                    Task.Run(async () => { await AddOfficeStaffToGroupRestAPIUAT(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, ChattingGroupId, NurseCoordinatorId, "3"); }).Wait();

                }
                else
                {
                    string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];

                    // Task.Run(async () => { await AddOfficeStaffToGroupRestAPI(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, ChattingGroupId, NurseCoordinatorId, Permission); }).Wait();

                    Task.Run(async () => { await AddOfficeStaffToGroupRestAPIUAT(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, ChattingGroupId, NurseCoordinatorId, "3"); }).Wait();
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "SetNurseCoordinator";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }




        public static async Task<int> AddOfficeStaffToGroupRestAPI(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId, string ChattingGroupId, string NurseCoordinatorId, string Permission)
        {
            try
            {

                #region FOR SESION GENERATION

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
                #endregion

                #region  ADD MEMBER TO QUICKBLOX

                int QuickBloxId = Convert.ToInt32(CaregiverQBId);

                var objAddDialog = new AddDialog();
                List<int> objoccupants_ids = new List<int>();
                objoccupants_ids.Add(QuickBloxId);

                try
                {
                    objAddDialog.name = GroupName;
                    var objPullAll = new PullAll();
                    objPullAll.occupants_ids = objoccupants_ids;
                    objAddDialog.push_all = objPullAll;
                }
                catch (Exception e)
                {

                }

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;


                int StatusCode = Convert.ToInt32(response2.StatusCode);

                var resultQB = (JObject)JsonConvert.DeserializeObject(result2);


                if (StatusCode == 200)
                {

                    var ChattingService = new ChattingServiceProxy();

                    result = ChattingService.SetNurseCoordinator(ChattingGroupId, NurseCoordinatorId, Permission).Result;
                }




                #endregion

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteMobileServices";
                objErrorlog.Methodname = "AddMemberToGroupRestAPI";
                objErrorlog.UserID = "";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
                //  return null;
            }
            return 1;
        }

        public string SaveQBIdOfNurseCoordinator(string Email, string QuickBloxId)
        {
            string result = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.SaveQBIdOfNurseCoordinator(Email, QuickBloxId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "SaveQBIdOfNurseCoordinator";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        public ActionResult AssignNurseCoordinator(int ChattingGroupId, string GroupName)
        {
            ViewBag.ChattingGroupId = ChattingGroupId;
            ViewBag.GroupName = GroupName;
            ViewBag.NurseCoordinatorList = GetUnAssignedNurseCoordinatorList(ChattingGroupId.ToString());
            ViewBag.NurseCoDetail = GetNurseCoordinatorPermissionGroupWiseList(ChattingGroupId.ToString());
            return PartialView("AssignNurseCoordinator");
        }

        private async Task<int> AddCareGiverToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId)
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

        //Get single NurseCoordinator permission
        public string GetNurseCoordinatorPermission(string ChattingGroupId, string LoginUserUserId)
        {
            string Permission = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();
                Permission = ChattingService.GetNurseCoordinatorPermissionGroupWise(ChattingGroupId, LoginUserUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetNurseCoordinatorPermission";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Permission;
        }

        //NurseCoordinator List in Assign New popup
        public List<ChattingGroupMember> GetNurseCoordinatorPermissionGroupWiseList(string ID)
        {
            var objGroupMemberDetailListing = new List<ChattingGroupMember>();
            try
            {

                var ChattingService = new ChattingServiceProxy();
                objGroupMemberDetailListing = ChattingService.GetNurseCoordinatorPermissionGroupWiseList(ID).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetNurseCoordinatorPermissionGroupWiseList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objGroupMemberDetailListing;
        }

        //Added By Pinki on 12th Sept,2017
        #region Scheduler can assign/remove multiple caregivers to/from group
        public ActionResult AssignCaregiver(int ChattingGroupId, string GroupName)
        {
            ViewBag.ChattingGroupId = ChattingGroupId;
            ViewBag.GroupName = GroupName;
            ViewBag.CaregiverList = GetUnassignedCaregiverList(ChattingGroupId.ToString());
            ViewBag.AssignedCaregiverDetail = GetAssignedCaregiverListGroupWise(ChattingGroupId.ToString());
            return PartialView("AssignCaregiver");
        }
        public List<CareGivers> GetUnassignedCaregiverList(string ChattingGroupId)
        {
            var Caregivers = new List<CareGivers>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                Caregivers = ChattingService.GetUnassignedCaregiverList(ChattingGroupId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetCaregiverList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Caregivers;
        }
        public List<ChattingGroupMember> GetAssignedCaregiverListGroupWise(string ID)
        {
            var objGroupMemberDetailListing = new List<ChattingGroupMember>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                objGroupMemberDetailListing = ChattingService.GetAssignedCaregiverListGroupWise(ID).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetAssignedCaregiverListGroupWise";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objGroupMemberDetailListing;
        }
        public string AddCaregiverIntoGroup(string ChattingGroupId, string NurseId, string QuickBloxId)
        {
            string result = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var nurseIds = NurseId.Split(',');
                foreach (var id in nurseIds)
                {
                    result = ChattingService.AddCaregiverIntoGroup(ChattingGroupId, id).Result;
                }

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();
                //    var SchedulerEmail = user.Email;
                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];

                var qbIds = QuickBloxId.Split(',').Select(Int32.Parse).ToArray();
                Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, qbIds); }).Wait();
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddCaregiverIntoGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }
        public JsonResult RemoveMemberFromGroupChat(string ChattingGroupId, string UserId, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();
                //result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();
                //  var SchedulerEmail = user.Email;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                if (OrganisationId > 0)
                {
                    SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                }
                else
                {
                     SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                }

                Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, OrganisationId); }).Wait();
                result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "RemoveMemberFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            //return result;
            var lstCaregiver = GetUnassignedCaregiverList(ChattingGroupId);
            return Json(new { result, lstCaregiver }, JsonRequestBehavior.AllowGet);
        }

        private async Task<int> RemoveCareGiverToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId,int OrganisationId)
        {
            //----
            try
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                //JObject json = JObject.Parse(result);
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End

                //  GetActivePatientRequest All Dialog Detail
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

                ////if occupants_ids get match then Call to remove from group
                bool flag = false;
                for (int k = 0; k < tempOccupants_ids.Count; k++)
                {
                    if (Convert.ToInt32(CaregiverQBId) == tempOccupants_ids[k])
                    {
                        flag = true;
                        break;
                    }
                }

                #region Remove Member from group
                if (flag == true)
                {
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

                }
                #endregion
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "RemoveCareGiverToDialodOnQuickBlox";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return 1;

        }

        public static async Task<int> AddMultipleUserToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, int[] UserQBIds)
        {
            //----
            try
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
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

                #region Add multiple member into Group on Quickblox

                List<int> PendingQBIDS = new List<int>();

                foreach (var obj in tempOccupants_ids)
                {
                    if (!UserQBIds.ToList().Contains(obj))
                    {
                        PendingQBIDS.Add(obj);
                    }
                }

                var query = tempOccupants_ids.Where(item => !UserQBIds.Contains(item)).ToList();

                List<int> lstToBeAdded = UserQBIds.Except(tempOccupants_ids).ToList();

                if (lstToBeAdded.Count() > 0)
                {
                    var objAddDialog = new AddDialog();
                    List<int> objoccupants_ids = new List<int>();
                    foreach (var qbid in lstToBeAdded)
                    {
                        objoccupants_ids.Add(Convert.ToInt32(qbid));
                    }

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

                #endregion
            }
            catch (Exception e)
            {

            }
            return 1;
        }
        #endregion

        //Added By Pinki on 26th Oct,2017
        #region Supervisor/Support can assign multiple member to group
        public ActionResult AssignMember(int ChattingGroupId, string GroupName, int GroupTypeId)
        {
            ViewBag.ChattingGroupId = ChattingGroupId;
            ViewBag.GroupName = GroupName;
            ViewBag.GroupTypeId = GroupTypeId;

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            ViewBag.MemberList = GetUnassignedMemberList(ChattingGroupId.ToString(),OrganisationId);
            ViewBag.AssignedMemberList = GetAssignedMemberListGroupWise(ChattingGroupId.ToString());
            return PartialView("AssignMember");
        }

        public List<CareGivers> GetUnassignedMemberList(string ChattingGroupId, int OrganisationId)
        {
            var Members = new List<CareGivers>();
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var ProviderUserKey = Membership.GetUser().ProviderUserKey.ToString();

                Members = ChattingService.GetAllUnAssignedMemberList(ChattingGroupId, ProviderUserKey,Convert.ToString(OrganisationId)).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetUnassignedMemberList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Members;
        }


        [HttpGet]
        public ActionResult SupplyForm()
        {
            return PartialView("SupplyForm");
        }

        //Added By Vinod on 17th Oct,2018


        public List<CareGivers> GetMemberList()
        {
            var Members = new List<CareGivers>();
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var ProviderUserKey = Membership.GetUser().ProviderUserKey.ToString();

                Members = ChattingService.GetMemberList(ProviderUserKey).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetMemberList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Members;
        }


        public List<ChattingGroupMember> GetAssignedMemberListGroupWise(string ID)
        {
            var objGroupMemberDetailListing = new List<ChattingGroupMember>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                objGroupMemberDetailListing = ChattingService.GetAssignedMemberListGroupWise(ID).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetAssignedMemberListGroupWise";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objGroupMemberDetailListing;
        }

        public string AddMemberIntoGroup(string ChattingGroupId, string UserIds, string QuickBloxId)
        {

            string result = "";
            string SuperAdminEmailId = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var userIds = UserIds.Split(',');
                foreach (var id in userIds)
                {
                    result = ChattingService.AssignGroupToUser(ChattingGroupId, id).Result;
                }
                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                if (OrganisationId > 0)
                {

                    SuperAdminEmailId = Session["OrgSuperAdminEmail"].ToString();
                }
                else
                {

                    SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                }

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();
                var SchedulerEmail = objDialogDetail.SchedulerEmailId;



                var qbIds = QuickBloxId.Split(',').Select(Int32.Parse).ToArray();
                Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SuperAdminEmailId, qbIds); }).Wait();
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddMemberIntoGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }
        public JsonResult RemoveParticipantsFromGroupChat(string ChattingGroupId, string UserId, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
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

                    SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                }
                else
                {
                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                }


                Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId,OrganisationId); }).Wait();
                result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "RemoveParticipantsFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            //return result;
            int OrganisationIdS = Convert.ToInt32(Session["OrganisationId"]);
            var lstMember = GetUnassignedMemberList(ChattingGroupId, OrganisationIdS);
            return Json(new { result, lstMember }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        //Hardik Masalawala 30-10-2017
        #region GetGroupDetailFromGroupName 
        public JsonResult GetGroupDetailFromGroupName(string GroupName)
        {
            var objDialogDetail = new Chatting();
            try
            {

                var ChattingService = new ChattingServiceProxy();
                objDialogDetail = ChattingService.GetGroupDetailFromGroupName(GroupName).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetGroupDetailFromGroupName";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            //return objDialogDetail;
            return Json(objDialogDetail, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EscapeSeq
        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        #endregion
        private void FillAllOffices()
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            try { 
            string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
            OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();
            var lstOffices = officeServiceProxy.GetAllOffices(logInUserId,OrganisationId.ToString()).Result;
            lstOffices.Insert(0, new Office() { OfficeId = 0, OfficeName = "All" });
            SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName");
            ViewBag.OfficeDropDownList = officeSelectList;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }


        public string SaveQBIdOfAdmin(string Email, string QuickBloxId)
        {
            string result = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.SaveQBIdOfAdmin(Email, QuickBloxId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "EditComment";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        // For Add admin into group when Group is already created & Office Admin(Office Manager not added in group)
        public string AddAdminIntoGroup(string ChattingGroupId, int OfficeId)
        {

            string result = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                MembershipUser user = Membership.GetUser();
                string[] roles = Roles.GetRolesForUser(user.UserName);
                string LogInUserId = user.ProviderUserKey.ToString();

                if (roles.Length > 0 && roles[0] == "SuperAdmin" || roles[0] == "Scheduler")
                {
                    OfficeModel objModel = new OfficeModel();

                    objModel = OfficeController.GetOfficeDetailByOfficeId(OfficeId.ToString());

                    AddMemberIntoGroup(ChattingGroupId, objModel.AdminUserId, objModel.AdminQuickBloxId);

                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "AddAdminIntoGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }



        #region Generate Any User QuickBloxId & Update in DB RestAPI

        //Generate Any User QuickBloxId & Update in DB RestAPI
        //public string RegisterCaregiver1()
        //{
        //    GenerateCaregiverQuickBloxIdRestAPI();
        //    return "";
        //}

        public static async Task<string> GenerateUserQuickBloxIdRestAPI(string UserId, string Email, int OfficeId, bool IsAllowGroupChat, int OrganisationId, string SuperOrgEmailId)
        {
            //-----------------
            string QuickbloxId = string.Empty;

            //  string QuickbloxId = "0";
            try
            {
                var SchedulerEmail = "Superadmin@paseva.com";

                string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                ////Sessoin Generated Start
                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");


                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

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


                //QuickBlox Call For create User Start//

                #region QuickBlox Call For create User Start


                var inputRequestParam = new UserdataQuickBloxReq();
                var UserRequest = new userReq();

                inputRequestParam.login = Email;
                inputRequestParam.password = "Welcome007!";
                inputRequestParam.email = Email;
                UserRequest.user = inputRequestParam;

                var userData1 = JsonConvert.SerializeObject(UserRequest);
                var contentData = new StringContent(userData1, Encoding.UTF8, "application/json");


                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl);
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
                var response1 = await clientGetDialogId.PostAsync("/users.json", contentData);
                var result1 = response1.Content.ReadAsStringAsync().Result;

                var MyData = (JObject)JsonConvert.DeserializeObject(result1); //MyData["user"]["id"].Value<string>() quick Blox Id
                QuickbloxId = MyData["user"]["id"].Value<string>();

                #endregion


                string SaveQb = "";

                var ChattingService = new ChattingServiceProxy();

                SaveQb = ChattingService.SaveQBId(UserId, QuickbloxId).Result;

                if (!string.IsNullOrEmpty(QuickbloxId) && OfficeId != 0 && IsAllowGroupChat == true)
                {
                    var ChattingController = new ChattingController();
                    ChattingController.AddMemberIntoOfficeGroup(OfficeId.ToString(), UserId, QuickbloxId, OrganisationId,SuperOrgEmailId);

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "ChattingController";
                objErrorlog.Methodname = "GenerateUserQuickBloxIdRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
            }

            //--------------------

            //string SaveQb = "";

            //var ChattingService = new ChattingServiceProxy();

            //SaveQb = ChattingService.SaveQBId(UserId, QuickbloxId).Result;

            return QuickbloxId;
        }

        #endregion


        public JsonResult RemoveNurseCordinatorFromGroupChat(string ChattingGroupId, string UserId, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();
                //result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();
                //  var SchedulerEmail = user.Email;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                if (OrganisationId > 0)
                {

                    SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                }
                else
                {


                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];

                }

               // string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];

                Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId,OrganisationId); }).Wait();
                result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "RemoveMemberFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            //return result;
            var lstNurseCoordinator = GetUnAssignedNurseCoordinatorList(ChattingGroupId);
            return Json(new { result, lstNurseCoordinator }, JsonRequestBehavior.AllowGet);
        }

        // New groupchat by krunal pawar 
        //public ActionResult GroupChat()
        //{
        //    FillAllOffices();
        //    return View();
        //}

        public ActionResult AddNewGroup()
        {

            ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            List<Office> OfficeList = new List<Office>();
            OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(),OrganisationId.ToString()).Result;
            OfficeList.Add(new Office { OfficeId = 0, OfficeName = "All" });

            //MultiSelectList officeSelectList = new MultiSelectList(OfficeList, "OfficeId", "OfficeName");

            //     SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");

            ViewBag.lstOffice = OfficeList;

                //officeSelectList;

            // OfficeList;

            //officeSelectList;

            // ViewBag.MemberList = GetUnassignedMemberList("277");
            return PartialView("AddNewGroup");
        }


        [HttpPost]
        public ActionResult CreateNewGroup(GroupChatModel GroupChatModel)
        {

            string result = "";
            try
            {
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                var GroupChat = new GroupChat();
                GroupChat.GroupDialogId = GroupChatModel.GroupDialogId;
                GroupChat.GroupName = GroupChatModel.GroupName;
                GroupChat.GroupSubject = GroupChatModel.GroupSubject;
                GroupChat.LogInUserId = LogInUserId;
                GroupChat.OfficeId = GroupChatModel.OfficeId;
                GroupChat.GroupTypeID = 2;
                GroupChat.OrganisationId = OrganisationId;

                GroupChatModel.QuickBloxIds = Session["FromQBId"].ToString() + ',' + GroupChatModel.QuickBloxIds;



                var ChattingService = new ChattingServiceProxy();

                if (OrganisationId > 0)
                {

                    string SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                        //ConfigurationManager.AppSettings["SuperOrgAdminEmailId"];

                    var Chatting = new Chatting();

                    // Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;

                    Chatting = AddOrganisationGroupDialogId(GroupChat);

                    //var userIds = GroupChatModel.MembersUserId.Split(',');
                    //foreach (var id in userIds)
                    //{
                    //    result = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
                    //}
                    //var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();
                    //Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(Chatting.DialogId, GroupChatModel.GroupName, "superadmin@paseva.com", qbIds); }).Wait();


                    var QuickBloxIds = GroupChatModel.QuickBloxIds.Split(',');


                    var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();

                    Task.Run(async () => { await AddMemberToGroupRestAPI(Chatting.ChattingGroupId.ToString(), Chatting.DialogId, GroupChatModel.GroupName, SchedulerEmail, qbIds, LogInUserId); }).Wait();

                }
                else
                {
                    string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                    var Chatting = new Chatting();
                    Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;

                    //var userIds = GroupChatModel.MembersUserId.Split(',');
                    //foreach (var id in userIds)
                    //{
                    //    result = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
                    //}
                    //var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();
                    //Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(Chatting.DialogId, GroupChatModel.GroupName, "superadmin@paseva.com", qbIds); }).Wait();

                    var QuickBloxIds = GroupChatModel.QuickBloxIds.Split(',');

                    var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();

                    Task.Run(async () => { await AddMemberToGroupRestAPI(Chatting.ChattingGroupId.ToString(), Chatting.DialogId, GroupChatModel.GroupName, SchedulerEmail, qbIds, LogInUserId); }).Wait();
                }
                result = "Success";


                if (result == "Success")
                {


                    TempData["Message"] = "New group created successfully.";
                    return RedirectToAction("GroupChat", "Chatting", new { IsAdded = true });
                }



                //var userIds = UserIds.Split(',');
                //foreach (var id in userIds)
                //{
                //    result = ChattingService.AssignGroupToUser(ChattingGroupId, id).Result;
                //}

                //var objDialogDetail = new Chatting();
                //objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                //MembershipUser user = Membership.GetUser();
                //var SchedulerEmail = objDialogDetail.SchedulerEmailId;

                // var qbIds = QuickBloxId.Split(',').Select(Int32.Parse).ToArray();
                //  Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, qbIds); }).Wait();
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "CreateNewGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return RedirectToAction("GroupChat", "Chatting");
        }



        [HttpPost]
        public ActionResult CreateGroupWithSingleAndMultipleOffices(GroupChatModel GroupChatModel)
        {

            string result = "";
            try
            {
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                var GroupChat = new GroupChat();
                GroupChat.GroupDialogId = GroupChatModel.GroupDialogId;
                GroupChat.GroupName = GroupChatModel.GroupName;
                GroupChat.GroupSubject = GroupChatModel.GroupSubject;
                GroupChat.LogInUserId = LogInUserId;
                GroupChat.OfficeId = GroupChatModel.OfficeId;
                GroupChat.GroupTypeID = 2;
                GroupChat.OrganisationId = OrganisationId;

                GroupChatModel.QuickBloxIds = Session["FromQBId"].ToString() + ',' + GroupChatModel.QuickBloxIds;
                var ChattingService = new ChattingServiceProxy();

                if (OrganisationId > 0)
                {

                    string SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                    //ConfigurationManager.AppSettings["SuperOrgAdminEmailId"];

                    var Chatting = new Chatting();

                    // Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;

                    Chatting = AddOrganisationGroupDialogId(GroupChat);

                    //var userIds = GroupChatModel.MembersUserId.Split(',');
                    //foreach (var id in userIds)
                    //{
                    //    result = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
                    //}
                    //var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();
                    //Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(Chatting.DialogId, GroupChatModel.GroupName, "superadmin@paseva.com", qbIds); }).Wait();

                 

                    var QuickBloxIds = GroupChatModel.QuickBloxIds.Split(',');

                    var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();

                    if (GroupChatModel.IsCheckOfficeGroup)
                    {



                        Task.Run(async () => { await AddGroupMemberToMultipleOfficesGroupRestAPI(Chatting.ChattingGroupId.ToString(), Chatting.DialogId, GroupChatModel.GroupName, SchedulerEmail, qbIds, LogInUserId, GroupChatModel.OfficeIds, OrganisationId); }).Wait();
                    }
                    else
                    {
                        Task.Run(async () => { await AddMemberToGroupRestAPI(Chatting.ChattingGroupId.ToString(), Chatting.DialogId, GroupChatModel.GroupName, SchedulerEmail, qbIds, LogInUserId); }).Wait();

                    }
                }
                else
                {

                    string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                    var Chatting = new Chatting();
                    Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;


                    #region
                    //var userIds = GroupChatModel.MembersUserId.Split(',');
                    //foreach (var id in userIds)
                    //{
                    //    result = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
                    //}
                    //var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();
                    //Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(Chatting.DialogId, GroupChatModel.GroupName, "superadmin@paseva.com", qbIds); }).Wait();
                    #endregion

                    //Adding SuperAdmin in All multiple office and custom Group
                    //var SchedulerList = new List<ScheduleInfo>();
                    //SchedulerList = ChattingService.GetALLSuperadminList().Result;

                    //foreach (var item in SchedulerList)
                    //{
                    //    GroupChatModel.QuickBloxIds = GroupChatModel.QuickBloxIds + ',' + item.QuickbloxId;
                    //}

                    var QuickBloxIds = GroupChatModel.QuickBloxIds.Split(',');

                    var qbIds = GroupChatModel.QuickBloxIds.Split(',').Select(Int32.Parse).ToArray();

                    if (GroupChatModel.IsCheckOfficeGroup)
                    {
                        Task.Run(async () => { await AddGroupMemberToMultipleOfficesGroupRestAPI(Chatting.ChattingGroupId.ToString(), Chatting.DialogId, GroupChatModel.GroupName, SchedulerEmail, qbIds, LogInUserId, GroupChatModel.OfficeIds, OrganisationId); }).Wait();
                    }
                    else
                    {
                        Task.Run(async () => { await AddMemberToGroupRestAPI(Chatting.ChattingGroupId.ToString(), Chatting.DialogId, GroupChatModel.GroupName, SchedulerEmail, qbIds, LogInUserId); }).Wait();
                    }
                }

                result = "Success";

                if (result == "Success")
                {

                    TempData["Message"] = "New group created successfully.";

                    return RedirectToAction("GroupChat", "Chatting", new { IsAdded = true });
                }


                //var userIds = UserIds.Split(',');
                //foreach (var id in userIds)
                //{
                //    result = ChattingService.AssignGroupToUser(ChattingGroupId, id).Result;
                //}

                //var objDialogDetail = new Chatting();
                //objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                //MembershipUser user = Membership.GetUser();
                //var SchedulerEmail = objDialogDetail.SchedulerEmailId;

                // var qbIds = QuickBloxId.Split(',').Select(Int32.Parse).ToArray();
                //  Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, qbIds); }).Wait();
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "CreateNewGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return RedirectToAction("GroupChat", "Chatting");
        }





   


        [HttpPost]
        public JsonResult GetAllMemberByOffice(string OfficeId)
        {
            string result = "";

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            var objGroupMemberDetailListing = new List<ChattingGroupMember>();

            try
            {
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                var ChattingService = new ChattingServiceProxy();
                objGroupMemberDetailListing = ChattingService.GetAllMemberByOffice(LogInUserId, OfficeId, OrganisationId.ToString()).Result;

                if (objGroupMemberDetailListing.Count > 0)
                    result = "Success";

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetAllMemberByOffice";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            //return result;
            //  var lstMember = GetUnassignedMemberList(ChattingGroupId);
            return Json(new { result, objGroupMemberDetailListing }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetGroupChatList(JQueryDataTableParamModel param)
        {
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                FillAllOffices();

                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();


                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "GroupName";
                }
                else
                {
                    sortOrder = "GroupName";
                }

                string search = "||"; //It's indicate blank filter

                if (!string.IsNullOrEmpty(param.sSearch))
                    search = param.sSearch;

                var sortDirection = Request["sSortDir_0"]; // asc or desc

                int pageNo = 1;

                int recordPerPage = param.iDisplayLength;

                //Find page number from the logic
                if (param.iDisplayStart > 0)
                    pageNo = (param.iDisplayStart / recordPerPage) + 1;

                ChattingsList = ChattingLiteService.GetChattingListPatientGroupWise(LogInUserId, pageNo, recordPerPage, search, sortOrder, sortDirection, "2").Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetChattingListPatientGroupWise";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("GroupChat", "Chatting");
            }
            if (ChattingsList.objChattingsList != null)
            {
                //   var result = from C in ChattingsList.objChattingsList select new[] { C, C, C, C, C, C, C, C, C };
                var result = ChattingsList.objChattingsList;
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ChattingsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ChattingsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = ChattingsList.TotalNumberofRecord,
                    iTotalDisplayRecords = ChattingsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // *********************************************************************************************************
        //                   Create Group for Chat on Qiuckblox Rest API
        // *********************************************************************************************************** 
        // Only use for Create Office Group
        public static async Task<string> CreateGroupChatOnQuickBloxRestAPI(string GroupName, List<string> MemberList, int OfficeId, string AdminUserId)
        {
            // string UserId = GetUserIDFromAccessToken();
            string DialogId = "";
            string res2 = "";
            string SuperAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();

            try
            {
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //var SchedulerEmail = "superadmin@paseva.com";//Check This
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();
                ////Sessoin Generated End

                //GetActivePatientRequest All Dialog Detail
                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
                ////--

                var objAddDialog = new CreateGroup();
                List<int> objoccupants_ids = new List<int>();
                List<string> name = new List<string>();

                //objAddDialog.occupants_ids.Add(1);//Group Creator QuickBlox Id
                objAddDialog.name = GroupName;//GroupName

                // For add custom param
                Data CustomParam = new Data();
                CustomParam.class_name = "ChatDialogType";
                CustomParam.ChatCategory = "3";
                CustomParam.OfficeID = OfficeId.ToString();
                objAddDialog.data = CustomParam;

                if (MemberList != null)
                {
                    foreach (var item in MemberList)
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            objoccupants_ids.Add(Convert.ToInt32(item));
                        }
                    }
                }
                objAddDialog.occupants_ids = objoccupants_ids;//Add Member QuickBloxId

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientGetDialogId.PostAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result2);
                DialogId = data1["_id"].Value<string>();

            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "CreateGroupChatOnQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }
            ////--

            var ChattingService = new ChattingServiceProxy();

            var Chatting = new Chatting();

            var GroupChat = new GroupChat();
            GroupChat.GroupDialogId = DialogId;
            GroupChat.GroupName = GroupName;
            GroupChat.LogInUserId = SuperAdminUserId;
            GroupChat.OfficeId = OfficeId;
            GroupChat.GroupTypeID = 3;

            Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;

            //   res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), "4D51E02D-D16E-4962-A01B-97624A683A64").Result; //for add second super admin

            var AdminUserIds = AdminUserId.Split(',');
            foreach (var id in AdminUserIds)
            {
                res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
            }
            // res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), AdminUserId).Result;

            return DialogId;

        }


        public static async Task<string> CreateOrganisationBasedGroupChatOnQuickBloxRestAPI(string GroupName, List<string> MemberList, int OfficeId, string AdminUserId, int OrganisationId, string OrgSuperAdminUserId, string OrgSuperAdminEmail)
        {
            // string UserId = GetUserIDFromAccessToken();
            string DialogId = "";
            string res2 = "";
            string SuperAdminUserId = OrgSuperAdminUserId;
                
            //Session["OrgSuperAdminUserId"].ToString();

            //ConfigurationManager.AppSettings["SuperOrgAdminUserId"].ToString();

            try
            {
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //var SchedulerEmail = "superadmin@paseva.com";//Check This
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                string SchedulerEmail = OrgSuperAdminEmail;
                    
                    //Session["OrgSuperAdminEmail"].ToString();
                    //ConfigurationManager.AppSettings["SuperOrgAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End

                //GetActivePatientRequest All Dialog Detail
                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
                ////--

                var objAddDialog = new CreateGroup();
                List<int> objoccupants_ids = new List<int>();
                List<string> name = new List<string>();


                //objAddDialog.occupants_ids.Add(1);//Group Creator QuickBlox Id
                objAddDialog.name = GroupName;//GroupName

                // For add custom param
                Data CustomParam = new Data();
                CustomParam.class_name = "ChatDialogType";
                CustomParam.ChatCategory = "3";
                CustomParam.OfficeID = OfficeId.ToString();
                objAddDialog.data = CustomParam;

                if (MemberList != null)
                {
                    foreach (var item in MemberList)
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            objoccupants_ids.Add(Convert.ToInt32(item));
                        }
                    }
                }
                objAddDialog.occupants_ids = objoccupants_ids;//Add Member QuickBloxId

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientGetDialogId.PostAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result2);
                DialogId = data1["_id"].Value<string>();

            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "CreateGroupChatOnQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }

            var ChattingService = new ChattingServiceProxy();

            var Chatting = new Chatting();

            var GroupChat = new GroupChat();
            GroupChat.GroupDialogId = DialogId;
            GroupChat.GroupName = GroupName;
            GroupChat.LogInUserId = SuperAdminUserId;
            GroupChat.OfficeId = OfficeId;
            GroupChat.GroupTypeID = 3;
            GroupChat.OrganisationId = OrganisationId;

            // Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;

            Chatting = AddOrganisationGroupDialogId(GroupChat);

            // res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), "4D51E02D-D16E-4962-A01B-97624A683A64").Result; //for add second super admin

            var AdminUserIds = AdminUserId.Split(',');
            foreach (var id in AdminUserIds)
            {
                res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
            }
            // res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), AdminUserId).Result;

            return DialogId;
        }


        public string RemoveMemberFromOfficeGroup(string OfficeId, string UserId, string QuickBloxId, int OrganisationId, string SuperOrgEmailId)
        {

            string result = "";
            string SchedulerEmail = "";

            try
            {
                if (OrganisationId > 0)
                {
                    SchedulerEmail = SuperOrgEmailId;
                }
                else
                { 
                     SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }

                var ChattingService = new ChattingServiceProxy();
                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetOfficeGroupDetailByOfficeId(OfficeId.ToString(), UserId).Result;
                                  
                //   if()

                if (!string.IsNullOrEmpty(objDialogDetail.UserId))
                {
                    Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, OrganisationId); }).Wait();

                    result = ChattingService.RemoveMemberFromGroupChat(objDialogDetail.ChattingGroupId.ToString(), UserId).Result;

                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "RemoveMemberFromOfficeGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return result;
            }

            return result;
        }

        public string AddMemberIntoOfficeGroup(string OfficeId, string UserId, string QuickBloxId, int OrganisationId, string SuperOrgAdminEmailId)
        {
            string result = "";
            string SuperAdminEmailId = "";

            try
            {
                //string SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"];

                //int OrganisationId = OrganisationId
                    //Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {
                    SuperAdminEmailId = SuperOrgAdminEmailId;
                }
                else
                {
                    SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                }

                var ChattingService = new ChattingServiceProxy();
                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetOfficeGroupDetailByOfficeId(OfficeId.ToString(), UserId).Result;

                var qbIds = QuickBloxId.Split(',').Select(Int32.Parse).ToArray();

                Task.Run(async () => { await AddMultipleUserToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SuperAdminEmailId, qbIds); }).Wait();

                result = ChattingService.AssignGroupToUser(objDialogDetail.ChattingGroupId.ToString(), UserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddMemberIntoOfficeGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        #region Delete Group
        // Delete Group Hardik Masalawala

        public JsonResult DeleteGroupChat(int ChattingGroupId, string DialogId)
        {

            try
            {
                var ChattingService = new ChattingServiceProxy();

                var LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                var result = ChattingService.DeleteGroupChat(ChattingGroupId.ToString(), DialogId, LoginUserId).Result;
                if (result.ToString().Equals("Success"))
                {
                    Task.Run(async () => { await DeleteGroupChatOnQuickBloxRestAPI(DialogId); }).Wait();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                // DeleteGroupChatManually(ChattingGroupId, DialogId);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "DeleteGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }



        public string DeleteGroupChatManually(int ChattingGroupId, string DialogId)
        {


            string result = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var LoginUserId = Membership.GetUser().ProviderUserKey.ToString();


                ChattingsList ChattingsList = new ChattingsList();
                List<Chatting> listChatting = ChattingService.GetChatGroupListForDeleteManually(LoginUserId, "1").Result;
                ChattingsList.objChattingsList = listChatting;

                foreach (var item in listChatting)
                {

                    var result1 = ChattingService.DeleteGroupChat(item.ChattingGroupId.ToString(), item.DialogId, LoginUserId).Result;

                    if (result1.ToString().Equals("Success"))
                    {
                        Task.Run(async () => { await DeleteGroupChatOnQuickBloxRestAPI(item.DialogId); }).Wait();

                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddMemberIntoOfficeGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        public async Task<string> DeleteGroupChatOnQuickBloxRestAPI(string GroupDialogId)
        {
            // string UserId = GetUserIDFromAccessToken();
            string Result = "";
            string SuperAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();
            try
            {
                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");


                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End

                //Delete QuickBlox Group RestAPI Call
                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + GroupDialogId + ".json?force=1");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);

                var response2 = await clientGetDialogId.DeleteAsync("");
                var result2 = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result2);

                int StatusCode = Convert.ToInt32(response2.StatusCode);

                var resultQB = (JObject)JsonConvert.DeserializeObject(result2);


                if (StatusCode == 200)
                {

                }
                else
                {

                }
                //if (data1.ToString() == "")
                //{
                //    Result = "Success";
                //}

            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "DeleteGroupChatOnQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }
            return Result;
        }
        #endregion

        #region Update Group chat & QuickBloxDialog Name 
        public ActionResult EditGroupDetail(int ChattingGroupId, string DialogId, string GroupName, string GroupSubject)
        {
            ViewBag.ChattingGroupId = ChattingGroupId;
            ViewBag.GroupName = GroupName;
            ViewBag.GroupSubject = GroupSubject;
            ViewBag.DialogId = DialogId;

            //ViewBag.GroupTypeId = GroupTypeId;
            //ViewBag.MemberList = GetUnassignedMemberList(ChattingGroupId.ToString());
            //ViewBag.AssignedMemberList = GetAssignedMemberListGroupWise(ChattingGroupId.ToString());
            return PartialView("EditGroupDetail");
        }

        public JsonResult UpdateGroupDetail(int ChattingGroupId, string DialogId, string GroupName, string GroupSubject)
        {
            try
            {
                var LoginUserID = Membership.GetUser().ProviderUserKey.ToString();
                var result = "";

                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.UpdateGroupDetail(ChattingGroupId.ToString(), DialogId, GroupName, GroupSubject, LoginUserID).Result;
                if (result == "Success")
                {
                    Task.Run(async () => { await UpdateGroupChatOnQuickBloxRestAPI(GroupName, DialogId); }).Wait();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "UpdateGroupDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public static async Task<string> UpdateGroupChatOnQuickBloxRestAPI(string GroupName, string GroupDialogId)
        {
            // string UserId = GetUserIDFromAccessToken();
            string res2 = "";
            string SuperAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();

            try
            {
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //var SchedulerEmail = "superadmin@paseva.com";//Check This
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");


                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End

                ////+++++++++++++++++++++++
                //Delete QuickBlox Group RestAPI Call
                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + GroupDialogId + ".json");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);

                var objAddDialog = new CreateGroup();
                List<string> name = new List<string>();

                //objAddDialog.occupants_ids.Add(1);//Group Creator QuickBlox Id
                objAddDialog.name = GroupName;//GroupName




                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientGetDialogId.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result2);
                GroupDialogId = data1["_id"].Value<string>();
                var NewGroupNew = "";
                NewGroupNew = data1["name"].Value<string>();

                if (!string.IsNullOrEmpty(GroupDialogId.ToString()))
                {
                    return NewGroupNew;
                }
                ////+++++++++++++++++++++++

                return NewGroupNew;
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "UpdateGroupChatOnQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }
            ////--

        }

        #endregion

        public string RemoveMemberFromAnyGroupChat(string ChattingGroupId, string GroupDialogId, string GroupName, string UserId, string QuickBloxId, int OrganisationId, string SuperOrgEmailId)
        {
            string result = "";
            var SchedulerEmail = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();



                //  MembershipUser user = Membership.GetUser();
                //  var SchedulerEmail = user.Email;

                if (OrganisationId > 0)
                {
                    SchedulerEmail = SuperOrgEmailId;
                }
                else
                {
                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }

                //var SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(GroupDialogId, GroupName, SchedulerEmail, QuickBloxId, OrganisationId); }).Wait();
                result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;

                result = "Success";

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "RemoveMemberFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }


            return result;

        }

        public JsonResult IsGroupNameAndSubjectExist(string GroupName, string GroupSubject)
        {
            var Chatting = new Chatting();

            bool result = true;
            try
            {

                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                ChattingServiceProxy ChattingService = new ChattingServiceProxy();

                Chatting.GroupName = GroupName;
                Chatting.GroupSubject = GroupSubject;
                //PatientDetail.InsertUserId = Membership.GetUser().ProviderUserKey.ToString();
                result = ChattingService.IsGroupNameAndSubjectExist(Chatting, LogInUserId).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "NurseCoordinatorController";
                log.Methodname = "IsGroupNameAndSubjectExist";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            //return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExitGroupChat(int ChattingGroupId, string GroupDialogId, string GroupName)
        {
            //,string QuickBloxId
            string result = "";
            string SuperAdminEmailId = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var QuickBloxId = Session["FromQBId"];

               int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {
                    SuperAdminEmailId = Convert.ToString(Session["OrgSuperAdminEmail"]);
                }
                else
                {
                    SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }
                
                // var SchedulerEmail = "Superadmin@paseva.com";

               // string SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                var LoginUserId = Membership.GetUser().ProviderUserKey.ToString();

                Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(GroupDialogId, GroupName, SuperAdminEmailId, QuickBloxId.ToString(),OrganisationId); }).Wait();
                result = ChattingService.ExitMemberFromGroupChat(ChattingGroupId.ToString(), LoginUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "ExitGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            //   return Json(false, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewGroupMemberList(string id)
        {
          int  OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                ViewBag.ChattingGroupId = id;
                ViewBag.GroupMemberDetail = GetGroupMemberDetail(id.ToString(),OrganisationId.ToString());
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetGroupMemberDetailList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public string AssignGroupAdmin(string ChattingGroupId, string UserIds)
        {

            string result = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                var userIds = UserIds.Split(',');
                foreach (var id in userIds)
                {
                    result = ChattingService.AssignGroupAdminToUser(ChattingGroupId, id, LogInUserId).Result;
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AssignGroupAdmin";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        #region AssignPermission
        public ActionResult _AssignPermission(int ChattingGroupId, string GroupName, int GroupTypeId)
        {

            ViewBag.ChattingGroupId = ChattingGroupId;
            ViewBag.GroupName = GroupName;
            ViewBag.GroupMemberList = GetGroupMemberListWithPermissionAndRole(ChattingGroupId.ToString());
            ViewBag.AssignedMemberList = GetGroupMemberListWithPermissionAndRole(ChattingGroupId.ToString());

            //ViewBag.GroupMemberList = GetUnAssignedNurseCoordinatorList(ChattingGroupId.ToString());
            //ViewBag.AssignedMemberList = GetNurseCoordinatorPermissionGroupWiseList(ChattingGroupId.ToString());
            return PartialView("_AssignPermission");
        }


        public List<ChattingGroupMember> GetGroupMemberListWithPermissionAndRole(string ChattingGroupId)
        {
            var objMemberList = new List<ChattingGroupMember>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                objMemberList = ChattingService.GetGroupMemberListWithPermissionAndRole(ChattingGroupId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetGroupMemberListWithPermissionAndRole";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objMemberList;
        }

        public JsonResult SetGroupChatMemberPermission(string ChattingGroupId, string ChattingGroupMemberId, string Permission)//SetNurseCoordinator
        {
            string result = "";
            try
            {
                string LogInUserId = Membership.GetUser().ProviderUserKey.ToString();

                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.SetGroupChatMemberPermission(ChattingGroupId, ChattingGroupMemberId, Permission, LogInUserId).Result;

                ViewBag.AssignedMemberList = GetGroupMemberListWithPermissionAndRole(ChattingGroupId.ToString());

                return Json(GetGroupMemberListWithPermissionAndRole(ChattingGroupId.ToString()), JsonRequestBehavior.AllowGet);


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "SetGroupChatMemberPermission";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public string GetGroupChatMemberPermission(string ChattingGroupId, string LoginUserUserId)
        {
            string result = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();
                result = ChattingService.GetGroupChatMemberPermission(ChattingGroupId, LoginUserUserId).Result;
                if (!string.IsNullOrEmpty(result))
                    return result;
                else
                {
                    result = null;
                }

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetGroupChatMemberPermission";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return res;
            }
            return result;
        }
        #endregion

        public ActionResult NoAuthorize()
        {
            return View();
        }

        public string DeleteOneToOneChatByUserId(string UserId)
        {
            string result = "";

            try
            {
                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                ChattingController ChattingController = new ChattingController();
                var ChattingService = new ChattingServiceProxy();
                ChattingsList ChattingsList = new ChattingsList();

                List<Chatting> listChatting = ChattingService.GetOneToOneChatListByUserId(UserId).Result;
                ChattingsList.objChattingsList = listChatting;


                foreach (var item in listChatting)
                {
                    //  ChattingController.DeleteOneToOneChatOnQBRestAPI(item.DialogId,item.FromEmail);

                    Task.Run(async () => { await DeleteOneToOneChatOnQBRestAPI(item.DialogId, item.FromEmail); }).Wait();
                    result = ChattingService.DeleteOneToOneChatByUserId(item.DialogId, UserId).Result;
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "DeleteOneToOneChatByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return result;
            }

            return result;
        }

        public async Task<string> DeleteOneToOneChatOnQBRestAPI(string GroupDialogId, string FromEmail)
        {
            // string UserId = GetUserIDFromAccessToken();
            string Result = "";
            string SuperAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();
            try
            {
                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                SchedulerEmail = FromEmail;

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");


                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End

                //Delete QuickBlox Group RestAPI Call
                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + GroupDialogId + ".json?force=1");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);

                var response2 = await clientGetDialogId.DeleteAsync("");
                var result2 = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result2);
                if (data1.ToString() == "")
                {
                    Result = "Success";
                }

            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "DeleteGroupChatOnQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }
            return Result;
        }

        public async Task<string> DeleteUserFromQuickBloxRestAPI(string QuickBloxId, string EmailId)
        {

            // string UserId = GetUserIDFromAccessToken();

            string Result = "";
            string SuperAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();
            try
            {
                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                SchedulerEmail = EmailId;

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End

                //Delete QuickBlox Group RestAPI Call
                var clientGetDialogId = new System.Net.Http.HttpClient();

                //clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + GroupDialogId + ".json?force=1");
                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/users/" + QuickBloxId + ".json");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);

                var response2 = await clientGetDialogId.DeleteAsync("");
                var result2 = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result2);
                if (data1 == null)
                {
                    Result = "Success";
                }

            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "DeleteUserFromQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }
            return Result;
        }

        public string AddMemberIntoGroup_V1(string ChattingGroupId, string UserIds, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();

                var objDialogDetail = new Chatting();

                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;

                MembershipUser user = Membership.GetUser();

                //    var SchedulerEmail = user.Email;
                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {

                    SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                }
                else
                {

                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];

                }

                 string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();

                 // string LoginUserId = "6b186dd2-b28c-4baa-8ff7-f68b7e6258fa";

                var QuickBloxIds = QuickBloxId.Split(',');


                var qbIds = QuickBloxId.Split(',').Select(Int32.Parse).ToArray();


                Task.Run(async () => { await AddMemberToGroupRestAPI(ChattingGroupId, objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, qbIds, LoginUserId); }).Wait();


                result = "Success";


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddMemberIntoGroup_V1";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        public static async Task<int> AddMemberToGroupRestAPI(string ChattingGroupId, string DialogId, string GroupName, string SchedulerEmail, int[] UserQBIds, string LoginUserId)
        {
            try
            {
                #region FOR SESION GENERATION

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
                #endregion

                #region  ADD MEMBER TO QUICKBLOX

                foreach (var obj in UserQBIds)
                {
                    int QuickBloxId = Convert.ToInt32(obj);

                    var objAddDialog = new AddDialog();
                    List<int> objoccupants_ids = new List<int>();
                    objoccupants_ids.Add(QuickBloxId);

                    try
                    {
                        objAddDialog.name = GroupName;
                        var objPullAll = new PullAll();
                        objPullAll.occupants_ids = objoccupants_ids;
                        objAddDialog.push_all = objPullAll;
                    }
                    catch (Exception e)
                    {

                    }

                    var clientAddMember = new System.Net.Http.HttpClient();

                    clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                    clientAddMember.DefaultRequestHeaders.Accept.Clear();
                    clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                    var jData2 = JsonConvert.SerializeObject(objAddDialog);
                    var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                    var response2 = await clientAddMember.PutAsync("", content2);
                    var result2 = response2.Content.ReadAsStringAsync().Result;

                    int StatusCode = Convert.ToInt32(response2.StatusCode);

                    var resultQB = (JObject)JsonConvert.DeserializeObject(result2);

                    if (StatusCode == 200)
                    {

                        var ChattingService = new ChattingServiceProxy();

                        result = ChattingService.AddMemberIntoGroup(ChattingGroupId, obj.ToString(), LoginUserId).Result;

                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteMobileServices";
                objErrorlog.Methodname = "AddMemberToGroupRestAPI";
                objErrorlog.UserID = "";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
                //  return null;
            }
            return 1;

        }



        public static async Task<int> AddGroupMemberToMultipleOfficesGroupRestAPI(string ChattingGroupId, string DialogId, string GroupName, string SchedulerEmail, int[] UserQBIds, string LoginUserId,string OfficeIds, int OrganisationId)
        {
            try
            {
                #region FOR SESION GENERATION

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
                #endregion

               List<int> UserList = UserQBIds.ToList<int>();

                #region  ADD MEMBER TO QUICKBLOX
                //foreach (var obj in UserQBIds)
                //{
                  //  int QuickBloxId = Convert.ToInt32(obj);
                    var objAddDialog = new AddDialog();
                    List<int> objoccupants_ids = new List<int>();
                // objoccupants_ids.Add(QuickBloxId);

                     objoccupants_ids.AddRange(UserList);
                    try
                    {
                        objAddDialog.name = GroupName;
                        var objPullAll = new PullAll();
                        objPullAll.occupants_ids = objoccupants_ids;
                        objAddDialog.push_all = objPullAll;
                    }
                    catch (Exception e)
                    {

                    }
                   
                    var clientAddMember = new System.Net.Http.HttpClient();

                    clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                    clientAddMember.DefaultRequestHeaders.Accept.Clear();
                    clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                    var jData2 = JsonConvert.SerializeObject(objAddDialog);
                    var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                    var response2 = await clientAddMember.PutAsync("", content2);
                    var result2 = response2.Content.ReadAsStringAsync().Result;

                    int StatusCode = Convert.ToInt32(response2.StatusCode);

                    var resultQB = (JObject)JsonConvert.DeserializeObject(result2);

                    if (StatusCode == 200)
                    {

                    var ChattingService = new ChattingServiceProxy();

                    ChattingController chattingController = new ChattingController();

                    DataTable dt= chattingController.GetAllMemberByOfficeWithMultipleOffices(LoginUserId, OfficeIds, OrganisationId, ChattingGroupId);

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("InsertMemberInChatroomInBulk", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MemberListForChatRoom", dt);
                            cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                            cmd.Parameters.AddWithValue("@OfficeIds", OfficeIds);

                            int i = cmd.ExecuteNonQuery();
                            con.Close();

                            if (i > 0)
                            {

                                return 1;

                                // result = ChattingService.AddMemberIntoGroup(ChattingGroupId, obj.ToString(), LoginUserId).Result;

                                //DataTable Dt=
                            }
                        }
                    }
               }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteMobileServices";
                objErrorlog.Methodname = "AddMemberToGroupRestAPI";
                objErrorlog.UserID = "";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
                //  return null;
            }
            return 1;
        }


        public DataTable GetAllMemberByOfficeWithMultipleOffices(string LoginUserId, string OfficeId, int OrganisationId, string ChattingGroupId )
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();

            DataTable dt = new DataTable();

            dt.Columns.Add("ChattingGroupId");
            dt.Columns.Add("UserId");
            dt.Columns.Add("Type");
            dt.Columns.Add("QBID");
            dt.Columns.Add("InsertDateTime");
          
            string result = "";
            try
            {
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllMemberByOffice",

                // office previously was in int , now i changed it in to nvarchar(max)

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllMemberByOffice",
                                                  Guid.Parse(LoginUserId), Convert.ToString(OfficeId), Convert.ToInt32(OrganisationId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        //var objGroupMemberDetail = new ChattingGroupMember();
                        //objGroupMemberDetail.UserId = item["UserId"].ToString();
                        //objGroupMemberDetail.MemberName = item["Name"].ToString();
                        //objGroupMemberDetail.QuickBloxId = item["QuickBloxId"].ToString();
                        //objGroupMemberDetail.Type = item["UserRole"].ToString();
                        //objGroupMemberDetailList.Add(objGroupMemberDetail);

                        dt.Rows.Add(ChattingGroupId, item["UserId"].ToString(), item["UserRole"].ToString(), item["QuickBloxId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllMemberByOffice";
                result = InsertErrorLog(objErrorlog);
            }

            return dt;
        }


        // Added by krunal pawar on 01-fab-2018 for assign multiple scheduler in the group
        #region Assign multiple scheduler in the group 
        public ActionResult AssignScheduler(int ChattingGroupId, string GroupName)
        {
            ViewBag.ChattingGroupId = ChattingGroupId;
            ViewBag.GroupName = GroupName;
            ViewBag.SchedulerList = GetUnassignedSchedulerList(ChattingGroupId.ToString());
            ViewBag.AssignedSchedulerDetail = GetAssignedSchedulerListGroupWise(ChattingGroupId.ToString());
            return PartialView("AssignScheduler");
        }

        public List<ScheduleInfo> GetUnassignedSchedulerList(string ChattingGroupId)
        {
            var SchedulerList = new List<ScheduleInfo>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                SchedulerList = ChattingService.GetUnassignedSchedulerList(ChattingGroupId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetUnassignedSchedulerList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return SchedulerList;
        }

        public List<ChattingGroupMember> GetAssignedSchedulerListGroupWise(string ID)
        {
            var objGroupMemberDetailListing = new List<ChattingGroupMember>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                objGroupMemberDetailListing = ChattingService.GetAssignedSchedulerListGroupWise(ID).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetAssignedCaregiverListGroupWise";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objGroupMemberDetailListing;
        }

        public JsonResult RemoveSchedulerFromGroupChat(string ChattingGroupId, string UserId, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
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
                    SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                }
                else
                {
                    SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                }

                Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, OrganisationId); }).Wait();
                result = ChattingService.RemoveMemberFromGroupChat(ChattingGroupId, UserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "RemoveSchedulerFromGroupChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            //return result;
            var lstScheduler = GetUnassignedSchedulerList(ChattingGroupId);
            return Json(new { result, lstScheduler }, JsonRequestBehavior.AllowGet);
        }



        #endregion

        public JsonResult GetPatientRoomGroupList(string QBGroupDialogIds, int GroupType)
        {
            //ChattingsList ChattingsList = new ChattingsList();

            int  OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            List<PatientChatList> ChattingsList = new List<PatientChatList>();
            try
            {
                FillAllOffices();

                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                var PatientChatModel = new PatientChatModel();

                PatientChatModel.QBGroupDialogIds = QBGroupDialogIds;
                PatientChatModel.GroupTypeID = GroupType;  // groupType = 1  for patient chat room

                ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();

                ChattingsList = ChattingLiteService.GetPatientRoomGroupList(PatientChatModel, LogInUserId, OrganisationId.ToString()).Result;
                //  ChattingsList = ChattingLiteService.GetPatientRoomGroupList(PatientChatModel,LogInUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetPatientRoomGroupList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                //return RedirectToAction("PatientGroupChatting", "Chatting");
            }

            return Json(ChattingsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOneToOnechatRoomGroupList(string QBGroupDialogIds)
        {
            //ChattingsList ChattingsList = new ChattingsList();

            List<PatientChatList> ChattingsList = new List<PatientChatList>();

            List<Chatting> ListChatting = new List<Chatting>();

            try
            {
                FillAllOffices();

                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                var PatientChatModel = new PatientChatModel();

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                PatientChatModel.QBGroupDialogIds = QBGroupDialogIds;

                // PatientChatModel.GroupTypeID = GroupType;  // groupType = 1  for patient chat room

                // ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
                //ChattingsList = ChattingLiteService.GetPatientRoomGroupList(PatientChatModel, LogInUserId).Result;
                //  ChattingsList = ChattingLiteService.GetPatientRoomGroupList(PatientChatModel,LogInUserId).Result;
                //DataSet ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOneToOneChatListByQbDialogs",

                DataSet ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOneToOneChatListByQbDialogs",
                                                     //  Guid.Parse(LogInUserId),
                                                     Convert.ToString(LogInUserId),
                                                        PatientChatModel.QBGroupDialogIds, OrganisationId);

                // PatientChatModel.GroupTypeID);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        Chatting objChatting = new Chatting();
                        objChatting.UserId = item["UserId"].ToString();
                        objChatting.NurseId = Convert.ToInt32(item["Id"]);
                        objChatting.CareGiverName = item["Name"].ToString();
                        objChatting.Role = item["Role"].ToString();
                        objChatting.QuickBloxDialogId = item["DialogId"].ToString();

                        // var ProfileImage = item["ProfileImage"].ToString();

                        //if (ProfileImage != null)
                        //{
                        //    objChatting.CaregiverProfileImage = CareGiverProfileImagesPath + ProfileImage;
                        //}
                        //else
                        //{
                        //    objChatting.CaregiverProfileImage = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                        //}
                        //
                        objChatting.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        objChatting.OfficeName = item["OfficeName"].ToString();

                        ListChatting.Add(objChatting);

                    }


                }

                // }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetPatientRoomGroupList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                //return RedirectToAction("PatientGroupChatting", "Chatting");
            }

            return Json(ListChatting, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PatientChatRoom()
        {
            FillAllOffices();
            return View();
        }

        public ActionResult GroupChat()
        {
            try
            {
                FillAllOffices();
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GroupChatNew";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return View();
        }

        // Chat Management System By Vinod Verma 16-10-2018

        string connection = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
        List<SelectListItem> NursesV = new List<SelectListItem>();

        public SelectList GetAllNurse()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                //SqlDataReader rdr = null;
                using (SqlCommand cmd = new SqlCommand("GetAllNursesSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            NursesV.Add(new SelectListItem { Text = rdr["Name"].ToString(), Value = rdr["UserId"].ToString() });
                        }
                    }
                    conn.Close();
                    return new SelectList(NursesV, "Value", "Text");
                }
            }
        }

        public ActionResult ChatManagement()
        {
            try
            {

                ViewBag.NurseList = GetAllNurse();

                //FillAllOffices();

                // Select All Office 

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                List<Office> OfficeList = new List<Office>();
                OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(),OrganisationId.ToString()).Result;
                SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");
                ViewBag.lstOffice = officeSelectList;

                // End Select All Office 


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "ChatManagementNew";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return View();
        }


        [HttpPost]
        public ActionResult ChatManagement(string ddlMember, int OfficeId)
        {
            //Int32[] qbIds = new Int32[] { };
            // var qbIds = "";
            string ConnectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
            //string ChatMemberType = "Caregiver";

            ChatManagementModel objChatManagementModel = new ChatManagementModel();

            //objGroupChatModel.OfficeId=collection.
            objChatManagementModel.UserId = ddlMember;
            objChatManagementModel.OfficeId = OfficeId;


            /*  Get QBID From UserID   */
            string txtQBID = string.Empty;
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT ISNULL(QuickBloxId,0) AS QuickBloxId FROM UserloginInfo WHERE UserId = @UserId", connection))
                {
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);

                    sqlCmd.Parameters.AddWithValue("@UserId", ddlMember);
                    sqlDa.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        // Int32.TryParse(dt.Rows[0]["QuickBloxId"].ToString(), out qbIds[0]);

                        txtQBID = dt.Rows[0]["QuickBloxId"].ToString();

                    }
                }
                connection.Close();

            }

            /*  End Get QBID From UserID   */
            var qbIds = txtQBID.Split(',').Select(Int32.Parse).ToArray();

            // Find the all chat groupID from given Office

            using (con = new SqlConnection(ConnectionString))
            { 
                con.Open();
            string CommandText = "SELECT ChattingGroupId" +
                                     "  FROM ChattingGroup" +
                                     " WHERE OfficeId=@OfficceId";

            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;

            cmd.Parameters.Add(
                    new SqlParameter("@OfficceId", System.Data.SqlDbType.NVarChar, 20, "OfficceId"));  // The name of the source column

            // Fill the parameter with the value retrieved
            // from the text field
            cmd.Parameters["@OfficceId"].Value = OfficeId;

                using (rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {

                        using (con = new SqlConnection(ConnectionString))
                        {
                            cmd = new SqlCommand("ChatManagementForAddUserToPatientGroup", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ChattingGroupId", rdr[0]);
                            cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                            cmd.Parameters.AddWithValue("@UserId", ddlMember);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            var ChattingService = new ChattingServiceProxy();

                            var objDialogDetail = new Chatting();
                            objDialogDetail = ChattingService.GetDialogDetail(rdr[0].ToString()).Result;


                            string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();

                            string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];


                            Task.Run(async () => { await AddMemberToGroupRestAPI(rdr[0].ToString(), objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, qbIds, LoginUserId); }).Wait();
                        }
                    }
                }
            }

            return RedirectToAction("ChatManagement", "Chatting");
        }


        // Chat Management System By pramendra 16-10-2021
        public ActionResult ChatManagementMember(int ChattingGroupId, string GroupName, int GroupTypeId)
        {
            //FillAllOffices();

            // Select All Office 
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            List<Office> OfficeList = new List<Office>();
            OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(),OrganisationId.ToString()).Result;
            SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");
            ViewBag.lstOffice = officeSelectList;

            // End Select All Office 


            ViewBag.ChattingGroupId = ChattingGroupId;
            ViewBag.GroupName = GroupName;
            ViewBag.GroupTypeId = GroupTypeId;
            ViewBag.MemberList = GetUnassignedMemberList(ChattingGroupId.ToString(), OrganisationId);
            ViewBag.AssignedMemberList = GetAssignedMemberListGroupWise(ChattingGroupId.ToString());
            return PartialView("ChatManagementMember");
        }


        //[OutputCache(Duration = 86400, VaryByParam ="none", Location =System.Web.UI.OutputCacheLocation.Client, NoStore =true)]

        public ActionResult OneToOneChat()

        {
            FillAllOffices();
            return View();
        }


        [OutputCache(Duration = 86400, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client, NoStore = true)]
        //public ActionResult GetOneToOneChatList(string searchFilter)
        //{
        //    var postModel = HttpContext.Cache.Get("OneToOneChatList");

        //    if (postModel == null)
        //    {
        //        postModel = this.GetOneToOneChatList1("||");
        //        HttpContext.Cache.Insert("OneToOneChatList", postModel, null, DateTime.Now.AddMinutes(86400), TimeSpan.FromMinutes(10));
        //    }

        //    return View(postModel);
        //}

        public JsonResult GetOneToOneChatList(string searchFilter)
        {
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                FillAllOffices();

                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                var PatientChatModel = new PatientChatModel();

                ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();

                // ************* this all param are static , Its not using in sp . Its putted bcoz if it need custom data table like functionality in future
                string sortOrder = string.Empty;
                sortOrder = "Name";
                var sortDirection = "ASC"; // asc or desc
                int pageNo = 1;
                int recordPerPage = 10;
                string search = "||"; //It's indicate blank filter
                string OfficeId = "0";

                search = searchFilter;
                //Cache["Employee"]
                // ************* 

                int OrganisationId = 0;
                if (!string.IsNullOrEmpty(Request["FilterOrganisationId"]))
                {
                    OrganisationId = Convert.ToInt32(Request["FilterOrganisationId"]);
                }
                else
                {
                    OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                }

                ChattingsList = ChattingLiteService.GetOneToOneChatList(LogInUserId, pageNo, recordPerPage, search, sortOrder, sortDirection, OfficeId, OrganisationId.ToString()).Result;
                //  ChattingsList = ChattingLiteService.GetPatientRoomGroupList(PatientChatModel,LogInUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetOneToOneChatList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                //return RedirectToAction("PatientGroupChatting", "Chatting");
            }

            return Json(ChattingsList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 86400, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client, NoStore = true)]
        public List<ScheduleInfo> GetALLSuperadminList()
        {
            var SchedulerList = new List<ScheduleInfo>();
            try
            {
                var ChattingService = new ChattingServiceProxy();
                SchedulerList = ChattingService.GetALLSuperadminList().Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "GetALLSuperadminList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return SchedulerList;
        }

        //public string DeletePrivateChat(string OfficeId, string UserId, string QuickBloxId)
        //{

        //    string result = "";

        //    try
        //    {
        //        string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();
        //        var ChattingService = new ChattingServiceProxy();
        //        var objDialogDetail = new Chatting();
        //        objDialogDetail = ChattingService.GetOfficeGroupDetailByOfficeId(OfficeId.ToString(), UserId).Result;

        //        //   if()

        //        if (!string.IsNullOrEmpty(objDialogDetail.UserId))
        //        {
        //            Task.Run(async () => { await RemoveCareGiverToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId); }).Wait();
        //            result = ChattingService.RemoveMemberFromGroupChat(objDialogDetail.ChattingGroupId.ToString(), UserId).Result;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "ChattingController";
        //        log.Methodname = "RemoveMemberFromOfficeGroup";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //        return result;
        //    }

        //    return result;
        //}



        //public ActionResult CreatePdf(string name, string quality, string description, string Productname)
        //{
        //    var ObjStatus = "";
        //    ObjStatus = subcreateFile(name, quality, description, Productname);
        //    return Json(ObjStatus, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult CreatePdf(List<PdfModel> things, string patientgroupname, string ChattingId, string OrdPhysicianName, string WoundCareType, string WoundLocation,string CatheterTypeSize,string FrequencyCathetersupplies,string Ostomysupplie)
        {

            //WoundCareType =
            //WoundLocation =
            //CatheterTypeSize =
            //FrequencyCathetersupplies =
            //Ostomysupplie =


            //  string Email="pramendra.singh@gmail.com";

            // int OrganisationId=convert.toint32(session["OrganisationId"]); 

            //  Task.Run(async () => { await ChattingController.GenerateUserQuickBloxIdRestAPItousertouser("",Email, 0, false,);OrganisationId }).Wait();

            //  GenerateSalesSupplyFormReport(patientgroupname);

            //List<MemberFornotification> MemberToNotify = new List<MemberFornotification>();

            //MemberToNotify.Add(new MemberFornotification { MemberToNotifyUser = "Ben Moss" });
            //MemberToNotify.Add(new MemberFornotification { MemberToNotifyUser = "Alok Babu" });
            //MemberToNotify.Add(new MemberFornotification { MemberToNotifyUser = "Superadmin UAT" });
            //MemberToNotify.Add(new MemberFornotification { MemberToNotifyUser = "Aaron" });

            //string ChatId = "6353";

            //string vMsg = "hello";
            //string UserName = "Ben moss";
            //string GroupName = "ToNotifiedUserForTagged";
            //string DialogId = "5689093564885i9hghjhfjktrytg";
            //string type = "1";

            //string MemberToNotify = "Alok Babu";


            //MemberToNotifyforTaggedNotification(MemberToNotify, ChatId, vMsg, UserName, GroupName, DialogId, type);

            //string QBID = "32563876";
            //string OfficeNaame = "Arizona";
            //int grouptypeid = 1;

            //string QBID = "32780213";
            //string OfficeNaame = "CA-SD";
            //int grouptypeid = 1;


            //getDialogListACCOROfficename(QBID, OfficeNaame, grouptypeid);


            //   GetAllUnreadMessagesToRead();

            // List<int> offcidddss = new List<int>();

            // //offcidddss.Add(4); offcidddss.Add(5);
            // //offcidddss.Add(6);

            // offcidddss.Add(12);offcidddss.Add(14);
            // offcidddss.Add(16);
            //offcidddss.Add(19);
            // offcidddss.Add(20);
            // //offcidddss.Add(21);
            // //offcidddss.Add(22);offcidddss.Add(23);
            // //offcidddss.Add(24);

            // for (int i = 0; i <= offcidddss.Count; i++)
            // {
            //     string userid = "FF09896D-0580-482C-93F2-8D86F71B0FE9";
            //     string Email = "dmarshman@homecareforyou.com";
            //     string officeid = Convert.ToString(offcidddss[i]);
            //     bool IsAllowGroupChat = false;
            //     string QuickBloxId = "32420887";


            //    AddMemberIntoOfficeGroup(officeid, userid, QuickBloxId);


            //     // Task.Run(async () => { await GenerateUserQuickBloxIdRestAPItouser(userid, Email, officeid, IsAllowGroupChat); }).Wait();

            // }

            //getandinsertData();


            var ObjStatus = "";

            ObjStatus = subcreateFile(things, patientgroupname, ChattingId, OrdPhysicianName, WoundCareType,WoundLocation, CatheterTypeSize, FrequencyCathetersupplies, Ostomysupplie);


            return Json(ObjStatus, JsonRequestBehavior.AllowGet);

        }


        // private string subcreateFile(string name, string quality, string description, string Productname)

        private string subcreateFile(List<PdfModel> things, string patientgroupname, string ChattingId, string OrdPhysicianName, string WoundCareType, string WoundLocation, string CatheterTypeSize, string FrequencyCathetersupplies, string Ostomysupplie)
        {

            DataTable Dt = new DataTable();

            //=GetDialogDetail(Id.ToString());


            //List<PdfModel> objpdf = new List<PdfModel>
            //{
            //    new PdfModel {name="ghhgjhhj",quality="gfhjk",description="gghghjj" },
            //    new PdfModel {name="ghjkl",quality="ghjkloiu",description="drtyuijk" },
            //    new PdfModel {name="gyuhijkl",quality="ftgyhjkl",description="gfhjkjkijii"}
            //};




            var dateTime = DateTime.Now;
            var longDateValue = dateTime.ToString("dd-MMM-yyyy");

            string OfficeName = "";
            string OfficeAddress = "";

            string[] patientstr = patientgroupname.Split('(');
            string Patientname = patientstr[0];

            string[] strmedicalid = patientstr[1].Split(')');
            string medicalId = strmedicalid[0];

            Dt.Columns.Add("GroupName");
            Dt.Columns.Add("EmployeeName");
            Dt.Columns.Add("ProductName");
            Dt.Columns.Add("Quantity");
            Dt.Columns.Add("Description");
            Dt.Columns.Add("CreatedDate");

            //MembershipUser user = Membership.GetUser();
            //string[] roles = Roles.GetRolesForUser(user.UserName);
            //string LogInUserId = user.ProviderUserKey.ToString();

            List<OfficeModel> listOffices = GetAddressAndOfficename(ChattingId);

            foreach (var officelist in listOffices)
            {
                OfficeName = officelist.OfficeName;
                OfficeAddress = officelist.Address;
            }

            var ObjStatus = "";
            string img = Server.MapPath("~/Content/image/paseva.jpg");

            // string word = Convert.ToString(quality);
            StringBuilder sb = new StringBuilder();

            sb.Append("<header class='clearfix'>");
            sb.Append("<img align='center' src='" + img + "'width='80px' height='60px'/>");
            sb.Append("<br>");
            sb.Append("<b><p style='background:#1dca75;text-align:center'>Suppy Form</p></b>");

            sb.Append("<br>");
            sb.Append("<b><p style='width:33%;'><b>Ordering Physician Name:</b><span style='color:#4090ca;padding-left: 5px;margin-left: 10px;'>" + OrdPhysicianName + "</span></p></b>");
            sb.Append("<br>");
            sb.Append("</header>");

            sb.Append("<table align='center' style='width:100%;'>");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<td style='width:33%;'><b>Wound Care Type:</b><span style='color:#4090ca;padding-left: 5px;'>" + WoundCareType+ "</span></td>");
            sb.Append("<td style='width:33%;'><b>Wound Location:</b><span style='color:#4090ca;padding-left: 0px;'>" + WoundLocation+ "</span></td>");
            sb.Append("<td style='width:33%;'></td>");
            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<br>");
            sb.Append("<br>");

            sb.Append("<table align='center' style='width: 100 %;'>");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<td style'width:33%;'> <b>Catheter Type Size:</b> <span style='color:#4090ca;padding-left: 5px;'>" + CatheterTypeSize + "</span></td>");
            sb.Append("<td style='width:33%;'><b>Frequency of change for Catheter supplies:</b> <span style='color:#4090ca;padding-left: 5px;'>" + FrequencyCathetersupplies + "</span></td>");
            sb.Append("<td style='width:33%;'><b>Ostomy type for Ostomy supplie:</b> <span style='color:#4090ca;padding-left: 5px;'>" + Ostomysupplie + "</span></td>");

            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<br>");
            sb.Append("<br>");

            sb.Append("<table align='center' style='width: 100 %;'>");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<td style'width:33%;'> <b>Patient Name:</b> <span style='color:#4090ca;padding-left: 5px;'>" + Patientname + "</span></td>");
            sb.Append("<td style='width:33%;'></td>");
            sb.Append("<td style='width:33%;'><b>Employee Name:</b> <span style='color:#4090ca;padding-left: 5px;'>" + Session["name"] + "</span></td>");

            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<table align='center' style='width: 100 %;'>");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<td style'width:33%;'> <b>Office Name:</b> <span style='color:#4090ca;padding-left: 5px;'>" + OfficeName + "</span></td>");
            sb.Append("<td style='width:33%;'><b>Office Address:</b> <span style='color:#4090ca;padding-left: 5px;'>" + OfficeAddress + "</span></td>");
            sb.Append("<td style='width:33%;'><b>Date:</b> <span style='color:#4090ca;padding-left: 5px;'>" + longDateValue + "</span></td>");

            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<br>");
            sb.Append("<br>");


            //sb.Append("<table align='center' style='width: 100 %;'>");
            //sb.Append("<tbody>");
            //sb.Append("<tr>");
            //sb.Append("<td style'width:33%;'> <b>Office Name:</b> <span style='color:#4090ca;padding-left: 5px;'>" + OfficeName + "</span></td>");
            //sb.Append("<td style='width:33%;'><b>Office Address:</b> <span style='color:#4090ca;padding-left: 5px;'>" + OfficeAddress + "</span></td>");
            //sb.Append("<td style='width:33%;'><b>Date:</b> <span style='color:#4090ca;padding-left: 5px;'>" + DateTime.Now + "</span></td>");

            //sb.Append("</tr>");
           // sb.Append("</tbody>");
           // sb.Append("</table>");

            sb.Append("<br>");
            sb.Append("<br>");


            sb.Append("<table border=1 align='center'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='width:3px;color:#1dca75;font-weight:700;'>Product Name</th>");
            sb.Append("<th style='width:20px;color:#1dca75;font-weight:700;'>Quantity</th>");
            sb.Append("<th style='width:20px;color:#1dca75;font-weight:700;'>Description</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            sb.Append("<tr>");

            foreach (var e1 in things)
            {
                sb.Append("<td>" + e1.name + "</td>");

                sb.Append("<td>" + e1.quality + "</td>");
                sb.Append("<td>" + e1.description + "</td>");
                Dt.Rows.Add(patientgroupname, Session["name"], e1.name, e1.quality, e1.description, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            }

            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("</main>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<br>");

            sb.Append("<footer>");
            sb.Append("Note:*report is created.");
            sb.Append("<br>");
            sb.Append(".............................................................................");
            sb.Append("</footer>");

            StringReader sr = new StringReader(sb.ToString());
            Document pdfdocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            HTMLWorker htmlparse = new HTMLWorker(pdfdocument);

            string PdfFilePath = ConfigurationManager.AppSettings["DownlLoadFilePath1"].ToString();

            ////file name to be created  

            string strPDFFileName = string.Format("SupplyReport_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "-" + ".pdf");

            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

            using (MemoryStream memorystream = new MemoryStream())
            {

                PdfWriter writer = PdfWriter.GetInstance(pdfdocument, memorystream);
                pdfdocument.Open();
                htmlparse.Parse(sr);
                pdfdocument.Close();

                byte[] bytes = memorystream.ToArray();

                System.IO.File.WriteAllBytes(strAttachment, bytes);

                memorystream.Close();

             //CreateMessage(strAttachment);

                string ExcelUrl = PdfFilePath + strPDFFileName;
                ObjStatus = ExcelUrl;
                //Email sending started 

                string AttachmentFileName = strAttachment;

                // string toAddress = "pramendrasingh8022@gmail.com"; 

                string toAddress = ConfigurationManager.AppSettings["Mail"].ToString();

                string subject = "Supply Form";

                bool IsFileAttachment = true;

                string body = " ";

                string CCMailID = " ";

                bool isBodyHtml = true;

               if (sendEmailWithAttachment(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("InsertPatientSupplies", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmployeeName", Session["name"]);
                            cmd.Parameters.AddWithValue("@officeName", OfficeName);
                            cmd.Parameters.AddWithValue("@PatientName", Patientname);
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            cmd.Parameters.AddWithValue("@GroupName", patientgroupname);
                            cmd.Parameters.AddWithValue("@SalesFormUrl", ObjStatus);
                            cmd.Parameters.AddWithValue("@supplyDescriptionList", Dt);
                            cmd.Parameters.AddWithValue("@WoundCareType", WoundCareType);
                            cmd.Parameters.AddWithValue("@WoundLocation", WoundLocation);
                            cmd.Parameters.AddWithValue("@CatheterTypeSize", CatheterTypeSize);
                            cmd.Parameters.AddWithValue("@ostomyType", Ostomysupplie);
                            cmd.Parameters.AddWithValue("@FrequencyOfChange", FrequencyCathetersupplies);
                            cmd.Parameters.AddWithValue("@OrdPhysicianName", OrdPhysicianName);

                            int i = cmd.ExecuteNonQuery();

                            if (i > 0)
                            {

                                AddMemberToChatRoomHavingSupplyForm(Convert.ToInt32(ChattingId), 1);

                                return ObjStatus;
                            }
                        }
                        con.Close();
                    }
                }

                return ObjStatus;
            }

        }


        public void AddMemberToChatRoomHavingSupplyForm(int ChattingId, int grouptypeid)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[GetSchedulerToChatRoom]", con))
                { 
                    cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingId);
                cmd.Parameters.AddWithValue("@GrouptypeId", grouptypeid);
                //cmd.Parameters.AddWithValue("@OfficeName", OfficeName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            string ChattingGroupId = ds.Tables[0].Rows[i]["ChattingGroupId"].ToString();
                            string GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                            string DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                            string OfficeId = ds.Tables[0].Rows[i]["OfficeId"].ToString();
                            string MasterChattingGroupTypeID = ds.Tables[0].Rows[i]["MasterChattingGroupTypeID"].ToString();
                            String QBID = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();

                            // Patients.Password = ds.Tables[0].Rows[0]["Password"].ToString();

                            // List<string> qbids = new List<string>();
                            //qbids.Add("32158385");
                            //      qbids.Add("32448043");
                            //      //qbids.Add("32158386");
                            //      //qbids.Add("32328543");


                            //foreach (var e1 in qbids)
                            //{

                            //    string GroupName = "Hall,Emily(SD200830015403)";
                            //    string DialogId = "5f525753d4594d03ae11349e";
                            string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                            //    string ChattingGroupId = "";
                            //    //string qbid = "32158385";
                            // Task.Run(async () => { await AddOfficeStaffToGroupRestAPI(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, ChattingGroupId, NurseCoordinatorId, Permission); }).Wait();

                            Task.Run(async () => { await AddSchdulerTOHavingSupplyInChatRoom(DialogId, GroupName, SchedulerEmail, QBID, ChattingGroupId); }).Wait();

                            //if (i == 54)
                            //{
                            //   // break;
                            //}
                        }
                    }
                }
            }


        }



        public static async Task<int> AddSchdulerTOHavingSupplyInChatRoom(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId, string ChattingGroupId)
        {
            try
            {

                string abcd = "";

                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
                //con.Open();
                //SqlCommand cmd = new SqlCommand("SetNurseCoordinatorAndOfficeManager", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                //cmd.Parameters.AddWithValue("@NurseCoordinatorId", NurseCoordinatorId);
                //cmd.Parameters.AddWithValue("@Permission", Permission);
                //cmd.Parameters.AddWithValue("@QBID", CaregiverQBId);
                //abcd = Convert.ToString(cmd.ExecuteScalar());
                //var ChattingService = new ChattingServiceProxy();

                //abcd = ChattingService.SetNurseCoordinatorAndOfficeManager(ChattingGroupId, NurseCoordinatorId, Permission, CaregiverQBId).Result;


                #region FOR SESION GENERATION

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
                #endregion

                #region  ADD MEMBER TO QUICKBLOX

                int QuickBloxId = Convert.ToInt32(CaregiverQBId);

                var objAddDialog = new AddDialog();
                List<int> objoccupants_ids = new List<int>();
                objoccupants_ids.Add(QuickBloxId);

                try
                {
                    objAddDialog.name = GroupName;
                    var objPullAll = new PullAll();
                    objPullAll.occupants_ids = objoccupants_ids;
                    objAddDialog.push_all = objPullAll;
                }
                catch (Exception e)
                {

                }

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;


                int StatusCode = Convert.ToInt32(response2.StatusCode);

                var resultQB = (JObject)JsonConvert.DeserializeObject(result2);


                if (StatusCode == 200 || StatusCode == 400)
                {
                    var ChattingService1 = new ChattingServiceProxy();
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("AddSchedulerInSupplyFormChatRoom", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                            //cmd.Parameters.AddWithValue("@NurseCoordinatorId", NurseCoordinatorId);
                            //cmd.Parameters.AddWithValue("@Permission", Permission);
                            cmd.Parameters.AddWithValue("@QBID", CaregiverQBId);
                            abcd = Convert.ToString(cmd.ExecuteScalar());
                        }
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteWebServices";
                objErrorlog.Methodname = "AddSchdulerTOHavingSupplyInChatRoom";
                objErrorlog.UserID = "";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
                //  return null;
            }
            return 1;

        }







        //private async void CreateMessage( string attachFile)
        //{

        //    string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();
        //    var client = new System.Net.Http.HttpClient();

        //    client.BaseAddress = new Uri(QuickbloxAPIUrl);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

        //    Random random = new Random();
        //    int Vnonce = random.Next(0, 9999);

        //    var input = new QuickBloxSession();
        //    input.application_id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
        //    input.auth_key = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
        //    input.nonce = Vnonce.ToString();
        //    input.timestamp = QuickBloxServiceProxy.Timestamp();

        //    input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
        //    //Encryption            
        //    input.signature = QuickBloxServiceProxy.Hash(input.signature, System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

        //    var userData = new Userdata();
        //    userData.login = SchedulerEmail; //"superadmin@caregiver.com"
        //    userData.password = "Welcome007!";
        //    input.user = userData;

        //    var jData1 = JsonConvert.SerializeObject(input);
        //    var content1 = new StringContent(jData1);

        //    var content = new StringContent(jData1, Encoding.UTF8, "application/json");
        //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //    var response = await client.PostAsync("/session.json", content);
        //    var result = response.Content.ReadAsStringAsync().Result;
        //    //JObject json = JObject.Parse(result);
        //    var data = (JObject)JsonConvert.DeserializeObject(result);
        //    string token = data["session"]["token"].Value<string>();

        //    ////Sessoin Generated End

        //    //  GetActivePatientRequest All Dialog Detail
        //    var clientGetDialogId = new System.Net.Http.HttpClient();

        //    clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
        //    clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
        //    clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
        //    var response1 = await clientGetDialogId.GetAsync("");
        //    var result1 = response1.Content.ReadAsStringAsync().Result;


        //}

        public List<OfficeModel> GetAddressAndOfficename(string ChattingId)
        {
            List<OfficeModel> listOffices = new List<OfficeModel>();

            OfficeModel OfficeModel = new OfficeModel();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetOfficeNamePdfCreate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChattingId ", ChattingId);
                    // OfficeName = Convert.ToString(cmd.ExecuteScalar());
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            if (dr.Read())
                            {
                                OfficeModel.OfficeName = dr[0].ToString();
                                OfficeModel.Address = dr[1].ToString();
                            }

                            listOffices.Add(OfficeModel);
                        }
                    }
                }
            }
            return listOffices;
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
                //if (!(string.IsNullOrEmpty(CCMailID)))
                //    mailMessage.CC.Add(CCMailID);
                //  MailAddress ma = new MailAddress("pramendrasingh400@gmail.com", "singhtripty");
                // MailAddress ma = new MailAddress("pramendrasingh400@gmail.com");

                MailAddress ma = new MailAddress(ConfigurationManager.AppSettings["ChatOutLookMail"].ToString());
                mailMessage.From = ma;

                mailMessage.Subject = subject;

                //LinkedResource Signature = null;

                if (IsFileAttachment == true)
                {
                    if (!string.IsNullOrEmpty(AttachmentFileName))
                    {
                        // mailMessage.Attachments.Add(AttachmentFileName);

                        Attachment attachFile = new Attachment(AttachmentFileName);

                        mailMessage.Attachments.Add(attachFile);
                    }
                }

                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                var smtpClient = new SmtpClient { EnableSsl = false };
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Host = ConfigurationManager.AppSettings["SMTPHostOutlook"];

                //smtpClient.Host ="smtp.live.com";
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.UseDefaultCredentials = false;

                // mailMessage.Bcc.Add(bccAddress);
                // smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                // smtpClient.Credentials = new NetworkCredential("pramendrasingh400@gmail.com","singhtripty");

                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ChatOutLookMail"].ToString(), ConfigurationManager.AppSettings["ChatOutlookPassword"].ToString());
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
            }
            return false;
        }


        public ActionResult LoadPartial()
        {
           // fetch from DB or other logic
            return PartialView("AuthFormView");
        }


        public ActionResult SupplyFormDetails(string ChattingGroupId, string GroupName)
        {
            //  ViewBag.ChattingGroupId = ChattingGroupId;
            // ViewBag.GroupName = GroupName;

            ViewBag.SupplyFormDetailslist = GetSupplyFormData(ChattingGroupId.ToString(), GroupName);
            List<SupplyFormData> ListSupplyFormData = ViewBag.SupplyFormDetailslist;

                return PartialView("SupplyFormDetails");
        }


        public List<SupplyFormData> GetSupplyFormData(string chattingId, string GroupName)
        {
            string result = "";
            SupplyFormData supplyformdata = new SupplyFormData();

            string SessionName = Session["name"].ToString();

            List<SupplyFormData> SupplyFormDataList = new List<SupplyFormData>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {

                using (SqlCommand cmd = new SqlCommand("GetSupplyFormData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@GroupName", GroupName);
                    // cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@CreateDate", " ");
                    cmd.Parameters.AddWithValue("@EmployeeName", SessionName);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();

                    con.Open();
                    da.Fill(ds);

                    
                    // List<PdfDataDescription> pdflist = new List<PdfDataDescription>();

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {

                            List<PdfDataDescription> pdflist = new List<PdfDataDescription>();

                            //string productCheck = ds.Tables[0].Rows[i]["ProductName"].ToString();
                            //string DescriptionCheck= ds.Tables[0].Rows[i]["Description"].ToString();
                            //string QuantitysCheck= ds.Tables[0].Rows[i]["Quantity"].ToString();

                            //if (ds.Tables[0].Rows[i]["ProductName"].ToString() != " " && ds.Tables[0].Rows[i]["Quantity"].ToString() != " " && ds.Tables[0].Rows[i]["Description"].ToString() != " ")
                            //{
                            //if (ds.Tables[0].Rows[i]["ProductName"].ToString() != null && ds.Tables[0].Rows[i]["Quantity"].ToString() != null && ds.Tables[0].Rows[i]["Description"].ToString() != null)
                            //{
                            //    SupplyFormData supplyformdatas = new SupplyFormData();
                            //    PdfDataDescription pdfdata = new PdfDataDescription();

                            //    supplyformdatas.EmployeeName = ds.Tables[0].Rows[i]["EmployeeName"].ToString();
                            //    supplyformdatas.OfficeName = ds.Tables[0].Rows[i]["officeName"].ToString();
                            //    supplyformdatas.PatientGroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                            //    supplyformdatas.SupplyFormUrl = ds.Tables[0].Rows[i]["SalesFormUrl"].ToString();
                            //    supplyformdatas.CreatedDate = ds.Tables[0].Rows[i]["CreatedDate"].ToString();

                            ////pdfdata.ProductName= ds.Tables[0].Rows[i]["ProductName"].ToString();
                            ////pdfdata.Quantity= ds.Tables[0].Rows[i]["Quantity"].ToString();
                            ////pdfdata.Description= ds.Tables[0].Rows[i]["Description"].ToString();

                            ////pdflist.Add(pdfdata);

                            //supplyformdatas.ProductNames = ds.Tables[0].Rows[i]["ProductName"].ToString();
                            //    supplyformdatas.Quantitys = ds.Tables[0].Rows[i]["Quantity"].ToString();
                            //    supplyformdatas.Descriptions = ds.Tables[0].Rows[i]["Description"].ToString();

                            //   // supplyformdata.SupplyFormDetails = pdflist;

                            //    supplyformdatas.CreatedDate = ds.Tables[0].Rows[i]["CreatedDate"].ToString();

                            //    SupplyFormDataList.Add(supplyformdatas);
                            // }


                            SupplyFormData supplyformdatas = new SupplyFormData();

                            supplyformdatas.EmployeeName = ds.Tables[1].Rows[i]["EmployeeName"].ToString();
                            supplyformdatas.OfficeName = ds.Tables[1].Rows[i]["officeName"].ToString();
                            supplyformdatas.PatientGroupName = ds.Tables[1].Rows[i]["GroupName"].ToString();
                            supplyformdatas.SupplyFormUrl = ds.Tables[1].Rows[i]["SalesFormUrl"].ToString();
                            supplyformdatas.CreatedDate = ds.Tables[1].Rows[i]["CreatedDate"].ToString();


                            //for(int j = 4*i; j <(4*i)+4; j++)
                            //{

                            //    string checkdatetab2 = ds.Tables[2].Rows[j]["CreatedDate"].ToString();
                            //    string checkgroupname2 = ds.Tables[2].Rows[j]["GroupName"].ToString();
                            //    if (checkdatetab2 == supplyformdatas.CreatedDate && supplyformdatas.PatientGroupName == checkgroupname2)
                            //    {


                            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                            {

                                using (SqlCommand cmd1 = new SqlCommand("GetSupplyFormData", con1))
                                {
                                    cmd1.CommandType = CommandType.StoredProcedure;

                                    cmd1.Parameters.AddWithValue("@GroupName", GroupName);
                                    //cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                    cmd1.Parameters.AddWithValue("@CreateDate", ds.Tables[1].Rows[i]["CreatedDate"].ToString());
                                    cmd1.Parameters.AddWithValue("@EmployeeName", SessionName);
                                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

                                    DataSet ds1 = new DataSet();

                                    con1.Open();
                                    da1.Fill(ds1);
                                    //PdfDataDescription pdfdatas = new PdfDataDescription();
                                    for (int j = 0; j < ds1.Tables[2].Rows.Count; j++)
                                    {

                                        //pdflist.Clear();
                                        PdfDataDescription pdfdatas = new PdfDataDescription();
                                        pdfdatas.ProductName = ds1.Tables[2].Rows[j]["ProductName"].ToString();
                                        pdfdatas.Quantity = ds1.Tables[2].Rows[j]["Quantity"].ToString();
                                        pdfdatas.Description = ds1.Tables[2].Rows[j]["Description"].ToString();
                                        pdfdatas.CreatedDate = ds1.Tables[2].Rows[j]["CreatedDate"].ToString();
                                        pdfdatas.PatientGroupName = ds1.Tables[2].Rows[j]["GroupName"].ToString();

                                        //  pdflist.Add(pdfdatas);

                                        // supplyformdatas.SupplyFormDetails = pdflist;

                                        //if (pdflist.Count == 4)
                                        //{
                                        pdflist.Add(pdfdatas);
                                        supplyformdatas.SupplyFormDetails = pdflist;

                                        //}

                                    }

                                    // // supplyformdatas.SupplyFormDetails = pdflist;


                                    //}


                                    //  }
                                    //pdfdata.ProductName= ds.Tables[0].Rows[i]["ProductName"].ToString();
                                    //pdfdata.Quantity= ds.Tables[0].Rows[i]["Quantity"].ToString();
                                    //pdfdata.Description= ds.Tables[0].Rows[i]["Description"].ToString();
                                    //pdflist.Add(pdfdata);
                                    //supplyformdatas.ProductNames = ds.Tables[0].Rows[i]["ProductName"].ToString();
                                    //supplyformdatas.Quantitys = ds.Tables[0].Rows[i]["Quantity"].ToString();
                                    //supplyformdatas.Descriptions = ds.Tables[0].Rows[i]["Description"].ToString();


                                    //supplyformdatas.SupplyFormDetails = pdflist;
                                    //supplyformdata.SupplyFormDetails = pdflist;

                                    //supplyformdatas.CreatedDate = ds.Tables[0].Rows[i]["CreatedDate"].ToString();




                                    SupplyFormDataList.Add(supplyformdatas);


                                }
                            }

                            //  }
                        }


                        // SupplyFormDataList.Add(supplyformdatas);



                    }
                }

            }

            return SupplyFormDataList.ToList();

         }


        public void getDialogListACCOROfficename(string QBID = "32448043", string OfficeName = "CA-SD", int grouptypeid = 1)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetChatroomListrealtedData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QBID", QBID);
                    cmd.Parameters.AddWithValue("@GrouptypeId", grouptypeid);
                    cmd.Parameters.AddWithValue("@OfficeName", OfficeName);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            string ChattingGroupId = ds.Tables[0].Rows[i]["ChattingGroupId"].ToString();
                            string GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                            string DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                            string OfficeId = ds.Tables[0].Rows[i]["OfficeId"].ToString();
                            string MasterChattingGroupTypeID = ds.Tables[0].Rows[i]["MasterChattingGroupTypeID"].ToString();
                            // Patients.Password = ds.Tables[0].Rows[0]["Password"].ToString();

                            // List<string> qbids = new List<string>();
                            //qbids.Add("32158385");
                            //      qbids.Add("32448043");
                            //      //qbids.Add("32158386");
                            //      //qbids.Add("32328543");


                            //foreach (var e1 in qbids)
                            //{

                            //    string GroupName = "Hall,Emily(SD200830015403)";
                            //    string DialogId = "5f525753d4594d03ae11349e";
                            string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"];
                            //    string ChattingGroupId = "";
                            //    //string qbid = "32158385";
                            // Task.Run(async () => { await AddOfficeStaffToGroupRestAPI(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId, ChattingGroupId, NurseCoordinatorId, Permission); }).Wait();

                            Task.Run(async () => { await AddOfficeStaffToGroupRestAPITest(DialogId, GroupName, SchedulerEmail, QBID, ChattingGroupId, "1234", "3"); }).Wait();

                            //if (i == 54)
                            //{
                            //   // break;
                            //}
                        }

                    }
                }

            }
         
        }

        public static async Task<int> AddOfficeStaffToGroupRestAPIUAT(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId, string ChattingGroupId, string NurseCoordinatorId, string Permission)
        {
            try
            {

                string abcd = "";

                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
                //con.Open();
                //SqlCommand cmd = new SqlCommand("SetNurseCoordinatorAndOfficeManager", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                //cmd.Parameters.AddWithValue("@NurseCoordinatorId", NurseCoordinatorId);
                //cmd.Parameters.AddWithValue("@Permission", Permission);
                //cmd.Parameters.AddWithValue("@QBID", CaregiverQBId);
                //abcd = Convert.ToString(cmd.ExecuteScalar());
                //var ChattingService = new ChattingServiceProxy();

                //abcd = ChattingService.SetNurseCoordinatorAndOfficeManager(ChattingGroupId, NurseCoordinatorId, Permission, CaregiverQBId).Result;


                #region FOR SESION GENERATION

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
                #endregion

                #region  ADD MEMBER TO QUICKBLOX

                int QuickBloxId = Convert.ToInt32(CaregiverQBId);

                var objAddDialog = new AddDialog();
                List<int> objoccupants_ids = new List<int>();
                objoccupants_ids.Add(QuickBloxId);

                try
                {
                    objAddDialog.name = GroupName;
                    var objPullAll = new PullAll();
                    objPullAll.occupants_ids = objoccupants_ids;
                    objAddDialog.push_all = objPullAll;
                }
                catch (Exception e)
                {

                }

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;


                int StatusCode = Convert.ToInt32(response2.StatusCode);

                var resultQB = (JObject)JsonConvert.DeserializeObject(result2);


                if (StatusCode == 200 || StatusCode == 400)
                {
                    var ChattingService1 = new ChattingServiceProxy();
                    result = ChattingService1.SetNurseCoordinatorAndOfficeManager(ChattingGroupId, NurseCoordinatorId, Permission, CaregiverQBId).Result;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SetNurseCoordinatorAndOfficeManager", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                            cmd.Parameters.AddWithValue("@NurseCoordinatorId", NurseCoordinatorId);
                            cmd.Parameters.AddWithValue("@Permission", Permission);
                            cmd.Parameters.AddWithValue("@QBID", CaregiverQBId);
                            abcd = Convert.ToString(cmd.ExecuteScalar());
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteMobileServices";
                objErrorlog.Methodname = "AddMemberToGroupRestAPI";
                objErrorlog.UserID = "";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
                //  return null;
            }
            return 1;

        }

        public static async Task<int> AddOfficeStaffToGroupRestAPITest(string DialogId, string GroupName, string SchedulerEmail, string CaregiverQBId, string ChattingGroupId, string NurseCoordinatorId, string Permission)
        {
            try
            {
                string abcd = "";

                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
                //con.Open();
                //SqlCommand cmd = new SqlCommand("SetNurseCoordinatorAndOfficeManager", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                //cmd.Parameters.AddWithValue("@NurseCoordinatorId", NurseCoordinatorId);
                //cmd.Parameters.AddWithValue("@Permission", Permission);
                //cmd.Parameters.AddWithValue("@QBID", CaregiverQBId);
                //abcd = Convert.ToString(cmd.ExecuteScalar());
                //var ChattingService = new ChattingServiceProxy();

                //abcd = ChattingService.SetNurseCoordinatorAndOfficeManager(ChattingGroupId, NurseCoordinatorId, Permission, CaregiverQBId).Result;

                #region FOR SESION GENERATION

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
                userData.login = SchedulerEmail;     //"superadmin@caregiver.com"
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
                #endregion

                #region  ADD MEMBER TO QUICKBLOX

                int QuickBloxId = Convert.ToInt32(CaregiverQBId);

                var objAddDialog = new AddDialog();
                List<int> objoccupants_ids = new List<int>();
                objoccupants_ids.Add(QuickBloxId);

                try
                {
                    objAddDialog.name = GroupName;
                    var objPullAll = new PullAll();
                    objPullAll.occupants_ids = objoccupants_ids;
                    objAddDialog.push_all = objPullAll;
                }
                catch (Exception e)
                {

                }

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;

                int StatusCode = Convert.ToInt32(response2.StatusCode);

                var resultQB = (JObject)JsonConvert.DeserializeObject(result2);

                if (StatusCode == 200 || StatusCode==400)
                {
                    var ChattingService1 = new ChattingServiceProxy();
                    result = ChattingService1.SetNurseCoordinatorAndOfficeManager(ChattingGroupId, NurseCoordinatorId, Permission, CaregiverQBId).Result;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SetNurseCoordinatorAndOfficeManager", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                            cmd.Parameters.AddWithValue("@NurseCoordinatorId", NurseCoordinatorId);
                            cmd.Parameters.AddWithValue("@Permission", Permission);
                            cmd.Parameters.AddWithValue("@QBID", CaregiverQBId);
                            abcd = Convert.ToString(cmd.ExecuteScalar());
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteMobileServices";
                objErrorlog.Methodname = "AddMemberToGroupRestAPI";
                objErrorlog.UserID = "";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
                //  return null;
            }
            return 1;

        }

        public string AddMemberCaregiverInPatientChatRoom(string ChattingGroupId, string UserIds, string QuickBloxId)
        {
            string result = "";
            string SchedulerEmail = "";
            try
            {

                var ChattingService = new ChattingServiceProxy();

                var objDialogDetail = new Chatting();
                objDialogDetail = ChattingService.GetDialogDetail(ChattingGroupId).Result;
                MembershipUser user = Membership.GetUser();
                //    var SchedulerEmail = user.Email;

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                if (OrganisationId > 0)
                {

                   SchedulerEmail = Session["OrgSuperAdminEmail"].ToString();
                }
                else
                {
                    SchedulerEmail = "superadmin@paseva.com";
                    // ConfigurationManager.AppSettings["SuperAdminEmailId"];
                }

                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();
                var QuickBloxIds = QuickBloxId.Split(',');

                //var qbIds = QuickBloxId.Split(',').Select(Int32.Parse).ToArray();
                var qbIds = QuickBloxId.Split(',').ToArray();

                for (int i = 0; i <= qbIds.Length; i++)
                {

                    Task.Run(async () => { await AddCaregiverMemberToGroupRestAPI(ChattingGroupId, objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, qbIds[i], LoginUserId); }).Wait();

                    result = "Success";

                }

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddMemberIntoGroup_V1";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        public static async Task<int> AddCaregiverMemberToGroupRestAPI(string ChattingGroupId, string DialogId, string GroupName, string SchedulerEmail, string UserQBIds, string LoginUserId)
        {

            string result1 = "";

            try
            {
                #region FOR SESION GENERATION

                string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();
                // string QuickbloxAPIUrl = "https://api.quickblox.com";
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
                // Welcome007!

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, System.Configuration.ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content1 = new StringContent(jData1);

                //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                //JObject json = JObject.Parse(result);
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();


                // string token = "c21956a8d3556f7c23f1cc33b56c3b47ed00e75e";

                ////Sessoin Generated End
                #endregion

                #region  ADD MEMBER TO QUICKBLOX

                //foreach (var obj in UserQBIds)
                //{
                int QuickBloxId = Convert.ToInt32(UserQBIds);

                var objAddDialog = new AddDialog();
                List<int> objoccupants_ids = new List<int>();
                objoccupants_ids.Add(QuickBloxId);

                try
                {
                    objAddDialog.name = GroupName;
                    var objPullAll = new PullAll();
                    objPullAll.occupants_ids = objoccupants_ids;
                    objAddDialog.push_all = objPullAll;
                }
                catch (Exception e)
                {

                }

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog/" + DialogId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;


                int StatusCode = Convert.ToInt32(response2.StatusCode);

                var resultQB = (JObject)JsonConvert.DeserializeObject(result2);


                if (StatusCode == 200 || StatusCode == 400)
                {

                    // var ChattingService = new ChattingServiceProxy();
                    // result = ChattingService.AddMemberIntoGroup(ChattingGroupId, Convert.ToString(UserQBIds), LoginUserId).Result;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("addCareGiverMemberIntoGroup", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ChattingGroupId", ChattingGroupId);
                            cmd.Parameters.AddWithValue("@QuickBloxId", UserQBIds);
                            cmd.Parameters.AddWithValue("@LoginUserId", LoginUserId);

                            //  result1 = Convert.ToString(cmd.ExecuteScalar());

                            result1 = Convert.ToString(cmd.ExecuteNonQuery());
                        }
                    }
                }

                // }
                #endregion

            }
            catch (Exception ex)
            {

            }

            return 1;
        }


        public static async Task<string> GenerateUserQuickBloxIdRestAPItouser(string UserId, string Email, int OfficeId, bool IsAllowGroupChat,int OrganisationId)
        {
            //-----------------
            string QuickbloxId = string.Empty;

            //  string QuickbloxId = "0";
            try
            {
                var SchedulerEmail = "Superadmin@paseva.com";

                string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                ////Sessoin Generated Start
                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");


                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

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

                //QuickBlox Call For create User Start//

                #region QuickBlox Call For create User Start


                var inputRequestParam = new UserdataQuickBloxReq();
                var UserRequest = new userReq();

                inputRequestParam.login = Email;
                inputRequestParam.password = "Welcome007!";
                inputRequestParam.email = Email;
                UserRequest.user = inputRequestParam;

                var userData1 = JsonConvert.SerializeObject(UserRequest);
                var contentData = new StringContent(userData1, Encoding.UTF8, "application/json");


                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl);
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
                var response1 = await clientGetDialogId.PostAsync("/users.json", contentData);
                var result1 = response1.Content.ReadAsStringAsync().Result;

                var MyData = (JObject)JsonConvert.DeserializeObject(result1); //MyData["user"]["id"].Value<string>() quick Blox Id
                QuickbloxId = MyData["user"]["id"].Value<string>();

                #endregion

                string SaveQb = "";

                var ChattingService = new ChattingServiceProxy();

                SaveQb = ChattingService.SaveQBId(UserId, QuickbloxId).Result;

                if (!string.IsNullOrEmpty(QuickbloxId) && OfficeId != 0 && IsAllowGroupChat == true)
                {
                    var ChattingController = new ChattingController();
                    ChattingController.AddMemberIntoOfficeGroup(OfficeId.ToString(), UserId, QuickbloxId, OrganisationId, SchedulerEmail);

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "ChattingController";
                objErrorlog.Methodname = "GenerateUserQuickBloxIdRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
            }

            //--------------------

            //string SaveQb = "";

            //var ChattingService = new ChattingServiceProxy();

            //SaveQb = ChattingService.SaveQBId(UserId, QuickbloxId).Result;

            return QuickbloxId;
        }


        #region
        // public void getandinsertData()
        //{
        //            List<string> useridss = new List<string>();


        //            //   useridss.Add("2500B19F-257E-450B-B8C0-BEBD8BEE5C82");
        //            //   useridss.Add("CB5503A8-1972-444D-BE9B-711CECFDA249");
        //            //   useridss.Add("8B691CC4-238C-48F6-B7A7-76A0D9496AFB");
        //            //   useridss.Add("A93E623B-B5F3-4173-984D-3116EFE3897F");
        //            //   useridss.Add ("DAEC1EBC-6FCC-47FA-BB31-DFE83F27C5A9");
        //            //    useridss.Add("8CEFB6BC-D686-4233-83DE-9B67A6B98F65");
        //            //   useridss.Add  ("30C26025-2F13-45EB-B8A9-EF19EDBF237B");
        //            //   useridss.Add("0B7DCE31-77A6-4C4F-9651-787FF04E7FE3");
        //            //   useridss.Add("43EBDA54-892E-456E-99AB-0813D84C6F79");
        //            //    useridss.Add("DCFB3BB0-F1E1-4B2E-A3B9-B84606991A8C");
        //            //    useridss.Add("EDB2EBE5-FFB4-4651-AA83-1F7B52CE2096");
        //            //    useridss.Add("AFB65770-C4C2-45AD-BFDD-86953FA1DAD6");
        //            //    useridss.Add("7E86A6E0-E6D9-4139-8F9D-1DCB3AED63A9");
        //            //   useridss.Add("E1913334-F753-499D-B419-57248938285A");
        //            //   useridss.Add("11DF2150-B8FE-4064-A98B-51833309583F");
        //            //   useridss.Add("322638DA-B4A3-4E4C-89A5-031BDB4294F6");
        //            //    useridss.Add("07CA7BCA-A9C1-4DDC-B38B-3BDF68916EC4");
        //            //  useridss.Add("4810DF42-20A1-40FF-9D59-12682C53AF9B");
        //            //   useridss.Add("BC34BC3E-0BEF-45A2-85BE-4884ABC8D2DD");
        //            //  useridss.Add("F6F161D3-4E35-4656-B87B-A97CCC5892DB");
        //            //   useridss.Add("004838B3-6601-4E2D-A00D-2517BC7E1524");
        //            //   useridss.Add("5F967094-4B13-4CBF-9171-0BF50D1CA3E1");
        //            //  useridss.Add("4A0174D6-3E1B-4DFF-8734-1863C644D175");
        //            //  useridss.Add("C33A27F5-FCDE-4380-95ED-0E30D13F411F");
        //            //   useridss.Add("1DF34D34-36F3-4D6A-9952-AE2EF3A109A8");
        //            //   useridss.Add("E96D169C-8BDB-4032-B3EE-16781A8F09A1");
        //            //   useridss.Add("81BF07A7-F0A9-408D-A3D9-2AB350C0C5BE");
        //            //   useridss.Add("DDD87B85-5D73-497E-BF20-D5180DE01836");
        //            //   useridss.Add("D8763CDA-D64A-4EA8-9844-C818E5412B6F");
        //            //   useridss.Add("F6BB77D2-4C8C-4D9F-91DC-D8073C883EE4");
        //            //  useridss.Add("89AE8A2E-5E17-4FEE-A28E-1E96F0E0C6DE");
        //            // useridss.Add("D230CFEC-946E-4E14-BE98-91E5632A0A68");
        //            // useridss.Add("0F55586E-E2B0-49D8-8B02-E418BB37636B");
        //            // useridss.Add("E0220CEA-49BE-4617-A2F9-429AC44F6736");
        //            //useridss.Add("34465332-2256-4510-85EE-52E34F8ED880");
        //            //useridss.Add("09B67C49-F802-4B2A-891C-C8714B59D18F");








        //            //useridss.Add("F8C6A5DE-F8E4-492D-8DA2-6E360B8BA166");
        //            //useridss.Add("67366C85-DB6A-45CD-B23B-3D8603EAD52C");
        //            //useridss.Add("B87296A2-1567-4DD4-85EE-EFF59053E746");
        //            //useridss.Add("D14C3232-FE36-4481-81C0-3809B0543143");
        //            //useridss.Add("021B088E-7FF3-4AD7-8EFA-75F9B91D68D9");
        //            //useridss.Add("26139798-6904-4CF4-A51F-561151EA4EDA");
        //            //useridss.Add("CF7C2A2A-2659-4B2A-BEA3-A970EF2A457B");
        //            //useridss.Add("B3B0B7C4-EA7E-4BF9-BCCE-9A73B61EEBB4");
        //            //useridss.Add("8C061845-F571-471D-865F-D0CF44B24223");
        //            //useridss.Add("4DF8D0D9-8453-4087-81BE-DEA4554C65F7");
        //            //useridss.Add("E424A977-E4F3-457C-BB5F-8428E24256E0");
        //            //useridss.Add("BC033ACC-4881-4C2E-93AE-74E3F2D2AF68");
        //            //useridss.Add("8C14BDC4-D095-49D1-BD28-E83855277951");
        //            //useridss.Add("EB11FA20-3285-4B61-BDBC-4295167F19B6");
        //            //useridss.Add("222C1FFE-A595-45FE-9383-43000D4F99AB");
        //            //useridss.Add("55BE71E6-A8EE-4356-B2F7-538CCAF07797");
        //            //useridss.Add("51C18E88-A510-4979-A9B8-DB28A269B970");
        //            //useridss.Add("9E7D1CF6-A1F3-48AA-A193-115B9ABBFF04");
        //            //useridss.Add("9464CF09-0BD1-4CEB-AC59-63FB9F7162E7");
        //            //useridss.Add("5530D8E6-5B98-495B-B214-AB29A6A5FF46");
        //            //useridss.Add("DDD0FCC6-D5ED-462E-87EC-322B4B7530CE");
        //            //useridss.Add("E5D1ECF0-DA16-496D-AA3E-11BD9E7B473D");



        //            //useridss.Add("387D1FA7-001C-4E7C-9E7A-0BA482BCD53B");
        //            //useridss.Add("C89870DF-7086-46B8-87B1-3E2DB5525AF0");
        //            //useridss.Add("2A4A1E2D-418D-424E-9503-F0FFBFBC0998");
        //            //useridss.Add("BE532915-368C-4CBB-85C1-6A1A56B09026");
        //            //useridss.Add("47839366-9A84-4EE6-8EBD-9DA6CA3510DD");
        //            //useridss.Add("959E3E7A-507A-40C2-8971-C56874C6D099");
        //            //useridss.Add("55FE3CA8-C53B-4E49-8D48-7362167BBECF");
        //            //useridss.Add("E0A1BBE3-58A7-4887-8A48-3CA5FC36D55C");
        //            //useridss.Add("073A9A87-6BF3-4000-96AB-08183587A44E");
        //            //useridss.Add("496F6AE0-8850-4044-BB1E-870AC3097FF8");
        //            //useridss.Add("811ADA9E-2AC4-47BC-8EAD-B0D57D8168A2");
        //            //useridss.Add("7E5AB7B9-EB8A-4716-A18E-EDF2D98FC24C");
        //            //useridss.Add("22DF4C57-5500-4F78-8C93-65B74CA11D28");
        //            //useridss.Add("D28CFF28-783A-48EA-9490-FF1BDA387519");
        //            //useridss.Add("8731F086-636B-4C56-AEF4-069A2CDAA854");
        //            //useridss.Add("9B2B7702-DF85-4989-A955-70D387FF11EA");
        //            //useridss.Add("7090D27D-633A-494D-9655-B9D638F8F9E7");
        //            //useridss.Add("2B3A03CD-9ED9-4C41-BAAA-EA870A7844AF");
        //            //useridss.Add("A92AB83B-E724-4007-A703-1FB82F1DE953");
        //            //useridss.Add("58A0F0A8-38A4-4345-BFFD-4EC69A34A69A");


        //            //useridss.Add("ABFC4569-F4DF-4B74-BD24-27B37469B812");
        //            //useridss.Add("EF08A30B-25D5-4430-A759-60CE64002CBE");
        //            //useridss.Add("0599FE65-9CB1-488E-93A1-9D9C51B60C0B");
        //            //useridss.Add("55282369-7D02-400F-B10C-D7481850ADFD");
        //            //useridss.Add("C6677DAD-FF24-42D8-A20F-D0B804825D0D");





        //            //useridss.Add("5459740A-9FCE-498B-8B2D-EEDB3AA44F9E");
        //            //useridss.Add("77C4AF15-2661-40AA-86F7-82E5781902F8");
        //            //useridss.Add("2173D9A2-AF11-48DF-AF9C-2E3B55800D77");
        //            //useridss.Add("F3B67787-6B9A-4CCD-9455-965F6015836A");
        //            //useridss.Add("1E241312-E46B-42C8-8B2B-D300A32F3EED");
        //            //useridss.Add("F4ECDC82-580F-456D-B560-940B027945F7");
        //            //useridss.Add("921E51CC-7A7E-4287-A3F5-DEF14DB803D6");
        //            //useridss.Add("6AB9FD89-58F1-49DE-885B-EF4FFF138E88");
        //            //useridss.Add("4558DB20-BFB8-47C3-89C9-A879E4B6BE23");
        //            //useridss.Add("478FADA5-6422-4177-926C-E318B826F7F8");
        //            //useridss.Add("DA4A79C8-953B-46BF-A0D1-BD9890BD3C78");
        //            //useridss.Add("40AD0A95-3304-4095-9FA9-1A86975844EC");
        //            //useridss.Add("6F654ED8-0769-4743-8B81-FAA819865FA6");
        //            //useridss.Add("CB874BC5-05FC-497E-94CB-808230A00532");
        //            //useridss.Add("F1724F68-7157-482F-8A18-9F18C6BA844A");
        //            //useridss.Add("A5075786-58FB-4E97-8568-40D1D5253B01");
        //            //useridss.Add("90B50EDE-FC53-42A6-804B-054BA3FA7477");



        //            //useridss.Add("6009EE51 - 87FC - 4C43 - 9733 - BACF2216A816");
        //            //useridss.Add("F8C6A5DE - F8E4 - 492D - 8DA2 - 6E360B8BA166");
        //            //useridss.Add("67366C85 - DB6A - 45CD - B23B - 3D8603EAD52C");
        //            //useridss.Add("B87296A2 - 1567 - 4DD4 - 85EE - EFF59053E746");
        //            //useridss.Add("D14C3232 - FE36 - 4481 - 81C0 - 3809B0543143");
        //            //useridss.Add("021B088E - 7FF3 - 4AD7 - 8EFA - 75F9B91D68D9");
        //            //useridss.Add("26139798 - 6904 - 4CF4 - A51F - 561151EA4EDA");
        //            //useridss.Add("CF7C2A2A - 2659 - 4B2A - BEA3 - A970EF2A457B");
        //            //useridss.Add("B3B0B7C4 - EA7E - 4BF9 - BCCE - 9A73B61EEBB4");
        //            //useridss.Add("8C061845 - F571 - 471D - 865F - D0CF44B24223");
        //            //useridss.Add("4DF8D0D9 - 8453 - 4087 - 81BE - DEA4554C65F7");
        //            //useridss.Add("E424A977 - E4F3 - 457C - BB5F - 8428E24256E0");
        //            //useridss.Add("BC033ACC - 4881 - 4C2E - 93AE - 74E3F2D2AF68");
        //            //useridss.Add("8C14BDC4 - D095 - 49D1 - BD28 - E83855277951");
        //            //useridss.Add("EB11FA20 - 3285 - 4B61 - BDBC - 4295167F19B6");
        //            //useridss.Add("222C1FFE - A595 - 45FE - 9383 - 43000D4F99AB");
        //            //useridss.Add("55BE71E6 - A8EE - 4356 - B2F7 - 538CCAF07797");
        //            //useridss.Add("51C18E88 - A510 - 4979 - A9B8 - DB28A269B970");
        //            //useridss.Add("9E7D1CF6 - A1F3 - 48AA - A193 - 115B9ABBFF04");
        //            //useridss.Add("9464CF09 - 0BD1 - 4CEB - AC59 - 63FB9F7162E7");
        //            //useridss.Add("5530D8E6 - 5B98 - 495B - B214 - AB29A6A5FF46");
        //            //useridss.Add("DDD0FCC6 - D5ED - 462E-87EC - 322B4B7530CE");
        //            //useridss.Add("E5D1ECF0 - DA16 - 496D - AA3E - 11BD9E7B473D");
        //            //useridss.Add("387D1FA7 - 001C - 4E7C - 9E7A - 0BA482BCD53B");




        //            //useridss.Add("C89870DF - 7086 - 46B8 - 87B1 - 3E2DB5525AF0");
        //            //useridss.Add("2A4A1E2D - 418D - 424E - 9503 - F0FFBFBC0998");
        //            //useridss.Add("BE532915 - 368C - 4CBB - 85C1 - 6A1A56B09026");
        //            //useridss.Add("47839366 - 9A84 - 4EE6 - 8EBD - 9DA6CA3510DD");
        //            //useridss.Add("959E3E7A - 507A - 40C2 - 8971 - C56874C6D099");
        //            //useridss.Add("55FE3CA8 - C53B - 4E49 - 8D48 - 7362167BBECF");

        //            //useridss.Add("F8C6A5DE - F8E4 - 492D - 8DA2 - 6E360B8BA166");
        //            //useridss.Add("67366C85 - DB6A - 45CD - B23B - 3D8603EAD52C");
        //            //useridss.Add("B87296A2 - 1567 - 4DD4 - 85EE - EFF59053E746");
        //            //useridss.Add("D14C3232 - FE36 - 4481 - 81C0 - 3809B0543143");
        //            //useridss.Add("021B088E - 7FF3 - 4AD7 - 8EFA - 75F9B91D68D9");
        //            //useridss.Add("26139798 - 6904 - 4CF4 - A51F - 561151EA4EDA");
        //            //useridss.Add("CF7C2A2A - 2659 - 4B2A - BEA3 - A970EF2A457B");
        //            //useridss.Add("B3B0B7C4 - EA7E - 4BF9 - BCCE - 9A73B61EEBB4");
        //            //useridss.Add("8C061845 - F571 - 471D - 865F - D0CF44B24223");
        //            //useridss.Add("4DF8D0D9 - 8453 - 4087 - 81BE - DEA4554C65F7");
        //            //useridss.Add("E424A977 - E4F3 - 457C - BB5F - 8428E24256E0");
        //            //useridss.Add("BC033ACC - 4881 - 4C2E - 93AE - 74E3F2D2AF68");
        //            //useridss.Add("8C14BDC4 - D095 - 49D1 - BD28 - E83855277951");
        //            //useridss.Add("EB11FA20 - 3285 - 4B61 - BDBC - 4295167F19B6");
        //            //useridss.Add("222C1FFE - A595 - 45FE - 9383 - 43000D4F99AB");
        //            //useridss.Add("55BE71E6 - A8EE - 4356 - B2F7 - 538CCAF07797");
        //            //useridss.Add("51C18E88 - A510 - 4979 - A9B8 - DB28A269B970");
        //            //useridss.Add("9E7D1CF6 - A1F3 - 48AA - A193 - 115B9ABBFF04");
        //            //useridss.Add("9464CF09 - 0BD1 - 4CEB - AC59 - 63FB9F7162E7");
        //            //useridss.Add("5530D8E6 - 5B98 - 495B - B214 - AB29A6A5FF46");
        //            //useridss.Add("DDD0FCC6 - D5ED - 462E - 87EC - 322B4B7530CE");    
        //            //useridss.Add("E5D1ECF0 - DA16 - 496D - AA3E - 11BD9E7B473D");
        //            //useridss.Add("387D1FA7 - 001C - 4E7C - 9E7A - 0BA482BCD53B");
        //            //useridss.Add("C89870DF - 7086 - 46B8 - 87B1 - 3E2DB5525AF0");




        //            //useridss.Add("2A4A1E2D - 418D - 424E - 9503 - F0FFBFBC0998");
        //            //useridss.Add("BE532915 - 368C - 4CBB - 85C1 - 6A1A56B09026");
        //            //useridss.Add("47839366 - 9A84 - 4EE6 - 8EBD - 9DA6CA3510DD");
        //            //useridss.Add("959E3E7A - 507A - 40C2 - 8971 - C56874C6D099");
        //            //useridss.Add("55FE3CA8 - C53B - 4E49 - 8D48 - 7362167BBECF");
        //            //useridss.Add("E0A1BBE3 - 58A7 - 4887 - 8A48 - 3CA5FC36D55C");
        //            //useridss.Add("073A9A87 - 6BF3 - 4000 - 96AB - 08183587A44E");
        //            //useridss.Add("496F6AE0 - 8850 - 4044 - BB1E - 870AC3097FF8");
        //            //useridss.Add("811ADA9E - 2AC4 - 47BC - 8EAD - B0D57D8168A2");
        //            //useridss.Add("7E5AB7B9 - EB8A - 4716 - A18E - EDF2D98FC24C");
        //            //useridss.Add("22DF4C57 - 5500 - 4F78 - 8C93 - 65B74CA11D28");
        //            //useridss.Add("D28CFF28 - 783A - 48EA - 9490 - FF1BDA387519");
        //            //useridss.Add("8731F086 - 636B - 4C56 - AEF4 - 069A2CDAA854");
        //            //useridss.Add("9B2B7702 - DF85 - 4989 - A955 - 70D387FF11EA");
        //            //useridss.Add("7090D27D - 633A - 494D - 9655 - B9D638F8F9E7");
        //            //useridss.Add("2B3A03CD - 9ED9 - 4C41 - BAAA - EA870A7844AF");
        //            //useridss.Add("A92AB83B - E724 - 4007 - A703 - 1FB82F1DE953");
        //            //useridss.Add("58A0F0A8 - 38A4 - 4345 - BFFD - 4EC69A34A69A");
        //            //useridss.Add("ABFC4569 - F4DF - 4B74 - BD24 - 27B37469B812");
        //            //useridss.Add("EF08A30B - 25D5 - 4430 - A759 - 60CE64002CBE");
        //            //useridss.Add("0599FE65 - 9CB1 - 488E - 93A1 - 9D9C51B60C0B");
        //            //useridss.Add("55282369 - 7D02 - 400F - B10C - D7481850ADFD");
        //            //useridss.Add("C6677DAD - FF24 - 42D8 - A20F - D0B804825D0D");
        //            //useridss.Add("5459740A - 9FCE - 498B - 8B2D - EEDB3AA44F9E");
        //            //useridss.Add("77C4AF15 - 2661 - 40AA - 86F7 - 82E5781902F8");
        //            //useridss.Add("2173D9A2 - AF11 - 48DF - AF9C - 2E3B55800D77");
        //            //useridss.Add("F3B67787 - 6B9A - 4CCD - 9455 - 965F6015836A");
        //            //useridss.Add("1E241312 - E46B - 42C8 - 8B2B - D300A32F3EED");
        //            //useridss.Add("F4ECDC82 - 580F - 456D - B560 - 940B027945F7");
        //            //useridss.Add("921E51CC - 7A7E - 4287 - A3F5 - DEF14DB803D6");
        //            //useridss.Add("6AB9FD89 - 58F1 - 49DE - 885B - EF4FFF138E88");
        //            //useridss.Add("4558DB20 - BFB8 - 47C3 - 89C9 - A879E4B6BE23");
        //            //useridss.Add("478FADA5 - 6422 - 4177 - 926C - E318B826F7F8");
        //            //useridss.Add("DA4A79C8 - 953B - 46BF - A0D1 - BD9890BD3C78");
        //            //useridss.Add("40AD0A95 - 3304 - 4095 - 9FA9 - 1A86975844EC");
        //            //useridss.Add("6F654ED8 - 0769 - 4743 - 8B81 - FAA819865FA6");
        //            //useridss.Add("CB874BC5 - 05FC - 497E - 94CB - 808230A00532");
        //            //useridss.Add("F1724F68 - 7157 - 482F - 8A18 - 9F18C6BA844A");
        //            //useridss.Add("A5075786 - 58FB - 4E97 - 8568 - 40D1D5253B01");  



        //            //useridss.Add("90B50EDE - FC53 - 42A6 - 804B - 054BA3FA7477");
        //            //useridss.Add("6009EE51 - 87FC - 4C43 - 9733 - BACF2216A816");
        //            //useridss.Add("CBC7077E - 2D66 - 40B6 - B88F - E7817FB752D3");
        //            //useridss.Add("11ECD5D4 - 22F9 - 424F - B1D7 - E0457244F743");
        //            //useridss.Add("5B41D6A9 - 067F - 4D54 - 9AA5 - DAEA30A8AC2F");
        //            //useridss.Add("83399B49 - A72C - 4E19 - 9F17 - 556D3ED451F5");
        //            //useridss.Add("3F732633 - E636 - 4A8B - 956B - FE587D9470AC");
        //            //useridss.Add("92E88E5C - 9218 - 41BC - 8E75 - 9C6410ED1C42");
        //            //useridss.Add("F21ECA72 - 9B14 - 4423 - B7AC - 9048694C0C83");
        //            //useridss.Add("6A4BDE94 - C300 - 4837 - 8B36 - E5567236ACB4");
        //            //useridss.Add("21C491B9 - 71D2 - 45D7 - 84EE - 2701F0F30DC3");
        //            //useridss.Add("804A3A5E - E6C4 - 4DC5 - A184 - F06D68230FCA");
        //            //useridss.Add("935DE1C2 - CA48 - 4A86 - AD85 - 0FF50F59DD18");
        //            //useridss.Add("E7B5EA12 - 6B9D - 42C5 - 9D52 - 6C98D915DBF8");
        //            //useridss.Add("AF51C276 - 9C5D - 4DF9 - A464 - 80B8E866A923");
        //            //useridss.Add("9F37F46D - 23C8 - 4A5E - 8398 - B154C3DAE724");
        //            //useridss.Add("2DFB4098 - 5F42 - 4977 - 8022 - 5FF70A82054D");
        //            //useridss.Add("81315901 - 579D - 47A2 - 836D - F4C696ACC3AD");
        //            //useridss.Add("DC3F7A32 - 062A - 4D53 - BFBA - 0C00BDE1E2F4");
        //            //useridss.Add("BA68F233 - 30EC - 404E - 8B2B - E120985DFC4E");
        //            //useridss.Add("A302449E - 67B9 - 4BFD - A9BD - 91B300E73671");
        //            //useridss.Add("90130D3A - 8519 - 42AA - AAAB - C619DBBA4A80");
        //            //useridss.Add("3C858587 - E5C1 - 49B4 - A807 - 8FE1D8700614");
        //            //useridss.Add("A457CFD9 - 6FAB - 4E2A - A5F0 - 3B7858C02196");
        //            //useridss.Add("CE385754 - AA61 - 4344 - 8B94 - 536BEE370281");
        //            //useridss.Add("CBBC0B68 - C939 - 459E - 9FB1 - 92DFFAC6DB03");
        //            //useridss.Add("0298808B - 63A4 - 4F68 - B2E1 - 5848FA83BB25");
        //            //useridss.Add("B2E40002 - FFC7 - 4114 - 824B - C58CD3C2602C");
        //            //useridss.Add("30C9004C - 5F61 - 40F4 - 9B81 - 35466C5EB5DE");
        //            //useridss.Add("62A2D1BF - 4EA9 - 45CC - AC78 - A81AA88F7024");
        //            //useridss.Add("57EE2F5C - BA0B - 4BBA - 9C5E - E332576AA981");
        //            //useridss.Add("11D3AC0E - 2417 - 450A - BBD4 - 5A2B36D7E7CA");
        //            //useridss.Add("08CD03EB - BB5E - 4100 - 9D58 - 7B3E8EA841A3");
        //            //useridss.Add("3EC834D8 - 676D - 479A - A127 - 9AB8521573FD");
        //            //useridss.Add("5311DF9F - 7D05 - 4422 - 92C6 - D9E9C7B5AB90");
        //            //useridss.Add("EDE3B0DE - 1FA0 - 462B - 893A - EDB4E95EF700");
        //            //useridss.Add("0BBC2775 - A369 - 493F - AB35 - 36168078368B");
        //            //useridss.Add("A4D8D9EF - 50D6 - 4B50 - B522 - 510189904CDA");
        //            //useridss.Add("A569DA55 - CA7E - 479F - A982 - E38C6CCCC1C6");
        //            //useridss.Add("3159DEF0 - 047E - 4C0E - BE77 - C0570317B00A");
        //            //useridss.Add("DB972E0D - 148B - 4064 - 8A57 - FDEAA2E32626");
        //            //useridss.Add("DD4E3739 - 97CB - 46D5 - 91F1 - E2AB4D130997");
        //            //useridss.Add("328A405E - 43D0 - 4A59 - 85B9 - 3794BAA04029");
        //            //useridss.Add("048FD74C - 61BA - 4798 - A4C8 - 38DAAA764200");
        //            //useridss.Add("FAEDF66A - 4DF4 - 4967 - BC67 - 86838548B804");
        //            //useridss.Add("C0815211 - A538 - 4BC1 - AD92 - 32C571687504");
        //            //useridss.Add("F7778BF8 - B9C2 - 4D7A - A269 - 6DEC1A330AD0");
        //            //useridss.Add("4AEC7ACC - 88A9 - 4F74 - 9FDA - E9014306B351");
        //            //useridss.Add("21674547 - 6F38 - 4D17 - B939 - 98067FD99565");
        //            //useridss.Add("05EA11F3 - 0E82 - 426F - A20C - 919516DD9C6B");
        //            //useridss.Add("777151BC - 4078 - 4EC2 - 94AA - 91A3921C26BB");
        //            //useridss.Add("3957624B - 9858 - 4F72 - 85D4 - BFB6FBBFC359");
        //            //useridss.Add("851B17B3 - 757D - 4CF0 - 93E1 - 479B7A350918");
        //            //useridss.Add("BAEA8939 - ED7F - 47F6 - 99EA - 949CA455897F");
        //            //useridss.Add("6F0C7872 - 31F0 - 4F46 - 8B18 - 48352C3498F1");
        //            //useridss.Add("A212D1B4 - CECC - 44D1 - B3F0 - 69FDD50D243B");
        //            //useridss.Add("B9B95C46 - 3EB6 - 448F - A31C - FB2C965C6AD0");
        //            //useridss.Add("D7099F42 - AB6E - 473E - 9206 - 8B37103F146A");
        //            //useridss.Add("D5F0F119 - F03B - 443E - 9BEE - ADE72F6593BF");
        //            //useridss.Add("DE7B7105 - 560E - 429D - AA98 - 6C53A0070C63");
        //            //useridss.Add("45DB07F6 - 1ED9 - 4485 - A391 - F7FC9EDE71DC");
        //            //useridss.Add("ED6BD23A - 1112 - 4E0C - 8B30 - 754871EDCF20");
        //            //useridss.Add("AE363A58 - E366 - 4151 - AD7D - A4D13CB58494");
        //            //useridss.Add("A2F938A3 - 9E56 - 4DF4 - BC30 - 9928DAD8930B");
        //            //useridss.Add("0547216C - C181 - 4F67 - AA1E - C78705ABFDC6");



        //            //useridss.Add("8104B80F - BD57 - 4A67 - 85A9 - 474144A4ABF1");
        //            //useridss.Add("793A4E7C - 54FD - 40C7 - 8C53 - 10C6C9E69F40");
        //            //useridss.Add("52CDED01 - 43BB - 4B18 - AD57 - 84AE78A809F2");
        //            //useridss.Add("898003C8 - AF98 - 489B - 9745 - C1EA72E885C1");
        //            //useridss.Add("49850C3B - BFA2 - 4265 - BDF1 - A60FC541B92D");
        //            //useridss.Add("F9D1E9FD - 43FC - 4977 - 85CC - F3BEADEAB3FC");
        //            //useridss.Add("EC05D6F1 - 8A53 - 4719 - A495 - 68BDE6289A9D");
        //            //useridss.Add("8FCDE74F - 0250 - 4168 - 996A - 00D1E80AFD16");
        //            //useridss.Add("1C44A259 - D325 - 47E3 - B2A2 - AE81D263FD67");
        //            //useridss.Add("96C00A95 - 961D - 462F - B8AA - BD97594AE8CE");
        //            //useridss.Add("8BDD7822 - E72B - 4068 - 823A - 6832F38D594D");
        //            //useridss.Add("4CC70C06 - 9C81 - 44D1 - A220 - 7D9D4AA5A31B");
        //            //useridss.Add("CED2D2DA - 2F54 - 48D7 - B788 - 2B7BCB424F36");
        //            //useridss.Add("ED7FD28C - 4AB3 - 4F76 - 98FC - F6310E332431");
        //            //useridss.Add("35D45A0E - 24F9 - 4505 - BCD5 - CB9690BA260A");
        //            //useridss.Add("D8F38883 - 2F61 - 4E0F - 8CDA - FA888C56E580");
        //            //useridss.Add("5FE09280 - 8174 - 4FF8 - AAC1 - 7E6F7FC7AF5F");
        //            //useridss.Add("55B5E690 - 02FE - 4D86 - A343 - C545B2F93B95");
        //            //useridss.Add("B177847F - 98B7 - 417A - 9B8E - 97103B6AAA25");
        //            //useridss.Add("DC0B5301 - 4509 - 47BF - 8A25 - 7FBC8A4432D0");
        //            //useridss.Add("FB6A0FA4 - 8E75 - 4B31 - A6AD - 4917D382C2F3");
        //            //useridss.Add("F5C10715 - 46AA - 490C - B067 - 04B38196C990");
        //            //useridss.Add("0A9B1AB9 - BB18 - 41B2 - 8C80 - 104D933B682A");
        //            //useridss.Add("6A5563D8 - F06C - 4287 - BE3D - FAA88E09FD06");
        //            //useridss.Add("3F581F61 - 0CA3 - 47BF - 9E6B - A186FA8BBFF8");

        //            useridss.Add("220CE04A - 9DE7 - 404C - BD36 - 460DE378E3AD");
        //            useridss.Add("CD0118D7 - FF1E - 4C07 - BD31 - D396CAB68F41");
        //            useridss.Add("F8670BC9 - F1B6 - 4062 - BAF8 - 8F14566EF315");
        //            useridss.Add("8B35C3E2 - 9477 - 4DB9 - BD10 - 89E3FABCAFD8");
        //            useridss.Add("7C54F6EA - 24D5 - 4229 - A60A - DCC20BBBD903");
        //            useridss.Add("063D87B8 - CC5D - 4CF7 - 87DD - DF1568DFD5BC");
        //            useridss.Add("022E2D34 - BE3C - 46C5 - A653 - D71592B313FB");
        //            useridss.Add("D83FD6B5 - 960F - 4AE2 - 902F - 483023FE35DE");
        //            useridss.Add("3A5A2F33 - 6BC2 - 4D1D - 9BDE - 944C2A787A08");
        //            useridss.Add("7524B59B - 0063 - 46BB - 84C5 - FB44EBFDBCD5");
        //            useridss.Add("4797DDBB - 1FEB - 44F5 - 86D7 - D0FA02152FA2");
        //            useridss.Add("01AC2A84 - 8435 - 43F0 - 882A - BB1CD93A3F4D");
        //            useridss.Add("C57A8A2A - CAA3 - 4A1F - 9B9B - BADF0E37CEB6");
        //            useridss.Add("3EE90F2F - 7C19 - 4A9A - A375 - 1B5FC58CCDD0");
        //            useridss.Add("E804AF3F - AC43 - 4659 - 9A43 - 4044129C456E");
        //            useridss.Add("D7AC7EA4 - 020F - 4A3C - A675 - BBCB221E9EC8");
        //            useridss.Add("DF4CB01D - 01F7 - 436B - 838D - EC2DEC7F058D");
        //            useridss.Add("8519D45E - D13E - 4EDE - 8E51 - 585F7FDF29EA");
        //            useridss.Add("335C6D7A - 72E4 - 4A05 - 9EE7 - 36DF045E03B3");
        //            useridss.Add("3D06EA4D - C4A3 - 4E36 - B6E7 - 840523CF2CD9");
        //            useridss.Add("DFA7F689 - 7804 - 42D7 - 8C1C - 37EB39FD0920");
        //            useridss.Add("E30E0BAE - 7241 - 45FE - AE21 - 31FB9172194D");
        //            useridss.Add("926A2CFE - 0B7B - 4C9C - A9E5 - 1082D063A3BC");
        //            useridss.Add("3C849071 - 67D4 - 4437 - A61F - E364F50466C5");
        //            useridss.Add("90BD650B - DAFF - 451D - 8325 - E618AC0D7FA1");
        //            useridss.Add("335C9BC8 - 6B46 - 4330 - 9E72 - C23250101582");
        //            useridss.Add("5FCB295F - 672B - 4212 - AFDF - D9D8DDE144CC");
        //            useridss.Add("5083DF8B - A06D - 48D1 - BC45 - F8D8AD2B407D");
        //            useridss.Add("612C146C - 10D7 - 452F - A8F6 - 64CE3CA3DD10");
        //            useridss.Add("24946005 - EFF4 - 4683 - 9642 - BE2E85F61D42");
        //            useridss.Add("2727FDCE - 3E42 - 4CD5 - AA84 - E6AC6DECB5C6");
        //            useridss.Add("132C0F61 - E958 - 4029 - 92A6 - 1C2FB1B6D0E5");
        //            useridss.Add("5DBEA4E8 - 3BF6 - 40D7 - A3A4 - BF0B58DB9514");
        //            useridss.Add("A03A7159 - B730 - 4801 - 8F8A - 41B1546BA771");
        //            useridss.Add("5D3168E6 - 3562 - 4E19 - 80CC - F104C9488DD1");
        //            useridss.Add("D25476E4 - ECE7 - 4159 - ABC8 - 195C4985D1C2");
        //            useridss.Add("EEB871AB - 0A2B - 4058 - B513 - B9ECDAEB7DAB");
        //            useridss.Add("E80F5C42 - 369B - 405B - BEF0 - E506858EE2F6");
        //            useridss.Add("95812365 - A674 - 49D4 - A378 - 8263557936F7");
        //            useridss.Add("5EAECAC6 - 2ACF - 4638 - 8279 - EBB117D01909");
        //            useridss.Add("C09D34B8 - D19F - 408D - B959 - 1530AEB9B8DF");
        //            useridss.Add("78BB7B5D - 1487 - 4E77 - 85A5 - 52ECF8D5C691");















        //            for (int i = 0; i <= useridss.Count; i++)
        //            {
        //                int j = 0;
        //                string result = "";

        //                string useidd = useridss[i].Trim();

        //                string[] Useridssss = useridss[i].Split(' ');

        //                string use1 = Useridssss[0];
        //                string use2 = Useridssss[1];
        //                string use3 = Useridssss[2];
        //                string use4 = Useridssss[3];
        //                string use5 = Useridssss[4];
        //                string use6 = Useridssss[5];
        //                string use7 = Useridssss[6];
        //                string use8 = Useridssss[7];
        //                string user9 = Useridssss[8];

        //                string useridgs = use1 + use2 + use3 + use4 + use5 + use6 + use7 + use8 + user9;



        //                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand("INSERtcommonCaregiverONUATandLive", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                //string useidd = useridss[i].Trim();
        //                cmd.Parameters.AddWithValue("@Adduseid",useridgs);
        //                j=cmd.ExecuteNonQuery();

        //                if(j==1)
        //                {

        //                    result = "success";
        //                }
        //                else
        //                {

        //                    result= "false";
        //                }

        //            }




        ////8104B80F - BD57 - 4A67 - 85A9 - 474144A4ABF1
        ////793A4E7C - 54FD - 40C7 - 8C53 - 10C6C9E69F40
        ////52CDED01 - 43BB - 4B18 - AD57 - 84AE78A809F2
        ////898003C8 - AF98 - 489B - 9745 - C1EA72E885C1
        ////49850C3B - BFA2 - 4265 - BDF1 - A60FC541B92D
        ////F9D1E9FD - 43FC - 4977 - 85CC - F3BEADEAB3FC
        ////EC05D6F1 - 8A53 - 4719 - A495 - 68BDE6289A9D
        ////8FCDE74F - 0250 - 4168 - 996A - 00D1E80AFD16
        ////1C44A259 - D325 - 47E3 - B2A2 - AE81D263FD67
        ////96C00A95 - 961D - 462F - B8AA - BD97594AE8CE
        ////8BDD7822 - E72B - 4068 - 823A - 6832F38D594D
        ////4CC70C06 - 9C81 - 44D1 - A220 - 7D9D4AA5A31B
        ////CED2D2DA - 2F54 - 48D7 - B788 - 2B7BCB424F36
        ////ED7FD28C - 4AB3 - 4F76 - 98FC - F6310E332431
        ////35D45A0E - 24F9 - 4505 - BCD5 - CB9690BA260A
        ////D8F38883 - 2F61 - 4E0F - 8CDA - FA888C56E580
        ////5FE09280 - 8174 - 4FF8 - AAC1 - 7E6F7FC7AF5F
        ////55B5E690 - 02FE - 4D86 - A343 - C545B2F93B95
        ////B177847F - 98B7 - 417A - 9B8E - 97103B6AAA25
        ////DC0B5301 - 4509 - 47BF - 8A25 - 7FBC8A4432D0
        ////FB6A0FA4 - 8E75 - 4B31 - A6AD - 4917D382C2F3
        ////F5C10715 - 46AA - 490C - B067 - 04B38196C990
        ////0A9B1AB9 - BB18 - 41B2 - 8C80 - 104D933B682A
        ////6A5563D8 - F06C - 4287 - BE3D - FAA88E09FD06
        ////3F581F61 - 0CA3 - 47BF - 9E6B - A186FA8BBFF8
        ////220CE04A - 9DE7 - 404C - BD36 - 460DE378E3AD
        ////CD0118D7 - FF1E - 4C07 - BD31 - D396CAB68F41
        ////F8670BC9 - F1B6 - 4062 - BAF8 - 8F14566EF315
        ////8B35C3E2 - 9477 - 4DB9 - BD10 - 89E3FABCAFD8
        ////7C54F6EA - 24D5 - 4229 - A60A - DCC20BBBD903
        ////063D87B8 - CC5D - 4CF7 - 87DD - DF1568DFD5BC
        ////022E2D34 - BE3C - 46C5 - A653 - D71592B313FB
        ////D83FD6B5 - 960F - 4AE2 - 902F - 483023FE35DE
        ////3A5A2F33 - 6BC2 - 4D1D - 9BDE - 944C2A787A08
        ////7524B59B - 0063 - 46BB - 84C5 - FB44EBFDBCD5
        ////4797DDBB - 1FEB - 44F5 - 86D7 - D0FA02152FA2
        ////01AC2A84 - 8435 - 43F0 - 882A - BB1CD93A3F4D
        ////C57A8A2A - CAA3 - 4A1F - 9B9B - BADF0E37CEB6
        ////3EE90F2F - 7C19 - 4A9A - A375 - 1B5FC58CCDD0
        ////E804AF3F - AC43 - 4659 - 9A43 - 4044129C456E
        ////D7AC7EA4 - 020F - 4A3C - A675 - BBCB221E9EC8
        ////DF4CB01D - 01F7 - 436B - 838D - EC2DEC7F058D
        ////8519D45E-D13E-4EDE - 8E51 - 585F7FDF29EA
        ////335C6D7A - 72E4 - 4A05 - 9EE7 - 36DF045E03B3
        ////3D06EA4D - C4A3 - 4E36 - B6E7 - 840523CF2CD9
        ////DFA7F689 - 7804 - 42D7 - 8C1C - 37EB39FD0920
        ////E30E0BAE - 7241 - 45FE - AE21 - 31FB9172194D
        ////926A2CFE - 0B7B - 4C9C - A9E5 - 1082D063A3BC
        ////3C849071 - 67D4 - 4437 - A61F - E364F50466C5
        ////90BD650B - DAFF - 451D - 8325 - E618AC0D7FA1
        ////335C9BC8 - 6B46 - 4330 - 9E72 - C23250101582
        ////5FCB295F - 672B - 4212 - AFDF - D9D8DDE144CC
        ////5083DF8B - A06D - 48D1 - BC45 - F8D8AD2B407D
        ////612C146C - 10D7 - 452F - A8F6 - 64CE3CA3DD10
        ////24946005 - EFF4 - 4683 - 9642 - BE2E85F61D42
        ////2727FDCE - 3E42 - 4CD5 - AA84 - E6AC6DECB5C6
        ////132C0F61 - E958 - 4029 - 92A6 - 1C2FB1B6D0E5
        ////5DBEA4E8 - 3BF6 - 40D7 - A3A4 - BF0B58DB9514
        ////A03A7159 - B730 - 4801 - 8F8A - 41B1546BA771
        ////5D3168E6 - 3562 - 4E19 - 80CC - F104C9488DD1
        ////D25476E4 - ECE7 - 4159 - ABC8 - 195C4985D1C2
        ////EEB871AB - 0A2B - 4058 - B513 - B9ECDAEB7DAB
        ////E80F5C42 - 369B - 405B - BEF0 - E506858EE2F6
        ////95812365 - A674 - 49D4 - A378 - 8263557936F7
        ////5EAECAC6 - 2ACF - 4638 - 8279 - EBB117D01909
        ////C09D34B8 - D19F - 408D - B959 - 1530AEB9B8DF
        ////78BB7B5D - 1487 - 4E77 - 85A5 - 52ECF8D5C691

        //}



        #endregion



        // public string MemberToNotifyforTaggedNotification(List<MemberFornotification> MemberToNotify, string ChatId, string vMsg, string UserName, string GroupName, string DialogId, string Type)
        public string MemberToNotifyforTaggedNotification(string EmailTo, string vMsg, string UserName, string GroupName)
        {
            string result = "";
            string DataToFilter = "";
            string NotifiedUserids = "";
            int NotifiedPermission = 0;
            string NotifiedBadgeCount = "";

            #region  Commented code for MemberToNotifyForTaggedNotification
            //List<string> listMemtoNotify = new List<string>();

            //var objGroupMemberDetailListingForChatNotification = new List<ChattingGroupMember>();

            //var ChattingService = new ChattingServiceProxy();
            //objGroupMemberDetailListingForChatNotification = ChattingService.GetGroupMemberDetail(ChatId).Result;


            ////foreach (var MemToNotified in MemberToNotify)
            ////{

            ////  NotifiedUserids = objGroupMemberDetailListingForChatNotification.Where(x => x.MemberName == MemToNotified.MemberToNotifyUser).Select(x => x.UserId).SingleOrDefault();

            //// NotifiedPermission = objGroupMemberDetailListingForChatNotification.Where(x => x.MemberName == MemToNotified.MemberToNotifyUser).Select(x => x.Permission).SingleOrDefault(); 



            ////  NotifiedUserids = "87a387e2-5c5b-4ba2-b4c0-c7472329dc62";

            //NotifiedUserids = objGroupMemberDetailListingForChatNotification.Where(x => x.MemberName == MemberToNotify).Select(x => x.UserId).SingleOrDefault();

            //NotifiedPermission = objGroupMemberDetailListingForChatNotification.Where(x => x.MemberName == MemberToNotify).Select(x => x.Permission).SingleOrDefault();



            ////   MemberListedData =
            ////   (from memtonotify in objGroupMemberDetailListingForChatNotification
            ////   where memtonotify.MemberName == MemToNotified.MemberToNotifyUser
            ////   select memtonotify.QuickBloxId)
            ////   .SingleOrDefault();



            //ClientDeviceNotification clientdevice = new ClientDeviceNotification();

            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
            //con.Open();

            //SqlCommand cmd = new SqlCommand("UserAgentDeviceID", con);

            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@userid", NotifiedUserids);

            //SqlDataReader dr = cmd.ExecuteReader();

            //if (dr.HasRows)
            //{

            //    while (dr.Read())
            //    {
            //        clientdevice.userDeviceID = dr[0].ToString();
            //        clientdevice.UserDeviceType = dr[1].ToString();
            //        clientdevice.UserName = dr[2].ToString();
            //        clientdevice.NotifiedBadgeCount = GetBadgeCountWithIncrement(NotifiedUserids);

            //    }

            //    Notification obj = new Notification();
            //    IosResponse ios_response = new IosResponse();
            //    AndroidResponse android_response = new AndroidResponse();
            //    PushNotification objPushNotification = new PushNotification();


            //    List<string> iosdevice_Permission1 = new List<String>();
            //    List<string> iosdevice_Permission2 = new List<String>();
            //    List<string> iosdevice_Permission3 = new List<String>();

            //    //List<String> androiddeviceids = new List<String>();

            //    List<string> androiddeviceids_Permission1 = new List<String>();
            //    List<string> androiddeviceids_Permission2 = new List<String>();
            //    List<string> androiddeviceids_Permission3 = new List<String>();


            //    string Name = UserName;
            //    string sMsg = "";
            //    if (Type == "1")
            //    {
            //        sMsg = UserName + " Sent Message : " + vMsg;
            //        //string Name = UserName;
            //    }
            //    else
            //    {
            //        sMsg = UserName + " @ " + GroupName + " : " + vMsg;
            //    }


            //    //for (int i = 0; i < notifiableUserList.Count; i++)
            //    //{

            //    if (clientdevice.UserDeviceType.ToLower() == "ios" && clientdevice.userDeviceID != "")
            //    {

            //        if (NotifiedPermission.ToString() == "1")
            //        {
            //            iosdevice_Permission1.Add(clientdevice.userDeviceID);
            //            if (iosdevice_Permission1.Count > 0)
            //                ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission1.ToArray(), sMsg, Type, GroupName, DialogId, clientdevice.NotifiedBadgeCount, "1");
            //            iosdevice_Permission1.Clear();
            //            result = ios_response.Status;
            //        }
            //        else if (NotifiedPermission.ToString() == "2")
            //        {
            //            iosdevice_Permission2.Add(clientdevice.userDeviceID);
            //            if (iosdevice_Permission2.Count > 0)
            //                ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission2.ToArray(), sMsg, Type, GroupName, DialogId, clientdevice.NotifiedBadgeCount, "2");
            //            iosdevice_Permission2.Clear();
            //            result = ios_response.Status;
            //        }
            //        else
            //        {
            //            iosdevice_Permission3.Add(clientdevice.userDeviceID);
            //            if (iosdevice_Permission3.Count > 0)
            //                ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission3.ToArray(), sMsg, Type, GroupName, DialogId, clientdevice.NotifiedBadgeCount, "");
            //            iosdevice_Permission3.Clear();
            //            result = ios_response.Status;
            //        }
            //    }
            //    else if (clientdevice.UserDeviceType.ToLower() == "android" && clientdevice.userDeviceID != "")
            //    {


            //        if (NotifiedPermission.ToString() == "1")
            //        {
            //            androiddeviceids_Permission1.Add(clientdevice.userDeviceID);
            //            if (androiddeviceids_Permission1.Count > 0)
            //                android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission1.ToArray(), sMsg, Type, GroupName, DialogId, "1", clientdevice.NotifiedBadgeCount);
            //            androiddeviceids_Permission1.Clear();
            //            result = android_response.Status;

            //        }
            //        else if (NotifiedPermission.ToString() == "2")
            //        {
            //            androiddeviceids_Permission2.Add(clientdevice.userDeviceID);
            //            if (androiddeviceids_Permission2.Count > 0)
            //                android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission2.ToArray(), sMsg, Type, GroupName, DialogId, "2", clientdevice.NotifiedBadgeCount);
            //            androiddeviceids_Permission2.Clear();
            //            result = android_response.Status;

            //        }
            //        else
            //        {
            //            androiddeviceids_Permission3.Add(clientdevice.userDeviceID);
            //            if (androiddeviceids_Permission3.Count > 0)
            //                android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission3.ToArray(), sMsg, Type, GroupName, DialogId, "", clientdevice.NotifiedBadgeCount);
            //            androiddeviceids_Permission3.Clear();

            //            result = android_response.Status;

            //        }
            //    }
            //}

            ////  }

            #endregion


            string toAddress = EmailTo;
                //ConfigurationManager.AppSettings["Mail"].ToString();

           /// string subject = "Supply Form";

            bool IsFileAttachment = true;

            string body = " ";

            string CCMailID = " ";

            bool isBodyHtml = true;
            string AttachmentFileName = null;

            //  if (sendEmailWithAttachment(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))

            if (sendEmailWithAttachment(toAddress, GroupName, vMsg, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
            {


            }

            return result;
        }



        //public int GetBadgeCountWithIncrement(string UserId)
        //{
        //    int Count = 0;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("GetBadgeCountWithIncrement", con);

        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@UserId",UserId);

        //        DataSet ds = new DataSet();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        da.Fill(ds);

        //        if (ds != null)
        //        {
        //            Count = Convert.ToInt32(ds.Tables[0].Rows[0]["BadgeCount"].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Count;
        //}




        //public string GenerateSalesSupplyFormReport(string patientgroupname)
        //{
        //    string result = "";


        //    try
        //    {

        //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("GeneratePatientSuppliesDetails", con);

        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@GroupName", patientgroupname);

        //        DataSet ds = new DataSet();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        da.Fill(ds);







        //        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
        //        //con.Open();
        //        //SqlCommand cmd = new SqlCommand("GeneratePatientSuppliesDetails", con);
        //        //cmd.CommandType = CommandType.StoredProcedure;
        //        //cmd.Parameters.AddWithValue("@GroupName", patientgroupname);

        //        ////cmd.Parameters.AddWithValue("@FromDate",fromdate);
        //        ////cmd.Parameters.AddWithValue("@ToDate",Todate);

        //        //cmd.CommandType = CommandType.StoredProcedure;
        //        //DataSet ds = new DataSet();
        //        //SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        //da.Fill(ds);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {

        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //            }

        //        }


        //    }
        //    catch(Exception ex)
        //    {
        //        throw (ex);
        //    }





        //    return result;
        //}


        //public static async Task<string> GenerateUserQuickBloxIdRestAPItousertouser(string UserId, string Email, int OfficeId, bool IsAllowGroupChat,string Name)
        //{
        //    //-----------------
        //    string QuickbloxId = string.Empty;

        //    //  string QuickbloxId = "0";
        //    try
        //    {
        //        // var SchedulerEmail = "Superadmin@paseva.com";
        //        // var SchedulerEmail = "vikinegi0@gmail.com";

        //        var SchedulerEmail = "vikki.negi@gmail.com";

        //        string QuickbloxAPIUrl = System.Configuration.ConfigurationManager.AppSettings["QuickbloxAPIUrl1"].ToString();

        //        ////Sessoin Generated Start
        //        var client = new System.Net.Http.HttpClient();

        //        client.BaseAddress = new Uri(QuickbloxAPIUrl);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");


        //        Random random = new Random();
        //        int Vnonce = random.Next(0, 9999);
        //        string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

        //        var input = new QuickBloxSession();
        //        input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id1"]);
        //        input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key1"];
        //        input.nonce = Vnonce.ToString();
        //        input.timestamp = QuickBloxServiceProxy.Timestamp();

        //        input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=welcome01!";
        //        //Encryption            
        //        input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret1"]);

        //        var userData = new Userdata();
        //        userData.login = SchedulerEmail; //"superadmin@caregiver.com"
        //        userData.password = "welcome01!";

        //        // userData.password = "Welcome007!";
        //        input.user = userData;

        //        var jData1 = JsonConvert.SerializeObject(input);
        //        var content1 = new StringContent(jData1);

        //        var content = new StringContent(jData1, Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync("/session.json", content);
        //        var result = response.Content.ReadAsStringAsync().Result;
        //        JObject json = JObject.Parse(result);
        //        var data = (JObject)JsonConvert.DeserializeObject(result);
        //        string token = data["session"]["token"].Value<string>();


        //        ////Sessoin Generated End


        //        //QuickBlox Call For create User Start//

        //        #region QuickBlox Call For create User Start


        //        var inputRequestParam = new UserdataQuickBloxReq();
        //        var UserRequest = new userReq();

        //        inputRequestParam.login = Email;
        //        inputRequestParam.password ="Welcome01!";
        //        inputRequestParam.email = Email;
        //        inputRequestParam.full_name = Name;

        //        UserRequest.user = inputRequestParam;

        //        var userData1 = JsonConvert.SerializeObject(UserRequest);
        //        var contentData = new StringContent(userData1, Encoding.UTF8, "application/json");


        //        var clientGetDialogId = new System.Net.Http.HttpClient();

        //        clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl);
        //        clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
        //        clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
        //        var response1 = await clientGetDialogId.PostAsync("/users.json", contentData);
        //        var result1 = response1.Content.ReadAsStringAsync().Result;

        //        var MyData = (JObject)JsonConvert.DeserializeObject(result1); //MyData["user"]["id"].Value<string>() quick Blox Id
        //        QuickbloxId = MyData["user"]["id"].Value<string>();

        //        #endregion


        //        string SaveQb = "";

        //        var ChattingService = new ChattingServiceProxy();

        //        SaveQb = ChattingService.SaveQBId(UserId, QuickbloxId).Result;

        //        //if (!string.IsNullOrEmpty(QuickbloxId) && OfficeId != 0 && IsAllowGroupChat == true)
        //        //{
        //        //    var ChattingController = new ChattingController();
        //        //    ChattingController.AddMemberIntoOfficeGroup(OfficeId.ToString(), UserId, QuickbloxId);





        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "ChattingController";
        //        objErrorlog.Methodname = "GenerateUserQuickBloxIdRestAPI";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(objErrorlog).Result;
        //    }

        //    return QuickbloxId;

        //}

        [HttpPost]
        public ActionResult CreateShedulerPdf(List<PdfModel> PdfDetail, string ChattingId, string OfficeName)
        {



            var ObjStatus = "";

            ObjStatus = subcreateaaFile11(PdfDetail, ChattingId, OfficeName);

            return Json(ObjStatus, JsonRequestBehavior.AllowGet);

        }


        // private string subcreateFile(string name, string quality, string description, string Productname)

        public string subcreateaaFile11(List<PdfModel> result, string ChattingId, string OfficeName)
        {
            //string OfficeName = "";
            //string OfficeAddress = "";


            DataTable Dt = new DataTable();

            Dt.Columns.Add("patientname");
            Dt.Columns.Add("ScheduledDate");
            Dt.Columns.Add("ScheduledTime");
            Dt.Columns.Add("ReScheduledDate");
            Dt.Columns.Add("ReScheduledTime");
            Dt.Columns.Add("CreatedDate");
            Dt.Columns.Add("ReschedulingFormUrl");
            Dt.Columns.Add("Reason");
            Dt.Columns.Add("TypesOfVisit");
            Dt.Columns.Add("OfficeName");
            Dt.Columns.Add("EmployeeName");


            var ObjStatus = "";
            string img = Server.MapPath("~/Content/image/paseva.jpg");

            // string word = Convert.ToString(quality);
            StringBuilder sb = new StringBuilder();


            sb.Append("<header class='clearfix'>");

            sb.Append("<img align='center' src='" + img + "'width='80px' height='60px' S/>");
            sb.Append("<br>");
            //sb.Append("<td style='width:33%;'><b>Employee Name:</b><span style='color:#4090ca;padding-left: 5px;'>" + Session["name"] + "</span></td>");

            sb.Append("<b><p style='background:#1dca75;text-align:center'>Re Scheduler Form</p></b>");
            //sb.Append("<td style='width:33%;'><b>Date:</b> <span style='color:#4090ca;padding-right: 5px;'>" + DateTime.Now + "</span></td>");

            sb.Append("<br>");
            sb.Append("</header>");

            sb.Append("<table align='center' style='width: 100 %;'>");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<td style='width:33%;'><b>Employee Name:</b><span style='color:#34333D;padding-left: 5px;'>" + Session["name"] + "</span></td>");
            // sb.Append("<td style='width:33%;'><b>Patient Name:</b><span style='color:#4090ca;padding-left: 5px;'>" + Patientname + "</span></td>");
            sb.Append("<td style='width:33%;'><b>Office Name:</b> <span style='color:#4090ca;padding-left: 15px;'>" + OfficeName + "</span></td>");
            //sb.Append("<td style='width:33%;'><b>Date:</b> <span style='color:#4090ca;padding-left: 5px;'>" + DateTime.Now + "</span></td>");

            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<br>");
            sb.Append("<br>");

            sb.Append("<table align='center' style='width: 100 %;'>");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            //sb.Append("<td style'width:33%;'> <b>Office Name:</b> <span style='color:#4090ca;padding-left: 5px;'>" + OfficeName + "</span></td>");
            //sb.Append("<td style='width:33%;'><b>Office Address:</b> <span style='color:#4090ca;padding-left: 5px;'>" + OfficeAddress + "</span></td>");
            sb.Append("<td style='width:33%;'><b>DateTime:</b> <span style='color:#34333D;padding-left: 15px;'>" + DateTime.Now + "</span></td>");

            //sb.Append("<td align='right'><b>Date:" + DateTime.Now + "</b></td>");

            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<br>");
            sb.Append("<br>");

            sb.Append("<table border=1 align='center'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='width:3px;color:##4090ca;font-weight:700;'>PatientName</th>");

            sb.Append("<th style='width:40px;color:##4090ca;font-weight:900;'>ScheduledDate</th>");
            sb.Append("<th style='width:20px;color:##4090ca;font-weight:700;'>Time</th>");
            sb.Append("<th style='width:40px;color:##4090ca;font-weight:900;'>ReScheduledDate</th>");
            sb.Append("<th style='width:20px;color:##4090ca;font-weight:700;'>Time</th>");
            sb.Append("<th style='width:20px;color:##4090ca;font-weight:700;'>Reason</th>");
            sb.Append("<th style='width:20px;color:##4090ca;font-weight:700;'>Types Of Visit</th>");


            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            sb.Append("<tr>");

            foreach (var e1 in result)

            {
                sb.Append("<td style='color:#34333D'>" + e1.PatientName + "</td>");



                sb.Append("<td  style='color:#34333D'>" + e1.ScheduledDate + "</td>");
                sb.Append("<td  style='color:#34333D'>" + e1.Time + "</td>");
                sb.Append("<td style='color:#34333D'>" + e1.ReScheduledDate + "</td>");
                sb.Append("<td  style='color:#34333D'>" + e1.Rtime + "</td>");
                sb.Append("<td  style='color:#34333D'>" + e1.Reason + "</td>");
                sb.Append("<td  style='color:#34333D'>" + e1.TypesOfVisit + "</td>");


            }


            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("</main>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<br>");

            sb.Append("<footer>");
            sb.Append("Note:*report is created.");
            sb.Append("<br>");
            sb.Append(".............................................................................");
            sb.Append("</footer>");

            StringReader sr = new StringReader(sb.ToString());
            Document pdfdocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            HTMLWorker htmlparse = new HTMLWorker(pdfdocument);

            string PdfFilePath = ConfigurationManager.AppSettings["DownlLoadFilePath2"].ToString();

            ////file name to be created  

            string strPDFFileName = string.Format("ReScheduleFormReport_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "-" + ".pdf");

            string strAttachment = Server.MapPath("~/Downloadss/RescheduleForm/" + strPDFFileName);

            using (MemoryStream memorystream = new MemoryStream())
            {

                PdfWriter writer = PdfWriter.GetInstance(pdfdocument, memorystream);
                pdfdocument.Open();
                htmlparse.Parse(sr);
                pdfdocument.Close();

                byte[] bytes = memorystream.ToArray();

                System.IO.File.WriteAllBytes(strAttachment, bytes);

                memorystream.Close();

                string ExcelUrl = PdfFilePath + strPDFFileName;
                ObjStatus = ExcelUrl;


                string AttachmentFileName = strAttachment;



                string toAddress = ConfigurationManager.AppSettings["RescheduleMail"].ToString();

                foreach (var e1 in result)
                {
                    Dt.Rows.Add(e1.PatientName, e1.ScheduledDate, e1.Time, e1.ReScheduledDate, e1.Rtime, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), ExcelUrl, e1.Reason, e1.TypesOfVisit, OfficeName, Session["name"]);
                }


                string subject = "ReSchedule Form";

                bool IsFileAttachment = true;

                string body = " ";

                string Result1 = "";

                string CCMailID = ConfigurationManager.AppSettings["RescheduleCCmail"].ToString();


                bool isBodyHtml = true;
                //var manresuts = sendEmailWithAttachment1(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml);
                if (sendEmailWithAttachment1(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("ReschedulingFormDataSupplies", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@RescheduleFormDescriptionList", Dt);

                            int i = cmd.ExecuteNonQuery();

                            if (i > 0)
                            {
                                return ObjStatus;


                            }
                            else
                            {
                                return ObjStatus = "";
                            }

                        }
                    }

                }
                return ObjStatus;
            }
        }

        private bool sendEmailWithAttachment1(string toAddress, string subject, string body, bool IsFileAttachment, string AttachmentFileName, string CCMailID, bool isBodyHtml = true)
        {
            try
            {
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


                MailAddress ma = new MailAddress(ConfigurationManager.AppSettings["RescheduleOutLookMail"].ToString());
                mailMessage.From = ma;

                mailMessage.Subject = subject;

                //LinkedResource Signature = null;

                if (IsFileAttachment == true)
                {
                    if (!string.IsNullOrEmpty(AttachmentFileName))
                    {

                        // mailMessage.Attachments.Add(AttachmentFileName);

                        Attachment attachFile = new Attachment(AttachmentFileName);

                        mailMessage.Attachments.Add(attachFile);


                    }
                }

                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                var smtpClient = new SmtpClient { EnableSsl = false };
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Host = ConfigurationManager.AppSettings["SMTPHostOutlook"];

                //smtpClient.Host ="smtp.live.com";

                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.UseDefaultCredentials = false;

                // mailMessage.Bcc.Add(bccAddress);
                // smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);

                // smtpClient.Credentials = new NetworkCredential("pramendrasingh400@gmail.com","singhtripty");

                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["RescheduleOutLookMail"].ToString(), ConfigurationManager.AppSettings["RescheduleOutlookPassword"].ToString());
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
            }
            return false;
        }



        public ActionResult ReScheduledFromDetails(string ChattingGroupId, string GroupName)
        {

            ViewBag.RescheduleFormDetailslist = GetReScheduledFromData(ChattingGroupId.ToString(), GroupName);
            List<ReScheduledFrom> ListSupplyFormData = ViewBag.RescheduleFormDetailslist;

            return PartialView("ReScheduledFromDetails");
        }


        public List<ReScheduledFrom> GetReScheduledFromData(string chattingId, string GroupName)
        {   
            string result = "";
            string OfficeName = "";
            string OfficeAddress = "";

            string[] patientstr = GroupName.Split('(');
            string Patientname = patientstr[0];

            //string SessionName = Session["name"].ToString();

            string SessionName = "Ben Moss";

            List<OfficeModel> listOffices = GetAddressAndOfficename(chattingId);


            foreach (var officelist in listOffices)
            {
                OfficeName = officelist.OfficeName;
                OfficeAddress = officelist.Address;

            }

            List<ReScheduledFrom> RescheduleFormDataList = new List<ReScheduledFrom>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("GetReshedulerData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@GroupName", GroupName);
                    cmd.Parameters.AddWithValue("@EmployeeName", SessionName);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();

                    con.Open();
                    da.Fill(ds);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            List<PdfDataDescription1> pdflist = new List<PdfDataDescription1>();

                            ReScheduledFrom supplyformdatas = new ReScheduledFrom();

                            supplyformdatas.ReschedulingFormUrl = ds.Tables[0].Rows[i]["ReschedulingFormUrl"].ToString();
                            supplyformdatas.CreatedDate = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                            supplyformdatas.OfficeName = OfficeName;
                            supplyformdatas.patientname = GroupName;
                            supplyformdatas.EmployeeName = SessionName;


                            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
                            {

                                using (SqlCommand cmd1 = new SqlCommand("ReScheduledFromData", con1))
                                {
                                    cmd1.CommandType = CommandType.StoredProcedure;

                                    cmd1.Parameters.AddWithValue("@PatientName", GroupName);
                                    //cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                    cmd1.Parameters.AddWithValue("@CreateDate", ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                                    cmd1.Parameters.AddWithValue("@ReschedulingFormUrl", ds.Tables[0].Rows[i]["ReschedulingFormUrl"].ToString());

                                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

                                    DataSet ds1 = new DataSet();

                                    con1.Open();
                                    da1.Fill(ds1);

                                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                    {

                                        PdfDataDescription1 pdfdescribedData = new PdfDataDescription1();

                                        pdfdescribedData.ScheduledDate = ds1.Tables[0].Rows[j]["ScheduledDate"].ToString();
                                        pdfdescribedData.ScheduledTime = ds1.Tables[0].Rows[j]["ScheduledTime"].ToString();
                                        pdfdescribedData.ReScheduledDate = ds1.Tables[0].Rows[j]["ReScheduledDate"].ToString();
                                        pdfdescribedData.ReScheduledTime = ds1.Tables[0].Rows[j]["ReScheduledTime"].ToString();


                                        pdfdescribedData.Reason = ds1.Tables[0].Rows[j]["Reason"].ToString();
                                        pdfdescribedData.TypesOfVisit = ds1.Tables[0].Rows[j]["TypesOfVisit"].ToString();


                                        pdflist.Add(pdfdescribedData);
                                        supplyformdatas.ReshuduleDataList = pdflist;


                                    }

                                }
                        }
                            RescheduleFormDataList.Add(supplyformdatas);
                        }

                    }

                }
            }
                    return RescheduleFormDataList.ToList();

        }



        public ActionResult PatientOneToOneChat()

        {
           // FillAllOffices();
            return View();
        }


        public async Task<string> CreatePatientGroupChatOnQuickBloxRestAPI(string GroupName, string MemberList, int OfficeId, string AdminUserId)
        {
            // string UserId = GetUserIDFromAccessToken();
            string DialogId = "";
            string res2 = "";
            string SuperAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();

            try
            {
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //var SchedulerEmail = "superadmin@paseva.com";//Check This
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");


                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //  var response = await client.PostAsync("/session.json", content);

                var response = await client.PostAsync("/session.json", content);

              //  var response= await client.PostAsJsonAsync("/session.json", content);

               // System.Threading.Thread.Sleep(1000);

                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End

                //GetActivePatientRequest All Dialog Detail
                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);
                ////--

                var objAddDialog = new CreateGroup();
                List<int> objoccupants_ids = new List<int>();
                List<string> name = new List<string>();

                //objAddDialog.occupants_ids.Add(1);//Group Creator QuickBlox Id
                objAddDialog.name = GroupName;//GroupName

                // For add custom param
                Data CustomParam = new Data();
                CustomParam.class_name = "ChatDialogType";
                CustomParam.ChatCategory = "2";
                CustomParam.OfficeID = OfficeId.ToString();
                objAddDialog.data = CustomParam;

                //if (MemberList != null)
                //{
                //    foreach (var item in MemberList)
                //    {
                //        if (!String.IsNullOrEmpty(item))
                //        {
                            objoccupants_ids.Add(Convert.ToInt32(MemberList));
                //        }
                //    }
                //}
                objAddDialog.occupants_ids = objoccupants_ids;//Add Member QuickBloxId

                var jData2 = JsonConvert.SerializeObject(objAddDialog);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientGetDialogId.PostAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;
                var data1 = (JObject)JsonConvert.DeserializeObject(result2);
                DialogId = data1["_id"].Value<string>();



                //Task.Run(async () =>
                //{

                //string rsulstnew =  AddGroupDialogIdBatchSchedule(DialogId, GroupName, InsertedUserID, OfficeId);

                string rsulstnew = AddGroupDialogId(DialogId, GroupName, InsertedUserID, OfficeId);


                //  }).Wait();


            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "CreateGroupChatOnQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }
            ////--

            //var ChattingService = new ChattingServiceProxy();

            //var Chatting = new Chatting();

            //var GroupChat = new GroupChat();
            //GroupChat.GroupDialogId = DialogId;
            //GroupChat.GroupName = GroupName;
            //GroupChat.LogInUserId = SuperAdminUserId;
            //GroupChat.OfficeId = OfficeId;
            //GroupChat.GroupTypeID = 3;

            //Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;

            ////   res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), "4D51E02D-D16E-4962-A01B-97624A683A64").Result; //for add second super admin

            //var AdminUserIds = AdminUserId.Split(',');
            //foreach (var id in AdminUserIds)
            //{
            //    res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
            //}
            // res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), AdminUserId).Result;


            if (DialogId != "")
            {
                 res2= "Success";
            }
            return res2;
        }

        public async Task<string> GetAllUnreadMessagesToRead()
        {
            // string UserId = GetUserIDFromAccessToken();
            string DialogId = "";
            string res2 = "";
            string SuperAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"].ToString();

            try
            {
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //var SchedulerEmail = "superadmin@paseva.com";//Check This
                ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

                Random random = new Random();
                int Vnonce = random.Next(0, 9999);
                string timestamp = Convert.ToString((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);// new DateTime.Now().ToString("yyMMddHHmmss");

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                input.auth_key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + SchedulerEmail + "&user[password]=Welcome007!";
                //Encryption            
                input.signature = QuickBloxServiceProxy.Hash(input.signature, ConfigurationManager.AppSettings["QuickbloxAuth_Secret"]);

                var userData = new Userdata();
                userData.login = SchedulerEmail; //"superadmin@caregiver.com"
                userData.password = "Welcome007!";
                input.user = userData;

                var jData1 = JsonConvert.SerializeObject(input);
                var content = new StringContent(jData1, Encoding.UTF8, "application/json");
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.PostAsync("/session.json", content);
                var result = response.Content.ReadAsStringAsync().Result;
                var data = (JObject)JsonConvert.DeserializeObject(result);
                string token = data["session"]["token"].Value<string>();

                ////Sessoin Generated End
                //GetActivePatientRequest All Dialog Detail
                var clientGetDialogId = new System.Net.Http.HttpClient();

                clientGetDialogId.BaseAddress = new Uri(QuickbloxAPIUrl + "/chat/Dialog.json");
                clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
                clientGetDialogId.DefaultRequestHeaders.Add("QB-Token", token);

                ////--
                //var objAddDialog = new CreateGroup();
                //List<int> objoccupants_ids = new List<int>();
                //List<string> name = new List<string>();
                ////objAddDialog.occupants_ids.Add(1);//Group Creator QuickBlox Id
                //objAddDialog.name = GroupName;//GroupName
                //// For add custom param
                //Data CustomParam = new Data();
                //CustomParam.class_name = "ChatDialogType";
                //CustomParam.ChatCategory = "2";
                //CustomParam.OfficeID = OfficeId.ToString();
                //objAddDialog.data = CustomParam;

                ////if (MemberList != null)
                ////{
                ////    foreach (var item in MemberList)
                ////    {
                ////        if (!String.IsNullOrEmpty(item))
                ////        {
                //objoccupants_ids.Add(Convert.ToInt32(MemberList));
                ////        }
                ////    }
                ////}
                //objAddDialog.occupants_ids = objoccupants_ids;//Add Member QuickBloxId

                //var jData2 = JsonConvert.SerializeObject(objAddDialog);
                //var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                //var response2 = await clientGetDialogId.PostAsync("", content2);
                //var result2 = response2.Content.ReadAsStringAsync().Result;
                //var data1 = (JObject)JsonConvert.DeserializeObject(result2);
                //DialogId = data1["_id"].Value<string>();

                //string IsSuccess = AddGroupDialogId(DialogId, GroupName, InsertedUserID, OfficeId);


            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "CreateGroupChatOnQuickBloxRestAPI";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return null;
            }
           
            
            ////--
            //var ChattingService = new ChattingServiceProxy();
            //var Chatting = new Chatting();

            //var GroupChat = new GroupChat();
            //GroupChat.GroupDialogId = DialogId;
            //GroupChat.GroupName = GroupName;
            //GroupChat.LogInUserId = SuperAdminUserId;
            //GroupChat.OfficeId = OfficeId;
            //GroupChat.GroupTypeID = 3;

            //Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;
            ////   res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), "4D51E02D-D16E-4962-A01B-97624A683A64").Result; //for add second super admin

            //var AdminUserIds = AdminUserId.Split(',');
            //foreach (var id in AdminUserIds)
            //{
            //    res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), id).Result;
            //}
            // res2 = ChattingService.AssignGroupToUser(Chatting.ChattingGroupId.ToString(), AdminUserId).Result;


            if (DialogId != "")
            {
                res2 = "Success";
            }

            return res2;
        }

        public async Task<int> ApiToChangeEmailToQuickblox(string Email, string UpdateEmail, string QuickBloxId)
        {
            try
            {
                string abcd = "";
                #region FOR SESION GENERATION
                //SchedulerEmail = "abdsr@solifetec.com";

                string QuickbloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"].ToString();
                int QuickbloxApp_Id = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
                string QuickbloxAuth_Key = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
                string QuickbloxAuth_Secret = ConfigurationManager.AppSettings["QuickbloxAuth_Secret"];

                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(QuickbloxAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

                Random random = new Random();
                int Vnonce = random.Next(0, 9999);

                var input = new QuickBloxSession();
                input.application_id = Convert.ToInt32(QuickbloxApp_Id);
                input.auth_key = QuickbloxAuth_Key;
                input.nonce = Vnonce.ToString();
                input.timestamp = QuickBloxServiceProxy.Timestamp();

                input.signature = "application_id=" + input.application_id + "&auth_key=" + input.auth_key + "&nonce=" + input.nonce + "&timestamp=" + input.timestamp + "&user[login]=" + Email + "&user[password]=Welcome007!";          
                input.signature = QuickBloxServiceProxy.Hash(input.signature, QuickbloxAuth_Secret);

                var userData = new Userdata();
                userData.login = Email; //"superadmin@caregiver.com"
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

                var requestData = new
                {
                    user = new
                    {
                        login = UpdateEmail,
                        email = UpdateEmail
                    }
                };

                var clientAddMember = new System.Net.Http.HttpClient();

                clientAddMember.BaseAddress = new Uri(QuickbloxAPIUrl + "/users/" + QuickBloxId + ".json");
                clientAddMember.DefaultRequestHeaders.Accept.Clear();
                clientAddMember.DefaultRequestHeaders.Add("QB-Token", token);

                var jData2 = JsonConvert.SerializeObject(requestData);
                var content2 = new StringContent(jData2, Encoding.UTF8, "application/json");
                var response2 = await clientAddMember.PutAsync("", content2);
                var result2 = response2.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(response2.StatusCode);

                var resultQB = (JObject)JsonConvert.DeserializeObject(result2);

                if (StatusCode == 201 || StatusCode == 400)
                {
                    return 1;
                }
                //#endregion
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return 1;
        }

        //public async Task<int> ApiToChangeEmailToQuickblox(string email, string updateEmail, string quickBloxId)
        //{
        //    try
        //    {
        //        // Configuration values
        //        string quickBloxAPIUrl = ConfigurationManager.AppSettings["QuickbloxAPIUrl"];
        //        int quickBloxAppId = Convert.ToInt32(ConfigurationManager.AppSettings["QuickbloxApp_Id"]);
        //        string quickBloxAuthKey = ConfigurationManager.AppSettings["QuickbloxAuth_Key"];
        //        string quickBloxAuthSecret = ConfigurationManager.AppSettings["QuickbloxAuth_Secret"];

        //        // Create HTTP client for QuickBlox API
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(quickBloxAPIUrl);
        //            client.DefaultRequestHeaders.Clear();
        //            client.DefaultRequestHeaders.Add("QuickBlox-REST-API-Version", "0.1.0");

        //            // Generate a nonce and timestamp
        //            Random random = new Random();
        //            int nonce = random.Next(0, 9999);
        //            string timestamp = QuickBloxServiceProxy.Timestamp();

        //            // Generate signature
        //            string signatureString = $"application_id={quickBloxAppId}&auth_key={quickBloxAuthKey}&nonce={nonce}&timestamp={timestamp}";
        //            string signature = QuickBloxServiceProxy.Hash(signatureString, quickBloxAuthSecret);

        //            // Prepare session creation payload
        //            var sessionRequest = new
        //            {
        //                application_id = quickBloxAppId,
        //                auth_key = quickBloxAuthKey,
        //                nonce = nonce,
        //                timestamp = timestamp,
        //                signature = signature
        //            };

        //            string sessionPayload = JsonConvert.SerializeObject(sessionRequest);
        //            var sessionContent = new StringContent(sessionPayload, Encoding.UTF8, "application/json");

        //            // Make the API call to create a session
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //            var sessionResponse = await client.PostAsync("/session.json", sessionContent);

        //            if (!sessionResponse.IsSuccessStatusCode)
        //            {
        //                throw new Exception($"Failed to create QuickBlox session. Status Code: {sessionResponse.StatusCode}");
        //            }

        //            var sessionResult = JsonConvert.DeserializeObject<JObject>(await sessionResponse.Content.ReadAsStringAsync());
        //            string token = sessionResult["session"]["token"].Value<string>();

        //            // Prepare user update payload
        //            var updateRequest = new
        //            {
        //                user = new
        //                {
        //                    login = updateEmail,
        //                    email = updateEmail
        //                }
        //            };

        //            string updatePayload = JsonConvert.SerializeObject(updateRequest);
        //            var updateContent = new StringContent(updatePayload, Encoding.UTF8, "application/json");

        //            // Set QB-Token and make the update request
        //            client.DefaultRequestHeaders.Clear();
        //            client.DefaultRequestHeaders.Add("QB-Token", token);

        //            var updateResponse = await client.PutAsync($"/users/{quickBloxId}.json", updateContent);

        //            if (!updateResponse.IsSuccessStatusCode)
        //            {
        //                throw new Exception($"Failed to update user email. Status Code: {updateResponse.StatusCode}, Response: {await updateResponse.Content.ReadAsStringAsync()}");
        //            }

        //            // Check for successful status codes
        //            return updateResponse.StatusCode == HttpStatusCode.OK ? 1 : 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log and rethrow exception
        //        // Logging mechanism can be added here
        //        throw new Exception("Error in updating QuickBlox email", ex);
        //    }
        //}


        public string AddGroupDialogIdBatchSchedule(string DialogId, string GroupName, string UserId, int OfficeId)
        {
            string result = "";
            try
            {
                //UAT
                //  string SuperAdminUserId = "6b186dd2-b28c-4baa-8ff7-f68b7e6258fa";

                string SuperAdminUserId = System.Configuration.ConfigurationManager.AppSettings["SuperAdminUserId"];

                //Live & Local
                //   string SuperAdminUserId = "D59FF81A-5F78-4D23-BE3B-C8630313E2BA";

                var ChattingService = new ChattingServiceProxy();

                var GroupChat = new GroupChat();

                GroupChat.GroupDialogId = DialogId;
                GroupChat.GroupName = GroupName;
                GroupChat.LogInUserId = SuperAdminUserId;
                GroupChat.OfficeId = OfficeId;
                GroupChat.GroupTypeID = 1;

                var Chatting = new Chatting();

                Chatting = ChattingService.AddGroupDialogId(GroupChat).Result;
                //   result = ChattingService.AddGroupDialogId(DialogId, GroupName, UserId, OfficeId.ToString()).Result;

                if (Chatting.Result == "Success")
                {
                    MembershipUser user = Membership.GetUser();
                    string[] roles = Roles.GetRolesForUser(user.UserName);
                    string LogInUserId = user.ProviderUserKey.ToString();

                    // string LogInUserId = "6b186dd2-b28c-4baa-8ff7-f68b7e6258fa";


                    //if (roles.Length > 0 && roles[0] == "SuperAdmin" || roles[0] == "Scheduler")
                    //{
                    OfficeModel objModel = new OfficeModel();

                    objModel = OfficeController.GetOfficeDetailByOfficeId(OfficeId.ToString());

                    // string UserIds = "4D51E02D-D16E-4962-A01B-97624A683A64" + ',' + objModel.AdminUserId;

                    //  string QBIds = "32168516" + ',' + objModel.AdminQuickBloxId;
                    //   var data =  GetGroupDetailFromGroupName(GroupName);

                    string UserIds = objModel.AdminUserId;
                    string QBIds = objModel.AdminQuickBloxId;

                    var SchedulerList = new List<ScheduleInfo>();

                    SchedulerList = ChattingService.GetALLSuperadminList().Result;

                    foreach (var item in SchedulerList)
                    {
                        UserIds = UserIds + ',' + item.UserId;

                        QBIds = QBIds + ',' + item.QuickbloxId;
                    }

                    var objDialogDetail = new Chatting();
                    objDialogDetail = ChattingService.GetGroupDetailFromGroupName(GroupName).Result;

                    
                    AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), UserIds, QBIds);

                    if (roles.Length > 0 && roles[0] == "Scheduler")
                    {
                        var SchedulerQBID = Session["FromQBId"];
                        AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, SchedulerQBID.ToString());
                    }
                    //else
                    //{

                    if (OfficeId.ToString() == "14" || OfficeId.ToString() == "6" || OfficeId.ToString() == "12" || OfficeId.ToString() == "20" || OfficeId.ToString() == "5")
                    {

                        if (LogInUserId != "441E7AA9-078E-46E6-A8C1-6E7D5A5B71C9" || LogInUserId != "B68C33BB-088B-4338-9FC5-3B3F30A2D76B")
                        {
                            ScheduleInfo schedulerInfo = GetSchedularDetailsAddedTochatRoom(OfficeId.ToString());
                            AddMemberIntoGroup_V1(objDialogDetail.ChattingGroupId.ToString(), LogInUserId, schedulerInfo.QuickbloxId.ToString());
                        }
                    }


                }


                // check role & result 
                // get group ID from  Group name
                // get ADMIN userId & QuickboxId from OfficeID
                //     AddMemberIntoGroup()


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ChattingController";
                log.Methodname = "AddGroupDialogId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        public static Chatting AddOrganisationGroupDialogId(GroupChat GroupChat)
        {
            string result = "";
            var objDialogDetail = new Chatting();

            try
            {
                //  DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddOrganisationBasedGroupDialogId",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_AddOrganisationBasedGroupDialogId",
                                                      GroupChat.GroupDialogId,
                                                    GroupChat.GroupName,
                                                    GroupChat.GroupSubject,
                                                    Guid.Parse(GroupChat.LogInUserId),
                                                    GroupChat.GroupTypeID,
                                                    GroupChat.OfficeId,
                                                    GroupChat.OrganisationId
                                                    );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    objDialogDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChattingGroupId"]);
                    objDialogDetail.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    objDialogDetail.DialogId = ds.Tables[0].Rows[0]["DialogId"].ToString();

                    objDialogDetail.Result = "Success";

                }
                //    if (i > 0)
                //{
                //    result = "Success";
                //}
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddGroupDialogId";
               // result = InsertErrorLog(objErrorlog);
            }
            return objDialogDetail;
        }


        private void insertdata(string result)
        {
           // string result = "Testing";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "insertdatatocheck", result);

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
                objErrorlog.Methodname = "InsertScheduleForNurse";
                //objErrorlog.UserID = CareGiverSchedule.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            //  return result;
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

    }


}

#endregion