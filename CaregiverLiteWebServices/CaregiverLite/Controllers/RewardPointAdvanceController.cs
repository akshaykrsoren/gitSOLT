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

namespace CaregiverLite.Controllers
{
    public class RewardPointAdvanceController : Controller
    {
        // GET: RewardPointAdvance
        public ActionResult RewardPointAdvance()
        {
            FillAllOffices();
            return View();
        }
        public ActionResult GetRewardPointAdvanceList(JQueryDataTableParamModel param)
        {
            var RewardPointsList = new RewardPointsList();
            try
            {

                int FilterOffice = 0;
                if (Convert.ToInt32(Request["FilterOffice"]) != 0) //Request["FilterOffice"] != null && Request["FilterOffice"] != "" &&
                {
                    FilterOffice = Convert.ToInt32(Request["FilterOffice"]);

                    if (FilterOffice == 0)//if (FilterOffice == "All")
                    {
                        FilterOffice = 0; //FilterOffice = "||";
                    }

                }
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
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "CompletedReqCount";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "TotalRewardPoint";
                }

                else if (sortColumnIndex == 5)
                {
                    sortOrder = "Office";
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
                string LogInUserId = Membership.GetUser().ProviderUserKey.ToString();
                var RewardPointLiteService = new RewardPointServiceProxy();
                RewardPointsList = RewardPointLiteService.GetAllRewardPointAdvanceDetails(pageNo, recordPerPage, search, sortOrder, sortDirection,FilterOffice, LogInUserId).Result;
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "RewardPointController";
                log.Methodname = "GetRewardPointList";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            if (RewardPointsList.objRewardPointList != null)
            {
                var result = from C in RewardPointsList.objRewardPointList select new[] { C, C, C, C, C, C, C, C, C ,C};
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = RewardPointsList.TotalNumberofRecord,
                    iTotalDisplayRecords = RewardPointsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = RewardPointsList.TotalNumberofRecord,
                    iTotalDisplayRecords = RewardPointsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public string EditComment(string NurseId, string Comment)
        {
            string result = "";
            try
            {
               
                var RewardPointService = new RewardPointServiceProxy();
                result = RewardPointService.EditComment(NurseId,Comment).Result;
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

        private void FillAllOffices()
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try 
            { 
            string logInUserId = Membership.GetUser().ProviderUserKey.ToString();
            OfficeServiceProxy officeServiceProxy = new OfficeServiceProxy();
            var lstOffices = officeServiceProxy.GetAllOffices(logInUserId,OrganisationId.ToString()).Result;
            lstOffices.Insert(0, new Office() { OfficeId = 0, OfficeName = "All" });
            SelectList officeSelectList = new SelectList(lstOffices, "OfficeId", "OfficeName");
            ViewBag.lstOffice = officeSelectList;
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
        }
    }
}