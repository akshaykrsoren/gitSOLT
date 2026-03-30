using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaregiverLiteWCF.Class;
using CaregiverLiteWCF;
using CaregiverLite.Models;
using Newtonsoft.Json.Linq;
using DifferenzLibrary;
using System.Web.Security;
using CaregiverLite.Action_Filters;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class VisitTypeController : Controller
    {
        // GET: VisitType

        public ActionResult VisitList()
        {

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {

                List<CaregiverLiteWCF.VisitType> VisitTypeList = GetAllVisits(Convert.ToInt32(OrganisationId));
                ViewBag.VisitTypeList = VisitTypeList;

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllServices";

            }

            return View();
        }
        

        public List<VisitType> GetAllVisits(int OrganisationId)
        {
            List<VisitType> VisitTypeList = new List<VisitType>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllVisitType", OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        VisitType objVisits = new VisitType();
                        objVisits.VisitTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["VisitTypeId"]);
                        objVisits.VisitTypeName = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();
                        objVisits.VisitTypeId = ds.Tables[0].Rows[i]["VisitTypeName"].ToString();
                        objVisits.Description = ds.Tables[0].Rows[i]["Description"].ToString();

                        VisitTypeList.Add(objVisits);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllServices";
                string result = InsertErrorLog(objErrorlog);
            }
            return VisitTypeList;
        }


        [HttpPost]
        public ActionResult VisitTypes(VisitTypeModel objVisitTypeModel)
        {
            string result = "";
            try
            {
                
                int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                if (ModelState.IsValid)
                {
                    VisitType objVisits = new CaregiverLiteWCF.VisitType();

                    objVisits.VisitTypeId = Convert.ToInt32(objVisitTypeModel.VisitTypeId);
                    objVisits.VisitTypeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objVisitTypeModel.VisitTypeName);

                    string UserId = Membership.GetUser().ProviderUserKey.ToString();
                     //int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                    result = InsertUpdateVisitType(objVisits, UserId, OrganisationId);
                    int n;
                    bool isNumeric = int.TryParse(result, out n);
                    if (isNumeric)
                    {
                        TempData["message"] = "Service added successfully.";
                        return RedirectToAction("Services", "Services");
                    }
                    else
                    {
                        List<CaregiverLiteWCF.VisitType> VisitTypeList = GetAllVisits(OrganisationId);
                        ViewBag.ServiceList = VisitTypeList;
                        ViewBag.Error = result;
                    }
                }
                else
                {
                    List<CaregiverLiteWCF.VisitType> VisitTypeList = GetAllVisits(OrganisationId);
                    ViewBag.ServiceList = VisitTypeList;
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "VisitTypeController";
                log.Methodname = "[HTTPPOST] visitts";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return View();
        }


        public string InsertUpdateVisitType(VisitType VisitType,string UserId, int OrganisationId)
        {
            string result = "";


            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateVisitType",
                                                    VisitType.VisitTypeId,
                                                    VisitType.VisitTypeName,
                                                    new Guid(UserId),
                                                    Convert.ToInt32(OrganisationId),
                                                    VisitType.Description);
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
                objErrorlog.Pagename = "VisitTypeController";
                objErrorlog.Methodname = "InsertUpdateVisitType";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string DeleteVisitType(string VisitTypeId)
        {
            string result = "";

            string UserId = Membership.GetUser().ProviderUserKey.ToString();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteVisitType",
                                                        VisitTypeId,
                                                        new Guid(UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "VisitTypeController";
                objErrorlog.Methodname = "DeleteVisits";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public string UpdateVisitType(string VisitTypeId, string VisitTypeName,string Description)
        {
            string result = "";
            try
            {           
                 VisitType objVisitType = new CaregiverLiteWCF.VisitType();
              
                 int  OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                 objVisitType.VisitTypeId = Convert.ToInt32(VisitTypeId);
                 objVisitType.VisitTypeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(VisitTypeName);
                 objVisitType.Description = Description;
                 string UserId = Membership.GetUser().ProviderUserKey.ToString();
                 result = InsertUpdateVisitType(objVisitType,UserId,OrganisationId);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "VisitTypeController";
                log.Methodname = "UpdateVisitType";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
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
               // result = InsertErrorLog(objErrorlog);
            }
            return result;
        }




    }
}