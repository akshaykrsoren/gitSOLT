using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaregiverLite.Models;
using System.Web.Security;
using CaregiverLite.Models.Utility;
using System.Configuration;
using CaregiverLite.Action_Filters;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using CaregiverLiteWCF;
using DifferenzLibrary;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace CaregiverLite.Controllers
{
    public class AccountController : Controller
    {

        
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult SessionTimeout()
        {
            return View();
        }
        public ActionResult SessionTimeoutTemp()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


        //[HttpPost]
        //public ActionResult SetSessionForNotificationCount()
        //{
        //    Session["NotificationUdateRequired"] = false;
        //    return this.Json(new { success = true });
        //}


        // [HttpPost]
        public ActionResult SetSessionForNotificationCount(string key, string value)
        {
            Session["NotificationUdateRequired"] = false;

            return this.Json(new { success = true });
        }

        public ActionResult ChatNotificationCount(string key, string value)
        {
            Session["ChatNotificationCount"] = value;

            return this.Json(new { success = true });
        }

        public ActionResult GroupChatNotificationCount(string key, string value)
        {
            Session["GroupChatNotificationCount"] = value;

            return this.Json(new { success = true });
        }

        public ActionResult PatientChatNotificationCount(string key, string value)
        {
            Session["PatientChatNotificationCount"] = value;

            return this.Json(new { success = true });
        }


        private string CheckActiveLogin(string UserId)
        {
            string result = "";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetCheckAtiveLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    result = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            return result;

        }


        private int ActiveLoginStatus(string UserId)
        {
            int result=0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ActiveLoginStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    result = Convert.ToInt32(Convert.ToBoolean(cmd.ExecuteScalar()));
                }
            }
            return result;
        }


        //[HttpPost]
        //public ActionResult Login(LoginModel objloginmodel, string ReturnUrl)
        //{
        //    CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //Roles.CreateRole("OrgSuperAdmin");


        //            MembershipUser mUser = Membership.GetUser(objloginmodel.UserName);
        //           // mUser.ChangePassword(mUser.ResetPassword(), objloginmodel.Password);

        //            bool isValidUser = Membership.ValidateUser(objloginmodel.UserName, objloginmodel.Password);

        //            if (isValidUser)
        //            {
        //                MembershipUser user = Membership.GetUser(objloginmodel.UserName);

        //                //QuickBlox
        //                Session["FromQBId"] = null;
        //                Session["FromUserEmail"] = user.Email;
        //                var LoginUserQuickBloxId = "";
        //                Session["NotificationUdateRequired"] = true;

        //                Session["ChatNotificationCount"] = 0;
        //                Session["GroupChatNotificationCount"] = 0;
        //                Session["PatientChatNotificationCount"] = 0;

        //                @Session["LoginUserUserId"] = user.ProviderUserKey.ToString();

        //                LoginUserQuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(user.ProviderUserKey.ToString()).Result;
        //                Session["FromQBId"] = LoginUserQuickBloxId ?? "0";

        //                Session["IsAllowForPatientChatRoom"] = "True";

        //                Session["IsOfficePermission"] = "True";

        //                Session["IsAllowOneToOneChat"] = "True";
        //                Session["IsAllowGroupChat"] = "True";

        //                Session["IsAllowToCreateGroupChat"] = "True";

        //                Session["OfficeId"] = null;
        //                //ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
        //                //LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxIdBySchedulerId(user.ProviderUserKey.ToString()).Result;
        //                //if (LoginUserQuickBloxId.Length == 0)
        //                //{
        //                //    Session["FromQBId"] = '0';
        //                //}
        //                //else
        //                //{
        //                //    Session["FromQBId"] = LoginUserQuickBloxId;
        //                //}
        //                //QuickBlox


        //                string LogedInUserId = user.ProviderUserKey.ToString();
        //                var ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddNotificationSoundforUser", LogedInUserId);
        //                Session["NotificationSoundStatus"] = (ds.Tables[0].Rows[0][0]).ToString();



        //                string[] roles = Roles.GetRolesForUser(user.UserName);
        //                if (roles.Length != 0)
        //                {
        //                    foreach (string role in roles)
        //                    {
        //                       // string userid = user.ProviderUserKey.ToString();
        //                        //string TestCheckLogin = CheckActiveLogin(userid);

        //                        // || role == "CareGiver")
        //                        if (role == "Nurse")
        //                        {
        //                            //if (TestCheckLogin == "0")
        //                            //{
        //                            //    // FillAllOffices();

        //                             FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            Session["UserId"] = user.ProviderUserKey.ToString();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            CareGivers objCareGivers = CareGiverLiteService.GetAllCareGiverByUserId(user.ProviderUserKey.ToString()).Result;
        //                            Session["NurseId"] = objCareGivers.NurseId;
        //                            Session["Name"] = objCareGivers.Name;
        //                            Session["role"] = role;
        //                            Session["IsSuperAdmin"] = "false";
        //                            Session["IsNurseCoordinator"] = "false";
        //                            Session["IsApproved"] = objCareGivers.IsApprove;
        //                            Session["IsOfficePermission"] = objCareGivers.IsOfficePermission;
        //                            //   Session["IsAllowOneToOneChat"] = objCareGivers.IsAllowOneToOneChat;
        //                            Session["IsAllowOneToOneChat"] = "True";
        //                            Session["IsAllowForPatientChatRoom"] = objCareGivers.IsAllowPatientChatRoom;
        //                            Session["IsAllowGroupChat"] = objCareGivers.IsAllowGroupChat;
        //                            Session["IsAllowToCreateGroupChat"] = objCareGivers.IsAllowToCreateGroupChat;
        //                            Session["OfficeId"] = objCareGivers.OfficeId;

        //                            return RedirectToAction("OneToOneChat", "Chatting");

        //                            //}
        //                            //else
        //                            //{
        //                            //    TempData["LoginError"] = "true";
        //                            //    TempData["message_login"] = "Your Paseva account is locked due to Covid Symptoms. Please connect with your manager to unlock it.";
        //                            //    // return RedirectToAction("Login", "Account");
        //                            //}
        //                            //return RedirectToAction("Dashboard", "Dashboard");
        //                            //if (objCareGivers.IsOfficePermission)
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //else
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //if (!objCareGivers.IsOfficePermission)
        //                            //{
        //                            //    if (objCareGivers.IsAllowPatientChatRoom)
        //                            //    {
        //                            //        return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        return RedirectToAction("NoAuthorize", "Chatting");
        //                            //    }
        //                            //}
        //                            //else if (objCareGivers.IsAllowOneToOneChat)
        //                            //{
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //}
        //                            //else if (objCareGivers.IsAllowPatientChatRoom)
        //                            //{
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //}
        //                            //else if (objCareGivers.IsAllowGroupChat)
        //                            //{
        //                            //    return RedirectToAction("GroupChat", "Chatting");
        //                            //}
        //                            //else
        //                            //{
        //                            //    return RedirectToAction("NoAuthorize", "Chatting");
        //                            //}
        //                        }
        //                        if (role == "SuperAdmin" || role == "OrgSuperAdmin")
        //                        {
        //                         //   FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            SchedulerModel objSchedulerModel = new SchedulerModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            Session["IsSuperAdmin"] = "true";
        //                            Session["IsNurseCoordinator"] = "false";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            objSchedulerModel = GetSchedulerDetailByUserId(user.ProviderUserKey.ToString());
        //                            Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;
        //                            //Session["Name"] = user.UserName;
        //                            Session["role"] = role;

        //                            Session["OrganisationId"] = objSchedulerModel.OrganisationId;

        //                            Session["OfficeId"] = objSchedulerModel.OfficeIds;
        //                            //  AllOffice();

        //                            return RedirectToAction( "Index", "Chart");

        //                           // return RedirectToAction("Patients", "Patients");
        //                        }
        //                        if (role == "Scheduler")
        //                        {
        //                          //  FillAllOffices();


        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            SchedulerModel objSchedulerModel = new SchedulerModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            Session["IsSuperAdmin"] = "true";
        //                            Session["IsNurseCoordinator"] = "false";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            objSchedulerModel = GetSchedulerDetailByUserId(user.ProviderUserKey.ToString());
        //                            //Session["Name"] = user.UserName;
        //                            Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;
        //                            Session["role"] = role;

        //                            Session["OrganisationId"] = objSchedulerModel.OrganisationId;

        //                            Session["OfficeId"] = objSchedulerModel.OfficeIds;
        //                            return RedirectToAction("PatientRequest", "PatientRequest");
        //                        }
        //                        if (role == "NurseCoordinator")
        //                        {
        //                          //  FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            NurseCoordinatorModel objNurseCoordinatorModel = new NurseCoordinatorModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            Session["IsSuperAdmin"] = "false";
        //                            Session["IsNurseCoordinator"] = "true";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            objNurseCoordinatorModel = GetNurseCoordinatorDetailByUserId(user.ProviderUserKey.ToString());
        //                            //Session["Name"] = user.UserName;
        //                            Session["Name"] = objNurseCoordinatorModel.FirstName + " " + objNurseCoordinatorModel.LastName;
        //                            Session["role"] = role;
        //                            TempData["role"] = role;
        //                            Session["IsAllowForPatientChatRoom"] = objNurseCoordinatorModel.IsAllowForPatientChatRoom;
        //                            Session["IsOfficePermission"] = objNurseCoordinatorModel.IsOfficePermission;
        //                            // Session["IsAllowOneToOneChat"] = objNurseCoordinatorModel.IsAllowOneToOneChat;
        //                            Session["IsAllowOneToOneChat"] = "True";
        //                            Session["IsAllowGroupChat"] = objNurseCoordinatorModel.IsAllowGroupChat;

        //                            Session["IsAllowToCreateGroupChat"] = objNurseCoordinatorModel.IsAllowToCreateGroupChat;
        //                            Session["OfficeId"] = objNurseCoordinatorModel.OfficeId;

        //                            return RedirectToAction("OneToOneChat", "Chatting");

        //                            //return RedirectToAction("Chatting", "Chatting");

        //                            //if (objNurseCoordinatorModel.IsOfficePermission)
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //else
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //if (!objNurseCoordinatorModel.IsOfficePermission)
        //                            //{
        //                            //    if (objNurseCoordinatorModel.IsAllowForPatientChatRoom)
        //                            //    {
        //                            //        return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        return RedirectToAction("NoAuthorize", "Chatting");
        //                            //    }
        //                            //}
        //                            //else if (objNurseCoordinatorModel.IsAllowOneToOneChat)
        //                            //{
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //}
        //                            //else if (objNurseCoordinatorModel.IsAllowForPatientChatRoom)
        //                            //{
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //}
        //                            //else if (objNurseCoordinatorModel.IsAllowGroupChat)
        //                            //{
        //                            //    return RedirectToAction("GroupChat", "Chatting");
        //                            //}
        //                            //else
        //                            //{
        //                            //    return RedirectToAction("NoAuthorize", "Chatting");
        //                            //}

        //                        }
        //                        else if (role == "Admin")
        //                        {
        //                         //   FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            AdminModel objAdminModel = new AdminModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;

        //                            objAdminModel = GetAdminDetailByUserId(user.ProviderUserKey.ToString());
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            Session["Name"] = objAdminModel.FirstName + " " + objAdminModel.LastName;
        //                            Session["role"] = role;
        //                            Session["IsSuperAdmin"] = "true";
        //                            Session["IsNurseCoordinator"] = "false";

        //                            Session["OrganisationId"] = objAdminModel.OrganisationId;

        //                            Session["OfficeId"] = objAdminModel.OfficeIds;
        //                            Session["OfficeName"] = OfficeNameByLoginUserid(user.ProviderUserKey.ToString());

        //                            return RedirectToAction("Patients", "Patients");
        //                        }
        //                        else if (role == "MarketingSuperAdmin")
        //                        {
        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;

        //                            MarketerDetailsModel objAdmin = GetMarketingDetailByUserId(Membership.GetUser(objloginmodel.UserName).ProviderUserKey.ToString());

        //                            Session["Name"] = objAdmin.MarketersName;
        //                            Session["IsSuperAdmin"] = "false";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            // Session["AdminId"] = objAdmin.MarketersId.ToString();
        //                            Session["Email"] = objAdmin.Email.ToString();
        //                            // Status.QuickbloxId = objAdmin.QuickBloxId.ToString();
        //                            Session["IsAllowForPatientChatRoom"] = false;
        //                            Session["IsAllowOneToOneChat"] = false;
        //                            Session["IsAllowGroupChat"] = false;
        //                            Session["IsAllowToCreateGroupChat"] = false;
        //                            Session["OfficeIds"] = Convert.ToString(objAdmin.OfficeId);
        //                            Session["OfficeName"] = objAdmin.OfficeName;
        //                            Session["Role"] = role;
        //                            //Status.Result = "Success";

        //                            // return RedirectToAction("Patients", "Patients");
        //                            return RedirectToAction("Index", "Client");
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                TempData["LoginError"] = "true";
        //                TempData["message_login"] = CaregiverLite.Views.Resources.Account.LoginErrorMsg;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ErrorLog log = new ErrorLog();
        //            log.Errormessage = e.Message;
        //            log.StackTrace = e.StackTrace;
        //            log.Pagename = "AccountController";
        //            log.Methodname = "[HttpPost] Login";
        //            ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //            string res = ErrorLogService.InsertErrorLog(log).Result;
        //        }
        //    }
        //    return View();
        //}


        [HttpPost]
        public ActionResult Login(LoginModel objloginmodel, string ReturnUrl)
        {
            CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
            if (ModelState.IsValid)
            {
                try
                {
                    //Roles.CreateRole("OrgSuperAdmin");
                    //MembershipUser mUser = Membership.GetUser(objloginmodel.UserName);
                    //mUser.ChangePassword(mUser.ResetPassword(), objloginmodel.Password);

                    bool isValidUser = Membership.ValidateUser(objloginmodel.UserName, objloginmodel.Password);

                    if (isValidUser)
                    {
                        MembershipUser user = Membership.GetUser(objloginmodel.UserName);

                        //QuickBlox
                        Session["FromQBId"] = null;
                        Session["FromUserEmail"] = user.Email;
                        var LoginUserQuickBloxId = "";
                        Session["NotificationUdateRequired"] = true;

                        Session["ChatNotificationCount"] = 0;
                        Session["GroupChatNotificationCount"] = 0;
                        Session["PatientChatNotificationCount"] = 0;

                        Session["LoginUserUserId"] = user.ProviderUserKey.ToString();

                        LoginUserQuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(user.ProviderUserKey.ToString()).Result;
                        Session["FromQBId"] = LoginUserQuickBloxId ?? "0";

                        Session["IsAllowForPatientChatRoom"] = "True";

                        Session["IsOfficePermission"] = "True";

                        Session["IsAllowOneToOneChat"] = "True";
                        Session["IsAllowGroupChat"] = "True";

                        Session["IsAllowToCreateGroupChat"] = "True";

                        Session["OfficeId"] = null;

                        //ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
                        //LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxIdBySchedulerId(user.ProviderUserKey.ToString()).Result;
                        //if (LoginUserQuickBloxId.Length == 0)
                        //{
                        //    Session["FromQBId"] = '0';
                        //}
                        //else
                        //{
                        //    Session["FromQBId"] = LoginUserQuickBloxId;
                        //}
                        //QuickBlox

                        string LogedInUserId = user.ProviderUserKey.ToString();
                        var ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddNotificationSoundforUser", LogedInUserId);
                        Session["NotificationSoundStatus"] = (ds.Tables[0].Rows[0][0]).ToString();

                        string[] roles = Roles.GetRolesForUser(user.UserName);
                        if (roles.Length != 0)
                        {
                            foreach (string role in roles)
                            {
                                string userid = user.ProviderUserKey.ToString();

                                //string TestCheckLogin = CheckActiveLogin(userid);

                                int testCheckLoginData = ActiveLoginStatus(userid);

                             if (testCheckLoginData == 1)
                              {

                                // || role == "CareGiver")
                                if (role == "Nurse")
                                {

                                    FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
                                    Session["UserId"] = user.ProviderUserKey.ToString();

                                    LogsServiceProxy logService = new LogsServiceProxy();
                                    Logs objLogs = new Logs();
                                    objLogs.PageName = "Login";
                                    objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
                                    objLogs.UserID = user.ProviderUserKey.ToString();
                                    string LoginResult = logService.InsertLog(objLogs).Result;
                                    CareGivers objCareGivers = CareGiverLiteService.GetAllCareGiverByUserId(user.ProviderUserKey.ToString()).Result;
                                    Session["NurseId"] = objCareGivers.NurseId;
                                    Session["Name"] = objCareGivers.Name;
                                    Session["role"] = role;
                                    Session["Email"] = objCareGivers.Email;
                                    Session["Phone"] = objCareGivers.Phone;
                                    Session["Address"] = objCareGivers.Address;
                                    Session["ZipCode"] = objCareGivers.ZipCode;
                                    Session["ProfileImage"] = objCareGivers.ProfileImage;

                                    Session["ServiceType"] = objCareGivers.ServiceName;
                                    Session["IsSuperAdmin"] = "false";
                                    Session["IsNurseCoordinator"] = "false";
                                    Session["IsApproved"] = objCareGivers.IsApprove;
                                    Session["IsOfficePermission"] = objCareGivers.IsOfficePermission;
                                    // Session["IsAllowOneToOneChat"] = objCareGivers.IsAllowOneToOneChat;
                                    Session["IsAllowOneToOneChat"] = "True";
                                    Session["IsAllowForPatientChatRoom"] = objCareGivers.IsAllowPatientChatRoom;
                                    Session["IsAllowGroupChat"] = objCareGivers.IsAllowGroupChat;
                                    Session["IsAllowToCreateGroupChat"] = objCareGivers.IsAllowToCreateGroupChat;
                                    Session["OfficeId"] = objCareGivers.OfficeId;

                                    Session["OrganisationId"] = objCareGivers.OrganisationId;
                                        // return RedirectToAction("OneToOneChat", "Chatting");
                                    Session["ProfileImage"] = objCareGivers.ProfileImage;
                                        //"http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/CaregiverLiteMobileService/Images/CareGiverProfileImages/CareGiver_5ff220e9-80be-47d9-a656-d1edeba0cdb91597124494.jpeg";//objCareGivers.ProfileImage;

                                        /*  Code Added for sent mail to the nurse if she taken more than the allowed time  */
                                        //DataTable dt =  GetAccessHoursTakenByNurse(objCareGivers.NurseId);
                                        //if(dt.Rows[0]["Name"].ToString() !="" && dt.Rows[0]["Email"].ToString() != "")
                                        //{
                                        //    var OrganisationName = "Paseva.com";
                                        //    string subject = "You have exceeded your allowed access time";
                                        //    string EmailBody = string.Format(
                                        //         "Dear {0},\n\nYou have exceeded your allowed working hours for this week.\nAllowed: {1} hrs\nUsed: {2} hrs\n\nPlease ensure your hours stay within the limit.\n\nThank you,\n{3}",
                                        //         dt.Rows[0]["Name"].ToString(),
                                        //         dt.Rows[0]["AllowedHours"].ToString(),
                                        //         dt.Rows[0]["TotalHours"].ToString(),
                                        //         OrganisationName
                                        //         );
                                        //    sendEmail(dt.Rows[0]["Email"].ToString(),subject, EmailBody,false,"","");
                                        //}

                                        /* END  */
                                        return RedirectToAction("LoginDetails", "Account");


                                    //  return RedirectToAction("OneToOneChat", "Chatting");

                                    //}
                                    //else
                                    //{
                                    //    TempData["LoginError"] = "true";
                                    //    TempData["message_login"] = "Your Paseva account is locked due to Covid Symptoms. Please connect with your manager to unlock it.";
                                    //    // return RedirectToAction("Login", "Account");
                                    //}
                                    //return RedirectToAction("Dashboard", "Dashboard");
                                    //if (objCareGivers.IsOfficePermission)
                                    //    return RedirectToAction("Chatting", "Chatting");
                                    //else
                                    //    return RedirectToAction("PatientGroupChatting", "Chatting");
                                    //if (!objCareGivers.IsOfficePermission)
                                    //{
                                    //    if (objCareGivers.IsAllowPatientChatRoom)
                                    //    {
                                    //        return RedirectToAction("PatientGroupChatting", "Chatting");
                                    //    }
                                    //    else
                                    //    {
                                    //        return RedirectToAction("NoAuthorize", "Chatting");
                                    //    }
                                    //}
                                    //else if (objCareGivers.IsAllowOneToOneChat)
                                    //{
                                    //    return RedirectToAction("Chatting", "Chatting");
                                    //}
                                    //else if (objCareGivers.IsAllowPatientChatRoom)
                                    //{
                                    //    return RedirectToAction("PatientGroupChatting", "Chatting");
                                    //}
                                    //else if (objCareGivers.IsAllowGroupChat)
                                    //{
                                    //    return RedirectToAction("GroupChat", "Chatting");
                                    //}
                                    //else
                                    //{
                                    //    return RedirectToAction("NoAuthorize", "Chatting");
                                    //}


                                }
                                if (role == "SuperAdmin" || role == "OrgSuperAdmin")
                                {
                                    // FillAllOffices();

                                    FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
                                    SchedulerModel objSchedulerModel = new SchedulerModel();

                                    LogsServiceProxy logService = new LogsServiceProxy();
                                    Logs objLogs = new Logs();
                                    objLogs.PageName = "Login";
                                    objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
                                    objLogs.UserID = user.ProviderUserKey.ToString();
                                    string LoginResult = logService.InsertLog(objLogs).Result;
                                    Session["IsSuperAdmin"] = "true";
                                    Session["IsNurseCoordinator"] = "false";
                                    Session["UserId"] = user.ProviderUserKey.ToString();
                                    Session["role"] = role;

                                    objSchedulerModel = GetSchedulerDetailByUserId(user.ProviderUserKey.ToString());
                                    if (objSchedulerModel.SchedulerId == 0)
                                    {
                                        AdminModel objAdminModel = new AdminModel();
                                        objAdminModel = GetAdminDetailByUserId(user.ProviderUserKey.ToString());
                                        //Session["UserId"] = user.ProviderUserKey.ToString();
                                        Session["Name"] = objAdminModel.FirstName + " " + objAdminModel.LastName;
                                        //Session["role"] = role;
                                        //Session["IsSuperAdmin"] = "true";
                                        //Session["IsNurseCoordinator"] = "false";

                                        Session["OrganisationId"] = objAdminModel.OrganisationId;

                                        if (objAdminModel.OrganisationId > 0)
                                        {
                                            List<string> OrganisationAdminDetails = GetOrganisationDetailForQbChat(objAdminModel.OrganisationId);

                                            Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
                                            Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
                                            Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
                                            Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
                                        }

                                        Session["OfficeId"] = objAdminModel.OfficeIds;
                                        Session["OfficeName"] = OfficeNameByLoginUserid(user.ProviderUserKey.ToString());

                                        //  return RedirectToAction("Patients", "Patients");
                                        return RedirectToAction("LoginDetails", "Account");
                                    }

                                    Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;

                                    //Session["Name"] = user.UserName;
                                    Session["role"] = role;
                                    Session["OrganisationId"] = objSchedulerModel.OrganisationId;

                                    if (objSchedulerModel.OrganisationId > 0)
                                    {
                                        List<string> OrganisationAdminDetails = GetOrganisationDetailForQbChat(objSchedulerModel.OrganisationId);

                                        Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
                                        Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
                                        Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
                                        Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
                                    }


                                    Session["OfficeId"] = objSchedulerModel.OfficeIds;

                                    //  AllOffice();
                                    // return RedirectToAction("Index", "Chart");

                                    return RedirectToAction("LoginDetails", "Account");

                                    // return RedirectToAction("Patients", "Patients");
                                }
                                if (role == "Scheduler")
                                {
                                    //  FillAllOffices();

                                    FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
                                    SchedulerModel objSchedulerModel = new SchedulerModel();

                                    LogsServiceProxy logService = new LogsServiceProxy();
                                    Logs objLogs = new Logs();
                                    objLogs.PageName = "Login";
                                    objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
                                    objLogs.UserID = user.ProviderUserKey.ToString();
                                    string LoginResult = logService.InsertLog(objLogs).Result;
                                    Session["IsSuperAdmin"] = "true";
                                    Session["IsNurseCoordinator"] = "false";
                                    Session["UserId"] = user.ProviderUserKey.ToString();
                                    objSchedulerModel = GetSchedulerDetailByUserId(user.ProviderUserKey.ToString());
                                    //Session["Name"] = user.UserName;
                                    Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;
                                    Session["role"] = role;
                                    Session["Email"] = objSchedulerModel.Email;

                                    Session["OrganisationId"] = objSchedulerModel.OrganisationId;

                                    if (objSchedulerModel.OrganisationId > 0)
                                    {
                                        List<string> OrganisationAdminDetails = GetOrganisationDetailForQbChat(objSchedulerModel.OrganisationId);

                                        Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
                                        Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
                                        Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
                                        Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
                                    }

                                    Session["OfficeId"] = objSchedulerModel.OfficeIds;

                                    //  return RedirectToAction("PatientRequest", "PatientRequest");
                                    return RedirectToAction("LoginDetails", "Account");
                                }
                                if (role == "NurseCoordinator")
                                {
                                    //  FillAllOffices();

                                    FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
                                    NurseCoordinatorModel objNurseCoordinatorModel = new NurseCoordinatorModel();

                                    LogsServiceProxy logService = new LogsServiceProxy();
                                    Logs objLogs = new Logs();
                                    objLogs.PageName = "Login";
                                    objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
                                    objLogs.UserID = user.ProviderUserKey.ToString();
                                    string LoginResult = logService.InsertLog(objLogs).Result;
                                    Session["IsSuperAdmin"] = "false";
                                    Session["IsNurseCoordinator"] = "true";
                                    Session["UserId"] = user.ProviderUserKey.ToString();
                                    objNurseCoordinatorModel = GetNurseCoordinatorDetailByUserId(user.ProviderUserKey.ToString());

                                        //Session["Name"] = user.UserName;

                                        Session["OrganisationId"] = objNurseCoordinatorModel.OrganisationId;

                                    Session["Name"] = objNurseCoordinatorModel.FirstName + " " + objNurseCoordinatorModel.LastName;
                                    Session["role"] = role;
                                    TempData["role"] = role;
                                    Session["IsAllowForPatientChatRoom"] = objNurseCoordinatorModel.IsAllowForPatientChatRoom;
                                    Session["IsOfficePermission"] = objNurseCoordinatorModel.IsOfficePermission;

                                    // Session["IsAllowOneToOneChat"] = objNurseCoordinatorModel.IsAllowOneToOneChat;
                                    Session["IsAllowOneToOneChat"] = "True";
                                    Session["IsAllowGroupChat"] = objNurseCoordinatorModel.IsAllowGroupChat;

                                    Session["IsAllowToCreateGroupChat"] = objNurseCoordinatorModel.IsAllowToCreateGroupChat;
                                    Session["OfficeId"] = objNurseCoordinatorModel.OfficeId;

                                    return RedirectToAction("OneToOneChat", "Chatting");

                                        #region
                                        //   return RedirectToAction("LoginDetails", "Account");

                                        //return RedirectToAction("Chatting", "Chatting");

                                        //if (objNurseCoordinatorModel.IsOfficePermission)
                                        //    return RedirectToAction("Chatting", "Chatting");
                                        //else
                                        //    return RedirectToAction("PatientGroupChatting", "Chatting");
                                        //if (!objNurseCoordinatorModel.IsOfficePermission)
                                        //{
                                        //    if (objNurseCoordinatorModel.IsAllowForPatientChatRoom)
                                        //    {
                                        //        return RedirectToAction("PatientGroupChatting", "Chatting");
                                        //    }
                                        //    else
                                        //    {
                                        //        return RedirectToAction("NoAuthorize", "Chatting");
                                        //    }
                                        //}
                                        //else if (objNurseCoordinatorModel.IsAllowOneToOneChat)
                                        //{
                                        //    return RedirectToAction("Chatting", "Chatting");
                                        //}
                                        //else if (objNurseCoordinatorModel.IsAllowForPatientChatRoom)
                                        //{
                                        //    return RedirectToAction("PatientGroupChatting", "Chatting");
                                        //}
                                        //else if (objNurseCoordinatorModel.IsAllowGroupChat)
                                        //{
                                        //    return RedirectToAction("GroupChat", "Chatting");
                                        //}
                                        //else
                                        //{
                                        //    return RedirectToAction("NoAuthorize", "Chatting");
                                        //}


                                        #endregion

                                    }
                                    else if (role == "Admin")
                                   { 
                                    //   FillAllOffices();

                                    FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
                                    AdminModel objAdminModel = new AdminModel();

                                    LogsServiceProxy logService = new LogsServiceProxy();
                                    Logs objLogs = new Logs();
                                    objLogs.PageName = "Login";
                                    objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
                                    objLogs.UserID = user.ProviderUserKey.ToString();
                                    string LoginResult = logService.InsertLog(objLogs).Result;

                                    objAdminModel = GetAdminDetailByUserId(user.ProviderUserKey.ToString());
                                    Session["UserId"] = user.ProviderUserKey.ToString();
                                    Session["Name"] = objAdminModel.FirstName + " " + objAdminModel.LastName;
                                    Session["role"] = role;
                                    Session["IsSuperAdmin"] = "true";
                                    Session["IsNurseCoordinator"] = "false";

                                    Session["OrganisationId"] = objAdminModel.OrganisationId;

                                    if (objAdminModel.OrganisationId > 0)
                                    {
                                        List<string> OrganisationAdminDetails = GetOrganisationDetailForQbChat(objAdminModel.OrganisationId);

                                        Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
                                        Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
                                        Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
                                        Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
                                    }


                                    Session["OfficeId"] = objAdminModel.OfficeIds;
                                    Session["OfficeName"] = OfficeNameByLoginUserid(user.ProviderUserKey.ToString());

                                    //  return RedirectToAction("Patients", "Patients");

                                    return RedirectToAction("LoginDetails", "Account");
                                }
                                else if (role == "MarketingSuperAdmin")
                                {
                                    FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
                                    LogsServiceProxy logService = new LogsServiceProxy();
                                    Logs objLogs = new Logs();
                                    objLogs.PageName = "Login";
                                    objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
                                    objLogs.UserID = user.ProviderUserKey.ToString();
                                    string LoginResult = logService.InsertLog(objLogs).Result;

                                    MarketerDetailsModel objAdmin = GetMarketingDetailByUserId(Membership.GetUser(objloginmodel.UserName).ProviderUserKey.ToString());

                                    Session["Name"] = objAdmin.MarketersName;
                                    Session["IsSuperAdmin"] = "false";
                                    Session["UserId"] = user.ProviderUserKey.ToString();
                                    // Session["AdminId"] = objAdmin.MarketersId.ToString();
                                    Session["Email"] = objAdmin.Email.ToString();
                                    // Status.QuickbloxId = objAdmin.QuickBloxId.ToString();

                                    Session["IsAllowForPatientChatRoom"] = false;
                                    Session["IsAllowOneToOneChat"] = false;
                                    Session["IsAllowGroupChat"] = false;
                                    Session["IsAllowToCreateGroupChat"] = false;
                                    Session["OfficeIds"] = Convert.ToString(objAdmin.OfficeId);
                                    Session["OfficeName"] = objAdmin.OfficeName;
                                    Session["Role"] = role;

                                    //Status.Result = "Success";

                                    //  return RedirectToAction("Patients", "Patients");
                                    return RedirectToAction("Index", "Client");

                                }

                            }
                            else
                            {
                                TempData["LoginError"] = "true";
                                TempData["message_login"] ="Your Account is Suspended";

                            }
                        }

                        }          
                    }
                    else
                    {
                        TempData["LoginError"] = "true";
                        TempData["message_login"] = CaregiverLite.Views.Resources.Account.LoginErrorMsg;
                    }
                }
                catch (Exception e)
                {
                    SqlConnection.ClearAllPools();
                    ErrorLog log = new ErrorLog();
                    log.Errormessage = e.Message;
                    log.StackTrace = e.StackTrace;
                    log.Pagename = "AccountController";
                    log.Methodname = "[HttpPost] Login";

                    ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                    string res = ErrorLogService.InsertErrorLog(log).Result;
                }
            }
            return View();
        }

        private string GetOrganisationDetailById(int OrganisationId)
        {
            string ORGname = "";
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOrganisationDetailById",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOrganisationDetailById",
                                                        OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ORGname = ds.Tables[0].Rows[0]["OrganisationName"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientDetailsList";
                //  string result = InsertErrorLog(objErrorlog);
            }
            return ORGname;
        }


        private List<string> GetOrganisationDetailForQbChat(int OrganisationId)
        {
            List<string> OrganisationDetailsList = new List<string>();

            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOrganisationDetailById",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOrganisationDetailForQbChat",
                                                        OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminName"].ToString());
                    OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminQBId"].ToString());
                    OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminEmail"].ToString());
                    OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminUserId"].ToString());

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientDetailsList";
                //  string result = InsertErrorLog(objErrorlog);
            }

            return OrganisationDetailsList;
        }

        [HttpGet]
        public ActionResult LoginDetails()
        {
            try
            {
                // insertdata("hello");
                int orgid = Convert.ToInt32(Session["OrganisationId"].ToString());

                ViewBag.Organtisation = (orgid > 0) ? GetOrganisationDetailById(orgid) : "Paseva";

                // insertdata(ViewBag.Organtisation);

                ViewBag.ImageUrl = (orgid > 0) ? "http://pasevauat.com/Content/image/Hospital.png" : "http://pasevauat.com/Content/image/Paseva_CompleteLogo.png";

                // insertdata(ViewBag.ImageUrl);

                if (Session["Role"].ToString() == "Nurse")
                {
                    ViewBag.RoleName = Session["ServiceType"].ToString();
                }
                else
                {
                    ViewBag.RoleName = Session["Role"].ToString();
                }

                ViewBag.Name = Session["Name"].ToString();
                ViewBag.Email = Session["FromUserEmail"].ToString();
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [HttpPost]
        public ActionResult LoginDetails(FormCollection fs)
        {
            try
            {
                string role = Session["role"].ToString();
                if (role == "Nurse")
                {
                    return RedirectToAction("OneToOneChat", "Chatting");
                }
                else if (role == "SuperAdmin" || role == "OrgSuperAdmin")
                {
                    return RedirectToAction("Index", "Chart");
                }
                else if (role == "Scheduler")
                {
                    return RedirectToAction("PatientRequest", "PatientRequest");
                }
                else if (role == "NurseCoordinator")
                {
                    return RedirectToAction("OneToOneChat", "Chatting");
                }
                else if (role == "Admin")
                {
                    return RedirectToAction("Patients", "Patients");
                }
                else
                {
                    TempData["LoginError"] = "true";
                    TempData["message_login"] = CaregiverLite.Views.Resources.Account.LoginErrorMsg;
                }

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AccountController";
                log.Methodname = "[HttpPost] Login";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return RedirectToAction("Login", "Account");
        }


        #region
        //[HttpPost]
        //public ActionResult Login(LoginModel objloginmodel, string ReturnUrl)
        //{
        //    CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //          //Roles.CreateRole("OrgSuperAdmin");
        //          //  MembershipUser mUser = Membership.GetUser(objloginmodel.UserName);
        //          //  mUser.ChangePassword(mUser.ResetPassword(), objloginmodel.Password);

        //            bool isValidUser = Membership.ValidateUser(objloginmodel.UserName, objloginmodel.Password);

        //            if (isValidUser)
        //            {
        //                MembershipUser user = Membership.GetUser(objloginmodel.UserName);

        //                //QuickBlox
        //                Session["FromQBId"] = null;
        //                Session["FromUserEmail"] = user.Email;
        //                var LoginUserQuickBloxId = "";
        //                Session["NotificationUdateRequired"] = true;

        //                Session["ChatNotificationCount"] = 0;
        //                Session["GroupChatNotificationCount"] = 0;
        //                Session["PatientChatNotificationCount"] = 0;

        //                @Session["LoginUserUserId"] = user.ProviderUserKey.ToString();

        //                LoginUserQuickBloxId = new ChattingServiceProxy().GetQuickBloxIdBySchedulerUserId(user.ProviderUserKey.ToString()).Result;
        //                Session["FromQBId"] = LoginUserQuickBloxId ?? "0";

        //                Session["IsAllowForPatientChatRoom"] = "True";

        //                Session["IsOfficePermission"] = "True";

        //                Session["IsAllowOneToOneChat"] = "True";
        //                Session["IsAllowGroupChat"] = "True";

        //                Session["IsAllowToCreateGroupChat"] = "True";

        //                Session["OfficeId"] = null;

        //                //ChattingServiceProxy ChattingLiteService = new ChattingServiceProxy();
        //                //LoginUserQuickBloxId = ChattingLiteService.GetQuickBloxIdBySchedulerId(user.ProviderUserKey.ToString()).Result;
        //                //if (LoginUserQuickBloxId.Length == 0)
        //                //{
        //                //    Session["FromQBId"] = '0';
        //                //}
        //                //else
        //                //{
        //                //    Session["FromQBId"] = LoginUserQuickBloxId;
        //                //}
        //                //QuickBlox

        //                string LogedInUserId = user.ProviderUserKey.ToString();
        //                var ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddNotificationSoundforUser", LogedInUserId);
        //                Session["NotificationSoundStatus"] = (ds.Tables[0].Rows[0][0]).ToString();

        //                string[] roles = Roles.GetRolesForUser(user.UserName);
        //                if (roles.Length != 0)
        //                {
        //                    foreach (string role in roles)
        //                    {
        //                        //string userid = user.ProviderUserKey.ToString();
        //                        //string TestCheckLogin = CheckActiveLogin(userid);

        //                        // || role == "CareGiver")
        //                        if (role == "Nurse")
        //                        {
        //                            //if (TestCheckLogin == "0")
        //                            //{
        //                            //    // FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            Session["UserId"] = user.ProviderUserKey.ToString();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            CareGivers objCareGivers = CareGiverLiteService.GetAllCareGiverByUserId(user.ProviderUserKey.ToString()).Result;
        //                            Session["NurseId"] = objCareGivers.NurseId;
        //                            Session["Name"] = objCareGivers.Name;
        //                            Session["role"] = role;
        //                            Session["IsSuperAdmin"] = "false";
        //                            Session["IsNurseCoordinator"] = "false";
        //                            Session["IsApproved"] = objCareGivers.IsApprove;
        //                            Session["IsOfficePermission"] = objCareGivers.IsOfficePermission;
        //                            // Session["IsAllowOneToOneChat"] = objCareGivers.IsAllowOneToOneChat;
        //                            Session["IsAllowOneToOneChat"] = "True";
        //                            Session["IsAllowForPatientChatRoom"] = objCareGivers.IsAllowPatientChatRoom;
        //                            Session["IsAllowGroupChat"] = objCareGivers.IsAllowGroupChat;
        //                            Session["IsAllowToCreateGroupChat"] = objCareGivers.IsAllowToCreateGroupChat;
        //                            Session["OfficeId"] = objCareGivers.OfficeId;

        //                            Session["OrganisationId"] = objCareGivers.OrganisationId;

        //                           // return RedirectToAction("OneToOneChat", "Chatting");

        //                              return RedirectToAction("LoginDetails", "Account");

        //                            //  return RedirectToAction("OneToOneChat", "Chatting");


        //                            //}
        //                            //else
        //                            //{
        //                            //    TempData["LoginError"] = "true";
        //                            //    TempData["message_login"] = "Your Paseva account is locked due to Covid Symptoms. Please connect with your manager to unlock it.";
        //                            //    // return RedirectToAction("Login", "Account");
        //                            //}
        //                            //return RedirectToAction("Dashboard", "Dashboard");
        //                            //if (objCareGivers.IsOfficePermission)
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //else
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //if (!objCareGivers.IsOfficePermission)
        //                            //{
        //                            //    if (objCareGivers.IsAllowPatientChatRoom)
        //                            //    {
        //                            //        return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        return RedirectToAction("NoAuthorize", "Chatting");
        //                            //    }
        //                            //}
        //                            //else if (objCareGivers.IsAllowOneToOneChat)
        //                            //{
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //}
        //                            //else if (objCareGivers.IsAllowPatientChatRoom)
        //                            //{
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //}
        //                            //else if (objCareGivers.IsAllowGroupChat)
        //                            //{
        //                            //    return RedirectToAction("GroupChat", "Chatting");
        //                            //}
        //                            //else
        //                            //{
        //                            //    return RedirectToAction("NoAuthorize", "Chatting");
        //                            //}
        //                        }
        //                        if (role == "SuperAdmin" || role == "OrgSuperAdmin")
        //                        {
        //                            // FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            SchedulerModel objSchedulerModel = new SchedulerModel();


        //                            AdminModel objAdminModel = new AdminModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            Session["IsSuperAdmin"] = "true";
        //                            Session["IsNurseCoordinator"] = "false";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();



        //                            objSchedulerModel = GetSchedulerDetailByUserId(user.ProviderUserKey.ToString());
        //                            if(objSchedulerModel.SchedulerId== 0)
        //                            {
        //                                objAdminModel = GetAdminDetailByUserId(user.ProviderUserKey.ToString());
        //                                Session["UserId"] = user.ProviderUserKey.ToString();
        //                                Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;
        //                                Session["role"] = role;
        //                                Session["IsSuperAdmin"] = "true";
        //                                Session["IsNurseCoordinator"] = "false";

        //                                Session["OrganisationId"] = objAdminModel.OrganisationId;

        //                                if (objAdminModel.OrganisationId > 0)
        //                                {
        //                                    List<string> OrganisationAdminDetails= GetOrganisationDetailForQbChat(objAdminModel.OrganisationId);

        //                                    Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
        //                                    Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
        //                                    Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
        //                                    Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
        //                                }

        //                                Session["OfficeId"] = objAdminModel.OfficeIds;
        //                                Session["OfficeName"] = OfficeNameByLoginUserid(user.ProviderUserKey.ToString());

        //                                //  return RedirectToAction("Patients", "Patients");
        //                                return RedirectToAction("LoginDetails", "Account");
        //                            }

        //                            Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;

        //                            //Session["Name"] = user.UserName;
        //                            Session["role"] = role;

        //                            Session["OrganisationId"] = objSchedulerModel.OrganisationId;


        //                            if (objSchedulerModel.OrganisationId > 0)
        //                            {
        //                                List<string> OrganisationAdminDetails = GetOrganisationDetailForQbChat(objSchedulerModel.OrganisationId);

        //                                Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
        //                                Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
        //                                Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
        //                                Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
        //                            }

        //                            Session["OfficeId"] = objSchedulerModel.OfficeIds;



        //                            //  AllOffice();

        //                           // return RedirectToAction("Index", "Chart");

        //                             return RedirectToAction("LoginDetails", "Account");

        //                            // return RedirectToAction("Patients", "Patients");
        //                        }
        //                        if (role == "Scheduler")
        //                        {
        //                            //  FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            SchedulerModel objSchedulerModel = new SchedulerModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            Session["IsSuperAdmin"] = "true";
        //                            Session["IsNurseCoordinator"] = "false";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            objSchedulerModel = GetSchedulerDetailByUserId(user.ProviderUserKey.ToString());
        //                            //Session["Name"] = user.UserName;
        //                            Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;
        //                            Session["role"] = role;

        //                            Session["OrganisationId"] = objSchedulerModel.OrganisationId;

        //                            if (objSchedulerModel.OrganisationId > 0)
        //                            {
        //                                List<string> OrganisationAdminDetails = GetOrganisationDetailForQbChat(objSchedulerModel.OrganisationId);

        //                                Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
        //                                Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
        //                                Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
        //                                Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
        //                            }

        //                            Session["OfficeId"] = objSchedulerModel.OfficeIds;

        //                          //  return RedirectToAction("PatientRequest", "PatientRequest");
        //                              return RedirectToAction("LoginDetails", "Account");

        //                        }
        //                        if (role == "NurseCoordinator")
        //                        {
        //                            //  FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            NurseCoordinatorModel objNurseCoordinatorModel = new NurseCoordinatorModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;
        //                            Session["IsSuperAdmin"] = "false";
        //                            Session["IsNurseCoordinator"] = "true";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            objNurseCoordinatorModel = GetNurseCoordinatorDetailByUserId(user.ProviderUserKey.ToString());
        //                            //Session["Name"] = user.UserName;

        //                            Session["Name"] = objNurseCoordinatorModel.FirstName + " " + objNurseCoordinatorModel.LastName;
        //                            Session["role"] = role;
        //                            TempData["role"] = role;
        //                            Session["IsAllowForPatientChatRoom"] = objNurseCoordinatorModel.IsAllowForPatientChatRoom;
        //                            Session["IsOfficePermission"] = objNurseCoordinatorModel.IsOfficePermission;

        //                            // Session["IsAllowOneToOneChat"] = objNurseCoordinatorModel.IsAllowOneToOneChat;
        //                            Session["IsAllowOneToOneChat"] = "True";
        //                            Session["IsAllowGroupChat"] = objNurseCoordinatorModel.IsAllowGroupChat;

        //                            Session["IsAllowToCreateGroupChat"] = objNurseCoordinatorModel.IsAllowToCreateGroupChat;
        //                            Session["OfficeId"] = objNurseCoordinatorModel.OfficeId;

        //                          //  return RedirectToAction("OneToOneChat", "Chatting");
        //                             return RedirectToAction("LoginDetails", "Account");

        //                            //return RedirectToAction("Chatting", "Chatting");

        //                            //if (objNurseCoordinatorModel.IsOfficePermission)
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //else
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //if (!objNurseCoordinatorModel.IsOfficePermission)
        //                            //{
        //                            //    if (objNurseCoordinatorModel.IsAllowForPatientChatRoom)
        //                            //    {
        //                            //        return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        return RedirectToAction("NoAuthorize", "Chatting");
        //                            //    }
        //                            //}
        //                            //else if (objNurseCoordinatorModel.IsAllowOneToOneChat)
        //                            //{
        //                            //    return RedirectToAction("Chatting", "Chatting");
        //                            //}
        //                            //else if (objNurseCoordinatorModel.IsAllowForPatientChatRoom)
        //                            //{
        //                            //    return RedirectToAction("PatientGroupChatting", "Chatting");
        //                            //}
        //                            //else if (objNurseCoordinatorModel.IsAllowGroupChat)
        //                            //{
        //                            //    return RedirectToAction("GroupChat", "Chatting");
        //                            //}
        //                            //else
        //                            //{
        //                            //    return RedirectToAction("NoAuthorize", "Chatting");
        //                            //}

        //                        }
        //                        else if (role == "Admin")
        //                        {
        //                            //   FillAllOffices();

        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            AdminModel objAdminModel = new AdminModel();

        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;

        //                            objAdminModel = GetAdminDetailByUserId(user.ProviderUserKey.ToString());
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            Session["Name"] = objAdminModel.FirstName + " " + objAdminModel.LastName;
        //                            Session["role"] = role;
        //                            Session["IsSuperAdmin"] = "true";
        //                            Session["IsNurseCoordinator"] = "false";

        //                            Session["OrganisationId"] = objAdminModel.OrganisationId;

        //                            if (objAdminModel.OrganisationId > 0)
        //                            {
        //                                List<string> OrganisationAdminDetails = GetOrganisationDetailForQbChat(objAdminModel.OrganisationId);

        //                                Session["OrgSuperAdminName"] = OrganisationAdminDetails[0].ToString();
        //                                Session["OrgSuperAdminQBId"] = OrganisationAdminDetails[1].ToString();
        //                                Session["OrgSuperAdminEmail"] = OrganisationAdminDetails[2].ToString();
        //                                Session["OrgSuperAdminUserId"] = OrganisationAdminDetails[3].ToString();
        //                            }


        //                            Session["OfficeId"] = objAdminModel.OfficeIds;
        //                            Session["OfficeName"] = OfficeNameByLoginUserid(user.ProviderUserKey.ToString());

        //                          //  return RedirectToAction("Patients", "Patients");
        //                              return RedirectToAction("LoginDetails", "Account");
        //                        }
        //                        else if (role == "MarketingSuperAdmin")
        //                        {
        //                            FormsAuthentication.SetAuthCookie(objloginmodel.UserName, false);
        //                            LogsServiceProxy logService = new LogsServiceProxy();
        //                            Logs objLogs = new Logs();
        //                            objLogs.PageName = "Login";
        //                            objLogs.Message = "User:" + user.ProviderUserKey.ToString() + " logged into the system";
        //                            objLogs.UserID = user.ProviderUserKey.ToString();
        //                            string LoginResult = logService.InsertLog(objLogs).Result;

        //                            MarketerDetailsModel objAdmin = GetMarketingDetailByUserId(Membership.GetUser(objloginmodel.UserName).ProviderUserKey.ToString());

        //                            Session["Name"] = objAdmin.MarketersName;
        //                            Session["IsSuperAdmin"] = "false";
        //                            Session["UserId"] = user.ProviderUserKey.ToString();
        //                            // Session["AdminId"] = objAdmin.MarketersId.ToString();
        //                            Session["Email"] = objAdmin.Email.ToString();
        //                            // Status.QuickbloxId = objAdmin.QuickBloxId.ToString();

        //                            Session["IsAllowForPatientChatRoom"] = false;
        //                            Session["IsAllowOneToOneChat"] = false;
        //                            Session["IsAllowGroupChat"] = false;
        //                            Session["IsAllowToCreateGroupChat"] = false;
        //                            Session["OfficeIds"] = Convert.ToString(objAdmin.OfficeId);
        //                            Session["OfficeName"] = objAdmin.OfficeName;
        //                            Session["Role"] = role;

        //                            //Status.Result = "Success";

        //                            //  return RedirectToAction("Patients", "Patients");
        //                            return RedirectToAction("Index", "Client");

        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                insertdata("false login");

        //                TempData["LoginError"] = "true";
        //                TempData["message_login"] = CaregiverLite.Views.Resources.Account.LoginErrorMsg;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ErrorLog log = new ErrorLog();
        //            log.Errormessage = e.Message;
        //            log.StackTrace = e.StackTrace;
        //            log.Pagename = "AccountController";
        //            log.Methodname = "[HttpPost] Login";
        //            ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //            string res = ErrorLogService.InsertErrorLog(log).Result;
        //        }
        //    }
        //    return View();
        //}


        //private string GetOrganisationDetailById(int OrganisationId)
        //{
        //    string ORGname = "";
        //    try
        //    {
        //        //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOrganisationDetailById",
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOrganisationDetailById",
        //                                                OrganisationId);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            ORGname = ds.Tables[0].Rows[0]["OrganisationName"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetPatientDetailsList";
        //        //  string result = InsertErrorLog(objErrorlog);
        //    }
        //    return ORGname;

        //}

        //private List<string> GetOrganisationDetailForQbChat(int OrganisationId)
        //{
        //    List<string> OrganisationDetailsList = new List<string>();

        //    try
        //    {
        //        //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOrganisationDetailById",
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOrganisationDetailForQbChat",
        //                                                OrganisationId);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminName"].ToString());
        //            OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminQBId"].ToString());
        //            OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminEmail"].ToString());
        //            OrganisationDetailsList.Add(ds.Tables[0].Rows[0]["OrgSuperAdminUserId"].ToString());

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetPatientDetailsList";
        //        //  string result = InsertErrorLog(objErrorlog);
        //    }

        //    return OrganisationDetailsList;
        //}


        //[HttpGet]
        //public ActionResult LoginDetails()
        //{
        //    try
        //    {
        //       // insertdata("hello");
        //        int orgid = Convert.ToInt32(Session["OrganisationId"].ToString());


        //        ViewBag.Organtisation = (orgid > 0) ? GetOrganisationDetailById(orgid) : "Paseva";

        //       // insertdata(ViewBag.Organtisation);
        //        ViewBag.ImageUrl = (orgid > 0) ? "http://pasevauat.com/Content/image/Hospital.png" : "http://pasevauat.com/Content/image/Paseva_CompleteLogo.png";

        //       // insertdata(ViewBag.ImageUrl);
        //        ViewBag.RoleName = Session["Role"].ToString();
        //        ViewBag.Name = Session["Name"].ToString();
        //        ViewBag.Email = Session["FromUserEmail"].ToString();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult LoginDetails(FormCollection fs)
        //{
        //    try
        //    {
        //        string role = Session["role"].ToString();
        //        if (role == "Nurse")
        //        {
        //            return RedirectToAction("OneToOneChat", "Chatting");
        //        }
        //        else if (role == "SuperAdmin" || role == "OrgSuperAdmin")
        //        {
        //            return RedirectToAction("Index", "Chart");
        //        }
        //        else if (role == "Scheduler")
        //        {
        //            return RedirectToAction("PatientRequest", "PatientRequest");
        //        }
        //        else if (role == "NurseCoordinator")
        //        {
        //            return RedirectToAction("OneToOneChat", "Chatting");
        //        }
        //        else if (role == "Admin")
        //        {
        //            return RedirectToAction("Patients", "Patients");
        //        }
        //        else
        //        {
        //            TempData["LoginError"] = "true";
        //            TempData["message_login"] = CaregiverLite.Views.Resources.Account.LoginErrorMsg;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "AccountController";
        //        log.Methodname = "[HttpPost] Login";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }

        //    return RedirectToAction("Login", "Account");
        //}

        #endregion


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

        public ActionResult Logout()
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                FormsAuthentication.SignOut();

                LogsServiceProxy logService = new LogsServiceProxy();
                Logs objLogs = new Logs();
                objLogs.PageName = "Logout";
                objLogs.Message = "User:" + Membership.GetUser().ProviderUserKey.ToString() + " logged out.";
                objLogs.UserID = Membership.GetUser().ProviderUserKey.ToString();
                string LoginResult = logService.InsertLog(objLogs).Result;

            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AccountController";
                log.Methodname = "Logout";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return RedirectToAction("Login", "Account");
        }


        public ActionResult Administrator()
        {
            return View();
        }

        public ActionResult Register()
        {

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            List<SelectListItem> li = new List<SelectListItem>();
            for (int i = 5; i <= 75; i += 5)
            {
                li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
            }
            ViewData["Radious"] = li;
            ServicesServiceProxy ServicesService = new ServicesServiceProxy();
            ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword ForgotPassword)
        {
            try
            {
                if(!string.IsNullOrEmpty(Membership.GetUserNameByEmail(ForgotPassword.Email)))
                {
                    ForgetPasswordServiceProxy ForgetPasswordService = new ForgetPasswordServiceProxy();
                    string result = ForgetPasswordService.ForgotPassword(ForgotPassword.Email).Result;
                    return RedirectToAction("PasswordEmailConfirmation", new { Email = ForgotPassword.Email });
                    //TempData["message"] = "Email send to your id with verification link";
                }
                else
                {
                    TempData["message"] = "Please enter your registered email address";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AccountController";
                log.Methodname = "ForgotPassword";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
             return View();
            
        }

        [HttpGet]
        public ActionResult PasswordEmailConfirmation(string Email)
        {
            if (Email != null)
            {
                TempData["RegisteredEmail"] = Email;
                return View();
            }
            else
            {
                return RedirectToAction("ForgotPassword");
            }
        }

        public ActionResult Profile()
        {

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            CareGiverModel objModel = new CareGiverModel();
            objModel = GetCareGiverDetailById(Session["NurseId"].ToString());
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

            int k = objModel.HoursRate.Count();
            string rate = "";
            if (k > 6)
            {
                rate = String.Format("{0:0.0}", objModel.HoursRate);
                objModel.HoursRate = Convert.ToDouble(rate).ToString("0.0");
            }
            ViewData["Radious"] = li;
            ServicesServiceProxy ServicesService = new ServicesServiceProxy();
            ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
            return View(objModel);
        }

        [Authorize(Roles = "Nurse")]
        private CareGiverModel GetCareGiverDetailById(string id)
        {
            CareGiverModel objCareGiver = new CareGiverModel();
            try
            {
                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                CareGivers CareGiver = CareGiverLiteService.GetAllCareGiverByNurseId(id).Result;
                objCareGiver.NurseId = CareGiver.NurseId;
                objCareGiver.Name = CareGiver.Name;
                objCareGiver.Services = CareGiver.ServiceId;
                objCareGiver.ServiceNames = CareGiver.ServiceName;
                objCareGiver.Email = CareGiver.Email;
                objCareGiver.Username = CareGiver.UserName;
                objCareGiver.Address = CareGiver.Address;
                objCareGiver.ZipCode = CareGiver.ZipCode;
                objCareGiver.ProfileImage = CareGiver.ProfileImage;
                objCareGiver.ProfileVideo = CareGiver.ProfileVideo;
                objCareGiver.ProfileVideoThumbnil = CareGiver.ProfileVideoThumbnil;
                objCareGiver.HoursRate = CareGiver.HourlyRate.ToString();
                objCareGiver.Phone = CareGiver.Phone;
                objCareGiver.Password = CareGiver.Password;
                objCareGiver.UserId = CareGiver.UserId;
                objCareGiver.ServiceRadius = CareGiver.DistanceFromLocation.ToString();
                objCareGiver.DistanceUnit = CareGiver.DistanceUnit;
                objCareGiver.Education = CareGiver.Education;
                objCareGiver.CanAdminEdit = CareGiver.CanAdminEdit;
                objCareGiver.ChargeToPatient = CareGiver.ChargeToPatient;
                objCareGiver.HoursRateList = CareGiver.HoursRate;
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
            return objCareGiver;
        }


        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult ChangePassword(CareGiverModel objCareGiverModel)
        //{
        //    try
        //    {
        //        CareGivers objCareGivers = new CareGivers();
        //        objCareGivers.UserId = Session["UserId"].ToString();
        //        objCareGivers.NewPassword = objCareGiverModel.NewPassword;

        //        CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
        //        string res = CareGiverLiteService.ResetPassword(objCareGivers).Result;
        //        if (res == "Success")
        //        {
        //            TempData["message"] = CaregiverLite.Views.Resources.Account.msgSuccessPassword;
        //        }
        //        else
        //        {
        //            TempData["message"] = CaregiverLite.Views.Resources.Account.msgErrorPassword;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        ErrorLog log = new ErrorLog();
        //        log.Errormessage = e.Message;
        //        log.StackTrace = e.StackTrace;
        //        log.Pagename = "ChangePassword";
        //        log.Methodname = "ChangePassword";
        //        ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
        //        string res = ErrorLogService.InsertErrorLog(log).Result;
        //    }
        //    ModelState.Clear();
        //    return View();
        //}


        [HttpPost]
        public ActionResult Register(CareGiverModel objCareGiverModel)
        {
            var CareGiver = new CareGivers();
            ServicesServiceProxy ServicesService = new ServicesServiceProxy();

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            try
            {

                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                //  string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();

                if (objCareGiverModel.NurseId == 0)
                {
                    MembershipCreateStatus status;
                    Membership.CreateUser(objCareGiverModel.Username, objCareGiverModel.Password, objCareGiverModel.Email, null, null, true, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(objCareGiverModel.Username, "Nurse");
                    }
                }

                CareGiver.NurseId = objCareGiverModel.NurseId;
                CareGiver.UserId = Membership.GetUser(objCareGiverModel.Username).ProviderUserKey.ToString();
                CareGiver.Name = objCareGiverModel.Name;
                CareGiver.Email = objCareGiverModel.Email;
                CareGiver.ServiceId = objCareGiverModel.ServiceNames;
                CareGiver.HourlyRate = Convert.ToDecimal(objCareGiverModel.HoursRate);
                CareGiver.DistanceFromLocation = Convert.ToDecimal(objCareGiverModel.ServiceRadius);
                CareGiver.Phone = objCareGiverModel.Phone;
                CareGiver.Address = objCareGiverModel.Address;
                CareGiver.ZipCode = objCareGiverModel.ZipCode;
                CareGiver.ProfileImage = objCareGiverModel.ProfileImage;
                CareGiver.Latitude = objCareGiverModel.Latitude;
                CareGiver.Longitude = objCareGiverModel.Longitude;
                CareGiver.DistanceUnit = "Miles";
                CareGiver.InsertUserId = CareGiver.UserId;
                CareGiver.UserName = objCareGiverModel.Username;
                CareGiver.Password = objCareGiverModel.Password;
                CareGiver.Education = objCareGiverModel.Education;
                CareGiver.CanAdminEdit = objCareGiverModel.CanAdminEdit;

                

                if (objCareGiverModel.ProfileImageFile != null && objCareGiverModel.ProfileImageFile.ContentLength > 0)
                {
                    int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    CareGiver.ProfileImage = UploadFile.getUploadFile_and_getFileURL(objCareGiverModel.ProfileImageFile, ConfigurationManager.AppSettings["NurseProfileImageURL"].ToString(), "CareGiver_" + CareGiver.UserId + datetime.ToString() + ".jpeg");
                }
                else
                {
                    CareGiver.ProfileImage = objCareGiverModel.ProfileImage.Split('/')[objCareGiverModel.ProfileImage.Split('/').Length - 1];
                }
                if (objCareGiverModel.ProfileVideoFile != null && objCareGiverModel.ProfileVideoFile.ContentLength > 0)
                {
                    int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    CareGiver.ProfileVideo = UploadFile.getUploadFile_and_getFileURL(objCareGiverModel.ProfileVideoFile, ConfigurationManager.AppSettings["NurseProfileImageURL"].ToString(), "CareGiver_Video_" + CareGiver.UserId + datetime.ToString() + ".mp4");

                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                    ffMpeg.GetVideoThumbnail(ConfigurationManager.AppSettings["NurseProfileImageURL"] + "\\CareGiver_Video_" + CareGiver.UserId + datetime.ToString() + ".mp4", ConfigurationManager.AppSettings["NurseProfileImageURL"] + "\\CareGiver_VideoThumbnil_" + CareGiver.UserId + datetime.ToString() + ".jpeg", 5);
                    CareGiver.ProfileVideoThumbnil = "CareGiver_VideoThumbnil_" + CareGiver.UserId + datetime.ToString() + ".jpeg";
                }
   
                                                                
                //else   
                //{  
                //    CareGiver.ProfileVideo = objCareGiverModel.ProfileVideo.Split('/')[objCareGiverModel.ProfileVideo.Split('/').Length - 1];
                //    CareGiver.ProfileVideoThumbnil = objCareGiverModel.ProfileVideoThumbnil.Split('/')[objCareGiverModel.ProfileVideoThumbnil.Split('/').Length - 1];
                //}   

                                                            
                string result = CareGiverLiteService.InsertUpdateCareGiverByAdmin(CareGiver).Result;

                if (objCareGiverModel.NurseId == 0)
                {
                    FormsAuthentication.SetAuthCookie(Membership.GetUser(new Guid(CareGiver.UserId.ToString())).UserName, false);
                }

                if (result == "Success")
                {
                    string Message = "";
                    string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/EmailTemplates/WelcomeCareGiver.html");
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        Message = reader.ReadToEnd();
                    }

                    Message = Message.Replace("{CareGiverName}", CareGiver.Name);

                    MessageQueue MQ = new MessageQueue();
                    MQ.EmailID = CareGiver.Email;
                    MQ.Message = Message;
                    MQ.Subject = "Welcome CareGiver";
                    MQ.UserID = CareGiver.UserId;
                    MQ.MobileNumber = "0";

                    MessageQueueServiceProxy MessageQueueService = new MessageQueueServiceProxy();
                    result = MessageQueueService.InsertMessageQueue(MQ).Result;

                    TempData["message_login"] = "Registration successful. Please login";
                    return RedirectToAction("Login", "Account");
                }

                ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
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
            }

            List<SelectListItem> li = new List<SelectListItem>();
            for (int i = 5; i <= 75; i += 5)
            {
                li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
            }
            ViewData["Radious"] = li;
            ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
            return View(objCareGiverModel);
        }


        [SessionExpire]
        public ActionResult Verification()
        {
            CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
            List<CareGivers> listcaregiver = CareGiverLiteService.GetCareGiverCertiByNurseId(Session["NurseId"].ToString()).Result;
            ViewBag.CertiList = listcaregiver;
            ViewBag.Message = "";
            return View(new CareGiverModel());
        }


        [SessionExpire]
        [HttpPost]
        public ActionResult Verification(CareGiverModel objCareGiverModel)
        {
            try
            {
                CareGivers objCareGivers = new CareGivers();
                objCareGivers.ProfileImage = objCareGiverModel.ProfileImage;
                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                if (objCareGiverModel.ProfileImageFile != null && objCareGiverModel.ProfileImageFile.ContentLength > 0)
                {
                    int datetime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    objCareGivers.Certificate = UploadFile.getUploadFile_and_getFileURL(objCareGiverModel.ProfileImageFile, ConfigurationManager.AppSettings["CareGiverDocumentsURL"].ToString(), "CareGiver_" + Session["UserId"].ToString() + datetime.ToString() + ".jpeg");

                    string res = CareGiverLiteService.InsertNurseCertificate(Session["NurseId"].ToString(), objCareGivers.Certificate, Session["UserId"].ToString()).Result;

                    ViewBag.Message = "";
                }
                else
                {
                    ViewBag.Message = "Please upload certificate";
                }
                List<CareGivers> listcaregiver = CareGiverLiteService.GetCareGiverCertiByNurseId(Session["NurseId"].ToString()).Result;
                ViewBag.CertiList = listcaregiver;

                return View();
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "Verification";
                log.Methodname = "Verification";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                return View();
            }
        }


        public ActionResult ResetPassword()
        {
            ForgotPassword objForgotPassword = new ForgotPassword();
            try
            {
                if (Request.QueryString["digest"] != null)
                {
                    string digest = Request.QueryString["digest"].ToString();
                    var parts = ValidateResetCode(HttpUtility.UrlDecode(digest));

                    if (!parts)
                    {
                        TempData["Message"] = "Url is invalid. Please try again";
                        return RedirectToAction("ForgotPassword", "Account");
                    }

                    var decrypted = Models.Encryption.Decrypt(HttpUtility.UrlDecode(digest));
                    var ArrayObj = decrypted.Split('&');

                    objForgotPassword.Email = ArrayObj[0];
                }
                else
                {
                    TempData["Message"] = "Url is invalid. Please try again";
                    return RedirectToAction("ForgotPassword", "Account");
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ResetPassword";
                log.Methodname = "ResetPassword";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return View(objForgotPassword);
        }

        public bool ValidateResetCode(string encryptedParam)
        {
            string decrypted = "";
            var results = false;

            try
            {
                decrypted = CaregiverLite.Models.Encryption.Decrypt(encryptedParam);
            }
            catch (Exception ex)
            {
                return results;
            }

            var parts = decrypted.Split('&');
            if (parts.Length != 3) return results;
            var expires = DateTime.Now.AddHours(-1);

            long ticks = 0;
            if (!long.TryParse(parts[2], out ticks)) return results;
            expires = new DateTime(ticks);

            if (expires < DateTime.Now) return results;
            results = true;

            return results;
        }


        [HttpPost]
        public ActionResult ResetPassword(ForgotPassword ForgotPassword)
        {
            try
            {
                string UserName = Membership.GetUserNameByEmail(ForgotPassword.Email);
                MembershipUser user = Membership.GetUser(UserName);

                if (user.ChangePassword(user.ResetPassword(), ForgotPassword.NewPassword))
                {
                    CaregiverLiteService.ForgetPasswordUpdatingUserLogininfoUserPassword(ForgotPassword.Email, ForgotPassword.NewPassword);
                    //TempData["message_login"] = "Your password is changed. Please Login";
                    return RedirectToAction("PasswordChangedSuccessful", "Account");
                }
                else
                {
                    TempData["message"] = "There is error while changing password. Please try again";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ResetPassword";
                log.Methodname = "ResetPassword";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return View();
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
                    result = false;
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

        public string DeleteNurseCertificateById(string NurseCertificateId)
        {
            string result = "";
            try
            {
                CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();
                result = CareGiverLiteService.DeleteNurseCertificateById(Session["UserId"].ToString(), NurseCertificateId).Result;
                TempData["Message"] = "Certificate is deleted successfully.";
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

        public SchedulerModel GetSchedulerDetailByUserId(string SchedulerUserId)
        {
            SchedulerModel objScheduler = new SchedulerModel();
            try
            {
                SchedulerServiceProxy CareGiverLiteService = new SchedulerServiceProxy();
                Scheduler Scheduler = CareGiverLiteService.GetSchedulerDetailByUserId(SchedulerUserId).Result;
                objScheduler.SchedulerId = Scheduler.SchedulerId;
                objScheduler.FirstName = Scheduler.FirstName;
                objScheduler.LastName = Scheduler.LastName;
                objScheduler.Email = Scheduler.Email;
                objScheduler.Username = Scheduler.UserName;
                objScheduler.OfficeIds = Scheduler.OfficeIds;
                objScheduler.OrganisationId = Scheduler.OrganisationId;
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

        public NurseCoordinatorModel GetNurseCoordinatorDetailByUserId(string NurseCoordinatorUserId)
        {
            NurseCoordinatorModel objNurseCoordinator = new NurseCoordinatorModel();
            try
            {
                NurseCoordinatorServiceProxy CareGiverLiteService = new NurseCoordinatorServiceProxy();
                NurseCoordinator NurseCo = CareGiverLiteService.GetNurseCoordinatorDetailByUserId(NurseCoordinatorUserId).Result;
                objNurseCoordinator.NurseCoordinatorId = NurseCo.NurseCoordinatorId;
                objNurseCoordinator.FirstName = NurseCo.FirstName;
                objNurseCoordinator.LastName = NurseCo.LastName;
                objNurseCoordinator.Username = NurseCo.UserName;
                objNurseCoordinator.IsAllowForPatientChatRoom = NurseCo.IsAllowForPatientChatRoom;
                objNurseCoordinator.IsOfficePermission = NurseCo.IsOfficePermission;
                objNurseCoordinator.IsAllowOneToOneChat = NurseCo.IsAllowOneToOneChat;
                objNurseCoordinator.IsAllowGroupChat = NurseCo.IsAllowGroupChat;
                objNurseCoordinator.OrganisationId = NurseCo.OrganisationId;
                objNurseCoordinator.IsAllowToCreateGroupChat = NurseCo.IsAllowToCreateGroupChat;
                objNurseCoordinator.OfficeId = NurseCo.OfficeId;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "GetNurseCoordinatorDetailByUserId";
                log.Methodname = "GetNurseCoordinatorDetailByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objNurseCoordinator;
        }


        public ActionResult PasswordChangedSuccessful()
        {
            return View();
        }

        public AdminModel GetAdminDetailByUserId(string AdminUserId)
        {
            AdminModel objAdmin = new AdminModel();
            try
            {
                AdminServiceProxy CareGiverLiteService = new AdminServiceProxy();
                Admin Admin = CareGiverLiteService.GetAdminDetailByUserId(AdminUserId).Result;
                objAdmin.AdminId = Admin.AdminId;
                objAdmin.FirstName = Admin.FirstName;
                objAdmin.LastName = Admin.LastName;
                objAdmin.Username = Admin.UserName;
                objAdmin.OfficeIds = Admin.OfficeIds;
                objAdmin.Email = Admin.Email;
                objAdmin.OrganisationId = Admin.OrganisationId;
                
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AccountController";
                log.Methodname = "GetAdminDetailByUserId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return objAdmin;
        }

        public JsonResult IsAllowForPatientChatRoom()
        {
            bool result = true;
            try
            {

                //CareGivers objCareGivers = CareGiverLiteService.GetAllCareGiverByUserId(user.ProviderUserKey.ToString()).Result;

                NurseCoordinatorModel objNurseCoordinator = new NurseCoordinatorModel();

                //  MembershipUser user = Membership.GetUser();
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();

                objNurseCoordinator = GetNurseCoordinatorDetailByUserId(LoginUserId);
                Session["IsAllowForPatientChatRoom"] = objNurseCoordinator.IsAllowForPatientChatRoom;
                result = objNurseCoordinator.IsAllowForPatientChatRoom;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "CareGiverController";
                log.Methodname = "IsAllowForPatientChatRoom";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsAllowForChat()
        {
            bool result = true;

            //string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            //var isAjax = Request.IsAjaxRequest();
            try
            {
                string LoginUserId = Membership.GetUser().ProviderUserKey.ToString();

           //     AllOffice();

                if (Session["role"].ToString() == "Nurse")
                {
                    CareGiverServiceProxy CareGiverLiteService = new CareGiverServiceProxy();

                    CareGivers objCareGivers = CareGiverLiteService.GetAllCareGiverByUserId(LoginUserId).Result;

                    Session["IsOfficePermission"] = objCareGivers.IsOfficePermission;

                  //  Session["IsAllowOneToOneChat"] = objCareGivers.IsAllowOneToOneChat;
                    Session["IsAllowOneToOneChat"] = "True";
                    Session["IsAllowForPatientChatRoom"] = objCareGivers.IsAllowPatientChatRoom;
                    Session["IsAllowGroupChat"] = objCareGivers.IsAllowGroupChat;
                    Session["IsAllowToCreateGroupChat"] = objCareGivers.IsAllowToCreateGroupChat;

                   //if (!objCareGivers.IsAllowOneToOneChat)
                   // {

                   //     if (objCareGivers.IsAllowPatientChatRoom)
                   //     {
                   //         return RedirectToAction("PatientGroupChatting", "Chatting");
                   //     }
                   //     else if (objCareGivers.IsAllowGroupChat)
                   //     {
                   //         return RedirectToAction("GroupChat", "Chatting");
                   //     }
                   //     else
                   //     {
                   //         return RedirectToAction("NoAuthorize", "Chatting");
                   //     }
                   // }
   
                   // else
                   // {
                   //     return RedirectToAction("Chatting", "Chatting");
                   //     //return RedirectToAction("NoAuthorize", "Chatting");
                   // }

                }
                else if (Session["role"].ToString() == "NurseCoordinator")
                {   
                    NurseCoordinatorModel objNurseCoordinator = new NurseCoordinatorModel();

                    objNurseCoordinator = GetNurseCoordinatorDetailByUserId(LoginUserId);
                    Session["IsOfficePermission"] = objNurseCoordinator.IsOfficePermission;

                    //  Session["IsAllowOneToOneChat"] = objNurseCoordinator.IsAllowOneToOneChat;

                    Session["IsAllowOneToOneChat"] = "True";
                    Session["IsAllowForPatientChatRoom"] = objNurseCoordinator.IsAllowForPatientChatRoom;
                    Session["IsAllowGroupChat"] = objNurseCoordinator.IsAllowGroupChat;
                    Session["IsAllowToCreateGroupChat"] = objNurseCoordinator.IsAllowToCreateGroupChat;

                     //result = objNurseCoordinator.IsAllowForPatientChatRoom;
                }       
            }      
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AccountController";
                log.Methodname = "IsAllowForChat";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ContactUs()
        {
            return View();
        }

        public string UpdatetNotificationSoundValue(string LogedInUserId)
        {
            string result = "";
            try
            {
                // result = new PushNotificationServiceProxy().UpdatetNotificationSoundValue(LogedInUserId).Result;
                //if (!String.IsNullOrEmpty(result))
                //{
                //    Session["NotificationSoundStatus"] = result;
                //}

                DataSet ds = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateNotificationSound", LogedInUserId);
                result = (ds.Tables[0].Rows[0][0]).ToString();
                Session["NotificationSoundStatus"] = (ds.Tables[0].Rows[0][0]).ToString();


            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "AccountController";
                log.Methodname = "UpdatetNotificationSoundValue";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }


        public string OfficeNameByLoginUserid(string LoginUserId)
        {
            string OfficeName = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAvailableOffices", Guid.Parse(LoginUserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    OfficeName = ds.Tables[0].Rows[0]["OfficeName"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOfficesList";
                //  string result = InsertErrorLog(objErrorlog);
            }
            return OfficeName;
        }


        private MarketerDetailsModel GetMarketingDetailByUserId(string UserId)
        {
            MarketerDetailsModel objMarketers = new MarketerDetailsModel();
            try
            {


                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetMarketingDetailByUserId_Device", new Guid(UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objMarketers.MarketersId = Convert.ToInt32(ds.Tables[0].Rows[0]["MarketersId"]);
                    objMarketers.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    objMarketers.MarketersName = ds.Tables[0].Rows[0]["MarketersName"].ToString();
                    objMarketers.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    objMarketers.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    objMarketers.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    // objMarketers.QuickBloxId = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();

                    // objScheduler.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"]);
                    objMarketers.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());
                    objMarketers.OfficeName = ds.Tables[0].Rows[0]["OfficeName"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteWebService";
                objErrorlog.Methodname = "GetAdminDetailByUserId";
                // string result = InsertErrorLog(objErrorlog);
            }
            return objMarketers;
        }

        //public void AllOffice()
        //{
        //    string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
        //    OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();
        //    var lstOffices = officeServiceProxy.GetAllOffices(logInUserId).Result;


        //    var result = string.Join(",", lstOffices);
        //}
        [AllowAnonymous]
        public DataSet GetAccessHoursTakenByNurse(int NurseID)
        {
            string Nur = "";
            DataSet dt = new DataSet();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOrganisationDetailById",
                 dt = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "PR_CheckCaregiverAccessTime",
                                                        NurseID);

                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    ORGname = ds.Tables[0].Rows[0]["OrganisationName"].ToString();
                //}
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientDetailsList";
                //  string result = InsertErrorLog(objErrorlog);
            }
            return dt;
        }
       
    }
}
