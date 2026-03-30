using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CaregiverLite.Models
{
    public class OfficeModel
    {

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Office), ErrorMessageResourceName = "RequiredMsgOffice")]

        public int OfficeId { get; set; }

        [Required(ErrorMessage = "Please Enter Office Name")]
        [DisplayName("OfficeName")]
        public string OfficeName { get; set; }


      //  [Required(ErrorMessage = "Please Enter Office Name")]
        [DisplayName("AdminName")]
        public string AdminName { get; set; }

      //  [Required(ErrorMessage = "Please Select Admin Name")]
        public string UserId { get; set; }

     //   [Required(ErrorMessage = "Please Select Admin Name")]
        public string AdminUserId { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        [DisplayName("Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        [DisplayName("City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [DisplayName("State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Enter ZipCode")]
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public bool IsDeleted { get; set; }
        public string Address { get; set; }
        public string InsertUserId { get; set; }
        public string UpdateUserId { get; set; }
        public string TimezoneId { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezonePostfix { get; set; }

        public int OrganisationId { get; set; }

        public string AssignedZipcodes { get; set; }

        public string AdminEmail { get; set; }

        public string AdminQuickBloxId { get; set; }

        public string OldAdminUserId { get; set; }
    }

    public class OfficeServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public OfficesList OfficeList { get; set; }
        public List<Office> lstOffice { get; set; }
        public Office OfficeDetail { get; set; }
        public OfficeModel OfficeModel { get; set; }
  		public List<Office> OfficesList { get; set; }
        public List<Admin> AdminList { get; set; }

        public OfficeServiceProxy()
        {

            rootSuffix = "CaregiverLiteService.svc/";
        }

        #region GetOffice List
        public async Task<OfficesList> GetAllOffices(string Userid, int pageno, int recordperpage, string search, string sortfield, string sortOrder, string OrganisationId)
        {

            OfficesList OfficesList = new OfficesList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOfficesList/" + Userid + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OfficesList = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).OfficeList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return OfficesList;
        }

        public async Task<List<Office>> GetAllOffices(string logInUserId, string OrganisationId)
        {
            List<Office> OfficesList = new List<Office>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAvailableOfficesList/" + logInUserId + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OfficesList = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).lstOffice;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return OfficesList;
        }
        #endregion

        //#region AddNewOffice
        //public async Task<string> AddOffice(Office Office)
        //{

        //    string result = "";
        //    try
        //    {
        //        var json = "";
        //        // Send request to server
        //        HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddOffice", new { Office }).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Parse the response body. Blocking!
        //            json = await response.Content.ReadAsStringAsync();
        //            result = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).Result;
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog.LogError(ex);
        //    }
        //    return result;
        //}
        //#endregion


        #region AddNewOffice
        public async Task<Office> AddOffice(Office Office)
        {
            //   string result = "";
            Office OfficeDetails = new Office();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddOffice", new { Office }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OfficeDetails = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).OfficeDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return OfficeDetails;
        }
        #endregion

        #region GetOfficeDetailByOfficeId On Edit Button Click
        public async Task<Office> GetOfficeDetailByOfficeId(string OfficeId)
        {

            Office OfficeDetails = new Office();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetOfficeDetailByOfficeId/" + OfficeId , this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OfficeDetails = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).OfficeDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                //ErrorLog.LogError(ex);
            }
            return OfficeDetails;
        }

        #endregion


        #region GetOfficeDetailByOfficeId On Edit Button Click
        public async Task<Office> GetOrganisationOfficeDetailByOfficeId(string OfficeId, string OrganisationId)
        {
            Office OfficeDetails = new Office();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetOrganisationOfficeDetailByOfficeId/" + OfficeId + "/"+ OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OfficeDetails = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).OfficeDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                //ErrorLog.LogError(ex);
            }
            return OfficeDetails;
        }

        #endregion



        #region EditOffice
        public async Task<string> EditOffice(Office Office)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditOffice", new { Office }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }
        #endregion

        #region DeleteOffice
        public async Task<string> DeleteOfficeByOfficeId(string OfficeId, string UserId, string OrganisationId)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteOfficeByOfficeId/" + OfficeId + "/" + UserId + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }
        #endregion

        #region AssignZipcodesToOfficeByOfficeId
        public async Task<string> AssignZipcodesToOfficeByOfficeId(Office Office)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignZipcodesToOfficeByOfficeId/" + Office.OfficeId + "/" + Office.AssignedZipcodes + "/" + Office.InsertUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }

        #endregion

        #region GetAssignZipcodesToOfficeByOfficeId
        public async Task<string> GetAssignZipcodesToOfficeByOfficeId(int OfficeId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAssignZipcodesToOfficeByOfficeId/" + OfficeId , this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }
        #endregion

        public async Task<List<Admin>> GetAllAdminName(string OrganisationId)
        {

            List<Admin> AdminList = new List<Admin>();
            try
            {
                var json = "";
                // Send request to server

                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAdminName"+ "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AdminList = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).AdminList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return AdminList;
        }


        public async Task<List<Office>> GetAllOfficeExceptAssignedOfficeByUserId(string UserId,string LoginUserId)
        {
            List<Office> OfficesList = new List<Office>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOfficeExceptAssignedOfficeByUserId/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    OfficesList = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).OfficesList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return OfficesList;
        }


        public async Task<string> AssignOfficeToUser(string OfficeId, string UserId, string LoginUserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignOfficeToUser/" + OfficeId + "/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        public async Task<List<Office>> GetAllAssignedOfficeByUserId(string UserId)
        {
            List<Office> Offices = new List<Office>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAssignedOfficeByUserId/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    Offices = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).OfficesList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return Offices;
        }


        public async Task<string> RemoveSchedulerFromOffice(string OfficeId, string UserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "RemoveSchedulerFromOffice/" + OfficeId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OfficeServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }


    }

}