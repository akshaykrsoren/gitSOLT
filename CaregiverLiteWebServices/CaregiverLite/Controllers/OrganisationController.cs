using CaregiverLite.Models;
using CaregiverLiteWCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration.Provider;
using CaregiverLite.Action_Filters;
using DifferenzLibrary;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class OrganisationController : Controller
    {


        // GET: Organisation
        //public ActionResult Organisation()
        //{
        //    return View();
        //}


        //public ActionResult GetAllOrganisationsList(JQueryDataTableParamModel param)
        //{
        //    string UserID = Membership.GetUser().ProviderUserKey.ToString();
        //    OrganisationsList OrganisationList = new OrganisationsList();

        //    try
        //    {
        //        string sortOrder = string.Empty;
        //        var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

        //        if (sortColumnIndex == 0)
        //        {
        //            sortOrder = "OrganisationName";
        //        }
        //        else if (sortColumnIndex == 1)
        //        {
        //            sortOrder = "OrganisationName";
        //        }
        //        else if (sortColumnIndex == 2)
        //        {
        //            sortOrder = "OrganisationName";
        //        }
        //        else if (sortColumnIndex == 3)
        //        {
        //            sortOrder = "Street";
        //        }
        //        else if (sortColumnIndex == 4)
        //        {
        //            sortOrder = "City";
        //        }
        //        else if (sortColumnIndex == 5)
        //        {
        //            sortOrder = "State";
        //        }
        //        else if (sortColumnIndex == 6)
        //        {
        //            sortOrder = "Zipcode";
        //        }
        //        else
        //        {
        //            sortOrder = "OrganisationName";
        //        }


        //        string search = "||"; //It's indicate blank filter

        //        if (!string.IsNullOrEmpty(param.sSearch))
        //            search = param.sSearch;

        //        var sortDirection = Request["sSortDir_0"]; // asc or desc
        //        int pageNo = 1;
        //        int recordPerPage = param.iDisplayLength;

        //        //Find page number from the logic
        //        if (param.iDisplayStart > 0)
        //        {
        //            pageNo = (param.iDisplayStart / recordPerPage) + 1;
        //        }

        //        OrganisationServiceProxy OrganisationLiteService = new OrganisationServiceProxy();
        //        OrganisationList = OrganisationLiteService.GetAllOrganisations(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection).Result;
        //        //ViewBag.OrganisationDropDownList = GetOrganisationDropDownList();
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "OrganisationController";
        //        log.Methodname = "GetAllOrganisations";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    if (OrganisationList.OrganisationList != null)
        //    {
        //        var result = from C in OrganisationList.OrganisationList select new[] { C, C, C, C, C, C, C, C, C, C, C };
        //        return Json(new
        //        {
        //            sEcho = param.sEcho,
        //            iTotalRecords = OrganisationList.TotalNumberofRecord,
        //            iTotalDisplayRecords = OrganisationList.FilteredRecord,
        //            aaData = result
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            sEcho = param.sEcho,
        //            iTotalRecords = OrganisationList.TotalNumberofRecord,
        //            iTotalDisplayRecords = OrganisationList.FilteredRecord
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //#region AddOrganisation

        //public ActionResult _AddOrganisation()
        //{

        //    //   ViewBag.lstLanguage = new SelectList(lstLanguage, "LanguageId", "LanguageName", ViewBag.LanguageId);

        //    List<Admin> AdminList = new List<Admin>();

        //    OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
        //    // ViewBag.ServiceList = OrganisationService.GetAllAdminName().Result;
        //    AdminList = OrganisationService.GetAllOrganisationAdminName().Result;

        //    //  var AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

        //    //   ViewData["AdminNameList"] = AdminNameList;


        //    ViewBag.AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

        //    return PartialView("_AddOrganisation");
        //}

        //[HttpPost]
        //public ActionResult AddOrganisation(OrganisationModel objOrganisationModel)
        //{
        //    try
        //    {
        //        OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
        //        string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
        //        var Organisation = new Organisation();
        //        Organisation.OrganisationId = objOrganisationModel.OrganisationId;
        //        Organisation.OrganisationName = objOrganisationModel.OrganisationName;
        //        Organisation.OrganisationAdminUserId = objOrganisationModel.OrganisationAdminUserId;
        //        Organisation.Street = objOrganisationModel.Street;
        //        Organisation.City = objOrganisationModel.City;
        //        Organisation.State = objOrganisationModel.State;
        //        Organisation.ZipCode = objOrganisationModel.ZipCode;
        //        Organisation.Latitude = objOrganisationModel.Latitude;
        //        Organisation.Longitude = objOrganisationModel.Longitude;
        //        Organisation.TimezoneId = objOrganisationModel.TimezoneId;
        //        Organisation.TimezoneOffset = Convert.ToInt32(objOrganisationModel.TimezoneOffset);
        //        Organisation.TimezonePostfix = objOrganisationModel.TimezonePostfix;
        //        Organisation.InsertUserId = InsertedUserID;


        //        Organisation objOrganisation = OrganisationService.AddOrganisation(Organisation).Result;

        //        if (objOrganisation.Result == "Success")
        //        {
        //            //List<string> UserQBIDs = new List<string>();

        //            //var SchedulerList = new List<ScheduleInfo>();

        //            //var ChattingService = new ChattingServiceProxy();

        //            //SchedulerList = ChattingService.GetALLSuperadminList().Result;

        //            ////  UserQBIDs.Add("32168516"); // this is second super admin QB ID 


        //            //foreach (var item in SchedulerList)
        //            //{
        //            //    UserQBIDs.Add(item.QuickbloxId);

        //            //    objOrganisation.AdminUserId = objOrganisation.AdminUserId + "," + item.UserId;
        //            //}

        //            //UserQBIDs.Add(objOrganisation.AdminQuickBloxId);

        //            //Task.Run(async () => { await ChattingController.CreateGroupChatOnQuickBloxRestAPI(objOrganisation.OrganisationName, UserQBIDs, objOrganisation.OrganisationId, objOrganisation.AdminUserId); }).Wait();


        //            TempData["Message"] = "Organisation is added successfully.";
        //            return RedirectToAction("Organisation", "Organisation", new { IsAdded = true });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Organisation", "Organisation");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "OrganisationController";
        //        log.Methodname = "[HttpPost] AddOrganisation";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return PartialView();
        //}
        //#endregion

        //#region EditOrganisation
        //public ActionResult _EditOrganisation(string id)
        //{
        //    //if (InsertedUserID == null)
        //    //{
        //    //    return RedirectToAction("SessionTimeout", "Account");
        //    //}
        //    OrganisationModel objModel = new OrganisationModel();
        //    try
        //    {
        //        List<Admin> AdminList = new List<Admin>();

        //        OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
        //        AdminList = OrganisationService.GetAllOrganisationAdminName().Result;
        //        ViewBag.AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

        //        objModel = GetOrganisationDetailByOrganisationId(id);

        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "OrganisationController";
        //        log.Methodname = "_EditOrganisation";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return PartialView(objModel);
        //}

        //public static OrganisationModel GetOrganisationDetailByOrganisationId(string id)
        //{
        //    OrganisationModel objOrganisation = new OrganisationModel();
        //    try
        //    {
        //        OrganisationServiceProxy CareGiverLiteService = new OrganisationServiceProxy();
        //        Organisation Organisation = CareGiverLiteService.GetOrganisationDetailByOrganisationId(id).Result;

        //        objOrganisation.OrganisationId = Organisation.OrganisationId;
        //        objOrganisation.OrganisationName = Organisation.OrganisationName;
        //        objOrganisation.OrganisationAdminUserId = Organisation.OrganisationAdminUserId;
        //        objOrganisation.Street = Organisation.Street;
        //        objOrganisation.City = Organisation.City;
        //        objOrganisation.State = Organisation.State;
        //        objOrganisation.ZipCode = Organisation.ZipCode;
        //        objOrganisation.OrganisationAdminEmail = Organisation.AdminEmail;

        //      // objOrganisation.or = Organisation.AdminQuickBloxId;

        //        objOrganisation.OldAdminUserId = Organisation.OrganisationAdminUserId;

        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "EditOrganisation";
        //        log.Methodname = "GetOrganisationDetailByOrganisationId";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return objOrganisation;
        //    //}
        //}

        //[HttpPost]
        //public ActionResult EditOrganisation(OrganisationModel objOrganisationModel)
        //{
        //    try
        //    {
        //        var ChattingService = new ChattingServiceProxy();
        //        ChattingsList ChattingsList = new ChattingsList();

        //        OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
        //        string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
        //        var Organisation = new Organisation();
        //        Organisation.OrganisationId = objOrganisationModel.OrganisationId;
        //        Organisation.OrganisationName = objOrganisationModel.OrganisationName;
        //        Organisation.OrganisationAdminUserId = objOrganisationModel.OrganisationAdminUserId;
        //        Organisation.Street = objOrganisationModel.Street;
        //        Organisation.City = objOrganisationModel.City;
        //        Organisation.State = objOrganisationModel.State;
        //        Organisation.ZipCode = objOrganisationModel.ZipCode;
        //        Organisation.Latitude = objOrganisationModel.Latitude;
        //        Organisation.Longitude = objOrganisationModel.Longitude;
        //        Organisation.TimezoneId = objOrganisationModel.TimezoneId;
        //        Organisation.TimezoneOffset = Convert.ToInt32(objOrganisationModel.TimezoneOffset);
        //        Organisation.TimezonePostfix = objOrganisationModel.TimezonePostfix;
        //        Organisation.InsertUserId = InsertedUserID;


        //        string result = OrganisationService.EditOrganisation(Organisation).Result;

        //        if (result == "Success")
        //        {

        //            //if (objOrganisationModel.OldAdminUserId != objOrganisationModel.AdminUserId)
        //            //{
        //            //    var ChattingController = new ChattingController();

        //            //    var NewAdminQuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(objOrganisationModel.AdminUserId).Result;

        //            //    ChattingController.RemoveMemberFromOrganisationGroup(objOrganisationModel.OrganisationId.ToString(), objOrganisationModel.OldAdminUserId, objOrganisationModel.AdminQuickBloxId);

        //            //    ChattingController.AddMemberIntoOrganisationGroup(objOrganisationModel.OrganisationId.ToString(), objOrganisationModel.AdminUserId, NewAdminQuickBloxId);

        //            //    // remove user from all old Organisation group
        //            //    List<Chatting> listGroupChatting = ChattingService.GetChatGroupListByOrganisationIdForUser(objOrganisationModel.OldAdminUserId, "2", objOrganisationModel.OrganisationId.ToString()).Result;
        //            //    ChattingsList.objChattingsList = listGroupChatting;
        //            //    foreach (var item in listGroupChatting)
        //            //    {
        //            //        ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objOrganisationModel.OldAdminUserId, objOrganisationModel.AdminQuickBloxId);
        //            //    }

        //            //    AddAdminIntoPatientGroupChat(Organisation.OrganisationId);
        //            //}
        //            //  AddAdminIntoOrganisationGroup(Organisation.OrganisationId);
        //            // AddAdminIntoPatientGroupChat(Organisation.OrganisationId);
        //            TempData["Message"] = "Organisation is Updated successfully.";
        //            return RedirectToAction("Organisation", "Organisation", new { IsAdded = true });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Organisation", "Organisation");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "OrganisationController";
        //        log.Methodname = "[HttpPost] AddOrganisation";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return PartialView();
        //}
        //#endregion

        //#region DeleteOrganisation
        //public string DeleteOrganisation(string OrganisationId)
        //{
        //    string result = "";
        //    try
        //    {
        //        string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

        //        OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
        //        result = OrganisationService.DeleteOrganisationByOrganisationId(OrganisationId, InsertedUserID).Result;

        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "CareGiverController";
        //        log.Methodname = "DeleteOrganisation";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return result;
        //}

        //#endregion

        //#region AssignOrganisation
        //public ActionResult _AssignZipcodes(string id)
        //{
        //    OrganisationModel objModel = new OrganisationModel();
        //    objModel.OrganisationId = Convert.ToInt32(id);
        //    objModel.AssignedZipcodes = GetAssignZipcodes(Convert.ToInt32(id));
        //    return PartialView(objModel);
        //}

        //[HttpPost]
        //public JsonResult AssignZipcodes(int OrganisationId, string AssignedZipcodes)
        //{
        //    string result = "";
        //    try
        //    {
        //        OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
        //        string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();



        //        var Organisation = new Organisation();
        //        Organisation.OrganisationId = OrganisationId;
        //        Organisation.AssignedZipcodes = AssignedZipcodes;
        //        Organisation.InsertUserId = InsertedUserID;


        //        result = OrganisationService.AssignZipcodesToOrganisationByOrganisationId(Organisation).Result;


        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "OrganisationController";
        //        log.Methodname = "[HttpPost] AddOrganisation";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //public string GetAssignZipcodes(int OrganisationId)
        //{
        //    string result = "";
        //    try
        //    {
        //        OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
        //        result = OrganisationService.GetAssignZipcodesToOrganisationByOrganisationId(OrganisationId).Result;
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "OrganisationController";
        //        log.Methodname = "GetAssignZipcodes";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return result;
        //}
        //#endregion

        //#region GetOrganisationDropDownList
        ////Static Method (Call From Anywhere)
        //public static OrganisationsList GetOrganisationDropDownList()
        //{
        //    var objOrganisationsList = new OrganisationsList();
        //    try
        //    {
        //        string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
        //        objOrganisationsList = CaregiverLiteService.GetOrganisationDropDownList(InsertedUserID);
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "OrganisationController";
        //        log.Methodname = "GetOrganisationDropDownList";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    return objOrganisationsList;
        //}
        //#endregion


        public ActionResult Organisation()
        {
            return View();
        }


        public ActionResult GetAllOrganisationsList(JQueryDataTableParamModel param)
        {
            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            OrganisationsList OrganisationList = new OrganisationsList();

            try
            {
                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "OrganisationName";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "OrganisationName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "OrganisationName";
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
                    sortOrder = "Zipcode";
                }
                else
                {
                    sortOrder = "OrganisationName";
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

                OrganisationServiceProxy OrganisationLiteService = new OrganisationServiceProxy();
                OrganisationList = OrganisationLiteService.GetAllOrganisations(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection).Result;
                //ViewBag.OrganisationDropDownList = GetOrganisationDropDownList();
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OrganisationController";
                log.Methodname = "GetAllOrganisations";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (OrganisationList.OrganisationList != null)
            {
                var result = from C in OrganisationList.OrganisationList select new[] { C, C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = OrganisationList.TotalNumberofRecord,
                    iTotalDisplayRecords = OrganisationList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = OrganisationList.TotalNumberofRecord,
                    iTotalDisplayRecords = OrganisationList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }


        #region AddOrganisation

        public ActionResult _AddOrganisation()
        {

            //   ViewBag.lstLanguage = new SelectList(lstLanguage, "LanguageId", "LanguageName", ViewBag.LanguageId);

            List<Admin> AdminList = new List<Admin>();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
            // ViewBag.ServiceList = OrganisationService.GetAllAdminName().Result;
            AdminList = OrganisationService.GetAllOrganisationAdminName(OrganisationId).Result;

            //  var AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

            //   ViewData["AdminNameList"] = AdminNameList;


            ViewBag.AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

            return PartialView("_AddOrganisation");
        }

        [HttpPost]
        public ActionResult AddOrganisation(OrganisationModel objOrganisationModel)
        {
            try
            {        
                OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();

                //string Username = Membership.GetUser(objOrganisationModel.OrganisationAdminUserId).UserName.ToString();
                var UserIdForUserName = Guid.Parse(objOrganisationModel.OrganisationAdminUserId);

                string Username = Membership.GetUser(UserIdForUserName).UserName.ToString();

                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                //if (objOrganisationModel.OrganisationId > 0)
                //{
                //    //before
                //    //Roles.RemoveUserFromRole(Username, "Admin");        

                //    //if (!Roles.IsUserInRole(Username, "OrgSuperAdmin"))
                //    //{
                //    //    Roles.AddUserToRole(Username, "OrgSuperAdmin");
                //    //}

                //    //Now Same add and edit org
                //    if (Roles.IsUserInRole(Username, "Admin"))
                //    {
                //        Roles.RemoveUserFromRole(Username, "Admin");
                //    }

                //    if (!Roles.IsUserInRole(Username, "OrgSuperAdmin"))
                //    {
                //        Roles.AddUserToRole(Username, "OrgSuperAdmin");
                //    }
                //}

                if (objOrganisationModel.OrganisationAdminUserId != null)
                {
                    Roles.RemoveUserFromRole(Username, "Admin");

                    if (!Roles.IsUserInRole(Username, "OrgSuperAdmin"))
                    {
                        Roles.AddUserToRole(Username, "OrgSuperAdmin");
                    }
                }

                var Organisation = new Organisation();
                Organisation.OrganisationId = objOrganisationModel.OrganisationId;
                Organisation.OrganisationName = objOrganisationModel.OrganisationName;
                Organisation.OrganisationAdminUserId = objOrganisationModel.OrganisationAdminUserId;
                Organisation.Street = objOrganisationModel.Street;
                Organisation.City = objOrganisationModel.City;
                Organisation.State = objOrganisationModel.State;
                Organisation.ZipCode = objOrganisationModel.ZipCode;
                Organisation.Latitude = objOrganisationModel.Latitude;
                Organisation.Longitude = objOrganisationModel.Longitude;
                Organisation.TimezoneId = objOrganisationModel.TimezoneId;
                Organisation.TimezoneOffset = Convert.ToInt32(objOrganisationModel.TimezoneOffset);
                Organisation.TimezonePostfix = objOrganisationModel.TimezonePostfix;
                Organisation.InsertUserId = InsertedUserID;


                Organisation objOrganisation = OrganisationService.AddOrganisation(Organisation).Result;

                if (objOrganisation.Result == "Success")
                {

                    //List<string> UserQBIDs = new List<string>();

                    //var SchedulerList = new List<ScheduleInfo>();

                    //var ChattingService = new ChattingServiceProxy();

                    //SchedulerList = ChattingService.GetALLSuperadminList().Result;

                    ////  UserQBIDs.Add("32168516"); // this is second super admin QB ID 

                    //foreach (var item in SchedulerList)
                    //{
                    //    UserQBIDs.Add(item.QuickbloxId);

                    //    objOrganisation.AdminUserId = objOrganisation.AdminUserId + "," + item.UserId;
                    //}

                    //UserQBIDs.Add(objOrganisation.AdminQuickBloxId);

                    //Task.Run(async () => { await ChattingController.CreateGroupChatOnQuickBloxRestAPI(objOrganisation.OrganisationName, UserQBIDs, objOrganisation.OrganisationId, objOrganisation.AdminUserId); }).Wait();


                    TempData["Message"] = "Organisation is added successfully.";
                    return RedirectToAction("Organisation", "Organisation", new { IsAdded = true });
                }
                else
                {
                    return RedirectToAction("Organisation", "Organisation");
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OrganisationController";
                log.Methodname = "[HttpPost] AddOrganisation";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }
        #endregion

        #region EditOrganisation
        public ActionResult _EditOrganisation(string id)
        {

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            //if (InsertedUserID == null)
            //{
            //    return RedirectToAction("SessionTimeout", "Account");
            //}

            OrganisationModel objModel = new OrganisationModel();
            try
            {
                List<Admin> AdminList = new List<Admin>();

                OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
                AdminList = OrganisationService.GetAllOrganisationAdminName(OrganisationId).Result;
                ViewBag.AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

                objModel = GetOrganisationDetailByOrganisationId(id);

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OrganisationController";
                log.Methodname = "_EditOrganisation";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }

        public static OrganisationModel GetOrganisationDetailByOrganisationId(string id)
        {
            OrganisationModel objOrganisation = new OrganisationModel();
            try
            {
                OrganisationServiceProxy CareGiverLiteService = new OrganisationServiceProxy();
                Organisation Organisation = CareGiverLiteService.GetOrganisationDetailByOrganisationId(id).Result;

                objOrganisation.OrganisationId = Organisation.OrganisationId;
                objOrganisation.OrganisationName = Organisation.OrganisationName;
                objOrganisation.OrganisationAdminUserId = Organisation.OrganisationAdminUserId;
                objOrganisation.Street = Organisation.Street;
                objOrganisation.City = Organisation.City;
                objOrganisation.State = Organisation.State;
                objOrganisation.ZipCode = Organisation.ZipCode;
                objOrganisation.OrganisationAdminEmail = Organisation.AdminEmail;

                // objOrganisation.or = Organisation.AdminQuickBloxId;

                objOrganisation.OldAdminUserId = Organisation.OrganisationAdminUserId;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "EditOrganisation";
                log.Methodname = "GetOrganisationDetailByOrganisationId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objOrganisation;
            //}
        }

        [HttpPost]
        public ActionResult EditOrganisation(OrganisationModel objOrganisationModel)
        {
            try
            {
                var ChattingService = new ChattingServiceProxy();
                ChattingsList ChattingsList = new ChattingsList();

                //old orguserid is and new is same so do not use and update roles.


                OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                var UserIdForUserName =Guid.Parse(objOrganisationModel.OrganisationAdminUserId);
             
                string Username = Membership.GetUser(UserIdForUserName).UserName.ToString();


                //if (objOrganisationModel.OrganisationId > 0)
                //{
                //    if (Roles.IsUserInRole(Username, "Admin"))
                //    {
                //        Roles.RemoveUserFromRole(Username, "Admin");
                //    }

                //    if (!Roles.IsUserInRole(Username, "OrgSuperAdmin"))
                //    {
                //        Roles.AddUserToRole(Username, "OrgSuperAdmin");
                //    }
                //}

                if (objOrganisationModel.OrganisationAdminUserId != null)
                {
                    Roles.RemoveUserFromRole(Username, "Admin");

                    if (!Roles.IsUserInRole(Username, "OrgSuperAdmin"))
                    {
                        Roles.AddUserToRole(Username, "OrgSuperAdmin");
                    }
                }


                var Organisation = new Organisation();
                Organisation.OrganisationId = objOrganisationModel.OrganisationId;
                Organisation.OrganisationName = objOrganisationModel.OrganisationName;
                Organisation.OrganisationAdminUserId = objOrganisationModel.OrganisationAdminUserId;
                Organisation.Street = objOrganisationModel.Street;
                Organisation.City = objOrganisationModel.City;
                Organisation.State = objOrganisationModel.State;
                Organisation.ZipCode = objOrganisationModel.ZipCode;
                Organisation.Latitude = objOrganisationModel.Latitude;
                Organisation.Longitude = objOrganisationModel.Longitude;
                Organisation.TimezoneId = objOrganisationModel.TimezoneId;
                Organisation.TimezoneOffset = Convert.ToInt32(objOrganisationModel.TimezoneOffset);
                Organisation.TimezonePostfix = objOrganisationModel.TimezonePostfix;
                Organisation.InsertUserId = InsertedUserID;

                string result = EditOrganisation(Organisation);

                    //OrganisationService.EditOrganisation(Organisation).Result;

                if (result == "Success")
                {

                    //if (objOrganisationModel.OldAdminUserId != objOrganisationModel.AdminUserId)
                    //{
                    //    var ChattingController = new ChattingController();

                    //    var NewAdminQuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(objOrganisationModel.AdminUserId).Result;

                    //    ChattingController.RemoveMemberFromOrganisationGroup(objOrganisationModel.OrganisationId.ToString(), objOrganisationModel.OldAdminUserId, objOrganisationModel.AdminQuickBloxId);

                    //    ChattingController.AddMemberIntoOrganisationGroup(objOrganisationModel.OrganisationId.ToString(), objOrganisationModel.AdminUserId, NewAdminQuickBloxId);

                    //    // remove user from all old Organisation group
                    //    List<Chatting> listGroupChatting = ChattingService.GetChatGroupListByOrganisationIdForUser(objOrganisationModel.OldAdminUserId, "2", objOrganisationModel.OrganisationId.ToString()).Result;
                    //    ChattingsList.objChattingsList = listGroupChatting;
                    //    foreach (var item in listGroupChatting)
                    //    {
                    //        ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objOrganisationModel.OldAdminUserId, objOrganisationModel.AdminQuickBloxId);
                    //    }

                    //    AddAdminIntoPatientGroupChat(Organisation.OrganisationId);
                    //}
                    //  AddAdminIntoOrganisationGroup(Organisation.OrganisationId);
                    // AddAdminIntoPatientGroupChat(Organisation.OrganisationId);

                    TempData["Message"] = "Organisation is Updated successfully.";
                    return RedirectToAction("Organisation", "Organisation", new { IsAdded = true });
                }
                else
                {
                    return RedirectToAction("Organisation", "Organisation");
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OrganisationController";
                log.Methodname = "[HttpPost] AddOrganisation";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }
        #endregion

        #region DeleteOrganisation
        public string DeleteOrganisation(string OrganisationId)
        {
            string result = "";
            try
            {
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
                result = OrganisationService.DeleteOrganisationByOrganisationId(OrganisationId, InsertedUserID).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "DeleteOrganisation";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        #endregion

        #region AssignOrganisation
        public ActionResult _AssignZipcodes(string id)
        {
            OrganisationModel objModel = new OrganisationModel();
            objModel.OrganisationId = Convert.ToInt32(id);
            objModel.AssignedZipcodes = GetAssignZipcodes(Convert.ToInt32(id));
            return PartialView(objModel);
        }

        [HttpPost]
        public JsonResult AssignZipcodes(int OrganisationId, string AssignedZipcodes)
        {
            string result = "";
            try
            {
                OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();



                var Organisation = new Organisation();
                Organisation.OrganisationId = OrganisationId;
                Organisation.AssignedZipcodes = AssignedZipcodes;
                Organisation.InsertUserId = InsertedUserID;


                result = OrganisationService.AssignZipcodesToOrganisationByOrganisationId(Organisation).Result;


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OrganisationController";
                log.Methodname = "[HttpPost] AddOrganisation";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string GetAssignZipcodes(int OrganisationId)
        {
            string result = "";
            try
            {
                OrganisationServiceProxy OrganisationService = new OrganisationServiceProxy();
                result = OrganisationService.GetAssignZipcodesToOrganisationByOrganisationId(OrganisationId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OrganisationController";
                log.Methodname = "GetAssignZipcodes";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }
        #endregion

        #region GetOrganisationDropDownList

        //Static Method (Call From Anywhere)
        public static OrganisationsList GetOrganisationDropDownList()
        {
            var objOrganisationsList = new OrganisationsList();
            try
            {
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                objOrganisationsList = CaregiverLiteService.GetOrganisationDropDownList(InsertedUserID);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OrganisationController";
                log.Methodname = "GetOrganisationDropDownList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objOrganisationsList;
        }

        public string EditOrganisation(Organisation organisation)
        {

            string result = "";
            try
            {
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "EditOffice",
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_Editorganisation",
                                                    organisation.OrganisationId,
                                                    organisation.OrganisationName,
                                                    organisation.OrganisationAdminUserId,
                                                    organisation.Street,
                                                    organisation.City,
                                                    organisation.State,
                                                    organisation.ZipCode,
                                                    organisation.Latitude,
                                                    organisation.Longitude,
                                                    Guid.Parse(organisation.InsertUserId),
                                                    organisation.TimezoneId,
                                                    organisation.TimezoneOffset,
                                                    organisation.TimezonePostfix
                                                    //organisation.OrganisationId
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
                objErrorlog.Methodname = "EditOrganisation";
                objErrorlog.UserID = organisation.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
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



        #endregion

    }

}