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
using DifferenzLibrary;
using System.Data;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin (This is Office Admin )
        public ActionResult Admin()
        {
            FillAllOffices();
            FillAllOrganisations();
            return View();
        }

        private void FillAllOffices(object SelectedValue = null)
        {
            try
            {
                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
                OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();
                var lstOffices = officeServiceProxy.GetAllOffices(logInUserId, OrganisationId.ToString()).Result;
                SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName", SelectedValue);
                ViewBag.lstOffice = officeSelectList;
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }


        public ActionResult AddAdmin()
        {
            return PartialView("AddAdmin");
        }


        [HttpPost]
        public ActionResult AddAdmin(AdminModel objAdmin)
        {
            try
            {

                AdminServiceProxy AdminService = new AdminServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                //if (ModelState.IsValid)
                //{


                if (objAdmin.AdminId == 0)
                {
                    MembershipCreateStatus status;
                    Membership.CreateUser(objAdmin.Username, objAdmin.Password, objAdmin.Email, null, null, true, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(objAdmin.Username, "Admin");
                    }
                }

                var Admin = new Admin();
                Admin.AdminId = objAdmin.AdminId;
                Admin.UserId = Membership.GetUser(objAdmin.Username).ProviderUserKey.ToString();
                Admin.FirstName = objAdmin.FirstName;
                Admin.LastName = objAdmin.LastName;
                Admin.Email = objAdmin.Email;
                Admin.InsertUserId = InsertedUserID;
                Admin.UserName = objAdmin.Username;
                Admin.Password = objAdmin.Password;
                Admin.OrganisationId  = Convert.ToInt32(Session["OrganisationId"]);

                string OrganisationEmail = Convert.ToString(Session["OrgSuperAdminEmail"]);

                string result = AdminService.AddAdmin(Admin).Result;

                if (result == "Success")
                {
                    //var SaveQBID = new ChattingController();
                    //SaveQBID.SaveQBIdOfAdmin(objAdmin.Email,objAdmin.QuickBloxId); 
                    
                    ForgetPasswordServiceProxy ForgetPasswordService = new ForgetPasswordServiceProxy();

                    string result1 = ForgetPasswordService.EmailtoSetPassword(objAdmin.Email).Result;

                    //ForgetPasswordService.ForgotPassword(objAdmin.Email).Result;

                    Task.Run(async () => { await ChattingController.GenerateUserQuickBloxIdRestAPI(Admin.UserId, Admin.Email, 0, false, Admin.OrganisationId, OrganisationEmail); }).Wait();


                    TempData["Message"] = "Office Manager is added successfully.";
                    return RedirectToAction("Admin", "Admin", new { IsAdded = true });
                }

                return RedirectToAction("Admin", "Admin");
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AdminController";
                log.Methodname = "[HttpPost] AddAdmin";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return RedirectToAction("Admin", "Admin");

            }
            return PartialView();
        }


        public ActionResult GetAdminList(JQueryDataTableParamModel param)
        {
            AdminsList AdminsList = new AdminsList();
            try
            {
                string IsActiveStatus = "";
                int FilterOfficeId = 0;

                if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                if (Request["FilterActiveStatus"] != null && Request["FilterActiveStatus"] != "")
                {
                    IsActiveStatus = Request["FilterActiveStatus"];
                }
                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 1)
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
                else
                {
                    sortOrder = "FirstName";
                }

                string search = "||";  //It's indicate blank filter

                if (!string.IsNullOrEmpty(param.sSearch))
                    search = param.sSearch;

                var sortDirection = Request["sSortDir_0"];   // asc or desc
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

                AdminServiceProxy AdminService = new AdminServiceProxy();
                AdminsList = AdminService.GetAllAdmins(pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId, OrganisationId, IsActiveStatus).Result;

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAdmin_Testing",
                                                      Convert.ToInt32(pageNo),
                                                      Convert.ToInt32(recordPerPage),
                                                      sortOrder,
                                                      sortDirection,
                                                      FilterOfficeId,
                                                      search,
                                                      Convert.ToInt32(OrganisationId),
                                                      IsActiveStatus);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AdminController";
                log.Methodname = "GetAdminList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (AdminsList.AdminList != null)
            {
                var result = from C in AdminsList.AdminList select new[] { C, C, C, C, C, C, C, C, C, C, C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = AdminsList.TotalNumberofRecord,
                    iTotalDisplayRecords = AdminsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = AdminsList.TotalNumberofRecord,
                    iTotalDisplayRecords = AdminsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditAdmin(string id)
        {
            AdminModel objModel = new AdminModel();
            try
            {

                //string username = "superadmin@paseva.com";
                //string password = "B3nM0sspwD";
                //MembershipUser mu = Membership.GetUser(username);
                //mu.ChangePassword(mu.ResetPassword(), password);


                objModel = GetAdminDetailById(id);

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AdminController";
                log.Methodname = "EditAdmin";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }


        public AdminModel GetAdminDetailById(string id)
        {
            AdminModel objAdmin = new AdminModel();
            try
            {
                AdminServiceProxy CareGiverLiteService = new AdminServiceProxy();
                Admin Admin = CareGiverLiteService.GetAdminDetailById(id).Result;
                objAdmin.AdminId = Admin.AdminId;
                objAdmin.FirstName = Admin.FirstName;
                objAdmin.LastName = Admin.LastName;
                objAdmin.Username = Admin.UserName;
                objAdmin.UserId = Admin.UserId;
                objAdmin.Email = Admin.Email;
                objAdmin.OldEmail = Admin.Email;
                objAdmin.Password = Admin.Password;
                objAdmin.IsActive = Admin.IsActive;
                objAdmin.QuickBloxId = Admin.QuickBloxId;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AdminController";
                log.Methodname = "GetAdminDetailById";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objAdmin;
        }


        [HttpPost]
        public ActionResult EditAdmin(AdminModel objAdminModel)
        {
            try
            {
                AdminServiceProxy AdminService = new AdminServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                //MembershipUser mUser = Membership.GetUser(objAdminModel.Username);
                //mUser.ChangePassword(mUser.ResetPassword(), objAdminModel.Password);

                var Admin = new Admin();
                Admin.AdminId = objAdminModel.AdminId;
                //  Scheduler.UserId = Membership.GetUser(objSchedulerModel.Username).ProviderUserKey.ToString();
                Admin.FirstName = objAdminModel.FirstName;
                Admin.LastName = objAdminModel.LastName;
                Admin.Email = objAdminModel.Email;
                //Admin.Password = objAdminModel.Password;
                Admin.InsertUserId = InsertedUserID;

                string result = AdminService.EditAdmin(Admin).Result;
                if (result == "Success")
                {
                    if (!string.Equals(objAdminModel.OldEmail, objAdminModel.Email, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(objAdminModel.OldEmail))
                    {
                        ChattingController chattingController = new ChattingController();

                        Task.Run(async () => { await chattingController.ApiToChangeEmailToQuickblox(objAdminModel.OldEmail, objAdminModel.Email, objAdminModel.QuickBloxId); }).Wait();
                      //  chattingController.ApiToChangeEmailToQuickblox(objAdminModel.OldEmail, objAdminModel.Email, objAdminModel.QuickBloxId);
                    }

                    TempData["Message"] = "Office Manager is updated successfully.";
                    return RedirectToAction("Admin", "Admin", new { IsAdded = true });
                }

                return RedirectToAction("Admin", "Admin");
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AdminController";
                log.Methodname = "[HttpPost] EditAdmin";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return PartialView();
        }


        public string DeleteAdmin(string AdminId, string UserName)
        {
            string result = "";
            try
            {
                MembershipUser mUser = Membership.GetUser(UserName);
                string AdminUserId = mUser.ProviderUserKey.ToString();
                Membership.DeleteUser(UserName, true);

                AdminServiceProxy CareGiverLiteService = new AdminServiceProxy();
                result = CareGiverLiteService.DeleteAdmin(AdminId, Membership.GetUser().ProviderUserKey.ToString()).Result;
    
                if (result == "Success")
                {
                    var QuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(AdminUserId).Result;

                    ChattingController ChattingController = new ChattingController();

                    // for delete user one to one chat dialog
                    ChattingController.DeleteOneToOneChatByUserId(AdminUserId);

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
                log.Methodname = "DeleteAdmin";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        
        }

        #region Superadmin can assign multiple chat groups to scheduler
        public ActionResult AssignGroup(string UserId, string Name, string QuickBloxId, string Email)
        {
            ViewBag.UserId = UserId;
            ViewBag.Name = Name;
            ViewBag.QuickBloxId = QuickBloxId;
            ViewBag.Email = Email;
            ViewBag.UnassignedGroupList = GetAllGroupExceptAssignedGroupByUserId(UserId);
            ViewBag.AssignedGroupList = GetAllAssignedGroupByUserId(UserId);
            return PartialView("AssignGroupToAdmin");
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

        public string AssignGroupToAdmin(string ChattingGroupIds, string UserId, string QuickBloxId)
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


               // string SchedulerEmail = ConfigurationManager.AppSettings["SuperAdminEmailId"].ToString();
                var ChattingGroupId = ChattingGroupIds.Split(',');

                foreach (var groupId in ChattingGroupId)
                {
                    result = ChattingService.AssignGroupToUser(groupId, UserId).Result;
                    if (!string.IsNullOrEmpty(result))
                    {
                        objDialogDetail = ChattingService.GetDialogDetail(groupId).Result;

                        Task.Run(async () => { await AddAdminToDialodOnQuickBlox(objDialogDetail.DialogId, objDialogDetail.GroupName, SchedulerEmail, QuickBloxId); }).Wait();
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
        private async Task<int> AddAdminToDialodOnQuickBlox(string DialogId, string GroupName, string SchedulerEmail, string QBId)
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
            //Add Member to group
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


            //if occupants_ids get match then Call to remove from group
            bool flag = false;
            for (int k = 0; k < tempOccupants_ids.Count; k++)
            {
                if (Convert.ToInt32(QBId) == tempOccupants_ids[k])
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
            }
            #endregion

            return 1;

        }
        #endregion




    }
}