using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using CaregiverLiteWCF.Class;
using DifferenzLibrary;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class OfficeController : Controller
    {
        // GET: Office
        public ActionResult Office()
        {
            return View();
        }


        public ActionResult GetAllOfficesList(JQueryDataTableParamModel param)
        {
            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            OfficesList OfficeList = new OfficesList();
            try
            {
                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "OfficeName";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "OfficeName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "AdminName";
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
                    sortOrder = "OfficeName";
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

                OfficeServiceProxy OfficeLiteService = new OfficeServiceProxy();
                OfficeList = OfficeLiteService.GetAllOffices(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection,OrganisationId.ToString()).Result;
                //ViewBag.OfficeDropDownList = GetOfficeDropDownList();
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OfficeController";
                log.Methodname = "GetAllOffices";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (OfficeList.OfficeList != null)
            {
                var result = from C in OfficeList.OfficeList select new[] { C, C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = OfficeList.TotalNumberofRecord,
                    iTotalDisplayRecords = OfficeList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = OfficeList.TotalNumberofRecord,
                    iTotalDisplayRecords = OfficeList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }


        #region AddOffice

        public ActionResult _AddOffice()
        {

            //   ViewBag.lstLanguage = new SelectList(lstLanguage, "LanguageId", "LanguageName", ViewBag.LanguageId);

            List<Admin> AdminList = new List<Admin>();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            OfficeServiceProxy OfficeService = new OfficeServiceProxy();
            // ViewBag.ServiceList = OfficeService.GetAllAdminName().Result;
              AdminList = OfficeService.GetAllAdminName(OrganisationId.ToString()).Result;

            //  var AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

            //   ViewData["AdminNameList"] = AdminNameList;


            ViewBag.AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

            return PartialView("_AddOffice");
        }


        [HttpPost]
        public ActionResult AddOffice(OfficeModel objOfficeModel)
        {
            try
            {
                OfficeServiceProxy OfficeService = new OfficeServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                var Office = new Office();
                Office.OfficeId = objOfficeModel.OfficeId;
                Office.OfficeName = objOfficeModel.OfficeName;
                Office.AdminUserId = objOfficeModel.AdminUserId;
                Office.Street = objOfficeModel.Street;
                Office.City = objOfficeModel.City;
                Office.State = objOfficeModel.State;
                Office.ZipCode = objOfficeModel.ZipCode;
                Office.Latitude = objOfficeModel.Latitude;
                Office.Longitude = objOfficeModel.Longitude;
                Office.TimezoneId = objOfficeModel.TimezoneId;
                Office.TimezoneOffset = Convert.ToInt32(objOfficeModel.TimezoneOffset);
                Office.TimezonePostfix = objOfficeModel.TimezonePostfix;
                Office.InsertUserId = InsertedUserID;
                Office.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                Office objOffice = OfficeService.AddOffice(Office).Result;

                if (objOffice.Result == "Success")
                {
                    List<string> UserQBIDs = new List<string>();

                    var SchedulerList = new List<ScheduleInfo>();

                    var ChattingService = new ChattingServiceProxy();

                    int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                    if (OrganisationId > 0)
                    {
                        var quickbloxds = Convert.ToString(Session["OrgSuperAdminQBId"]);

                        var OrgSuperAdminEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);
                        var OrgSuperAdminUserId = Convert.ToString(Session["OrgSuperAdminUserId"]);

                        UserQBIDs.Add(quickbloxds);

                        UserQBIDs.Add(objOffice.AdminQuickBloxId);


                        SchedulerList = GetALLSuperadminListOrganisationBased(OrganisationId);
                        foreach (var item in SchedulerList)
                        {
                            UserQBIDs.Add(item.QuickbloxId);
                            objOffice.AdminUserId = objOffice.AdminUserId + "," + item.UserId;
                        }


                        Task.Run(async () => { await ChattingController.CreateOrganisationBasedGroupChatOnQuickBloxRestAPI(objOffice.OfficeName, UserQBIDs, objOffice.OfficeId, objOffice.AdminUserId, OrganisationId, OrgSuperAdminUserId, OrgSuperAdminEmail); }).Wait();

                        TempData["Message"] = "Office is added successfully.";
                        return RedirectToAction("Office", "Office", new { IsAdded = true });


                        //SchedulerList = GetALLOrganisationSuperadminList();
                        //foreach (var item in SchedulerList)
                        //{
                        //    UserQBIDs.Add(item.QuickbloxId);
                        //    objOffice.AdminUserId = objOffice.AdminUserId + "," + item.UserId;
                        //}

                        //UserQBIDs.Add(objOffice.AdminQuickBloxId);
                        //ChattingController chatting = new ChattingController();
                        //Task.Run(async () => { await chatting.CreateOrganisationBasedGroupChatOnQuickBloxRestAPI(objOffice.OfficeName, UserQBIDs, objOffice.OfficeId, objOffice.AdminUserId,OrganisationId); }).Wait();

                        //TempData["Message"] = "Office is added successfully.";
                        //return RedirectToAction("Office", "Office", new { IsAdded = true });
                    }
                    else
                    {
                        SchedulerList = ChattingService.GetALLSuperadminList().Result;

                        foreach (var item in SchedulerList)
                        {
                            UserQBIDs.Add(item.QuickbloxId);
                            objOffice.AdminUserId = objOffice.AdminUserId + "," + item.UserId;
                        }

                        UserQBIDs.Add(objOffice.AdminQuickBloxId);

                        Task.Run(async () => { await ChattingController.CreateGroupChatOnQuickBloxRestAPI(objOffice.OfficeName, UserQBIDs, objOffice.OfficeId, objOffice.AdminUserId); }).Wait();

                        TempData["Message"] = "Office is added successfully.";
                        return RedirectToAction("Office", "Office", new { IsAdded = true });
                    }

                    // UserQBIDs.Add("32168516"); // this is second super admin QB ID 

                    //foreach (var item in SchedulerList)
                    //{
                    //    UserQBIDs.Add(item.QuickbloxId);

                    //    objOffice.AdminUserId = objOffice.AdminUserId + "," + item.UserId;
                    //}
                    
                    //UserQBIDs.Add(objOffice.AdminQuickBloxId);

               
                    
                    //Task.Run(async () => { await ChattingController.CreateGroupChatOnQuickBloxRestAPI(objOffice.OfficeName, UserQBIDs, objOffice.OfficeId,objOffice.AdminUserId); }).Wait();


                    //TempData["Message"] = "Office is added successfully.";
                    //return RedirectToAction("Office", "Office", new { IsAdded = true });
                }
                else
                {
                    return RedirectToAction("Office", "Office");
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OfficeController";
                log.Methodname = "[HttpPost] AddOffice";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }
        #endregion




        public List<ScheduleInfo> GetALLSuperadminListOrganisationBased(int OrganisationId)
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetALLSuperadminListOrganisationBased", Convert.ToInt32(OrganisationId));
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
                objErrorlog.Methodname = "GetALLSuperadminListorganisationbased";
                //string result = InsertErrorLog(objErrorlog);
            }
            return SchedulerList;
        }


        #region EditOffice
        public ActionResult _EditOffice(string id)
        {
            //if (InsertedUserID == null)
            //{
            //    return RedirectToAction("SessionTimeout", "Account");
            //}
        
            OfficeModel objModel = new OfficeModel();
            try
            {
                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                List<Admin> AdminList = new List<Admin>();
                OfficeServiceProxy OfficeService = new OfficeServiceProxy();
                AdminList = OfficeService.GetAllAdminName(OrganisationId.ToString()).Result;
                ViewBag.AdminNameList = new SelectList(AdminList, "UserId", "Name", ViewBag.UserId);

                if (OrganisationId > 0)
                {
                    objModel = GetOrganisationOfficeDetailByOfficeId(id,OrganisationId.ToString());
                }
                else
                {
                    objModel = GetOfficeDetailByOfficeId(id);
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OfficeController";
                log.Methodname = "_EditOffice";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }

        public static OfficeModel GetOfficeDetailByOfficeId(string id)
        {
            OfficeModel objOffice = new OfficeModel();
            try
            {
                
                OfficeServiceProxy CareGiverLiteService = new OfficeServiceProxy();
                Office Office = CareGiverLiteService.GetOfficeDetailByOfficeId(id).Result;

                objOffice.OfficeId = Office.OfficeId;
                objOffice.OfficeName = Office.OfficeName;
                objOffice.AdminUserId = Office.AdminUserId;
                objOffice.Street= Office.Street;
                objOffice.City = Office.City;
                objOffice.State = Office.State;
                objOffice.ZipCode= Office.ZipCode;
                objOffice.AdminEmail = Office.AdminEmail;
                objOffice.AdminQuickBloxId = Office.AdminQuickBloxId;

                objOffice.OldAdminUserId = Office.AdminUserId;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "EditOffice";
                log.Methodname = "GetOfficeDetailByOfficeId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objOffice;
            //}
        }


        public static OfficeModel GetOrganisationOfficeDetailByOfficeId(string id, string OrganisationId)
        {
            OfficeModel objOffice = new OfficeModel();
            try
            {
                OfficeServiceProxy CareGiverLiteService = new OfficeServiceProxy();
                Office Office = CareGiverLiteService.GetOrganisationOfficeDetailByOfficeId(id,OrganisationId).Result;

                objOffice.OfficeId = Office.OfficeId;
                objOffice.OfficeName = Office.OfficeName;
                objOffice.AdminUserId = Office.AdminUserId;
                objOffice.Street = Office.Street;
                objOffice.City = Office.City;
                objOffice.State = Office.State;
                objOffice.ZipCode = Office.ZipCode;
                objOffice.AdminEmail = Office.AdminEmail;
                objOffice.AdminQuickBloxId = Office.AdminQuickBloxId;

                objOffice.OldAdminUserId = Office.AdminUserId;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "EditOffice";
                log.Methodname = "GetOrganisationOfficeDetailByOfficeId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objOffice;
            //}
        }


        [HttpPost]
        public ActionResult EditOffice(OfficeModel objOfficeModel)
        {
            try
            {
              //  int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                var ChattingService = new ChattingServiceProxy();
                ChattingsList ChattingsList = new ChattingsList();

                OfficeServiceProxy OfficeService = new OfficeServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                var Office = new Office();
                Office.OfficeId = objOfficeModel.OfficeId;
                Office.OfficeName = objOfficeModel.OfficeName;
                Office.AdminUserId = objOfficeModel.AdminUserId;
                Office.Street = objOfficeModel.Street;
                Office.City = objOfficeModel.City;
                Office.State = objOfficeModel.State;
                Office.ZipCode = objOfficeModel.ZipCode;
                Office.Latitude = objOfficeModel.Latitude;
                Office.Longitude = objOfficeModel.Longitude;
                Office.TimezoneId = objOfficeModel.TimezoneId;
                Office.TimezoneOffset = Convert.ToInt32(objOfficeModel.TimezoneOffset);
                Office.TimezonePostfix = objOfficeModel.TimezonePostfix;
                Office.InsertUserId = InsertedUserID;
                Office.OrganisationId= Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail= Convert.ToString(Session["OrgSuperAdminEmail"]);

                string result = OfficeService.EditOffice(Office).Result;

                if (result == "Success")
                {
                    if (Office.OrganisationId > 0)
                    {
                        if (objOfficeModel.OldAdminUserId != objOfficeModel.AdminUserId)
                        {   
                            var ChattingController = new ChattingController();

                            var NewAdminQuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(objOfficeModel.AdminUserId).Result;

                            ChattingController.RemoveMemberFromOfficeGroup(objOfficeModel.OfficeId.ToString(), objOfficeModel.OldAdminUserId, objOfficeModel.AdminQuickBloxId, Office.OrganisationId, OrganisationEmail);

                            ChattingController.AddMemberIntoOfficeGroup(objOfficeModel.OfficeId.ToString(), objOfficeModel.AdminUserId, NewAdminQuickBloxId, Office.OrganisationId, OrganisationEmail);

                            // remove user from all old office group
                            List<Chatting> listGroupChatting = ChattingService.GetChatGroupListByOfficeIdForUser(objOfficeModel.OldAdminUserId, "2", objOfficeModel.OfficeId.ToString()).Result;
                            ChattingsList.objChattingsList = listGroupChatting;
                            foreach (var item in listGroupChatting)
                            {
                                ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objOfficeModel.OldAdminUserId, objOfficeModel.AdminQuickBloxId, Office.OrganisationId,OrganisationEmail);
                            }

                            AddAdminIntoPatientGroupChat(Office.OfficeId);
                        }
                    }
                    else
                    {
                        if (objOfficeModel.OldAdminUserId != objOfficeModel.AdminUserId)
                        {
                            var ChattingController = new ChattingController();

                            var NewAdminQuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(objOfficeModel.AdminUserId).Result;

                            ChattingController.RemoveMemberFromOfficeGroup(objOfficeModel.OfficeId.ToString(), objOfficeModel.OldAdminUserId, objOfficeModel.AdminQuickBloxId,Office.OrganisationId,OrganisationEmail);

                            ChattingController.AddMemberIntoOfficeGroup(objOfficeModel.OfficeId.ToString(), objOfficeModel.AdminUserId, NewAdminQuickBloxId,Office.OrganisationId,OrganisationEmail);

                            // remove user from all old office group
                            List<Chatting> listGroupChatting = ChattingService.GetChatGroupListByOfficeIdForUser(objOfficeModel.OldAdminUserId, "2", objOfficeModel.OfficeId.ToString()).Result;
                            ChattingsList.objChattingsList = listGroupChatting;
                            foreach (var item in listGroupChatting)
                            {
                                ChattingController.RemoveMemberFromAnyGroupChat(item.ChattingGroupId.ToString(), item.DialogId, item.GroupName, objOfficeModel.OldAdminUserId, objOfficeModel.AdminQuickBloxId,Office.OrganisationId,OrganisationEmail);
                            }

                            AddAdminIntoPatientGroupChat(Office.OfficeId);
                        }
                    }
                    //  AddAdminIntoOfficeGroup(Office.OfficeId);
                   // AddAdminIntoPatientGroupChat(Office.OfficeId);
                    TempData["Message"] = "Office is Updated successfully.";
                    return RedirectToAction("Office", "Office", new { IsAdded = true });
                }
                else
                {
                    return RedirectToAction("Office", "Office");
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OfficeController";
                log.Methodname = "[HttpPost] AddOffice";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }
        #endregion

        #region DeleteOffice
        public string DeleteOffice(string OfficeId)
        {
            string result = "";
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                
                OfficeServiceProxy OfficeService = new OfficeServiceProxy();
                result = OfficeService.DeleteOfficeByOfficeId(OfficeId, InsertedUserID, OrganisationId.ToString()).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "DeleteOffice";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        #endregion

        #region AssignOffice
        public ActionResult _AssignZipcodes(string id)
        {
            OfficeModel objModel = new OfficeModel();
            objModel.OfficeId = Convert.ToInt32(id);
            objModel.AssignedZipcodes = GetAssignZipcodes(Convert.ToInt32(id));
            return PartialView(objModel);
        }

        [HttpPost]
        public JsonResult AssignZipcodes(int OfficeId,string AssignedZipcodes)
        {
            string result = "";
            try
            {
                OfficeServiceProxy OfficeService = new OfficeServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                var Office = new Office();
                Office.OfficeId = OfficeId;
                Office.AssignedZipcodes = AssignedZipcodes;
                Office.InsertUserId = InsertedUserID;

                 result= OfficeService.AssignZipcodesToOfficeByOfficeId(Office).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OfficeController";
                log.Methodname = "[HttpPost] AddOffice";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string GetAssignZipcodes(int OfficeId)
        {
            string result = "";
            try
            {
                OfficeServiceProxy OfficeService = new OfficeServiceProxy();
                result = OfficeService.GetAssignZipcodesToOfficeByOfficeId(OfficeId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OfficeController";
                log.Methodname = "GetAssignZipcodes";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }
        #endregion

        #region GetOfficeDropDownList
        //Static Method (Call From Anywhere)
        public static OfficesList GetOfficeDropDownList()
        {
            var objOfficesList = new OfficesList();
            try
            {
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                objOfficesList = CaregiverLiteService.GetOfficeDropDownList(InsertedUserID);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "OfficeController";
                log.Methodname = "GetOfficeDropDownList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objOfficesList;
        }
        #endregion


        // for add new assign Admin into his office Patient Group Chat
        public string AddAdminIntoPatientGroupChat(int OfficeId)
        {

            string result = "";
            try
            {
                var ChattingService = new ChattingServiceProxy();

                var ChattingController = new ChattingController();
                MembershipUser user = Membership.GetUser();
                string[] roles = Roles.GetRolesForUser(user.UserName);
                string LogInUserId = user.ProviderUserKey.ToString();

                OfficeModel objModel = new OfficeModel();

                objModel = OfficeController.GetOfficeDetailByOfficeId(OfficeId.ToString());

                ChattingsList ChattingsList = new ChattingsList();
                List<Chatting> listChatting = ChattingService.GetPatientChattingGroupByOfficeId(OfficeId.ToString()).Result;
                ChattingsList.objChattingsList = listChatting;

                foreach (var item in listChatting)
                {
                    ChattingController.AddMemberIntoGroup(item.ChattingGroupId.ToString(), objModel.AdminUserId, objModel.AdminQuickBloxId);
               } 
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "AddCaregiverIntoGroup";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        public List<ScheduleInfo> GetALLOrganisationSuperadminList()
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetALLOrganisationSuperadminList");
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetALLOrganisationSuperadminList");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ScheduleInfo objSchedule = new ScheduleInfo();
                        objSchedule.SchedulerId = Convert.ToInt32(ds.Tables[0].Rows[i]["SchedulerId"]);
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
               // string result = InsertErrorLog(objErrorlog);
            }
            return SchedulerList;
        }


        public JsonResult ChkOfficeAvailability(string OfficeName)
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            int data = 0;
            // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ChkOfficeAvailability",OfficeName,OrganisationId);
            DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_ChkOfficeAvailability", OfficeName, OrganisationId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                data = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}