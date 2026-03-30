using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaregiverLite.Models;
using CaregiverLiteWCF;
using System.Data;
using DifferenzLibrary;
using System.Web.Security;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using CaregiverLite.Action_Filters;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class InTakeController : Controller
    {
        // GET: InTake
        public ActionResult InTakePatientDetails()
        {
            MembershipUser user = Membership.GetUser();
            string LogInUserId = user.ProviderUserKey.ToString();
            DataTable LoginUserQBIDFromUserID = GetLoginUserQBID(LogInUserId);

            //Session["QBID"] = LoginUserQBIDFromUserID.Rows[0]["QuickBloxId"];
            //Session["Email"] = LoginUserQBIDFromUserID.Rows[0]["Email"];
            //Session["UserLoginID"] = LogInUserId;

            ViewBag.QBID = LoginUserQBIDFromUserID.Rows[0]["QuickBloxId"];
            ViewBag.Email = LoginUserQBIDFromUserID.Rows[0]["Email"];
            ViewBag.UserLoginID = LogInUserId;
            return View();
        }

        [HttpPost]
        public ActionResult AddPatientDetails(InTakePatientDetails objPatientDetails)
        {
            var PatientDetail = new PatientsDetail();
            string result = "";

            try
            {
                result = AddPatientsInTheInTake(objPatientDetails); //321
                return Json(result, JsonRequestBehavior.AllowGet);

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


        //public string AddPatientsInTheInTake(PatientsDetail PatientsDetail)
        //{
        //    string result = "";
        //    SqlConnection conn = null;
        //    SqlCommand cmd = null;
        //    string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;

        //    try
        //    {
        //        conn = new SqlConnection(connectionString);
        //        conn.Open();
        //        cmd = new SqlCommand("PR_Add_InTake_PatientDetail", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //     //   cmd.Parameters.Add("@PatientId", SqlDbType.Int).Value = PatientsDetail.PatientId;
        //        cmd.Parameters.Add("@PatientName", SqlDbType.NVarChar).Value = PatientsDetail.PatientName;
        //        cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = PatientsDetail.FirstName;
        //        cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = PatientsDetail.LastName;
        //        cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = PatientsDetail.PhoneNo;
        //        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = PatientsDetail.City;
        //        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = PatientsDetail.State;
        //        cmd.Parameters.Add("@PayerId", SqlDbType.NVarChar).Value = (object)PatientsDetail.PayerId ?? DBNull.Value;
        //        cmd.Parameters.Add("@DOB", SqlDbType.VarChar).Value = PatientsDetail.DateOfBirth;//(object)PatientsDetail.DateOfBirth ?? DBNull.Value;
        //        cmd.Parameters.Add("@OfficeId", SqlDbType.VarChar).Value = PatientsDetail.OfficeId;
        //        cmd.Parameters.Add("@referredby", SqlDbType.VarChar).Value = PatientsDetail.ReferredBy;
        //        cmd.Parameters.Add("@primarymd", SqlDbType.VarChar).Value = PatientsDetail.primarymd;
        //        cmd.Parameters.Add("@zipcode", SqlDbType.VarChar).Value = PatientsDetail.ZipCode;
        //        cmd.Parameters.Add("@patientaddress", SqlDbType.VarChar).Value = PatientsDetail.Address;
        //        cmd.Parameters.Add("@juridictioncode", SqlDbType.VarChar).Value = PatientsDetail.jurisdictioncode;
        //        cmd.Parameters.Add("@payerprogram", SqlDbType.VarChar).Value = PatientsDetail.payerprogram;
        //        cmd.Parameters.Add("@procedurecode", SqlDbType.VarChar).Value = PatientsDetail.procedurecode;

        //        int RowAffected = cmd.ExecuteNonQuery();
        //        result = RowAffected > 0 ? "success" : "failed";
        //    }
        //    catch(Exception ex)
        //    {

        //        result = "failed";
        //    }
        //    finally
        //    {  
        //        if (cmd != null)
        //            cmd.Dispose();

        //        if (conn != null)
        //        {
        //            if (conn.State == ConnectionState.Open)
        //                conn.Close();

        //            conn.Dispose();
        //        }
        //    }

        //    return result;
        //}

        public string AddPatientsInTheInTake(InTakePatientDetails objPatientDetails)
        {
            string result = "";
            string patientname = objPatientDetails.FirstName + " " + objPatientDetails.LastName;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;

            try
            {

                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Add_InTake_PatientDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PatientId", SqlDbType.VarChar).Value = objPatientDetails.InTake_PatientId;
                cmd.Parameters.Add("@PatientName", SqlDbType.NVarChar).Value = patientname;
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = objPatientDetails.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = objPatientDetails.LastName;
                cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = objPatientDetails.PhoneNo;
                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = objPatientDetails.City;
                cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = objPatientDetails.State;
                cmd.Parameters.Add("@PayerId", SqlDbType.NVarChar).Value = objPatientDetails.PayerId; //(object)PatientsDetail.PayerId ?? DBNull.Value;
                cmd.Parameters.Add("@DOB", SqlDbType.VarChar).Value = objPatientDetails.DateOfBirth;//(object)PatientsDetail.DateOfBirth ?? DBNull.Value;
                cmd.Parameters.Add("@OfficeId", SqlDbType.VarChar).Value = objPatientDetails.OfficeId;
                cmd.Parameters.Add("@referredby", SqlDbType.VarChar).Value = objPatientDetails.ReferredBy;
                cmd.Parameters.Add("@primarymd", SqlDbType.VarChar).Value = objPatientDetails.primarymd;
                cmd.Parameters.Add("@zipcode", SqlDbType.VarChar).Value = objPatientDetails.ZipCode;
                //   cmd.Parameters.Add("@patientaddress", SqlDbType.VarChar).Value = objPatientDetails.Address;
                cmd.Parameters.Add("@juridictioncode", SqlDbType.VarChar).Value = objPatientDetails.jurisdictioncode;
                cmd.Parameters.Add("@payerprogram", SqlDbType.VarChar).Value = objPatientDetails.payerprogram;
                cmd.Parameters.Add("@procedurecode", SqlDbType.VarChar).Value = objPatientDetails.procedurecode;
                cmd.Parameters.Add("@street", SqlDbType.VarChar).Value = objPatientDetails.street;
                cmd.Parameters.Add("@lat", SqlDbType.VarChar).Value = objPatientDetails.Latitide;
                cmd.Parameters.Add("@long", SqlDbType.VarChar).Value = objPatientDetails.Longitude;
                cmd.Parameters.Add("@TimeZoneId", SqlDbType.VarChar).Value = objPatientDetails.TimeZoneId;
                cmd.Parameters.Add("@TimezonePostfix", SqlDbType.VarChar).Value = objPatientDetails.TimezonePostfix;
                cmd.Parameters.Add("@TimezoneOffset", SqlDbType.VarChar).Value = objPatientDetails.TimezoneOffset;
                // cmd.Parameters.Add("@TimezoneOffset", SqlDbType.VarChar).Value = objPatientDetails.PatMedicalID;
                //int RowAffected = cmd.ExecuteNonQuery();
                //result = RowAffected > 0 ? "success" : "failed";

                object insertedId = cmd.ExecuteScalar();
                int intakeId;
                if (insertedId != null && int.TryParse(insertedId.ToString(), out intakeId))
                {
                    result = intakeId.ToString();
                }
                else
                {
                    result = "failed";
                }

                conn.Close();
            }
            catch (Exception ex)
            {

                result = "failed";
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Dispose();
                }
            }

            return result;
        }


        [HttpGet]
        public JsonResult CheckInTakePatientData() // providing the Intake patients list
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Get_InTake_PatientDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                return Json(msg, JsonRequestBehavior.AllowGet);
                // conn.Close();
            }
            catch (Exception ex)
            {
                // msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return Json(msg);
        }


        [HttpGet]
        public JsonResult InTakePatientChatListFrmChantMemberTbl() // providing the Intake patients list
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Get_InTake_PatientDetailFrmChattingGrpTbl", conn); //Session["QBID"]
                cmd.Parameters.Add("@QBID", SqlDbType.VarChar).Value = ViewBag.QBID.ToString();// Session["QBID"].ToString();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                return Json(msg, JsonRequestBehavior.AllowGet);
                // conn.Close();
            }
            catch (Exception ex)
            {
                // msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return Json(msg);
        }


        [HttpPost]
        public JsonResult RejectInTakePatient(int key)   //It is used for removing the intake patient Details
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Remove_InTake_PatientDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Intakepatientid", SqlDbType.Int).Value = key;
                int RowAffected = cmd.ExecuteNonQuery();
                msg = RowAffected > 0 ? "success" : "failed";
                // conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AcceptInTakePatientAndTransferIntoPatientModule(int key)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {

                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Remove_InTake_PatientDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Intakepatientid", SqlDbType.Int).Value = key;
                int RowAffected = cmd.ExecuteNonQuery();
                msg = RowAffected > 0 ? "success" : "failed";
                // conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateInTakePatientDetails(InTakePatientDetails InTakePatient)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Remove_InTake_PatientDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //  cmd.Parameters.Add("@Intakepatientid", SqlDbType.Int).Value = ;
                int RowAffected = cmd.ExecuteNonQuery();
                msg = RowAffected > 0 ? "success" : "failed";
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult updatePatientDetails(InTakePatientDetails objPatientDetails1)  ////It is used for update intake patient Details
        {
            string results = "";
            results = UpdatePatientDetails1(objPatientDetails1);
            return Json(results, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public string UpdatePatientDetails1(InTakePatientDetails objPatientDetails1)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Update_InTake_PatientDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Intakepatientid", SqlDbType.Int).Value = objPatientDetails1.InTakePatientid;
                //cmd.Parameters.Add("@PatientName", SqlDbType.NVarChar).Value = patientname;
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = objPatientDetails1.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = objPatientDetails1.LastName;
                cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = objPatientDetails1.PhoneNo;
                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = objPatientDetails1.City;
                cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = objPatientDetails1.State;
                cmd.Parameters.Add("@PayerId", SqlDbType.NVarChar).Value = objPatientDetails1.PayerId; //(object)PatientsDetail.PayerId ?? DBNull.Value;
                cmd.Parameters.Add("@DOB", SqlDbType.VarChar).Value = objPatientDetails1.DateOfBirth;//(object)PatientsDetail.DateOfBirth ?? DBNull.Value;
                cmd.Parameters.Add("@OfficeId", SqlDbType.VarChar).Value = objPatientDetails1.OfficeId;
                cmd.Parameters.Add("@referredby", SqlDbType.VarChar).Value = objPatientDetails1.ReferredBy;
                cmd.Parameters.Add("@primarymd", SqlDbType.VarChar).Value = objPatientDetails1.primarymd;
                cmd.Parameters.Add("@zipcode", SqlDbType.VarChar).Value = objPatientDetails1.ZipCode;
                // cmd.Parameters.Add("@patientaddress", SqlDbType.VarChar).Value = objPatientDetails1.Address;
                cmd.Parameters.Add("@juridictioncode", SqlDbType.VarChar).Value = objPatientDetails1.jurisdictioncode;
                cmd.Parameters.Add("@payerprogram", SqlDbType.VarChar).Value = objPatientDetails1.payerprogram;
                cmd.Parameters.Add("@procedurecode", SqlDbType.VarChar).Value = objPatientDetails1.procedurecode;
                cmd.Parameters.Add("@street", SqlDbType.VarChar).Value = objPatientDetails1.street; //PatientId
                cmd.Parameters.Add("@PatientId", SqlDbType.VarChar).Value = objPatientDetails1.PatMedicalID;
                cmd.Parameters.Add("@lat", SqlDbType.VarChar).Value = objPatientDetails1.Latitide;
                cmd.Parameters.Add("@long", SqlDbType.VarChar).Value = objPatientDetails1.Longitude;
                cmd.Parameters.Add("@TimeZoneId", SqlDbType.VarChar).Value = objPatientDetails1.TimeZoneId;
                cmd.Parameters.Add("@TimezonePostfix", SqlDbType.VarChar).Value = objPatientDetails1.TimezonePostfix;
                cmd.Parameters.Add("@TimezoneOffset", SqlDbType.VarChar).Value = objPatientDetails1.TimezoneOffset;
                int RowAffected = cmd.ExecuteNonQuery();
                msg = RowAffected > 0 ? "success" : "failed";
                conn.Close();

            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return msg;
        }


        [HttpPost]
        public ActionResult FetchIntakePatientDetail(int IntakePatientID)
        {
            string results = "";
            results = fetchPatDetails(IntakePatientID);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public string fetchPatDetails(int Id)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_fetch_Intake_PatientDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Intakepatientid", SqlDbType.Int).Value = Id;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return msg;
        }

        [HttpPost]
        public ActionResult Transfer_Intake_PatientDetails(int IntakePatientID)
        {
            string results = "";
            results = Transfer_PatientDetailsInto_Patients(IntakePatientID);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public string Transfer_PatientDetailsInto_Patients(int IntakePatientID)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Transferdata_Intake_ToPatientTable", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IntakePatientID", SqlDbType.Int).Value = IntakePatientID;
                //  SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                //msg = JsonConvert.SerializeObject(dt);
                int RowAffected = cmd.ExecuteNonQuery();
                msg = RowAffected > 0 ? "success" : "failed";
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return msg;
        }

        //public ActionResult IntakePatientChatPage()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult SavingDataIntoChattingGrouptbl(string dialogid, string grpname, string userid, string QBID, string Officename, string IntakePatientId)
        {
            string results = "";
            results = SavingDataIntoChattingGrouptbl1(dialogid, grpname, userid, QBID, Officename, IntakePatientId);
            return Json(results, JsonRequestBehavior.AllowGet);
        }


        public string SavingDataIntoChattingGrouptbl1(string dialogid, string grpname, string userid, string QBID, string Officename, string IntakePatientId)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_InTake_SaveChattingGroupDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Dialogid", SqlDbType.VarChar).Value = dialogid;
                cmd.Parameters.Add("@GroupName", SqlDbType.VarChar).Value = grpname;
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = userid;
                cmd.Parameters.Add("@InTakeFlg", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@Officename", SqlDbType.VarChar).Value = Officename; //1
                cmd.Parameters.Add("@IntakePatientId", SqlDbType.VarChar).Value = IntakePatientId;
                //  int RowAffected = cmd.ExecuteNonQuery();
                //  msg = RowAffected > 0 ? "success" : "failed";
                object result = cmd.ExecuteScalar();

                if (result != null)
                    msg = result.ToString(); // This is the inserted ChattingGroupId
                else
                    msg = "failed";
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return msg;
        }


        [HttpGet]
        public JsonResult AppendOfficeList()
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            MembershipUser user = Membership.GetUser();
            string LogInUserId = user.ProviderUserKey.ToString();

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("ORG_GetAllAvailableOffices", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AdminUserId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(LogInUserId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                conn.Close();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public DataTable GetLoginUserQBID(string userid)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("GetQuickBloxDetByUserId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userid;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //msg = JsonConvert.SerializeObject(dt);
                // conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }


        [HttpGet]
        public JsonResult AppendMemberList(string ChattingGrpId)
        {
            string msg = "";
            string dialogs = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            //DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_InTake_FetchMembersData", conn);
                cmd.Parameters.AddWithValue("ChattingGrpId", ChattingGrpId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                msg = JsonConvert.SerializeObject(ds.Tables[0]);
                dialogs = JsonConvert.SerializeObject(ds.Tables[1]);
                msg = msg + ";" + dialogs;
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                conn.Close();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult SaveSelectedMembers(string userId,string chattingGroupId)
        //{
        //    string results = "";
        //    results = SaveSelectedMembers1(userId, chattingGroupId);
        //    return Json(results, JsonRequestBehavior.AllowGet);
        //}

        public string AddLoginUserIntoChatRoom1(string userId, string chattingGroupId)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_InTake_AddMembersIntoGroup", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@ChattingGroupId", SqlDbType.VarChar).Value = chattingGroupId;
                int rows = cmd.ExecuteNonQuery();
                msg = rows > 0 ? "success" : "failed";
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return msg;
        }
        [HttpPost]
        public ActionResult SaveSelectedMembers(string[] userId, string chattingGroupId)
        {
            string result = SaveSelectedMembers1(userId, chattingGroupId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string SaveSelectedMembers1(string[] userIds, string chattingGroupId)
        {
            string msg = "";
            int totalInserted = 0;
            SqlConnection conn = null;
            SqlCommand cmd = null;

            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                foreach (string id in userIds)
                {
                    cmd = new SqlCommand("PR_InTake_AddMembersIntoGroup", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = id.Trim();
                    cmd.Parameters.Add("@ChattingGroupId", SqlDbType.VarChar).Value = chattingGroupId;

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        totalInserted++;
                }

                msg = totalInserted > 0 ? "success" : "failed";
            }
            catch (Exception)
            {
                msg = "failed";
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (conn != null && conn.State == ConnectionState.Open) conn.Close();
            }

            return msg;
        }


        [HttpPost]
        public ActionResult AddLoginUserIntoChatRoom(string chattingGroupId)
        {
            MembershipUser user = Membership.GetUser();
            string LogInUserId = user.ProviderUserKey.ToString();
            string result = AddLoginUserIntoChatRoom1(LogInUserId, chattingGroupId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FetchDialogIDAndQBID(int chattingGroupId)
        {
            string dialogId = FetchDialogID(chattingGroupId);
            string qbIdsJson = FetchDialogIDQBID(chattingGroupId);

            var result = new
            {
                DialogID = dialogId,
                QBIDs = JsonConvert.DeserializeObject<List<string>>(qbIdsJson)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string FetchDialogID(int chattingGroupId)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("GetDialogDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = chattingGroupId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                conn.Close();
            }
            // return Json(msg, JsonRequestBehavior.AllowGet);
            return msg;
        }


        public string FetchDialogIDQBID(int chattingGroupId)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("ORG_GetChattingGroupMemberByChattingGroupId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = chattingGroupId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    List<string> qbidList = new List<string>();

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["QBID"] != DBNull.Value)
                        {
                            qbidList.Add(row["QBID"].ToString());
                        }
                    }
                    msg = JsonConvert.SerializeObject(qbidList);
                }

                else
                {
                    msg = "[]";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                conn.Close();
            }
            //return Json(msg, JsonRequestBehavior.AllowGet);
            return msg;
        }


        [HttpGet]
        public JsonResult GetAllAddedMembersListByChattingGrpID(int chattingGroupId)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("ORG_GetChattingGroupMemberByChattingGroupId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = chattingGroupId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                conn.Close();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckPatientValidID(string patientid)
        {
            string results = "";
            results = CheckPatientValidID1(patientid);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public string CheckPatientValidID1(string patientid)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_InTake_CheckPatientIDExist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@medicalId", SqlDbType.VarChar).Value = patientid;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return msg;
        }


        [HttpPost]
        public JsonResult Get_IntakePatientRoomGroupList(string QBGroupDialogIds, int GroupType)
        {
            List<PatientChatList> ChattingsList = new List<PatientChatList>();
            MembershipUser user = Membership.GetUser();
            string LogInUserId = user.ProviderUserKey.ToString();
            var PatientChatModel = new PatientChatModel();

            PatientChatModel.QBGroupDialogIds = QBGroupDialogIds;
            PatientChatModel.GroupTypeID = GroupType;  // groupType = 1  for patient chat room
            ChattingsList = Get_IntakePatientRoomGroupList1(PatientChatModel, GroupType, LogInUserId, "0");
            return Json(ChattingsList, JsonRequestBehavior.AllowGet);
        }

        public List<PatientChatList> Get_IntakePatientRoomGroupList1(PatientChatModel PatientChatModel, int GroupType, string LogInUserId, string OrganisationId)
        {
            List<PatientChatList> ListChatting = new List<PatientChatList>();
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataSet ds = new DataSet();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("ORG_Get_Intake_PatientRoomGroupList", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LogInUserId", SqlDbType.VarChar).Value = LogInUserId;
                cmd.Parameters.Add("@QBGroupDialogIds", SqlDbType.VarChar).Value = PatientChatModel.QBGroupDialogIds;
                cmd.Parameters.Add("@GroupTypeID", SqlDbType.Int).Value = GroupType;
                cmd.Parameters.Add("@OrganisationId", SqlDbType.Int).Value = OrganisationId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);


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
                        // objChatting.GroupTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["MasterChattingGroupTypeID"]);
                        objChatting.GroupSubject = Convert.ToString(ds.Tables[0].Rows[i]["GroupSubject"]);


                        ListChatting.Add(objChatting);
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                conn.Close();
            }
            return ListChatting;
        }


        [HttpPost]
        public ActionResult fetchBydefaultMembersToAddInChatRoom()
        {
            string results = "";
            results = fetchBydefaultMembersToAddInChatRoom1();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public string fetchBydefaultMembersToAddInChatRoom1()
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Intake_ByDefaultAddedMembers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            finally
            {
                conn.Close();
            }
            return msg;
        }


    }
}