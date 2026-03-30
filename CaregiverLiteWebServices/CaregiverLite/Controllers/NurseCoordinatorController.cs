using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CaregiverLite.Models;
using System.Web.Security;
using CaregiverLite.Action_Filters;
using CaregiverLiteWCF;
using System.Threading.Tasks;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class NurseCoordinatorController : Controller
    {
        // GET: NurseCoordinator
        public ActionResult NurseCoordinator()
        {
            FillAllOffices();
            return View();
        }

        public ActionResult GetNurseCoordinatorList(JQueryDataTableParamModel param)
        {
            string loginUserId = Membership.GetUser().ProviderUserKey.ToString();
            NurseCoordinatorsList NurseCoordinatorsList = new NurseCoordinatorsList();
            try
            {
                int FilterOfficeId = 0;
                string IsActiveStatus = "";
                if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                if (Request["FilterActiveStatus"] != null && Request["FilterActiveStatus"] != "")
                {
                    IsActiveStatus = Request["FilterActiveStatus"];
                }
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
                    sortOrder = "IsAllowOneToOneChat";
                }
                else if (sortColumnIndex == 6)
                {
                    sortOrder = "IsAllowForPatientChatRoom";
                }
                else if (sortColumnIndex == 7)
                {
                    sortOrder = "IsAllowGroupChat";
                }
                else if (sortColumnIndex == 8)
                {
                    sortOrder = "IsAllowToCreateGroupChat";
                }
                else if (sortColumnIndex == 9)
                {
                    sortOrder = "OfficeName";
                }
                else if (sortColumnIndex == 10)
                {
                    sortOrder = "OfficeName";
                }
                else
                {
                    sortOrder = "FirstName";
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


                int OrganisationId = 0;
                if (!string.IsNullOrEmpty(Request["FilterOrganisationId"]))
                {
                    OrganisationId = Convert.ToInt32(Request["FilterOrganisationId"]);
                }
                else
                {
                    OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                }



                NurseCoordinatorServiceProxy NurseCoordinatorLiteService = new NurseCoordinatorServiceProxy();
                NurseCoordinatorsList = NurseCoordinatorLiteService.GetAllNurseCoordinators(loginUserId, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId, OrganisationId, IsActiveStatus).Result;
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
            if (NurseCoordinatorsList.NurseCoordinatorList != null)
            {
                var result = from C in NurseCoordinatorsList.NurseCoordinatorList select new[] { C, C, C, C, C, C, C, C, C, C, C, C,C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = NurseCoordinatorsList.TotalNumberofRecord,
                    iTotalDisplayRecords = NurseCoordinatorsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = NurseCoordinatorsList.TotalNumberofRecord,
                    iTotalDisplayRecords = NurseCoordinatorsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private void FillAllOffices(object SelectedValue = null)
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            try { 
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


        public ActionResult AddNurseCoordinator()
        {
            var nurseCoordinatorModel = new NurseCoordinatorModel();
            FillAllOffices();
            nurseCoordinatorModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;

            //List<Office> OfficeList = new List<Office>();
            //OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString()).Result;
            //SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");
            //ViewBag.lstOffice = officeSelectList;

            return PartialView(nurseCoordinatorModel);
        }


        [HttpPost]
        public ActionResult AddNurseCoordinator(NurseCoordinatorModel objNurseCoordinatorModel)
        {
            try
            {
                NurseCoordinatorServiceProxy NurseCoordinatorService = new NurseCoordinatorServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                if (objNurseCoordinatorModel.NurseCoordinatorId == 0)
                {
                    MembershipCreateStatus status;
                    Membership.CreateUser(objNurseCoordinatorModel.Username, objNurseCoordinatorModel.Password, objNurseCoordinatorModel.Email, null, null, true, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(objNurseCoordinatorModel.Username, "NurseCoordinator");
                    }
                }
                var NurseCoordinator = new NurseCoordinator();
                NurseCoordinator.NurseCoordinatorId = objNurseCoordinatorModel.NurseCoordinatorId;
                NurseCoordinator.UserId = Membership.GetUser(objNurseCoordinatorModel.Username).ProviderUserKey.ToString();
                NurseCoordinator.FirstName = objNurseCoordinatorModel.FirstName;
                NurseCoordinator.LastName = objNurseCoordinatorModel.LastName;
                NurseCoordinator.Email = objNurseCoordinatorModel.Email;
                NurseCoordinator.InsertUserId = InsertedUserID;
                NurseCoordinator.UserName = objNurseCoordinatorModel.Username;
                NurseCoordinator.Password = objNurseCoordinatorModel.Password;
                NurseCoordinator.OfficeId = objNurseCoordinatorModel.OfficeId;
                NurseCoordinator.JobTitle = objNurseCoordinatorModel.JobTitle;
                NurseCoordinator.IsAllowForPatientChatRoom = objNurseCoordinatorModel.IsAllowForPatientChatRoom;
                NurseCoordinator.IsAllowOneToOneChat = objNurseCoordinatorModel.IsAllowOneToOneChat;
                NurseCoordinator.IsAllowGroupChat = objNurseCoordinatorModel.IsAllowGroupChat;
                NurseCoordinator.IsAllowToCreateGroupChat = objNurseCoordinatorModel.IsAllowToCreateGroupChat;
                NurseCoordinator.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

          
                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                string result = NurseCoordinatorService.AddNurseCoordinator(NurseCoordinator).Result;

                if (result == "Success")
                {
                    
                    ForgetPasswordServiceProxy ForgetPasswordService = new ForgetPasswordServiceProxy();
                    string result1 = ForgetPasswordService.EmailtoSetPassword(objNurseCoordinatorModel.Email).Result;

                    //ForgetPasswordService.ForgotPassword(objNurseCoordinatorModel.Email).Result;

                    Task.Run(async () => { await ChattingController.GenerateUserQuickBloxIdRestAPI(NurseCoordinator.UserId, NurseCoordinator.Email, NurseCoordinator.OfficeId,NurseCoordinator.IsAllowGroupChat, NurseCoordinator.OrganisationId,OrganisationEmail); }).Wait();

                    TempData["Message"] = "Office Staff is added successfully.";
                    return RedirectToAction("NurseCoordinator", "NurseCoordinator", new { IsAdded = true });
                }
                else
                {
                    return RedirectToAction("NurseCoordinator", "NurseCoordinator");
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "NurseCoordinatorController";
                log.Methodname = "[HttpPost] AddNurseCoordinator";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public NurseCoordinatorModel GetNurseCoordinatorDetailById(string id)
        {
            NurseCoordinatorModel objNurseCoordinator = new NurseCoordinatorModel();
            try
            {
                NurseCoordinatorServiceProxy CareGiverLiteService = new NurseCoordinatorServiceProxy();
                NurseCoordinator NurseCoordinator1 = CareGiverLiteService.GetNurseCoordinatorDetailById(id).Result;

                objNurseCoordinator.NurseCoordinatorId = NurseCoordinator1.NurseCoordinatorId;
                objNurseCoordinator.FirstName = NurseCoordinator1.FirstName;
                objNurseCoordinator.LastName = NurseCoordinator1.LastName;
                objNurseCoordinator.Username = NurseCoordinator1.UserName;
                objNurseCoordinator.Email = NurseCoordinator1.Email;
                objNurseCoordinator.OldEmail = NurseCoordinator1.Email;
                objNurseCoordinator.Password = NurseCoordinator1.Password;
                objNurseCoordinator.OfficeId = NurseCoordinator1.OfficeId;
                objNurseCoordinator.OldOfficeId = NurseCoordinator1.OfficeId;
                objNurseCoordinator.IsActive = NurseCoordinator1.IsActive;
                objNurseCoordinator.QuickBloxId = NurseCoordinator1.QBID;
                objNurseCoordinator.UserId = NurseCoordinator1.UserId;
                objNurseCoordinator.JobTitle = NurseCoordinator1.JobTitle;
                objNurseCoordinator.IsAllowForPatientChatRoom = NurseCoordinator1.IsAllowForPatientChatRoom;
                objNurseCoordinator.IsAllowOneToOneChat = NurseCoordinator1.IsAllowOneToOneChat;
                objNurseCoordinator.IsAllowGroupChat = NurseCoordinator1.IsAllowGroupChat;
                objNurseCoordinator.IsAllowToCreateGroupChat = NurseCoordinator1.IsAllowToCreateGroupChat;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "GetCareGiverDetailById";
                log.Methodname = "GetNurseCoordinatorDetailById";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objNurseCoordinator;
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
                log.Pagename = "NurseCoordinatorController";
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
                log.Pagename = "NurseCoordinatorController";
                log.Methodname = "IsEmailExists";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }


        public string DeleteNurseCoordinator(string NurseCoordinatorId, string UserName)
        {
            string result = "";
            try
            {

                MembershipUser mUser = Membership.GetUser(UserName);
                string NurseCoordinatorUserId = mUser.ProviderUserKey.ToString();
                Membership.DeleteUser(UserName, true);

                NurseCoordinatorServiceProxy CareGiverLiteService = new NurseCoordinatorServiceProxy();//ProviderUserKey
                //result = CareGiverLiteService.DeleteNurseCoordinator(NurseCoordinatorId, Membership.GetUser().ProviderUserKey.ToString()).Result;
                result = CareGiverLiteService.DeleteNurseCoordinator(NurseCoordinatorId, mUser.ProviderUserKey.ToString()).Result;

            
                if (result == "Success")
                {
                    var QuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(NurseCoordinatorUserId).Result;

                    ChattingController ChattingController = new ChattingController();
                    ChattingController.DeleteOneToOneChatByUserId(NurseCoordinatorUserId);

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
                log.Methodname = "DeleteNurseCoordinator";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        [HttpPost]
        public ActionResult EditNurseCoordinator(NurseCoordinatorModel objNurseCoordinatorModel)
        {
            try
            {
                NurseCoordinatorServiceProxy NurseCoordinatorService = new NurseCoordinatorServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                //MembershipUser mUser = Membership.GetUser(objNurseCoordinatorModel.Username);
                //mUser.ChangePassword(mUser.ResetPassword(), objNurseCoordinatorModel.Password);

                var NurseCoordinator = new NurseCoordinator();
                NurseCoordinator.NurseCoordinatorId = objNurseCoordinatorModel.NurseCoordinatorId;
                NurseCoordinator.FirstName = objNurseCoordinatorModel.FirstName;
                NurseCoordinator.LastName = objNurseCoordinatorModel.LastName;
                NurseCoordinator.Email = objNurseCoordinatorModel.Email;
                //NurseCoordinator.Password = objNurseCoordinatorModel.Password;
                NurseCoordinator.OfficeId = objNurseCoordinatorModel.OfficeId;
                NurseCoordinator.IsAllowForPatientChatRoom = objNurseCoordinatorModel.IsAllowForPatientChatRoom;
                NurseCoordinator.JobTitle = objNurseCoordinatorModel.JobTitle;
                NurseCoordinator.InsertUserId = InsertedUserID;
                NurseCoordinator.IsAllowOneToOneChat = objNurseCoordinatorModel.IsAllowOneToOneChat;
                NurseCoordinator.IsAllowGroupChat = objNurseCoordinatorModel.IsAllowGroupChat;
                NurseCoordinator.IsAllowToCreateGroupChat = objNurseCoordinatorModel.IsAllowToCreateGroupChat;
                NurseCoordinator.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                string result = NurseCoordinatorService.EditNurseCoordinator(NurseCoordinator).Result;
                if (result == "Success")
                {
                    var ChattingController = new ChattingController();

                    var ChattingService = new ChattingServiceProxy();
                    ChattingsList ChattingsList = new ChattingsList();

                    //  if (objNurseCoordinatorModel.OldEmail != objNurseCoordinatorModel.Email)

                    if (!string.Equals(objNurseCoordinatorModel.OldEmail, objNurseCoordinatorModel.Email, StringComparison.OrdinalIgnoreCase)  && !string.IsNullOrEmpty(objNurseCoordinatorModel.OldEmail))
                    {
                        
                        Task.Run(async () => { await ChattingController.ApiToChangeEmailToQuickblox(objNurseCoordinatorModel.OldEmail, objNurseCoordinatorModel.Email, objNurseCoordinatorModel.QuickBloxId); }).Wait();
                        //ChattingController.ApiToChangeEmailToQuickblox(objNurseCoordinatorModel.OldEmail, objNurseCoordinatorModel.Email, objNurseCoordinatorModel.QuickBloxId);
                    }

                    if (objNurseCoordinatorModel.OldOfficeId != objNurseCoordinatorModel.OfficeId)
                    {
                        ChattingController.RemoveMemberFromOfficeGroup(objNurseCoordinatorModel.OldOfficeId.ToString(), objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId, NurseCoordinator.OrganisationId,OrganisationEmail);

                        ChattingController.AddMemberIntoOfficeGroup(objNurseCoordinatorModel.OfficeId.ToString(), objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId, NurseCoordinator.OrganisationId,OrganisationEmail);

                        //// ************************************************************************
                        ////// When Office change that time remove user from all existing patient chat room
                        List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objNurseCoordinatorModel.UserId, "1").Result;
                        ChattingsList.objChattingsList = listChatting;
                        foreach (var item in listChatting)
                        {
                            ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId,NurseCoordinator.OrganisationId,OrganisationEmail);
                        }

                        //// When Office change that time remove user from all existing group chat 
                        List<Chatting> listGroupChatting = ChattingService.GetChatGroupListByTypeIdForUser(objNurseCoordinatorModel.UserId, "2").Result;
                        ChattingsList.objChattingsList = listGroupChatting;
                        foreach (var item in listGroupChatting)
                        {
                            ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId, NurseCoordinator.OrganisationId,OrganisationEmail);
                        }
                        ChattingController.RemoveMemberFromOfficeGroup(objNurseCoordinatorModel.OldOfficeId.ToString(), objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId,NurseCoordinator.OrganisationId, OrganisationEmail);
                        // ************************************************************************

                    }

                    if (!objNurseCoordinatorModel.IsAllowForPatientChatRoom)
                    {
                        // ChattingController.RemoveMemberFromGroupChat();

                        //var ChattingService = new ChattingServiceProxy();
                        //ChattingsList ChattingsList = new ChattingsList();
                        List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objNurseCoordinatorModel.UserId, "1").Result;
                        ChattingsList.objChattingsList = listChatting;


                        foreach (var item in listChatting)
                        {
                            ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId, NurseCoordinator.OrganisationId,OrganisationEmail);
                        }

                    }

                    if (!objNurseCoordinatorModel.IsAllowGroupChat)
                    {
                        // ChattingController.RemoveMemberFromGroupChat();

                        //var ChattingService = new ChattingServiceProxy();
                        //ChattingsList ChattingsList = new ChattingsList();
                        List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objNurseCoordinatorModel.UserId, "2").Result;
                        ChattingsList.objChattingsList = listChatting;


                        foreach (var item in listChatting)
                        {
                            ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId, NurseCoordinator.OrganisationId,OrganisationEmail);
                        }
                        ChattingController.RemoveMemberFromOfficeGroup(objNurseCoordinatorModel.OldOfficeId.ToString(), objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId, NurseCoordinator.OrganisationId,OrganisationEmail);
                    }

                    if (objNurseCoordinatorModel.IsAllowGroupChat)
                    {
                        // ChattingController.RemoveMemberFromGroupChat();

                        //var ChattingService = new ChattingServiceProxy();
                        //ChattingsList ChattingsList = new ChattingsList();
                        List<Chatting> listChatting = ChattingService.GetChatGroupListByTypeIdForUser(objNurseCoordinatorModel.UserId, "3").Result;
                        ChattingsList.objChattingsList = listChatting;

                        if (listChatting.Count == 0)
                        {
                            ChattingController.AddMemberIntoOfficeGroup(objNurseCoordinatorModel.OfficeId.ToString(), objNurseCoordinatorModel.UserId, objNurseCoordinatorModel.QuickBloxId, NurseCoordinator.OrganisationId, OrganisationEmail);
                        }
                    }

                    TempData["Message"] = "Office Staff is updated successfully.";
                    return RedirectToAction("NurseCoordinator", "NurseCoordinator", new { IsAdded = true });
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "SchedulersController";
                log.Methodname = "[HttpPost] EditNurseCoordinator";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }

        public ActionResult EditNurseCoordinator(string id)
        {
            NurseCoordinatorModel objModel = new NurseCoordinatorModel();
            try
            {
                objModel = GetNurseCoordinatorDetailById(id);
                FillAllOffices(objModel.OfficeId > 0 ? (object)objModel.OfficeId : null);
                objModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "EditNurseCoordinator";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }
    }
}