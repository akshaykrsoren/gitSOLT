using CaregiverLiteWCF;
using CaregiverLiteWCF.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Models
{
    public class MarketerDetailsModel
    {


        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgPatient")]
        public int MarketersId { get; set; }


        [Required(ErrorMessage = "Please Enter Patient Name")]
        [DisplayName("MarketersName")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string MarketersName { get; set; }

        //[Required(ErrorMessage = "Please Enter Medical-Id")]
        //[DisplayName("MedicalId")]
        //public string MedicalId { get; set; }

        //[Required(ErrorMessage = "Please Enter Address")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter PhoneNo")]
        [DisplayName("PhoneNo")]
        public string PhoneNo { get; set; }

        //[Required(ErrorMessage = "Please Enter Email-Id")]
        [DisplayName("Email")]
        public string Email { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        [Required(ErrorMessage = "Please Enter ZipCode")]
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }

        public string InsertUserId { get; set; }
        public string UpdateUserId { get; set; }

        public string TimezoneId { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezonePostfix { get; set; }

        //[Required(ErrorMessage = "Please Enter PrimaryMD")]
        //[DisplayName("PrimaryMD")]
        //public string PrimaryMD { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        [DisplayName("Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        [DisplayName("City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [DisplayName("State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Select Office")]
        [DisplayName("Office")]
        public int OfficeId { get; set; }


        public string OfficeName { get; set; }

        public String UserId { get; set; }


        public string quality { get; set; }

        public string description { get; set; }

        public string UserName { get; set; }
        //[Required(ErrorMessage = "Please Enter Password")]
        //[DisplayName("Password")]
        public string Password { get; set; }

        public IEnumerable<SelectListItem> OfficeSelectList { get; set; }


    }

    //public class MarketingsDetailsServiceProxy : CaregiverLiteBaseService
    //{
    //    public string Result { get; set; }
    //    public bool ResultInBool { get; set; }
    //    public List<MarketingsDetail> MaketingsDetailsList { get; set; }
    //    public MarketersDetailsList ListMarketingsDetails { get; set; }
    //    public MarketingsDetail MarketingsDetail { get; set; }
    //    public MarketingsDetailsServiceProxy()
    //    {
    //        rootSuffix = "CaregiverLiteService.svc/";
    //    }

    //    public async Task<string> AddMarketers(MarketingsDetail MarketingsDetail)
    //    {

    //        string result = "";
    //        try
    //        {
    //            var json = "";
    //            // Send request to server
    //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddPatient", new { MarketingsDetail }).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                // Parse the response body. Blocking!
    //                json = await response.Content.ReadAsStringAsync();
    //                result = JsonConvert.DeserializeObject<MarketingsDetailsServiceProxy>(json).Result;
    //            }
    //            else
    //            {
    //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //ErrorLog.LogError(ex);
    //        }
    //        return result;
    //    }

    //    public async Task<MarketersDetailsList> GetAllMarketingDetail(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, int OfficeId)
    //    {

    //        List<MarketingsDetail> objPatientDetailsList = new List<MarketingsDetail>();
    //        MarketersDetailsList MarketingsDetail = new MarketersDetailsList();
    //        try
    //        {
    //            var json = "";
    //            // Send request to server
    //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetMarketingDetailsList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + OfficeId + "/" + search, this.cancellationToken).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                // Parse the response body. Blocking!
    //                json = await response.Content.ReadAsStringAsync();
    //                MarketingsDetail = JsonConvert.DeserializeObject<MarketersDetailsList>(json).res;
    //            }
    //            else
    //            {
    //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //ErrorLog.LogError(ex);
    //        }
    //        return MarketingsDetail;
    //    }

    //    public async Task<string> DeleteService(string ServiceId, string UserId)
    //    {

    //        string result = "";
    //        try
    //        {
    //            var json = "";
    //            // Send request to server
    //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteService/" + ServiceId + "/" + UserId, this.cancellationToken).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                // Parse the response body. Blocking!
    //                json = await response.Content.ReadAsStringAsync();
    //                result = JsonConvert.DeserializeObject<ServicesServiceProxy>(json).Result;
    //            }
    //            else
    //            {
    //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //ErrorLog.LogError(ex);
    //        }
    //        return result;
    //    }

    //    public async Task<MarketingsDetail> GetMarketingsDetailById(string MarketingsDetailId)
    //    {

    //        MarketingsDetail MarketingsDetails = new MarketingsDetail();
    //        try
    //        {
    //            var json = "";
    //            // Send request to server
    //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientDetailById/" + MarketingsDetailId, this.cancellationToken).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                // Parse the response body. Blocking!
    //                json = await response.Content.ReadAsStringAsync();
    //                MarketingsDetails = JsonConvert.DeserializeObject<MarketingsDetailsServiceProxy>(json).MarketingsDetail;
    //            }
    //            else
    //            {
    //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            //ErrorLog.LogError(ex);
    //        }
    //        return MarketingsDetails;
    //    }

    //    public async Task<string> EditMarketingsDetails(MarketingsDetail MarketingsDetail)
    //    {

    //        string result = "";
    //        try
    //        {
    //            var json = "";
    //            // Send request to server
    //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditPatientDetails", new { MarketingsDetail }).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                // Parse the response body. Blocking!
    //                json = await response.Content.ReadAsStringAsync();
    //                result = JsonConvert.DeserializeObject<MarketingsDetailsServiceProxy>(json).Result;
    //            }
    //            else
    //            {
    //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //ErrorLog.LogError(ex);
    //        }
    //        return result;
    //    }

    //    public async Task<string> DeleteMarketingsDetail(string MarketingsDetailId, string UserID)
    //    {

    //        string result = "";
    //        try
    //        {
    //            var json = "";
    //            // Send request to server
    //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeletePatientDetail/" + MarketingsDetailId + "/" + UserID, this.cancellationToken).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                // Parse the response body. Blocking!
    //                json = await response.Content.ReadAsStringAsync();
    //                result = JsonConvert.DeserializeObject<MarketingsDetailsServiceProxy>(json).Result;
    //            }
    //            else
    //            {
    //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //ErrorLog.LogError(ex);
    //        }
    //        return result;
    //    }

    //    public async Task<bool> IsMarketersIdExist(MarketingsDetail MarketingsDetail)
    //    {

    //        bool result = false;
    //        try
    //        {
    //            var json = "";
    //            // Send request to server
    //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "IsMedicalIdExist", new { MarketingsDetail }).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                // Parse the response body. Blocking!
    //                json = await response.Content.ReadAsStringAsync();
    //                result = JsonConvert.DeserializeObject<MarketingsDetailsServiceProxy>(json).ResultInBool;
    //            }
    //            else
    //            {
    //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //ErrorLog.LogError(ex);
    //        }
    //        return result;
    //    }

   // }
}