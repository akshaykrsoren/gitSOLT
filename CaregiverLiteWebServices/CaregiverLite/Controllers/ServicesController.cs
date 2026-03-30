using CaregiverLite.Action_Filters;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class ServicesController : Controller
    {
        // GET: Service
        public ActionResult Services()
        {
            try
            {

                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                List<CaregiverLiteWCF.Services> ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
                ViewBag.ServiceList = ServiceList;
                VisitTypeController visitTypeCTRL = new VisitTypeController();              
                List<CaregiverLiteWCF.VisitType> VisitTypeList = visitTypeCTRL.GetAllVisits(Convert.ToInt32(OrganisationId));
                ViewBag.VisitTypeList = VisitTypeList;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ServicesController";
                log.Methodname = "Services";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Services(ServiceVisitModel objServiceModel)
        {
            string result = "";
            try
            {
                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                VisitTypeController visitTypeCTRL = new VisitTypeController();
                List<CaregiverLiteWCF.VisitType> VisitTypeList = visitTypeCTRL.GetAllVisits(Convert.ToInt32(OrganisationId));
                ViewBag.VisitTypeList = VisitTypeList;
                if (ModelState.IsValid)
                {
                    Services objService = new CaregiverLiteWCF.Services();

                    objService.ServiceId = Convert.ToInt32(objServiceModel.ServiceId);
                    objService.ServiceName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objServiceModel.ServiceName);

                    objService.UserId = Membership.GetUser().ProviderUserKey.ToString();
                    objService.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                    result = ServicesService.InsertUpdateService(objService).Result;
                    int n;
                    bool isNumeric = int.TryParse(result, out n);
                    if (isNumeric)
                    {
                        TempData["message"] = "Service added successfully.";
                        return RedirectToAction("Services", "Services");
                    }
                    else
                    {
                        List<CaregiverLiteWCF.Services> ServiceList = ServicesService.GetAllServices(Convert.ToString(objService.OrganisationId)).Result;
                        ViewBag.ServiceList = ServiceList;
                        ViewBag.Error = result;
                    }
                   
                }
                else
                {
                    List<CaregiverLiteWCF.Services> ServiceList = ServicesService.GetAllServices(Convert.ToString(OrganisationId)).Result;
                    ViewBag.ServiceList = ServiceList;
                   
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ServicesController";
                log.Methodname = "[HTTPPOST] Services";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return View();
        }

        public string UpdateService(string ServiceId, string ServiceName, string Description)
        {
            string result = "";
            try
            {
                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                Services objService = new CaregiverLiteWCF.Services();

                objService.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                objService.ServiceId = Convert.ToInt32(ServiceId);
                objService.ServiceName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ServiceName);
                objService.Description = Description;
                objService.UserId = Membership.GetUser().ProviderUserKey.ToString();
                result = ServicesService.InsertUpdateService(objService).Result;                           
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ServicesController";
                log.Methodname = "UpdateService";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        public string DeleteService(string ServiceId)
        {
            string result = "";
            try
            {
                ServicesServiceProxy ServicesService = new ServicesServiceProxy();
                result = ServicesService.DeleteService(ServiceId, Membership.GetUser().ProviderUserKey.ToString()).Result;                
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "ServicesController";
                log.Methodname = "UpdateService";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }
    }
}