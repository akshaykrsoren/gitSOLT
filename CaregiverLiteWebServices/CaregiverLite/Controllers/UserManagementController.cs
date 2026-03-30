using CaregiverLite.Action_Filters;
using CaregiverLite.Controllers;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using DifferenzLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class UserManagementController : Controller
    {
        string UserRole = "";

        // GET: UserManagement
        public ActionResult Index()
        {
            return View();
        }
        public string GetUserProfileIdByuserId(string UserId, string Role)
        {
            string id = "";

            try
            {

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetUserProfileIdByUserId",
                                                     UserId,
                                                     Role
                                                   );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    id = ds.Tables[0].Rows[0][0].ToString();

                }

            }

            catch (Exception e)
            {

            }
            return id;
        }
        public ActionResult UserProfile(string id)
        {
            UserRole = Session["role"].ToString();
            Userprofile objUserprofile = new Userprofile();


            try
            {
                CareGiverController CCtrl = new CareGiverController();


                string ProfileId = GetUserProfileIdByuserId(id, UserRole);

                if (UserRole == "Nurse")
                {
                    objUserprofile.ObjCaregiverModel = CCtrl.GetCareGiverDetailById(ProfileId);
                    @ViewBag.ProfileName = objUserprofile.ObjCaregiverModel.FirstName + " " + objUserprofile.ObjCaregiverModel.LastName;
                }
                else if (UserRole == "Admin")
                {

                    AccountController ACtrl = new AccountController();
                    objUserprofile.ObjAdminModelModel = ACtrl.GetAdminDetailByUserId(id); 
                    @ViewBag.ProfileName = objUserprofile.ObjAdminModelModel.FirstName + " " + objUserprofile.ObjAdminModelModel.LastName;
                }
                else
               if (UserRole == "NurseCoordinator") 
                {
                    NurseCoordinatorController NCtrl = new NurseCoordinatorController();
                    objUserprofile.ObjNurseCoordinatorModel = NCtrl.GetNurseCoordinatorDetailById(ProfileId );
                    @ViewBag.ProfileName = objUserprofile.ObjNurseCoordinatorModel.FirstName + " " + objUserprofile.ObjNurseCoordinatorModel.LastName;
                }
                else if (UserRole == "Scheduler")
                { 
                    AccountController SCtrl = new AccountController();
                    objUserprofile.ObjSchedulerModel = SCtrl.GetSchedulerDetailByUserId(id);
                    @ViewBag.ProfileName = objUserprofile.ObjSchedulerModel.FirstName + " " + objUserprofile.ObjSchedulerModel.LastName;
                }

                else if (UserRole == "SuperAdmin"  )
                { 
                    objUserprofile = getAdminDetail(UserRole, id);
                    @ViewBag.ProfileName = objUserprofile.ObjSchedulerModel.FirstName + " " + objUserprofile.ObjSchedulerModel.LastName;
                }
                else if (UserRole == "OrgSuperAdmin")
                { 
                    objUserprofile = getAdminDetail(UserRole, id);
                    @ViewBag.ProfileName = objUserprofile.ObjAdminModelModel.FirstName + " " + objUserprofile.ObjAdminModelModel.LastName;
                }


            }

            catch (Exception e)
            {

            }
            return PartialView(objUserprofile);
        }



        public ActionResult OpenEditProfile(string id)
        {
            UserRole = Session["role"].ToString();
            Userprofile objUserprofile = new Userprofile();


            try
            {
                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                List<Office> OfficeList = new List<Office>();
                OfficeList = new OfficeServiceProxy().GetAllOffices(Membership.GetUser().ProviderUserKey.ToString(), OrganisationId.ToString()).Result;
                SelectList officeSelectList = new SelectList(OfficeList, "OfficeId", "OfficeName");
                ViewBag.lstOffice = officeSelectList;

                string ProfileId = GetUserProfileIdByuserId(id, UserRole);
                if (UserRole == "Nurse")
                {
                    CareGiverController CCtrl = new CareGiverController();
                    objUserprofile.ObjCaregiverModel = CCtrl.GetCareGiverDetailById(ProfileId);
                    List<SelectListItem> li = new List<SelectListItem>();
                    for (int i = 5; i <= 75; i += 5)
                    {
                        if (i == Convert.ToInt32(objUserprofile.ObjCaregiverModel.ServiceRadius))
                        {
                            li.Add(new SelectListItem { Selected = true, Text = i + " Miles", Value = i.ToString() });
                        }
                        else
                        {
                            li.Add(new SelectListItem { Text = i + " Miles", Value = i.ToString() });
                        }

                    }
                    ViewData["Radious"] = li;
                    int k = objUserprofile.ObjCaregiverModel.HoursRate.Count();
                    string rate = "";
                    if (k > 6)
                    {
                        rate = String.Format("{0:0.0}", objUserprofile.ObjCaregiverModel.HoursRate);
                        objUserprofile.ObjCaregiverModel.HoursRate = Convert.ToDouble(rate).ToString("0.0");
                    }


                    objUserprofile.ObjCaregiverModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;

                    ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                    ViewBag.ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
                    @ViewBag.ProfileName = objUserprofile.ObjCaregiverModel.FirstName + " " + objUserprofile.ObjCaregiverModel.LastName;
                    return PartialView("EditCareGiverProfile", objUserprofile);
                }
                else if (UserRole == "Admin")
                {
                    AccountController ACtrl = new AccountController();
                    objUserprofile.ObjAdminModelModel = ACtrl.GetAdminDetailByUserId(id);
                    @ViewBag.ProfileName = objUserprofile.ObjAdminModelModel.FirstName + " " + objUserprofile.ObjAdminModelModel.LastName;
                    return PartialView("EditAdminProfile", objUserprofile);
                }
                else
               if (UserRole == "NurseCoordinator")
                {
                    NurseCoordinatorController NCtrl = new NurseCoordinatorController();
                    objUserprofile.ObjNurseCoordinatorModel = NCtrl.GetNurseCoordinatorDetailById(ProfileId);
                    @ViewBag.ProfileName = objUserprofile.ObjNurseCoordinatorModel.FirstName + " " + objUserprofile.ObjNurseCoordinatorModel.LastName;
                    objUserprofile.ObjNurseCoordinatorModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
                    return PartialView("EditNurseCoordinatorProfile", objUserprofile);
                }
                else if (UserRole == "Scheduler")
                {
                    AccountController SCtrl = new AccountController();
                    objUserprofile.ObjSchedulerModel = SCtrl.GetSchedulerDetailByUserId(id);
                    @ViewBag.ProfileName = objUserprofile.ObjSchedulerModel.FirstName + " " + objUserprofile.ObjSchedulerModel.LastName;
                    return PartialView("EditSchedularProfile", objUserprofile);
                }
                else if (UserRole == "SuperAdmin")
                { 
                    objUserprofile = getAdminDetail(UserRole, id);
                    @ViewBag.ProfileName = objUserprofile.ObjSchedulerModel.FirstName + " " + objUserprofile.ObjSchedulerModel.LastName;
                    return PartialView("EditSchedularProfile", objUserprofile);
                }
                else if (UserRole == "OrgSuperAdmin")
                {
                    
                    objUserprofile= getAdminDetail(UserRole, id); 
                    @ViewBag.ProfileName = objUserprofile.ObjAdminModelModel.FirstName + " " + objUserprofile.ObjAdminModelModel.LastName;
                    return PartialView("EditAdminProfile", objUserprofile);
                }


            }

            catch (Exception e)
            {

            }
            return PartialView(objUserprofile);
        }


        [HttpPost]
        public ActionResult UpdateCareGiverProfile()
        {
            CareGiverModel objCareGiverModel = new CareGiverModel();
            try
            {

                CareGiverServiceProxy CareGiverService = new CareGiverServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                string json = System.Web.HttpContext.Current.Request["ObjCareGiver"];
                objCareGiverModel = JsonConvert.DeserializeObject<CareGiverServiceProxy>(json).CareGiverModelDetail;
                string NurseFullName = System.Web.HttpContext.Current.Request["NurseFullName"];



                // Retrieve the user by username
                //MembershipUser user = Membership.GetUser(InsertedUserID);

                //if (user != null)
                //{
                //    // Update the email address
                //    user.Email = "newemail@example.com";

                //    // Save the changes
                //    Membership.UpdateUser(user);
                //    Console.WriteLine("Email updated successfully.");
                //}
                //else
                //{
                //    Console.WriteLine("User not found.");
                //}




                var CareGiver = new CareGivers();
                CareGiver.NurseId = objCareGiverModel.NurseId;
                CareGiver.UserId = Membership.GetUser(objCareGiverModel.Username).ProviderUserKey.ToString();
                objCareGiverModel.Name = objCareGiverModel.FirstName + " " + objCareGiverModel.LastName;
                CareGiver.Name = objCareGiverModel.Name;
                CareGiver.NurseFullName = NurseFullName;
                CareGiver.FirstName = objCareGiverModel.FirstName;
                CareGiver.LastName = objCareGiverModel.LastName;
                CareGiver.Email = objCareGiverModel.Email;
                CareGiver.ServiceId = objCareGiverModel.Services;
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
                CareGiver.CanAdminEdit = objCareGiverModel.CanAdminEdit;
                //CareGiver.Password = objCareGiverModel.Password;
                CareGiver.ChargeToPatient = Convert.ToDecimal(objCareGiverModel.ChargeToPatient);
                CareGiver.Education = objCareGiverModel.Education;
                CareGiver.OfficeId = objCareGiverModel.OfficeId;
                CareGiver.IsAllowOneToOneChat = objCareGiverModel.IsAllowOneToOneChat;
                CareGiver.IsAllowPatientChatRoom = objCareGiverModel.IsAllowPatientChatRoom;
                CareGiver.IsAllowGroupChat = objCareGiverModel.IsAllowGroupChat;
                CareGiver.IsAllowToCreateGroupChat = objCareGiverModel.IsAllowToCreateGroupChat;
                CareGiver.PayrollId = objCareGiverModel.PayrollId;
                int temppayrolId = 0;
                if (!String.IsNullOrEmpty(objCareGiverModel.PayrollId))
                {
                    temppayrolId = Convert.ToInt32(objCareGiverModel.PayrollId);
                }

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_UpdateCareGiver_Web",
                                                   CareGiver.NurseId,
                                                   new Guid(CareGiver.UserId),
                                                   CareGiver.Name,
                                                   CareGiver.FirstName,
                                                   CareGiver.LastName,
                                                   CareGiver.NurseFullName,
                                                   CareGiver.Email,
                                                   CareGiver.ServiceId,
                                                   CareGiver.HourlyRate,
                                                   CareGiver.DistanceFromLocation,
                                                   CareGiver.Phone,
                                                   CareGiver.Address,
                                                   CareGiver.Street,
                                                   CareGiver.City,
                                                   CareGiver.State,
                                                   CareGiver.ZipCode,
                                                   CareGiver.ProfileImage,
                                                   CareGiver.Latitude,
                                                   CareGiver.Longitude,
                                                   CareGiver.DistanceUnit,
                                                   new Guid(CareGiver.InsertUserId),
                                                   CareGiver.UserName,
                                                   //CareGiver.Password,
                                                   CareGiver.Education,
                                                   CareGiver.CanAdminEdit,
                                                   CareGiver.OfficeId,
                                                   CareGiver.IsAllowOneToOneChat,
                                                   CareGiver.IsAllowPatientChatRoom,
                                                   CareGiver.IsAllowGroupChat,
                                                   CareGiver.IsAllowToCreateGroupChat,
                                                   temppayrolId
                                                   );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["Name"] = objCareGiverModel.FirstName + " " + objCareGiverModel.LastName;
                    //var ChattingController = new ChattingController();

                    //if (objCareGiverModel.OldEmail != objCareGiverModel.Email)
                    //{

                    //    Task.Run(async () => { await ChattingController.ApiToChangeEmailToQuickblox(objCareGiverModel.OldEmail, objCareGiverModel.Email, objCareGiverModel.QuickBloxId); }).Wait();

                    //}

                    string result1 = ds.Tables[0].Rows[0][0].ToString();
                    TempData["Message"] = "Data updated successfully.";
                    return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ModelState.AddModelError("Error", result);
                    return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
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



            return Json(objCareGiverModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateNurseCoordinatorProfile()
        {
            try
            {
                NurseCoordinatorModel objNurseCoordinatorModel = new NurseCoordinatorModel();
                NurseCoordinatorServiceProxy NurseCoordinatorService = new NurseCoordinatorServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                string json = System.Web.HttpContext.Current.Request["ObjNurseCoordinate"];
                objNurseCoordinatorModel = JsonConvert.DeserializeObject<NurseCoordinatorServiceProxy>(json).NurseCoordinatorModel;

                var NurseCoordinator = new NurseCoordinator();
                NurseCoordinator.NurseCoordinatorId = objNurseCoordinatorModel.NurseCoordinatorId;
                NurseCoordinator.FirstName = objNurseCoordinatorModel.FirstName;
                NurseCoordinator.LastName = objNurseCoordinatorModel.LastName;
                NurseCoordinator.Email = objNurseCoordinatorModel.Email;
                NurseCoordinator.Password = objNurseCoordinatorModel.Password;
                NurseCoordinator.OfficeId = objNurseCoordinatorModel.OfficeId;
                NurseCoordinator.IsAllowForPatientChatRoom = objNurseCoordinatorModel.IsAllowForPatientChatRoom;
                NurseCoordinator.JobTitle = objNurseCoordinatorModel.JobTitle;
                NurseCoordinator.InsertUserId = InsertedUserID;
                NurseCoordinator.IsAllowOneToOneChat = objNurseCoordinatorModel.IsAllowOneToOneChat;
                NurseCoordinator.IsAllowGroupChat = objNurseCoordinatorModel.IsAllowGroupChat;
                NurseCoordinator.IsAllowToCreateGroupChat = objNurseCoordinatorModel.IsAllowToCreateGroupChat;

                string result = NurseCoordinatorService.EditNurseCoordinator(NurseCoordinator).Result;
                if (result == "Success")
                {
                    Session["Name"] = objNurseCoordinatorModel.FirstName + " " + objNurseCoordinatorModel.LastName;
                    //var ChattingController = new ChattingController();


                    //if (objNurseCoordinatorModel.OldEmail != objNurseCoordinatorModel.Email)
                    //{

                    //    Task.Run(async () => { await ChattingController.ApiToChangeEmailToQuickblox(objNurseCoordinatorModel.OldEmail, objNurseCoordinatorModel.Email, objNurseCoordinatorModel.QuickBloxId); }).Wait();

                    //}

                    TempData["Message"] = "Data updated successfully.";
                    return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
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

            return Json(new { Result = "Somthing Wrong " }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult UpdateAdminProfile()
        {
            try
            {
                Admin objAdminModel = new Admin();
                AdminServiceProxy AdminService = new AdminServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                string json = System.Web.HttpContext.Current.Request["ObjAdminData"];
                objAdminModel = JsonConvert.DeserializeObject<AdminServiceProxy>(json).AdminDetail;

                var x = JsonConvert.DeserializeObject(json);               

                var Admin = new Admin();
                Admin.AdminId = objAdminModel.AdminId;
                Admin.FirstName = objAdminModel.FirstName;
                Admin.LastName = objAdminModel.LastName;
                Admin.Email = objAdminModel.Email;
                Admin.InsertUserId = InsertedUserID;
                string result = AdminService.EditAdmin(Admin).Result;
                if (result == "Success")
               {
                    Session["Name"] = objAdminModel.FirstName + " " + objAdminModel.LastName;
                    //    var ChattingController = new ChattingController();

                    //    if (objAdminModel.OldEmail != objAdminModel.Email)
                    //    {
                    //        Task.Run(async () => { await ChattingController.ApiToChangeEmailToQuickblox(objAdminModel.OldEmail, objAdminModel.Email, objAdminModel.QuickBloxId); }).Wait();
                    //    }

                    TempData["Message"] = "Data updated successfully.";
                    return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["Message"] = "Failed to update.";
                    return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
                }

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
                return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult UpdateSchedulerProfile()
        {
            try
            {

                Scheduler objSchedulerModel = new Scheduler();
                SchedulerServiceProxy SchedulerService = new SchedulerServiceProxy();
                string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
                string json = System.Web.HttpContext.Current.Request["ObjSchedular"];
                objSchedulerModel = JsonConvert.DeserializeObject<SchedulerServiceProxy>(json).SchedulerDetail;
                   

                var Scheduler = new Scheduler();
                Scheduler.SchedulerId = objSchedulerModel.SchedulerId;

                Scheduler.FirstName = objSchedulerModel.FirstName;
                Scheduler.LastName = objSchedulerModel.LastName;
                Scheduler.Email = objSchedulerModel.Email;

            
                Scheduler.InsertUserId = InsertedUserID;


                string result = SchedulerService.EditScheduler(Scheduler).Result;
                if (result == "Success")
                {
                    Session["Name"] = objSchedulerModel.FirstName + " " + objSchedulerModel.LastName;
                    //var ChattingController = new ChattingController();

                    //if (objSchedulerModel.OldEmail != objSchedulerModel.Email)
                    //{

                    //    Task.Run(async () => { await ChattingController.ApiToChangeEmailToQuickblox(objSchedulerModel.OldEmail, objSchedulerModel.Email, objSchedulerModel.QuickBloxId); }).Wait();

                    //}

                    TempData["Message"] = "Data updated successfully.";
                    return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
                }



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
                return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public Userprofile getAdminDetail(string role ,string userid)
        {
            Userprofile userprofile = new Userprofile();
            userprofile.ObjSchedulerModel = new SchedulerModel();
            userprofile.ObjAdminModelModel = new AdminModel();
            AdminModel objAdminModel = new AdminModel();
            AccountController Accountctrl = new AccountController();
            try
            {            
              if (role == "SuperAdmin" || role == "OrgSuperAdmin")
               {             
                   SchedulerModel objSchedulerModel = new SchedulerModel(); 
             
                   objSchedulerModel = Accountctrl.GetSchedulerDetailByUserId(userid);
                   if (objSchedulerModel.SchedulerId == 0)
                   { 
                       objAdminModel = Accountctrl.GetAdminDetailByUserId(userid); 
                       userprofile.ObjAdminModelModel.FirstName = objAdminModel.FirstName;
                       userprofile.ObjAdminModelModel.LastName = objAdminModel.LastName;
                       userprofile.ObjAdminModelModel.Email = objAdminModel.Email; 
                   }
                   else
                   {
                       userprofile.ObjSchedulerModel.FirstName = objSchedulerModel.FirstName;
                       userprofile.ObjSchedulerModel.LastName = objSchedulerModel.LastName;
                       userprofile.ObjSchedulerModel.Email = objSchedulerModel.Email;
                   }
               }
            }
            catch (Exception ex)
            {


            }

            return userprofile;
        }
     }
    public class Userprofile
    {
        public CareGiverModel ObjCaregiverModel { get; set; }
        public AdminModel ObjAdminModelModel { get; set; }
        public SchedulerModel ObjSchedulerModel { get; set; }
        public NurseCoordinatorModel ObjNurseCoordinatorModel { get; set; }




    }
}