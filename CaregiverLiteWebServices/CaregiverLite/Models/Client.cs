using CaregiverLiteWCF;
using DifferenzLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Models
{
    public class Client
    { 
        public int ClientId { get; set; }
          
        public string ClientName { get; set; }
        public string CRNumber { get; set; }

        public string MarketersName { get; set; }

        public string ClientRequestId { get; set; }


        public string Address { get; set; }
          
        public string Phone { get; set; }
          
        public string Gender { get; set; }
          
        public string Email { get; set; }
          
        public string Latitude { get; set; }
          
        public string Longitude { get; set; }
          
        public string ZipCode { get; set; }
          
        public string InsertUserId { get; set; }
          
        public string UpdateUserId { get; set; }
          
        public string TimezoneId { get; set; }
          
        public string TimezoneOffset { get; set; }
          
        public string TimezonePostfix { get; set; }
          
        public string Street { get; set; }
          
        public string City { get; set; }
          
        public string State { get; set; }

          
        public string UserName { get; set; }
          
        public string Password { get; set; }
          
        public string UserId { get; set; }

          
        public string DeviceType { get; set; }

          
        public string DeviceToken { get; set; }

          
        public string AccessToken { get; set; }

          
        public bool IsApprove { get; set; }

          
        public bool IsAvailable { get; set; }

          
        public string QuickbloxId { get; set; }


          
        public string AccessCode { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public string Date { get; set; }


        public string Active { get; set; }


          
        public string CreatedDate { get; set; }

          
        public bool AutoLogin { get; set; }
          
        public string ProfileImage { get; set; }
          
        public bool IsDeleted { get; set; }
          
        public bool CanAdminEdit { get; set; }

          
        public string Remarks { get; set; }
          
        public string CompanyName { get; set; }
        public string OfficeId { get; set; }

        public string OfficeName { get; set; }
        public IEnumerable<SelectListItem> OfficeSelectList { get; set; }



        public ClientRequest DBGetClientDetailByCRNNumber(string CRNNumber, string LoginUserId)
        {
            //Client objClientDetail = new Client();
            ClientRequest objClientDetail = new ClientRequest();

            string Marketers = "";
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(),
                                                         "GetClientRequestDetailByCRNNumber",
                                                         LoginUserId,
                                                         CRNNumber,
                                                         Marketers);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<Client> ClientList = new List<Client>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        objClientDetail.ClientId = Convert.ToInt32(ds.Tables[0].Rows[i]["Clientid"]);
                        objClientDetail.CRNumber = ds.Tables[0].Rows[i]["CRNumber"].ToString();
                        objClientDetail.ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString();
                        objClientDetail.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        objClientDetail.City = ds.Tables[0].Rows[i]["City"].ToString();
                        objClientDetail.Street = ds.Tables[0].Rows[i]["Street"].ToString();
                        objClientDetail.State = ds.Tables[0].Rows[i]["State"].ToString();
                        objClientDetail.ZipCode = ds.Tables[0].Rows[i]["ZipCode"].ToString();
                        objClientDetail.Phone = ds.Tables[0].Rows[i]["Phone"].ToString();
                        objClientDetail.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        objClientDetail.Latitude = ds.Tables[0].Rows[i]["Latitude"].ToString();
                        objClientDetail.Longitude = ds.Tables[0].Rows[i]["Longitude"].ToString();

                        objClientDetail.Company = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        int officeId = 0;
                        Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["OfficeId"]), out officeId);
                        objClientDetail.OfficeId = officeId.ToString();
                        objClientDetail.OfficeName = ds.Tables[0].Rows[i]["Office"].ToString();

                        List<MarketerDetailsModel> lstmarketer = DBGetMarekterListByOfficeId(officeId);

                        SelectList markleterSelectList = new SelectList(lstmarketer, "MarketersId", "MarketersName");

                        objClientDetail.marketersSelectList = markleterSelectList;


                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "ClientRequest";
                objErrorlog.Methodname = "DBGetClientDetailByCRNNumber";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string resError = ErrorLogService.InsertErrorLog(objErrorlog).Result;
            }
            return objClientDetail;
        }

        public List<MarketerDetailsModel> DBGetMarekterListByOfficeId(int officeId)
        {
            List<MarketerDetailsModel> LstMarketerDetail = new List<MarketerDetailsModel>();
            try
            {
                DataSet ds = DataAccess.ExecuteDataset(Settings.CareGiverSuperAdminDatabase().ToString(),
                                                         "GetMarketersListByOfficeId",
                                                         officeId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        MarketerDetailsModel objMarketerDetail = new MarketerDetailsModel();
                        objMarketerDetail.MarketersId = Convert.ToInt32(ds.Tables[0].Rows[i]["MarketersId"]);
                        objMarketerDetail.MarketersName = ds.Tables[0].Rows[i]["MarketersName"].ToString();

                        LstMarketerDetail.Add(objMarketerDetail);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorlog = new ErrorLog();
                objErrorlog.Errormessage = ex.Message;
                objErrorlog.StackTrace = ex.StackTrace;
                objErrorlog.Pagename = "ClientRequest";
                objErrorlog.Methodname = "DBGetMarekterListByOfficeId";
                ErrorLogServiceProxy ErrorLogService = new ErrorLogServiceProxy();
                string resError = ErrorLogService.InsertErrorLog(objErrorlog).Result;
            }
            return LstMarketerDetail;
        }

    }
    
    public class ClientDetailsList
    {
          
        public int TotalNumberofRecord { get; set; }

          
        public int FilteredRecord { get; set; }

          
        public List<Client> ClientList { get; set; }
    }




}
