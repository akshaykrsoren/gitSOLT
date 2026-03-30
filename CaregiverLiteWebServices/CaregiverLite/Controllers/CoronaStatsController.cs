using CaregiverLiteWCF.Class;
using CaregiverLiteWCF;
using CaregiverLite.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using CaregiverLite.Action_Filters;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CaregiverLite.Controllers
{
    public class CoronaStatsController : Controller
    {
        // GET: CoronaStats
        // [HttpGet]
        [SessionExpire]
        public ActionResult CoronaStats()
        {
            FillAllOffices();
            return View();
        }
        


       // [HttpPost]
        public ActionResult GetCoronaStatsList(JQueryDataTableParamModel param)
        {

            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            CornaStatsDetailsList CornaStatsDetailsList = new CornaStatsDetailsList();
            try
            {
                int FilterOfficeId = 0;

                if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                if (sortColumnIndex == 0)
                {
                    sortOrder = "NurseName";
                }
                else if (sortColumnIndex == 1)
                {
                    sortOrder = "NurseName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "NurseName";
                }
                else if (sortColumnIndex == 3)
                {
                    sortOrder = "NurseName";
                }
                else if (sortColumnIndex == 4)
                {
                    sortOrder = "NurseName";
                }
                else
                {
                    sortOrder = "NurseName";
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

                CoronaStatsDetailsServiceProxy CoronaStatsDetailsLiteService = new CoronaStatsDetailsServiceProxy();

                CornaStatsDetailsList = CoronaStatsDetailsLiteService.GetAllCornaStatsDetails(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId).Result;

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
            if (CornaStatsDetailsList.VitalCornaStatsDetailsList != null)
            {
                var result = from C in CornaStatsDetailsList.VitalCornaStatsDetailsList select new[] { C, C, C, C, C, C, C, C, C,C};
              //  var result = CornaStatsDetailsList.VitalCornaStatsDetailsList;
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = CornaStatsDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = CornaStatsDetailsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = CornaStatsDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = CornaStatsDetailsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public string AvailabeHealthStatusForLogin(int BodySymptomsId, int ActiveStatus)
        {
            string result = "";
            int i;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand("GetBodySymptomsActiveForTemparature", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BodySymptomsId", BodySymptomsId);
            cmd.Parameters.AddWithValue("@ActiveStatus", ActiveStatus);
            i = cmd.ExecuteNonQuery();
            if (i == 1)
            {
                result = "success";
            }

            return result;
        }

        //[HttpPost]
        // public string AvailabeHealthStatusForLogin(int BodySymptomsId,int ActiveStatus)
        // {
        //    string result = "";
        //    int i;

        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("GetBodySymptomsActiveForTemparature", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@BodySymptomsId", BodySymptomsId);
        //    cmd.Parameters.AddWithValue("@ActiveStatus", ActiveStatus);
        //    i=cmd.ExecuteNonQuery();
        //    if (i == 1)
        //    {
        //        result = "success";

        //    }

        //    return result;
        //}

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
    }
}