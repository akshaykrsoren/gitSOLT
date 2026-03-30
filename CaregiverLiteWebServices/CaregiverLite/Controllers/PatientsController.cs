using CaregiverLiteWCF.Class;
using CaregiverLiteWCF;
using CaregiverLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CaregiverLite.Action_Filters;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using Excel;
using OfficeOpenXml;
using System.Configuration;
using System.Threading.Tasks;
using DifferenzLibrary;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

//using Excel1 = Microsoft.Office.Interop.Excel;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class PatientsController : Controller
    {

        // GET: Patients
        public ActionResult Patients()
        {
            FillAllOffices();
            FillAllOrganisations();
            return View();
        }

        public ActionResult GetPatientDetailsList(JQueryDataTableParamModel param)
        {
            string UserID = Membership.GetUser().ProviderUserKey.ToString();
            PatientDetailsList PatientDetailsList = new PatientDetailsList();
            string FilterActiveStatus = "";
            string IsActiveStatus = "";
            try
            {
                int FilterOfficeId = 0;

                if (!string.IsNullOrEmpty(Request["FilterOfficeId"]))
                    FilterOfficeId = Convert.ToInt32(Request["FilterOfficeId"]);

                string sortOrder = string.Empty;
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                if (Request["FilterActiveStatus"] != null && Request["FilterActiveStatus"] != "")
                {
                    IsActiveStatus = Request["FilterActiveStatus"];
                }


                if (Request["FilterActiveStatus"] != null && Request["FilterActiveStatus"] != "")
                {
                    IsActiveStatus = Request["FilterActiveStatus"];
                }


                //if (sortColumnIndex == 0)
                //{
                //    sortOrder = "FirstName";
                //}
                //if (sortColumnIndex == 1)
                //{
                //    sortOrder = "LastName";
                //}
                //else if (sortColumnIndex == 2)
                //{
                //    sortOrder = "MedicalId";
                //}
                //else if (sortColumnIndex == 3)
                //{
                //    sortOrder = "Street";
                //}
                //else if (sortColumnIndex == 4)
                //{
                //    sortOrder = "City";
                //}
                //else if (sortColumnIndex == 5)
                //{
                //    sortOrder = "State";
                //}
                //else if (sortColumnIndex == 6)
                //{
                //    sortOrder = "ZipCode";
                //}
                //else if (sortColumnIndex == 7)
                //{
                //    sortOrder = "PhoneNo";
                //}
                //else if (sortColumnIndex == 8)
                //{
                //    sortOrder = "PrimaryMD";
                //}
                //else if (sortColumnIndex == 9)
                //{
                //    sortOrder = "OfficeName";
                //}
                //else
                //{
                //    //sortOrder = "PatientName";

                //}




                if (sortColumnIndex == 0)
                {
                    sortOrder = "FirstName";
                }
                if (sortColumnIndex == 1)
                {
                    sortOrder = "LastName";
                }
                else if (sortColumnIndex == 2)
                {
                    sortOrder = "MedicalId";
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
                    sortOrder = "ZipCode";
                }
                else if (sortColumnIndex == 7)
                {
                    sortOrder = "PhoneNo";
                }
                else if (sortColumnIndex == 8)
                {
                    sortOrder = "PrimaryMD";
                }
                else if (sortColumnIndex == 9)
                {
                    sortOrder = "OfficeName";
                }
                else
                {
                    //sortOrder = "PatientName";

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


                PatientDetailsServiceProxy PatientDetailLiteService = new PatientDetailsServiceProxy();
       
                PatientDetailsList = PatientDetailLiteService.GetAllPatientDetail(UserID, pageNo, recordPerPage, search, sortOrder, sortDirection, FilterOfficeId, OrganisationId, IsActiveStatus).Result;
                

                
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

            if (PatientDetailsList.PatientList != null)
            {
                var result = from C in PatientDetailsList.PatientList select new[] { C, C, C, C, C, C, C, C, C, C,C,C };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = PatientDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = PatientDetailsList.FilteredRecord,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = PatientDetailsList.TotalNumberofRecord,
                    iTotalDisplayRecords = PatientDetailsList.FilteredRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }
        


        
        private void FillAllOffices(object SelectedValue = null)
        {
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
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
            catch (Exception ex)
            {
                Response.Redirect("~/Account/SessionTimeout");

            }
       
        }




        //private void GetAllPayerId(object SelectedValue = null)
        //{
        //    var lstPayerId = GetAllAvailablePayerId();
        //    SelectList PayerSelectList = new SelectList(lstPayerId, "PayerProgramsID", "PayerID", SelectedValue);
        //    ViewBag.PayerSelectList = PayerSelectList;
        //}


        private void GetAllPayerProgram(string PayerId,object SelectedValue = null)
        {

            var lstPayerProgram = GetAllAvailablePayerProgramList(PayerId);

            foreach(var items in lstPayerProgram)
            {
                if (items.PayerID.Equals(SelectedValue.ToString()))
                {
                    items.PayerProgramsID = 1;
                }            
            }


            //var lstpayerprograms=lstPayerProgram.

            //var lstPayerProgram = listPayerProgram.ToList();


            string y = "";
            try
            {
                 y = lstPayerProgram.Find(x => x.PayerID == SelectedValue.ToString()).PayerProgramsID.ToString();
            }
            catch(Exception ex)
            {

            }
            SelectList PayerProgramSelectList = new SelectList(lstPayerProgram, "PayerProgramsID", "PayerID", y);
            ViewBag.PayerProgram = PayerProgramSelectList;

            ViewBag.PayerprogramIDss = y;


        }

        private void GetAllPRocedureCode(string PayerId,string PayerProgram, object SelectedValue = null)
        {
            string ArrayValue = SelectedValue.ToString().Split(',')[0];

            var lstProcedureCode =GetAllAvailableProcedureCodeList(PayerId, PayerProgram);

            var y = lstProcedureCode.Find(zx => zx.PayerID == ArrayValue.ToString()).PayerProgramsID.ToString();

            SelectList lstProcedureCodeSelectList = new SelectList(lstProcedureCode, "PayerProgramsID", "PayerID", y);
            ViewBag.lstProcedureCodeSelectList = lstProcedureCodeSelectList;

            ViewBag.ProcedureCodeIDss = y;
            
        }

        private void GetAllJurisdictionCode(string PayerId ,object SelectedValue = null)
        {

            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            if (OrganisationId == 0)
            {
                var lstJurisdictionCode = GetAllAvailableJurisdictionEntitiesList(PayerId);

                var y = lstJurisdictionCode.Count > 0 ? lstJurisdictionCode.Find(zx => zx.PayerID.Contains(SelectedValue.ToString())).PayerProgramsID.ToString() : null;

                // var y = lstJurisdictionCode.Find(zx => zx.PayerID == SelectedValue.ToString()).PayerProgramsID.ToString();

                SelectList lstJurisdictionCodeSelectList = new SelectList(lstJurisdictionCode, "PayerProgramsID", "PayerID", y);
                ViewBag.lstJurisdictionCodeSelectList = lstJurisdictionCodeSelectList;

                ViewBag.JurisdictionCode = y;
            }
        }


        public List<PayerProgram> GetAllAvailablePayerId()
        {
            List<PayerProgram> PayerProgramLists = new List<PayerProgram>();
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerId",OrganisationId);

               // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerId");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                   // List<PayerProgram> PayerProgramList = new List<PayerProgram>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PayerProgram objPayerProgram = new PayerProgram();
                        objPayerProgram.PayerProgramsID = Convert.ToInt32(ds.Tables[0].Rows[i]["PayerProgramsID"].ToString());
                        objPayerProgram.PayerID = ds.Tables[0].Rows[i]["PayerID"].ToString();

                        PayerProgramLists.Add(objPayerProgram);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailablePayerId";
                //string result = InsertErrorLog(objErrorlog);
            }
            return PayerProgramLists;
        }


        public List<PayerProgram> GetAllAvailablePayerbyOfficeId(int OfficeId)
        {
            List<PayerProgram> PayerProgramLists = new List<PayerProgram>();
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            try
            {
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerId", OrganisationId);

                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerId");

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerIdByOfficeId", OfficeId, OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // List<PayerProgram> PayerProgramList = new List<PayerProgram>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PayerProgram objPayerProgram = new PayerProgram();
                        objPayerProgram.PayerProgramsID = Convert.ToInt32(ds.Tables[0].Rows[i]["PayerProgramsID"].ToString());
                        objPayerProgram.PayerID = ds.Tables[0].Rows[i]["PayerID"].ToString();

                        PayerProgramLists.Add(objPayerProgram);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailablePayerId";
                //string result = InsertErrorLog(objErrorlog);
            }
            return PayerProgramLists;
        }


        public JsonResult GetAllAvailablePayerIdByOfficeId(string OfficeId)
        {
            
            int OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
            List<SelectListItem> listItem = new List<SelectListItem>();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerIdByOfficeId", OfficeId, OrganisationId);

               //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerId");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listItem.Add(new SelectListItem
                        {
                            Text = ds.Tables[0].Rows[i]["PayerID"].ToString(),
                            Value = ds.Tables[0].Rows[i]["PayerProgramsID"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailablePayerId";
                //string result = InsertErrorLog(objErrorlog);
            }

            return Json(listItem, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllAvailablePayerProgram(string PayerId, string OfficeId)
        {
            List<PayerProgram> ProgramLists = new List<PayerProgram>();

            List<SelectListItem> listItem = new List<SelectListItem>();

            try
            {
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerProgramByPayerID",PayerId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerProgramByPayerIDByOfficeId", PayerId, OfficeId);
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                 
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listItem.Add(new SelectListItem
                        {
                            Text = ds.Tables[0].Rows[i]["ProgramsID"].ToString(),
                            Value = ds.Tables[0].Rows[i]["PayerProgramsID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOfficesList";
                //string result = InsertErrorLog(objErrorlog);
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllAvailableProcedureCode(string PayerId, string PayerProgram)
        {
          
            List<SelectListItem> listItem = new List<SelectListItem>();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllProcedureCodeByPayerIdWithPayerProgram", PayerId,PayerProgram);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // List<PayerProgram> ProgramList = new List<PayerProgram>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        listItem.Add(new SelectListItem
                        {
                            Text = ds.Tables[0].Rows[i]["HCProcedureCode"].ToString(),
                            Value = ds.Tables[0].Rows[i]["ServiceModifiers"].ToString()

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOfficesList";
                //string result = InsertErrorLog(objErrorlog);
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllAvailableJurisdictionEntities(string PayerId)
        {
            List<PayerProgram> ProgramLists = new List<PayerProgram>();
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllJurisdictionalEntitiesByPayerId", PayerId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listItem.Add(new SelectListItem
                        {
                            Text = ds.Tables[0].Rows[i]["JurisdictionalEntitiesID"].ToString(),
                            Value = ds.Tables[0].Rows[i]["JurisdictionalId"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOfficesList";
                //string result = InsertErrorLog(objErrorlog);
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public List<PayerProgram> GetAllAvailablePayerProgramList(string PayerId)
        {
            List<PayerProgram> ProgramLists = new List<PayerProgram>();

            List<SelectListItem> listItem = new List<SelectListItem>();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPayerProgramByPayerID", PayerId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // List<PayerProgram> ProgramList = new List<PayerProgram>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PayerProgram objPayerProgram = new PayerProgram();
                        objPayerProgram.PayerProgramsID = int.Parse(ds.Tables[0].Rows[i]["PayerProgramsID"].ToString());
                        objPayerProgram.PayerID = ds.Tables[0].Rows[i]["ProgramsID"].ToString();

                        ProgramLists.Add(objPayerProgram);
                    }
                }


                //foreach (var Programlist in ProgramLists)
                //{
                //    listItem.Add(new SelectListItem
                //    {
                //        Text = Programlist.PayerID,
                //        Value = (Programlist.PayerProgramsID).ToString(),


                //    });
                //}



            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOfficesList";
                //string result = InsertErrorLog(objErrorlog);
            }
            // return Json(listItem, JsonRequestBehavior.AllowGet);
            return ProgramLists;
        }


        public List<PayerProgram> GetAllAvailableProcedureCodeList(string PayerId, string PayerProgram)
        {
            List<PayerProgram> ProgramLists = new List<PayerProgram>();
            List<SelectListItem> listItem = new List<SelectListItem>();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllProcedureCodeByPayerIdWithPayerProgram", PayerId, PayerProgram);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // List<PayerProgram> ProgramList = new List<PayerProgram>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PayerProgram objPayerProgram = new PayerProgram();
                        objPayerProgram.PayerProgramsID = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceModifiers"].ToString());
                        objPayerProgram.PayerID = ds.Tables[0].Rows[i]["HCProcedureCode"].ToString();

                        ProgramLists.Add(objPayerProgram);
                    }
                }

                //foreach (var Programlist in ProgramLists)
                //{
                //    listItem.Add(new SelectListItem
                //    {
                //        Text = Programlist.PayerID,
                //        Value = (Programlist.PayerProgramsID).ToString(),

                //    });
                //}
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOfficesList";
                //string result = InsertErrorLog(objErrorlog);
            }
            // return Json(listItem, JsonRequestBehavior.AllowGet);

            return ProgramLists;
        }


        public List<PayerProgram> GetAllAvailableJurisdictionEntitiesList(string PayerId)
        {
            List<PayerProgram> ProgramLists = new List<PayerProgram>();
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllJurisdictionalEntitiesByPayerId", PayerId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // List<PayerProgram> ProgramList = new List<PayerProgram>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PayerProgram objPayerProgram = new PayerProgram();
                        objPayerProgram.PayerProgramsID = Convert.ToInt32(ds.Tables[0].Rows[i]["JurisdictionalId"].ToString());
                        objPayerProgram.PayerID = ds.Tables[0].Rows[i]["JurisdictionalEntitiesID"].ToString();

                        ProgramLists.Add(objPayerProgram);
                    }
                }


                //foreach (var Programlist in ProgramLists)
                //{
                //    listItem.Add(new SelectListItem
                //    {
                //        Text = Programlist.PayerID,
                //        Value = (Programlist.PayerProgramsID).ToString(),

                //    });
                //}
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOfficesList";
                //string result = InsertErrorLog(objErrorlog);
            }
            // return Json(listItem, JsonRequestBehavior.AllowGet);
            return ProgramLists;
        }

        public ActionResult AddPatient()
        {
            var patientDetailsModel = new PatientDetailsModel();

            //PatientDetailsServiceProxy PatientDetailsService = new PatientDetailsServiceProxy();
            //ViewBag.ServiceList = PatientDetailsService.GetAllServices().Result;
            FillAllOffices();
            object SelectedValue = null;

            // var lstPayerProgram = GetAllAvailablePayerId();
            // SelectList PayerProgramSelectList = new SelectList(lstPayerProgram, "PayerProgramsID", "PayerID", SelectedValue);
            // ViewBag.lstPayerProgram = PayerProgramSelectList;

            

            patientDetailsModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
            patientDetailsModel.PayerIDSelectList = ViewBag.lstPayerProgram as IEnumerable<SelectListItem>;

            patientDetailsModel.PayerIDSelectList = GetPayerId();
            patientDetailsModel.PayerProgramSelectList = GetPayerProgram();
            patientDetailsModel.ProcedureCodeSelectList = GetProcedureCodeSelectList();
            patientDetailsModel.JurisdictionEntitiesSelectList = GetJurisdictioEntitiesSelectList();
            patientDetailsModel.OrganisationId= Convert.ToInt32(Session["OrganisationId"]);

            ViewBag.lstOfOfficesSandData = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();
            ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

            return PartialView(patientDetailsModel);
        }


        public IEnumerable<SelectListItem> GetPayerId()
        {
            List<SelectListItem> PayerProgram = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "Select",
                    Disabled=true
                }
            };
            return PayerProgram;
        }


        public IEnumerable<SelectListItem> GetPayerProgram()
        {
            List<SelectListItem> PayerProgram = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "Select",
                    Disabled=true
                }
            };
            return PayerProgram;
        }


        public IEnumerable<SelectListItem> GetProcedureCodeSelectList()
        {
            List<SelectListItem> ProcedureCodeList = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "Select",
                    Disabled=true
                }
            };
            return ProcedureCodeList;
        }


        public IEnumerable<SelectListItem> GetJurisdictioEntitiesSelectList()
        {
            List<SelectListItem> JurisdictioEntitiesList = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "Select",
                    Disabled=true
                }
            };
            return JurisdictioEntitiesList;
        }


        [HttpPost]
        public ActionResult AddPatientDetails(PatientDetailsModel objPatientDetails)
        {
            var PatientDetail = new PatientsDetail();

            //DataTable dt = new DataTable();
            
            string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
            try
            {
                string json = System.Web.HttpContext.Current.Request["ObjPatient"];
                if (json !=null || json != "")
                {
                    PatientDetailsServiceProxy PatientDetailsService = new PatientDetailsServiceProxy();

                     json = System.Web.HttpContext.Current.Request["ObjPatient"];
                    objPatientDetails = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).PatientModelDetail;

                    //SchedulePatientRequestServiceProxy CareGiverLiteService = new SchedulePatientRequestServiceProxy();
                    PatientDetail.PatientId = objPatientDetails.PatientId;
                    PatientDetail.PatientName = objPatientDetails.PatientName;
                    PatientDetail.FirstName = objPatientDetails.FirstName;
                    PatientDetail.LastName = objPatientDetails.LastName;
                    PatientDetail.MedicalId = objPatientDetails.MedicalId;
                    PatientDetail.Email = objPatientDetails.Email;
                    PatientDetail.PhoneNo = objPatientDetails.PhoneNo;
                    //PatientDetail.Address = objPatientDetails.Address;
                    PatientDetail.Street = objPatientDetails.Street;
                    PatientDetail.City = objPatientDetails.City;
                    PatientDetail.State = objPatientDetails.State;
                    PatientDetail.Latitude = objPatientDetails.Latitude;
                    PatientDetail.Longitude = objPatientDetails.Longitude;
                    PatientDetail.ZipCode = objPatientDetails.ZipCode;
                    PatientDetail.InsertUserId = Membership.GetUser().ProviderUserKey.ToString();
                    PatientDetail.TimezoneId = objPatientDetails.TimezoneId;
                    PatientDetail.TimezoneOffset = objPatientDetails.TimezoneOffset;
                    PatientDetail.TimezonePostfix = objPatientDetails.TimezonePostfix;
                    PatientDetail.PrimaryMD = objPatientDetails.PrimaryMD;
                    PatientDetail.OfficeId = objPatientDetails.OfficeId;
                    PatientDetail.DateOfBirth = objPatientDetails.DateOfBirth==""?null: objPatientDetails.DateOfBirth;
                    PatientDetail.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                    PatientDetail.PayerId = objPatientDetails.PayerId == "Select" ? null : objPatientDetails.PayerId;
                    PatientDetail.PayerProgram = objPatientDetails.PayerProgram == "Select" ? null : objPatientDetails.PayerProgram;
                    PatientDetail.ClientPayerId = objPatientDetails.ClientPayerId == null ? null : objPatientDetails.ClientPayerId;
                    PatientDetail.ProcedureCode = objPatientDetails.ProcedureCode == "Select" ? null : objPatientDetails.ProcedureCode;
                    PatientDetail.JurisdictionCode = objPatientDetails.JurisdictionCode == "Select" ? null : objPatientDetails.JurisdictionCode;

                    //var PayerInformationlist = objPatientDetails.PayerInformations;

                    //dt.Columns.Add("PatientId");
                    //dt.Columns.Add("MedicalId");
                    //dt.Columns.Add("PayerId");
                    //dt.Columns.Add("PayerProgram");
                    //dt.Columns.Add("ProcedureCode");
                    //dt.Columns.Add("JurisdictionId");
                    //dt.Columns.Add("ClientPayerId");

                    //string result = PatientDetailsService.AddPatient(PatientDetail).Result;

                    string result = AddPatients(PatientDetail);

                    if (result == "Success")
                    {
                        if (PatientDetail.OrganisationId > 0)
                        {
                            if (PatientDetail.PayerId != null)
                            {
                                string Results = GetAndPostClientDataForAllMed(objPatientDetails.MedicalId);
                            }
                        }
                        else
                        {

                            if (PatientDetail.PayerId != null)
                            {
                                string Results = GetAndPostClientData(objPatientDetails.MedicalId);
                            }
                        }

                        TempData["Message"] = "Patient Details is Added successfully.";

                        return Json(result, JsonRequestBehavior.AllowGet);

                       // return RedirectToAction("Patients", "Patients", new { IsAdded = true });
                    }
                    else
                    {
                        TempData["error"] = true;
                        TempData["Message"] = result;// "Patient Details Not Added successfully.";
                        return RedirectToAction("Patients", "Patients");
                        //return PartialView("AddPatient", objPatientDetails);
                    }
                }
                else
                {
                    TempData["error"] = true;
                    
                    string messages = string.Join("; ", ModelState.Values
                                        .Where(x => x.Errors.Count > 0)
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    throw new Exception(messages);
                }
            }
            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "[HttpPost] AddPatientDetails";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                TempData["Message"] = "Patient Details not Added";
                return RedirectToAction("Patients", "Patients", new { IsAdded = false });
                //return PartialView("AddPatient", objPatientDetails);
            }
        }


        public string AddPatients(PatientsDetail PatientsDetail)
        {
            string result = "";
            try
            {
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddPatientWithSandData",
                //"AddPatient",
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_AddPatientWithSandData",
                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_AddPatientWithSandData",
                                                PatientsDetail.PatientId,
                                                    PatientsDetail.PatientName,
                                                    PatientsDetail.FirstName,
                                                    PatientsDetail.LastName,
                                                    PatientsDetail.MedicalId,
                                                    PatientsDetail.Email,
                                                    PatientsDetail.PhoneNo,
                                                    //PatientsDetail.Address,
                                                    PatientsDetail.Street,
                                                    PatientsDetail.City,
                                                    PatientsDetail.State,
                                                    PatientsDetail.Latitude,
                                                    PatientsDetail.Longitude,
                                                    PatientsDetail.ZipCode,
                                                    Guid.Parse(PatientsDetail.InsertUserId),
                                                    PatientsDetail.TimezoneId,
                                                    PatientsDetail.TimezoneOffset,
                                                    PatientsDetail.TimezonePostfix,
                                                    PatientsDetail.PrimaryMD,
                                                    PatientsDetail.OfficeId,
                                                    PatientsDetail.PayerId,
                                                    PatientsDetail.PayerProgram,
                                                    PatientsDetail.ClientPayerId,
                                                    PatientsDetail.ProcedureCode,
                                                    PatientsDetail.JurisdictionCode,
                                                    PatientsDetail.DateOfBirth,
                                                    PatientsDetail.OrganisationId
                                                    );

                if (ds.Tables[0].Rows[0][0].ToString() == "Success")
                {
                    result = "Success";
                }
                else
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddPatient";
                objErrorlog.UserID = PatientsDetail.InsertUserId;
                result = "Fail";
            }
            return result;
        }


        [HttpPost]
        public ActionResult EditPatientDetail(PatientDetailsModel objPatientDetails)
        {
            var PatientDetail = new PatientsDetail();

            string InsertedUserID = Membership.GetUser().ProviderUserKey.ToString();
            try
            {
                string json = System.Web.HttpContext.Current.Request["ObjPatient"];
                // PatientDetailsServiceProxy PatientDetailsService = new PatientDetailsServiceProxy();

                json = System.Web.HttpContext.Current.Request["ObjPatient"];
                objPatientDetails = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).PatientModelDetail;

                PatientDetailsServiceProxy PatientDetailsService = new PatientDetailsServiceProxy();
                //SchedulePatientRequestServiceProxy CareGiverLiteService = new SchedulePatientRequestServiceProxy();
                PatientDetail.PatientId = objPatientDetails.PatientId;
                PatientDetail.PatientName = objPatientDetails.PatientName;
                PatientDetail.FirstName = objPatientDetails.FirstName;
                PatientDetail.LastName = objPatientDetails.LastName;
                PatientDetail.MedicalId = objPatientDetails.MedicalId;
                PatientDetail.Email = objPatientDetails.Email;
                PatientDetail.PhoneNo = objPatientDetails.PhoneNo;

                //PatientDetail.Address = objPatientDetails.Address;

                PatientDetail.Street = objPatientDetails.Street;
                PatientDetail.City = objPatientDetails.City;
                PatientDetail.State = objPatientDetails.State;
                PatientDetail.Latitude = objPatientDetails.Latitude;
                PatientDetail.Longitude = objPatientDetails.Longitude;
                PatientDetail.ZipCode = objPatientDetails.ZipCode;
                PatientDetail.InsertUserId = Membership.GetUser().ProviderUserKey.ToString();
                PatientDetail.TimezoneId = objPatientDetails.TimezoneId;
                PatientDetail.TimezoneOffset = objPatientDetails.TimezoneOffset;
                PatientDetail.TimezonePostfix = objPatientDetails.TimezonePostfix;
                PatientDetail.PrimaryMD = objPatientDetails.PrimaryMD;
                PatientDetail.OfficeId = objPatientDetails.OfficeId;
                PatientDetail.DateOfBirth = objPatientDetails.DateOfBirth == "" ? null : objPatientDetails.DateOfBirth;
                PatientDetail.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                PatientDetail.PayerId = objPatientDetails.PayerId == "Select..." ? null : objPatientDetails.PayerId;
                PatientDetail.PayerProgram = objPatientDetails.PayerProgram == "Select" ? null : objPatientDetails.PayerProgram;
                PatientDetail.ClientPayerId = objPatientDetails.OfficeId==5 ? objPatientDetails.ClientPayerId==null ? null:objPatientDetails.ClientPayerId : objPatientDetails.ClientPayerId == null ? null : objPatientDetails.ClientPayerId;
                PatientDetail.ProcedureCode = objPatientDetails.ProcedureCode == "Select" ? null : objPatientDetails.ProcedureCode;
                PatientDetail.JurisdictionCode = objPatientDetails.JurisdictionCode == "Select" ? null : objPatientDetails.JurisdictionCode;

                //  string result=  PatientDetailsService.EditPatientDetails(PatientDetail).Result;

                string result = EditPatientDetails(PatientDetail);

                if (result == "Success")
                {

                    if (PatientDetail.OrganisationId > 0)
                    {
                        if (PatientDetail.PayerId != null)
                        {
                            if (PatientDetail.OrganisationId == 3)
                            {
                                string Results = GetAndPostClientDataForAllMed(objPatientDetails.MedicalId);
                            }
                        }
                    }
                    else
                    {
                        if (PatientDetail.PayerId != null)
                        {
                            string Results = GetAndPostClientData(objPatientDetails.MedicalId);
                        }
                    }

                    TempData["Message"] = "Patient Details is updated successfully.";

                    return Json(result, JsonRequestBehavior.AllowGet);

                    //string Results = GetAndPostClientData(objPatientDetails.MedicalId);
                    //TempData["Message"] = "Patient Details is updated successfully.";
                    //return RedirectToAction("Patients", "Patients", new { IsAdded = true });
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "[HttpPost] EditPatientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView();
        }


        public string EditPatientDetails(PatientsDetail PatientsDetail)
        {
            string result = "";
            try
            {
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdatePatientDetailsWithSandData",
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_UpdatePatientDetailsWithSandData",
                                                    //"UpdatePatientDetails",
                                                    PatientsDetail.PatientId,
                                                    PatientsDetail.PatientName,
                                                    PatientsDetail.FirstName,
                                                    PatientsDetail.LastName,
                                                    PatientsDetail.MedicalId,
                                                    PatientsDetail.Email,
                                                    PatientsDetail.PhoneNo,
                                                    PatientsDetail.Street,
                                                    PatientsDetail.City,
                                                    PatientsDetail.State,
                                                    PatientsDetail.Latitude,
                                                    PatientsDetail.Longitude,
                                                    PatientsDetail.ZipCode,
                                                    Guid.Parse(PatientsDetail.InsertUserId),
                                                    PatientsDetail.TimezoneId,
                                                    PatientsDetail.TimezoneOffset,
                                                    PatientsDetail.TimezonePostfix,
                                                    PatientsDetail.PrimaryMD,
                                                    PatientsDetail.OfficeId,
                                                    PatientsDetail.PayerId,
                                                    PatientsDetail.PayerProgram,
                                                    PatientsDetail.ClientPayerId,
                                                    PatientsDetail.ProcedureCode,
                                                    PatientsDetail.JurisdictionCode,
                                                    PatientsDetail.DateOfBirth
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
                objErrorlog.Methodname = "EditPatientDetails";
                objErrorlog.UserID = PatientsDetail.InsertUserId;
               // result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public ActionResult EditPatientDetail(string id)
        {
            PatientDetailsModel objModel = new PatientDetailsModel();
            try
            {
                objModel = GetPatientDetailById(id);

                FillAllOffices(objModel.OfficeId > 0 ? (object)objModel.OfficeId : null );
                objModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
                object SelectedValues = null;

                if (objModel.PayerId != null && objModel.PayerId != "")
                {
                    SelectedValues = objModel.PayerId;
                    var lstPayerProgram = GetAllAvailablePayerbyOfficeId(objModel.OfficeId);
                        //GetAllAvailablePayerId();
                    var x = lstPayerProgram.Find(z => z.PayerID == objModel.PayerId).PayerProgramsID.ToString();

                    SelectList PayerProgramSelectList = new SelectList(lstPayerProgram, "PayerProgramsID", "PayerID", Convert.ToInt32(x));
                    ViewBag.lstPayerProgram = PayerProgramSelectList;
                    objModel.PayerIDSelectList = ViewBag.lstPayerProgram as IEnumerable<SelectListItem>;
                    objModel.PayerIDs = Convert.ToInt32(x);

                    GetAllPayerProgram(x, objModel.PayerProgram);
                    objModel.PayerProgramSelectList = ViewBag.PayerProgram as IEnumerable<SelectListItem>;
                    objModel.PayerProgramIdss = Convert.ToInt32(ViewBag.PayerprogramIDss);

                    GetAllPRocedureCode(objModel.PayerId, objModel.PayerProgram, objModel.ProcedureCode);
                    objModel.ProcedureCodeSelectList = ViewBag.lstProcedureCodeSelectList as IEnumerable<SelectListItem>;
                    objModel.ProcedureCodeId = Convert.ToInt32(ViewBag.ProcedureCodeIDss);
                    ViewBag.ProcID = objModel.ProcedureCode;

                    GetAllJurisdictionCode(objModel.PayerId, objModel.JurisdictionCode);
                    objModel.JurisdictionEntitiesSelectList = ViewBag.lstJurisdictionCodeSelectList as IEnumerable<SelectListItem>;
                    objModel.JurisdictionCodeId = Convert.ToInt32(ViewBag.JurisdictionCode);
                    ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                    objModel.OrganisationId= Convert.ToInt32(Session["OrganisationId"]);

                    ViewBag.lstOfOfficesSandData = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();

                    ViewBag.DateOfBirths = objModel.DateOfBirth;
                }
                else
                {
                    SelectedValues = null;
                    var lstPayerProgram = GetAllAvailablePayerbyOfficeId(objModel.OfficeId);

                    //GetAllAvailablePayerId();

                    SelectList PayerProgramSelectList = new SelectList(lstPayerProgram, "PayerProgramsID", "PayerID", SelectedValues);
                    ViewBag.lstPayerProgram = PayerProgramSelectList;
                    objModel.PayerIDSelectList = ViewBag.lstPayerProgram as IEnumerable<SelectListItem>;
                    objModel.PayerProgramSelectList = GetPayerProgram();
                    objModel.ProcedureCodeSelectList = GetProcedureCodeSelectList();
                    objModel.JurisdictionEntitiesSelectList = GetJurisdictioEntitiesSelectList();
                    ViewBag.DateOfBirths = objModel.DateOfBirth;
                    ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                    objModel.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);


                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "EditPatientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }


        public ActionResult PatientProfileDetail(string id)
        {
            PatientDetailsModel objModel = new PatientDetailsModel();
            try
            {
                objModel = GetPatientDetailById(id);

                FillAllOffices(objModel.OfficeId > 0 ? (object)objModel.OfficeId : null);
                objModel.OfficeSelectList = ViewBag.lstOffice as IEnumerable<SelectListItem>;
                object SelectedValues = null;

                if (objModel.PayerId != null && objModel.PayerId != "")
                {
                    SelectedValues = objModel.PayerId;
                    var lstPayerProgram = GetAllAvailablePayerId();
                    var x = lstPayerProgram.Find(z => z.PayerID == objModel.PayerId).PayerProgramsID.ToString();

                    SelectList PayerProgramSelectList = new SelectList(lstPayerProgram, "PayerProgramsID", "PayerID", Convert.ToInt32(x));
                    ViewBag.lstPayerProgram = PayerProgramSelectList;
                    objModel.PayerIDSelectList = ViewBag.lstPayerProgram as IEnumerable<SelectListItem>;
                    objModel.PayerIDs = Convert.ToInt32(x);

                    GetAllPayerProgram(x, objModel.PayerProgram);
                    objModel.PayerProgramSelectList = ViewBag.PayerProgram as IEnumerable<SelectListItem>;
                    objModel.PayerProgramIdss = Convert.ToInt32(ViewBag.PayerprogramIDss);

                    GetAllPRocedureCode(objModel.PayerId, objModel.PayerProgram, objModel.ProcedureCode);
                    objModel.ProcedureCodeSelectList = ViewBag.lstProcedureCodeSelectList as IEnumerable<SelectListItem>;
                    objModel.ProcedureCodeId = Convert.ToInt32(ViewBag.ProcedureCodeIDss);
                    ViewBag.ProcID = objModel.ProcedureCode;

                    GetAllJurisdictionCode(objModel.PayerId, objModel.JurisdictionCode);
                    objModel.JurisdictionEntitiesSelectList = ViewBag.lstJurisdictionCodeSelectList as IEnumerable<SelectListItem>;
                    objModel.JurisdictionCodeId = Convert.ToInt32(ViewBag.JurisdictionCode);

                    ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                    objModel.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);

                    ViewBag.lstOfOfficesSandData = ConfigurationManager.AppSettings["OfficeIdSandData"].ToString();

                    ViewBag.DateOfBirths = objModel.DateOfBirth;
                }
                else
                {
                    SelectedValues = null;
                    var lstPayerProgram = GetAllAvailablePayerId();

                    SelectList PayerProgramSelectList = new SelectList(lstPayerProgram, "PayerProgramsID", "PayerID", SelectedValues);
                    ViewBag.lstPayerProgram = PayerProgramSelectList;
                    objModel.PayerIDSelectList = ViewBag.lstPayerProgram as IEnumerable<SelectListItem>;
                    objModel.PayerProgramSelectList = GetPayerProgram();
                    objModel.ProcedureCodeSelectList = GetProcedureCodeSelectList();
                    objModel.JurisdictionEntitiesSelectList = GetJurisdictioEntitiesSelectList();
                    ViewBag.DateOfBirths = objModel.DateOfBirth;

                    ViewBag.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                    objModel.OrganisationId = Convert.ToInt32(Session["OrganisationId"]);
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "GetPatientDetail";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return PartialView(objModel);
        }

        public PatientDetailsModel GetPatientDetailById(string id)
        {
            PatientDetailsModel objPatientDetails = new PatientDetailsModel();
            try
            {
                PatientDetailsServiceProxy CareGiverLiteService = new PatientDetailsServiceProxy();
                //PatientsDetail PatientsDetail1 = CareGiverLiteService.GetPatientDetailById(id).Result;

                PatientsDetail PatientsDetail1 = GetPatientDetailForSandDataDetails(id);

                objPatientDetails.PatientId = PatientsDetail1.PatientId;
                objPatientDetails.PatientName = PatientsDetail1.PatientName;
                objPatientDetails.FirstName = PatientsDetail1.FirstName;
                objPatientDetails.LastName = PatientsDetail1.LastName;               
                objPatientDetails.MedicalId = PatientsDetail1.MedicalId;
                objPatientDetails.PhoneNo = PatientsDetail1.PhoneNo;
                objPatientDetails.Address = PatientsDetail1.Address;
                objPatientDetails.Street = PatientsDetail1.Street;
                objPatientDetails.City = PatientsDetail1.City;
                objPatientDetails.State = PatientsDetail1.State;
                objPatientDetails.ZipCode = PatientsDetail1.ZipCode;
                objPatientDetails.PrimaryMD = PatientsDetail1.PrimaryMD;
                objPatientDetails.OfficeId = PatientsDetail1.OfficeId;
                objPatientDetails.DateOfBirth = PatientsDetail1.DateOfBirth;
                objPatientDetails.IsActive= PatientsDetail1.IsActive;

                //if (PatientsDetail1.UserName == "" || PatientsDetail1.UserName == null)
                //{
                //    objPatientDetails.UserName = "";
                //}
                //else
                //{
                //    objPatientDetails.UserName = PatientsDetail1.UserName;
                //}
                //if (PatientsDetail1.Password == "" || PatientsDetail1.Password == null)
                //{
                //    objPatientDetails.Password = "";
                //}
                //else
                //{
                //    objPatientDetails.Password = PatientsDetail1.Password;
                //}

                if (PatientsDetail1.PayerId == "" || PatientsDetail1.PayerId == null)
                {
                    objPatientDetails.PayerId = "";
                }
                else
                {
                    objPatientDetails.PayerId = PatientsDetail1.PayerId;
                }

                if (PatientsDetail1.ClientPayerId == "" || PatientsDetail1.ClientPayerId == null)
                {
                    objPatientDetails.ClientPayerId = "";
                }
                else
                {
                    objPatientDetails.ClientPayerId = PatientsDetail1.ClientPayerId;
                }

                if (PatientsDetail1.ProcedureCode == "" || PatientsDetail1.ProcedureCode == null)
                {
                    objPatientDetails.ProcedureCode = "";
                }
                else
                {
                    objPatientDetails.ProcedureCode = PatientsDetail1.ProcedureCode;
                }

                if (PatientsDetail1.PayerProgram == "" || PatientsDetail1.PayerProgram == null)
                {
                    objPatientDetails.PayerProgram = "";
                }
                else
                {
                    objPatientDetails.PayerProgram = PatientsDetail1.PayerProgram;
                }

                if (PatientsDetail1.JurisdictionCode == "" || PatientsDetail1.JurisdictionCode == null)
                {
                    objPatientDetails.JurisdictionCode = "";
                }
                else
                {
                    objPatientDetails.JurisdictionCode = PatientsDetail1.JurisdictionCode;
                }
         
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "GetPatientDetailById";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }

            return objPatientDetails;
        }


        public PatientsDetail GetPatientDetailForSandDataDetails(string PatientDetailId)
        {
            PatientsDetail objPatientDetail = new PatientsDetail();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientSandDataDetailById", PatientDetailId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    objPatientDetail.PatientId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientId"]);
                    objPatientDetail.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objPatientDetail.FirstName= ds.Tables[0].Rows[0]["FirstName"].ToString();
                    objPatientDetail.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    objPatientDetail.MedicalId = ds.Tables[0].Rows[0]["MedicalId"].ToString();
                    objPatientDetail.PhoneNo = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                    objPatientDetail.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    objPatientDetail.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objPatientDetail.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objPatientDetail.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objPatientDetail.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objPatientDetail.PrimaryMD = ds.Tables[0].Rows[0]["PrimaryMD"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DateOfBirth"].ToString()))
                    {

                        objPatientDetail.DateOfBirth = Convert.ToString(DateTime.Parse((ds.Tables[0].Rows[0]["DateOfBirth"]).ToString()).ToString("MM-dd-yyyy"));
                    }
                    else
                    {
                        objPatientDetail.DateOfBirth = "";
                    }

                    //objPatientDetail.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    //objPatientDetail.Password = ds.Tables[0].Rows[0]["Password"].ToString();

                    int officeId = 0;
                    Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["OfficeId"]), out officeId);
                    objPatientDetail.OfficeId = officeId;

                    
                    objPatientDetail.IsActive = Convert.ToString(Convert.ToInt32(ds.Tables[0].Rows[0]["IsActive"]));

                    objPatientDetail.ClientPayerId = ds.Tables[0].Rows[0]["ClientPayerId"].ToString();
                    objPatientDetail.PayerId = ds.Tables[0].Rows[0]["PayerId"].ToString();
                    objPatientDetail.PayerProgram = ds.Tables[0].Rows[0]["PayerProgram"].ToString();

                    objPatientDetail.ProcedureCode = ds.Tables[0].Rows[0]["ProcedureCode"].ToString();
                    objPatientDetail.JurisdictionCode = ds.Tables[0].Rows[0]["JurisdictionCode"].ToString();

                    //objPatientDetail.PayerId= ds.Tables[0].Rows[0]["Password"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientDetailById";
              //  string result = InsertErrorLog(objErrorlog);
            }
            return objPatientDetail;
        }




        public string DeletePatientDetails(string PatientDetailId)
        {
            string result = "";
            try
            {
                PatientDetailsServiceProxy CareGiverLiteService = new PatientDetailsServiceProxy();//ProviderUserKey
                result = CareGiverLiteService.DeletePatientDetail(PatientDetailId, Membership.GetUser().ProviderUserKey.ToString()).Result;
                if (result == "Success")
                {
                    TempData["Message"] = "Patient deleted successfully.";
                    //return RedirectToAction("Patients", "Patients", new { IsAdded = true });
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "DeletePatientDetails";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return result;
        }

        #region ImportPatientFromExcel
        [HttpPost]
        public ActionResult ImportExcelData(HttpPostedFileBase file)
        {
            var Result = "";// new Status();
            try
            {
                var httpPostedFile = file; //System.Web.HttpContext.Current.Request.Files["UploadedExcel"];
                if (httpPostedFile != null && httpPostedFile.ContentLength > 0)
                {
                    Stream stream = httpPostedFile.InputStream;
                    DataTable data = null;
                    DataSet Exceldata = null;
                    //For Reading Excel
                    IExcelDataReader ExcelReader = null;

                    if (httpPostedFile.FileName.EndsWith(".xls"))
                    {
                        ExcelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        //Create column names from first row
                        ExcelReader.IsFirstRowAsColumnNames = true;
                        Exceldata = ExcelReader.AsDataSet();
                    }
                    else if (httpPostedFile.FileName.EndsWith(".xlsx"))
                    {
                        ExcelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        //Create column names from first row
                        ExcelReader.IsFirstRowAsColumnNames = true;
                        Exceldata = ExcelReader.AsDataSet();
                    }


                    //For Check Excel Data available Or Not
                    if (Exceldata != null)
                    {
                        //for convert dataset to datatable
                        data = Exceldata.Tables[0];
                    }

                    if (data != null)
                    {
                        if (data.Columns.Count < 2)
                        {
                            TempData["Message"] = "No Matching Data Found In Uploaded File";
                            TempData["error"] = true;
                            return RedirectToAction("Patients", "Patients");
                            //ObjStatus.IsSuccess = false;
                            //ObjStatus.Message = "No Matching Data Found In Uploaded File";
                            //Give Error message 
                        }
                        else
                        {
                            if (data.Columns[0].ToString().ToLower().Trim() == "patient"
                                && data.Columns[1].ToString().ToLower().Trim() == "mrn"
                                 && data.Columns[2].ToString().ToLower().Trim() == "street"
                                  && data.Columns[3].ToString().ToLower().Trim() == "city"
                                  && data.Columns[4].ToString().ToLower().Trim() == "state"
                                  && data.Columns[5].ToString().ToLower().Trim() == "zipcode"
                                  && data.Columns[6].ToString().ToLower().Trim() == "telephone"
                                  && data.Columns[7].ToString().ToLower().Trim() == "primarymd"
                                  && data.Columns[8].ToString().ToLower().Trim() == "office"
                               )
                            {

                                int BlankRowCount = 0;
                                DataTable UserDataTable = new DataTable();
                                UserDataTable.Columns.Add("PatientName", typeof(string));
                                UserDataTable.Columns.Add("MedicalId", typeof(string));
                                UserDataTable.Columns.Add("Street", typeof(string));
                                UserDataTable.Columns.Add("City", typeof(string));
                                UserDataTable.Columns.Add("State", typeof(string));
                                UserDataTable.Columns.Add("ZipCode", typeof(string));
                                UserDataTable.Columns.Add("PhoneNo", typeof(string));
                                UserDataTable.Columns.Add("PrimaryMD", typeof(string));
                                UserDataTable.Columns.Add("OfficeName", typeof(string));

                                for (int i = 0; i < data.Rows.Count; i++)
                                {
                                    if (data.Rows[i][0].ToString() != "" && data.Rows[i][1].ToString() != "" && data.Rows[i][2].ToString() != "" && data.Rows[i][3].ToString() != "" && data.Rows[i][4].ToString() != "" && data.Rows[i][5].ToString() != "" && data.Rows[i][6].ToString() != "" && data.Rows[i][7].ToString() != "" && data.Rows[i][8].ToString() != "")
                                    {

                                        DataRow DataRow = UserDataTable.NewRow();
                                        DataRow["PatientName"] = data.Rows[i][0].ToString();
                                        DataRow["MedicalId"] = data.Rows[i][1].ToString();
                                        DataRow["Street"] = data.Rows[i][2].ToString();
                                        DataRow["City"] = data.Rows[i][3].ToString();
                                        DataRow["State"] = data.Rows[i][4].ToString();
                                        DataRow["ZipCode"] = data.Rows[i][5].ToString();
                                        DataRow["PhoneNo"] = data.Rows[i][6].ToString();
                                        DataRow["PrimaryMD"] = data.Rows[i][7].ToString();
                                        DataRow["OfficeName"] = data.Rows[i][8].ToString();
                                        UserDataTable.Rows.Add(DataRow);
                                        BlankRowCount = 0;
                                    }
                                    else
                                    {
                                        BlankRowCount++;

                                        //if (BlankRowCount == 5)
                                        //{
                                        //    break;
                                        //}
                                    }
                                }
                                if (UserDataTable.Rows.Count > 0)
                                {
                                    Result = CaregiverLiteService.AddPatientDetailsFromExcel(UserDataTable, Membership.GetUser().ProviderUserKey.ToString());
                                    if (Result == "Success")
                                    {
                                        TempData["Message"] = "Patients' Details Uploaded Successfully";
                                        return RedirectToAction("Patients", "Patients", new { IsAdded = true });
                                    }
                                    else
                                    {
                                        //TempData["Message"] = "No Matching Data Found In Uploaded File; " + Result;
                                        TempData["Message"] = "No Matching Data Found In Uploaded File, specified Office was not found";
                                        TempData["error"] = true;
                                        return RedirectToAction("Patients", "Patients");
                                    }
                                }
                                else
                                {
                                    TempData["Message"] = "No Matching Data Found In Uploaded File";
                                    TempData["error"] = true;
                                    return RedirectToAction("Patients", "Patients");
                                    
                                    //ObjStatus.Message = "No Matching Data Found In Uploaded File";
                                }
                            }
                            else
                            {
                                TempData["Message"] = "No Matching Data Found In Uploaded File";
                                TempData["error"] = true;
                                return RedirectToAction("Patients", "Patients");
                               
                                //ObjStatus.Message = "No Matching Data Found In Uploaded File";
                                //"No Matching Data Found In Uploaded File";
                            }
                        }
                    }
                    else
                    {
                        TempData["Message"] = "No Data Found In Selected File";
                        TempData["error"] = true;
                        return RedirectToAction("Patients", "Patients");
                        //ObjStatus.IsSuccess = false;
                        //ObjStatus.Message = "No Data Found In Uploaded File";
                    }
                }
                else
                {
                    TempData["Message"] = "Please Select File For Import Patient Data";
                    TempData["error"] = true;
                    return RedirectToAction("Patients", "Patients");
                    // ObjStatus.IsSuccess = false;
                    //ObjStatus.Message = "No Excel File Found.";
                }
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientsController";
                log.Methodname = "ImportExcelData";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
                TempData["Message"] = "No Matching Data Found In Uploaded File";// e.Message + " in Selected file";
                TempData["error"] = true;
                return RedirectToAction("Patients", "Patients");

            }
            return RedirectToAction("Patients", "Patients");//return View(); //return Json(new { Status = ObjStatus }, JsonRequestBehavior.AllowGet); ;
        }
        #endregion

        #region ExportDataSetToExcel

        public string ExportDataSetToExcel(string strOfficeId)
        {

            string result = "";
            var ObjStatus = "";
            try
            {
                DataSet ds = new DataSet();
                string UserID = Membership.GetUser().ProviderUserKey.ToString();
                int officeId = 0;
                int.TryParse(strOfficeId, out officeId);
                ds = CaregiverLiteService.GetPatientDetailsListForExcel(UserID, officeId);

                if (ds != null)
                {   //F:\HardikMasalawala\WorkOn\PaSeva_PGW_Latest\CaregiverLite\CaregiverLite\Content\ExportedFile\
                    string ExcelUploadPath  = ConfigurationManager.AppSettings["ExportedFilePath"].ToString();// Server.MapPath("")+ "\\Content\\ExportedFile\\";// ConfigurationManager.AppSettings["DealerSchemeSalesUploadPath"].ToString();
                    string ExcelPath = ConfigurationManager.AppSettings["DownlLoadFilePath"].ToString();
                 
                    var fileName = "PatientDetails_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
                    //File Path
                    var path = ExcelUploadPath + fileName;

                    var files = new DirectoryInfo(ExcelUploadPath).GetFiles();
                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file.FullName);
                    }

                    FileInfo newFile = new FileInfo(path);
                    ExcelPackage workbook = new ExcelPackage(newFile);
                    var PatientDataSheet = workbook.Workbook.Worksheets.Add("PatientDetail");

                    //For Write Column Name In Excel
                    PatientDataSheet.Cells[1, 1].Value = "Patient Name";
                    PatientDataSheet.Cells[1, 2].Value = "MRN";
                    PatientDataSheet.Cells[1, 3].Value = "Street";
                    PatientDataSheet.Cells[1, 4].Value = "City";
                    PatientDataSheet.Cells[1, 5].Value = "State";
                    PatientDataSheet.Cells[1, 6].Value = "Zipcode";
                    PatientDataSheet.Cells[1, 7].Value = "Telephone";
                    PatientDataSheet.Cells[1, 8].Value = "PrimaryMD";
                    PatientDataSheet.Cells[1, 9].Value = "Office";

                    PatientDataSheet.Column(1).Width = 40;
                    PatientDataSheet.Column(2).Width = 20;
                    PatientDataSheet.Column(3).Width = 50;
                    PatientDataSheet.Column(4).Width = 30;
                    PatientDataSheet.Column(5).Width = 10;
                    PatientDataSheet.Column(6).Width = 10;
                    PatientDataSheet.Column(7).Width = 20;
                    PatientDataSheet.Column(8).Width = 40;
                    PatientDataSheet.Column(9).Width = 40;

                    PatientDataSheet.Row(1).Style.Font.Bold = true;
                    PatientDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.Red);

                   
                    int RowNumber = 1;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        RowNumber = (RowNumber + 1);

                        PatientDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["Patient"].ToString();
                        PatientDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["MRN"];
                        PatientDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["Street"].ToString();
                        PatientDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["City"].ToString();
                        PatientDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["State"].ToString();
                        PatientDataSheet.Cells[RowNumber, 6].Value =Convert.ToInt32(ds.Tables[0].Rows[i]["Zipcode"]);
                        PatientDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["Telephone"].ToString();
                        PatientDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["PrimaryMD"].ToString();
                        PatientDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                    }

                    PatientDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    PatientDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    PatientDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    PatientDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    PatientDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    PatientDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    PatientDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    PatientDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    workbook.Save();

                    string ExcelUrl = ExcelPath + fileName;
                    ObjStatus = ExcelUrl;                   
                }
                else
                {
                   
                }
                return ObjStatus;
            }
            catch (Exception e)
            {
                result = "Fail";
                string msg = e.Message;
            }
            return ObjStatus;
        }

        #endregion


        #region InsuranceExportDataSetToExcel

        public string InsuranceExportDataSetToExcel(string FromDate, string ToDate, int strOfficeId)
        {

            string result = "";
            var ObjStatus = "";
            try
            {
                DataSet ds = new DataSet();
                string UserID = Membership.GetUser().ProviderUserKey.ToString();
                //int officeId = 0;
                //int.TryParse(strOfficeId, out officeId);
                ds = GetPatientInsuranceDetailsListForExcel(FromDate, ToDate, strOfficeId);

                if (ds != null)
                {
                    string ExcelUploadPath = "";
                    string ExcelPath = "";
                    try
                    {
                       ExcelUploadPath = ConfigurationManager.AppSettings["InsuranceExportedFilePath"].ToString();// Server.MapPath("")+ "\\Content\\ExportedFile\\";// ConfigurationManager.AppSettings["DealerSchemeSalesUploadPath"].ToString();
                       ExcelPath = ConfigurationManager.AppSettings["InsuranceDownlLoadFilePath"].ToString();
                    }
                    catch(Exception ex)
                    {
                        //insertdata(ex.Message);
                    }

                    var fileName = "InsuranceDetails_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xlsx";
                   
                    //File Path
                    var path =System.IO.Path.Combine(ExcelUploadPath,fileName);


                    var files = new DirectoryInfo(ExcelUploadPath).GetFiles();
                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file.FullName);
                    }

                    FileInfo newFile = new FileInfo(path);
                    ExcelPackage workbook = new ExcelPackage(newFile);
                    var PatientDataSheet = workbook.Workbook.Worksheets.Add("InsurancePolicyDetail");

                    //For Write Column Name In Excel
                   // PatientDataSheet.Cells[1, 1].Value = "InsurancePolicyId";
                    PatientDataSheet.Cells[1, 1].Value = "PatientRequestId";
                    PatientDataSheet.Cells[1, 2].Value = "PatientName";
                    PatientDataSheet.Cells[1, 3].Value = "CareGiverName";
                    PatientDataSheet.Cells[1, 4].Value = "IsInsurance";
                    PatientDataSheet.Cells[1, 5].Value = "InsuranceName";
                    PatientDataSheet.Cells[1, 6].Value = "InsurancePolicy";
                    PatientDataSheet.Cells[1, 7].Value = "IsCurrentPlan";
                    PatientDataSheet.Cells[1, 8].Value = "NewInsurancePlanName";
                    PatientDataSheet.Cells[1, 9].Value = "NewInsurancePolicyNumber";
                    PatientDataSheet.Cells[1, 10].Value = "NewInsurancePolicyGroupNumber";
                    PatientDataSheet.Cells[1, 11].Value = "IsMedicalGroup";
                    PatientDataSheet.Cells[1, 12].Value = "NewMedicalGroupName";
                    PatientDataSheet.Cells[1, 13].Value = "MedicalGroupAdditionalInformation";
                    PatientDataSheet.Cells[1, 14].Value = "IsfutureTerminationDate";
                    PatientDataSheet.Cells[1, 15].Value = "AddfutureTerminationDate";
                    PatientDataSheet.Cells[1, 16].Value = "Medicalid";
                    PatientDataSheet.Cells[1, 17].Value = "OfficeName";
                    PatientDataSheet.Cells[1, 18].Value = "Submission Date";

                    PatientDataSheet.Column(1).Width = 40;
                    PatientDataSheet.Column(2).Width = 20;
                    PatientDataSheet.Column(3).Width = 50;
                    PatientDataSheet.Column(4).Width = 30;
                    PatientDataSheet.Column(5).Width = 10;
                    PatientDataSheet.Column(6).Width = 10;
                    PatientDataSheet.Column(7).Width = 20;
                    PatientDataSheet.Column(8).Width = 40;
                    PatientDataSheet.Column(9).Width = 40;
                    PatientDataSheet.Column(10).Width = 40;
                    PatientDataSheet.Column(11).Width = 20;
                    PatientDataSheet.Column(12).Width = 50;
                    PatientDataSheet.Column(13).Width = 30;
                    PatientDataSheet.Column(14).Width = 10;
                    PatientDataSheet.Column(15).Width = 10;
                    PatientDataSheet.Column(16).Width = 20;
                    PatientDataSheet.Column(17).Width = 40;
                    PatientDataSheet.Column(18).Width = 40;

                    PatientDataSheet.Row(1).Style.Font.Bold = true;
                    PatientDataSheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.Red);

                    int RowNumber = 1;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        RowNumber = (RowNumber + 1);

                        //string datacheck = ds.Tables[0].Rows[i]["InsurancePolicy"].ToString() == IsNumeric  ? "" : ds.Tables[0].Rows[i]["InsurancePolicy"].ToString();
                       // PatientDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["InsurancePolicyId"].ToString();

                        PatientDataSheet.Cells[RowNumber, 1].Value = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        PatientDataSheet.Cells[RowNumber, 2].Value = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        PatientDataSheet.Cells[RowNumber, 3].Value = ds.Tables[0].Rows[i]["CareGiverName"].ToString();
                        PatientDataSheet.Cells[RowNumber, 4].Value = ds.Tables[0].Rows[i]["IsInsurance"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["InsuranceName"].ToString()))
                        {
                            PatientDataSheet.Cells[RowNumber, 5].Value = ds.Tables[0].Rows[i]["InsuranceName"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 5].Value = "";
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["InsurancePolicyNumber"].ToString()))
                        {
                            PatientDataSheet.Cells[RowNumber, 6].Value = ds.Tables[0].Rows[i]["InsurancePolicyNumber"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 6].Value = "";
                        }


                        PatientDataSheet.Cells[RowNumber, 7].Value = ds.Tables[0].Rows[i]["IsCurrentPlan"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NewInsurancePlanName"].ToString()))
                        {
                            PatientDataSheet.Cells[RowNumber, 8].Value = ds.Tables[0].Rows[i]["NewInsurancePlanName"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 8].Value = "";
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NewInsurancePolicyNumber"].ToString()))
                        {
                                PatientDataSheet.Cells[RowNumber, 9].Value = ds.Tables[0].Rows[i]["NewInsurancePolicyNumber"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 9].Value = "";
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NewInsurancePolicyGroupNumber"].ToString()))
                          {
                               PatientDataSheet.Cells[RowNumber, 10].Value = ds.Tables[0].Rows[i]["NewInsurancePolicyGroupNumber"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 10].Value = "";
                        }

                        PatientDataSheet.Cells[RowNumber, 11].Value = ds.Tables[0].Rows[i]["IsMedicalGroup"].ToString();

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NewMedicalGroupName"].ToString()))
                        {
                            PatientDataSheet.Cells[RowNumber, 12].Value = ds.Tables[0].Rows[i]["NewMedicalGroupName"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 12].Value = "";
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["MedicalGroupAdditionalInformation"].ToString()))
                            {
                                PatientDataSheet.Cells[RowNumber, 13].Value = ds.Tables[0].Rows[i]["MedicalGroupAdditionalInformation"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 13].Value = "";
                        }

                        PatientDataSheet.Cells[RowNumber, 14].Value = ds.Tables[0].Rows[i]["IsfutureTerminationDate"].ToString();

                        string CheckDateFormat= DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["AddfutureTerminationDate"])).ToString("yyyy-MM-dd");

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["AddfutureTerminationDate"].ToString()) && CheckDateFormat != "1900-01-01")
                        {
                            PatientDataSheet.Cells[RowNumber, 15].Value = DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["AddfutureTerminationDate"])).ToString("yyyy-MM-dd");
                            //ds.Tables[0].Rows[i]["AddfutureTerminationDate"].ToString();
                        }
                        else
                        {
                            PatientDataSheet.Cells[RowNumber, 15].Value = "";
                        }
                        

                        PatientDataSheet.Cells[RowNumber, 16].Value = ds.Tables[0].Rows[i]["Medicalid"].ToString();
                        PatientDataSheet.Cells[RowNumber, 17].Value = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        PatientDataSheet.Cells[RowNumber, 18].Value = DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["InsertDateTime"])).ToString("yyyy-MM-dd");
                    }
                    
                    PatientDataSheet.Cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    PatientDataSheet.Cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    PatientDataSheet.Cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    PatientDataSheet.Cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    PatientDataSheet.Cells.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    PatientDataSheet.Cells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    PatientDataSheet.Cells.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    PatientDataSheet.Cells.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    workbook.SaveAs(newFile);

                    string ExcelUrl = ExcelPath + fileName;
                    ObjStatus = ExcelUrl;
                }
                else
                {

                }
                return ObjStatus;
            }
            catch (Exception e)
            {
                result = "Fail";
                string msg = e.Message;
            }
            return ObjStatus;
        }


        public static DataSet GetPatientInsuranceDetailsListForExcel(string FromDate, string ToDate,int OfficeId)
        {
            //string loginUserId

            DataSet Ds = new DataSet();
            try
            {
                DataSet tmpDs = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientInsuranceDetailsListForExcel", FromDate,ToDate,OfficeId);

                if (tmpDs != null && tmpDs.Tables.Count > 0 && tmpDs.Tables[0].Rows.Count > 0)
                {
                    Ds = tmpDs;
                    return Ds;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientDetailsListForExcel";
                //string result = InsertErrorLog(objErrorlog);
                return Ds;
            }
            return Ds;
        }


        #endregion


        [HttpGet]
        public ActionResult InsurancePolicyExport()
        {
            return PartialView("InsurancePolicyExport");
        }



        public JsonResult IsMedicalIdExist(string MedicalID)
        {
            var PatientDetail = new PatientsDetail();

          
            bool result = true;
            try
            {
               
                PatientDetailsServiceProxy PatientDetailsService = new PatientDetailsServiceProxy();
                
                PatientDetail.MedicalId = MedicalID;
                PatientDetail.InsertUserId = Membership.GetUser().ProviderUserKey.ToString();
                result = PatientDetailsService.IsMedicalIdExist(PatientDetail).Result;
                
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog();
                log.Errormessage = e.Message;
                log.StackTrace = e.StackTrace;
                log.Pagename = "PatientController";
                log.Methodname = "IsMedicalIdExist";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string res = ErrorLogService.InsertErrorLog(log).Result;
            }
            return Json(result, JsonRequestBehavior.AllowGet) ;
            //return result ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }


        #region
        //public string GetAndPostClientData(string MedicalId)
        //{
        //    EmployeeClientModel objModel = new EmployeeClientModel();
        //    string result = "";
        //    string ClientPayerId = "";

        //    //List<EmployeeResponse> employeeLis = new List<EmployeeResponse>();

        //    List<ClientAddRequest> clientAddRequest = new List<ClientAddRequest>();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientToaddSandDat", MedicalId);
        //        if (ds != null)
        //            if (ds.Tables.Count > 0)
        //            {
        //                foreach (DataRow item in ds.Tables[0].Rows)
        //                {
        //                    ClientAddRequest clientrequestAdd = new ClientAddRequest();
        //                    ProviderIdentification objprovider = new ProviderIdentification();
        //                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
        //                    objprovider.ProviderQualifier = "MedicaidID";
        //                    clientrequestAdd.ProviderIdentification = objprovider;
        //                    string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();

        //                    var stringNumber = ChkMedicalId;
        //                    int numericValue;
        //                    bool isNumber = int.TryParse(stringNumber, out numericValue);

        //                    if (!isNumber && Regex.IsMatch(stringNumber, "^[a-zA-Z0-9]*$") && Convert.ToString(item["MedicalId"]).Length >= 8)
        //                    {
        //                        string medicadid = string.Empty;

        //                        string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
        //                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, 8), 8);
        //                        int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

        //                        for (int i = jlen; i <= 8; i++)
        //                        {
        //                            if (i == 8)
        //                            {
        //                                // string randomstr = GenerateRandomString();
        //                                medicadid = s.Append(strAppend).ToString();
        //                            }
        //                            else
        //                            {
        //                                medicadid = s.Append('0').ToString();
        //                            }
        //                        }
        //                        clientrequestAdd.ClientMedicaidID = medicadid;
        //                        clientrequestAdd.ClientID = medicadid;
        //                        clientrequestAdd.ClientIdentifier = medicadid;
        //                        ClientPayerId = medicadid;
        //                    }
        //                    else
        //                    {
        //                        if (!isNumber && Regex.IsMatch(stringNumber, "^[a-zA-Z0-9]*$"))
        //                        {
        //                            string medicadid = string.Empty;
        //                            string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
        //                            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length)-2)), 8);
        //                            int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

        //                            for (int i = jlen; i <= 8; i++)
        //                            {
        //                                if (i == 8)
        //                                {
        //                                    // string randomstr = GenerateRandomString();
        //                                    medicadid = s.Append(strAppend).ToString();
        //                                }
        //                                else
        //                                {
        //                                    medicadid = s.Append('0').ToString();
        //                                }
        //                            }
        //                            clientrequestAdd.ClientMedicaidID = medicadid;
        //                            clientrequestAdd.ClientID = medicadid;
        //                            clientrequestAdd.ClientIdentifier = medicadid;
        //                            ClientPayerId = medicadid;
        //                        }
        //                        else
        //                        {

        //                            string medicadid = string.Empty;

        //                            string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);

        //                            int jlen = Convert.ToString(item["MedicalId"]).Length;
        //                            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);
        //                            for (int i = jlen; i <= 8; i++)
        //                            {
        //                                if (i == 8)
        //                                {
        //                                    // string randomstr = GenerateRandomString();
        //                                    medicadid = s.Append(strAppendByOffice).ToString();
        //                                }
        //                                else
        //                                {
        //                                    medicadid = s.Append('0').ToString();
        //                                }
        //                            }
        //                            clientrequestAdd.ClientMedicaidID = medicadid;
        //                            clientrequestAdd.ClientID = medicadid;
        //                            clientrequestAdd.ClientIdentifier = medicadid;
        //                            ClientPayerId = medicadid;


        //                        }
        //                    }

        //                    if (!Convert.ToString(item["PatientName"]).Contains(','))
        //                    {
        //                        clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"]));
        //                        clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"]));
        //                    }
        //                    else
        //                    {
        //                        clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"])).Split(',')[1];
        //                        clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"])).Split(',')[0];
        //                    }
        //                    //clientrequestAdd.ClientMedicaidID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
        //                    clientrequestAdd.ClientQualifier = "ClientMedicaidID";
        //                    //clientrequestAdd.ClientIdentifier = Convert.ToString(item["MedicalId"]).Substring(0, 7);
        //                    clientrequestAdd.ClientOtherID = Convert.ToString(item["PrimaryMD"]);
        //                    clientrequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
        //                    clientrequestAdd.ClientTimezone = "US/Pacific";

        //                    List<ClientPayerInformation> lstclientpayer = new List<ClientPayerInformation>();
        //                    ClientPayerInformation objclientPayer = new ClientPayerInformation();

        //                    objclientPayer.ClientEligibilityDateBegin = "2022-01-01";
        //                    objclientPayer.ClientEligibilityDateEnd = "2024-01-01";

        //                    objclientPayer.ClientStatus = "02";
        //                    objclientPayer.EffectiveEndDate = "2024-01-01";
        //                    objclientPayer.EffectiveStartDate = "2022-01-01";

        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])) && Convert.ToString(item["PayerId"]).ToString() == "")
        //                    {
        //                        objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.PayerID = "CAMCWP";
        //                    }

        //                    if (!string.IsNullOrEmpty(ClientPayerId))
        //                    {
        //                        objclientPayer.ClientPayerID = ClientPayerId;
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.ClientPayerID = "987654321";
        //                    }

        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["JurisdictionCode"])) && Convert.ToString(item["JurisdictionCode"]).ToString() == "")
        //                    {
        //                        objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.JurisdictionID = "SDiego37";
        //                    }

        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])) && Convert.ToString(item["PayerProgram"]).ToString() == "")
        //                    {
        //                        objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.PayerProgram = "HHCS";
        //                    }

        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])) && Convert.ToString(item["ProcedureCode"]).ToString() == "")
        //                    {
        //                        objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.ProcedureCode = "S9124";
        //                    }

        //                    //if (Convert.ToString(item["PayerId"]) != null && Convert.ToString(item["PayerId"]).ToString() == "")
        //                    //{
        //                    //    objclientPayer.ClientPayerID = Convert.ToString(item["ClientPayerId"]);
        //                    //    objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
        //                    //    objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
        //                    //    objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
        //                    //    objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
        //                    //}
        //                    //else
        //                    //{             
        //                    //        objclientPayer.ClientPayerID = "987654321"; ;
        //                    //        objclientPayer.ClientPayerID = "987654321";
        //                    //        objclientPayer.JurisdictionID = "SDiego37";
        //                    //        objclientPayer.PayerID = "CAMCWP";
        //                    //        objclientPayer.ProcedureCode = "S9124";                
        //                    //}

        //                    //if (IsNumeric(Convert.ToString(item["ClientPayerId"])))
        //                    //{
        //                    //    objclientPayer.ClientPayerID = Convert.ToString(item["ClientPayerId"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.ClientPayerID = "987654321";
        //                    //}

        //                    //if (IsAlphaNumericAlpha(Convert.ToString(item["JurisdictionCode"])))
        //                    //{
        //                    //    objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.JurisdictionID = "SDiego37";
        //                    //}


        //                    //if (IsAlpha(Convert.ToString(item["PayerId"])))
        //                    //{
        //                    //    objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.PayerID = "CAMCWP";
        //                    //}

        //                    //if (IsAlphaNumericData(Convert.ToString(item["PayerProgram"])))
        //                    //{
        //                    //    objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.PayerProgram = "HHCS";
        //                    //}

        //                    //if (IsAlphaNumeric(Convert.ToString(item["ProcedureCode"])))
        //                    //{
        //                    //    objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.ProcedureCode = "S9124";
        //                    //}

        //                    lstclientpayer.Add(objclientPayer);

        //                    clientrequestAdd.ClientPayerInformation = lstclientpayer;

        //                    List<ClientPhones> objclientphone = new List<ClientPhones>();
        //                    ClientPhones clientphonesss = new ClientPhones();
        //                    clientphonesss.ClientPhone = Convert.ToString(item["PhoneNo"]);
        //                    clientphonesss.ClientPhoneType = "Home";

        //                    objclientphone.Add(clientphonesss);
        //                    clientrequestAdd.ClientPhone = objclientphone;

        //                    //clientrequestAdd.ClientPhone.Add(clientphones);
        //                    List<ClientAddress> objclientadd = new List<ClientAddress>();
        //                    ClientAddress clientadd = new ClientAddress();
        //                    clientadd.ClientCity = Convert.ToString(item["City"]);
        //                    clientadd.ClientCounty = "";
        //                    clientadd.ClientState = Convert.ToString(item["State"]);
        //                    clientadd.ClientZip = Convert.ToString(item["ZipCode"]);
        //                    clientadd.ClientAddressLongitude = Convert.ToString(item["Longitude"]);
        //                    clientadd.ClientAddressLatitude = Convert.ToString(item["Latitude"]);
        //                    clientadd.ClientAddressType = "Home";
        //                    clientadd.ClientAddressLine1 = Convert.ToString(item["Street"]);
        //                    clientadd.ClientAddressLine2 = Convert.ToString(item["TimeZoneId"]);
        //                    clientadd.ClientAddressIsPrimary = "True";
        //                    objclientadd.Add(clientadd);
        //                    clientrequestAdd.ClientAddress = objclientadd;
        //                    clientAddRequest.Add(clientrequestAdd);
        //                }
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CaregiverLiteServices";
        //        objErrorlog.Methodname = "GetAllClientRequestAndReview";
        //        InsertErrorLog(objErrorlog);
        //        return result = "False";
        //    }

        //    if (clientAddRequest.Count > 0 && clientAddRequest != null)
        //    {
        //      var arraylist = clientAddRequest.ToArray();
        //     List<ClientAddRequest> request = new List<ClientAddRequest>();
        //    foreach (var ReqItem in arraylist)
        //    {
        //        request.Add(ReqItem);
        //    }

        //    var clientGetDialogId = new System.Net.Http.HttpClient();
        //    string Token = ConfigurationManager.AppSettings["mykey"].ToString();
        //    string actheader = ConfigurationManager.AppSettings["Token"].ToString();

        //    string x = JsonConvert.SerializeObject(request);

        //        Task.Run(async () => { result = await objModel.SubmitRequestDat(x); }).Wait(); ;

        //    }
        //    else
        //    {
        //        return result = "false";
        //    }
        //    return result;
        //}

        #endregion


        //public string GetAndPostClientData(string MedicalId)
        //{
        //    EmployeeClientModel objModel = new EmployeeClientModel();
        //    string result = "";
        //    string ClientPayerId = "";

        //    //List<EmployeeResponse> employeeLis = new List<EmployeeResponse>();

        //    List<ClientAddRequest> clientAddRequest = new List<ClientAddRequest>();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientToaddSandDat", MedicalId);
        //        if (ds != null)
        //            if (ds.Tables.Count > 0)
        //            {
        //                int y = 0;
        //                foreach (DataRow item in ds.Tables[0].Rows)
        //                {


        //                    ClientAddRequest clientrequestAdd = new ClientAddRequest();

        //                    ProviderIdentification objprovider = new ProviderIdentification();
        //                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
        //                    objprovider.ProviderQualifier = "MedicaidID";
        //                    clientrequestAdd.ProviderIdentification = objprovider;

        //                    //  string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();

        //                    string ChkMedicalId = string.Empty;
        //                    string strChkMedical = Convert.ToString(item["MedicalId"]).ToString();
        //                    string strChkMedicalFInal = string.Empty;


        //                    for (int i = 0; i < strChkMedical.Length; i++)
        //                    {
        //                        if (Char.IsLetterOrDigit(strChkMedical[i]))
        //                            strChkMedicalFInal += strChkMedical[i];
        //                    }
        //                    if (strChkMedicalFInal.Length > 0)
        //                    {
        //                        ChkMedicalId = strChkMedicalFInal.ToString();
        //                    }

        //                    var stringNumber = ChkMedicalId;
        //                    int numericValue;
        //                    bool isNumber = int.TryParse(stringNumber, out numericValue);

        //                    bool istrue = (Regex.IsMatch(ChkMedicalId, @"[a-zA-Z]") && Regex.IsMatch(ChkMedicalId, @"[0-9]"));

        //                    if (!isNumber && istrue && Convert.ToString(item["MedicalId"]).Length >= 8)
        //                    {
        //                        string medicadid = string.Empty;
        //                        int dlen = Convert.ToString(item["MedicalId"]).Length;

        //                        string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
        //                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 8), 8);

        //                        int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

        //                        for (int i = jlen; i <= 8; i++)
        //                        {

        //                            if (i == 8)
        //                            {
        //                                // string randomstr = GenerateRandomString();
        //                                medicadid = s.Append(strAppend).ToString();
        //                            }
        //                            else
        //                            {
        //                                medicadid = s.Append('0').ToString();
        //                            }
        //                        }

        //                        string str7 = medicadid;
        //                        string str8 = string.Empty;
        //                        string str9 = string.Empty;
        //                        int val8 = 0;

        //                        for (int i = 0; i < str7.Length; i++)
        //                        {
        //                            if (Char.IsLetterOrDigit(str7[i]))
        //                                str8 += str7[i];
        //                        }
        //                        if (str8.Length > 0)
        //                        {
        //                            str9 = str8.ToString();
        //                        }

        //                        clientrequestAdd.ClientMedicaidID = str9;
        //                        clientrequestAdd.ClientID = str9;
        //                        clientrequestAdd.ClientIdentifier = str9;
        //                        ClientPayerId = str9;
        //                    }
        //                    else
        //                    {
        //                        if (!isNumber && istrue)
        //                        {
        //                            string medicadid = string.Empty;
        //                            string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
        //                            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
        //                            int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

        //                            for (int i = jlen; i <= 8; i++)
        //                            {
        //                                if (i == 8)
        //                                {
        //                                    // string randomstr = GenerateRandomString();
        //                                    medicadid = s.Append(strAppend).ToString();
        //                                }
        //                                else
        //                                {
        //                                    medicadid = s.Append('0').ToString();
        //                                }
        //                            }
        //                            clientrequestAdd.ClientMedicaidID = medicadid;
        //                            clientrequestAdd.ClientID = medicadid;
        //                            clientrequestAdd.ClientIdentifier = medicadid;
        //                            ClientPayerId = medicadid;
        //                        }
        //                        else
        //                        {

        //                            string medicadid = string.Empty;

        //                            string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);

        //                            int jlen = Convert.ToString(item["MedicalId"]).Length;
        //                            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);
        //                            for (int i = jlen; i <= 8; i++)
        //                            {
        //                                if (i == 8)
        //                                {
        //                                    // string randomstr = GenerateRandomString();
        //                                    medicadid = s.Append(strAppendByOffice).ToString();
        //                                }
        //                                else
        //                                {
        //                                    medicadid = s.Append('0').ToString();
        //                                }
        //                            }
        //                            clientrequestAdd.ClientMedicaidID = medicadid;
        //                            clientrequestAdd.ClientID = medicadid;
        //                            clientrequestAdd.ClientIdentifier = medicadid;
        //                            ClientPayerId = medicadid;
        //                        }
        //                    }



        //                    //if (Convert.ToString(item["MedicalId"]).Length >= 8)
        //                    //{
        //                    //    clientrequestAdd.ClientMedicaidID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
        //                    //    clientrequestAdd.ClientID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
        //                    //    clientrequestAdd.ClientIdentifier = Convert.ToString(item["MedicalId"]).Substring(0, 7);
        //                    //}
        //                    //else
        //                    //{
        //                    //    string medicadid = string.Empty;

        //                    //    int jlen = Convert.ToString(item["MedicalId"]).Length;
        //                    //    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);
        //                    //    for (int i = jlen; i <= 8; i++)
        //                    //    {
        //                    //        if (i == 8)
        //                    //        {
        //                    //            // string randomstr = GenerateRandomString();
        //                    //            medicadid = s.Append('C').ToString();
        //                    //        }
        //                    //        else
        //                    //        {
        //                    //            medicadid = s.Append('0').ToString();
        //                    //        }
        //                    //    }
        //                    //    clientrequestAdd.ClientMedicaidID = medicadid;
        //                    //    clientrequestAdd.ClientID = medicadid;
        //                    //    clientrequestAdd.ClientIdentifier = medicadid;
        //                    //}

        //                    if (!Convert.ToString(item["PatientName"]).Contains(','))
        //                    {
        //                        clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"]));
        //                        clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"]));
        //                    }
        //                    else
        //                    {
        //                        clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"])).Split(',')[1];
        //                        clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"])).Split(',')[0];
        //                    }
        //                    //clientrequestAdd.ClientMedicaidID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
        //                    clientrequestAdd.ClientQualifier = "ClientMedicaidID";
        //                    //clientrequestAdd.ClientIdentifier = Convert.ToString(item["MedicalId"]).Substring(0, 7);

        //                    clientrequestAdd.ClientOtherID = Convert.ToString(item["PrimaryMD"]);

        //                    string str3 = Convert.ToString(item["PrimaryMD"]);
        //                    string str4 = string.Empty;
        //                    int val = 0;

        //                    for (int i = 0; i < str3.Length; i++)
        //                    {
        //                        if (Char.IsLetterOrDigit(str3[i]))
        //                            str4 += str3[i];
        //                    }
        //                    if (str4.Length > 0)
        //                    {
        //                        clientrequestAdd.ClientOtherID = str4.ToString();
        //                    }

        //                    clientrequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
        //                   // clientrequestAdd.SequenceID = Convert.ToString((int)(item["timestampdata"]) + y);
        //                    //Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
        //                    clientrequestAdd.ClientTimezone = "US/Pacific";


        //                    List<ClientPayerInformation> lstclientpayer = new List<ClientPayerInformation>();


        //                    ClientPayerInformation objclientPayer = new ClientPayerInformation();

        //                    objclientPayer.ClientEligibilityDateBegin = "2022-01-01";
        //                    objclientPayer.ClientEligibilityDateEnd = "2024-01-01";

        //                    objclientPayer.ClientStatus = "02";
        //                    objclientPayer.EffectiveEndDate = "2024-01-01";
        //                    objclientPayer.EffectiveStartDate = "2022-01-01";

        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
        //                    {
        //                        objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.PayerID = "CAMCWP";
        //                    }

        //                    if (!string.IsNullOrEmpty(ClientPayerId))
        //                    {
        //                        objclientPayer.ClientPayerID = ClientPayerId;
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.ClientPayerID = "987654321";
        //                    }

        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["JurisdictionCode"])))
        //                    {
        //                        objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.JurisdictionID = "ASN";
        //                    }


        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
        //                    {
        //                        objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
        //                    }
        //                    else
        //                    {
        //                        objclientPayer.PayerProgram = "HHCS";
        //                    }

        //                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
        //                    {
        //                        objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
        //                    }
        //                    else
        //                    {

        //                        objclientPayer.ProcedureCode = "G0156";
        //                    }
        //                    //^[A - Za - z\d]{ 4}\d{ 6,7}\z


        //                    //if (IsNumeric(Convert.ToString(item["ClientPayerId"])))
        //                    //{
        //                    //    objclientPayer.ClientPayerID = Convert.ToString(item["ClientPayerId"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.ClientPayerID = "987654321";
        //                    //}

        //                    //if (IsAlphaNumericAlpha(Convert.ToString(item["JurisdictionCode"])))
        //                    //{
        //                    //    objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.JurisdictionID = "SDiego37";
        //                    //}


        //                    //if (IsAlpha(Convert.ToString(item["PayerId"])))
        //                    //{
        //                    //    objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.PayerID = "CAMCWP";
        //                    //}

        //                    //if (IsAlphaNumericData(Convert.ToString(item["PayerProgram"])))
        //                    //{
        //                    //    objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.PayerProgram = "HHCS";
        //                    //}

        //                    //if (IsAlphaNumeric(Convert.ToString(item["ProcedureCode"])))
        //                    //{
        //                    //    objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    objclientPayer.ProcedureCode = "S9124";
        //                    //}


        //                    lstclientpayer.Add(objclientPayer);
        //                    clientrequestAdd.ClientPayerInformation = lstclientpayer;

        //                    List<ClientPhones> objclientphone = new List<ClientPhones>();
        //                    ClientPhones clientphonesss = new ClientPhones();

        //                    clientphonesss.ClientPhone = Convert.ToString(item["PhoneNo"]);

        //                    string str1 = Convert.ToString(item["PhoneNo"]);
        //                    string str2 = string.Empty;
        //                    int val1 = 0;

        //                    for (int i = 0; i < str1.Length; i++)
        //                    {
        //                        if (Char.IsDigit(str1[i]))
        //                            str2 += str1[i];
        //                    }
        //                    if (str2.Length > 0)
        //                    {
        //                        clientphonesss.ClientPhone = str2.ToString();
        //                    }

        //                    clientphonesss.ClientPhoneType = "Home";

        //                    objclientphone.Add(clientphonesss);
        //                    clientrequestAdd.ClientPhone = objclientphone;

        //                    //clientrequestAdd.ClientPhone.Add(clientphones);
        //                    List<ClientAddress> objclientadd = new List<ClientAddress>();
        //                    ClientAddress clientadd = new ClientAddress();

        //                    clientadd.ClientCity = Convert.ToString(item["City"]);

        //                    string str5 = Convert.ToString(item["City"]);
        //                    string str6 = string.Empty;
        //                    int val3 = 0;

        //                    for (int i = 0; i < str5.Length; i++)
        //                    {
        //                        if (Char.IsLetter(str5[i]) || char.IsWhiteSpace(str5[i]))
        //                            str6 += str5[i];
        //                    }
        //                    if (str6.Length > 0)
        //                    {
        //                        clientadd.ClientCity = str6.ToString();
        //                    }

        //                    clientadd.ClientCounty = "";
        //                    clientadd.ClientState = Convert.ToString(item["State"]).ToUpper();
        //                    clientadd.ClientZip = Convert.ToString(item["ZipCode"]);
        //                    clientadd.ClientAddressLongitude = Convert.ToString(item["Longitude"]);
        //                    clientadd.ClientAddressLatitude = Convert.ToString(item["Latitude"]);
        //                    clientadd.ClientAddressType = "Home";
        //                    clientadd.ClientAddressLine1 = Convert.ToString(item["Street"]);
        //                    clientadd.ClientAddressLine2 = Convert.ToString(item["TimeZoneId"]);
        //                    clientadd.ClientAddressIsPrimary = "True";
        //                    objclientadd.Add(clientadd);
        //                    clientrequestAdd.ClientAddress = objclientadd;

        //                    if (clientAddRequest.Count > 0)
        //                    {
        //                        var checkCondition = clientAddRequest.Where(xy => xy.ClientID == ClientPayerId).ToList().Select(xy => xy.ClientID == ClientPayerId).ToList();
        //                        if (checkCondition.Count > 0)
        //                        {
        //                            insertdata(Convert.ToString(item["MedicalId"]).ToString());
        //                        }
        //                        else
        //                        {
        //                            clientAddRequest.Add(clientrequestAdd);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        clientAddRequest.Add(clientrequestAdd);
        //                    }




        //                }
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CaregiverLiteServices";
        //        objErrorlog.Methodname = "GetAllClientRequestAndReview";
        //        InsertErrorLog(objErrorlog);
        //        return result = "False";
        //    }

        //    if (clientAddRequest.Count > 0 && clientAddRequest != null)
        //    {
        //        var arraylist = clientAddRequest.ToArray();
        //        List<ClientAddRequest> request = new List<ClientAddRequest>();
        //        foreach (var ReqItem in arraylist)
        //        {
        //            request.Add(ReqItem);
        //        }

        //        var clientGetDialogId = new System.Net.Http.HttpClient();
        //        string Token = ConfigurationManager.AppSettings["mykey"].ToString();
        //        string actheader = ConfigurationManager.AppSettings["Token"].ToString();

        //        string x = JsonConvert.SerializeObject(request);

        //        Task.Run(async () => { result = await objModel.SubmitRequestDat(x); }).Wait(); ;

        //    }
        //    else
        //    {
        //        return result = "false";
        //    }
        //    return result;
        //}


        public string GetAndPostClientData(string MedicalId)
        {
            EmployeeClientModel objModel = new EmployeeClientModel();
            string result = "";
            string ClientPayerId = "";
            int OfficeId = 0;

            //List<EmployeeResponse> employeeLis = new List<EmployeeResponse>();

            List<ClientAddRequest> clientAddRequest = new List<ClientAddRequest>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientToaddSandDat", MedicalId);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        int y = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {

                            ClientAddRequest clientrequestAdd = new ClientAddRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();

                            OfficeId = Convert.ToInt32(item["OfficeId"]);



                            string ChkMedicalId = string.Empty;
                            string strChkMedical = Convert.ToString(item["MedicalId"]).ToString();
                            string strChkMedicalFInal = string.Empty;


                            switch (OfficeId)
                            {
                                case 1:
                                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDMA"].ToString();
                                    objprovider.ProviderQualifier = "Other";

                                    clientrequestAdd.ProviderIdentification = objprovider;

                                    ChkMedicalId = Convert.ToString(item["ClientPayerId"]).ToString();
                                    clientrequestAdd.ClientMedicaidID = ChkMedicalId;
                                    // clientrequestAdd.ClientID = ChkMedicalId;
                                    clientrequestAdd.ClientIdentifier = ChkMedicalId;
                                    clientrequestAdd.ClientBirthDate= DateTime.Parse(Convert.ToString(item["DateOfBirth"]).ToString()).ToString("yyyy-MM-dd"); ;
                                    ClientPayerId = ChkMedicalId;


                                    clientrequestAdd.ClientTimezone = "US/Eastern";
                                    break;
                                case 5:
                                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                                    objprovider.ProviderQualifier = "MedicaidID";

                                    clientrequestAdd.ProviderIdentification = objprovider;

                                    for (int i = 0; i < strChkMedical.Length; i++)
                                    {
                                        if (Char.IsLetterOrDigit(strChkMedical[i]))
                                            strChkMedicalFInal += strChkMedical[i];
                                    }
                                    if (strChkMedicalFInal.Length > 0)
                                    {
                                        ChkMedicalId = strChkMedicalFInal.ToString();
                                    }

                                    var stringNumber = ChkMedicalId;
                                    int numericValue;
                                    bool isNumber = int.TryParse(stringNumber, out numericValue);

                                    bool istrue = (Regex.IsMatch(ChkMedicalId, @"[a-zA-Z]") && Regex.IsMatch(ChkMedicalId, @"[0-9]"));

                                    if (!isNumber && istrue && Convert.ToString(item["MedicalId"]).Length >= 8)
                                    {
                                        string medicadid = string.Empty;
                                        int dlen = Convert.ToString(item["MedicalId"]).Length;

                                        string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 8), 8);

                                        int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

                                        for (int i = jlen; i <= 8; i++)
                                        {

                                            if (i == 8)
                                            {
                                                // string randomstr = GenerateRandomString();
                                                medicadid = s.Append(strAppend).ToString();
                                            }
                                            else
                                            {
                                                medicadid = s.Append('0').ToString();
                                            }
                                        }

                                        string str7 = medicadid;
                                        string str8 = string.Empty;
                                        string str9 = string.Empty;
                                        int val8 = 0;

                                        for (int i = 0; i < str7.Length; i++)
                                        {
                                            if (Char.IsLetterOrDigit(str7[i]))
                                                str8 += str7[i];
                                        }
                                        if (str8.Length > 0)
                                        {
                                            str9 = str8.ToString();
                                        }

                                        clientrequestAdd.ClientMedicaidID = str9;
                                        clientrequestAdd.ClientID = str9;
                                        clientrequestAdd.ClientIdentifier = str9;
                                        ClientPayerId = str9;
                                    }
                                    else
                                    {
                                        if (!isNumber && istrue)
                                        {
                                            string medicadid = string.Empty;
                                            string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
                                            int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

                                            for (int i = jlen; i <= 8; i++)
                                            {
                                                if (i == 8)
                                                {
                                                    // string randomstr = GenerateRandomString();
                                                    medicadid = s.Append(strAppend).ToString();
                                                }
                                                else
                                                {
                                                    medicadid = s.Append('0').ToString();
                                                }
                                            }
                                            clientrequestAdd.ClientMedicaidID = medicadid;
                                            clientrequestAdd.ClientID = medicadid;
                                            clientrequestAdd.ClientIdentifier = medicadid;
                                            ClientPayerId = medicadid;
                                        }
                                        else
                                        {

                                            string medicadid = string.Empty;

                                            string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);
                                            int jlen = Convert.ToString(item["MedicalId"]).Length;

                                            if (jlen >= 8)
                                            {
                                                jlen = Convert.ToString(item["MedicalId"]).Substring(1, 8).Length;
                                                StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(1, 8));

                                                for (int i = jlen; i <= 8; i++)
                                                {
                                                    if (i == 8)
                                                    {
                                                        // string randomstr = GenerateRandomString();
                                                        medicadid = s.Append(strAppendByOffice).ToString();
                                                    }
                                                    else
                                                    {
                                                        medicadid = s.Append('0').ToString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                jlen = Convert.ToString(item["MedicalId"]).Length;
                                                StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);

                                                for (int i = jlen; i <= 8; i++)
                                                {
                                                    if (i == 8)
                                                    {
                                                        // string randomstr = GenerateRandomString();
                                                        medicadid = s.Append(strAppendByOffice).ToString();
                                                    }
                                                    else
                                                    {
                                                        medicadid = s.Append('0').ToString();
                                                    }
                                                }
                                            }
                                            clientrequestAdd.ClientMedicaidID = medicadid;
                                            clientrequestAdd.ClientID = medicadid;
                                            clientrequestAdd.ClientIdentifier = medicadid;
                                            ClientPayerId = medicadid;

                                        }
                                    }

                                    clientrequestAdd.ClientTimezone = "US/Pacific";
                                    break;

                                    case 12:
                                    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDCASD"].ToString();
                                    objprovider.ProviderQualifier = "MedicaidID";

                                    clientrequestAdd.ProviderIdentification = objprovider;

                                    // string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();

                                    for (int i = 0; i < strChkMedical.Length; i++)
                                    {
                                        if (Char.IsLetterOrDigit(strChkMedical[i]))
                                            strChkMedicalFInal += strChkMedical[i];
                                    }
                                    if (strChkMedicalFInal.Length > 0)
                                    {
                                        ChkMedicalId = strChkMedicalFInal.ToString();
                                    }

                                    var stringNumber1= ChkMedicalId;
                                    int numericValue1;
                                    bool isNumber1 = int.TryParse(stringNumber1, out numericValue1);

                                    bool istrue1 = (Regex.IsMatch(ChkMedicalId, @"[a-zA-Z]") && Regex.IsMatch(ChkMedicalId, @"[0-9]"));

                                    if (!isNumber1 && istrue1 && Convert.ToString(item["MedicalId"]).Length >= 8)
                                    {
                                        string medicadid = string.Empty;
                                        int dlen = Convert.ToString(item["MedicalId"]).Length;

                                        string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 8), 8);

                                        int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

                                        for (int i = jlen; i <= 8; i++)
                                        {

                                            if (i == 8)
                                            {
                                                // string randomstr = GenerateRandomString();
                                                medicadid = s.Append(strAppend).ToString();
                                            }
                                            else
                                            {
                                                medicadid = s.Append('0').ToString();
                                            }
                                        }

                                        string str7 = medicadid;
                                        string str8 = string.Empty;
                                        string str9 = string.Empty;
                                        int val8 = 0;

                                        for (int i = 0; i < str7.Length; i++)
                                        {
                                            if (Char.IsLetterOrDigit(str7[i]))
                                                str8 += str7[i];
                                        }
                                        if (str8.Length > 0)
                                        {
                                            str9 = str8.ToString();
                                        }

                                        clientrequestAdd.ClientMedicaidID = str9;
                                        clientrequestAdd.ClientID = str9;
                                        clientrequestAdd.ClientIdentifier = str9;
                                        ClientPayerId = str9;
                                    }
                                    else
                                    {
                                        if (!isNumber1 && istrue1)
                                        {
                                            string medicadid = string.Empty;
                                            string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
                                            int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

                                            for (int i = jlen; i <= 8; i++)
                                            {
                                                if (i == 8)
                                                {
                                                    // string randomstr = GenerateRandomString();
                                                    medicadid = s.Append(strAppend).ToString();
                                                }
                                                else
                                                {
                                                    medicadid = s.Append('0').ToString();
                                                }
                                            }
                                            clientrequestAdd.ClientMedicaidID = medicadid;
                                            clientrequestAdd.ClientID = medicadid;
                                            clientrequestAdd.ClientIdentifier = medicadid;
                                            ClientPayerId = medicadid;
                                        }
                                        else
                                        {

                                            string medicadid = string.Empty;

                                            string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);
                                            int jlen = Convert.ToString(item["MedicalId"]).Length;

                                            if (jlen >= 8)
                                            {
                                                jlen = Convert.ToString(item["MedicalId"]).Substring(1, 8).Length;
                                                StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(1, 8));

                                                for (int i = jlen; i <= 8; i++)
                                                {
                                                    if (i == 8)
                                                    {
                                                        // string randomstr = GenerateRandomString();
                                                        medicadid = s.Append(strAppendByOffice).ToString();
                                                    }
                                                    else
                                                    {
                                                        medicadid = s.Append('0').ToString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                jlen = Convert.ToString(item["MedicalId"]).Length;
                                                StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);

                                                for (int i = jlen; i <= 8; i++)
                                                {
                                                    if (i == 8)
                                                    {
                                                        // string randomstr = GenerateRandomString();
                                                        medicadid = s.Append(strAppendByOffice).ToString();
                                                    }
                                                    else
                                                    {
                                                        medicadid = s.Append('0').ToString();
                                                    }
                                                }
                                            }
                                            clientrequestAdd.ClientMedicaidID = medicadid;
                                            clientrequestAdd.ClientID = medicadid;
                                            clientrequestAdd.ClientIdentifier = medicadid;
                                            ClientPayerId = medicadid;

                                        }
                                    }

                                    clientrequestAdd.ClientTimezone = "US/Pacific";

                                    break;
                            }


                            #region
                            //if (Convert.ToInt32(item["OfficeId"]) == 5)
                            //{
                            //    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                            //}
                            //else
                            //{
                            //    objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDCASD"].ToString();
                            //}

                            //objprovider.ProviderQualifier = "MedicaidID";


                            // clientrequestAdd.ProviderIdentification = objprovider;


                            //// string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();


                            //for (int i = 0; i < strChkMedical.Length; i++)
                            //{
                            //    if (Char.IsLetterOrDigit(strChkMedical[i]))
                            //        strChkMedicalFInal += strChkMedical[i];
                            //}
                            //if (strChkMedicalFInal.Length > 0)
                            //{
                            //    ChkMedicalId = strChkMedicalFInal.ToString();
                            //}

                            //var stringNumber = ChkMedicalId;
                            //int numericValue;
                            //bool isNumber = int.TryParse(stringNumber, out numericValue);

                            //bool istrue = (Regex.IsMatch(ChkMedicalId, @"[a-zA-Z]") && Regex.IsMatch(ChkMedicalId, @"[0-9]"));

                            //if (!isNumber && istrue && Convert.ToString(item["MedicalId"]).Length >= 8)
                            //{
                            //    string medicadid = string.Empty;
                            //    int dlen = Convert.ToString(item["MedicalId"]).Length;

                            //    string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                            //    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 8), 8);

                            //    int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

                            //    for (int i = jlen; i <= 8; i++)
                            //    {

                            //        if (i == 8)
                            //        {
                            //            // string randomstr = GenerateRandomString();
                            //            medicadid = s.Append(strAppend).ToString();
                            //        }
                            //        else
                            //        {
                            //            medicadid = s.Append('0').ToString();
                            //        }
                            //    }

                            //    string str7 = medicadid;
                            //    string str8 = string.Empty;
                            //    string str9 = string.Empty;
                            //    int val8 = 0;

                            //    for (int i = 0; i < str7.Length; i++)
                            //    {
                            //        if (Char.IsLetterOrDigit(str7[i]))
                            //            str8 += str7[i];
                            //    }
                            //    if (str8.Length > 0)
                            //    {
                            //        str9 = str8.ToString();
                            //    }

                            //    clientrequestAdd.ClientMedicaidID = str9;
                            //    clientrequestAdd.ClientID = str9;
                            //    clientrequestAdd.ClientIdentifier = str9;
                            //    ClientPayerId = str9;
                            //}
                            //else
                            //{
                            //    if (!isNumber && istrue)
                            //    {
                            //        string medicadid = string.Empty;
                            //        string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                            //        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
                            //        int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

                            //        for (int i = jlen; i <= 8; i++)
                            //        {
                            //            if (i == 8)
                            //            {
                            //                // string randomstr = GenerateRandomString();
                            //                medicadid = s.Append(strAppend).ToString();
                            //            }
                            //            else
                            //            {
                            //                medicadid = s.Append('0').ToString();
                            //            }
                            //        }
                            //        clientrequestAdd.ClientMedicaidID = medicadid;
                            //        clientrequestAdd.ClientID = medicadid;
                            //        clientrequestAdd.ClientIdentifier = medicadid;
                            //        ClientPayerId = medicadid;
                            //    }
                            //    else
                            //    {

                            //        string medicadid = string.Empty;

                            //        string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);
                            //        int jlen = Convert.ToString(item["MedicalId"]).Length;

                            //        if (jlen >= 8)
                            //        {
                            //            jlen = Convert.ToString(item["MedicalId"]).Substring(1, 8).Length;
                            //            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(1, 8));

                            //            for (int i = jlen; i <= 8; i++)
                            //            {
                            //                if (i == 8)
                            //                {
                            //                    // string randomstr = GenerateRandomString();
                            //                    medicadid = s.Append(strAppendByOffice).ToString();
                            //                }
                            //                else
                            //                {
                            //                    medicadid = s.Append('0').ToString();
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            jlen = Convert.ToString(item["MedicalId"]).Length;
                            //            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);

                            //            for (int i = jlen; i <= 8; i++)
                            //            {
                            //                if (i == 8)
                            //                {
                            //                    // string randomstr = GenerateRandomString();
                            //                    medicadid = s.Append(strAppendByOffice).ToString();
                            //                }
                            //                else
                            //                {
                            //                    medicadid = s.Append('0').ToString();
                            //                }
                            //            }
                            //        }
                            //        clientrequestAdd.ClientMedicaidID = medicadid;
                            //        clientrequestAdd.ClientID = medicadid;
                            //        clientrequestAdd.ClientIdentifier = medicadid;
                            //        ClientPayerId = medicadid;

                            //    }
                            //}
                            #endregion


                            if (!Convert.ToString(item["PatientName"]).Contains(','))
                            {
                                clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"]));
                                clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"]));
                            }
                            else
                            {
                                clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"])).Split(',')[1];
                                clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"])).Split(',')[0];
                            }
                            //clientrequestAdd.ClientMedicaidID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
                        
                              clientrequestAdd.ClientQualifier = "ClientMedicaidID";
                          
                            //clientrequestAdd.ClientIdentifier = Convert.ToString(item["MedicalId"]).Substring(0, 7);

                            clientrequestAdd.ClientOtherID = Convert.ToString(item["PrimaryMD"]);

                            string str3 = Convert.ToString(item["PrimaryMD"]);
                            string str4 = string.Empty;
                            int val = 0;

                            for (int i = 0; i < str3.Length; i++)
                            {
                                if (Char.IsLetterOrDigit(str3[i]))
                                    str4 += str3[i];
                            }

                            if (str4.Length > 0)
                            {
                                clientrequestAdd.ClientOtherID = str4.ToString();
                            }

                            clientrequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];

                          //  clientrequestAdd.ClientTimezone = "US/Pacific";

                            List<ClientPayerInformation> lstclientpayer = new List<ClientPayerInformation>();

                            string procedureCodeRaw = Convert.ToString(item["ProcedureCode"]);
                            string[] procedureCodes = procedureCodeRaw
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(code => code.Trim())
                                .Where(code => !string.IsNullOrEmpty(code))
                                .ToArray();

                            foreach (string code in procedureCodes)
                            {
                                var objClientPayer = new ClientPayerInformation
                                {
                                    ClientEligibilityDateBegin = "2022-01-01",
                                    ClientEligibilityDateEnd = "2026-01-01",
                                    ClientStatus = "02",
                                    EffectiveStartDate = "2022-01-01",
                                    EffectiveEndDate = "2026-01-01",
                                    PayerID = !string.IsNullOrEmpty(Convert.ToString(item["PayerId"])) ? Convert.ToString(item["PayerId"]) : null,
                                    ClientPayerID = !string.IsNullOrEmpty(ClientPayerId) ? ClientPayerId : null,
                                    JurisdictionID = !string.IsNullOrEmpty(Convert.ToString(item["JurisdictionCode"])) ? Convert.ToString(item["JurisdictionCode"]) : null,
                                    PayerProgram = !string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])) ? Convert.ToString(item["PayerProgram"]) : null,
                                    ProcedureCode = code
                                };

                                lstclientpayer.Add(objClientPayer);
                            }

                            clientrequestAdd.ClientPayerInformation = lstclientpayer;

                            List<ClientPhones> objclientphone = new List<ClientPhones>();
                            ClientPhones clientphonesss = new ClientPhones();

                            clientphonesss.ClientPhone = Convert.ToString(item["PhoneNo"]);

                            string str1 = Convert.ToString(item["PhoneNo"]);
                            string str2 = string.Empty;
                            int val1 = 0;

                            for (int i = 0; i < str1.Length; i++)
                            {
                                if (Char.IsDigit(str1[i]))
                                    str2 += str1[i];
                            }

                            if (str2.Length > 0)
                            {
                                clientphonesss.ClientPhone = str2.ToString();
                            }

                            clientphonesss.ClientPhoneType = "Home";

                            objclientphone.Add(clientphonesss);
                            clientrequestAdd.ClientPhone = objclientphone;

                            //clientrequestAdd.ClientPhone.Add(clientphones);
                            List<ClientAddress> objclientadd = new List<ClientAddress>();
                            ClientAddress clientadd = new ClientAddress();

                            clientadd.ClientCity = Convert.ToString(item["City"]);

                            string str5 = Convert.ToString(item["City"]);
                            string str6 = string.Empty;
                            int val3 = 0;

                             for (int i = 0; i < str5.Length; i++)
                             {
                                if (Char.IsLetter(str5[i]) || char.IsWhiteSpace(str5[i]))
                                    str6 += str5[i];
                             }

                            if (str6.Length > 0)
                            {
                                clientadd.ClientCity = str6.ToString();
                            }

                            clientadd.ClientCounty = "";
                            clientadd.ClientState = Convert.ToString(item["State"]).ToUpper();
                            clientadd.ClientZip = Convert.ToString(item["ZipCode"]);
                            clientadd.ClientAddressLongitude = Convert.ToString(item["Longitude"]);
                            clientadd.ClientAddressLatitude = Convert.ToString(item["Latitude"]);
                            clientadd.ClientAddressType = "Home";
                            clientadd.ClientAddressLine1 = Convert.ToString(item["Street"]);
                            clientadd.ClientAddressLine2 = Convert.ToString(item["TimeZoneId"]);
                            clientadd.ClientAddressIsPrimary = OfficeId > 1 ? "True" : "false";
                            objclientadd.Add(clientadd);
                            clientrequestAdd.ClientAddress = objclientadd;
          
                            if (clientAddRequest.Count > 0)
                            {
                                var checkCondition = clientAddRequest.Where(xy => xy.ClientID == ClientPayerId).ToList().Select(xy => xy.ClientID == ClientPayerId).ToList();
                                if (checkCondition.Count > 0)
                                {
                                    //insertdata(Convert.ToString(item["MedicalId"]).ToString());
                                }
                                else
                                {
                                    clientAddRequest.Add(clientrequestAdd);
                                }
                            }
                            else
                            {
                                clientAddRequest.Add(clientrequestAdd);
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteServices";
                objErrorlog.Methodname = "GetAllClientRequestAndReview";
                InsertErrorLog(objErrorlog);
                return result = "False";
            }

            if (clientAddRequest.Count > 0 && clientAddRequest != null)
            {
                var arraylist = clientAddRequest.ToArray();
                List<ClientAddRequest> request = new List<ClientAddRequest>();
                foreach (var ReqItem in arraylist)
                {
                    request.Add(ReqItem);
                }

                var clientGetDialogId = new System.Net.Http.HttpClient();

                string Token = ConfigurationManager.AppSettings["mykey"].ToString();
                string actheader = ConfigurationManager.AppSettings["Token"].ToString();

                string x = JsonConvert.SerializeObject(request);

                Task.Run(async () => { result = await objModel.SubmitRequestDat(x,OfficeId); }).Wait(); ;
            }
            else
            {
                return result = "false";
            }
            return result;
        }


        public string GetAndPostClientDataForAllMed(string MedicalId)
        {
            EmployeeClientModel objModel = new EmployeeClientModel();
            string result = "";
            string ClientPayerId = "";

            //  { "A", "B", "C", "D" };

            string[] arr = { "1A", "2A", "3A", "4A","5A","6A" };
            Random random = new Random();
            int randomIndex = random.Next(arr.Length);
            string randomValue = arr[randomIndex];

            //List<EmployeeResponse> employeeLis = new List<EmployeeResponse>();

           // GetBatchClientToaddSandDataByOrganisation

            List<ClientAddRequest> clientAddRequest = new List<ClientAddRequest>();
            try
            {
                 DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetBatchClientToaddSandDataByOrganisation", MedicalId);

              //  DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientToaddSandDat", MedicalId);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        int y = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            ClientAddRequest clientrequestAdd = new ClientAddRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();
                            objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_IDMed"].ToString();
                          
                            objprovider.ProviderQualifier = "NPI";

                            clientrequestAdd.ProviderIdentification = objprovider;

                            string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();



                            //string ChkMedicalId = string.Empty;
                            //string strChkMedical = Convert.ToString(item["MedicalId"]).ToString();
                            //string strChkMedicalFInal = string.Empty;

                            //for (int i = 0; i < strChkMedical.Length; i++)
                            //{
                            //    if (Char.IsLetterOrDigit(strChkMedical[i]))
                            //        strChkMedicalFInal += strChkMedical[i];
                            //}
                            //if (strChkMedicalFInal.Length > 0)
                            //{
                            //    ChkMedicalId = strChkMedicalFInal.ToString();
                            //}

                            //var stringNumber = ChkMedicalId;
                            //int numericValue;
                            //bool isNumber = int.TryParse(stringNumber, out numericValue);

                            //bool istrue = (Regex.IsMatch(ChkMedicalId, @"[a-zA-Z]") && Regex.IsMatch(ChkMedicalId, @"[0-9]"));

                            //if (!isNumber && istrue && Convert.ToString(item["MedicalId"]).Length >= 8)
                            //{
                            //    string medicadid = string.Empty;
                            //    int dlen = Convert.ToString(item["MedicalId"]).Length;

                            //    string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                            //    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 8), 8);

                            //    int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

                            //    for (int i = jlen; i <= 8; i++)
                            //    {

                            //        if (i == 8)
                            //        {
                            //            // string randomstr = GenerateRandomString();
                            //            medicadid = s.Append(strAppend).ToString();
                            //        }
                            //        else
                            //        {
                            //            medicadid = s.Append('0').ToString();
                            //        }
                            //    }

                            //    string str7 = medicadid;
                            //    string str8 = string.Empty;
                            //    string str9 = string.Empty;
                            //    int val8 = 0;

                            //    for (int i = 0; i < str7.Length; i++)
                            //    {
                            //        if (Char.IsLetterOrDigit(str7[i]))
                            //            str8 += str7[i];
                            //    }
                            //    if (str8.Length > 0)
                            //    {
                            //        str9 = str8.ToString();
                            //    }

                            //    clientrequestAdd.ClientMedicaidID = str9;
                            //    clientrequestAdd.ClientID = str9;
                            //    clientrequestAdd.ClientIdentifier = str9;
                            //    ClientPayerId = str9;
                            //}
                            //else
                            //{
                            //    if (!isNumber && istrue)
                            //    {
                            //        string medicadid = string.Empty;
                            //        string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                            //        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
                            //        int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

                            //        for (int i = jlen; i <= 8; i++)
                            //        {
                            //            if (i == 8)
                            //            {
                            //                // string randomstr = GenerateRandomString();
                            //                medicadid = s.Append(strAppend).ToString();
                            //            }
                            //            else
                            //            {
                            //                medicadid = s.Append('0').ToString();
                            //            }
                            //        }
                            //        clientrequestAdd.ClientMedicaidID = medicadid;
                            //        clientrequestAdd.ClientID = medicadid;
                            //        clientrequestAdd.ClientIdentifier = medicadid;
                            //        ClientPayerId = medicadid;
                            //    }
                            //    else
                            //    {

                            //        string medicadid = string.Empty;

                            //        string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);
                            //        int jlen = Convert.ToString(item["MedicalId"]).Length;

                            //        if (jlen >= 8)
                            //        {

                            //            jlen = Convert.ToString(item["MedicalId"]).Substring(1, 8).Length;
                            //            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(1, 8));


                            //            for (int i = jlen; i <= 8; i++)
                            //            {
                            //                if (i == 8)
                            //                {
                            //                    // string randomstr = GenerateRandomString();
                            //                    medicadid = s.Append(strAppendByOffice).ToString();
                            //                }
                            //                else
                            //                {
                            //                    medicadid = s.Append('0').ToString();
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            jlen = Convert.ToString(item["MedicalId"]).Length;
                            //            StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);

                            //            for (int i = jlen; i <= 8; i++)
                            //            {
                            //                if (i == 8)
                            //                {
                            //                    // string randomstr = GenerateRandomString();
                            //                    medicadid = s.Append(strAppendByOffice).ToString();
                            //                }
                            //                else
                            //                {
                            //                    medicadid = s.Append('0').ToString();
                            //                }
                            //            }
                            //        }
                            //        clientrequestAdd.ClientMedicaidID = medicadid;
                            //        clientrequestAdd.ClientID = medicadid;
                            //        clientrequestAdd.ClientIdentifier = medicadid;
                            //        ClientPayerId = medicadid;

                            //    }
                            //}

                        

                            //if (!Convert.ToString(item["PatientName"]).Contains(','))
                            //{
                                clientrequestAdd.ClientFirstName = (Convert.ToString(item["FirstName"]));
                                clientrequestAdd.ClientLastName = (Convert.ToString(item["LastName"]));
                            //}
                            //else
                            //{
                            //    clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"])).Split(',')[1];
                            //    clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"])).Split(',')[0];
                            //}


                            //clientrequestAdd.ClientMedicaidID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
                       
                             clientrequestAdd.ClientQualifier = "ClientCustomID";
                          
                            //clientrequestAdd.ClientIdentifier = Convert.ToString(item["MedicalId"]).Substring(0, 7);

                            clientrequestAdd.ClientOtherID = Convert.ToString(item["PrimaryMD"]);

                            string str3 = Convert.ToString(item["PrimaryMD"]);
                            string str4 = string.Empty;
                            int val = 0;

                            for (int i = 0; i < str3.Length; i++)
                            {
                                if (Char.IsLetterOrDigit(str3[i]))
                                    str4 += str3[i];
                            }
                            if (str4.Length > 0)
                            {
                                clientrequestAdd.ClientOtherID = str4.ToString();
                            }

                            clientrequestAdd.SequenceID = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
                            // clientrequestAdd.SequenceID = Convert.ToString((int)(item["timestampdata"]) + y);
                            //Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
                            clientrequestAdd.ClientTimezone = "US/Eastern";


                            List<ClientPayerInformation> lstclientpayer = new List<ClientPayerInformation>();

                            string procedureCodeRaw = Convert.ToString(item["ProcedureCode"]);

                            string[] procedureCodes = procedureCodeRaw
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(code => code.Trim())
                            .Where(code => !string.IsNullOrEmpty(code))
                            .ToArray();

                            foreach (string code in procedureCodes)
                            {
                                var objClientPayer = new ClientPayerInformation
                                {
                                    ClientEligibilityDateBegin = "2022-01-01",
                                    ClientEligibilityDateEnd = "2026-01-01",
                                    ClientStatus = "02",
                                    EffectiveStartDate = "2022-01-01",
                                    EffectiveEndDate = "2026-01-01",
                                    PayerID = !string.IsNullOrEmpty(Convert.ToString(item["PayerId"])) ? Convert.ToString(item["PayerId"]) : null,
                                    ClientPayerID = !string.IsNullOrEmpty(ClientPayerId) ? ClientPayerId : null,
                                    JurisdictionID = !string.IsNullOrEmpty(Convert.ToString(item["JurisdictionCode"])) ? Convert.ToString(item["JurisdictionCode"]) : null,
                                    PayerProgram = !string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])) ? Convert.ToString(item["PayerProgram"]) : null,
                                    ProcedureCode = code
                                };

                                lstclientpayer.Add(objClientPayer);
                            }

                            #region
                            //if (procedureArrayCode.Contains(','))
                            //{

                            //    int ProcedureData = Convert.ToString(item["ProcedureCode"]).Split(',').Length;
                            //    string[] ProcedureArray = Convert.ToString(item["ProcedureCode"]).Split(',');

                            //    for (int i = 0; i <= ProcedureData - 1; i++)
                            //    {
                            //        ClientPayerInformation objclientPayer = new ClientPayerInformation();

                            //        objclientPayer.ClientEligibilityDateBegin = "2024-01-01";
                            //        objclientPayer.ClientEligibilityDateEnd = "2026-01-01";

                            //        objclientPayer.ClientStatus = "02";
                            //        objclientPayer.EffectiveEndDate = "2026-01-01";
                            //        objclientPayer.EffectiveStartDate = "2024-01-01";

                            //        if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                            //        {
                            //            objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
                            //        }

                            //        if (!string.IsNullOrEmpty(ClientPayerId))
                            //        {       
                            //            objclientPayer.ClientPayerID = ChkMedicalId.ToString();

                            //                //MedicalId.ToString();
                            //                //ClientPayerId;
                            //                //objclientPayer.ClientPayerID= "1231000"+randomValue;

                            //        }       

                            //        if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
                            //        {
                            //            objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
                            //        }

                            //        if (!string.IsNullOrEmpty(ProcedureArray[i]))
                            //        {
                            //            objclientPayer.ProcedureCode = Convert.ToString(ProcedureArray[i]);
                            //        }


                            //        lstclientpayer.Add(objclientPayer);
                            //    }
                            //}
                            //else
                            //{

                            //    ClientPayerInformation objclientPayer = new ClientPayerInformation();

                            //    objclientPayer.ClientEligibilityDateBegin = "2024-01-01";
                            //    objclientPayer.ClientEligibilityDateEnd = "2026-01-01";

                            //    objclientPayer.ClientStatus = "02";
                            //    objclientPayer.EffectiveEndDate = "2026-01-01";
                            //    objclientPayer.EffectiveStartDate = "2024-01-01";

                            //    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                            //    {
                            //        objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
                            //    }


                            //    if (!string.IsNullOrEmpty(ClientPayerId))
                            //    {
                            //        objclientPayer.ClientPayerID = ChkMedicalId.ToString();

                            //            //MedicalId.ToString() ;
                            //        //ClientPayerId;
                            //        //  objclientPayer.ClientPayerID= "1231000"+randomValue;
                            //    }

                            //    //if (!string.IsNullOrEmpty(Convert.ToString(item["JurisdictionCode"])))
                            //    //{
                            //    //    objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
                            //    //}

                            //    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
                            //    {
                            //        objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
                            //    }

                            //    if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"]).ToString()))
                            //    {
                            //        objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]).ToString();
                            //    }

                            //    lstclientpayer.Add(objclientPayer);
                            //}

                            #endregion

                            clientrequestAdd.ClientPayerInformation = lstclientpayer;

                            List<ClientPhones> objclientphone = new List<ClientPhones>();
                            ClientPhones clientphonesss = new ClientPhones();

                            clientphonesss.ClientPhone = Convert.ToString(item["PhoneNo"]);

                            string str1 = Convert.ToString(item["PhoneNo"]);
                            string str2 = string.Empty;
                            int val1 = 0;

                            for (int i = 0; i < str1.Length; i++)
                            {
                                if (Char.IsDigit(str1[i]))
                                    str2 += str1[i];
                            }

                            if (str2.Length > 0)
                            {
                                clientphonesss.ClientPhone = str2.ToString();
                            }

                            clientphonesss.ClientPhoneType = "Home";

                            objclientphone.Add(clientphonesss);
                            clientrequestAdd.ClientPhone = objclientphone;

                            //clientrequestAdd.ClientPhone.Add(clientphones);
                            List<ClientAddress> objclientadd = new List<ClientAddress>();
                            ClientAddress clientadd = new ClientAddress();

                            clientadd.ClientCity = Convert.ToString(item["City"]);

                            string str5 = Convert.ToString(item["City"]);
                            string str6 = string.Empty;
                            int val3 = 0;

                            for (int i = 0; i < str5.Length; i++)
                            {
                                if (Char.IsLetter(str5[i]) || char.IsWhiteSpace(str5[i]))
                                    str6 += str5[i];
                            }

                            if (str6.Length > 0)
                            {
                                clientadd.ClientCity = str6.ToString();
                            }

                            clientadd.ClientCounty = Convert.ToString(item["city"]).ToUpper().Replace(",","");
                            clientadd.ClientState = Convert.ToString(item["State"]).ToUpper();
                            clientadd.ClientZip = Convert.ToString(item["ZipCode"]);
                            clientadd.ClientAddressLongitude = Convert.ToString(item["Longitude"]);
                            clientadd.ClientAddressLatitude = Convert.ToString(item["Latitude"]);
                            clientadd.ClientAddressType = "Home";
                            clientadd.ClientAddressLine1 = Convert.ToString(item["Street"]);
                            clientadd.ClientAddressLine2 = Convert.ToString(item["TimeZoneId"]);
                            clientadd.ClientAddressIsPrimary = "true";
                            objclientadd.Add(clientadd);
                            clientrequestAdd.ClientAddress = objclientadd;

                            // overwriting and this will be commented in future when it will be on live server
                            // Console.WriteLine("Random value: " + randomValue);

                            clientrequestAdd.ClientID = ChkMedicalId.ToString();
                            clientrequestAdd.ClientIdentifier = clientrequestAdd.ClientID;
                            clientrequestAdd.ClientMedicaidID = clientrequestAdd.ClientID;

                            if (clientAddRequest.Count > 0)
                            {
                                var checkCondition = clientAddRequest.Where(xy => xy.ClientID == ClientPayerId).ToList().Select(xy => xy.ClientID == ClientPayerId).ToList();
                                if (checkCondition.Count > 0)
                                {
                                    //insertdata(Convert.ToString(item["MedicalId"]).ToString());
                                }
                                else
                                {
                                    clientAddRequest.Add(clientrequestAdd);
                                }
                            }
                            else
                            {
                                clientAddRequest.Add(clientrequestAdd);
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteServices";
                objErrorlog.Methodname = "GetAllClientRequestAndReview";
                InsertErrorLog(objErrorlog);
                return result = "False";
            }

            if (clientAddRequest.Count > 0 && clientAddRequest != null)
            {
                var arraylist = clientAddRequest.ToArray();
                List<ClientAddRequest> request = new List<ClientAddRequest>();
                foreach (var ReqItem in arraylist)
                {
                    request.Add(ReqItem);
                }

                var clientGetDialogId = new System.Net.Http.HttpClient();
                string Token = ConfigurationManager.AppSettings["mykeyMed"].ToString();
                string actheader = ConfigurationManager.AppSettings["TokenMed"].ToString();

                string x = JsonConvert.SerializeObject(request);

                Task.Run(async () => { result = await objModel.SubmitRequestDatAllMed(x); }).Wait(); ;
            }
            else
            {
                return result = "false";
            }
            return result;
        }

        public bool IsAlpha(String strToCheck)
        {
            Regex objAlphaPattern = new Regex("[\u0027^(CADDS|CAIHSS|CAMCWP|CAMSSP|CAHHA|CACCS|CAHCBA)$\u0027]");
            bool istrue = objAlphaPattern.IsMatch(strToCheck);

            return istrue;
        }


        // Function to Check for AlphaNumeric.  
        public bool IsAlphaNumeric(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[\u0027^(G0156|S5130|S9122|T1005|T1019|T2017|Z8561|Z8562|Z8563|Z8574|Z8575|Z9027|Z9028|Z9029|Z9030|Z9081|Z9111|Z9125|Z9525|S9125|H2014|T2020|G0299|G0300|G0162|T1002|T1003|Z9232|Z9234|Z9245|Z9248|Z9249|S9123|S9124|Z9073|99501|99502|99600|G0151|G0152|G0153|G0154|G0155|T1030|T1031|Z9403|Z9046|Z9047|Z9010|Z9011|Z9102|Z9026|Z9211|Z9214|Z9217)$\u0027]");

            bool istrue = objAlphaNumericPattern.IsMatch(strToCheck);
            return istrue;
        }


        public bool IsAlphaNumericAlpha(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("^[A-Z]{2}[0-9]{4}$");

            bool istrue = objAlphaNumericPattern.IsMatch(strToCheck);
            return istrue;
        }


        public bool IsAlphaNumericAlphaCheckMedicald(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("^[A-Z0-9]{4}$");

            bool istrue = objAlphaNumericPattern.IsMatch(strToCheck);
            return istrue;
        }

        public bool IsAlphaNumericData(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[\u0027^(PCS|HHCS)$\u0027]");

            bool istrue = objAlphaNumericPattern.IsMatch(strToCheck);
            return true;
        }

        public bool IsNumeric(String strToCheck)
        {
            Regex objNotNaturalPattern = new Regex("^[0-9]{1,9}$");
            bool istrue = (objNotNaturalPattern.IsMatch(strToCheck));

            return istrue;
        }


        //private string GenerateRandomString()
        //{
        //    Random rnd = new Random();
        //    string txtRand = string.Empty;
        //    for (int i = 0; i < 8; i++) txtRand += ((char)rnd.Next(65, 90)).ToString();
        //    return txtRand;
        //}


        //private void insertClientData(String Id, string UDID)
        //{
        //    string result = "Testing";
        //    try
        //    {
        //        int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertDataClient", Id,UDID);

        //        if (i > 0)
        //        {
        //            result = "Success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog objErrorlog = new ErrorLog();
        //        //objErrorlog.Errormessage = ex.Message;
        //        //objErrorlog.StackTrace = ex.StackTrace;
        //        //objErrorlog.Pagename = "CareGiverLiteService";
        //        //objErrorlog.Methodname = "InsertScheduleForNurse";
        //        //objErrorlog.UserID = CareGiverSchedule.UserId;
        //        //result = InsertErrorLog(objErrorlog);
        //    }
        //    //  return result;
        //}


        [HttpGet]
        public ActionResult SendClientRequestDataPage()
        {
            return PartialView("SendClientRequestDataPage");
        }
        [HttpPost]
        public JsonResult GetAndPostClientDataByDateRange(string FromDate, String ToDate, String OfficeId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            EmployeeClientModel objModel = new EmployeeClientModel();
            string APIresult = "";
            string ClientPayerId = "";

            //insertdata(FromDate);

            //DateTime fromDatetime = Convert.ToDateTime(FromDate);
            //string FromScheduleDateTime = fromDatetime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

            //DateTime ToDatetime = Convert.ToDateTime(ToDate);
            //string ToScheduleDateTime = ToDatetime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");


            List<ClientAddRequest> clientAddRequest = new List<ClientAddRequest>();
            try
            {


                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetClientToaddSandDatByDateRange",
                                                                                                                 FromDate,
                                                                                                                 ToDate,
                                                                                                                 OfficeId);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int y = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            ClientAddRequest clientrequestAdd = new ClientAddRequest();

                            ProviderIdentification objprovider = new ProviderIdentification();
                            objprovider.ProviderID = ConfigurationManager.AppSettings["Provider_ID"].ToString();
                            objprovider.ProviderQualifier = "MedicaidID";
                            clientrequestAdd.ProviderIdentification = objprovider;

                            //  string ChkMedicalId = Convert.ToString(item["MedicalId"]).ToString();

                            string ChkMedicalId = string.Empty;
                            string strChkMedical = Convert.ToString(item["MedicalId"]).ToString();
                            string strChkMedicalFInal = string.Empty;
                            

                            for (int i = 0; i < strChkMedical.Length; i++)
                            {
                                if (Char.IsLetterOrDigit(strChkMedical[i]))
                                    strChkMedicalFInal += strChkMedical[i];
                            }
                            if (strChkMedicalFInal.Length > 0)
                            {
                                ChkMedicalId = strChkMedicalFInal.ToString();
                            }

                            var stringNumber = ChkMedicalId;
                            int numericValue;
                            bool isNumber = int.TryParse(stringNumber, out numericValue);

                            bool istrue = (Regex.IsMatch(ChkMedicalId, @"[a-zA-Z]") && Regex.IsMatch(ChkMedicalId, @"[0-9]"));
                            
                            if (!isNumber && istrue && Convert.ToString(item["MedicalId"]).Length >= 8)
                            {
                                string medicadid = string.Empty;
                                int dlen = Convert.ToString(item["MedicalId"]).Length;

                                //insertdata(y.ToString());

                                 string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                 StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen-8), 8);
                                
                                StringBuilder s5 = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen-8),8);
                                int jlen = Convert.ToString(item["MedicalId"]).Substring(2, 8).Length;

                                StringBuilder s2 = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 9), 8);

                                for (int i = jlen; i <= 8; i++)
                                {
                                    StringBuilder s4 = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(dlen - 9),9);
                                    if (i == 8)
                                    {
                                        // string randomstr = GenerateRandomString();
                                        medicadid = s.Append(strAppend).ToString();
                                    }
                                    else
                                    {
                                        medicadid = s.Append('0').ToString();
                                    }
                                }

                                string str7 = medicadid;
                                string str8 = string.Empty;
                                string str9 = string.Empty;
                                int val8 = 0;

                                for (int i = 0; i < str7.Length; i++)
                                {
                                    if (Char.IsLetterOrDigit(str7[i]))
                                    {
                                        str8 += str7[i];
                                    }
                                    else
                                    {
                                        str8 += "0";
                                    }
                                }
                                if (str8.Length > 0)
                                {
                                     str9= str8.ToString();
                                }

                                clientrequestAdd.ClientMedicaidID = str9;
                                clientrequestAdd.ClientID = str9;
                                clientrequestAdd.ClientIdentifier = str9;
                                ClientPayerId = str9;
                            }
                            else
                            {
                                if (!isNumber && istrue)
                                {
                                    string medicadid = string.Empty;
                                    string strAppend = Convert.ToString(item["MedicalId"]).Substring(1, 1);
                                    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)), 8);
                                    int jlen = Convert.ToString(item["MedicalId"]).Substring(2, ((Convert.ToString(item["MedicalId"]).Length) - 2)).Length;

                                    for (int i = jlen; i <= 8; i++)
                                    {
                                        if (i == 8)
                                        {
                                            // string randomstr = GenerateRandomString();
                                            medicadid = s.Append(strAppend).ToString();
                                        }
                                        else
                                        {
                                            medicadid = s.Append('0').ToString();
                                        }
                                    }
                                    clientrequestAdd.ClientMedicaidID = medicadid;
                                    clientrequestAdd.ClientID = medicadid;
                                    clientrequestAdd.ClientIdentifier = medicadid;
                                    ClientPayerId = medicadid;
                                }
                                else
                                {

                                    string medicadid = string.Empty;

                                    string strAppendByOffice = Convert.ToString(item["officeName"]).Substring((Convert.ToString(item["officeName"]).Length - 1), 1);
                                    int jlen = Convert.ToString(item["MedicalId"]).Length;

                                    if (jlen >= 8)
                                    {

                                        jlen = Convert.ToString(item["MedicalId"]).Substring(1, 8).Length;
                                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]).Substring(1, 8));


                                        for (int i = jlen; i <= 8; i++)
                                        {
                                            if (i == 8)
                                            {
                                                // string randomstr = GenerateRandomString();
                                                medicadid = s.Append(strAppendByOffice).ToString();
                                            }
                                            else
                                            {
                                                medicadid = s.Append('0').ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        jlen = Convert.ToString(item["MedicalId"]).Length;
                                        StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);

                                        for (int i = jlen; i <= 8; i++)
                                        {
                                            if (i == 8)
                                            {
                                                // string randomstr = GenerateRandomString();
                                                medicadid = s.Append(strAppendByOffice).ToString();
                                            }
                                            else
                                            {
                                                medicadid = s.Append('0').ToString();
                                            }
                                        }
                                    }
                                    clientrequestAdd.ClientMedicaidID = medicadid;
                                    clientrequestAdd.ClientID = medicadid;
                                    clientrequestAdd.ClientIdentifier = medicadid;
                                    ClientPayerId = medicadid;
                                }
                            }



                            //if (Convert.ToString(item["MedicalId"]).Length >= 8)
                            //{
                            //    clientrequestAdd.ClientMedicaidID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
                            //    clientrequestAdd.ClientID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
                            //    clientrequestAdd.ClientIdentifier = Convert.ToString(item["MedicalId"]).Substring(0, 7);
                            //}
                            //else
                            //{
                            //    string medicadid = string.Empty;

                            //    int jlen = Convert.ToString(item["MedicalId"]).Length;
                            //    StringBuilder s = new StringBuilder(Convert.ToString(item["MedicalId"]), 8);
                            //    for (int i = jlen; i <= 8; i++)
                            //    {
                            //        if (i == 8)
                            //        {
                            //            // string randomstr = GenerateRandomString();
                            //            medicadid = s.Append('C').ToString();
                            //        }
                            //        else
                            //        {
                            //            medicadid = s.Append('0').ToString();
                            //        }
                            //    }
                            //    clientrequestAdd.ClientMedicaidID = medicadid;
                            //    clientrequestAdd.ClientID = medicadid;
                            //    clientrequestAdd.ClientIdentifier = medicadid;
                            //}

                            if (!Convert.ToString(item["PatientName"]).Contains(','))
                            {
                                clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"]));
                                clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"]));
                            }
                            else
                            {
                                clientrequestAdd.ClientFirstName = (Convert.ToString(item["PatientName"])).Split(',')[1];
                                clientrequestAdd.ClientLastName = (Convert.ToString(item["PatientName"])).Split(',')[0];
                            }
                            //clientrequestAdd.ClientMedicaidID = Convert.ToString(item["MedicalId"]).Substring(0, 7);
                            clientrequestAdd.ClientQualifier = "ClientMedicaidID";
                            //clientrequestAdd.ClientIdentifier = Convert.ToString(item["MedicalId"]).Substring(0, 7);

                            clientrequestAdd.ClientOtherID = Convert.ToString(item["PrimaryMD"]);

                            string str3 = Convert.ToString(item["PrimaryMD"]);
                            string str4 = string.Empty;
                            int val = 0;

                            for (int i = 0; i < str3.Length; i++)
                            {
                                if (Char.IsLetterOrDigit(str3[i]))
                                    str4 += str3[i];
                            }
                            if (str4.Length > 0)
                            {
                                clientrequestAdd.ClientOtherID = str4.ToString();
                            }


                            clientrequestAdd.SequenceID = Convert.ToString((int)(item["timestampdata"]) + y);

                            //Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).Split('.')[0];
                            clientrequestAdd.ClientTimezone = "US/Pacific";


                            List<ClientPayerInformation> lstclientpayer = new List<ClientPayerInformation>();

                            string procedureArrayCode = Convert.ToString(item["ProcedureCode"]).ToString();

                            if (procedureArrayCode.Contains(','))
                            {
                                int ProcedureData = Convert.ToString(item["ProcedureCode"]).Split(',').Length;
                                string[] ProcedureArray = Convert.ToString(item["ProcedureCode"]).Split(',');

                                for (int i = 0; i <= ProcedureData - 1; i++)
                                {
                                    ClientPayerInformation objclientPayer = new ClientPayerInformation();

                                    objclientPayer.ClientEligibilityDateBegin = "2022-01-01";
                                    objclientPayer.ClientEligibilityDateEnd = "2024-01-01";

                                    objclientPayer.ClientStatus = "02";
                                    objclientPayer.EffectiveEndDate = "2024-01-01";
                                    objclientPayer.EffectiveStartDate = "2022-01-01";

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                                    {
                                        objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
                                    }
                                    //else
                                    //{
                                    //    objclientPayer.PayerID = "CAMCWP";
                                    //}

                                    if (!string.IsNullOrEmpty(ClientPayerId))
                                    {
                                        objclientPayer.ClientPayerID = ClientPayerId;
                                    }
                                    //else
                                    //{
                                    //    objclientPayer.ClientPayerID = "987654321";
                                    //}

                                    if (!string.IsNullOrEmpty(Convert.ToString(item["JurisdictionCode"])))
                                    {
                                        objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
                                    }
                                    //else
                                    //{
                                    //    objclientPayer.JurisdictionID = "ASN";
                                    //}


                                    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
                                    {
                                        objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
                                    }
                                    //else
                                    //{
                                    //    objclientPayer.PayerProgram = "HHCS";
                                    //}

                                    //if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                    //{
                                    //    objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
                                    //}


                                    if (!string.IsNullOrEmpty(ProcedureArray[i]))
                                    {
                                        objclientPayer.ProcedureCode = Convert.ToString(ProcedureArray[i]);
                                    }

                                    //else
                                    //{

                                    //    objclientPayer.ProcedureCode = "S9124";
                                    //}
                                    //^[A - Za - z\d]{ 4}\d{ 6,7}\z


                                    //if (IsNumeric(Convert.ToString(item["ClientPayerId"])))
                                    //{
                                    //    objclientPayer.ClientPayerID = Convert.ToString(item["ClientPayerId"]);
                                    //}
                                    //else
                                    //{
                                    //    objclientPayer.ClientPayerID = "987654321";
                                    //}

                                    //if (IsAlphaNumericAlpha(Convert.ToString(item["JurisdictionCode"])))
                                    //{
                                    //    objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
                                    //}
                                    //else
                                    //{
                                    //    objclientPayer.JurisdictionID = "SDiego37";
                                    //}


                                    //if (IsAlpha(Convert.ToString(item["PayerId"])))
                                    //{
                                    //    objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
                                    //}
                                    //else
                                    //{
                                    //    objclientPayer.PayerID = "CAMCWP";
                                    //}

                                    //if (IsAlphaNumericData(Convert.ToString(item["PayerProgram"])))
                                    //{
                                    //    objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
                                    //}
                                    //else
                                    //{
                                    //    objclientPayer.PayerProgram = "HHCS";
                                    //}

                                    //if (IsAlphaNumeric(Convert.ToString(item["ProcedureCode"])))
                                    //{
                                    //    objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
                                    //}
                                    //else
                                    //{
                                    //    objclientPayer.ProcedureCode = "S9124";
                                    //}


                                    lstclientpayer.Add(objclientPayer);
                                }
                            }
                            else
                            {

                                ClientPayerInformation objclientPayer = new ClientPayerInformation();

                                objclientPayer.ClientEligibilityDateBegin = "2022-01-01";
                                objclientPayer.ClientEligibilityDateEnd = "2024-01-01";

                                objclientPayer.ClientStatus = "02";
                                objclientPayer.EffectiveEndDate = "2024-01-01";
                                objclientPayer.EffectiveStartDate = "2022-01-01";

                                if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                                {
                                    objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
                                }
                                //else
                                //{
                                //    objclientPayer.PayerID = "CAMCWP";
                                //}

                                if (!string.IsNullOrEmpty(ClientPayerId))
                                {
                                    objclientPayer.ClientPayerID = ClientPayerId;
                                }
                                //else
                                //{
                                //    objclientPayer.ClientPayerID = "987654321";
                                //}

                                if (!string.IsNullOrEmpty(Convert.ToString(item["JurisdictionCode"])))
                                {
                                    objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
                                }
                                //else
                                //{
                                //    objclientPayer.JurisdictionID = "ASN";
                                //}


                                if (!string.IsNullOrEmpty(Convert.ToString(item["PayerProgram"])))
                                {
                                    objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
                                }
                                //else
                                //{
                                //    objclientPayer.PayerProgram = "HHCS";
                                //}

                                if (!string.IsNullOrEmpty(Convert.ToString(item["ProcedureCode"])))
                                {
                                    objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
                                }
                                //else
                                //{

                                //    objclientPayer.ProcedureCode = "S9124";
                                //}
                                //^[A - Za - z\d]{ 4}\d{ 6,7}\z


                                //if (IsNumeric(Convert.ToString(item["ClientPayerId"])))
                                //{
                                //    objclientPayer.ClientPayerID = Convert.ToString(item["ClientPayerId"]);
                                //}
                                //else
                                //{
                                //    objclientPayer.ClientPayerID = "987654321";
                                //}

                                //if (IsAlphaNumericAlpha(Convert.ToString(item["JurisdictionCode"])))
                                //{
                                //    objclientPayer.JurisdictionID = Convert.ToString(item["JurisdictionCode"]);
                                //}
                                //else
                                //{
                                //    objclientPayer.JurisdictionID = "SDiego37";
                                //}


                                //if (IsAlpha(Convert.ToString(item["PayerId"])))
                                //{
                                //    objclientPayer.PayerID = Convert.ToString(item["PayerId"]);
                                //}
                                //else
                                //{
                                //    objclientPayer.PayerID = "CAMCWP";
                                //}

                                //if (IsAlphaNumericData(Convert.ToString(item["PayerProgram"])))
                                //{
                                //    objclientPayer.PayerProgram = Convert.ToString(item["PayerProgram"]);
                                //}
                                //else
                                //{
                                //    objclientPayer.PayerProgram = "HHCS";
                                //}

                                //if (IsAlphaNumeric(Convert.ToString(item["ProcedureCode"])))
                                //{
                                //    objclientPayer.ProcedureCode = Convert.ToString(item["ProcedureCode"]);
                                //}
                                //else
                                //{
                                //    objclientPayer.ProcedureCode = "S9124";
                                //}


                                lstclientpayer.Add(objclientPayer);

                            }
                            clientrequestAdd.ClientPayerInformation = lstclientpayer;

                            List<ClientPhones> objclientphone = new List<ClientPhones>();
                            ClientPhones clientphonesss = new ClientPhones();

                            clientphonesss.ClientPhone = Convert.ToString(item["PhoneNo"]);

                            string str1 = Convert.ToString(item["PhoneNo"]);
                            string str2 = string.Empty;
                            int val1 = 0;
                            
                            for (int i = 0; i < str1.Length; i++)
                            {
                                if (Char.IsDigit(str1[i]))
                                    str2 += str1[i];
                            }
                            if (str2.Length > 0)
                            {                              
                                clientphonesss.ClientPhone = str2.ToString();
                            }

                            clientphonesss.ClientPhoneType = "Home";

                            objclientphone.Add(clientphonesss);
                            clientrequestAdd.ClientPhone = objclientphone;

                            //clientrequestAdd.ClientPhone.Add(clientphones);
                            List<ClientAddress> objclientadd = new List<ClientAddress>();
                            ClientAddress clientadd = new ClientAddress();

                            clientadd.ClientCity = Convert.ToString(item["City"]).ToUpper(); ;

                            string str5 = Convert.ToString(item["City"]);
                            string str6 = string.Empty;
                            int val3 = 0;

                            for (int i = 0; i < str5.Length; i++)
                            {
                                if (Char.IsLetter(str5[i]) || char.IsWhiteSpace(str5[i]))
                                    str6 += str5[i];
                            }
                            if (str6.Length > 0)
                            {
                                clientadd.ClientCity = str6.ToString().ToUpper();
                            }

                            clientadd.ClientCounty = "";
                            clientadd.ClientState = Convert.ToString(item["State"]).ToUpper();
                            clientadd.ClientZip = Convert.ToString(item["ZipCode"]);
                            clientadd.ClientAddressLongitude = Convert.ToString(item["Longitude"]);
                            clientadd.ClientAddressLatitude = Convert.ToString(item["Latitude"]);
                            clientadd.ClientAddressType = "Home";
                            clientadd.ClientAddressLine1 = Convert.ToString(item["Street"]);
                            clientadd.ClientAddressLine2 = Convert.ToString(item["TimeZoneId"]);
                            clientadd.ClientAddressIsPrimary = "True";
                            objclientadd.Add(clientadd);
                            clientrequestAdd.ClientAddress = objclientadd;

                            if (clientAddRequest.Count > 0)
                            {
                                  var checkCondition = clientAddRequest.Where(xy => xy.ClientID == ClientPayerId).ToList().Select(xy => xy.ClientID == ClientPayerId).ToList();
                                if (checkCondition.Count > 0)
                                {
                                    //insertdata(Convert.ToString(item["MedicalId"]).ToString());
                                    break;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                                    {
                                        clientAddRequest.Add(clientrequestAdd);
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(item["PayerId"])))
                                {
                                    clientAddRequest.Add(clientrequestAdd);
                                }
                            }
                            y = y + 1;
                        } 

                        }
                    }

                    if (clientAddRequest.Count <= 0)
                    {
                        result["Success"] = false;
                        result["Message"] = "No data Available to send";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CaregiverLiteWebServices";
                objErrorlog.Methodname = "GetAllClientRequestAndReview";
                InsertErrorLog(objErrorlog);
                result["Success"] = false;
                result["Message"] = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var arraylist = clientAddRequest.ToArray();

            List<ClientAddRequest> request = new List<ClientAddRequest>();

            foreach (var ReqItem in arraylist)
            {
                request.Add(ReqItem);
            }

            var clientGetDialogId = new System.Net.Http.HttpClient();

            string Token = ConfigurationManager.AppSettings["mykey"].ToString();
            string actheader = ConfigurationManager.AppSettings["Token"].ToString();

            string x = JsonConvert.SerializeObject(request);


          //  insertdata(x);

            // string x = @"[{""ProviderIdentification"": { ""ProviderQualifier"": ""MedicadID"",""Provider ID"": ""000000077""},""EmployeeQualifier"": ""EmployeeCustomID"",""EmployeeIdentifier"": ""999999999"",""EmployeeOtherID"": ""999999999"",""SequenceID"": 99811930002,""EmployeeLastName"": ""Employee"",""EmployeeFirstName"": ""Test"",""EmployeeEmail"": ""dummy@sandata.com""}]";
            //try
            //{

            //    clientGetDialogId.BaseAddress = new Uri("https://uat-api.sandata.com/interfaces/intake/clients/rest/api/v1.1");
            //    clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
            //    clientGetDialogId.DefaultRequestHeaders.Add("Authorization", Token);
            //    clientGetDialogId.DefaultRequestHeaders.Add("account", actheader);
            //    var content2 = new StringContent(x, Encoding.UTF8, "application/json");

            //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //    var response2 = await clientGetDialogId.PostAsync("", content2);


            //    result1 = response2.Content.ReadAsStringAsync().Result;


            //}
            //catch (Exception ex)
            //{

            //}


            Task.Run(async () => { APIresult = await objModel.SubmitRequestDatMultiple(x); }).Wait();
            if (APIresult != "")
            {
                if (APIresult.Contains("FAILED"))
                {
                    result["Success"] = false;
                    result["Message"] = "Data not Sent";
                }
                else
                {
                    result["Success"] = true;
                    result["Message"] = "Data Sent";
                }
            }
            else
            {
                result["Success"] = false;
                result["Message"] = "Data not Sent";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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

        public string ActiveDeactiveCustomer(string id, string PageName, bool IsActive)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ActiveDeactiveCustomer",
                      id, PageName, IsActive
                     );
                if (i > 0)
                {
                    result = "Success";
                }
                else
                {
                    result = "Fail";
                }
            }
            catch (SqlException  ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "ActiveDeactiveCustomer";
                objErrorlog.Methodname = "InsertErrorLog";
                objErrorlog.UserID = "";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


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

    }
}