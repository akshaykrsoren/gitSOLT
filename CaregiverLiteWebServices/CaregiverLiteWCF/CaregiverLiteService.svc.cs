using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DifferenzLibrary;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Configuration;
using CaregiverLiteWCF.Class;
using System.Web.Security;
using System.Web;
using System.Net.Mail;
using System.Security;
using System.ServiceModel.Web;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace CaregiverLiteWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CaregiverLiteService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CaregiverLiteService.svc or CaregiverLiteService.svc.cs at the Solution Explorer and start debugging.
    public class CaregiverLiteService : ICaregiverLiteService
    {
        // GLOBLE VARIABLES
        string CareGiverDocumentPath = ConfigurationManager.AppSettings["CareGiverDocumentsURL"].ToString();
        string CareGiverProfileImagesPath = ConfigurationManager.AppSettings["CareGiverProfileImagesURL"].ToString();
        string CareGiverProfileVideoURL = ConfigurationManager.AppSettings["CareGiverProfileVideoURL"].ToString();
        string CareGiverProfileCertificate = ConfigurationManager.AppSettings["CareGiverProfileCertificate"].ToString();
        // ERROR LOG 
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

        // LOGS
        public string InsertLog(Logs objLogs)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertLog",
                   objLogs.PageName, objLogs.Message, new Guid(objLogs.UserID)
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
                objErrorlog.Methodname = "InsertLog";
                objErrorlog.UserID = objLogs.UserID;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }



        //SERVICES
        public List<Services> GetAllServices(string OrganisationId)
        {
            List<Services> ServiceList = new List<Services>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllServices",Convert.ToInt32(OrganisationId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Services objService = new Services();
                        objService.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objService.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objService.Description = ds.Tables[0].Rows[i]["Description"].ToString();

                        ServiceList.Add(objService);
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
            return ServiceList;
        }


        public string InsertUpdateService(Services Service)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateService",
                                                    Service.ServiceId,
                                                    Service.ServiceName,
                                                    new Guid(Service.UserId),
                                                    Convert.ToInt32(Service.OrganisationId),
                                                    Service.Description);
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
                objErrorlog.Methodname = "InsertUpdateService";
                objErrorlog.UserID = Service.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string DeleteService(string ServiceId, string UserId)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteService",
                                                        ServiceId,
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
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "DeleteService";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

      
        // Care Giver
        public CareGiversList GetAllCareGivers(string LoginUserId, string pageNo, string recordPerPage, string IsApprove, string sortfield, string sortorder, string search, string FilterOffice, string OrganisationId)
        {
            CareGiversList ListCareGiver = new CareGiversList();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllCareGiversToCheckPayrollId",
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllCareGivers",ORG_GetAllCareGivers
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllCareGivers_Testing",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllCareGivers",
                                                        LoginUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        Convert.ToInt32(IsApprove),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        FilterOffice,                                                      
                                                        OrganisationId
                                                        //,
                                                       // IsActive
                                                        );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    List<CareGivers> CareGiverList = new List<CareGivers>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        CareGivers objCareGiver = new CareGivers();
                        objCareGiver.NurseId = Convert.ToInt32(ds.Tables[1].Rows[i]["NurseId"]);
                        objCareGiver.UserId = ds.Tables[1].Rows[i]["UserId"].ToString();
                        objCareGiver.Name = ds.Tables[1].Rows[i]["Name"].ToString();

                        objCareGiver.FirstName = ds.Tables[1].Rows[i]["FirstName"].ToString();
                        objCareGiver.LastName = ds.Tables[1].Rows[i]["LastName"].ToString();
                        objCareGiver.Email = ds.Tables[1].Rows[i]["Email"].ToString();
                        objCareGiver.ServiceId = ds.Tables[1].Rows[i]["Service"].ToString();
                        objCareGiver.ServiceName = ds.Tables[1].Rows[i]["ServiceName"].ToString();
                        objCareGiver.HourlyRate = Convert.ToDecimal(ds.Tables[1].Rows[i]["HourlyRate"].ToString());
                        objCareGiver.ChargeToPatient = Convert.ToDecimal(ds.Tables[1].Rows[i]["ChargeToPatient"].ToString());
                        objCareGiver.DistanceFromLocation = Convert.ToDecimal(ds.Tables[1].Rows[i]["DistanceFromLocation"].ToString());
                        objCareGiver.Phone = ds.Tables[1].Rows[i]["Phone"].ToString();
                        objCareGiver.Address = ds.Tables[1].Rows[i]["Address"].ToString();
                        objCareGiver.Street = ds.Tables[1].Rows[i]["Street"].ToString();
                        objCareGiver.City = ds.Tables[1].Rows[i]["City"].ToString();
                        objCareGiver.State = ds.Tables[1].Rows[i]["State"].ToString();
                        objCareGiver.ZipCode = ds.Tables[1].Rows[i]["ZipCode"].ToString();
                        objCareGiver.Office = ds.Tables[1].Rows[i]["Office"].ToString();
                        // objCareGiver.InsertedOn = ds.Tables[0].Rows[i]["InsertDateTime"].ToString();
                        objCareGiver.OfficeId = Convert.ToInt32(ds.Tables[1].Rows[i]["OfficeId"]);

                        if (ds.Tables[1].Rows[i]["PayrollId"].ToString() == "" && ds.Tables[1].Rows[i]["PayrollId"].ToString() == null)
                        {
                            objCareGiver.PayrollId = "";
                        }
                        else
                        {
                            objCareGiver.PayrollId = ds.Tables[1].Rows[i]["PayrollId"].ToString();
                        }

                        objCareGiver.InsertedOn = Convert.ToDateTime(ds.Tables[1].Rows[i]["InsertDateTime"]).ToString("dd MMM yyyy hh:mm tt");

                        var ProfileImage = ds.Tables[1].Rows[i]["ProfileImage"].ToString();
                        //if (ProfileImage != null)
                        if(!string.IsNullOrEmpty(ProfileImage))
                        {
                            objCareGiver.ProfileImage = CareGiverProfileImagesPath + ProfileImage;
                        }
                        else
                        {
                            objCareGiver.ProfileImage = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                        }

                        objCareGiver.ProfileVideo = (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["ProfileVideo"].ToString())) ? CareGiverProfileVideoURL + ds.Tables[1].Rows[i]["ProfileVideo"].ToString() : "";
                        objCareGiver.ProfileVideoThumbnil = (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["ProfileVideoThumbnil"].ToString())) ? CareGiverProfileImagesPath + ds.Tables[1].Rows[i]["ProfileVideoThumbnil"].ToString() : "";

                        objCareGiver.Certificate = ds.Tables[1].Rows[i]["Certificate"].ToString();
                        objCareGiver.IsAvailable = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsAvailable"].ToString());
                        objCareGiver.IsBusy = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsBusy"].ToString());
                        objCareGiver.IsApprove = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsApprove"].ToString());
                        objCareGiver.Latitude = ds.Tables[1].Rows[i]["Latitude"].ToString();
                        objCareGiver.Longitude = ds.Tables[1].Rows[i]["Longitude"].ToString();
                        objCareGiver.DistanceUnit = ds.Tables[1].Rows[i]["DistanceUnit"].ToString();
                        objCareGiver.UserName = ds.Tables[1].Rows[i]["UserName"].ToString();
                        objCareGiver.Password = ds.Tables[1].Rows[i]["Password"].ToString();
                        objCareGiver.CanAdminEdit = Convert.ToBoolean(ds.Tables[1].Rows[i]["CanAdminEdit"].ToString());
                        objCareGiver.QuickBloxId = ds.Tables[1].Rows[i]["QuickBloxId"].ToString();
                        objCareGiver.IsActive = Convert.ToString(ds.Tables[1].Rows[i]["IsActive"].ToString());

                        objCareGiver.IsAllowOneToOneChat = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsAllowOneToOneChat"]);
                        objCareGiver.IsAllowPatientChatRoom = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsAllowPatientChatRoom"]);
                        objCareGiver.IsAllowGroupChat = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsAllowGroupChat"]);
                        objCareGiver.IsAllowToCreateGroupChat = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsAllowToCreateGroupChat"]);
                        CareGiverList.Add(objCareGiver);
                    }

                    ListCareGiver.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    ListCareGiver.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListCareGiver.CareGiverList = CareGiverList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllCareGivers";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListCareGiver;
        }

        public CareGiversList GetAllCareGiversList(string SchedulerUserId)
        {
            CareGiversList ListCareGiver = new CareGiversList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllCareGiversList", Guid.Parse(SchedulerUserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<CareGivers> CareGiverList = new List<CareGivers>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGiver = new CareGivers();
                        objCareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objCareGiver.Name = ds.Tables[0].Rows[i]["Name"].ToString();

                        objCareGiver.FirstName= ds.Tables[0].Rows[i]["FirstName"].ToString();
                        objCareGiver.LastName= ds.Tables[0].Rows[i]["LastName"].ToString();
                        objCareGiver.DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                        objCareGiver.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        objCareGiver.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        CareGiverList.Add(objCareGiver);
                    }

                      ListCareGiver.CareGiverList = CareGiverList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllCareGiversList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListCareGiver;
        }

        public SchedulersList GetAllScheduler(string LoginUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string OfficeId, string OrganisationId,string IsActiveStatus)
        {
            SchedulersList ListScheduler = new SchedulersList();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllSchedulers",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllSchedulers_Testing",
                                                       LoginUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        Convert.ToInt32(OfficeId),                                                      
                                                        Convert.ToInt32(OrganisationId),
                                                        IsActiveStatus);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Scheduler> SchedulerList = new List<Scheduler>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Scheduler objScheduler = new Scheduler();
                        objScheduler.SchedulerId = Convert.ToInt32(ds.Tables[0].Rows[i]["SchedulerId"]);
                        objScheduler.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objScheduler.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        objScheduler.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        objScheduler.IsActive= Convert.ToString(ds.Tables[0].Rows[i]["IsActive"].ToString());
                        objScheduler.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objScheduler.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objScheduler.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        objScheduler.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        SchedulerList.Add(objScheduler);
                    }

                    ListScheduler.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListScheduler.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListScheduler.SchedulerList = SchedulerList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllScheduler";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListScheduler;
        }


        public SchedulersList GetAllSchedulerList()
        {
            SchedulersList ListScheduler = new SchedulersList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllSchedulersList");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Scheduler> SchedulerList = new List<Scheduler>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Scheduler objScheduler = new Scheduler();
                        objScheduler.SchedulerId = Convert.ToInt32(ds.Tables[0].Rows[i]["SchedulerId"]);
                        objScheduler.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objScheduler.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        objScheduler.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        objScheduler.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objScheduler.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();

                        SchedulerList.Add(objScheduler);
                    }

                    ListScheduler.SchedulerList = SchedulerList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllSchedulerList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListScheduler;
        }

        public Scheduler GetSchedulerDetailById(string SchedulerId)
        {
            Scheduler Scheduler = new Scheduler();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetSchedulerDetailById", SchedulerId, new Guid());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Scheduler.SchedulerId = Convert.ToInt32(ds.Tables[0].Rows[0]["SchedulerId"]);
                    Scheduler.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    Scheduler.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    Scheduler.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    Scheduler.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    Scheduler.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    Scheduler.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    Scheduler.IsActive = Convert.ToString(ds.Tables[0].Rows[0]["IsActive"].ToString());
                    Scheduler.QuickBloxId = Convert.ToString(ds.Tables[0].Rows[0]["QuickBloxId"].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetSchedulerDetailById";
                string result = InsertErrorLog(objErrorlog);
            }
            return Scheduler;
        }


        public List<GetDatesList> GetFilterDates(SchedulePatientRequest SchedulePatientRequest)
        {
            List<GetDatesList> GetDatesList = new List<GetDatesList>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetFilterDates",
                                                       SchedulePatientRequest.Date,       // + " 00:00:00").ToString().Split(' ')[0],
                                                       SchedulePatientRequest.IsRepeat,
                                                       SchedulePatientRequest.RepeatEvery,
                                                       SchedulePatientRequest.RepeatTypeID,
                                                       SchedulePatientRequest.RepeatDate,
                                                       SchedulePatientRequest.DayOfWeek,
                                                       SchedulePatientRequest.DaysOfMonth
                                                       );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GetDatesList objGetDatesList = new GetDatesList();
                        objGetDatesList.ListDate = Convert.ToString(ds.Tables[0].Rows[i]["dates"]);

                        GetDatesList.Add(objGetDatesList);
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllDate";
                string result = InsertErrorLog(objErrorlog);
            }

            return GetDatesList;
        }

        // Care Giver
        public List<CareGivers> GetAllNotifiedCareGiversByRequestId(string PatientRequestId)
        {
            List<CareGivers> CareGiverList = new List<CareGivers>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllNotifiedCareGiversByRequestId",
                                                       PatientRequestId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGiver = new CareGivers();
                        objCareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objCareGiver.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objCareGiver.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objCareGiver.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objCareGiver.ServiceId = ds.Tables[0].Rows[i]["Service"].ToString();
                        objCareGiver.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objCareGiver.HourlyRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["HourlyRate"].ToString());
                        objCareGiver.DistanceFromLocation = Convert.ToDecimal(ds.Tables[0].Rows[i]["DistanceFromLocation"].ToString());
                        objCareGiver.Phone = ds.Tables[0].Rows[i]["Phone"].ToString();
                        objCareGiver.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        objCareGiver.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objCareGiver.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objCareGiver.State = ds.Tables[0].Rows[i]["State"].ToString();
                        objCareGiver.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objCareGiver.ProfileImage = CareGiverProfileImagesPath + (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ProfileImage"].ToString()) ? "default.png" : ds.Tables[0].Rows[i]["ProfileImage"].ToString());
                        //if (!File.Exists(objCareGiver.ProfileImage))
                        //    objCareGiver.ProfileImage = CareGiverProfileImagesPath + "default.png";
                        objCareGiver.Certificate = ds.Tables[0].Rows[i]["Certificate"].ToString();
                        objCareGiver.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAvailable"].ToString());
                        objCareGiver.IsBusy = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsBusy"].ToString());
                        objCareGiver.IsApprove = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsApprove"].ToString());
                        objCareGiver.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objCareGiver.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();
                        objCareGiver.DistanceUnit = ds.Tables[0].Rows[i]["DistanceUnit"].ToString();
                        objCareGiver.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objCareGiver.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                        objCareGiver.TimezoneId = ds.Tables[0].Rows[i]["TimezoneId"].ToString();
                        objCareGiver.CurrentLatitude = ds.Tables[0].Rows[i]["CurrentLatitude"].ToString();
                        objCareGiver.CurrentLongitude = ds.Tables[0].Rows[i]["CurrentLongitude"].ToString();
                        objCareGiver.Education = ds.Tables[0].Rows[i]["Education"].ToString();

                        CareGiverList.Add(objCareGiver);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllNotifiedCareGiversByRequestId";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiverList;
        }


        // Get Track Location of Care Giver

        public List<CareGiverTrackLocation> GetTrackLocationByPatientRequestId(string PatientRequestId)
        {
            List<CareGiverTrackLocation> CareGiverTrackLocationList = new List<CareGiverTrackLocation>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTrackLocationByPatientRequestId",
                                                       PatientRequestId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGiverTrackLocation objCareGiverTrackLocation = new CareGiverTrackLocation();
                        objCareGiverTrackLocation.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]);
                        objCareGiverTrackLocation.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        //  DateTime.Parse(Convert.ToString(item["Date"])).ToString("MM/dd/yyyy");
                        objCareGiverTrackLocation.LocationDateTime = DateTime.Parse(ds.Tables[0].Rows[i]["LocationDateTime"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
                        objCareGiverTrackLocation.LocationLatitude = ds.Tables[0].Rows[i]["LocationLatitude"].ToString();
                        objCareGiverTrackLocation.LocationLongitude = ds.Tables[0].Rows[i]["LocationLongitude"].ToString();
                        objCareGiverTrackLocation.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        objCareGiverTrackLocation.TotalMiles = ds.Tables[0].Rows[i]["TotalMiles"].ToString();
                        CareGiverTrackLocationList.Add(objCareGiverTrackLocation);
                    }
                }
                //else if (ds != null && ds.Tables[1].Rows.Count > 0)
                //{
                //    CareGiverTrackLocation objCareGiverTrackLocation = new CareGiverTrackLocation();
                //    objCareGiverTrackLocation.CheckInLatLong = ds.Tables[1].Rows[0]["CheckInLatitude"].ToString()+","+ ds.Tables[1].Rows[0]["CheckinLongitude"].ToString();                    
                //    objCareGiverTrackLocation.CheckoutLatLong = ds.Tables[1].Rows[0]["checkoutLatitude"].ToString() + "," + ds.Tables[1].Rows[0]["CheckoutLongitude"].ToString();
                //    CareGiverTrackLocationList.Add(objCareGiverTrackLocation);
                //}
                }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetTrackLocationByPatientRequestId";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiverTrackLocationList;
        }



        //public List<CareGiverTrackLocation> GetTrackLocationByPatientRequestId(string PatientRequestId)
        //{

        //    var origin = "";
        //    var destination = "";



        //    List<CareGiverTrackLocation> CareGiverTrackLocationList = new List<CareGiverTrackLocation>();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTrackLocationByPatientRequestId",
        //                                               PatientRequestId);

        //        int lastindex = ds.Tables[0].Rows.Count;

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {

        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                CareGiverTrackLocation objCareGiverTrackLocation = new CareGiverTrackLocation();
        //                objCareGiverTrackLocation.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]);
        //                objCareGiverTrackLocation.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
        //                //DateTime.Parse(Convert.ToString(item["Date"])).ToString("MM/dd/yyyy");
        //                objCareGiverTrackLocation.LocationDateTime = DateTime.Parse(ds.Tables[0].Rows[i]["LocationDateTime"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
        //                objCareGiverTrackLocation.LocationLatitude = ds.Tables[0].Rows[i]["LocationLatitude"].ToString();
        //                objCareGiverTrackLocation.LocationLongitude = ds.Tables[0].Rows[i]["LocationLongitude"].ToString();
        //                objCareGiverTrackLocation.Status = ds.Tables[0].Rows[i]["Status"].ToString();


        //                origin = ds.Tables[0].Rows[0]["LocationLatitude"].ToString() + ',' + ds.Tables[0].Rows[0]["LocationLongitude"].ToString();

        //                destination = ds.Tables[0].Rows[lastindex - 1]["LocationLatitude"].ToString() + ',' + ds.Tables[0].Rows[lastindex - 1]["LocationLongitude"].ToString();
        //                objCareGiverTrackLocation.TotalMiles = GetDistanceBetweenTwoLatlong(origin, destination).Result;


        //                // objCareGiverTrackLocation.TotalMiles = ds.Tables[0].Rows[i]["TotalMiles"].ToString();

        //                CareGiverTrackLocationList.Add(objCareGiverTrackLocation);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetTrackLocationByPatientRequestId";
        //        string result = InsertErrorLog(objErrorlog);
        //    }
        //    return CareGiverTrackLocationList;
        //}




        //private async Task<string> GetDistanceBetweenTwoLatlong(string origin, string destination)
        //{

        //    double xyz = 0;
        //    string distance = "";
        //    //double jkf =Convert.ToDouble(origin);

        //    //double abc = Convert.ToDouble(destination);

        //    var clientGetDialogId = new System.Net.Http.HttpClient();

        //    var requestUrl = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&key={2}&origins={0}&destinations={1}", origin, destination, "AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");

        //    // clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=41.6880133,-71.1579393&destinations=+&key=AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
        //    // clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&key={2}&origins={0}&destinations={origin}", origin,destination&key=+'AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc');

        //    clientGetDialogId.BaseAddress = new Uri(requestUrl);
        //    clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
        //    var response1 = await clientGetDialogId.GetAsync("");
        //    var result1 = response1.Content.ReadAsStringAsync().Result;
        //    var data = (JObject)JsonConvert.DeserializeObject(result1);

        //    foreach (var row in data["rows"])
        //    {
        //        foreach (var elements in row["elements"])
        //        {
        //            foreach (var dist in elements["distance"])
        //            {

        //                distance = (string)dist;
        //                break;
        //            }

        //        }
        //    }


        //    return distance;



        //}





        public List<CareGiverTrackLocation> GetTrackLocationByNurseId(string NurseId, string date)
        {
            //  var ConvertDate = Convert.ToDateTime(date).ToString("MM-DD-YYYY");

            // string fdate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            List<CareGiverTrackLocation> CareGiverTrackLocationList = new List<CareGiverTrackLocation>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTrackLocationByNurseId",
                                                       NurseId,
                                                       date
                                                      );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGiverTrackLocation objCareGiverTrackLocation = new CareGiverTrackLocation();
                        objCareGiverTrackLocation.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]);
                        objCareGiverTrackLocation.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        //  DateTime.Parse(Convert.ToString(item["Date"])).ToString("MM/dd/yyyy");
                        objCareGiverTrackLocation.LocationDateTime = DateTime.Parse(ds.Tables[0].Rows[i]["LocationDateTime"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
                        objCareGiverTrackLocation.LocationLatitude = ds.Tables[0].Rows[i]["LocationLatitude"].ToString();
                        objCareGiverTrackLocation.LocationLongitude = ds.Tables[0].Rows[i]["LocationLongitude"].ToString();
                        //  objCareGiverTrackLocation.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                        CareGiverTrackLocationList.Add(objCareGiverTrackLocation);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetTrackLocationByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiverTrackLocationList;
        }


        //public string InsertUpdateCareGiverByAdmin(CareGivers CareGiver)
        //{
        //    string result = "";
        //    try
        //    {
        //        int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateCareGiver_v2",
        //                                            CareGiver.NurseId,
        //                                            new Guid(CareGiver.UserId),
        //                                            CareGiver.Name,
        //                                            CareGiver.Email,
        //                                            CareGiver.ServiceId,
        //                                            CareGiver.HourlyRate,
        //                                            CareGiver.DistanceFromLocation,
        //                                            CareGiver.Phone,
        //                                            CareGiver.Address,
        //                                            CareGiver.ZipCode,
        //                                            CareGiver.ProfileImage,
        //                                            CareGiver.ProfileVideo,
        //                                            CareGiver.ProfileVideoThumbnil,
        //                                            CareGiver.Latitude,
        //                                            CareGiver.Longitude,
        //                                            CareGiver.DistanceUnit,
        //                                            new Guid(CareGiver.InsertUserId),
        //                                            CareGiver.UserName,
        //                                            CareGiver.Password,
        //                                            CareGiver.Education,
        //                                            0,
        //                                            0,
        //                                            CareGiver.CanAdminEdit
        //                                            );

        //        if (i > 0)
        //        {
        //            result = "Success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "InsertUpdateCareGiverByAdmin";
        //        objErrorlog.UserID = CareGiver.InsertUserId;
        //        result = InsertErrorLog(objErrorlog);
        //    }
        //    return result;
        //}

        public string InsertUpdateCareGiverByAdmin(CareGivers CareGiver)
        {
            string result = "";
            try
            {
                // int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateCareGiver_web",
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateCareGiver_Web",        
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_InsertUpdateCareGiver_Web",


                string query = "EXEC ORGNew_InsertUpdateCareGiver_Web " +
    $"{CareGiver.NurseId}, '{CareGiver.UserId}', '{CareGiver.Name}', '{CareGiver.FirstName}', " +
    $"'{CareGiver.LastName}', '{CareGiver.Email}', {CareGiver.ServiceId}, {CareGiver.HourlyRate}, " +
    $"{CareGiver.DistanceFromLocation}, '{CareGiver.Phone}', '{CareGiver.Address}', " +
    $"'{CareGiver.Street}', '{CareGiver.City}', '{CareGiver.State}', '{CareGiver.ZipCode}', " +
    $"'{CareGiver.ProfileImage}', '{CareGiver.ProfileVideo}', '{CareGiver.ProfileVideoThumbnil}', " +
    $"{CareGiver.Latitude}, {CareGiver.Longitude}, '{CareGiver.DistanceUnit}', " +
    $"'{CareGiver.InsertUserId}', '{CareGiver.UserName}', '{CareGiver.Password}', " +
    $"'{CareGiver.Education}', 0, 0, " +
    $"{CareGiver.CanAdminEdit}, '{CareGiver.TimezoneId}', {CareGiver.TimezoneOffset}, " +
    $"'{CareGiver.TimezonePostfix}', {CareGiver.OfficeId}, {CareGiver.PayrollId}, " +
    $"{CareGiver.IsAllowOneToOneChat}, {CareGiver.IsAllowPatientChatRoom}, {CareGiver.IsAllowGroupChat}, " +
    $"{CareGiver.IsAllowToCreateGroupChat}, {CareGiver.OrganisationId};";

                Console.WriteLine(query); // Print to console for debugging
          //      System.IO.File.AppendAllText("log.txt", query + Environment.NewLine); // Save to a log file




                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_InsertUpdateCareGiver_Web",
                                                  CareGiver.NurseId,
                                                    new Guid(CareGiver.UserId),
                                                    CareGiver.Name,
                                                    CareGiver.FirstName,
                                                    CareGiver.LastName,
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
                                                    CareGiver.ProfileVideo,
                                                    CareGiver.ProfileVideoThumbnil,
                                                    CareGiver.Latitude,
                                                    CareGiver.Longitude,
                                                    CareGiver.DistanceUnit,
                                                    new Guid(CareGiver.InsertUserId),
                                                    CareGiver.UserName,
                                                    CareGiver.Password,
                                                    CareGiver.Education,
                                                    0,
                                                    0,
                                                    CareGiver.CanAdminEdit,
                                                    CareGiver.TimezoneId,
                                                    CareGiver.TimezoneOffset,
                                                    CareGiver.TimezonePostfix,
                                                    CareGiver.OfficeId,
                                                    CareGiver.PayrollId,
                                                    CareGiver.IsAllowOneToOneChat,
                                                    CareGiver.IsAllowPatientChatRoom,
                                                    CareGiver.IsAllowGroupChat,
                                                    CareGiver.IsAllowToCreateGroupChat,
                                                    CareGiver.OrganisationId
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
                objErrorlog.Methodname = "InsertUpdateCareGiverByAdmin";
                objErrorlog.UserID = CareGiver.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public string AddScheduler(Scheduler Scheduler)
        {
            string result = "";
            try
            {
                // int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AddScheduler",
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_AddScheduler",
                                                     Scheduler.SchedulerId,
                                                    new Guid(Scheduler.UserId),
                                                    Scheduler.FirstName,
                                                    Scheduler.LastName,
                                                    Scheduler.UserName,
                                                    Scheduler.Email,
                                                    Scheduler.Password,
                                                    new Guid(Scheduler.InsertUserId),    
                                                    Scheduler.OrganisationId
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
                objErrorlog.Methodname = "AddScheduler";
                objErrorlog.UserID = Scheduler.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string EditScheduler(Scheduler Scheduler)
        {
            string result = "";
            try
            {
              
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "EditScheduler",
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_EditScheduler",
                                                    Scheduler.SchedulerId,
                                                    // new Guid(Scheduler.UserId),
                                                    Scheduler.FirstName,
                                                    Scheduler.LastName,
                                                   // Scheduler.UserName,
                                                   Scheduler.Email,
                                                    //Scheduler.Password,
                                                    new Guid(Scheduler.InsertUserId)
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
                objErrorlog.Methodname = "EditScheduler";
                objErrorlog.UserID = Scheduler.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public string InsertNurseProfile(CareGivers CareGiver)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertNurseProfile",
                                                   CareGiver.NurseId,
                                                   new Guid(CareGiver.UserId),
                                                   CareGiver.Name,
                                                   CareGiver.Email,
                                                   CareGiver.ServiceId,
                                                   CareGiver.HourlyRate,
                                                   CareGiver.DistanceFromLocation,
                                                   CareGiver.Phone,
                                                   CareGiver.Address,
                                                   CareGiver.ZipCode,
                                                   CareGiver.ProfileImage,
                                                   CareGiver.ProfileVideo,
                                                   CareGiver.ProfileVideoThumbnil,
                                                   CareGiver.Latitude,
                                                   CareGiver.Longitude,
                                                   CareGiver.DistanceUnit,
                                                   new Guid(CareGiver.InsertUserId),
                                                   CareGiver.UserName,
                                                   CareGiver.Password,
                                                   CareGiver.Education,
                                                   0,
                                                   0,
                                                   CareGiver.CanAdminEdit,
                                                   CareGiver.ChargeToPatient
                                                   );

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
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertNurseProfile";
                objErrorlog.UserID = CareGiver.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string UpdateCareGiver(CareGivers CareGiver)
        {
            string result = "";
            try
            {
                
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateCareGiver_Web",
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
                                                    CareGiver.PayrollId                     
                                                    );

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
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "UpdateCareGiver";
                objErrorlog.UserID = CareGiver.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public CareGivers GetAllCareGiverByNurseId(string NurseId)
        {
            CareGivers CareGiver = new CareGivers();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllCareGiverByNurseId", NurseId, new Guid());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseId"]);
                    CareGiver.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    CareGiver.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    CareGiver.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    CareGiver.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    CareGiver.IsActive = Convert.ToString(ds.Tables[0].Rows[0]["IsActive"].ToString());

                    CareGiver.NurseFullName= ds.Tables[0].Rows[0]["NurseFullName"].ToString();

                    CareGiver.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    CareGiver.ServiceId = ds.Tables[0].Rows[0]["Service"].ToString();
                    CareGiver.ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString();
                    CareGiver.HourlyRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["HourlyRate"].ToString());

                    HoursRate HoursRate = new HoursRate();
                    HoursRate.WeekDayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekDayHourlyRate"].ToString());
                    HoursRate.WeekNightRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekNightHourlyRate"].ToString());
                    HoursRate.WeekEndDayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekEndDayHourlyRate"].ToString());
                    HoursRate.WeekEndNightRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekEndNightHourlyRate"].ToString());
                    HoursRate.HolidayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["HolidayHourlyRate"].ToString());
                    CareGiver.HoursRate = HoursRate;
                    CareGiver.ChargeToPatient = Convert.ToDecimal(ds.Tables[0].Rows[0]["ChargeToPatient"].ToString());
                    CareGiver.DistanceFromLocation = Convert.ToDecimal(ds.Tables[0].Rows[0]["DistanceFromLocation"].ToString());
                    CareGiver.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                    CareGiver.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    CareGiver.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    CareGiver.City = ds.Tables[0].Rows[0]["City"].ToString();
                    CareGiver.State = ds.Tables[0].Rows[0]["State"].ToString();
                    CareGiver.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProfileImage"].ToString()))
                    {
                        CareGiver.ProfileImage = CareGiverProfileImagesPath + ds.Tables[0].Rows[0]["ProfileImage"].ToString();
                    }
                    else
                    {
                        CareGiver.ProfileImage = "";

                    }
                    CareGiver.ProfileVideo = (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProfileVideo"].ToString())) ? CareGiverProfileVideoURL + ds.Tables[0].Rows[0]["ProfileVideo"].ToString() : "";
                    CareGiver.ProfileVideoThumbnil = (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProfileVideoThumbnil"].ToString())) ? CareGiverProfileImagesPath + ds.Tables[0].Rows[0]["ProfileVideoThumbnil"].ToString() : "";
                    CareGiver.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAvailable"].ToString());
                    CareGiver.IsBusy = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBusy"].ToString());
                    CareGiver.IsApprove = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsApprove"].ToString());
                    CareGiver.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    CareGiver.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    CareGiver.DistanceUnit = ds.Tables[0].Rows[0]["DistanceUnit"].ToString();
                    CareGiver.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    CareGiver.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    CareGiver.Education = ds.Tables[0].Rows[0]["Education"].ToString();
                    CareGiver.CanAdminEdit = Convert.ToBoolean(ds.Tables[0].Rows[0]["CanAdminEdit"]);
                    CareGiver.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"]);
                    CareGiver.Office = ds.Tables[0].Rows[0]["Office"].ToString();
                    //CareGiver.IsActive =Convert.ToBoolean( ds.Tables[0].Rows[0]["IsActive"].ToString());
                    CareGiver.QuickBloxId = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();

                    CareGiver.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"]);

                    

                    if (ds.Tables[0].Rows[0]["EEID"].ToString() == null || ds.Tables[0].Rows[0]["EEID"].ToString() == "")
                    {
                        CareGiver.PayrollId = "";
                    }
                    else
                    {
                        CareGiver.PayrollId = ds.Tables[0].Rows[0]["EEID"].ToString();
                    }
 
                    CareGiver.IsAllowOneToOneChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowOneToOneChat"]);
                    CareGiver.IsAllowPatientChatRoom = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowPatientChatRoom"]);
                    CareGiver.IsAllowGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowGroupChat"]);
                    CareGiver.IsAllowToCreateGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowToCreateGroupChat"]);

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneId"].ToString()))
                    {
                        CareGiver.TimezoneId = Convert.ToString(ds.Tables[0].Rows[0]["TimezoneId"]);
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString()))
                    {
                        CareGiver.TimezoneOffset = Convert.ToInt32(ds.Tables[0].Rows[0]["TimezoneOffset"]);
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimezonePostfix"].ToString()))
                    {
                        CareGiver.TimezonePostfix = Convert.ToString(ds.Tables[0].Rows[0]["TimezonePostfix"]);
                    }

                    //HoursRate = new HoursRate();
                    //HoursRate.WeekDayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekDayRate"]);
                    //HoursRate.WeekNightRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekNightRate"]);
                    //HoursRate.WeekEndDayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekEndDayRate"]);
                    //HoursRate.WeekEndNightRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekEndNightRate"]);
                    //HoursRate.HolidayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["HolidayRate"]);
                    //CareGiver.ChargeToPatientHoursRate = HoursRate;
                    //HoursRate = new HoursRate();
                    //HoursRate.WeekDayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekDayRate"]);
                    //HoursRate.WeekNightRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekNightRate"]);
                    //HoursRate.WeekEndDayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekEndDayRate"]);
                    //HoursRate.WeekEndNightRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["WeekEndNightRate"]);
                    //HoursRate.HolidayRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["HolidayRate"]);
                    //CareGiver.ChargeToPatientHoursRateWithoutStripe = HoursRate;

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllCareGiverByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiver;
        }

        public string ApproveRejectNurse(CareGivers CareGiver)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ApproveRejectNurse_v1",
                                                    CareGiver.NurseId,
                                                    CareGiver.IsApprove,
                                                    new Guid(CareGiver.InsertUserId),
                                                    new Guid(CareGiver.UserId),
                                                    CareGiver.UserName,
                                                    CareGiver.Password,
                                                    CareGiver.ChargeToPatient,
                                                    CareGiver.IsAllowOneToOneChat,
                                                    CareGiver.IsAllowPatientChatRoom,
                                                    CareGiver.IsAllowGroupChat,
                                                    CareGiver.IsAllowToCreateGroupChat
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
                objErrorlog.Methodname = "ApproveRejectNurse";
                objErrorlog.UserID = CareGiver.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public List<CareGivers> GetAllNursesByDateFilter(string FromDate, string ToDate)
        {
            List<CareGivers> CareGiverList = new List<CareGivers>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllNursesByDateFilter",
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGiver = new CareGivers();
                        objCareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objCareGiver.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objCareGiver.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objCareGiver.HourlyRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["HourlyRate"].ToString());
                        objCareGiver.ProfileImage = CareGiverProfileImagesPath + ds.Tables[0].Rows[i]["ProfileImage"].ToString();
                        objCareGiver.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAvailable"].ToString());
                        objCareGiver.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objCareGiver.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

                        CareGiverList.Add(objCareGiver);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllNursesByDateFilter";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiverList;
        }


        public string DeleteNurse(string NurseId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteNurseForScheduler",
                                                    NurseId,
                                                    new Guid(UserId));

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
                objErrorlog.Methodname = "DeleteNurse";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public string DeleteScheduler(string SchedulerId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteScheduler",
                                                    SchedulerId,
                                                    new Guid(UserId));

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
                objErrorlog.Methodname = "DeleteScheduler";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public List<CareGivers> GetCareGiverByServiceId(string ServiceId)
        {
            List<CareGivers> CareGiverList = new List<CareGivers>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCareGiverByServiceId", ServiceId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGiver = new CareGivers();
                        objCareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objCareGiver.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objCareGiver.HourlyRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["HourlyRate"].ToString());

                        CareGiverList.Add(objCareGiver);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiverByServiceId";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiverList;
        }

        public List<CareGivers> GetCareGiverForPatiantRequest(PatientRequests PatientRequest)
        {
            List<CareGivers> CareGiverList = new List<CareGivers>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCareGiverForPatiantRequest",
                                                        PatientRequest.FromTime,
                                                        PatientRequest.ToTime,
                                                        PatientRequest.Date,
                                                        PatientRequest.ServiceRadius,
                                                        PatientRequest.ServiceId,
                                                        PatientRequest.FromHourRate,
                                                        PatientRequest.ToHourRate);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGiver = new CareGivers();
                        objCareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objCareGiver.Name = ds.Tables[0].Rows[i]["Name"].ToString();

                        CareGiverList.Add(objCareGiver);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiverForPatiantRequest";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiverList;
        }

        // PATIENTS
        public PatientsList GetAllUsersForAdmin(string pageNo, string recordPerPage, string sortfield, string sortorder, string search)
        {
            PatientsList PatientList = new PatientsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllUsersForAdmin",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
                {
                    List<Patients> ListPatients = new List<Patients>();
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        Patients objPatient = new Patients();
                        objPatient.PatientId = Convert.ToInt32(ds.Tables[2].Rows[i]["PatientId"]);
                        objPatient.UserId = ds.Tables[2].Rows[i]["UserId"].ToString();
                        objPatient.Name = ds.Tables[2].Rows[i]["Name"].ToString();
                        objPatient.Email = ds.Tables[2].Rows[i]["Email"].ToString();
                        objPatient.Phone = ds.Tables[2].Rows[i]["Phone"].ToString();
                        objPatient.Address = ds.Tables[2].Rows[i]["Address"].ToString();
                        objPatient.ZipCode = ds.Tables[2].Rows[i]["ZipCode"].ToString();

                        ListPatients.Add(objPatient);
                    }

                    PatientList.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    PatientList.FilteredRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    PatientList.PatientList = ListPatients;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "ApproveRejectNurse";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientList;
        }

        public List<Patients> GetAllPatientsByDateFilter(string FromDate, string ToDate)
        {
            List<Patients> PatientList = new List<Patients>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPatientsByDateFilter",
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Patients objPatient = new Patients();
                        objPatient.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatient.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objPatient.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objPatient.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objPatient.Phone = ds.Tables[0].Rows[i]["Phone"].ToString();
                        objPatient.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objPatient.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

                        PatientList.Add(objPatient);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientsByDateFilter";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientList;
        }

        public List<Patients> GetPatientInfoForDashboard(string ZipCode, string FromDate, string ToDate)
        {
            List<Patients> PatientList = new List<Patients>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientInfoForDashboard",
                                                        ZipCode,
                                                        Convert.ToDateTime(FromDate).ToString("yyyy-M-dd"),
                                                        Convert.ToDateTime(ToDate).ToString("yyyy-M-dd"));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Patients objPatient = new Patients();
                        objPatient.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatient.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objPatient.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objPatient.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objPatient.InsertDateTime = ds.Tables[0].Rows[i]["InsertDateTime"].ToString();

                        PatientList.Add(objPatient);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientInfoForDashboard";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientList;
        }

        // CAREGIVER TIME SLOTS
        public List<CareGiverTimeSlots> GetTimeSlotByNurseId(string NurseId, string Week, string Year)
        {
            List<CareGiverTimeSlots> TimeSlotList = new List<CareGiverTimeSlots>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTimeSlotByNurseId",
                                                        NurseId,
                                                        Week,
                                                        Year);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGiverTimeSlots objTimeSlot = new CareGiverTimeSlots();
                        objTimeSlot.TimeSlotId = Convert.ToInt32(ds.Tables[0].Rows[i]["TimeSlotId"]);
                        objTimeSlot.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objTimeSlot.Week = Convert.ToInt32(ds.Tables[0].Rows[i]["Week"]);
                        objTimeSlot.Year = Convert.ToInt32(ds.Tables[0].Rows[i]["Year"]);
                        objTimeSlot.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objTimeSlot.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();

                        TimeSlotList.Add(objTimeSlot);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetTimeSlotByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return TimeSlotList;
        }

        public string InsertTimeSlotForNurse(CareGiverTimeSlots CareGiverTimeSlot)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertTimeSlotForNurse",
                                                    CareGiverTimeSlot.TimeSlotId,
                                                    CareGiverTimeSlot.NurseId,
                                                    CareGiverTimeSlot.Week,
                                                    CareGiverTimeSlot.Year,
                                                    CareGiverTimeSlot.FromTime,
                                                    CareGiverTimeSlot.ToTime,
                                                    new Guid(CareGiverTimeSlot.UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    result = "-1";
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertTimeSlotForNurse";
                objErrorlog.UserID = CareGiverTimeSlot.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        // CAREGIVER SCHEDULE
        public List<CareGiverSchedule> GetScheduleByTimeSlotId(string TimeSlotId)
        {
            List<CareGiverSchedule> ScheduleList = new List<CareGiverSchedule>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetScheduleByTimeSlotId",
                                                        TimeSlotId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGiverSchedule objSchedule = new CareGiverSchedule();
                        objSchedule.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"]);
                        objSchedule.TimeSlotId = Convert.ToInt32(ds.Tables[0].Rows[i]["TimeSlotId"]);
                        objSchedule.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objSchedule.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("yyyy-MM-dd");
                        objSchedule.IsAppointed = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAppointed"]);
                        objSchedule.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAvailable"]);
                        objSchedule.IsPartial = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPartial"]);
                        objSchedule.PartialFromTime = ds.Tables[0].Rows[i]["PartialFromTime"].ToString();
                        objSchedule.PartialToTime = ds.Tables[0].Rows[i]["PartialToTime"].ToString();
                        ScheduleList.Add(objSchedule);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetScheduleByTimeSlotId";
                string result = InsertErrorLog(objErrorlog);
            }
            return ScheduleList;
        }

        public string InsertScheduleForNurse(CareGiverSchedule CareGiverSchedule)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertScheduleForNurse",
                                                    CareGiverSchedule.NurseId,
                                                    CareGiverSchedule.TimeSlotId,
                                                    CareGiverSchedule.Date,
                                                    new Guid(CareGiverSchedule.UserId));

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
                objErrorlog.Methodname = "InsertScheduleForNurse";
                objErrorlog.UserID = CareGiverSchedule.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string DeleteScheduleByTimeSlotId(CareGiverSchedule CareGiverSchedule)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteScheduleByTimeSlotId",
                                                    CareGiverSchedule.NurseId,
                                                    CareGiverSchedule.TimeSlotId,
                                                    new Guid(CareGiverSchedule.UserId));

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
                objErrorlog.Methodname = "DeleteScheduleByTimeSlotId";
                objErrorlog.UserID = CareGiverSchedule.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        // PATIENTS REQUESTS
        public PatientRequestList GetAllPatientRequests(string pageNo, string recordPerPage, string sortfield, string sortorder, string search)
        {
            PatientRequestList ListPatientRequest = new PatientRequestList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPatientRequests",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<PatientRequests> PatientRequestList = new List<PatientRequests>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientEmail = ds.Tables[0].Rows[i]["PatientEmail"].ToString();
                        objPatientRequest.PatientPhone = ds.Tables[0].Rows[i]["PatientPhone"].ToString();
                        objPatientRequest.PatientAddress = ds.Tables[0].Rows[i]["PatientAddress"].ToString();
                        objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objPatientRequest.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objPatientRequest.HourRate = Convert.ToInt32(ds.Tables[0].Rows[i]["HourRate"]);
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalHours"]);
                        objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.NurseEmail = ds.Tables[0].Rows[i]["NurseEmail"].ToString();
                        objPatientRequest.NursePhone = ds.Tables[0].Rows[i]["NursePhone"].ToString();
                        objPatientRequest.RescheduleStatus = ds.Tables[0].Rows[i]["RescheduleStatus"].ToString();
                        objPatientRequest.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAvailable"]);
                        objPatientRequest.IsBusy = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsBusy"]);

                        PatientRequestList.Add(objPatientRequest);
                    }

                    ListPatientRequest.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListPatientRequest.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListPatientRequest.PatientRequestsList = PatientRequestList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientRequests";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListPatientRequest;
        }

        public PatientRequests GetAppointmentDetailByAppointmentId(string AppointmentId)
        {
            PatientRequests objPatientRequest = new PatientRequests();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAppointmentDetailByAppointmentId", Convert.ToInt32(AppointmentId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["AppointmentId"]);
                    objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientId"]);
                    objPatientRequest.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objPatientRequest.PatientEmail = ds.Tables[0].Rows[0]["PatientEmail"].ToString();
                    objPatientRequest.PatientPhone = ds.Tables[0].Rows[0]["PatientPhone"].ToString();
                    objPatientRequest.PatientAddress = ds.Tables[0].Rows[0]["PatientAddress"].ToString();
                    objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[0]["ServiceId"]);
                    objPatientRequest.ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString();
                    objPatientRequest.HourRate = Convert.ToInt32(ds.Tables[0].Rows[0]["HourRate"]);
                    objPatientRequest.Date = ds.Tables[0].Rows[0]["Date"].ToString();
                    objPatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    objPatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalHours"]);
                    objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalAmount"]);
                    objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseId"]);
                    objPatientRequest.NurseName = ds.Tables[0].Rows[0]["NurseName"].ToString();
                    objPatientRequest.NurseEmail = ds.Tables[0].Rows[0]["NurseEmail"].ToString();
                    objPatientRequest.NursePhone = ds.Tables[0].Rows[0]["NursePhone"].ToString();
                    objPatientRequest.RescheduleStatus = ds.Tables[0].Rows[0]["RescheduleStatus"].ToString();
                    objPatientRequest.NurseDeviceType = ds.Tables[0].Rows[0]["NurseDeviceType"].ToString();
                    objPatientRequest.NurseDeviceToken = ds.Tables[0].Rows[0]["NurseDeviceToken"].ToString();
                    objPatientRequest.PatientUserId = ds.Tables[0].Rows[0]["PatientUserId"].ToString();
                    objPatientRequest.PatientDeviceType = ds.Tables[0].Rows[0]["PatientDeviceType"].ToString();
                    objPatientRequest.PatientDeviceToken = ds.Tables[0].Rows[0]["PatientDeviceToken"].ToString();
                    objPatientRequest.NurseUserId = ds.Tables[0].Rows[0]["NurseUserId"].ToString();
                    objPatientRequest.ChargeToPatientRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["ChargeToPatientRate"]);
                    objPatientRequest.CareGiverCharge = Convert.ToDecimal(ds.Tables[0].Rows[0]["CareGiverCharge"]);
                    objPatientRequest.StripeFee = Convert.ToDecimal(ds.Tables[0].Rows[0]["StripeFee"]);
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientRequests";
                string result = InsertErrorLog(objErrorlog);
            }
            return objPatientRequest;
        }

        public string InsertPatientRequestForAdmin(PatientRequests PatientRequest)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertPatientRequestForAdmin",
                                                        PatientRequest.AppointmentId,
                                                        PatientRequest.PatientId,
                                                        PatientRequest.NurseId,
                                                        PatientRequest.Date,
                                                        PatientRequest.FromTime,
                                                        PatientRequest.ToTime,
                                                        PatientRequest.HourRate,
                                                        PatientRequest.TotalAmount,
                                                        PatientRequest.ServiceId,
                                                        new Guid(PatientRequest.InsertUserId));

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
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertPatientRequestForAdmin";
                objErrorlog.UserID = PatientRequest.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string CancelPatientAppointment(string AppointmentId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "CancelPatientAppointment",
                                                        AppointmentId,
                                                        new Guid(UserId));

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
                objErrorlog.Methodname = "CancelPatientAppointment";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string ChangeCareGiver(string AppointmentId, string NurseId, string UserId)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ChangeCareGiver",
                                                        AppointmentId,
                                                        NurseId,
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
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "ChangeCareGiver";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public PatientRequests GetRescheduleAppoinmentRequest(string AppointmentId)
        {
            PatientRequests objPatientRequest = new PatientRequests();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetRescheduleAppoinmentRequest", Convert.ToInt32(AppointmentId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["AppointmentId"]);
                    objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientId"]);
                    objPatientRequest.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objPatientRequest.PatientEmail = ds.Tables[0].Rows[0]["PatientEmail"].ToString();
                    objPatientRequest.PatientPhone = ds.Tables[0].Rows[0]["PatientPhone"].ToString();
                    objPatientRequest.PatientAddress = ds.Tables[0].Rows[0]["PatientAddress"].ToString();
                    objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[0]["ServiceId"]);
                    objPatientRequest.ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString();
                    objPatientRequest.HourRate = Convert.ToInt32(ds.Tables[0].Rows[0]["HourRate"]);
                    objPatientRequest.Date = ds.Tables[0].Rows[0]["Date"].ToString();
                    objPatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    objPatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalHours"]);
                    objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalAmount"]);
                    objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseId"]);
                    objPatientRequest.NurseName = ds.Tables[0].Rows[0]["NurseName"].ToString();
                    objPatientRequest.NurseEmail = ds.Tables[0].Rows[0]["NurseEmail"].ToString();
                    objPatientRequest.NursePhone = ds.Tables[0].Rows[0]["NursePhone"].ToString();
                    objPatientRequest.RescheduleStatus = ds.Tables[0].Rows[0]["RescheduleStatus"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientRequests";
                string result = InsertErrorLog(objErrorlog);
            }
            return objPatientRequest;
        }

        public string RescheduleAppoinment(PatientRequests PatientRequest)
        {
            string result = "";
            try
            {
                CareGivers CareGiver = GetAllCareGiverByNurseId(PatientRequest.NurseId.ToString());

                decimal MorningShiftHours = GetMorningShiftHours(PatientRequest.Date, PatientRequest.FromTime, PatientRequest.ToTime);
                decimal NightShiftHours = GetNightShiftHours(PatientRequest.Date, PatientRequest.FromTime, PatientRequest.ToTime);
                decimal MorningShiftChargeForHoursRate = GetMorningShiftCharge(PatientRequest.Date, CareGiver.HoursRate);
                decimal NightShiftChargeForHoursRate = GetNightShiftCharge(PatientRequest.Date, CareGiver.HoursRate);
                decimal MorningShiftCharge = GetMorningShiftCharge(PatientRequest.Date, CareGiver.ChargeToPatientHoursRate);
                decimal NightShiftCharge = GetNightShiftCharge(PatientRequest.Date, CareGiver.ChargeToPatientHoursRate);
                decimal CareGiverCharge = (MorningShiftChargeForHoursRate * MorningShiftHours) + (NightShiftChargeForHoursRate * NightShiftHours);

                decimal ChargeToPatient = CareGiver.ChargeToPatient;
                decimal PayableAmount = (MorningShiftCharge * MorningShiftHours) + (NightShiftCharge * NightShiftHours);
                decimal TotalAmount = (StripePayment.GetStripeTotalAmount(MorningShiftCharge, Convert.ToDecimal(ConfigurationManager.AppSettings["StripePercentage"].ToString())) * MorningShiftHours) +
                    (StripePayment.GetStripeTotalAmount(NightShiftCharge, Convert.ToDecimal(ConfigurationManager.AppSettings["StripePercentage"].ToString())) * NightShiftHours);
                decimal StripeFee = TotalAmount - PayableAmount;
                decimal PaidAmount = PatientRequest.PaidAmount;

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "RescheduleAppoinment",
                                                        PatientRequest.AppointmentId,
                                                        PatientRequest.Date,
                                                        PatientRequest.FromTime,
                                                        PatientRequest.ToTime,
                                                        PatientRequest.TravelBufferMinutes,
                                                        new Guid(PatientRequest.InsertUserId),
                                                        PatientRequest.NurseId,
                                                        PatientRequest.HourRate,
                                                        PatientRequest.ChargeToPatientRate,
                                                        CareGiverCharge,
                                                        TotalAmount,
                                                        StripeFee,
                                                        PayableAmount);

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
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "RescheduleAppoinment";
                objErrorlog.UserID = PatientRequest.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public List<PatientRequests> GetAppointmentsByDateFilter(string FromDate, string ToDate)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAppointmentsByDateFilter",
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objPatientRequest.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objPatientRequest.HourRate = Convert.ToInt32(ds.Tables[0].Rows[i]["HourRate"]);
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalHours"]);
                        objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.RescheduleStatus = ds.Tables[0].Rows[i]["RescheduleStatus"].ToString();

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientRequests";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public List<PatientRequests> GetPaymentReceived(string NurseId, string PatientId, string ZipCode, string FromDate, string ToDate)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                if (NurseId == "0")
                {
                    NurseId = "";
                }
                if (PatientId == "0")
                {
                    PatientId = "";
                }
                if (ZipCode == "0")
                {
                    ZipCode = "";
                }

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPaymentReceived",
                                                        PatientId,
                                                        NurseId,
                                                        ZipCode,
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPaymentReceived";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public List<PatientRequests> GetProjectedPayment(string NurseId, string PatientId, string ZipCode, string FromDate, string ToDate)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                if (NurseId == "0")
                {
                    NurseId = "";
                }
                if (PatientId == "0")
                {
                    PatientId = "";
                }
                if (ZipCode == "0")
                {
                    ZipCode = "";
                }
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetProjectedPayment",
                                                        PatientId,
                                                        NurseId,
                                                        ZipCode,
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.DuePayment = Convert.ToDecimal(ds.Tables[0].Rows[i]["DuePayment"].ToString());

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetProjectedPayment";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public List<PatientRequests> GetRescheduleApprovalAlerts()
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetRescheduleApprovalAlerts");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objPatientRequest.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.RescheduleStatus = ds.Tables[0].Rows[i]["RescheduleStatus"].ToString();

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetRescheduleApprovalAlerts";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public List<PatientRequests> GetRescheduleAlerts()
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetRescheduleAlerts");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.RescheduleType = ds.Tables[0].Rows[i]["RescheduleType"].ToString();

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetRescheduleAlerts";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        // UNAVAILABILITY REQUEST
        public UnavailabilityRequestsList GetAllUnavailabilityRequest(string pageNo, string recordPerPage, string sortfield, string sortorder, string search)
        {
            UnavailabilityRequestsList ListUnavailabilityRequest = new UnavailabilityRequestsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllUnavailabilityRequest",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<UnavailabilityRequests> UnavailabilityRequestList = new List<UnavailabilityRequests>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UnavailabilityRequests objUnavailabilityRequest = new UnavailabilityRequests();
                        objUnavailabilityRequest.UnavailabilityRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["UnavailabilityRequestId"]);
                        objUnavailabilityRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objUnavailabilityRequest.NurseName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objUnavailabilityRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objUnavailabilityRequest.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objUnavailabilityRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objUnavailabilityRequest.TimeSlot = ds.Tables[0].Rows[i]["TimeSlot"].ToString();
                        objUnavailabilityRequest.IsApproved = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsApproved"].ToString());
                        objUnavailabilityRequest.IsReject = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsReject"].ToString());
                        objUnavailabilityRequest.NoOfAppointment = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfAppointment"].ToString());

                        UnavailabilityRequestList.Add(objUnavailabilityRequest);
                    }

                    ListUnavailabilityRequest.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListUnavailabilityRequest.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListUnavailabilityRequest.UnavailabilityRequestList = UnavailabilityRequestList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllUnavailabilityRequest";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListUnavailabilityRequest;
        }

        public string ApproveRejectUnavailabilityRequest(UnavailabilityRequests UnavailabilityRequest)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ApproveRejectUnavailabilityRequest",
                                                        UnavailabilityRequest.UnavailabilityRequestId,
                                                        Convert.ToBoolean(UnavailabilityRequest.IsApproved),
                                                        Convert.ToBoolean(UnavailabilityRequest.IsReject),
                                                        UnavailabilityRequest.RejectComment,
                                                        new Guid(UnavailabilityRequest.UserId));

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
                objErrorlog.Methodname = "ApproveRejectUnavailabilityRequest";
                objErrorlog.UserID = UnavailabilityRequest.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public List<PatientRequests> GetAppointmentsByUnavailabilityRequestId(string UnavailabilityRequestId)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAppointmentsByUnavailabilityRequestId", UnavailabilityRequestId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientEmail = ds.Tables[0].Rows[i]["PatientEmail"].ToString();
                        objPatientRequest.PatientPhone = ds.Tables[0].Rows[i]["PatientPhone"].ToString();
                        objPatientRequest.PatientAddress = ds.Tables[0].Rows[i]["PatientAddress"].ToString();
                        objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objPatientRequest.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objPatientRequest.HourRate = Convert.ToInt32(ds.Tables[0].Rows[i]["HourRate"]);
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalHours"]);
                        objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAppointmentsByUnavailabilityRequestId";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        // DISPUTE
        public List<Disputes> GetAllDisputes()
        {
            List<Disputes> DisputesList = new List<Disputes>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllDisputes");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Disputes objDispute = new Disputes();
                        objDispute.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objDispute.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objDispute.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objDispute.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objDispute.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objDispute.DisputeId = Convert.ToInt32(ds.Tables[0].Rows[i]["DisputeId"]);
                        objDispute.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objDispute.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objDispute.DisputeReason = ds.Tables[0].Rows[i]["DisputeReason"].ToString();
                        objDispute.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                        DisputesList.Add(objDispute);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllDisputes";
                string result = InsertErrorLog(objErrorlog);
            }
            return DisputesList;
        }

        public string InserDisputeComment(Disputes Dispute)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InserDisputeComment",
                                                        Dispute.DisputeId,
                                                        Dispute.DisputeReason,
                                                        Dispute.Status,
                                                        new Guid(Dispute.InsertUserId));

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
                objErrorlog.Methodname = "InserDisputeComment";
                objErrorlog.UserID = Dispute.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public Disputes GetDisputesDetailByDisputeId(string DisputeId)
        {
            Disputes objDispute = new Disputes();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetDisputesDetailByDisputeId", DisputeId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objDispute.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["AppointmentId"]);
                    objDispute.PatientId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientId"]);
                    objDispute.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objDispute.Date = ds.Tables[0].Rows[0]["Date"].ToString();
                    objDispute.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    objDispute.DisputeId = Convert.ToInt32(ds.Tables[0].Rows[0]["DisputeId"]);
                    objDispute.NurseId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseId"]);
                    objDispute.NurseName = ds.Tables[0].Rows[0]["NurseName"].ToString();
                    objDispute.DisputeReason = ds.Tables[0].Rows[0]["DisputeReason"].ToString();
                    objDispute.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    objDispute.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    objDispute.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalHours"]);
                    objDispute.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalAmount"]);
                    objDispute.PaidAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["PaidAmount"]);
                    objDispute.HourRate = Convert.ToInt32(ds.Tables[0].Rows[0]["HourRate"]);
                    objDispute.NurseSecretKey = ds.Tables[0].Rows[0]["NurseSecretKey"].ToString();
                    objDispute.NursePublishableKey = ds.Tables[0].Rows[0]["NursePublishableKey"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetDisputesDetailByDisputeId";
                string result = InsertErrorLog(objErrorlog);
            }
            return objDispute;
        }

        public string InsertPaymentTransaction(PaymentTransaction Transaction)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertPaymentTransaction",
                                                        Transaction.DisputeId,
                                                        Transaction.AppointmentId,
                                                        Transaction.NurseId,
                                                        Transaction.PatientId,
                                                        Transaction.Currency,
                                                        Transaction.Amount,
                                                        Transaction.ChargeId,
                                                        Transaction.SecretKey,
                                                        Transaction.Description,
                                                        new Guid(Transaction.InsertUserId));

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
                objErrorlog.Methodname = "InserDisputeComment";
                objErrorlog.UserID = Transaction.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        // Message Queue
        public string InsertMessageQueue(MessageQueue MessageQueue)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertMessageQueue",
                    MessageQueue.Subject,
                    MessageQueue.Message,
                    MessageQueue.UserID == "" ? "" : MessageQueue.UserID,
                    MessageQueue.EmailID,
                    MessageQueue.MobileNumber,
                    Convert.ToBoolean(MessageQueue.IsFileAttachment),
                    MessageQueue.AttachmentFileName
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
                objErrorlog.Methodname = "InsertMessageQueue";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        // Permission
        public List<Permission> GetAllPermissionForUser(string UserId)
        {
            List<Permission> lstPermission = new List<Permission>();
            Permission objPermission = new Permission();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPermissionForUser", new Guid(UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objPermission = new Permission();
                        objPermission.PermissionId = ds.Tables[0].Rows[i]["PermissionId"].ToString();
                        objPermission.PermissionDescription = ds.Tables[0].Rows[i]["PermissionDescription"].ToString();
                        lstPermission.Add(objPermission);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientRequests";
                string result = InsertErrorLog(objErrorlog);
            }
            return lstPermission;
        }

        public List<Role> GetAllRolesForUser(string UserId)
        {
            List<Role> lstRole = new List<Role>();
            Role objRole = new Role();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllRolesForUser", new Guid(UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objRole.RoleId = ds.Tables[0].Rows[i]["RoleId"].ToString();
                        objRole.RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString();
                        lstRole.Add(objRole);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientRequests";
                string result = InsertErrorLog(objErrorlog);
            }
            return lstRole;
        }

        // Administrator
        public string InsertUpdateSuperAdmin(SuperAdmin SuperAdmin)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertSuperAdmin",
                                                    new Guid(SuperAdmin.UserId),
                                                    SuperAdmin.Name,
                                                    SuperAdmin.Password,
                                                     SuperAdmin.UserName,
                                                      SuperAdmin.Email
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
                objErrorlog.Pagename = "SuperAdminService";
                objErrorlog.Methodname = "InsertUpdateSuperAdmin";
                objErrorlog.UserID = SuperAdmin.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string InsertUserPermission(string UserId, string PermissionId)
        {

            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUserPermission",
                                                    new Guid(UserId),
                                                    new Guid(PermissionId)
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
                objErrorlog.Pagename = "SuperAdminService";
                objErrorlog.Methodname = "InsertUserPerrmission";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string DeleteUserWithPermissions(string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteUserWithPermissions", new Guid(UserId));

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
                objErrorlog.Methodname = "DeleteUserWithPermissions";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public List<SuperAdmin> GetAllAdmin()
        {
            List<SuperAdmin> lstSuperAdmin = new List<SuperAdmin>();
            SuperAdmin objSuperAdmin = new SuperAdmin();
            try
            {

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAdmins");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objSuperAdmin.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objSuperAdmin.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objSuperAdmin.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objSuperAdmin.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        lstSuperAdmin.Add(objSuperAdmin);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "SuperAdminService";
                objErrorlog.Methodname = "GetAllAdmin";
                string result = InsertErrorLog(objErrorlog);
            }
            return lstSuperAdmin;
        }

        //public AdminsList GetAllAdmin(string pageNo, string recordPerPage, string sortfield, string sortorder, string search)
        //{
        //    AdminsList ListAdminsList = new AdminsList();
        //    List<SuperAdmin> lstSuperAdmin = new List<SuperAdmin>();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAdmins",
        //                                                Convert.ToInt32(pageNo),
        //                                                Convert.ToInt32(recordPerPage),
        //                                                sortfield,
        //                                                sortorder,
        //                                                search);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {

        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                SuperAdmin objSuperAdmin = new SuperAdmin();
        //                objSuperAdmin.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
        //                objSuperAdmin.Name = ds.Tables[0].Rows[i]["Name"].ToString();
        //                objSuperAdmin.Email = ds.Tables[0].Rows[i]["Email"].ToString();
        //                objSuperAdmin.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
        //                objSuperAdmin.Password = ds.Tables[0].Rows[i]["Password"].ToString();
        //                lstSuperAdmin.Add(objSuperAdmin);
        //            }

        //            ListAdminsList.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
        //            ListAdminsList.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
        //            ListAdminsList.AdminList = lstSuperAdmin;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetAllAdmin";
        //        string result = InsertErrorLog(objErrorlog);
        //    }
        //    return ListAdminsList;
        //}

        public string DeleteAdmin(string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteAdmin", new Guid(UserId));

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
                objErrorlog.Methodname = "DeleteAdmin";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public SuperAdmin GetAdminDetailsById(string UserId)
        {
            List<SuperAdmin> lstSuperAdmin = new List<SuperAdmin>();
            SuperAdmin objSuperAdmin = new SuperAdmin();
            try
            {

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAdminDetailsById", new Guid(UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objSuperAdmin.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objSuperAdmin.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objSuperAdmin.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objSuperAdmin.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objSuperAdmin.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "SuperAdminService";
                objErrorlog.Methodname = "GetAdminDetailsById";
                string result = InsertErrorLog(objErrorlog);
            }
            return objSuperAdmin;
        }
        // Push Notification
        public string InsertPushNotification(Notification ObjNotification)
        {
            string result = "";
            try
            {
                DataSet set = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertPushNotification",
                                                       ObjNotification.AppointmentId,
                                                       ObjNotification.NurseId,
                                                       ObjNotification.PatientId,
                                                       ObjNotification.Type,
                                                       ObjNotification.Title,
                                                       ObjNotification.Message,
                                                       ObjNotification.UserList,
                                                       new Guid(ObjNotification.InsertUserId));

                if (set.Tables[0].Rows.Count > 0)
                {
                    result = set.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
            return result;

        }

        public string InsertSentNotification(Notification ObjNotification)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertSentNotification",
                                                    ObjNotification.NotificationId,
                                                    ObjNotification.IosResponse.Status,
                                                    ObjNotification.IosResponse.Response,
                                                    ObjNotification.IosResponse.Json,
                                                    ObjNotification.AndroidResponse.Status,
                                                    ObjNotification.AndroidResponse.Response,
                                                    ObjNotification.AndroidResponse.Json,
                                                    new Guid(ObjNotification.InsertUserId));

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
            return result;

        }
        //CC
        public CareGivers GetCareGiverByUserId(string UserId)
        {
            CareGivers CareGiver = new CareGivers();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCareGiverByUserId", new Guid(UserId));
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetCareGiverByUserId", new Guid(UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseId"]);
                    CareGiver.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    CareGiver.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    CareGiver.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    CareGiver.ServiceId = ds.Tables[0].Rows[0]["Service"].ToString();
                    CareGiver.ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString();
                    CareGiver.HourlyRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["HourlyRate"].ToString());
                    CareGiver.DistanceFromLocation = Convert.ToDecimal(ds.Tables[0].Rows[0]["DistanceFromLocation"].ToString());
                    CareGiver.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                    CareGiver.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    CareGiver.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProfileImage"].ToString()))
                    { 
                    CareGiver.ProfileImage = CareGiverProfileImagesPath + ds.Tables[0].Rows[0]["ProfileImage"].ToString();
                    }
                    else
                    {
                        CareGiver.ProfileImage = "";
                    }

                    CareGiver.Certificate = CareGiverDocumentPath + ds.Tables[0].Rows[0]["Certificate"].ToString();
                    CareGiver.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAvailable"].ToString());
                    CareGiver.IsBusy = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBusy"].ToString());
                    CareGiver.IsApprove = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsApprove"].ToString());
                    CareGiver.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    CareGiver.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    CareGiver.DistanceUnit = ds.Tables[0].Rows[0]["DistanceUnit"].ToString();
                    CareGiver.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    CareGiver.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    CareGiver.IsOfficePermission = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsOfficePermission"]);
                    CareGiver.IsAllowOneToOneChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowOneToOneChat"]);
                    CareGiver.IsAllowPatientChatRoom = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowPatientChatRoom"]);
                    CareGiver.IsAllowGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowGroupChat"]);
                    CareGiver.IsAllowToCreateGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowToCreateGroupChat"]);
                    CareGiver.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"]);

                    var ProfileImage = ds.Tables[0].Rows[0]["ProfileImage"].ToString();
                    if (!string.IsNullOrEmpty(ProfileImage))
                    {
                        CareGiver.ProfileImage = CareGiverProfileImagesPath + ProfileImage;

                    }
                    else
                    {
                        CareGiver.ProfileImage = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                    }

                    CareGiver.OrganisationId= Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"]);
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllCareGiverByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiver;
        }

        //cc
        public PatientRequestList GetAllPatientRequestsByNurseId(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string NurseId)
        {
            PatientRequestList ListPatientRequest = new PatientRequestList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllPatientRequestsByNurseId",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        NurseId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<PatientRequests> PatientRequestList = new List<PatientRequests>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientEmail = ds.Tables[0].Rows[i]["PatientEmail"].ToString();
                        objPatientRequest.PatientPhone = ds.Tables[0].Rows[i]["PatientPhone"].ToString();
                        objPatientRequest.PatientAddress = ds.Tables[0].Rows[i]["PatientAddress"].ToString();
                        objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objPatientRequest.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objPatientRequest.HourRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["HourRate"]);
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalHours"]);
                        objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.NurseEmail = ds.Tables[0].Rows[i]["NurseEmail"].ToString();
                        objPatientRequest.NursePhone = ds.Tables[0].Rows[i]["NursePhone"].ToString();
                        objPatientRequest.RescheduleStatus = ds.Tables[0].Rows[i]["RescheduleStatus"].ToString();
                        objPatientRequest.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAvailable"]);
                        objPatientRequest.IsBusy = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsBusy"]);
                        objPatientRequest.IsComplete = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsComplete"]);
                        PatientRequestList.Add(objPatientRequest);
                    }

                    ListPatientRequest.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListPatientRequest.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListPatientRequest.PatientRequestsList = PatientRequestList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllPatientRequests";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListPatientRequest;
        }
        public string ResetPassword(CareGivers CareGiver)
        {
            string result = "";
            try
            {
                if (CareGiver.UserId != "" && CareGiver.UserId != null)
                {
                    MembershipUser user = Membership.GetUser(new Guid(CareGiver.UserId));
                    if (user.ChangePassword(user.ResetPassword(), CareGiver.NewPassword))
                    {
                        int j = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateUserPassword",
                                                           new Guid(CareGiver.UserId),
                                                           CareGiver.NewPassword);

                        result = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "ResetPassword";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        public string InsertNurseCertificate(string NurseId, string Certificate, string UserId)
        {
            string result = "";
            int i = 0;
            try
            {
                i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertNurseCertificate",
                                                          NurseId,
                                                          Certificate,
                                                          new Guid(UserId));
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertNurseCertificate";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            if (i > 0)
            {
                result = "Success";
            }
            return result;
        }

        public List<CareGivers> GetCareGiverCertiByNurseId(string NurseId)
        {
            CareGivers CareGiver = new CareGivers();
            List<CareGivers> objCareGivers = new List<CareGivers>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCareGiverCertiByNurseId", Convert.ToInt32(NurseId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGiver = new CareGivers();
                        CareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        var Certificates = ds.Tables[0].Rows[i]["Certificate"].ToString();
                        if (Certificates != null && Certificates != "")
                        {
                            CareGiver.Certificate = CareGiverProfileCertificate + ds.Tables[0].Rows[i]["Certificate"].ToString();
                        }
                        CareGiver.NurseCertificateId = ds.Tables[0].Rows[i]["NurseCertificateId"].ToString();
                        CareGiver.IsCertificateApproved = ds.Tables[0].Rows[i]["IsApproved"].ToString();

                        objCareGivers.Add(CareGiver);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllCareGiverByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return objCareGivers;
        }

        public List<PatientRequests> GetPaymentReceivedByNurseId(string NurseId, string PatientName, string FromDate, string ToDate)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                if (NurseId == "0")
                {
                    NurseId = "";
                }
                if (PatientName == "0")
                {
                    PatientName = "";
                }
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPaymentReceivedByNurseId",
                                                        NurseId,
                                                        PatientName,
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.PaidAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["PaidAmount"]);

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPaymentReceived";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public PatientRequestList GetAppointmentsByNurseId(string NurseId, string IsComplete, string pageNo, string recordPerPage, string sortfield, string sortorder, string search)
        {
            PatientRequestList objPatientRequestList = new PatientRequestList();
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAppointmentsByNurseId",
                                                        NurseId,
                                                        IsComplete,
                                                         Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.ServiceId = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceId"]);
                        objPatientRequest.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        objPatientRequest.HourRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["HourRate"]);
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalHours"]);
                        objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);
                        objPatientRequest.PaidAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["PaidAmount"]);
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.RescheduleStatus = ds.Tables[0].Rows[i]["RescheduleStatus"].ToString();
                        objPatientRequest.PatientAddress = ds.Tables[0].Rows[i]["PatientAddress"].ToString();

                        PatientRequestList.Add(objPatientRequest);
                    }
                    objPatientRequestList.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    objPatientRequestList.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    objPatientRequestList.PatientRequestsList = PatientRequestList;
                    objPatientRequestList.TotalAmount = Convert.ToDecimal(ds.Tables[3].Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAppointmentsByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return objPatientRequestList;
        }
        //cc
        public List<PatientRequests> GetRescheduleAlertsByNurseId(string NurseId)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetRescheduleAlertsByNurseId",
                                                       Convert.ToInt32(NurseId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.RescheduleType = ds.Tables[0].Rows[i]["RescheduleType"].ToString();

                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetRescheduleAlerts";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public List<PatientRequests> GetCancelledAppointmentByPatientForNurseId(string NurseId)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCancelledAppointmentByPatientForNurseId",
                                                       Convert.ToInt32(NurseId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientAddress = ds.Tables[0].Rows[i]["PatientAddress"].ToString();
                        objPatientRequest.HourRate = Convert.ToInt32(ds.Tables[0].Rows[i]["HourRate"]);
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objPatientRequest.TotalHours = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalHours"]);

                        var hours = Math.Floor(Convert.ToDecimal(objPatientRequest.TotalHours) / 60);
                        var minutes = Convert.ToDecimal(objPatientRequest.TotalHours) % 60;
                        minutes = (minutes == 0 ? 0 : (minutes / 60));
                        var TotalHourHtml = hours + minutes;

                        objPatientRequest.TotalHours = Convert.ToInt32(TotalHourHtml);
                        objPatientRequest.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalAmount"]);
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);


                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetRescheduleAlerts";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public List<PatientRequests> GetPaymentRequertForMapByNurseId(string NurseId, string NurseName, string FromDate, string ToDate)
        {
            List<PatientRequests> PatientRequestList = new List<PatientRequests>();
            try
            {
                if (NurseId == "0")
                {
                    NurseId = "";
                }
                if (NurseName == "0")
                {
                    NurseName = "";
                }
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPaymentRequertForMapByNurseId",
                                                        NurseId,
                                                        NurseName,
                                                        Convert.ToDateTime(FromDate),
                                                        Convert.ToDateTime(ToDate));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientRequests objPatientRequest = new PatientRequests();
                        objPatientRequest.AppointmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AppointmentId"]);
                        objPatientRequest.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientRequest.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientRequest.PatientZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objPatientRequest.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objPatientRequest.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objPatientRequest.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objPatientRequest.NurseName = ds.Tables[0].Rows[i]["NurseName"].ToString();
                        objPatientRequest.PaidAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["PaidAmount"]);
                        objPatientRequest.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objPatientRequest.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();
                        objPatientRequest.PatientAddress = ds.Tables[0].Rows[i]["Address"].ToString();
                        PatientRequestList.Add(objPatientRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPaymentReceived";
                string result = InsertErrorLog(objErrorlog);
            }
            return PatientRequestList;
        }

        public string UpdateAvailableOrBusyStatus(string NurseId, string UserId, string StatusId, string StatusType)
        {
            string result = "";
            try
            {
                int j = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateAvailableOrBusyStatus",
                                                   Convert.ToInt32(NurseId),
                                                   new Guid(UserId),
                                                  Convert.ToInt32(StatusId),
                                                  StatusType);
                if (j > 0)
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
                objErrorlog.Methodname = "UpdateAvailableOrBusyStatus";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public StripeAccessToken GetStripeAccessTokenByNurseId(string NurseId)
        {
            StripeAccessToken objAccessToken = new StripeAccessToken();
            try
            {
                DataSet set = DifferenzLibrary.DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetStripeAccessTokenByNurseId", Convert.ToInt32(NurseId));

                if (set.Tables[0].Rows.Count > 0)
                {
                    objAccessToken.NurseId = int.Parse(set.Tables[0].Rows[0]["NurseId"].ToString());
                    objAccessToken.AccessToken = set.Tables[0].Rows[0]["AccessToken"].ToString();
                    objAccessToken.Livemode = set.Tables[0].Rows[0]["LiveMode"].ToString();
                    objAccessToken.RefreshToken = set.Tables[0].Rows[0]["RefreshToken"].ToString();
                    objAccessToken.TokenType = set.Tables[0].Rows[0]["TokenType"].ToString();
                    objAccessToken.StripePublishableKey = set.Tables[0].Rows[0]["StripePublishableKey"].ToString();
                    objAccessToken.StripeUserID = set.Tables[0].Rows[0]["StripeUserID"].ToString();
                    objAccessToken.Scope = set.Tables[0].Rows[0]["Scope"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
            return objAccessToken;
        }

        //    Stripe Access Token  //
        public string InsertStripeAccessToken(StripeAccessToken StripeAccessToken)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertStripeAccessToken",
                    StripeAccessToken.AccessToken,
                    StripeAccessToken.Livemode,
                    StripeAccessToken.RefreshToken,
                    StripeAccessToken.TokenType,
                    StripeAccessToken.StripePublishableKey,
                    StripeAccessToken.StripeUserID,
                    StripeAccessToken.Scope,
                    StripeAccessToken.NurseId,
                    new Guid(StripeAccessToken.UserId)
                    );

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
            return result;
        }
        public string UpdateNurseSchedule(CareGiverMultipleTimeSlots objCareGiverMultipleTimeSlots)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateNurseSchedule",
                                                  objCareGiverMultipleTimeSlots.TimeSlots, objCareGiverMultipleTimeSlots.Schedules, objCareGiverMultipleTimeSlots.NurseId,
                                                  objCareGiverMultipleTimeSlots.Week, objCareGiverMultipleTimeSlots.Year, 0,
                                                    new Guid(objCareGiverMultipleTimeSlots.UserId));
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
                objErrorlog.Pagename = "SuperAdminService";
                objErrorlog.Methodname = "UpdateNurseSchedule";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        public string DeleteTimeSlotByTimeSlotId(CareGiverTimeSlots CareGiverTimeSlot)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteTimeSlotByTimeSlotId",
                                                    CareGiverTimeSlot.TimeSlotId,
                                                    new Guid(CareGiverTimeSlot.UserId));

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
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "DeleteTimeSlotByTimeSlotId";
                objErrorlog.UserID = CareGiverTimeSlot.UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        string SiteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"].ToString();

        #region ==> ForgotPassword
        public string ForgotPassword(string Email)
        {
            try
            {
                Random rand = new Random();
                int verificationcode = rand.Next(100000, 999999);
                string Result = "";
                string encrypted = Encryption.Encrypt(String.Format("{0}&{1}&{2}", Email, verificationcode, DateTime.Now.AddMinutes(1440).Ticks));

                var passwordLink = SiteUrl +
                      "Account/ResetPassword?digest=" +
                      HttpUtility.UrlEncode(encrypted);
                string EmailBody = "<p>A request has been recieved to reset your password.If you did not initiate the request, then please ignore this email.</p>";
                EmailBody += "<p>Please click the following link to reset your password:  <a href='" + passwordLink + "'>Reset Password</a></p>";

                if (Membership.GetUserNameByEmail(Email) == null)
                {
                    Result = "Email is not exists";
                    return Result;
                }

                string UserName = Membership.GetUserNameByEmail(Email);
                string UserId = Membership.GetUser(UserName).ProviderUserKey.ToString();
                Result = InsertUpdateResetPasswordToken(verificationcode.ToString(), UserId);

                if (Result == "Success")
                {
                    if (sendEmail(Email, "Forgot Password", EmailBody, false, "", ""))
                    {
                        return "True";
                    }
                    else
                    {

                        return "False";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "DeleteTimeSlotByTimeSlotId";
                string res = InsertErrorLog(objErrorlog);
            }
            return "";
        }
        #endregion


        #region ==> EmailtoSetPassword
        public string EmailtoSetPassword(string Email)
        {
            try
            {
                Random rand = new Random();
                int verificationcode = rand.Next(100000, 999999);
                string Result = "";
                string encrypted = Encryption.Encrypt(String.Format("{0}&{1}&{2}", Email, verificationcode, DateTime.Now.AddMinutes(1440).Ticks));

                var passwordLink = SiteUrl +
                      "Account/ResetPassword?digest=" +
                      HttpUtility.UrlEncode(encrypted);
                string EmailBody = "<p>Your organization has added you to PaSeva. <br> Please click the following link to set your password:  <a href='" + passwordLink + "'>Set Password</a></p><br>";
                EmailBody += "<p>If you or your organization did not initiate this request, please ignore this email.</p><br>";

          
                EmailBody += "<p>Welcome and thank you,<br>";

                EmailBody += "<strong>PaSeva Support Team</strong></p>";

                            
                if (Membership.GetUserNameByEmail(Email) == null)
                {
                    Result = "Email is not exists";
                    return Result;
                }

                string UserName = Membership.GetUserNameByEmail(Email);
                string UserId = Membership.GetUser(UserName).ProviderUserKey.ToString();
                Result = InsertUpdateResetPasswordToken(verificationcode.ToString(), UserId);

                if (Result == "Success")
                {
                    if (sendEmail(Email, "Welcome-You’ve Been Added to PaSeva", EmailBody, false, "", ""))
                    {
                        return "True";
                    }
                    else
                    {

                        return "False";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "DeleteTimeSlotByTimeSlotId";
                string res = InsertErrorLog(objErrorlog);
            }
            return "";
        }
        #endregion



        private string InsertUpdateResetPasswordToken(string Token, string UserID)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateResetPasswordToken",
                                                    Token,
                                                    new Guid(UserID));

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
                objErrorlog.Methodname = "InsertUpdateResetPasswordToken";
                objErrorlog.UserID = UserID;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        private bool sendEmail(string toAddress, string subject, string body, bool IsFileAttachment, string AttachmentFileName, string CCMailID, bool isBodyHtml = true)
        {
            try
            {
                var mailMessage = new MailMessage();
                // MailAddress bccAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"]);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                mailMessage.To.Add(toAddress);
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"]);
                if (!(string.IsNullOrEmpty(CCMailID)))
                    mailMessage.CC.Add(CCMailID);
                mailMessage.Subject = subject;
                if (IsFileAttachment == true)
                {
                    string[] ach = AttachmentFileName.Split(';');
                    for (int i = 0; i < ach.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(ach[i]))
                        {
                            Attachment attachFile = new Attachment(ach[i]);
                            mailMessage.Attachments.Add(attachFile);
                        }
                    }
                }


                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;
                var smtpClient = new SmtpClient { EnableSsl = false };
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Host = "smtp.office365.com";
                //ConfigurationManager.AppSettings["SMTPHost"];
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.UseDefaultCredentials = false;
               // mailMessage.Bcc.Add(bccAddress);
                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SENDER_EMAIL_ID"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "sendEmail";
                string result = InsertErrorLog(objErrorlog);
            }
            return false;
        }

        public string DeleteNurseCertificateById(string UserId, string NurseCertificateId)
        {
            string result = "";


            CareGivers objCareGiver = GetCareGiverByUserId(UserId);
            int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteNurseCertificateById",
                                                         new Guid(UserId),
                                                         Convert.ToInt32(NurseCertificateId));
            if (i > 0)
            {
                result = "Success";
            }
            return result;
        }


        public List<CareGiverSchedule> GetAppointmentByTimeSlotId(string TimeSlotId, string SlotDate)
        {
            List<CareGiverSchedule> ScheduleList = new List<CareGiverSchedule>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAppointmentByTimeSlotId",
                                                        TimeSlotId,
                                                        SlotDate);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGiverSchedule objSchedule = new CareGiverSchedule();

                        objSchedule.PartialFromTime = ds.Tables[0].Rows[i]["PartialFromTime"].ToString();
                        objSchedule.PartialToTime = ds.Tables[0].Rows[i]["PartialToTime"].ToString();
                        ScheduleList.Add(objSchedule);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAppointmentByTimeSlotId";
                string result = InsertErrorLog(objErrorlog);
            }
            return ScheduleList;
        }
        public List<UnavailabilityRequest> GetAppointmentsForUnavailability(string TimeSlotId, string SlotDate)
        {
            List<UnavailabilityRequest> ScheduleList = new List<UnavailabilityRequest>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAppointmentsForUnavailability",
                                                        TimeSlotId,
                                                        SlotDate);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UnavailabilityRequest objSchedule = new UnavailabilityRequest();
                        objSchedule.SlotFromTime = ds.Tables[0].Rows[i]["SlotFromTime"].ToString();
                        objSchedule.SlotToTime = ds.Tables[0].Rows[i]["SlotToTime"].ToString();
                        objSchedule.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objSchedule.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objSchedule.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objSchedule.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objSchedule.DisplayDate = ds.Tables[0].Rows[i]["DisplayDate"].ToString();
                        objSchedule.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        objSchedule.ServiceId = ds.Tables[0].Rows[i]["ServiceId"].ToString();

                        ScheduleList.Add(objSchedule);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAppointmentsForUnavailability";
                string result = InsertErrorLog(objErrorlog);
            }
            return ScheduleList;
        }

        public string InsertUnavailabilityRequest(UnavailabilityRequest UnavailabilityRequest)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUnavailabilityRequest",
                    UnavailabilityRequest.NurseId,
                    UnavailabilityRequest.ServiceId,
                    UnavailabilityRequest.Date,
                    UnavailabilityRequest.FromTime,
                    UnavailabilityRequest.ToTime,
                    UnavailabilityRequest.CancelComment,
                    new Guid(UnavailabilityRequest.UserId),
                    UnavailabilityRequest.TimeSlotId
                    );

                if (i > 0)
                {
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
            return result;
        }

        public string GetUnavailabilityRequestByTimeSlot(string TimeSlotId, string SlotDate)
        {
            string Result = null;
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetUnavailabilityRequestByTimeSlot",
                                                        TimeSlotId,
                                                        SlotDate
                                                       );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Result = ds.Tables[0].Rows[i]["Result"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetUnavailabilityRequestByTimeSlot";
                string result = InsertErrorLog(objErrorlog);
            }
            return Result;
        }
        public decimal GetMorningShiftHours(string Date, string strFromTime, string strToTime)
        {
            TimeSpan MorningShiftStartTime = Convert.ToDateTime(ConfigurationManager.AppSettings["MorningShiftStartTime"]).TimeOfDay;
            TimeSpan MorningShiftEndTime = Convert.ToDateTime(ConfigurationManager.AppSettings["MorningShiftEndTime"]).TimeOfDay;
            TimeSpan NightShiftStartTime = Convert.ToDateTime(ConfigurationManager.AppSettings["NightShiftStartTime"]).TimeOfDay;
            TimeSpan NightShiftEndTime = Convert.ToDateTime(ConfigurationManager.AppSettings["NightShiftEndTime"]).TimeOfDay;
            TimeSpan FromTime = Convert.ToDateTime(strFromTime).TimeOfDay;
            TimeSpan ToTime = Convert.ToDateTime(strToTime).TimeOfDay;

            decimal MorningShiftHours = 0;
            if (FromTime >= MorningShiftStartTime && ToTime <= MorningShiftEndTime)
            {
                MorningShiftHours = Convert.ToDecimal((ToTime.Subtract(FromTime)).TotalHours);
            }
            else if (FromTime <= MorningShiftStartTime && ToTime >= MorningShiftStartTime && ToTime <= MorningShiftEndTime)
            {
                MorningShiftHours = Convert.ToDecimal(ToTime.Subtract(MorningShiftStartTime).TotalHours);
            }
            else if (FromTime >= MorningShiftStartTime && FromTime <= MorningShiftEndTime && ToTime >= MorningShiftEndTime)
            {
                MorningShiftHours = Convert.ToDecimal(MorningShiftEndTime.Subtract(FromTime).TotalHours);
            }
            else if (FromTime <= MorningShiftStartTime && ToTime >= MorningShiftEndTime)
            {
                MorningShiftHours = Convert.ToDecimal(MorningShiftEndTime.Subtract(MorningShiftStartTime).TotalHours);
            }
            return MorningShiftHours;
        }

        public decimal GetNightShiftHours(string Date, string strFromTime, string strToTime)
        {
            TimeSpan MorningShiftStartTime = Convert.ToDateTime(ConfigurationManager.AppSettings["MorningShiftStartTime"]).TimeOfDay;
            TimeSpan MorningShiftEndTime = Convert.ToDateTime(ConfigurationManager.AppSettings["MorningShiftEndTime"]).TimeOfDay;
            TimeSpan NightShiftStartTime = Convert.ToDateTime(ConfigurationManager.AppSettings["NightShiftStartTime"]).TimeOfDay;
            TimeSpan NightShiftEndTime = Convert.ToDateTime(ConfigurationManager.AppSettings["NightShiftEndTime"]).TimeOfDay;
            TimeSpan FromTime = Convert.ToDateTime(strFromTime).TimeOfDay;
            TimeSpan ToTime = Convert.ToDateTime(strToTime).TimeOfDay;

            decimal NightShiftHours = 0;

            if (FromTime <= MorningShiftStartTime && ToTime <= MorningShiftStartTime)
            {
                NightShiftHours = Convert.ToDecimal((ToTime.Subtract(FromTime)).TotalHours);
            }
            else if (FromTime <= MorningShiftStartTime && ToTime >= MorningShiftStartTime && ToTime <= MorningShiftEndTime)
            {
                NightShiftHours = Convert.ToDecimal((MorningShiftStartTime.Subtract(FromTime)).TotalHours);
            }
            else if (FromTime >= MorningShiftStartTime && FromTime <= MorningShiftEndTime && ToTime >= MorningShiftEndTime)
            {
                NightShiftHours = Convert.ToDecimal(ToTime.Subtract(MorningShiftEndTime).TotalHours);
            }
            else if (FromTime >= MorningShiftEndTime && ToTime >= MorningShiftEndTime)
            {
                NightShiftHours = Convert.ToDecimal(ToTime.Subtract(FromTime).TotalHours);
            }
            else if (FromTime <= MorningShiftStartTime && ToTime >= MorningShiftEndTime)
            {
                NightShiftHours = Convert.ToDecimal((MorningShiftStartTime.Subtract(FromTime)).TotalHours);
                NightShiftHours += Convert.ToDecimal(ToTime.Subtract(MorningShiftEndTime).TotalHours);
            }

            return NightShiftHours;
        }

        public decimal GetMorningShiftCharge(string Date, HoursRate HoursRate)
        {
            decimal MorningShiftCharge = HoursRate.WeekDayRate;
            string Day = Convert.ToDateTime(Date).Day.ToString();
            string Month = Convert.ToDateTime(Date).ToString("MMMM");
            string WeekDay = Convert.ToDateTime(Date).ToString("dddd").ToLower();

            List<Holiday> HolidaysList = GetHolidaysList();
            HolidaysList = HolidaysList.Where(x => x.Day == Day && x.Month == Month).ToList();
            if (HolidaysList.Count > 0)
                MorningShiftCharge = HoursRate.HolidayRate > MorningShiftCharge ? HoursRate.HolidayRate : MorningShiftCharge;
            if (WeekDay == "saturday" || WeekDay == "sunday")
                MorningShiftCharge = HoursRate.WeekEndDayRate > MorningShiftCharge ? HoursRate.WeekEndDayRate : MorningShiftCharge;

            return Math.Round(MorningShiftCharge, 2, MidpointRounding.AwayFromZero);
        }


        public decimal GetNightShiftCharge(string Date, HoursRate HoursRate)
        {
            decimal NightShiftCharge = HoursRate.WeekNightRate;
            string Day = Convert.ToDateTime(Date).Day.ToString();
            string Month = Convert.ToDateTime(Date).ToString("MMMM");
            string WeekDay = Convert.ToDateTime(Date).ToString("dddd").ToLower();

            List<Holiday> HolidaysList = GetHolidaysList();
            HolidaysList = HolidaysList.Where(x => x.Day == Day && x.Month == Month).ToList();
            if (HolidaysList.Count > 0)
                NightShiftCharge = HoursRate.HolidayRate > NightShiftCharge ? HoursRate.HolidayRate : NightShiftCharge;
            if (WeekDay == "saturday" || WeekDay == "sunday")
                NightShiftCharge = HoursRate.WeekEndNightRate > NightShiftCharge ? HoursRate.WeekEndNightRate : NightShiftCharge;
            return Math.Round(NightShiftCharge, 2, MidpointRounding.AwayFromZero);
        }


        public List<Holiday> GetHolidaysList()
        {
            List<Holiday> HolidaysList = new List<Holiday>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ListHolidaysForMobile");
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            Holiday objHoliday = new Holiday();
                            objHoliday.ID = Convert.ToInt32(item["ID"]);
                            objHoliday.Name = item["Name"].ToString();
                            objHoliday.Day = item["Day"].ToString();
                            objHoliday.Month = item["Month"].ToString();

                            HolidaysList.Add(objHoliday);
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "PatientsService";
                objErrorlog.Methodname = "GetHolidaysList";
                InsertErrorLog(objErrorlog);
            }
            return HolidaysList;
        }

        #region Caregiver Review

        public string InsertUpdateCareGiverReview(CareGiverReview CareGiverReview)
        {
            string result = string.Empty;
            //int i = 0;
            try
            {
                //i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateCareGiverReview",
                //                                    CareGiverReview.PatientId,
                //                                    CareGiverReview.NurseId,
                //                                    CareGiverReview.ServiceId,
                //                                    CareGiverReview.AppointmentId,
                //                                    CareGiverReview.Rating,
                //                                    CareGiverReview.ReviewTitle,
                //                                    CareGiverReview.ReviewContent,
                //                                    new Guid(CareGiverReview.InsertUserId),
                //                                    CareGiverReview.IsApproved
                //                                    );
                Int32 ValidDay = Convert.ToInt32(ConfigurationManager.AppSettings["ValidDay"]);
                DataSet dsData = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateCareGiverReview",
                                                   CareGiverReview.PatientId,
                                                   CareGiverReview.NurseId,
                                                   CareGiverReview.ServiceId,
                                                   CareGiverReview.AppointmentId,
                                                   CareGiverReview.Rating,
                                                   CareGiverReview.ReviewTitle,
                                                   CareGiverReview.ReviewContent,
                                                   new Guid(CareGiverReview.InsertUserId),
                                                   CareGiverReview.IsApproved,
                                                   ValidDay
                                                   );

                //if (i > 0)
                if (dsData.Tables[0].Rows[0][0].ToString() == "1")
                {
                    string strCaregiverEmail = "";
                    string strcaregiverName = "";
                    string strPatientName = "";
                    string SuperAdminEmail = ConfigurationManager.AppSettings["SuperAdminEmail"];
                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientReviewEmail", CareGiverReview.NurseId, CareGiverReview.PatientId);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strCaregiverEmail = ds.Tables[0].Rows[0]["caregiveremail"].ToString();
                        strcaregiverName = ds.Tables[0].Rows[0]["Name"].ToString();
                        strPatientName = ds.Tables[1].Rows[0]["PatientName"].ToString();
                        string msg = "";

                        msg += "Hello,<br><br>";
                        msg += strcaregiverName + " just wrote a review for patient " + strPatientName + "<br><br>";
                        msg += "See details as below.<br><br>";
                        msg += "Ratings   &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp: " + CareGiverReview.Rating + "&nbsp stars  <br>";
                        //msg += "Review Title &nbsp: " + CareGiverReview.ReviewTitle + " <br>";
                        msg += "Description  &nbsp&nbsp: " + CareGiverReview.ReviewContent + " <br><br>";
                        msg += "Warm Regards,<br>";
                        msg += "Team PaSeva";
                        //jigneshc @differenzsystem.com
                        //if (sendEmail(SuperAdminEmail, "CareGiver Review on "+strcaregiverName+"", msg, false, "", ""))
                        if (sendEmail(SuperAdminEmail, "CareGiver Review on " + strPatientName + "", msg, false, "", ""))
                        {
                            result = "Success";
                        }
                        else
                        {
                            result = "SupperAdminMailFail";
                        }
                    }
                }
                else if (dsData.Tables[0].Rows[0][0].ToString() == "2")
                {
                    result = "OutOfDate";
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertUpdateCareGiverReview";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public CareGiverReviewList GetAllCareGiverReview(string PatientId)
        {
            List<CareGiverReviewList> lstReview = new List<CareGiverReviewList>();
            CareGiverReviewList objReviewList = new CareGiverReviewList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllCareGiverReview", Convert.ToInt32(PatientId));
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {

                        List<DisplayCareGiverReview> lstCareGiverReview = new List<DisplayCareGiverReview>();
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            DisplayCareGiverReview objCareGiverReview = new DisplayCareGiverReview();
                            objCareGiverReview.PatientId = Convert.ToInt32(item["PatientId"]);
                            objCareGiverReview.NurseId = Convert.ToInt32(item["NurseId"]);
                            objCareGiverReview.PatientName = Convert.ToString(item["PatientName"]);
                            objCareGiverReview.CareGiverName = Convert.ToString(item["NurseName"]);
                            objCareGiverReview.ReviewTitle = Convert.ToString(item["ReviewTitle"]);
                            objCareGiverReview.ReviewContent = Convert.ToString(item["ReviewContent"]);
                            //objCareGiverReview.ReviewDate = Convert.ToString(item["InsertDateTime"]);
                            objCareGiverReview.ReviewDate = Convert.ToString(item["ReviewDate"]);

                            objCareGiverReview.StarRating = Convert.ToDouble(item["Rating"]);
                            lstCareGiverReview.Add(objCareGiverReview);
                        }

                        objReviewList.ListCareGiverReview = lstCareGiverReview;

                        objReviewList.AverageStarRating = Convert.ToDouble(ds.Tables[1].Rows[0]["Average"]);
                        objReviewList.TotalStar = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRating"]);

                        objReviewList.StarOne = Convert.ToString(ds.Tables[2].Rows[0]["STAR1"]);
                        objReviewList.StarTwo = Convert.ToString(ds.Tables[3].Rows[0]["STAR2"]);
                        objReviewList.StarThree = Convert.ToString(ds.Tables[4].Rows[0]["STAR3"]);
                        objReviewList.StarFour = Convert.ToString(ds.Tables[5].Rows[0]["STAR4"]);
                        objReviewList.StarFive = Convert.ToString(ds.Tables[6].Rows[0]["STAR5"]);

                        if (objReviewList != null && objReviewList.ListCareGiverReview.Count > 0 && objReviewList.ListCareGiverReview != null)
                        {
                            objReviewList.Result = "Success";
                        }
                        else
                        {
                            objReviewList.Result = "Fail";
                        }
                        return objReviewList;
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllCareGiverReview";
                InsertErrorLog(objErrorlog);
                objReviewList.Result = "Fail";
            }
            return objReviewList;
        }

        public CareGiverReviewList GetCareGiverReview(string NurseId)
        {
            List<CareGiverReviewList> lstReview = new List<CareGiverReviewList>();
            CareGiverReviewList objReviewList = new CareGiverReviewList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCareGiverReview", Convert.ToInt32(NurseId));
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {

                        List<DisplayCareGiverReview> lstCareGiverReview = new List<DisplayCareGiverReview>();
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            DisplayCareGiverReview objCareGiverReview = new DisplayCareGiverReview();
                            objCareGiverReview.PatientId = Convert.ToInt32(item["PatientId"]);
                            objCareGiverReview.NurseId = Convert.ToInt32(item["NurseId"]);
                            objCareGiverReview.PatientName = Convert.ToString(item["PatientName"]);
                            objCareGiverReview.CareGiverName = Convert.ToString(item["NurseName"]);
                            objCareGiverReview.ReviewTitle = Convert.ToString(item["ReviewTitle"]);
                            objCareGiverReview.ReviewContent = Convert.ToString(item["ReviewContent"]);
                            //objCareGiverReview.ReviewDate = Convert.ToString(item["InsertDateTime"]);
                            objCareGiverReview.ReviewDate = Convert.ToString(item["ReviewDate"]);

                            objCareGiverReview.StarRating = Convert.ToDouble(item["Rating"]);
                            lstCareGiverReview.Add(objCareGiverReview);
                        }

                        objReviewList.ListCareGiverReview = lstCareGiverReview;

                        objReviewList.AverageStarRating = Convert.ToDouble(ds.Tables[1].Rows[0]["Average"]);
                        objReviewList.TotalStar = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRating"]);

                        objReviewList.StarOne = Convert.ToString(ds.Tables[2].Rows[0]["STAR1"]);
                        objReviewList.StarTwo = Convert.ToString(ds.Tables[3].Rows[0]["STAR2"]);
                        objReviewList.StarThree = Convert.ToString(ds.Tables[4].Rows[0]["STAR3"]);
                        objReviewList.StarFour = Convert.ToString(ds.Tables[5].Rows[0]["STAR4"]);
                        objReviewList.StarFive = Convert.ToString(ds.Tables[6].Rows[0]["STAR5"]);

                        if (objReviewList != null && objReviewList.ListCareGiverReview.Count > 0 && objReviewList.ListCareGiverReview != null)
                        {
                            objReviewList.Result = "Success";
                        }
                        else
                        {
                            objReviewList.Result = "Fail";
                        }
                        return objReviewList;
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiverReview";
                InsertErrorLog(objErrorlog);
                objReviewList.Result = "Fail";
            }
            return objReviewList;
        }

        public CareGiverUserNRate GetCareGiverUserNRate(string NurseId)
        {
            CareGiverUserNRate objReview = new CareGiverUserNRate();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCareGiverUserNRate", Convert.ToInt32(NurseId));
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        objReview.AverageReview = Convert.ToDouble(ds.Tables[0].Rows[0]["Average"]);
                        //objReview.NoOfUser = Convert.ToInt32(ds.Tables[0].Rows[0]["NoOfUser"]);

                        var TotalReview = 0;
                        TotalReview = Convert.ToInt32(ds.Tables[1].Rows[0]["STAR1"]) + Convert.ToInt32(ds.Tables[2].Rows[0]["STAR2"]) + Convert.ToInt32(ds.Tables[3].Rows[0]["STAR3"]) + Convert.ToInt32(ds.Tables[4].Rows[0]["STAR4"]) + Convert.ToInt32(ds.Tables[5].Rows[0]["STAR5"]);
                        objReview.NoOfUser = TotalReview;

                        objReview.StarOne = Convert.ToString(ds.Tables[1].Rows[0]["STAR1"]);
                        objReview.StarTwo = Convert.ToString(ds.Tables[2].Rows[0]["STAR2"]);
                        objReview.StarThree = Convert.ToString(ds.Tables[3].Rows[0]["STAR3"]);
                        objReview.StarFour = Convert.ToString(ds.Tables[4].Rows[0]["STAR4"]);
                        objReview.StarFive = Convert.ToString(ds.Tables[5].Rows[0]["STAR5"]);

                        if (objReview != null && objReview.NoOfUser > 0)
                        {
                            objReview.Result = "Success";
                        }
                        else
                        {
                            objReview.Result = "Fail";
                        }
                        return objReview;
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiverUserNRate";
                InsertErrorLog(objErrorlog);
                objReview.Result = "Fail";
            }
            return objReview;
        }


        public string CheckValidAppointmentDate(string AppointmentId)
        {
            string result = string.Empty;
            try
            {
                Int32 ValidDay = Convert.ToInt32(ConfigurationManager.AppSettings["ValidDay"]);
                DataSet dsData = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "CheckValidAppointmentDate",
                                                           Convert.ToInt32(AppointmentId),
                                                           ValidDay);
                if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                {
                    if (dsData.Tables[0].Rows[0]["Result"].ToString() == "1")
                    {
                        result = "ValidDate";
                    }
                    else
                    {
                        result = "OutOfDate";
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "PatientsService";
                objErrorlog.Methodname = "CheckValidAppointmentDate";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        #endregion

        public CareGiversList InsertSchedulePatientRequest(SchedulePatientRequest SchedulePatientRequest)
        {
            CareGiversList objList = new CareGiversList();
            List<CareGivers> objCareGiverListing = new List<CareGivers>();
            string result = "";
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CaregiverLiteDatabase().ToString(), "InsertSchedulePatientRequest",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CaregiverLiteDatabase().ToString(), "ORG_InsertSchedulePatientRequest",
                                                        SchedulePatientRequest.PatientName,
                                                        SchedulePatientRequest.Address,
                                                        SchedulePatientRequest.Street,
                                                        SchedulePatientRequest.City,
                                                        SchedulePatientRequest.State,
                                                        SchedulePatientRequest.Latitude,
                                                        SchedulePatientRequest.Longitude,
                                                        SchedulePatientRequest.ZipCode,
                                                        SchedulePatientRequest.MedicalId,
                                                        SchedulePatientRequest.Description,
                                                        SchedulePatientRequest.Date,
                                                        SchedulePatientRequest.FromTime,
                                                        SchedulePatientRequest.ToTime,
                                                        SchedulePatientRequest.IsCancelled,
                                                        SchedulePatientRequest.ServiceNames,
                                                        SchedulePatientRequest.VisitTypeNames,
                                                        SchedulePatientRequest.TimezoneId,
                                                        SchedulePatientRequest.TimezoneOffset,
                                                        SchedulePatientRequest.TimezonePostfix,                                                    
                                                        Guid.Parse(SchedulePatientRequest.InsertUserId),
                                                        ConfigurationManager.AppSettings["MaxDistance"],
                                                        SchedulePatientRequest.Office,
                                                        SchedulePatientRequest.OrganisationId,
                                                        SchedulePatientRequest.IsRequiredDriving
                                                        //,

                                                        //SchedulePatientRequest.IsRepeat,
                                                        //SchedulePatientRequest.RepeatEvery,
                                                        //SchedulePatientRequest.RepeatTypeID,
                                                        //SchedulePatientRequest.RepeatDate,
                                                        //SchedulePatientRequest.DayOfWeek,
                                                        //SchedulePatientRequest.DaysOfMonth
                                                        );
                                                        //SchedulePatientRequest.InsertUserId);
                                                        //new System.Data.SqlTypes.SqlGuid(Guid.NewGuid().ToString()));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGivers = new CareGivers();
                        objCareGivers.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        objCareGivers.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"].ToString());
                        objCareGivers.DeviceToken = ds.Tables[0].Rows[i]["DeviceToken"].ToString();
                        objCareGivers.DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
                        objCareGivers.Office = ds.Tables[0].Rows[i]["Office"].ToString();
                        objCareGiverListing.Add(objCareGivers);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertSchedulePatientRequest";
                objErrorlog.UserID = SchedulePatientRequest.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            objList.CareGiverList = objCareGiverListing;
            return objList;
        }

        public string InsertSchedulePatientRequestTemp(string PatientRequestId, string NurseId, string IsNotificationSent)
        {
            CareGiversList objList = new CareGiversList();
            List<CareGivers> objCareGiverListing = new List<CareGivers>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CaregiverLiteDatabase().ToString(), "InsertSchedulePatientRequestTemp",
                                                      PatientRequestId,
                                                      NurseId,
                                                      IsNotificationSent.ToLower() == "success" ? true : false,
                                                      new System.Data.SqlTypes.SqlGuid(Guid.NewGuid().ToString()));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        result = ds.Tables[0].Rows[i]["Result"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "InsertSchedulePatientRequestTemp";
                objErrorlog.UserID = null;
                result = InsertErrorLog(objErrorlog);
            }
            objList.CareGiverList = objCareGiverListing;
            return result;
        }


        public SchedulePatientRequestList GetAllSchedulePatientRequest(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterStatus, string FilterOffice, string OrganisationId)
        {
            SchedulePatientRequestList ListSchedulePatientRequest = new SchedulePatientRequestList();
            try
            {

                if (FromDate == "||" && ToDate == "||")
                {
                    FromDate = "1000-01-01";
                    ToDate = "1000-01-01";
                }

                

                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetALLSchedulePatientRequest",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNEW_GetALLSchedulePatientRequest",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        IsAdmin,
                                                        LogInUserId,
                                                         Convert.ToDateTime(FromDate),
                                                         Convert.ToDateTime(ToDate),
                                                        FilterStatus,
                                                        FilterOffice,
                                                        OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    List<SchedulePatientRequest> SchedulePatientRequestList = new List<SchedulePatientRequest>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        SchedulePatientRequest objSchedulePatientRequest = new SchedulePatientRequest();
                        objSchedulePatientRequest.PatientRequestId = Convert.ToInt32(ds.Tables[1].Rows[i]["PatientRequestId"]);
                        objSchedulePatientRequest.SchedulerName = ds.Tables[1].Rows[i]["SchedulerName"].ToString();
                        objSchedulePatientRequest.PatientName = ds.Tables[1].Rows[i]["PatientName"].ToString();
                        objSchedulePatientRequest.Address = ds.Tables[1].Rows[i]["Address"].ToString();
                        objSchedulePatientRequest.ZipCode = ds.Tables[1].Rows[i]["ZipCode"].ToString();
                        objSchedulePatientRequest.MedicalId = ds.Tables[1].Rows[i]["MedicalId"].ToString();
                        objSchedulePatientRequest.Description = ds.Tables[1].Rows[i]["Description"].ToString();
                        objSchedulePatientRequest.InsertDateTime = ds.Tables[1].Rows[i]["InsertDateTime"].ToString();

                            //DateTime.Parse(ds.Tables[1].Rows[i]["InsertDateTime"].ToString()).ToString();
                            //.ToString("MM/dd/yyyy");

                        objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[1].Rows[i]["Date"].ToString()).ToString("MM/dd/yyyy");
                        objSchedulePatientRequest.FromTime = ds.Tables[1].Rows[i]["FromTime"].ToString();
                        objSchedulePatientRequest.ToTime = ds.Tables[1].Rows[i]["ToTime"].ToString();
                        objSchedulePatientRequest.CaregiverName = ds.Tables[1].Rows[i]["CaregiverName"].ToString();
                        objSchedulePatientRequest.OfficeName = ds.Tables[1].Rows[i]["Office"].ToString();
                        objSchedulePatientRequest.TotalCaregiversNotified = Convert.ToInt32(ds.Tables[1].Rows[i]["TotalCaregiversNotified"].ToString());
                        objSchedulePatientRequest.DrivingStartTime = ds.Tables[1].Rows[i]["DrivingStartTime"].ToString();
                        objSchedulePatientRequest.CheckInTime = ds.Tables[1].Rows[i]["CheckInDateTime"].ToString();
                        objSchedulePatientRequest.CheckOutTime = ds.Tables[1].Rows[i]["CheckOutDateTime"].ToString();
                        objSchedulePatientRequest.Miles = ds.Tables[1].Rows[i]["DrivingTotalDistance"].ToString();
                        objSchedulePatientRequest.NurseId = Convert.ToInt32(ds.Tables[1].Rows[i]["NurseId"]);
                        var PatientSignaturePath = ConfigurationManager.AppSettings["CareGiverSignature"].ToString();
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["PatientSignature"].ToString()))
                        {
                            objSchedulePatientRequest.PatientSignature = PatientSignaturePath + ds.Tables[1].Rows[i]["PatientSignature"].ToString();
                        }

                        var Iscancel = ds.Tables[1].Rows[i]["IsCancelled"].ToString();
                        if (Iscancel == "False")
                        {
                            objSchedulePatientRequest.IsCancelled = false;
                        }
                        else
                        {
                            objSchedulePatientRequest.IsCancelled = true;
                        }

                        objSchedulePatientRequest.Status = ds.Tables[1].Rows[i]["Status"].ToString();
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezoneId"].ToString()))
                        {
                            objSchedulePatientRequest.TimezoneId = ds.Tables[1].Rows[i]["TimezoneId"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezoneOffset"].ToString()))
                        {
                            objSchedulePatientRequest.TimezoneOffset = Convert.ToInt32(ds.Tables[1].Rows[i]["TimezoneOffset"]);
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezonePostfix"].ToString()))
                        {
                            objSchedulePatientRequest.TimezonePostfix = ds.Tables[1].Rows[i]["TimezonePostfix"].ToString();
                        }

                        objSchedulePatientRequest.CheckInLatLong = ds.Tables[1].Rows[i]["CheckInLatLong"].ToString();
                        objSchedulePatientRequest.CheckOutLatLong = ds.Tables[1].Rows[i]["CheckOutLatLong"].ToString();

                        try
                        {
                            objSchedulePatientRequest.CheckInAddress = getAddressGoogleAPI(ds.Tables[1].Rows[i]["CheckInLatLong"].ToString());
                            objSchedulePatientRequest.CheckOutAddress = getAddressGoogleAPI(ds.Tables[1].Rows[i]["CheckOutLatLong"].ToString());
                        }
                        catch (Exception e)
                        { }

                        SchedulePatientRequestList.Add(objSchedulePatientRequest);
                    }

                    ListSchedulePatientRequest.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    ListSchedulePatientRequest.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListSchedulePatientRequest.SchedulePatientRequestsList = SchedulePatientRequestList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListSchedulePatientRequest;
        }

        public CareGiversList CancelPatientRequest(string patientid, string Userid)
        {
            string result = "";
            CareGiversList objList = new CareGiversList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "CancelSchedulePatientRequest", patientid, Guid.Parse(Userid));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objList.CareGiverList = new List<CareGivers>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGivers = new CareGivers();
                        objCareGivers.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        objCareGivers.DeviceToken = ds.Tables[0].Rows[i]["DeviceToken"].ToString();
                        objCareGivers.DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
                        objList.CareGiverList.Add(objCareGivers);
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message == "ALREADYCANCELLED")
                {
                    result = "The request has been already cancelled";
                }
                else
                {
                    ErrorLog objErrorlog = new ErrorLog();
                    objErrorlog.Errormessage = ex.Message;
                    objErrorlog.StackTrace = ex.StackTrace;
                    objErrorlog.Pagename = "CareGiverLiteService";
                    objErrorlog.Methodname = "CancelPatientRequest";
                    objErrorlog.UserID = Userid;
                    result = InsertErrorLog(objErrorlog);
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "CancelPatientRequest";
                objErrorlog.UserID = Userid;
                result = InsertErrorLog(objErrorlog);
            }
            return objList;
        }

        public CareGivers GetCareGiverDetailsByUserId(string UserId)
        {
            CareGivers CareGiver = new CareGivers();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCareGiversDetailsByUserId", new Guid(UserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CareGiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseId"]);
                    CareGiver.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    CareGiver.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    CareGiver.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    CareGiver.ServiceId = ds.Tables[0].Rows[0]["Service"].ToString();
                    CareGiver.ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString();
                    CareGiver.DistanceFromLocation = Convert.ToDecimal(ds.Tables[0].Rows[0]["DistanceFromLocation"].ToString());
                    CareGiver.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                    CareGiver.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    CareGiver.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    CareGiver.ProfileImage = CareGiver.ProfileImage = CareGiverProfileImagesPath + ds.Tables[0].Rows[0]["ProfileImage"].ToString();
                    CareGiver.Certificate = CareGiverProfileCertificate + ds.Tables[0].Rows[0]["Certificate"].ToString();
                    CareGiver.IsAvailable = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAvailable"].ToString());
                    CareGiver.IsBusy = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBusy"].ToString());
                    CareGiver.IsApprove = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsApprove"].ToString());
                    CareGiver.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    CareGiver.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    CareGiver.DistanceUnit = ds.Tables[0].Rows[0]["DistanceUnit"].ToString();
                    CareGiver.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    CareGiver.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    CareGiver.ProfileVideo = (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProfileVideo"].ToString())) ? CareGiverProfileVideoURL + ds.Tables[0].Rows[0]["ProfileVideo"].ToString() : "";
                    CareGiver.ProfileVideoThumbnil = (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProfileVideoThumbnil"].ToString())) ? CareGiverProfileImagesPath + ds.Tables[0].Rows[0]["ProfileVideoThumbnil"].ToString() : "";
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiversDetailsByUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return CareGiver;
        }


        public SchedulePatientRequest GetPatientRequestDetailByMedicalID(string LoginUserId, string MedicalID)
        {
            SchedulePatientRequest objSchedulePatientRequest = new SchedulePatientRequest();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientRequestDetailByMedicalID", LoginUserId, MedicalID);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objSchedulePatientRequest.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRequestId"]);
                    objSchedulePatientRequest.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objSchedulePatientRequest.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    objSchedulePatientRequest.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objSchedulePatientRequest.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objSchedulePatientRequest.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objSchedulePatientRequest.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objSchedulePatientRequest.MedicalId = ds.Tables[0].Rows[0]["MedicalId"].ToString();
                    objSchedulePatientRequest.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    //objSchedulePatientRequest.InsertDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy");
                    //  objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    objSchedulePatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    objSchedulePatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    // objSchedulePatientRequest.CaregiverName = ds.Tables[0].Rows[0]["CaregiverName"].ToString();
                    objSchedulePatientRequest.ServiceNames = ds.Tables[0].Rows[0]["ServiceNames"].ToString();

                    objSchedulePatientRequest.OfficeName = ds.Tables[0].Rows[0]["Office"].ToString();
                    objSchedulePatientRequest.Office = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());
                    objSchedulePatientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneId"].ToString();
                    objSchedulePatientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString());
                    objSchedulePatientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfix"].ToString();
                    // objSchedulePatientRequest.OfficeAddress = ds.Tables[0].Rows[0]["OfficeAddress"].ToString();

                    objSchedulePatientRequest.PayerId = (ds.Tables[0].Rows[0]["PayerId"].ToString());
                    objSchedulePatientRequest.PayerProgram = ds.Tables[0].Rows[0]["PayerProgram"].ToString();
                    objSchedulePatientRequest.ProcedureCode = (ds.Tables[0].Rows[0]["ProcedureCode"]).ToString();
                    objSchedulePatientRequest.JurisdictionCode = ds.Tables[0].Rows[0]["JurisdictionCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiversDetailsByUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return objSchedulePatientRequest;
        }


        public SchedulePatientRequest GetPatientRequestDetailByMedicalIDByOrganisation(string LoginUserId, string MedicalID, string OrganisationId)
        {
            SchedulePatientRequest objSchedulePatientRequest = new SchedulePatientRequest();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientRequestDetailByMedicalIDByOrganisation", LoginUserId, MedicalID, OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objSchedulePatientRequest.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRequestId"]);
                    objSchedulePatientRequest.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                    objSchedulePatientRequest.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    objSchedulePatientRequest.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objSchedulePatientRequest.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objSchedulePatientRequest.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objSchedulePatientRequest.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objSchedulePatientRequest.MedicalId = ds.Tables[0].Rows[0]["MedicalId"].ToString();
                    objSchedulePatientRequest.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    //objSchedulePatientRequest.InsertDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy");
                    //  objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    objSchedulePatientRequest.FromTime = ds.Tables[0].Rows[0]["FromTime"].ToString();
                    objSchedulePatientRequest.ToTime = ds.Tables[0].Rows[0]["ToTime"].ToString();
                    // objSchedulePatientRequest.CaregiverName = ds.Tables[0].Rows[0]["CaregiverName"].ToString();
                    objSchedulePatientRequest.ServiceNames = ds.Tables[0].Rows[0]["ServiceNames"].ToString();

                    objSchedulePatientRequest.OfficeName = ds.Tables[0].Rows[0]["Office"].ToString();
                    objSchedulePatientRequest.Office = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());
                    objSchedulePatientRequest.TimezoneId = ds.Tables[0].Rows[0]["TimezoneId"].ToString();
                    objSchedulePatientRequest.TimezoneOffset = Convert.ToInt16(ds.Tables[0].Rows[0]["TimezoneOffset"].ToString());
                    objSchedulePatientRequest.TimezonePostfix = ds.Tables[0].Rows[0]["TimezonePostfix"].ToString();
                    // objSchedulePatientRequest.OfficeAddress = ds.Tables[0].Rows[0]["OfficeAddress"].ToString();

                    objSchedulePatientRequest.PayerId = (ds.Tables[0].Rows[0]["PayerId"].ToString());
                    objSchedulePatientRequest.PayerProgram = ds.Tables[0].Rows[0]["PayerProgram"].ToString();
                    objSchedulePatientRequest.ProcedureCode = (ds.Tables[0].Rows[0]["ProcedureCode"]).ToString();
                    objSchedulePatientRequest.JurisdictionCode = ds.Tables[0].Rows[0]["JurisdictionCode"].ToString();

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCareGiversDetailsByUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return objSchedulePatientRequest;
        }


        public RewardPointsList GetAllRewardPointDetail(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string FilterOffice, string UserId)
        {
            RewardPointsList ListRewardPoint = new RewardPointsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllRewardPointDetails",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        FilterOffice,
                                                        Convert.ToString(new Guid(UserId)));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    List<RewardPoint> RewardPointList = new List<RewardPoint>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        RewardPoint objRewardPoint = new RewardPoint();
                        objRewardPoint.NurseId = Convert.ToInt32(ds.Tables[1].Rows[i]["NurseId"]);
                        objRewardPoint.Name = ds.Tables[1].Rows[i]["Name"].ToString();
                        objRewardPoint.CompletedReqCount = ds.Tables[1].Rows[i]["CompletedReqCount"].ToString();
                        objRewardPoint.TotalRewardPoint = ds.Tables[1].Rows[i]["TotalRewardPoint"].ToString();
                        objRewardPoint.Office = ds.Tables[1].Rows[i]["Office"].ToString();
                        //objRewardPoint.Photo = ds.Tables[0].Rows[i]["ProfileImage"].ToString();
                        var ProfileImage = ds.Tables[1].Rows[i]["ProfileImage"].ToString();
                        if (ProfileImage != null)
                        {
                            objRewardPoint.Photo = CareGiverProfileImagesPath + ProfileImage;
                        }
                        else
                        {
                            objRewardPoint.Photo = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                        }
                        //objRewardPoint.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();
                        RewardPointList.Add(objRewardPoint);
                    }

                    ListRewardPoint.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    ListRewardPoint.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListRewardPoint.objRewardPointList = RewardPointList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllRewardPointDetail";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListRewardPoint;
        }

        public RewardPointsList GetAllRewardPointAdvanceDetail(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string FilterOffice, string UserId)
        {

            RewardPointsList ListRewardPoint = new RewardPointsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllRewardPointDetailsAdv",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        FilterOffice,
                                                        Convert.ToString(new Guid(UserId)));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    List<RewardPoint> RewardPointList = new List<RewardPoint>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        RewardPoint objRewardPoint = new RewardPoint();
                        objRewardPoint.NurseId = Convert.ToInt32(ds.Tables[1].Rows[i]["NurseId"]);
                        objRewardPoint.Name = ds.Tables[1].Rows[i]["Name"].ToString();
                        objRewardPoint.CompletedReqCount = ds.Tables[1].Rows[i]["CompletedReqCount"].ToString();
                        objRewardPoint.TotalRewardPoint = ds.Tables[1].Rows[i]["TotalRewardPoint"].ToString();
                        objRewardPoint.Office = ds.Tables[1].Rows[i]["Office"].ToString();
                        //objRewardPoint.Photo = ds.Tables[0].Rows[i]["ProfileImage"].ToString();
                        var ProfileImage = ds.Tables[1].Rows[i]["ProfileImage"].ToString();
                        if (ProfileImage != null)
                        {
                            objRewardPoint.Photo = CareGiverProfileImagesPath + ProfileImage;
                        }
                        else
                        {
                            objRewardPoint.Photo = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                        }
                        objRewardPoint.Comment = ds.Tables[1].Rows[i]["Comment"].ToString();
                        RewardPointList.Add(objRewardPoint);
                    }

                    ListRewardPoint.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    ListRewardPoint.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListRewardPoint.objRewardPointList = RewardPointList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllRewardPointAdvanceDetail";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListRewardPoint;
        }

        public string EditComment(string NurseId, string Comment)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateRewardComment",
                                                    NurseId,
                                                    Comment
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
                objErrorlog.Methodname = "EditComment";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        //Reward Point View Profile Modal
        public RewardPointsList GetAllRewardPointByNurseId(string NurseId1)
        {
            RewardPointsList ListRewardPoint = new RewardPointsList();
            List<RewardPoint> RewardPointList = new List<RewardPoint>();
            int NurseId = Convert.ToInt32(NurseId1);
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllRewardPointByNurseId", NurseId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        var objRewardPoint = new RewardPoint();

                        objRewardPoint.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
                        objRewardPoint.Name = ds.Tables[0].Rows[i]["CareGiverName"].ToString();
                        objRewardPoint.SchedulerName = ds.Tables[0].Rows[i]["SchedulerName"].ToString();
                        objRewardPoint.AppointmentDate = ds.Tables[0].Rows[i]["AppointmentDate"].ToString().Substring(0, 10);

                        var time = ds.Tables[0].Rows[i]["AppointmentTime"].ToString();
                        DateTime date = DateTime.Parse(time, System.Globalization.CultureInfo.CurrentCulture);

                        objRewardPoint.Time = date.ToString("hh:mm:ss tt");
                        objRewardPoint.Point = ds.Tables[0].Rows[i]["Points"].ToString();
                        RewardPointList.Add(objRewardPoint);
                    }

                    ListRewardPoint.objRewardPointList = RewardPointList;
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    var objRewardPoint = new RewardPoint();
                    objRewardPoint.NurseId = Convert.ToInt32(ds.Tables[1].Rows[0]["NurseId"]);
                    objRewardPoint.Name = ds.Tables[1].Rows[0]["Name"].ToString();
                    objRewardPoint.Email = ds.Tables[1].Rows[0]["Email"].ToString();
                    ListRewardPoint.objRewardPoint = objRewardPoint;

                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllRewardPointByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListRewardPoint;
        }

        // Get Rating From Patient Request Id
        public string GetRateByPatientRequestId(string PatientRequestId)
        {
            //List<CareGiverTrackLocation> CareGiverTrackLocationList = new List<CareGiverTrackLocation>();
            //try
            //{
            //    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetTrackLocationByPatientRequestId",
            //                                           PatientRequestId);

            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {

            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            CareGiverTrackLocation objCareGiverTrackLocation = new CareGiverTrackLocation();
            //            objCareGiverTrackLocation.PatientRequestId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientRequestId"]);
            //            objCareGiverTrackLocation.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"]);
            //            //  DateTime.Parse(Convert.ToString(item["Date"])).ToString("MM/dd/yyyy");
            //            objCareGiverTrackLocation.LocationDateTime = DateTime.Parse(ds.Tables[0].Rows[i]["LocationDateTime"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
            //            objCareGiverTrackLocation.LocationLatitude = ds.Tables[0].Rows[i]["LocationLatitude"].ToString();
            //            objCareGiverTrackLocation.LocationLongitude = ds.Tables[0].Rows[i]["LocationLongitude"].ToString();
            //            objCareGiverTrackLocation.Status = ds.Tables[0].Rows[i]["Status"].ToString();
            //            objCareGiverTrackLocation.TotalMiles = ds.Tables[0].Rows[i]["TotalMiles"].ToString();
            //            CareGiverTrackLocationList.Add(objCareGiverTrackLocation);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorLog objErrorlog = new ErrorLog();
            //    objErrorlog.Errormessage = ex.Message;
            //    objErrorlog.StackTrace = ex.StackTrace;
            //    objErrorlog.Pagename = "CareGiverLiteService";
            //    objErrorlog.Methodname = "GetTrackLocationByPatientRequestId";
            //    string result = InsertErrorLog(objErrorlog);
            //}
            //return CareGiverTrackLocationList;
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetRatingByPatientRequestIdSchedulerId",
                                                    PatientRequestId
                                                    );
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "")
                    {
                        result = "0";
                    }
                    else
                    {
                        result = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
                else { result = "0"; }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetRateByPatientRequestId";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        // Set Rating From Patient Request Id
        public string SetRateByPatientRequestId(string PatientRequestId, string Rating)
        {

            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "InsertUpdateRating",
                                                    PatientRequestId,
                                                    Rating);
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
                objErrorlog.Methodname = "SetRateByPatientRequestId";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        
        public RatingsList GetAllRatingByNurseId(string NurseId1)
        {
            RatingsList ListRating = new RatingsList();
            List<Rating> RatingList = new List<Rating>();
            int NurseId = Convert.ToInt32(NurseId1);
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllRatingByNurseId", NurseId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        var objRewardPoint = new Rating();


                        objRewardPoint.Rate = ds.Tables[0].Rows[i]["Rating"].ToString();
                        objRewardPoint.SchedulerName = ds.Tables[0].Rows[i]["SchedulerName"].ToString();
                        objRewardPoint.AppointmentDate = ds.Tables[0].Rows[i]["InsertDate"].ToString().Substring(0, 10);

                        var time = ds.Tables[0].Rows[i]["InsertTime"].ToString();
                        DateTime date = DateTime.Parse(time, System.Globalization.CultureInfo.CurrentCulture);

                        objRewardPoint.Time = date.ToString("hh:mm:ss tt");
                        //objRewardPoint.Time = ds.Tables[0].Rows[i]["InsertTime"].ToString();
                        objRewardPoint.Point = ds.Tables[0].Rows[i]["Point"].ToString();
                        RatingList.Add(objRewardPoint);
                    }

                    ListRating.objRatingsList = RatingList;
                }
                else
                {
                    var objRewardPoint = new Rating();


                    objRewardPoint.Rate = "-";
                    objRewardPoint.SchedulerName = "-";
                    objRewardPoint.AppointmentDate = "-";
                    objRewardPoint.Time = "-";
                    objRewardPoint.Point = "-";
                    RatingList.Add(objRewardPoint);
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllRatingByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListRating;
        }

        public ChattingsList GetAllChattingList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string OfficeId)
        {

            ChattingsList ListChatting = new ChattingsList();
            try
            {

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllOneToOneChatList_web",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        Convert.ToInt32(OfficeId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Chatting> ChattingList = new List<Chatting>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Chatting objChatting = new Chatting();
                        objChatting.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objChatting.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        objChatting.CareGiverName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objChatting.Role = ds.Tables[0].Rows[i]["Role"].ToString();

                        objChatting.QuickBloxDialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                        var ProfileImage = ds.Tables[0].Rows[i]["ProfileImage"].ToString();

                        if (ProfileImage != null)
                        {
                            objChatting.CaregiverProfileImage = CareGiverProfileImagesPath + ProfileImage;
                        }
                        else
                        {
                            objChatting.CaregiverProfileImage = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                        }
                        //
                        objChatting.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objChatting.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        
                        //objChatting.CaregiverProfileImage = ds.Tables[0].Rows[i]["ProfileImage"].ToString();
                        ChattingList.Add(objChatting);
                    }

                    ListChatting.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListChatting.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListChatting.objChattingsList = ChattingList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllChattingList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListChatting;
        }

        public ChattingsList GetChattingListPatientGroupWise(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string GroupTypeId)
        {

            ChattingsList ListChatting = new ChattingsList();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllCaregiverForChattingGroupWise",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetGroupChatList",
                                                     LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        Convert.ToInt32(GroupTypeId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Chatting> ChattingList = new List<Chatting>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Chatting objChatting = new Chatting();
                        objChatting.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"]);
                        objChatting.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                        objChatting.DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                        objChatting.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"]);
                        objChatting.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objChatting.GroupAdminUserId = ds.Tables[0].Rows[i]["GroupAdminUserId"].ToString();
                        objChatting.IsGroupAdmin = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsGroupAdmin"]);
                        objChatting.IsOfficeGroup = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsOfficeGroup"]);
                        objChatting.GroupTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["MasterChattingGroupTypeID"]);
                        objChatting.GroupSubject = Convert.ToString(ds.Tables[0].Rows[i]["GroupSubject"]);

                        //if (ds.Tables[0].Rows[i]["AssignedNurseCoordinatorId"].ToString() != "")
                        //{
                        //    objChatting.NurseCoordinatorId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignedNurseCoordinatorId"].ToString());
                        //}
                        //else
                        //{
                        //    objChatting.NurseCoordinatorId = 0;
                        //}

                        //if (ds.Tables[0].Rows[i]["NurseCoordinatorPermission"].ToString() != "")
                        //{
                        //    objChatting.NurseCoordinatorPermission = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseCoordinatorPermission"].ToString());
                        //}
                        //else
                        //{
                        //    objChatting.NurseCoordinatorPermission = 0;
                        //}

                        //var ProfileImage = ds.Tables[0].Rows[i]["ProfileImage"].ToString();

                        //if (ProfileImage != null)
                        //{
                        //    objChatting.CaregiverProfileImage = CareGiverProfileImagesPath + ProfileImage;
                        //}
                        //else
                        //{
                        //    objChatting.CaregiverProfileImage = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                        //}
                        //objChatting.CaregiverProfileImage = ds.Tables[0].Rows[i]["ProfileImage"].ToString();
                        ChattingList.Add(objChatting);
                    }

                    ListChatting.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListChatting.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListChatting.objChattingsList = ChattingList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetChattingListPatientGroupWise";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListChatting;
        }

        public Chatting GetQuickBloxIdByNurseId(string nurseId1)
        {
            //Auth
            string QBID = "";
            int NurseId = Convert.ToInt32(nurseId1);
            Chatting objChattingCaregiver = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetQuickBloxIdByNurseId", NurseId);
               
                objChattingCaregiver.QuickBloxId = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();
                objChattingCaregiver.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                objChattingCaregiver.UserId = ds.Tables[0].Rows[0]["UserID"].ToString();
                objChattingCaregiver.ToEmail = ds.Tables[0].Rows[0]["Email"].ToString();
                objChattingCaregiver.CareGiverName = ds.Tables[0].Rows[0]["CareGiverName"].ToString();
                
                //QBID = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();          
                //QBID += "," + ds.Tables[0].Rows[0]["UserName"].ToString();     
                //QBID += "," + ds.Tables[0].Rows[0]["UserID"].ToString();         
                //QBID += "," + ds.Tables[0].Rows[0]["Email"].ToString();          
                //QBID += "," + ds.Tables[0].Rows[0]["CareGiverName"].ToString();  
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetQuickBloxIdByNurseId";
                string result = InsertErrorLog(objErrorlog);
            }

            return objChattingCaregiver;
        }

        public string GetQuickBloxIdBySchedulerUserId(string SchedulerUserId)
        {
            string QBID = "";
            //int SchedulerId = Convert.ToInt32(SchedulerUserId1);
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetQuickBloxIdBySchedulerUserId", SchedulerUserId);
                QBID = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetQuickBloxIdBySchedulerUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return QBID;
        }

        //Update QUickBLoxId In Table
        public string SaveQBId(string UserID, string QuickbloxId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateQuickbloxId",
                                                    UserID,
                                                    QuickbloxId
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
                objErrorlog.Methodname = "SaveQBId";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string SaveDialogId(string NurseUserId, string SchedulerUserId, string DialogId, string UserType)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "SaveDialogId_v1",
                                                    NurseUserId,
                                                    SchedulerUserId,
                                                    DialogId,
                                                    UserType
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
                objErrorlog.Methodname = "SaveDialogId";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string GetDialogId(string NurseUserId, string SchedulerUserId)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetDialogId",
                                                    NurseUserId,
                                                    SchedulerUserId
                                                    );
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else { result = ds.Tables[0].Rows[0]["DialogId"].ToString(); }


            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetDialogId";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        public Chatting GetDialogDetail(string Id)
        {
            var objDialogDetail = new Chatting();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetDialogDetail",
                                                  Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //    result = "0";
                    //}
                    //else
                    //{

                    objDialogDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChattingGroupId"]);
                    objDialogDetail.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    objDialogDetail.GroupSubject = ds.Tables[0].Rows[0]["GroupSubject"].ToString();
                    objDialogDetail.DialogId = ds.Tables[0].Rows[0]["DialogId"].ToString();
                    objDialogDetail.SchedulerEmailId = ds.Tables[0].Rows[0]["SchedulerEmailId"].ToString();
                    objDialogDetail.SchedulerUserId = ds.Tables[0].Rows[0]["SchedulerUserId"].ToString();

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetDialogDetail";
                result = InsertErrorLog(objErrorlog);
            }
            return objDialogDetail;
        }



        public List<ChattingGroupMember> GetGroupMemberDetail(string Id, string OrganisationId)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            string result = "";
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetChattingGroupMemberByChattingGroupId",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetChattingGroupMemberByChattingGroupId",
                                                  Id,
                                                  Convert.ToInt32(OrganisationId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupMemberDetail = new ChattingGroupMember();
                        objGroupMemberDetail.ChattingGroupMemberId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupMemberId"]);
                        objGroupMemberDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"]);
                        objGroupMemberDetail.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objGroupMemberDetail.Type = ds.Tables[0].Rows[i]["Type"].ToString();
                        objGroupMemberDetail.QuickBloxId = ds.Tables[0].Rows[i]["QBID"].ToString();
                        objGroupMemberDetail.Email = ds.Tables[0].Rows[i]["EmailId"].ToString();
                        objGroupMemberDetail.MemberName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objGroupMemberDetail.MemberId = ds.Tables[0].Rows[i]["MemberId"].ToString();
                        objGroupMemberDetail.IsGroupAdmin = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsGroupAdmin"]);
                        objGroupMemberDetailList.Add(objGroupMemberDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetGroupMemberDetail";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupMemberDetailList;
        }

        public Scheduler GetSchedulerDetailByUserId(string SchedulerUserId)
        {
            Scheduler Scheduler = new Scheduler();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetSchedulerDetailByUserId", SchedulerUserId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetSchedulerDetailByUserId", SchedulerUserId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Scheduler.SchedulerId = Convert.ToInt32(ds.Tables[0].Rows[0]["SchedulerId"]);
                    Scheduler.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    Scheduler.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    Scheduler.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    Scheduler.OfficeIds = ds.Tables[0].Rows[0]["OfficeIds"].ToString();
                    Scheduler.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    Scheduler.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"]);

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetSchedulerDetailByUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return Scheduler;
        }

        public ChattingsList GetAllDialogId(string SchedulerUserId)
        {
            string result = "";
            var objChattingList = new ChattingsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetDialogIdsFromSchedulerUserId",
                                                    Guid.Parse(SchedulerUserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Chatting> ChattingList = new List<Chatting>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Chatting objChatting = new Chatting();
                        objChatting.QuickBloxDialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                        //   objChatting.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        objChatting.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        ChattingList.Add(objChatting);
                    }
                    objChattingList.objChattingsList = ChattingList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllDialogId";
                result = InsertErrorLog(objErrorlog);
            }
            return objChattingList;
        }

        public ChattingsList GetAllGroupDialogId(string SchedulerUserId)
        {
            string result = "";
            var objChattingList = new ChattingsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetGroupDialogIdsFromSchedulerUserId",
                                                    Guid.Parse(SchedulerUserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Chatting> ChattingList = new List<Chatting>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Chatting objChatting = new Chatting();
                        objChatting.DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                        objChatting.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                        objChatting.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"].ToString());
                        objChatting.GroupTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["MasterChattingGroupTypeID"].ToString());
                        ChattingList.Add(objChatting);
                    }
                    objChattingList.objChattingsList = ChattingList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllGroupDialogId";
                result = InsertErrorLog(objErrorlog);
            }
            return objChattingList;
        }


        public string RemovePatientRequest(string PatientRequestId, string UserID)
        {
            string result = "";
            try
            {
                Guid UserGUID = Guid.Parse(UserID);
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "DeletePatientRequestById",
                                                         Convert.ToInt32(PatientRequestId), UserGUID);

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
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "RemovePatientRequest";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public bool checkDialogId(string GroupName)
        {
            try
            {
                //string result = "";

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "checkDialogId", GroupName);

                if (ds.Tables[0].Rows[0][0].ToString() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "checkDialogId";
                string result = InsertErrorLog(objErrorlog);
            }
            return false;
        }


        //public Chatting AddGroupDialogId(string DialogId, string GroupName, string UserId, string OfficeId)

        public Chatting AddGroupDialogId(GroupChat GroupChat)
        {
            string result = "";
            var objDialogDetail = new Chatting();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddGroupDialogId",
                                                    GroupChat.GroupDialogId,
                                                    GroupChat.GroupName,
                                                    GroupChat.GroupSubject,
                                                    Guid.Parse(GroupChat.LogInUserId),
                                                    GroupChat.GroupTypeID,
                                                    GroupChat.OfficeId                                                    
                                                    );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objDialogDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChattingGroupId"]);
                    objDialogDetail.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    objDialogDetail.DialogId = ds.Tables[0].Rows[0]["DialogId"].ToString();

                    objDialogDetail.Result = "Success";
                }
                //    if (i > 0)
                //{
                //    result = "Success";
                //}
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddGroupDialogId";
                result = InsertErrorLog(objErrorlog);
            }
            return objDialogDetail;
        }


        public Chatting AddOrganisationGroupDialogId(GroupChat GroupChat)
        {
            string result = "";
            var objDialogDetail = new Chatting();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddOrganisationGroupDialogId",
                                                    GroupChat.GroupDialogId,
                                                    GroupChat.GroupName,
                                                    GroupChat.GroupSubject,
                                                    Convert.ToString(GroupChat.LogInUserId),
                                                    GroupChat.GroupTypeID,
                                                    GroupChat.OfficeId,
                                                    GroupChat.OrganisationId
                                                    );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objDialogDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChattingGroupId"]);
                    objDialogDetail.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    objDialogDetail.DialogId = ds.Tables[0].Rows[0]["DialogId"].ToString();

                    objDialogDetail.Result = "Success";
                }
                //    if (i > 0)
                //{
                //    result = "Success";
                //}
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddOrganisationGroupDialogId";
                result = InsertErrorLog(objErrorlog);
            }
            return objDialogDetail;
        }



        public List<CareGivers> NotifyUserForChatMessage(CareGivers CareGiver)
        {
            string result = "";
            List<CareGivers> resultList = new List<CareGivers>();
            try
            {
                resultList = GetNotifiableUser(CareGiver.UserId, CareGiver.DialogId);
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverSuperAdminService";
                objErrorlog.Methodname = "NotifyUserForChatMessage";
                objErrorlog.UserID = CareGiver.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return resultList;
        }

        //*****************************************Fetch All GetNotifiableUser
        private List<CareGivers> GetNotifiableUser(string UserId, string DialogId)
        {
            List<CareGivers> objList = new List<CareGivers>();
            List<CareGivers> objCareGiverListing = new List<CareGivers>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetNotifiableUser_v1",
                                                         Guid.Parse(UserId), DialogId); 

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGivers = new CareGivers();
                        objCareGivers.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        objCareGivers.DeviceToken = ds.Tables[0].Rows[i]["DeviceToken"].ToString();
                        objCareGivers.DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
                        //CareGiverName
                        objCareGivers.BadgeCount = GetBadgeCountWithIncrement(ds.Tables[0].Rows[i]["UserId"].ToString());
                        objCareGivers.Name = ds.Tables[0].Rows[i]["CareGiverName"].ToString();
                        objCareGivers.Permission = Convert.ToString(ds.Tables[0].Rows[i]["Permission"] ?? "");

                        objCareGivers.IsOfficeGroup = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsOfficeGroup"]);
                        objCareGivers.IsGroupAdmin = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsGroupAdmin"]);
                        objCareGivers.GroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"].ToString());
                        objCareGivers.GroupSubject = Convert.ToString(ds.Tables[0].Rows[i]["GroupSubject"].ToString());
                        objCareGiverListing.Add(objCareGivers);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetNotifiableUser";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            objList = objCareGiverListing;
            return objList;
        }


        #region Get Badge Count With Increment
        public int GetBadgeCountWithIncrement(string UserId)
        {
            int Count = 0;
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetBadgeCountWithIncrement",
                                                    new Guid(UserId));
                if (ds != null)
                {
                    Count = Convert.ToInt32(ds.Tables[0].Rows[0]["BadgeCount"].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetIncrementedBadgeCount";
                objErrorlog.UserID = UserId;
                string result = InsertErrorLog(objErrorlog);
            }
            return Count;
        }

        #endregion

        //********************Get Address From GoogleAPI
        public string getAddressGoogleAPI(string LatLong)
        {
            string Address = "";//= new Location();
            using (var w = new WebClient())
            {
                //var url = "http://maps.google.com/maps/api/geocode/json?latlng=" + LatLong + "&sensor=false";
                var url = "https://maps.google.com/maps/api/geocode/json?latlng=" + LatLong + "&key=AIzaSyDCQgMe2fLgmaF9GDvxF61wsIXnktEBKhg";
                var json_data = string.Empty;
                json_data = w.DownloadString(url);
                JObject obj = JsonConvert.DeserializeObject<JObject>(json_data);
                Address = obj["results"][0]["formatted_address"].ToString();
            }
            return Address;
        }


        //********NurseCoordinator*******//
        public NurseCoordinatorsList GetAllNurseCoordinators(string loginUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string officeId, string search, string OrganisationId, string IsActiveStatus)
        {
            NurseCoordinatorsList ListNurseCoordinator = new NurseCoordinatorsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllNurseCoordinators_Testing",
                                                        loginUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        officeId,
                                                        search,
                                                        OrganisationId,
                                                        IsActiveStatus);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<NurseCoordinator> NurseCoordinatorList = new List<NurseCoordinator>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        NurseCoordinator objNurseCoordinator = new NurseCoordinator();
                        objNurseCoordinator.NurseCoordinatorId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseCoordinatorId"]);
                        objNurseCoordinator.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objNurseCoordinator.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        objNurseCoordinator.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        objNurseCoordinator.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objNurseCoordinator.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objNurseCoordinator.JobTitle = ds.Tables[0].Rows[i]["JobTitle"].ToString();
                        objNurseCoordinator.IsActive = Convert.ToString(ds.Tables[0].Rows[i]["IsActive"].ToString());

                        objNurseCoordinator.IsAllowForPatientChatRoom = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAllowForPatientChatRoom"]);
                        objNurseCoordinator.IsAllowOneToOneChat = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAllowOneToOneChat"]);
                        objNurseCoordinator.IsAllowGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAllowGroupChat"]);
                        objNurseCoordinator.IsAllowToCreateGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsAllowToCreateGroupChat"]);

                        int officeId1 = 0;
                        Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["OfficeId"]), out officeId1);
                        objNurseCoordinator.OfficeId = officeId1;

                        objNurseCoordinator.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        NurseCoordinatorList.Add(objNurseCoordinator);
                    }

                    ListNurseCoordinator.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListNurseCoordinator.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListNurseCoordinator.NurseCoordinatorList = NurseCoordinatorList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllNurseCoordinators";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListNurseCoordinator;
        }

        public string AddNurseCoordinator(NurseCoordinator NurseCoordinator)
        {
            string result = "";
            try
            {
               
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AddNurseCoordinator_v1",

                     int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_AddNurseCoordinator_v1",
                                                    NurseCoordinator.NurseCoordinatorId,
                                                    new Guid(NurseCoordinator.UserId),
                                                    NurseCoordinator.FirstName,
                                                    NurseCoordinator.LastName,
                                                    NurseCoordinator.UserName,
                                                    NurseCoordinator.Email,
                                                    NurseCoordinator.Password,
                                                    NurseCoordinator.IsAllowForPatientChatRoom,
                                                    NurseCoordinator.OfficeId,
                                                    NurseCoordinator.JobTitle,
                                                    new Guid(NurseCoordinator.InsertUserId),
                                                    NurseCoordinator.IsAllowOneToOneChat,
                                                    NurseCoordinator.IsAllowGroupChat,
                                                    NurseCoordinator.IsAllowToCreateGroupChat,
                                                    NurseCoordinator.OrganisationId
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
                objErrorlog.Methodname = "AddNurseCoordinator";
                objErrorlog.UserID = NurseCoordinator.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string DeleteNurseCoordinator(string NurseCoordinatorId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteNurseCoordinator",
                                                    NurseCoordinatorId,
                                                    new Guid(UserId));

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
                objErrorlog.Methodname = "DeleteNurseCoordinator";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string EditNurseCoordinator(NurseCoordinator NurseCoordinator)
        {
            string result = "";
            try
            {
                
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "EditNurseCoordinator_v1",
                    int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_EditNurseCoordinator_v1",
                                                    NurseCoordinator.NurseCoordinatorId,
                                                    NurseCoordinator.FirstName,
                                                    NurseCoordinator.LastName,
                                                    NurseCoordinator.Email,
                                                    //NurseCoordinator.Password,
                                                    NurseCoordinator.IsAllowForPatientChatRoom,
                                                    NurseCoordinator.OfficeId,
                                                    NurseCoordinator.JobTitle,
                                                    new Guid(NurseCoordinator.InsertUserId),
                                                    NurseCoordinator.IsAllowOneToOneChat,
                                                    NurseCoordinator.IsAllowGroupChat,
                                                    NurseCoordinator.IsAllowToCreateGroupChat
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
                objErrorlog.Methodname = "EditNurseCoordinator";
                objErrorlog.UserID = NurseCoordinator.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public NurseCoordinator GetNurseCoordinatorDetailById(string NurseCoordinatorId)
        {
            NurseCoordinator NurseCo = new NurseCoordinator();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetNurseCoordinatorDetailById", NurseCoordinatorId, new Guid());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    NurseCo.NurseCoordinatorId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseCoordinatorId"]);
                    NurseCo.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    NurseCo.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    NurseCo.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    NurseCo.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    NurseCo.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    NurseCo.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    NurseCo.QBID = ds.Tables[0].Rows[0]["QuickbloxId"].ToString();
                    NurseCo.JobTitle = ds.Tables[0].Rows[0]["JobTitle"].ToString();

                    NurseCo.IsActive = Convert.ToString(ds.Tables[0].Rows[0]["IsActive"].ToString());

                    NurseCo.IsAllowForPatientChatRoom = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowForPatientChatRoom"]);

                    NurseCo.IsAllowOneToOneChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowOneToOneChat"]);
                    NurseCo.IsAllowGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowGroupChat"]);

                    NurseCo.IsAllowToCreateGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowToCreateGroupChat"]);

                    int officeId1 = 0;
                    Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["OfficeId"]), out officeId1);
                    NurseCo.OfficeId = officeId1;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetNurseCoordinatorDetailById";
                string result = InsertErrorLog(objErrorlog);
            }
            return NurseCo;
        }



        public List<NurseCoordinator> GetUnAssignedNurseCoordinatorList(string ChattingGroupId)
        {
            List<NurseCoordinator> ListNurseCoordinator = new List<NurseCoordinator>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetUnAssignedNurseCoordinatorList", Convert.ToInt32(ChattingGroupId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<NurseCoordinator> NurseCoordinatorList = new List<NurseCoordinator>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        NurseCoordinator objNurseCoordinator = new NurseCoordinator();
                        objNurseCoordinator.NurseCoordinatorId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseCoordinatorId"]);
                        objNurseCoordinator.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objNurseCoordinator.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        objNurseCoordinator.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        objNurseCoordinator.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objNurseCoordinator.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objNurseCoordinator.QBID = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        NurseCoordinatorList.Add(objNurseCoordinator);
                    }

                    ListNurseCoordinator = NurseCoordinatorList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetUnAssignedNurseCoordinatorList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListNurseCoordinator;
        }

        public string SetNurseCoordinator(string ChattingGroupId, string NurseCoordinatorId, string Permission)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "SetNurseCoordinator",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    Convert.ToInt32(NurseCoordinatorId),
                                                    Convert.ToInt32(Permission)
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
                objErrorlog.Methodname = "SetNurseCoordinator";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string SetNurseCoordinatorAndOfficeManager(string ChattingGroupId, string NurseCoordinatorId, string Permission,string CaregiverQBId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "SetNurseCoordinatorAndOfficeManager",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    Convert.ToInt32(NurseCoordinatorId),
                                                    Convert.ToInt32(Permission),
                                                    CaregiverQBId
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
                objErrorlog.Methodname = "SetNurseCoordinator";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string SaveQBIdOfNurseCoordinator(string Email, string QuickbloxId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "SaveQBIdOfNurseCoordinator",
                                                    Email,
                                                    QuickbloxId
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
                objErrorlog.Methodname = "SaveQBIdOfNurseCoordinator";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public NurseCoordinator GetNurseCoordinatorDetailByUserId(string NurseCoordinatorUserId)
        {
            NurseCoordinator NurseCoordinator = new NurseCoordinator();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetNurseCoordinatorDetailByUserId", NurseCoordinatorUserId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    NurseCoordinator.NurseCoordinatorId = Convert.ToInt32(ds.Tables[0].Rows[0]["NurseCoordinatorId"]);
                    NurseCoordinator.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    NurseCoordinator.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    NurseCoordinator.OrganisationId= Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"].ToString());
                    NurseCoordinator.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    NurseCoordinator.IsAllowForPatientChatRoom = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowForPatientChatRoom"].ToString());
                    NurseCoordinator.IsOfficePermission = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsOfficePermission"]);

                    NurseCoordinator.IsAllowOneToOneChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowOneToOneChat"]);
                    NurseCoordinator.IsAllowGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowGroupChat"]);
                    NurseCoordinator.IsAllowToCreateGroupChat = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowToCreateGroupChat"]);

                    NurseCoordinator.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"]);
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetNurseCoordinatorDetailByUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return NurseCoordinator;
        }

        public string GetNurseCoordinatorPermissionGroupWise(string ChattingGroupId, string LoginUserUserId)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetNurseCoordinatorPermissionGroupWise",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    Guid.Parse(LoginUserUserId)
                                                    );
                result = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "SaveQBIdOfNurseCoordinator";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public List<ChattingGroupMember> GetNurseCoordinatorPermissionGroupWiseList(string ChattingGroupId)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetNurseCoordinatorPermissionGroupWiseList",
                                                  Convert.ToInt32(ChattingGroupId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupMemberDetail = new ChattingGroupMember();

                        if (ds.Tables[0].Rows[i]["Permission"].ToString() == "1") { objGroupMemberDetail.Type = "Read"; }
                        if (ds.Tables[0].Rows[i]["Permission"].ToString() == "2") { objGroupMemberDetail.Type = "Read/Write"; }
                        if (ds.Tables[0].Rows[i]["Permission"].ToString() == "3") { objGroupMemberDetail.Type = "Read/Write/Attachment"; }
                        objGroupMemberDetail.MemberName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objGroupMemberDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objGroupMemberDetail.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objGroupMemberDetail.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        objGroupMemberDetailList.Add(objGroupMemberDetail);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetNurseCoordinatorPermissionGroupWiseList";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupMemberDetailList;
        }



        //public async Task<string> GetLatlongDistanceData(string origin, string destination)
        //{

        //    double xyz = 0;
        //    string distance = "";
        //    //double jkf =Convert.ToDouble(origin);

        //    //double abc = Convert.ToDouble(destination);

        //    var clientGetDialogId = new System.Net.Http.HttpClient();

        //  //  var requestUrl = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&key={2}&origins={0}&destinations={1}", origin, destination, "AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");

        //   clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins="+origin+"&destinations="+destination+"&key=AIzaSyDVDf4FSn4yOdhCzo9EZyAks5cG73oq5cc");
        //   // clientGetDialogId.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/directions/json?origin=" + origin +"&destinations="+ destination +"&key=AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw");

        //   // clientGetDialogId.BaseAddress = new Uri(requestUrl);
        //    clientGetDialogId.DefaultRequestHeaders.Accept.Clear();
        //    var response1 = await clientGetDialogId.GetAsync("");
        //    var result1 = response1.Content.ReadAsStringAsync().Result;
        //    var data = (JObject)JsonConvert.DeserializeObject(result1);

        //    foreach (var row in data["rows"])
        //    {

        //        foreach (var elements in row["elements"])
        //        {
        //            foreach (var dist in elements["distance"])
        //            {

        //                distance = (string)dist;
        //                break;

        //            }

        //        }
        //    }


        //    return distance;



        //}


        //***************//

        //16-08-2017


        public CareGiversList GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers(SchedulePatientRequest SchedulePatientRequest)/*From InsertSchedulePatientRequest*/
        {

           

            var origin = "";
            var destination = "";

            CareGiversList objList = new CareGiversList();
            List<CareGivers> objCareGiverListing = new List<CareGivers>();
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(SchedulePatientRequest.ServiceNames) && SchedulePatientRequest.ServiceNames.EndsWith(","))
                {
                    SchedulePatientRequest.ServiceNames = SchedulePatientRequest.ServiceNames.TrimEnd(SchedulePatientRequest.ServiceNames[SchedulePatientRequest.ServiceNames.Length - 1]);
                }
                
                 //  DataSet ds = DataAccess.ExecuteDataset(Settings.CaregiverLiteDatabase().ToString(), "GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers_V1_test",

                 DataSet ds = DataAccess.ExecuteDataset(Settings.CaregiverLiteDatabase().ToString(), "ORG_GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers_V1_test",
                                                        SchedulePatientRequest.PatientName,
                                                        SchedulePatientRequest.Address,
                                                        SchedulePatientRequest.Latitude,
                                                        SchedulePatientRequest.Longitude,
                                                        SchedulePatientRequest.ZipCode,
                                                        SchedulePatientRequest.MedicalId,
                                                        SchedulePatientRequest.Description,
                                                        SchedulePatientRequest.Date,                                      
                                                        SchedulePatientRequest.FromTime,
                                                        SchedulePatientRequest.ToTime,
                                                        SchedulePatientRequest.IsCancelled,
                                                        SchedulePatientRequest.ServiceNames,
                                                        SchedulePatientRequest.TimezoneId,
                                                        SchedulePatientRequest.TimezoneOffset,
                                                        SchedulePatientRequest.TimezonePostfix,
                                                        //Guid.Parse(Membership.GetUser().ProviderUserKey.ToString()),
                                                        SchedulePatientRequest.MaxDistance,
                                                        SchedulePatientRequest.MaxCaregiver,
                                                        Convert.ToInt32(SchedulePatientRequest.Office),
                                                        SchedulePatientRequest.OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCareGivers = new CareGivers();
                        objCareGivers.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        objCareGivers.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objCareGivers.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objCareGivers.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                        objCareGivers.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objCareGivers.Address = ds.Tables[0].Rows[i]["address"].ToString();
                        objCareGivers.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objCareGivers.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objCareGivers.State = ds.Tables[0].Rows[i]["State"].ToString();
                        objCareGivers.DeviceToken = ds.Tables[0].Rows[i]["DeviceToken"].ToString();
                        objCareGivers.DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
                        objCareGivers.Latitude = ds.Tables[0].Rows[i]["latitude"].ToString();
                        objCareGivers.Longitude = ds.Tables[0].Rows[i]["longitude"].ToString();
                        objCareGivers.CurrentLatitude = ds.Tables[0].Rows[i]["CurrentLatitude"].ToString();
                        objCareGivers.CurrentLongitude = ds.Tables[0].Rows[i]["CurrentLongitude"].ToString();
                        objCareGivers.DistanceUnit = ds.Tables[0].Rows[i]["Distance"].ToString().Substring(0, 5);
                        objCareGivers.IsNurseBusy = ds.Tables[0].Rows[i]["NurseBusy"].ToString();
                        objCareGivers.Office = ds.Tables[0].Rows[i]["Office"].ToString();

                        //origin = (SchedulePatientRequest.Latitude +','+
                        // SchedulePatientRequest.Longitude);
                        //destination = ds.Tables[0].Rows[i]["latitude"].ToString() + ',' + ds.Tables[0].Rows[i]["longitude"].ToString();
                        // objCareGivers.DistanceUnit = GetLatlongDistanceData((SchedulePatientRequest.Address+','+SchedulePatientRequest.ZipCode), ds.Tables[0].Rows[i]["address"].ToString()).Result; 
                        //objCareGivers.DistanceUnit = GetLatlongDistanceData(origin,destination).Result;

                        objCareGiverListing.Add(objCareGivers);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCaregiverBasedOnInputedMilesAndNumberOfCaregivers";
                objErrorlog.UserID = SchedulePatientRequest.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            objList.CareGiverList = objCareGiverListing;
            return objList;
        }


        //16-08-2017
        #region Patients Details

        /*30-08-2017 start*/
        public string AddPatient(PatientsDetail PatientsDetail)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddPatient",
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
                                                    PatientsDetail.JurisdictionCode
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
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public PatientDetailsList GetPatientDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OfficeId, string search, string OrganisationId,string IsActiveStatus)
        {

            PatientDetailsList ListPatientDetails = new PatientDetailsList();


            try
            {
                //"GetAllPatientDetails"
      ///  DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllPatientDetails",
             DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllPatientDetails_Testing",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        Convert.ToInt32(OfficeId),
                                                        search,                                                      
                                                        Convert.ToInt32(OrganisationId),
                                                        IsActiveStatus
                                                        );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<PatientsDetail> PatientDetailsList = new List<PatientsDetail>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        PatientsDetail objPatientDetail = new PatientsDetail();
                        objPatientDetail.PatientId = Convert.ToInt32(ds.Tables[0].Rows[i]["PatientId"]);
                        objPatientDetail.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        objPatientDetail.FirstName= ds.Tables[0].Rows[i]["FirstName"].ToString();
                        objPatientDetail.LastName= ds.Tables[0].Rows[i]["LastName"].ToString();
                        objPatientDetail.MedicalId = ds.Tables[0].Rows[i]["MedicalId"].ToString();
                        objPatientDetail.PhoneNo = ds.Tables[0].Rows[i]["PhoneNo"].ToString();
                        objPatientDetail.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        objPatientDetail.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objPatientDetail.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objPatientDetail.State = ds.Tables[0].Rows[i]["State"].ToString();
                        objPatientDetail.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objPatientDetail.PrimaryMD = ds.Tables[0].Rows[i]["PrimaryMD"].ToString();
                        int officeId = 0;
                        Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["OfficeId"]), out officeId);
                        objPatientDetail.OfficeId = officeId;

                        objPatientDetail.IsActive = Convert.ToString(ds.Tables[0].Rows[i]["IsActive"]);

                        objPatientDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        PatientDetailsList.Add(objPatientDetail);
                    }

                    ListPatientDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListPatientDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListPatientDetails.PatientList = PatientDetailsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientDetailsList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListPatientDetails;
        }


        public PatientsDetail GetPatientDetailById(string PatientDetailId)
        {
            PatientsDetail objPatientDetail = new PatientsDetail();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientDetailById", PatientDetailId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    objPatientDetail.PatientId = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientId"]);
                    objPatientDetail.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();

                    objPatientDetail.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    objPatientDetail.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();

                    objPatientDetail.MedicalId = ds.Tables[0].Rows[0]["MedicalId"].ToString();
                    objPatientDetail.PhoneNo = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                    objPatientDetail.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    objPatientDetail.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    objPatientDetail.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objPatientDetail.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objPatientDetail.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    objPatientDetail.PrimaryMD = ds.Tables[0].Rows[0]["PrimaryMD"].ToString();

                    objPatientDetail.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    objPatientDetail.Password = ds.Tables[0].Rows[0]["Password"].ToString();

                    int officeId = 0;
                    Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["OfficeId"]), out officeId);
                    objPatientDetail.OfficeId = officeId;


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
                string result = InsertErrorLog(objErrorlog);
            }
            return objPatientDetail;
        }


        public string EditPatientDetails(PatientsDetail PatientsDetail)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdatePatientDetails",
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
                                                    PatientsDetail.JurisdictionCode
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
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string DeletePatientDetail(string PatientDetailId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeletePatientDetail",
                                                    PatientDetailId, Guid.Parse(UserId));

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
                objErrorlog.Methodname = "DeletePatientDetail";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public static string AddPatientDetailsFromExcel(DataTable UserDataTable, String UserId)
        {
            string result = "";

            var ConnestionString = Settings.CareGiverSuperAdminDatabase().ToString();
            using (var con = new SqlConnection(ConnestionString))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int BulkCount = 1000;
                        int TotalBulks = ((int)UserDataTable.Rows.Count / BulkCount) + 1;
                        for (int i = 0; i < TotalBulks; i++)
                        {

                            var chundDt = UserDataTable.Rows.Cast<System.Data.DataRow>().Skip(i * BulkCount).Take(BulkCount).CopyToDataTable();
                            if (chundDt.Rows.Count > 0)
                            {
                                using (SqlCommand cmd = new SqlCommand("ImportPatientsDataFromExcel", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Transaction = tran;
                                    cmd.Parameters.AddWithValue("@UserId", UserId);
                                    var UserDetailsList = new SqlParameter("@UserDetails", SqlDbType.Structured);
                                    UserDetailsList.TypeName = "TTImportPatientDetailsList";
                                    UserDetailsList.Value = chundDt;

                                    cmd.Parameters.Add(UserDetailsList);
                                    cmd.CommandTimeout = 6000;

                                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                    {
                                        var ds = new DataSet();
                                        da.Fill(ds);
                                        if (ds != null && ds.Tables.Count > 0)
                                        {
                                            var CurrentRow = ds.Tables[0].Rows[0];
                                        }
                                        else
                                        {
                                            //ObjStatus.IsSuccess = false;
                                            //ObjStatus.Message = "Connection Error";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        tran.Commit();
                        result = "Success";

                        //ObjStatus.IsSuccess = true;

                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;

                        tran.Rollback();
                        con.Close();


                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return result;
        }



        public static string AddPatientAppointmentFromExcel(DataTable UserDataTable, String UserId)
        {
            string result = "";

            var ConnestionString = Settings.CareGiverSuperAdminDatabase().ToString();
            using (var con = new SqlConnection(ConnestionString))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int BulkCount = 1000;
                        int TotalBulks = ((int)UserDataTable.Rows.Count / BulkCount) + 1;
                        for (int i = 0; i < TotalBulks; i++)
                        {

                            var chundDt = UserDataTable.Rows.Cast<System.Data.DataRow>().Skip(i * BulkCount).Take(BulkCount).CopyToDataTable();
                            if (chundDt.Rows.Count > 0)
                            {
                                using (SqlCommand cmd = new SqlCommand("ImportPatientsAppointmentDataFromExcel_V1", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Transaction = tran;
                                    cmd.Parameters.AddWithValue("@UserId", UserId);
                                    var PatientDetailsList = new SqlParameter("@PatientDetails", SqlDbType.Structured);
                                    PatientDetailsList.TypeName = "TTImportPatientAppointmentDetailsList";
                                    PatientDetailsList.Value = chundDt;

                                    cmd.Parameters.Add(PatientDetailsList);
                                    cmd.CommandTimeout = 6000;

                                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                    {                                        
                                        var ds = new DataSet();
                                        da.Fill(ds);
                                        if (ds != null && ds.Tables.Count > 0)
                                        {
                                            var CurrentRow = ds.Tables[0].Rows[0];
                                        }
                                        else
                                        {
                                            //ObjStatus.IsSuccess = false;
                                            //ObjStatus.Message = "Connection Error";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        tran.Commit();
                        result = "Success";

                        //ObjStatus.IsSuccess = true;

                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;

                        tran.Rollback();
                        con.Close();

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return result;
        }


        public static DataSet GetPatientDetailsListForExcel(string loginUserId, int OfficeId)
        {
            DataSet Ds = new DataSet();
            try
            {
                DataSet tmpDs = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientDetailsListForExcel", loginUserId, OfficeId);

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

        public static DataSet GetPatientInsuranceDetailsListForExcel( int OfficeId)
        {
            //string loginUserId

            DataSet Ds = new DataSet();
            try
            {
                DataSet tmpDs = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientInsuranceDetailsListForExcel", OfficeId);

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

        public bool IsMedicalIdExist(PatientsDetail PatientsDetail)
        {
            bool result = false;
            string error = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "IsMedicalIdExist", PatientsDetail.MedicalId);

                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    result = false;//not Exist
                }
                else
                {
                    result = true;//yes Exist
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "IsMedicalIdExist";
                objErrorlog.UserID = PatientsDetail.InsertUserId;
                error = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        /*30-08-2017 End*/
        #endregion


        //Added By pramendra
        //on 9th Sept,2019
        public List<ScheduleInfo> GetAllSchedulerbyCaregiverId(CareGivers CareGiver)
        {
            List<ScheduleInfo> lstScheduleInfo = new List<ScheduleInfo>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllSchedulersbyCaregiverId", Convert.ToInt32(CareGiver.NurseId));
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            ScheduleInfo objScheduleInfo = new ScheduleInfo();
                            objScheduleInfo.SchedulerId = Convert.ToInt32(item["SchedulerId"]);
                            objScheduleInfo.UserId = Convert.ToString(item["UserId"]);
                            objScheduleInfo.UserName = Convert.ToString(item["UserName"]);
                            objScheduleInfo.FirstName = Convert.ToString(item["FirstName"]);
                            objScheduleInfo.LastName = Convert.ToString(item["LastName"]);
                            objScheduleInfo.Email = Convert.ToString(item["Email"]);
                            objScheduleInfo.QuickbloxId = Convert.ToString(item["QuickbloxId"]);
                            objScheduleInfo.QBDialogId = Convert.ToString(item["QBDialogId"]);
                            lstScheduleInfo.Add(objScheduleInfo);
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllSchedulerbyCaregiverId";
                InsertErrorLog(objErrorlog);
            }
            return lstScheduleInfo;
        }

        //11th Sept,2017
        public Chatting GetQuickBloxDetByUserId(string UserId)
        {
            Chatting objChatting = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetQuickBloxDetByUserId", UserId);

                objChatting.QuickBloxId = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();
                objChatting.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                objChatting.UserId = ds.Tables[0].Rows[0]["UserID"].ToString();
                objChatting.ToEmail = ds.Tables[0].Rows[0]["Email"].ToString();
                objChatting.CareGiverName = ds.Tables[0].Rows[0]["CareGiverName"].ToString();

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetQuickBloxIdByUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return objChatting;
        }


        public Chatting GetQuickBloxDetById(string Id)
        {
            Chatting objChatting = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetQuickBloxDetById", Convert.ToInt32(Id));

                objChatting.QuickBloxId = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();
                objChatting.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                objChatting.UserId = ds.Tables[0].Rows[0]["UserID"].ToString();
                objChatting.ToEmail = ds.Tables[0].Rows[0]["Email"].ToString();
                objChatting.CareGiverName = ds.Tables[0].Rows[0]["CareGiverName"].ToString();

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetQuickBloxIdById";
                string result = InsertErrorLog(objErrorlog);
            }
            return objChatting;
        }


        //12th Sept,2017
        public List<Chatting> GetPatientChattingGroupList(string UserId)
        {
            List<Chatting> listChattingGroup = new List<Chatting>();
            Chatting objChattingGroup = new Chatting();
            try
            {
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientChattingGroupList", Guid.Parse(UserId));
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetPatientChattingGroupList", Guid.Parse(UserId));
                if (ds != null && ds.Tables.Count > 0)
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objChattingGroup = new Chatting();
                        objChattingGroup.DialogId = item["DialogId"].ToString();
                        objChattingGroup.GroupName = Convert.ToString(item["GroupName"]);
                        objChattingGroup.ChattingGroupId = Convert.ToInt32(item["ChattingGroupId"]);
                        objChattingGroup.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        objChattingGroup.OfficeName = item["OfficeName"].ToString();
                        listChattingGroup.Add(objChattingGroup);
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientChattingGroupList";
                InsertErrorLog(objErrorlog);
            }
            return listChattingGroup;
        }

   

        public List<CareGivers> GetUnassignedCaregiverList(string ChattingGroupId)
        {
            List<CareGivers> Caregivers = new List<CareGivers>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetUnassignedCaregiverList"
                                                        , Convert.ToInt32(ChattingGroupId));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CareGivers objCaregiver = new CareGivers();
                        objCaregiver.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["nurseId"]);
                        objCaregiver.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objCaregiver.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objCaregiver.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objCaregiver.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objCaregiver.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        Caregivers.Add(objCaregiver);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetCaregiverList";
                string result = InsertErrorLog(objErrorlog);
            }
            return Caregivers;
        }



        public List<ChattingGroupMember> GetAssignedCaregiverListGroupWise(string ChattingGroupId)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAssignedCaregiverListGroupWise",
                                                  Convert.ToInt32(ChattingGroupId));
                if (ds.Tables[0].Rows.Count == 0)
                    result = "0";
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupMemberDetail = new ChattingGroupMember();
                        objGroupMemberDetail.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objGroupMemberDetail.MemberName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objGroupMemberDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objGroupMemberDetail.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        objGroupMemberDetailList.Add(objGroupMemberDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAssignedCaregiverListGroupWise";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupMemberDetailList;
        }


        public string AddCaregiverIntoGroup(string ChattingGroupId, string NurseId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AddCaregiverIntoGroup",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    Convert.ToInt32(NurseId));
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
                objErrorlog.Methodname = "AddCaregiverIntoGroup";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public string RemoveMemberFromGroupChat(string ChattingGroupId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "RemoveMemberFromGroupChat",
                                                    Convert.ToInt32(ChattingGroupId), Guid.Parse(UserId));
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
                objErrorlog.Methodname = "RemoveMemberFromGroupChat";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        // started on 18 Sept
        #region Superadmin can assign multiple chat groups to scheduler/caregiver
        public List<Chatting> GetAllGroupExceptAssignedGroupByUserId(string UserId, string LoginUserId)
        {
            var objGroupList = new List<Chatting>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllGroupExceptAssignedGroupByUserId",
                                                  Guid.Parse(UserId),
                                                  Guid.Parse(LoginUserId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupDetail = new Chatting();

                        objGroupDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"].ToString());
                        objGroupDetail.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                        objGroupDetail.DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();

                        objGroupList.Add(objGroupDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllGroupExceptAssignedGroupByUserId";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupList;
        }
        public List<Chatting> GetAllAssignedGroupByUserId(string UserId, string LoginUserId)
        {
            var objGroupList = new List<Chatting>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAssignedGroupByUserId",
                                                  Guid.Parse(UserId),
                                                  Guid.Parse(LoginUserId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupDetail = new Chatting();

                        objGroupDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"].ToString());
                        objGroupDetail.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                        objGroupDetail.DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                        objGroupDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objGroupDetail.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());

                        objGroupList.Add(objGroupDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAssignedGroupByUserId";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupList;
        }
        //public string AssignGroupToCaregiver(string ChattingGroupId, string UserId)
        //{
        //    string result = "";
        //    try
        //    {
        //        int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AssignGroupToCaregiver",
        //                                            Convert.ToInt32(ChattingGroupId),
        //                                            new Guid(UserId));
        //        if (i > 0)
        //        {
        //            result = "Success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "AssignGroupToCaregiver";
        //        result = InsertErrorLog(objErrorlog);
        //    }
        //    return result;
        //}



        // Added By Vinod Verma on 17th Oct 2018


        public List<CareGivers> GetAllMemberList(string UserId)
        {
            List<CareGivers> listCaregiver = new List<CareGivers>();

            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllMemberList"
                                                                                                        , Guid.Parse(UserId));
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            CareGivers objCaregiver = new CareGivers();
                            objCaregiver.NurseId = Convert.ToInt32(item["Id"]);
                            objCaregiver.UserId = Convert.ToString(item["UserId"]);
                            objCaregiver.Name = Convert.ToString(item["Name"]) + " (" + Convert.ToString(item["UserRole"]) + ")";
                            //  objCaregiver.Name = Convert.ToString(item["Name"]);
                            objCaregiver.UserName = Convert.ToString(item["UserName"]);
                            objCaregiver.Email = Convert.ToString(item["Email"]);
                            objCaregiver.QuickBloxId = Convert.ToString(item["QuickbloxId"]);

                            listCaregiver.Add(objCaregiver);
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteMobileService";
                objErrorlog.Methodname = "GetAllUnAssignedMemberList";
                InsertErrorLog(objErrorlog);
            }
            return listCaregiver;
        }


        public string AssignGroupToUser(string ChattingGroupId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AssignGroupToUser",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    new Guid(UserId));
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
                objErrorlog.Methodname = "AssignGroupToUser";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        #endregion

        #region Change Password In UserloginInfo Using Email From Membership table(Forget Password Module) StaticMethod
        public static string ForgetPasswordUpdatingUserLogininfoUserPassword(string Email, string NewPassword)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString()
                                                    , "ChangeUserLoginInfoPasswordFromForgotPassword",
                                                    Email, NewPassword);
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
                objErrorlog.Methodname = "ForgetPasswordUpdatingUserLogininfoUserPassword";
                //result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        #endregion
        //started on 26th Oct,2017
        #region Supervisor/Support can assign multiple member to group
        public List<CareGivers> GetAllUnAssignedMemberList(string ChattingGroupId, string UserId, string OrganisationId)
        {
            List<CareGivers> listCaregiver = new List<CareGivers>();

            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetCaregiverListToAddIntoPatientGroupByChattingGroupId_v1"

                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetCaregiverListToAddIntoPatientGroupByChattingGroupId_v1"
                                                                                                        , Convert.ToInt32(ChattingGroupId)
                                                                                                        , Guid.Parse(UserId),Convert.ToInt32(OrganisationId));
                if (ds != null)
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            CareGivers objCaregiver = new CareGivers();
                            objCaregiver.NurseId = Convert.ToInt32(item["Id"]);
                            objCaregiver.UserId = Convert.ToString(item["UserId"]);
                            objCaregiver.Name = Convert.ToString(item["Name"]) + " (" + Convert.ToString(item["UserRole"]) + ")";
                            //  objCaregiver.Name = Convert.ToString(item["Name"]);
                            objCaregiver.UserName = Convert.ToString(item["UserName"]);
                            objCaregiver.Email = Convert.ToString(item["Email"]);
                            objCaregiver.QuickBloxId = Convert.ToString(item["QuickbloxId"]);

                            listCaregiver.Add(objCaregiver);
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteMobileService";
                objErrorlog.Methodname = "GetAllUnAssignedMemberList";
                InsertErrorLog(objErrorlog);
            }
            return listCaregiver;
        }


        public List<ChattingGroupMember> GetAssignedMemberListGroupWise(string ChattingGroupId)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            string result = "";
            try
            {
                // GetChattingGroupMemberByChattingGroupId
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAssignedMemberListGroupWise",
                                                  Convert.ToInt32(ChattingGroupId));
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetChattingGroupMemberByChattingGroupId",
                //                                 Convert.ToInt32(ChattingGroupId));
                if (ds.Tables[0].Rows.Count == 0)
                    result = "0";
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupMemberDetail = new ChattingGroupMember();
                        objGroupMemberDetail.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objGroupMemberDetail.MemberName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objGroupMemberDetail.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        objGroupMemberDetail.Type = ds.Tables[0].Rows[i]["UserType"].ToString();
                        objGroupMemberDetail.IsGroupAdmin = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsGroupAdmin"].ToString());
                        objGroupMemberDetailList.Add(objGroupMemberDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAssignedMemberListGroupWise";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupMemberDetailList;
        }
        #endregion

        //Hardik Masalawala 30-10-2017
        #region GetGroupDetailFromGroupName 
        public Chatting GetGroupDetailFromGroupName(string GroupName)
        {
            var objDialogDetail = new Chatting();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetDialogDetailFromGroupName",
                                                  GroupName);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {

                    objDialogDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChattingGroupId"]);
                    objDialogDetail.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    objDialogDetail.DialogId = ds.Tables[0].Rows[0]["DialogId"].ToString();

                }


            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetGroupDetailFromGroupName";
                result = InsertErrorLog(objErrorlog);
            }
            return objDialogDetail;
        }

        #endregion

        #region Office

        #region GetAllOffices
        public OfficesList GetAllOfficesList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string OrganisationId)
        {
            OfficesList ListOffice = new OfficesList();

            try
            {
               // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllOffices",
                     DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllOffices",
                                                        Guid.Parse(LogInUserId),
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        Convert.ToInt32(OrganisationId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Office> OfficeList = new List<Office>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Office objOffice = new Office();
                        objOffice.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOffice.AdminUserId = ds.Tables[0].Rows[i]["AdminUserId"].ToString();
                        objOffice.AdminName = ds.Tables[0].Rows[i]["AdminName"].ToString();
                        objOffice.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objOffice.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objOffice.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objOffice.State = ds.Tables[0].Rows[i]["State"].ToString();
                        objOffice.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        //objOffice.InserteOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["InsertDateTime"]).ToString("dd MMM yyyy hh:mm tt");
                        objOffice.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objOffice.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();
                        objOffice.AssignedZipcodes = ds.Tables[0].Rows[i]["AssignedZipcodes"].ToString();


                        OfficeList.Add(objOffice);
                    }

                    ListOffice.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListOffice.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListOffice.OfficeList = OfficeList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllOffices";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListOffice;
        }

        public List<Office> GetAllAvailableOfficesList(string LoginUserId, string OrganisationId)
        {
            List<Office> lstOffices = new List<Office>();
            try
            {
                //  DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAvailableOffices", Guid.Parse(LoginUserId), Convert.ToInt32(OrganisationId));

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAvailableOffices", Guid.Parse(LoginUserId), Convert.ToInt32(OrganisationId));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Office> OfficeList = new List<Office>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Office objOffice = new Office();
                        objOffice.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOffice.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                        lstOffices.Add(objOffice);
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
                string result = InsertErrorLog(objErrorlog);
            }
            return lstOffices;
        }


      



        #endregion

        #region AddOffice
        public Office AddOffice(Office Office)
        {
            //string result = "";
            Office objOffice = new Office();
            try
            {
                //  DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddOffice",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_AddOffice",
                                                  Office.OfficeId,
                                                    Office.OfficeName,
                                                    Office.AdminUserId,
                                                    Office.Street,
                                                    Office.City,
                                                    Office.State,
                                                    Office.ZipCode,
                                                    Office.Latitude,
                                                    Office.Longitude,
                                                    Guid.Parse(Office.InsertUserId),
                                                    Office.TimezoneId,
                                                    Office.TimezoneOffset,
                                                    Office.TimezonePostfix,
                                                    Office.OrganisationId
                                                    );

                //if (i > 0)
                //{
                //    result = "Success";
                //}

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    objOffice.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"]);
                    objOffice.OfficeName = ds.Tables[0].Rows[0]["OfficeName"].ToString();
                    objOffice.AdminUserId = ds.Tables[0].Rows[0]["AdminUserId"].ToString();
                    objOffice.AdminQuickBloxId = ds.Tables[0].Rows[0]["AdminQuickBloxId"].ToString();
                    objOffice.Result = ds.Tables[0].Rows[0]["Result"].ToString();

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddOffice";
                objErrorlog.UserID = Office.InsertUserId;
                objOffice.Result = InsertErrorLog(objErrorlog);
            }
            return objOffice;
        }
        #endregion

        #region AdminNameList
        //AdminNameList
        public List<Admin> GetAllAdminName(string OrganisationId)
        {
            List<Admin> AdminNameList = new List<Admin>();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAdminName", OrganisationId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAdminName", OrganisationId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Admin objAdmin = new Admin();
                        objAdmin.AdminId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdminId"]);
                        objAdmin.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objAdmin.Name = ds.Tables[0].Rows[i]["Name"].ToString();

                        AdminNameList.Add(objAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAdminName";
                string result = InsertErrorLog(objErrorlog);
            }
            return AdminNameList;
        }

        #endregion


        #region EditOffice

        public string EditOffice(Office Office)
        {
            string result = "";
            try
            {
                //int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "EditOffice",
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_EditOffice",
                                                   Office.OfficeId,
                                                    Office.OfficeName,
                                                    Office.AdminUserId,
                                                    Office.Street,
                                                    Office.City,
                                                    Office.State,
                                                    Office.ZipCode,
                                                    Office.Latitude,
                                                    Office.Longitude,
                                                    Guid.Parse(Office.InsertUserId),
                                                    Office.TimezoneId,
                                                    Office.TimezoneOffset,
                                                    Office.TimezonePostfix,
                                                    Office.OrganisationId

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
                objErrorlog.Methodname = "EditOffice";
                objErrorlog.UserID = Office.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public Office GetOfficeDetailByOfficeId(string OfficeId)
        {
            Office Office = new Office();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOfficeDetailByOfficeId", OfficeId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Office.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["officeId"]);
                    Office.AdminUserId = ds.Tables[0].Rows[0]["AdminUserId"].ToString();
                    Office.OfficeName = ds.Tables[0].Rows[0]["officeName"].ToString();
                    Office.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    Office.City = ds.Tables[0].Rows[0]["City"].ToString();
                    Office.State = ds.Tables[0].Rows[0]["State"].ToString();
                    Office.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    Office.AdminEmail = ds.Tables[0].Rows[0]["AdminEmail"].ToString();
                    Office.AdminQuickBloxId = ds.Tables[0].Rows[0]["AdminQuickbloxId"].ToString();


                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetOfficeDetailByOfficeId";
                string result = InsertErrorLog(objErrorlog);
            }
            return Office;
        }



        public Office GetOrganisationOfficeDetailByOfficeId(string OfficeId, string OrganisationId)
        {
            Office Office = new Office();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOrganisationOfficeDetailByOfficeId", OfficeId, Convert.ToInt32(OrganisationId));

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOrganisationOfficeDetailByOfficeId", OfficeId, Convert.ToInt32(OrganisationId));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Office.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["officeId"]);
                    Office.AdminUserId = ds.Tables[0].Rows[0]["AdminUserId"].ToString();
                    Office.OfficeName = ds.Tables[0].Rows[0]["officeName"].ToString();
                    Office.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    Office.City = ds.Tables[0].Rows[0]["City"].ToString();
                    Office.State = ds.Tables[0].Rows[0]["State"].ToString();
                    Office.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    Office.AdminEmail = ds.Tables[0].Rows[0]["AdminEmail"].ToString();
                    Office.AdminQuickBloxId = ds.Tables[0].Rows[0]["AdminQuickbloxId"].ToString();


                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetOfficeDetailByOfficeId";
                string result = InsertErrorLog(objErrorlog);
            }
            return Office;
        }


        #endregion

        #region DeleteOffice
        public string DeleteOfficeByOfficeId(string OfficeId, string UserId, string OrganisationId)
        {
            string result = "";
            try
            {
                // int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteOfficeByOfficeId",

                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_DeleteOfficeByOfficeId",
                                                     OfficeId,
                                                    new Guid(UserId),
                                                    Convert.ToInt32(OrganisationId));

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
                objErrorlog.Methodname = "DeleteOfficeByOfficeId";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        #endregion

        #region AssignZipcodesToOfficeByOfficeId

        public string AssignZipcodesToOfficeByOfficeId(string OfficeId, string AssignedZipcodes, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AssignZipcodesToOfficeByOfficeId",
                                                    OfficeId,
                                                    AssignedZipcodes,
                                                    Guid.Parse(UserId)
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
                objErrorlog.Methodname = "AssignZipcodesToOfficeByOfficeId";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string GetAssignZipcodesToOfficeByOfficeId(string OfficeId)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAssignZipcodesToOfficeByOfficeId",
                                                    OfficeId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["AssignedZipcodes"].ToString();
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAssignZipcodesToOfficeByOfficeId";

                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        #endregion
        #endregion

        #region Admin
        // Created By Krunal Pawar on 01-11-2017 for Office Admin

        public string AddAdmin(Admin Admin)
        {
            string result = "";
            try
            {
                //  int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AddAdmin",
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_AddAdmin",
                                                      Admin.AdminId,
                                                    new Guid(Admin.UserId),
                                                    Admin.FirstName,
                                                    Admin.LastName,
                                                    Admin.UserName,
                                                    Admin.Email,
                                                    Admin.Password,
                                                    new Guid(Admin.InsertUserId),
                                                    Admin.OrganisationId
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
                objErrorlog.Methodname = "AddOfficeAdmin";
                objErrorlog.UserID = Admin.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public AdminsList GetAllOfficeAdmin(string pageNo, string recordPerPage, string sortfield, string sortorder, string officeId, string search, string OrganisationId,string IsActiveStatus)
        {
            AdminsList ListAdmin = new AdminsList();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAdmin",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAdmin_Testing",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        officeId,
                                                        search,
                                                        Convert.ToInt32(OrganisationId),
                                                        Convert.ToInt32(IsActiveStatus));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Admin> AdminList = new List<Admin>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Admin objAdmin = new Admin();
                        objAdmin.AdminId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdminId"]);
                        objAdmin.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objAdmin.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        objAdmin.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        objAdmin.IsActive= Convert.ToString(ds.Tables[0].Rows[i]["IsActive"].ToString());
                        objAdmin.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objAdmin.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        objAdmin.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objAdmin.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        AdminList.Add(objAdmin);
                    }

                    ListAdmin.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListAdmin.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListAdmin.AdminList = AdminList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAdmin";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListAdmin;
        }


        public Admin GetAdminDetailById(string AdminId)
        {
            Admin Admin = new Admin();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAdminDetailById", AdminId, new Guid());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Admin.AdminId = Convert.ToInt32(ds.Tables[0].Rows[0]["AdminId"]);
                    Admin.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    Admin.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    Admin.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    Admin.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    Admin.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    Admin.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    Admin.QuickBloxId = ds.Tables[0].Rows[0]["QuickBloxId"].ToString();
                    Admin.IsActive = Convert.ToString(ds.Tables[0].Rows[0]["IsActive"].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAdminDetailById";
                string result = InsertErrorLog(objErrorlog);
            }
            return Admin;
        }

        public string EditAdmin(Admin Admin)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ORGNew_EditAdmin",
                                                    Admin.AdminId,
                                                    Admin.FirstName,
                                                    Admin.LastName,
                                                    Admin.Email,
                                                    //Admin.Password,
                                                    new Guid(Admin.InsertUserId)
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
                objErrorlog.Methodname = "EditAdmin";
                objErrorlog.UserID = Admin.InsertUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public string DeleteAdmin(string AdminId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteAdmin",
                                                    AdminId,
                                                    new Guid(UserId));

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
                objErrorlog.Methodname = "DeleteAdmin";
                objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public Admin GetAdminDetailByUserId(string AdminUserId)
        {
            Admin Admin = new Admin();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAdminDetailByUserId", AdminUserId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAdminDetailByUserId", AdminUserId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Admin.AdminId = Convert.ToInt32(ds.Tables[0].Rows[0]["AdminId"]);
                    Admin.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                    Admin.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    Admin.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    Admin.OfficeIds = ds.Tables[0].Rows[0]["OfficeIds"].ToString();
                    Admin.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    Admin.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"]);
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAdminDetailByUserId";
                string result = InsertErrorLog(objErrorlog);
            }
            return Admin;
        }
        #endregion

        #region GetOfficeDropDownList
        public static OfficesList GetOfficeDropDownList(string LogInUserId)
        {
            OfficesList ListOffice = new OfficesList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAvailableOffices",
                                                        Guid.Parse(LogInUserId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Office> OfficeList = new List<Office>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Office objOffice = new Office();
                        objOffice.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOffice.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        OfficeList.Add(objOffice);
                    }

                    ListOffice.OfficeList = OfficeList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetOfficeDropDownList";
                //string result = InsertErrorLog(objErrorlog);
            }
            return ListOffice;
        }
        #endregion


        public List<Office> GetAllOfficeExceptAssignedOfficeByUserId(string UserId, string LoginUserId)
        {
            var OfficesList = new List<Office>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllOfficeExceptAssignedOfficeByUserId",
                                                  Guid.Parse(UserId),
                                                   Guid.Parse(LoginUserId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objOffice = new Office();

                        objOffice.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOffice.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                        OfficesList.Add(objOffice);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllOfficeExceptAssignedOfficeByUserId";
                result = InsertErrorLog(objErrorlog);
            }
            return OfficesList;
        }


        public string AssignOfficeToUser(string OfficeId, string UserId, string LoginUserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AssignOfficeToUser",
                                                    Convert.ToInt32(OfficeId),
                                                    new Guid(UserId),
                                                    new Guid(LoginUserId));
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
                objErrorlog.Methodname = "AssignOfficeToUser";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public List<Office> GetAllAssignedOfficeByUserId(string UserId)
        {
            var objOfficeList = new List<Office>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAssignedOfficeByUserId",
                                                  Guid.Parse(UserId));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objOfficeDetail = new Office();

                        objOfficeDetail.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOfficeDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();

                        objOfficeList.Add(objOfficeDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAssignedOfficeByUserId";
                result = InsertErrorLog(objErrorlog);
            }
            return objOfficeList;
        }


        public string RemoveSchedulerFromOffice(string OfficeId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "RemoveSchedulerFromOffice",
                                                    Convert.ToInt32(OfficeId), Guid.Parse(UserId));
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
                objErrorlog.Methodname = "RemoveSchedulerFromOffice";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string SaveQBIdOfAdmin(string Email, string QuickbloxId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "SaveQBIdOfAdmin",
                                                    Email,
                                                    QuickbloxId
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
                objErrorlog.Methodname = "SaveQBIdOfAdmin";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public List<Chatting> GetPatientChattingGroupByOfficeId(string OfficeId)
        {
            List<Chatting> listChattingGroup = new List<Chatting>();
            Chatting objChattingGroup = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetPatientChattingGroupByOfficeId", OfficeId);
                if (ds != null && ds.Tables.Count > 0)
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objChattingGroup = new Chatting();
                        objChattingGroup.DialogId = item["DialogId"].ToString();
                        objChattingGroup.GroupName = Convert.ToString(item["GroupName"]);
                        objChattingGroup.ChattingGroupId = Convert.ToInt32(item["ChattingGroupId"]);
                        objChattingGroup.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        objChattingGroup.OfficeName = item["OfficeName"].ToString();
                        listChattingGroup.Add(objChattingGroup);
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetPatientChattingGroupByOfficeId";
                InsertErrorLog(objErrorlog);
            }
            return listChattingGroup;
        }


        public List<ChattingGroupMember> GetAllMemberByOffice(string LoginUserId, string OfficeId,string OrganisationId)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            string result = "";
            try
            { 
                    // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllMemberByOffice",
                   // office previously was in int , now i changed it in to nvarchar(max)

                    DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllMemberByOffice",
                                                      Guid.Parse(LoginUserId), Convert.ToString(OfficeId), Convert.ToInt32(OrganisationId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var objGroupMemberDetail = new ChattingGroupMember();
                        objGroupMemberDetail.UserId = item["UserId"].ToString();
                        objGroupMemberDetail.MemberName = item["Name"].ToString();
                        objGroupMemberDetail.QuickBloxId = item["QuickBloxId"].ToString();
                        objGroupMemberDetail.Type = item["UserRole"].ToString();
                        objGroupMemberDetailList.Add(objGroupMemberDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllMemberByOffice";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupMemberDetailList;
        }



        public Chatting GetOfficeGroupDetailByOfficeId(string OfficeId, string UserId)
        {
            var objDialogDetail = new Chatting();
            string result = "";
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOfficeGroupDetailByOfficeId",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOfficeGroupDetailByOfficeId",
                                                 OfficeId
                                                  , Guid.Parse(UserId));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objDialogDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChattingGroupId"]);
                    objDialogDetail.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    objDialogDetail.DialogId = ds.Tables[0].Rows[0]["DialogId"].ToString();
                    objDialogDetail.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();

                }


            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetGroupDetailFromGroupName";
                result = InsertErrorLog(objErrorlog);
            }
            return objDialogDetail;
        }

        #region DeleteGroupChat
        public string DeleteGroupChat(string ChattingGroupId, string DialogId, string LoginUserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteGroupChat",
                                                   new Guid(LoginUserId),
                                                   ChattingGroupId,
                                                   DialogId
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
                objErrorlog.Methodname = "DeleteGroupChat";
                objErrorlog.UserID = LoginUserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }
        #endregion

        #region Update Group Chat Detail
        public string UpdateGroupDetail(string ChattingGroupId, string DialogId, string GroupName, string GroupSubject, string LoginUserID)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "UpdateGroupDetail",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    DialogId,
                                                    GroupName,
                                                    GroupSubject,
                                                    new Guid(LoginUserID));
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
                objErrorlog.Methodname = "UpdateGroupDetail";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        #endregion


        public List<Chatting> GetChatGroupListByTypeIdForUser(string UserId, string GroupTypeId)
        {
            List<Chatting> listChattingGroup = new List<Chatting>();
            Chatting objChattingGroup = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetChatGroupListByTypeIdForUser", Guid.Parse(UserId), Convert.ToInt32(GroupTypeId));
                if (ds != null && ds.Tables.Count > 0)
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objChattingGroup = new Chatting();
                        objChattingGroup.DialogId = item["DialogId"].ToString();
                        objChattingGroup.GroupName = Convert.ToString(item["GroupName"]);
                        objChattingGroup.ChattingGroupId = Convert.ToInt32(item["ChattingGroupId"]);
                        objChattingGroup.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        objChattingGroup.OfficeName = item["OfficeName"].ToString();
                        objChattingGroup.GroupTypeId = Convert.ToInt32(item["GroupTypeId"]);
                        listChattingGroup.Add(objChattingGroup);
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetChatGroupListByTypeIdForUser";
                InsertErrorLog(objErrorlog);
            }
            return listChattingGroup;
        }



        public bool IsGroupNameAndSubjectExist(Chatting Chatting, string LoginUserId)
        {
            bool result = false;
            string error = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "IsGroupNameAndSubjectExist", Chatting.GroupName, Chatting.GroupSubject, Guid.Parse(LoginUserId));


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = true;//yes Exist
                }

            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetGroupDetailFromGroupName";
                error = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        public string ExitMemberFromGroupChat(string ChattingGroupId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "ExitMemberFromGroupChat",
                                                    Convert.ToInt32(ChattingGroupId), Guid.Parse(UserId));
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
                objErrorlog.Methodname = "ExitMemberFromGroupChat";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }



        public string AssignGroupAdminToUser(string ChattingGroupId, string UserId, string LoginUserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AssignGroupAdminToUser",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    new Guid(UserId),
                                                    new Guid(LoginUserId)
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
                objErrorlog.Methodname = "AssignGroupAdminToUser";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }


        //public List<Chatting> GetOneToOneChatListByUserId(string UserId)
        //{
        //    List<Chatting> listChattingGroup = new List<Chatting>();
        //    Chatting objChattingGroup = new Chatting();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOneToOneChatListByUserId", Guid.Parse(UserId));

        //        if (ds != null && ds.Tables.Count > 0)
        //        {
        //            foreach (DataRow item in ds.Tables[0].Rows)
        //            {
        //                objChattingGroup = new Chatting();
        //                objChattingGroup.DialogId = item["DialogId"].ToString();
        //                objChattingGroup.GroupName = Convert.ToString(item["GroupName"]);
        //                objChattingGroup.ChattingGroupId = Convert.ToInt32(item["ChattingGroupId"]);
        //                objChattingGroup.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
        //                objChattingGroup.OfficeName = item["OfficeName"].ToString();
        //                listChattingGroup.Add(objChattingGroup);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetOneToOneChatListByUserId";
        //        InsertErrorLog(objErrorlog);
        //    }
        //    return listChattingGroup;
        //}


        public List<Chatting> GetOneToOneChatListByUserId(string UserId)
        {
            List<Chatting> listChattingGroup = new List<Chatting>();
            Chatting objChattingGroup = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOneToOneChatListByUserId", Guid.Parse(UserId));

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objChattingGroup = new Chatting();
                        objChattingGroup.DialogId = item["DialogId"].ToString();
                        objChattingGroup.GroupName = Convert.ToString(item["GroupName"]);
                        objChattingGroup.FromEmail = item["FromEmail"].ToString();
                        objChattingGroup.ToEmail = item["ToEmail"].ToString();
                        // objChattingGroup.ChattingGroupId = Convert.ToInt32(item["ChattingGroupId"]);
                        //   objChattingGroup.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        //   objChattingGroup.OfficeName = item["OfficeName"].ToString();
                        listChattingGroup.Add(objChattingGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetOneToOneChatListByUserId";
                InsertErrorLog(objErrorlog);
            }
            return listChattingGroup;
        }

        public string DeleteOneToOneChatByUserId(string DialogId, string UserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "DeleteOneToOneChatByUserId",
                                                    DialogId, Guid.Parse(UserId));
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
                objErrorlog.Methodname = "DeleteOneToOneChatByUserId";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        #region AssignPermission
        public List<ChattingGroupMember> GetGroupMemberListWithPermissionAndRole(string Id)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetGroupMemberListWithPermissionAndRole",
                                                  Id);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result = "0";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupMemberDetail = new ChattingGroupMember();
                        objGroupMemberDetail.ChattingGroupMemberId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupMemberId"]);
                        objGroupMemberDetail.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"]);
                        objGroupMemberDetail.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objGroupMemberDetail.Type = ds.Tables[0].Rows[i]["Type"].ToString();
                        objGroupMemberDetail.QuickBloxId = ds.Tables[0].Rows[i]["QBID"].ToString();
                        objGroupMemberDetail.Email = ds.Tables[0].Rows[i]["EmailId"].ToString();
                        objGroupMemberDetail.MemberName = ds.Tables[0].Rows[i]["Name"].ToString();

                        if (ds.Tables[0].Rows[i]["Permission"].ToString() == "1") { objGroupMemberDetail.PermissionType = "Read"; }
                        if (ds.Tables[0].Rows[i]["Permission"].ToString() == "2") { objGroupMemberDetail.PermissionType = "Read/Write"; }
                        if (ds.Tables[0].Rows[i]["Permission"].ToString() == "3" || ds.Tables[0].Rows[i]["Permission"].ToString() == "0") { objGroupMemberDetail.PermissionType = "Read/Write/Attachment"; }

                        objGroupMemberDetail.Permission = Convert.ToInt32(ds.Tables[0].Rows[i]["Permission"].ToString());
                        objGroupMemberDetailList.Add(objGroupMemberDetail);
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetGroupMemberListWithPermissionAndRole";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupMemberDetailList;
        }

        public string SetGroupChatMemberPermission(string ChattingGroupId, string ChattingGroupMemberId, string Permission, string LoginUserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "SetGroupChatMemberPermission",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    Convert.ToInt32(ChattingGroupMemberId),
                                                    Convert.ToInt32(Permission),
                                                    Guid.Parse(LoginUserId)
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
                objErrorlog.Methodname = "SetGroupChatMemberPermission";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        public string GetGroupChatMemberPermission(string ChattingGroupId, string LoginUserId)
        {
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetGroupChatMemberPermission",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    Guid.Parse(LoginUserId)
                                                    );
                if (ds.Tables.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["Permission"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetGroupChatMemberPermission";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        #endregion


        public List<Chatting> GetChatGroupListByOfficeIdForUser(string UserId, string GroupTypeId, string OfficeId)
        {
            List<Chatting> listChattingGroup = new List<Chatting>();
            Chatting objChattingGroup = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetChatGroupListByOfficeIdForUser", Guid.Parse(UserId), Convert.ToInt32(GroupTypeId), Convert.ToInt32(OfficeId));
                if (ds != null && ds.Tables.Count > 0)
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objChattingGroup = new Chatting();
                        objChattingGroup.DialogId = item["DialogId"].ToString();
                        objChattingGroup.GroupName = Convert.ToString(item["GroupName"]);
                        objChattingGroup.ChattingGroupId = Convert.ToInt32(item["ChattingGroupId"]);
                        objChattingGroup.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        objChattingGroup.OfficeName = item["OfficeName"].ToString();
                        objChattingGroup.GroupTypeId = Convert.ToInt32(item["GroupTypeId"]);
                        listChattingGroup.Add(objChattingGroup);
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetChatGroupListByOfficeIdForUser";
                InsertErrorLog(objErrorlog);
            }
            return listChattingGroup;
        }


        public List<ScheduleInfo> GetUnassignedSchedulerList(string ChattingGroupId)
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetUnassignedSchedulerList"
                                                        , Convert.ToInt32(ChattingGroupId));
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
                objErrorlog.Methodname = "GetUnassignedSchedulerList";
                string result = InsertErrorLog(objErrorlog);
            }
            return SchedulerList;
        }

        public List<ChattingGroupMember> GetAssignedSchedulerListGroupWise(string ChattingGroupId)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            string result = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAssignedSchedulerListGroupWise",
                                                  Convert.ToInt32(ChattingGroupId));
                if (ds.Tables[0].Rows.Count == 0)
                    result = "0";
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objGroupMemberDetail = new ChattingGroupMember();
                        objGroupMemberDetail.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objGroupMemberDetail.MemberName = ds.Tables[0].Rows[i]["Name"].ToString();
                        objGroupMemberDetail.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objGroupMemberDetail.QuickBloxId = ds.Tables[0].Rows[i]["QuickBloxId"].ToString();
                        objGroupMemberDetailList.Add(objGroupMemberDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAssignedSchedulerListGroupWise";
                result = InsertErrorLog(objErrorlog);
            }
            return objGroupMemberDetailList;
        }



        public string AddMemberIntoGroup(string ChattingGroupId, string QuickBloxId,string LoginUserId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "AddMemberIntoGroup_V1",
                                                    Convert.ToInt32(ChattingGroupId),
                                                    Convert.ToInt32(QuickBloxId),
                                                    Guid.Parse(LoginUserId));
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
                objErrorlog.Methodname = "AddMemberIntoGroup";
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }





        public List<Chatting> GetChatGroupListForDeleteManually(string UserId, string GroupTypeId)
        {
            List<Chatting> listChattingGroup = new List<Chatting>();
            Chatting objChattingGroup = new Chatting();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetChatGroupListForDeleteManually", Guid.Parse(UserId), Convert.ToInt32(GroupTypeId));
                if (ds != null && ds.Tables.Count > 0)
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objChattingGroup = new Chatting();
                        objChattingGroup.DialogId = item["DialogId"].ToString();
                        objChattingGroup.GroupName = Convert.ToString(item["GroupName"]);
                        objChattingGroup.ChattingGroupId = Convert.ToInt32(item["ChattingGroupId"]);
                        objChattingGroup.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        //   objChattingGroup.OfficeName = item["OfficeName"].ToString();
                        objChattingGroup.GroupTypeId = Convert.ToInt32(item["MasterChattingGroupTypeID"]);
                        listChattingGroup.Add(objChattingGroup);
                    }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetChatGroupListForDeleteManually";
                InsertErrorLog(objErrorlog);
            }
            return listChattingGroup;
        }

        public List<PatientChatList> GetPatientRoomGroupList(PatientChatModel PatientChatModel,string LogInUserId, string OrganisationId)
        {
            List<PatientChatList> ListChatting = new List<PatientChatList>();


            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetPatientRoomGroupList",
                                                     Guid.Parse(LogInUserId),
                                                     PatientChatModel.QBGroupDialogIds,
                                                     PatientChatModel.GroupTypeID,
                                                     Convert.ToInt32(OrganisationId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientChatList objChatting = new PatientChatList();
                        objChatting.ChattingGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["ChattingGroupId"]);
                        objChatting.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                        objChatting.DialogId = ds.Tables[0].Rows[i]["DialogId"].ToString();
                        objChatting.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"]);
                        objChatting.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        objChatting.GroupAdminUserId = ds.Tables[0].Rows[i]["GroupAdminUserId"].ToString();
                        objChatting.IsGroupAdmin = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsGroupAdmin"]);
                        objChatting.IsOfficeGroup = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsOfficeGroup"]);
                        objChatting.GroupTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["MasterChattingGroupTypeID"]);
                        objChatting.GroupSubject = Convert.ToString(ds.Tables[0].Rows[i]["GroupSubject"]);
                       

                        ListChatting.Add(objChatting);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetChattingListPatientGroupWise";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListChatting;
        }


        public ChattingsList GetOneToOneChatList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string OfficeId, string OrganisationId)
        {

            ChattingsList ListChatting = new ChattingsList();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetOneToOneChatList",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOneToOneChatList",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        Convert.ToInt32(OfficeId),
                                                        Convert.ToInt32(OrganisationId));

                ListChatting.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    List<Chatting> ChattingList = new List<Chatting>();

                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        Chatting objChatting = new Chatting();
                        objChatting.UserId = item["UserId"].ToString();
                        objChatting.NurseId = Convert.ToInt32(item["Id"]);
                        objChatting.CareGiverName = item["Name"].ToString();
                        objChatting.Role = item["Role"].ToString();
                        objChatting.QuickBloxDialogId = item["DialogId"].ToString();

                        var ProfileImage = item["ProfileImage"].ToString();

                        if (ProfileImage != null)
                        {
                            objChatting.CaregiverProfileImage = CareGiverProfileImagesPath + ProfileImage;
                        }
                        else
                        {
                            objChatting.CaregiverProfileImage = ConfigurationManager.AppSettings["DefaultCaregiverProfile"].ToString();
                        }
                        //
                        objChatting.OfficeId = Convert.ToInt32(item["OfficeId"].ToString());
                        objChatting.OfficeName = item["OfficeName"].ToString();
           

                        ChattingList.Add(objChatting);

                    }
        
                   // ListChatting.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                   // ListChatting.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListChatting.objChattingsList = ChattingList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetOneToOneChatList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListChatting;
        }



        public List<ScheduleInfo> GetALLSuperadminList()
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetALLSuperadminList");
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
                string result = InsertErrorLog(objErrorlog);
            }
            return SchedulerList;
        }


        #region CheckInOutBySuperAdmin

        public string CheckInOutBySuperAdmin(string PatientRequestId)
        {
            string result = "";
            try
            {
                int i = DataAccess.ExecuteNonQuery(Settings.CareGiverSuperAdminDatabase().ToString(), "CheckInOutBySuperAdmin",
                                                    PatientRequestId                                                   
                                                   // ,Guid.Parse(UserId)
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
                objErrorlog.Methodname = "CheckInOutBySuperAdmin";
                //objErrorlog.UserID = UserId;
                result = InsertErrorLog(objErrorlog);
            }
            return result;
        }

        #endregion


        //public AttendanceDetailsList GetAttendanceDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OfficeId, string search)
        //{

        //    AttendanceDetailsList ListAttendanceDetails = new AttendanceDetailsList();

        //    string TotalTravel = string.Empty;
        //    string CheckInTotalTime = string.Empty;
        //    string NewDateVar = string.Empty;
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceList_Vin",
        //                                                LogInUserId,
        //                                                Convert.ToInt32(pageNo),
        //                                                Convert.ToInt32(recordPerPage),
        //                                                sortfield,
        //                                                sortorder,
        //                                                Convert.ToInt32(OfficeId),
        //                                                search);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            List<AttendanceManagementDetails> AttendanceDetailsList = new List<AttendanceManagementDetails>();
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                AttendanceManagementDetails objAttendanceDetail = new AttendanceManagementDetails();
        //                objAttendanceDetail.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
        //                objAttendanceDetail.Name = ds.Tables[0].Rows[i]["Name"].ToString();
        //                objAttendanceDetail.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
        //                //uobj.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
        //                // objAttendanceDetail.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
        //                objAttendanceDetail.Date = ds.Tables[0].Rows[i]["Date"].ToString();
        //                objAttendanceDetail.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
        //                objAttendanceDetail.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
        //                objAttendanceDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
        //                objAttendanceDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

        //                // objAttendanceDetail.CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
        //                CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
        //                if (CheckInTotalTime == "")
        //                {
        //                    objAttendanceDetail.CheckInTotalTime = "NA";
        //                }
        //                else
        //                {
        //                    objAttendanceDetail.CheckInTotalTime = CheckInTotalTime;
        //                }
        //                objAttendanceDetail.RequestedDuration = (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("hh")) + " hrs " +
        //                    (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("mm")) + " mins";

        //                TotalTravel = ds.Tables[0].Rows[i]["TotalHours"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalMi"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalSe"].ToString();
        //                if (TotalTravel == " :  : ")
        //                {
        //                    // :  : 
        //                    objAttendanceDetail.TotalTravelTime = "No detail available";
        //                }
        //                else
        //                {
        //                    objAttendanceDetail.TotalTravelTime = TotalTravel;

        //                }


        //                objAttendanceDetail.DrivingStopLatitude = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
        //                objAttendanceDetail.DrivingStopLongitude = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

        //                AttendanceDetailsList.Add(objAttendanceDetail);
        //            }
        //            ListAttendanceDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
        //            ListAttendanceDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
        //            ListAttendanceDetails.AttendanceManagemenList = AttendanceDetailsList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetAttendanceDetailsList";
        //        string result = InsertErrorLog(objErrorlog);
        //    }
        //    return ListAttendanceDetails;
        //}


        public AttendanceDetailsList GetAttendanceDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OfficeId, string search, string OrganisationId)
        {

            AttendanceDetailsList ListAttendanceDetails = new AttendanceDetailsList();

            string TotalTravel = string.Empty;
            string CheckInTotalTime = string.Empty;
            string NewDateVar = string.Empty;

            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceList_Vin",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAttendanceList_Vin",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        Convert.ToInt32(OfficeId),
                                                        search,
                                                        Convert.ToInt32(OrganisationId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<AttendanceManagementDetails> AttendanceDetailsList = new List<AttendanceManagementDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        AttendanceManagementDetails objAttendanceDetail = new AttendanceManagementDetails();
                        objAttendanceDetail.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        objAttendanceDetail.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objAttendanceDetail.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        //uobj.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        // objAttendanceDetail.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        objAttendanceDetail.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objAttendanceDetail.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objAttendanceDetail.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objAttendanceDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objAttendanceDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["isManual"].ToString()))
                        {
                            objAttendanceDetail.IsMaual = Convert.ToInt32(ds.Tables[0].Rows[i]["isManual"]);
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["isEdited"].ToString()))
                        {
                            objAttendanceDetail.IsEdited = Convert.ToInt32(ds.Tables[0].Rows[i]["isEdited"]);
                        }


                           objAttendanceDetail.IsForceCheckIn = Convert.ToInt32(ds.Tables[0].Rows[i]["IsForceCheckin"]);
                        
                        // objAttendanceDetail.CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                        CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                        if (CheckInTotalTime == "")
                        {
                            objAttendanceDetail.CheckInTotalTime = "NA";
                        }
                        else
                        {
                            objAttendanceDetail.CheckInTotalTime = CheckInTotalTime;
                        }
                        objAttendanceDetail.RequestedDuration = (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("hh")) + " hrs " +
                            (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("mm")) + " mins";

                        TotalTravel = ds.Tables[0].Rows[i]["TotalHours"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalMi"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalSe"].ToString();
                        if (TotalTravel == " :  : ")
                        {
                            // :  : 
                            objAttendanceDetail.TotalTravelTime = "No detail available";
                        }
                        else
                        {
                            objAttendanceDetail.TotalTravelTime = TotalTravel;

                        }


                        objAttendanceDetail.DrivingStopLatitude = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                        objAttendanceDetail.DrivingStopLongitude = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                        AttendanceDetailsList.Add(objAttendanceDetail);
                    }
                    ListAttendanceDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListAttendanceDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListAttendanceDetails.AttendanceManagemenList = AttendanceDetailsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAttendanceDetailsList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListAttendanceDetails;
        }


        public CornaStatsDetailsList GetCornaStatsDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string OfficeId, string search)
        {

            CornaStatsDetailsList ListCornaStatsDetails = new CornaStatsDetailsList();

         
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllCoronaStatsList",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        Convert.ToInt32(OfficeId),
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<CornaStatsDetails> CornaStatsDetailsList = new List<CornaStatsDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        CornaStatsDetails objCornaStatsDetails = new CornaStatsDetails();
                        objCornaStatsDetails.BodySymptomsId = Convert.ToInt32(ds.Tables[0].Rows[i]["BodySymptomsId"].ToString());
                        // objCornaStatsDetails.NurseId= Convert.ToInt32(ds.Tables[0].Rows[i][""].ToString());
                        objCornaStatsDetails.NurseName = ds.Tables[0].Rows[i]["Name"].ToString();                 
                        objCornaStatsDetails.Datetimes = ds.Tables[0].Rows[i]["InsertDateTime"].ToString(); 
                        objCornaStatsDetails.BodyPain = ds.Tables[0].Rows[i]["BodyPain"].ToString();
                        objCornaStatsDetails.BodyTemperature = ds.Tables[0].Rows[i]["BodyTempreture"].ToString()+ "°F";
                        objCornaStatsDetails.BreathingDifficulty = ds.Tables[0].Rows[i]["Breathing"].ToString();
                        objCornaStatsDetails.Cough = ds.Tables[0].Rows[i]["Cough"].ToString();
                        objCornaStatsDetails.SoreThroat = ds.Tables[0].Rows[i]["Sorethroat"].ToString();
                        objCornaStatsDetails.ActiveStatus = ds.Tables[0].Rows[i]["ActiveStatus"].ToString();
                        objCornaStatsDetails.OfficeName= ds.Tables[0].Rows[i]["Office"].ToString();
                        objCornaStatsDetails.Username= ds.Tables[0].Rows[i]["UserName"].ToString();
                        objCornaStatsDetails.OfficeId= Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());

                        CornaStatsDetailsList.Add(objCornaStatsDetails);
                    }
                    ListCornaStatsDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListCornaStatsDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListCornaStatsDetails.VitalCornaStatsDetailsList= CornaStatsDetailsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAttendanceDetailsList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListCornaStatsDetails;
        }


        public SchedulePatientRequestList GetAllPasevaCustomerSchedulePatientRequest(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string IsAdmin, string LogInUserId, string FromDate, string ToDate, string FilterStatus, string FilterOffice)
        {
            SchedulePatientRequestList ListSchedulePatientRequest = new SchedulePatientRequestList();
            try
            {
                if (FromDate == "||" && ToDate == "||")
                {
                    FromDate = "1000-01-01";
                    ToDate = "1000-01-01";
                }
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetALLPasevaCustomerSchedulePatientRequest",
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search,
                                                        IsAdmin,
                                                        LogInUserId,
                                                         Convert.ToDateTime(FromDate),
                                                         Convert.ToDateTime(ToDate),
                                                        FilterStatus,
                                                        FilterOffice);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    List<SchedulePatientRequest> SchedulePatientRequestList = new List<SchedulePatientRequest>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        SchedulePatientRequest objSchedulePatientRequest = new SchedulePatientRequest();
                        objSchedulePatientRequest.PatientRequestId = Convert.ToInt32(ds.Tables[1].Rows[i]["PatientRequestId"]);
                        objSchedulePatientRequest.SchedulerName = ds.Tables[1].Rows[i]["SchedulerName"].ToString();
                        objSchedulePatientRequest.PatientName = ds.Tables[1].Rows[i]["PatientName"].ToString();
                        objSchedulePatientRequest.Address = ds.Tables[1].Rows[i]["Address"].ToString();
                        objSchedulePatientRequest.ZipCode = ds.Tables[1].Rows[i]["ZipCode"].ToString();
                        objSchedulePatientRequest.MedicalId = ds.Tables[1].Rows[i]["MedicalId"].ToString();
                        objSchedulePatientRequest.Description = ds.Tables[1].Rows[i]["Description"].ToString();
                        objSchedulePatientRequest.InsertDateTime = DateTime.Parse(ds.Tables[1].Rows[i]["InsertDateTime"].ToString()).ToString("MM/dd/yyyy");
                        objSchedulePatientRequest.Date = DateTime.Parse(ds.Tables[1].Rows[i]["Date"].ToString()).ToString("MM/dd/yyyy");
                        objSchedulePatientRequest.FromTime = ds.Tables[1].Rows[i]["FromTime"].ToString();
                        objSchedulePatientRequest.ToTime = ds.Tables[1].Rows[i]["ToTime"].ToString();
                        objSchedulePatientRequest.CaregiverName = ds.Tables[1].Rows[i]["CaregiverName"].ToString();
                        objSchedulePatientRequest.OfficeName = ds.Tables[1].Rows[i]["Office"].ToString();
                        objSchedulePatientRequest.TotalCaregiversNotified = Convert.ToInt32(ds.Tables[1].Rows[i]["TotalCaregiversNotified"].ToString());
                        objSchedulePatientRequest.DrivingStartTime = ds.Tables[1].Rows[i]["DrivingStartTime"].ToString();
                        objSchedulePatientRequest.CheckInTime = ds.Tables[1].Rows[i]["CheckInDateTime"].ToString();
                        objSchedulePatientRequest.CheckOutTime = ds.Tables[1].Rows[i]["CheckOutDateTime"].ToString();
                        objSchedulePatientRequest.Miles = ds.Tables[1].Rows[i]["DrivingTotalDistance"].ToString();
                        objSchedulePatientRequest.NurseId = Convert.ToInt32(ds.Tables[1].Rows[i]["NurseId"]);
                        var PatientSignaturePath = ConfigurationManager.AppSettings["CareGiverSignature"].ToString();
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["PatientSignature"].ToString()))
                        {
                            objSchedulePatientRequest.PatientSignature = PatientSignaturePath + ds.Tables[1].Rows[i]["PatientSignature"].ToString();
                        }
                        var Iscancel = ds.Tables[1].Rows[i]["IsCancelled"].ToString();
                        if (Iscancel == "False")
                        {
                            objSchedulePatientRequest.IsCancelled = false;
                        }
                        else
                        {
                            objSchedulePatientRequest.IsCancelled = true;
                        }

                        objSchedulePatientRequest.Status = ds.Tables[1].Rows[i]["Status"].ToString();
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezoneId"].ToString()))
                        {
                            objSchedulePatientRequest.TimezoneId = ds.Tables[1].Rows[i]["TimezoneId"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezoneOffset"].ToString()))
                        {
                            objSchedulePatientRequest.TimezoneOffset = Convert.ToInt32(ds.Tables[1].Rows[i]["TimezoneOffset"]);
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TimezonePostfix"].ToString()))
                        {
                            objSchedulePatientRequest.TimezonePostfix = ds.Tables[1].Rows[i]["TimezonePostfix"].ToString();
                        }

                        objSchedulePatientRequest.CheckInLatLong = ds.Tables[1].Rows[i]["CheckInLatLong"].ToString();
                        objSchedulePatientRequest.CheckOutLatLong = ds.Tables[1].Rows[i]["CheckOutLatLong"].ToString();
                        try
                        {
                            objSchedulePatientRequest.CheckInAddress = getAddressGoogleAPI(ds.Tables[1].Rows[i]["CheckInLatLong"].ToString());
                            objSchedulePatientRequest.CheckOutAddress = getAddressGoogleAPI(ds.Tables[1].Rows[i]["CheckOutLatLong"].ToString());
                        }
                        catch (Exception e)
                        { }

                        SchedulePatientRequestList.Add(objSchedulePatientRequest);
                    }

                    ListSchedulePatientRequest.TotalNumberofRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    ListSchedulePatientRequest.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListSchedulePatientRequest.SchedulePatientRequestsList = SchedulePatientRequestList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetALLSchedulePatientRequest";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListSchedulePatientRequest;
        }



        public ReportsDetailsList AGetAttendanceDetailsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string FilterNurseId, string search)
        {

            ReportsDetailsList ListAttendanceDetails = new ReportsDetailsList();

            string TotalTravel = string.Empty;
            string CheckInTotalTime = string.Empty;
            string NewDateVar = string.Empty;
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceList_vinnew",
                                                        LogInUserId,
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        Convert.ToInt32(FilterNurseId),

                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<ReportsManagementDetails> AttendanceDetailsList = new List<ReportsManagementDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        ReportsManagementDetails objAttendanceDetail = new ReportsManagementDetails();
                        objAttendanceDetail.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
                        objAttendanceDetail.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objAttendanceDetail.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        //uobj.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
                        // objAttendanceDetail.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        objAttendanceDetail.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                        objAttendanceDetail.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
                        objAttendanceDetail.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
                        objAttendanceDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objAttendanceDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

                        // objAttendanceDetail.CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                        CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
                        if (CheckInTotalTime == "")
                        {
                            objAttendanceDetail.CheckInTotalTime = "NA";
                        }
                        else
                        {
                            objAttendanceDetail.CheckInTotalTime = CheckInTotalTime;
                        }
                        objAttendanceDetail.RequestedDuration = (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("hh")) + " hrs " +
                            (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("mm")) + " mins";

                        TotalTravel = ds.Tables[0].Rows[i]["TotalHours"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalMi"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalSe"].ToString();
                        if (TotalTravel == " :  : ")
                        {
                            // :  :
                            objAttendanceDetail.TotalTravelTime = "No detail available";
                        }
                        else
                        {
                            objAttendanceDetail.TotalTravelTime = TotalTravel;

                        }


                        objAttendanceDetail.DrivingStopLatitude = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
                        objAttendanceDetail.DrivingStopLongitude = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

                        AttendanceDetailsList.Add(objAttendanceDetail);
                    }
                    ListAttendanceDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListAttendanceDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListAttendanceDetails.AttendanceManagemenList = AttendanceDetailsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAttendanceDetailsList";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListAttendanceDetails;
        }





        //public ReportsDetailsList GetAllAttendanceportsOfCaregiver(string pageNo, string recordPerPage, string sortfield, string sortorder, string search, string IsAdmin, string LogInUserId, string FromDate, string ToDate, string Filtercaregiver)
        //{

        //    ReportsDetailsList ListAttendanceDetails = new ReportsDetailsList();

        //    string TotalTravel = string.Empty;
        //    string CheckInTotalTime = string.Empty;
        //    string NewDateVar = string.Empty;
        //    try
        //    {
        //        if (FromDate == "||" && ToDate == "||")
        //        {
        //            FromDate = "1000-01-01";
        //            ToDate = "1000-01-01";
        //        }
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAttendanceList_vinnTestingData",
        //                                               Convert.ToInt32(pageNo),
        //                                               Convert.ToInt32(recordPerPage),
        //                                               sortfield,
        //                                               sortorder,
        //                                               search,
        //                                               IsAdmin,
        //                                               LogInUserId,
        //                                                Convert.ToDateTime(FromDate),
        //                                                Convert.ToDateTime(ToDate),
        //                                               Filtercaregiver);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            List<ReportsManagementDetails> AttendanceDetailsList = new List<ReportsManagementDetails>();
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                ReportsManagementDetails objAttendanceDetail = new ReportsManagementDetails();
        //                objAttendanceDetail.PatientRequestId = ds.Tables[0].Rows[i]["PatientRequestId"].ToString();
        //                objAttendanceDetail.Name = ds.Tables[0].Rows[i]["Name"].ToString();
        //                objAttendanceDetail.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
        //                //uobj.NurseId = Convert.ToInt32(ds.Tables[0].Rows[i]["NurseId"].ToString());
        //                // objAttendanceDetail.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
        //                objAttendanceDetail.Date = ds.Tables[0].Rows[i]["Date"].ToString();
        //                objAttendanceDetail.FromTime = ds.Tables[0].Rows[i]["FromTime"].ToString();
        //                objAttendanceDetail.ToTime = ds.Tables[0].Rows[i]["ToTime"].ToString();
        //                objAttendanceDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
        //                objAttendanceDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

        //                // objAttendanceDetail.CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
        //                CheckInTotalTime = ds.Tables[0].Rows[i]["CheckInTotalTime"].ToString();
        //                if (CheckInTotalTime == "")
        //                {
        //                    objAttendanceDetail.CheckInTotalTime = "NA";
        //                }
        //                else
        //                {
        //                    objAttendanceDetail.CheckInTotalTime = CheckInTotalTime;
        //                }
        //                objAttendanceDetail.RequestedDuration = (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("hh")) + " hrs " +
        //                    (Convert.ToDateTime((Convert.ToDateTime(objAttendanceDetail.ToTime) - Convert.ToDateTime(objAttendanceDetail.FromTime)).ToString()).ToString("mm")) + " mins";

        //                TotalTravel = ds.Tables[0].Rows[i]["TotalHours"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalMi"].ToString() + " : " + ds.Tables[0].Rows[i]["TotalSe"].ToString();
        //                if (TotalTravel == " :  : ")
        //                {
        //                    // :  :
        //                    objAttendanceDetail.TotalTravelTime = "No detail available";
        //                }
        //                else
        //                {
        //                    objAttendanceDetail.TotalTravelTime = TotalTravel;

        //                }


        //                objAttendanceDetail.DrivingStopLatitude = ds.Tables[0].Rows[i]["DrivingStopLatitude"].ToString();
        //                objAttendanceDetail.DrivingStopLongitude = ds.Tables[0].Rows[i]["DrivingStopLongitude"].ToString();

        //                AttendanceDetailsList.Add(objAttendanceDetail);
        //            }
        //            ListAttendanceDetails.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
        //            ListAttendanceDetails.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
        //            ListAttendanceDetails.AttendanceManagemenList = AttendanceDetailsList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetAttendanceDetailsList";
        //        string result = InsertErrorLog(objErrorlog);
        //    }
        //    return ListAttendanceDetails;
        //}


        //#region Organisation Based Data
        //public OrganisationsList GetAllOrganisationsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search)
        //{
        //    OrganisationsList ListOrganisation = new OrganisationsList();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllOrganisations",
        //                                                Guid.Parse(LogInUserId),
        //                                                Convert.ToInt32(pageNo),
        //                                                Convert.ToInt32(recordPerPage),
        //                                                sortfield,
        //                                                sortorder,
        //                                                search);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            List<Organisation> OrganisationList = new List<Organisation>();
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                Organisation objOrganisation = new Organisation();
        //                objOrganisation.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[i]["OrganisationId"].ToString());
        //                objOrganisation.OrganisationAdminUserId = ds.Tables[0].Rows[i]["OrganisationAdminUserId"].ToString();
        //                objOrganisation.OrganisationAdminName = ds.Tables[0].Rows[i]["AdminName"].ToString();
        //                objOrganisation.OrganisationName = ds.Tables[0].Rows[i]["OrganisationName"].ToString();
        //                objOrganisation.Street = ds.Tables[0].Rows[i]["Street"].ToString();
        //                objOrganisation.City = ds.Tables[0].Rows[i]["City"].ToString();
        //                objOrganisation.State = ds.Tables[0].Rows[i]["State"].ToString();
        //                objOrganisation.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
        //                objOrganisation.InsertDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["InsertDateTime"]).ToString("dd MMM yyyy hh:mm tt");
        //                objOrganisation.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
        //                objOrganisation.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();
        //                objOrganisation.AssignedZipcodes = ds.Tables[0].Rows[i]["AssignedZipcodes"].ToString();


        //                OrganisationList.Add(objOrganisation);
        //            }

        //            ListOrganisation.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
        //            ListOrganisation.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
        //            ListOrganisation.OrganisationList = OrganisationList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetAllOffices";
        //        string result = InsertErrorLog(objErrorlog);
        //    }
        //    return ListOrganisation;
        //}
        //#endregion

        //#region GetOrganisationDropDownList
        //public static OrganisationsList GetOrganisationDropDownList(string LogInUserId)
        //{
        //    OrganisationsList ListOrganisation = new OrganisationsList();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAvailableOffices",
        //                                                Guid.Parse(LogInUserId));


        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {


        //            List<Organisation> OrganisationsList = new List<Organisation>();


        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                Organisation objOrganisation = new Organisation();
        //                objOrganisation.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
        //                objOrganisation.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
        //                OrganisationsList.Add(objOrganisation);
        //            }



        //            ListOrganisation.OrganisationList = OrganisationsList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetOfficeDropDownList";
        //        //string result = InsertErrorLog(objErrorlog);
        //    }
        //    return ListOrganisation;
        //}
        //#endregion

        //#region AddOrganisation
        //public Organisation AddOrganisation(Organisation Organisation)
        //{
        //    //string result = "";
        //    Organisation objOrganisation = new Organisation();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddOrganisation",
        //                                            Organisation.OrganisationId,
        //                                            Organisation.OrganisationName,
        //                                            Organisation.OrganisationAdminUserId,
        //                                            Organisation.Street,
        //                                            Organisation.City,
        //                                            Organisation.State,
        //                                            Organisation.ZipCode,
        //                                            Organisation.Latitude,
        //                                            Organisation.Longitude,
        //                                            Guid.Parse(Organisation.InsertUserId),
        //                                            Organisation.TimezoneId,
        //                                            Organisation.TimezoneOffset,
        //                                            Organisation.TimezonePostfix

        //                                            );

        //        //if (i > 0)
        //        //{
        //        //    result = "Success";
        //        //}

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {

        //            objOrganisation.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"]);
        //            objOrganisation.OfficeName = ds.Tables[0].Rows[0]["OrganisationName"].ToString();
        //            objOrganisation.OrganisationAdminUserId = ds.Tables[0].Rows[0]["OrganisationAdminUserId"].ToString();
        //            //objOrganisation.AdminQuickBloxId = ds.Tables[0].Rows[0]["AdminQuickBloxId"].ToString();
        //            objOrganisation.Result = ds.Tables[0].Rows[0]["Result"].ToString();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "AddOrganisation";
        //        objErrorlog.UserID = Organisation.InsertUserId;
        //        objOrganisation.Result = InsertErrorLog(objErrorlog);
        //    }
        //    return objOrganisation;
        //}

        //public List<Admin> GetAllOrganisationAdminName()
        //{
        //    List<Admin> AdminNameList = new List<Admin>();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAdminName");

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                Admin objAdmin = new Admin();
        //                objAdmin.AdminId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdminId"]);
        //                objAdmin.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
        //                objAdmin.Name = ds.Tables[0].Rows[i]["Name"].ToString();

        //                AdminNameList.Add(objAdmin);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetAllAdminName";
        //        string result = InsertErrorLog(objErrorlog);
        //    }
        //    return AdminNameList;
        //}


        //#endregion


        //public List<Organisation> GetAllAvailableOrganisationsList(string LoginUserId)
        //{
        //    List<Organisation> lstOrganisation = new List<Organisation>();
        //    try
        //    {
        //        DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAvailableOrganisations", Guid.Parse(LoginUserId));

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //           // List<Organisation> OfficeList = new List<Organisation>();
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                Organisation objOrganisation = new Organisation();
        //                objOrganisation.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[i]["OrganisationId"].ToString());
        //                objOrganisation.OrganisationName = ds.Tables[0].Rows[i]["OrganisationName"].ToString();

        //                lstOrganisation.Add(objOrganisation);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog objErrorlog = new ErrorLog();
        //        objErrorlog.Errormessage = ex.Message;
        //        objErrorlog.StackTrace = ex.StackTrace;
        //        objErrorlog.Pagename = "CareGiverLiteService";
        //        objErrorlog.Methodname = "GetAllAvailableOrganisationsList";
        //        string result = InsertErrorLog(objErrorlog);
        //    }
        //    return lstOrganisation;
        //}


        #region Organisation Based Data
        public OrganisationsList GetAllOrganisationsList(string LogInUserId, string pageNo, string recordPerPage, string sortfield, string sortorder, string search)
        {
            OrganisationsList ListOrganisation = new OrganisationsList();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllOrganisations",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllOrganisations",
                                                       Guid.Parse(LogInUserId),
                                                        Convert.ToInt32(pageNo),
                                                        Convert.ToInt32(recordPerPage),
                                                        sortfield,
                                                        sortorder,
                                                        search);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Organisation> OrganisationList = new List<Organisation>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Organisation objOrganisation = new Organisation();
                        objOrganisation.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[i]["OrganisationId"].ToString());
                        objOrganisation.OrganisationAdminUserId = ds.Tables[0].Rows[i]["OrganisationAdminUserId"].ToString();
                        objOrganisation.OrganisationAdminName = ds.Tables[0].Rows[i]["AdminName"].ToString();
                        objOrganisation.OrganisationName = ds.Tables[0].Rows[i]["OrganisationName"].ToString();
                        objOrganisation.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objOrganisation.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objOrganisation.State = ds.Tables[0].Rows[i]["State"].ToString();
                        objOrganisation.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objOrganisation.InsertDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["InsertDateTime"]).ToString("dd MMM yyyy hh:mm tt");
                        objOrganisation.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objOrganisation.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();
                        objOrganisation.AssignedZipcodes = ds.Tables[0].Rows[i]["AssignedZipcodes"].ToString();


                        OrganisationList.Add(objOrganisation);
                    }

                    ListOrganisation.TotalNumberofRecord = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    ListOrganisation.FilteredRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    ListOrganisation.OrganisationList = OrganisationList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllOffices";
                string result = InsertErrorLog(objErrorlog);
            }
            return ListOrganisation;
        }
        #endregion

        #region GetOrganisationDropDownList
        public static OrganisationsList GetOrganisationDropDownList(string LogInUserId)
        {
            OrganisationsList ListOrganisation = new OrganisationsList();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAvailableOffices",
                                                        Guid.Parse(LogInUserId));


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {


                    List<Organisation> OrganisationsList = new List<Organisation>();


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Organisation objOrganisation = new Organisation();
                        objOrganisation.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[i]["OfficeId"].ToString());
                        objOrganisation.OfficeName = ds.Tables[0].Rows[i]["OfficeName"].ToString();
                        OrganisationsList.Add(objOrganisation);
                    }

                    ListOrganisation.OrganisationList = OrganisationsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetOfficeDropDownList";
                //string result = InsertErrorLog(objErrorlog);
            }
            return ListOrganisation;
        }
        #endregion

        #region AddOrganisation
        public Organisation AddOrganisation(Organisation Organisation)
        {
            //string result = "";
            Organisation objOrganisation = new Organisation();
            try
            {
                // DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "AddOrganisation",
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_AddOrganisation",
                                                     Organisation.OrganisationId,
                                                    Organisation.OrganisationName,
                                                    Organisation.OrganisationAdminUserId,
                                                    Organisation.Street,
                                                    Organisation.City,
                                                    Organisation.State,
                                                    Organisation.ZipCode,
                                                    Organisation.Latitude,
                                                    Organisation.Longitude,
                                                    Guid.Parse(Organisation.InsertUserId),
                                                    Organisation.TimezoneId,
                                                    Organisation.TimezoneOffset,
                                                    Organisation.TimezonePostfix
                                                    );


                //if (i > 0)
                //{
                //    result = "Success";
                //}


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    objOrganisation.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"]);
                    objOrganisation.OfficeName = ds.Tables[0].Rows[0]["OrganisationName"].ToString();
                    objOrganisation.OrganisationAdminUserId = ds.Tables[0].Rows[0]["OrganisationAdminUserId"].ToString();
                    //objOrganisation.AdminQuickBloxId = ds.Tables[0].Rows[0]["AdminQuickBloxId"].ToString();
                    objOrganisation.Result = ds.Tables[0].Rows[0]["Result"].ToString();

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "AddOrganisation";
                objErrorlog.UserID = Organisation.InsertUserId;
                objOrganisation.Result = InsertErrorLog(objErrorlog);
            }
            return objOrganisation;
        }


        public List<Admin> GetAllOrganisationAdminName(string OrganisationId)
        {
            List<Admin> AdminNameList = new List<Admin>();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAdminName", OrganisationId);

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAdminName", OrganisationId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Admin objAdmin = new Admin();
                        objAdmin.AdminId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdminId"]);
                        objAdmin.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                        objAdmin.Name = ds.Tables[0].Rows[i]["Name"].ToString();

                        AdminNameList.Add(objAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAdminName";
                string result = InsertErrorLog(objErrorlog);
            }
            return AdminNameList;
        }
        #endregion


        public List<Organisation> GetAllAvailableOrganisationsList(string LoginUserId, string OrganisationId)
        {
            List<Organisation> lstOrganisation = new List<Organisation>();
            try
            {
                //DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "GetAllAvailableOrganisations", Guid.Parse(LoginUserId));

                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetAllAvailableOrganisations", Guid.Parse(LoginUserId),Convert.ToInt32(OrganisationId));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // List<Organisation> OfficeList = new List<Organisation>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Organisation objOrganisation = new Organisation();
                        objOrganisation.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[i]["OrganisationId"].ToString());
                        objOrganisation.OrganisationName = ds.Tables[0].Rows[i]["OrganisationName"].ToString();

                        lstOrganisation.Add(objOrganisation);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetAllAvailableOrganisationsList";
                string result = InsertErrorLog(objErrorlog);
            }
            return lstOrganisation;
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

       
        public Organisation GetOrganisationDetailByOrganisationId(string OrganisationId)
        {

            Organisation Organisation = new Organisation();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(), "ORG_GetOrganisationDetailByOrganisationId", OrganisationId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Organisation.OrganisationId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationId"]);
                    Organisation.OrganisationAdminUserId = ds.Tables[0].Rows[0]["OrganisationAdminUserId"].ToString();
                    Organisation.OrganisationName = ds.Tables[0].Rows[0]["OrganisationName"].ToString();
                    Organisation.Street = ds.Tables[0].Rows[0]["Street"].ToString();
                    Organisation.City = ds.Tables[0].Rows[0]["City"].ToString();
                    Organisation.State = ds.Tables[0].Rows[0]["State"].ToString();
                    Organisation.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    Organisation.AdminEmail = ds.Tables[0].Rows[0]["AdminEmail"].ToString();
                   // Organisation.AdminQuickBloxId = ds.Tables[0].Rows[0]["AdminQuickbloxId"].ToString();

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "CareGiverLiteService";
                objErrorlog.Methodname = "GetOrganisationDetailByOrganisationId";
                string result = InsertErrorLog(objErrorlog);
            }
            return Organisation;


        }
    }
}


