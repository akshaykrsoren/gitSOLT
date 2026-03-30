using CaregiverLiteWCF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class OrganisationModel
    {
        public int OrganisationId { get; set; }

        [Required(ErrorMessage = "Please Enter Organisation Name")]
        [DisplayName("OrganisationName")]
        public string OrganisationName { get; set; }


        //  [Required(ErrorMessage = "Please Enter Office Name")]
        //[DisplayName("OrganisationName")]
        //public string OrganisationName { get; set; }

        //  [Required(ErrorMessage = "Please Select Admin Name")]
        public string UserId { get; set; }

        //   [Required(ErrorMessage = "Please Select Admin Name")]
        public string OrganisationAdminUserId { get; set; }

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

        public string AssignedZipcodes { get; set; }

        public string OrganisationAdminEmail { get; set; }

        public string OrganisationAdminQuickBloxId { get; set; }

        public string OldAdminUserId { get; set; }
    }

    public class OrganisationServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public OrganisationsList OrganisationList { get; set; }
        public List<Organisation> lstOrganisation { get; set; }
        public Organisation OrganisationDetail { get; set; }
        public OrganisationModel OrganisationModel { get; set; }
        public List<Organisation> OrganisationsList { get; set; }
        public List<Admin> OrganisationAdminList { get; set; }

        public OrganisationServiceProxy()
        {

            rootSuffix = "CaregiverLiteService.svc/";
        }

        //    #region GetOffice List
        //    public async Task<OrganisationsList> GetAllOrganisations(string Userid, int pageno, int recordperpage, string search, string sortfield, string sortOrder)
        //    {

        //        OrganisationsList OrganisationsList = new OrganisationsList();

        //        List<Organisation> lstorg = new List<Organisation>();
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOrganisationsList/" + Userid + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();

        //                var resultQB = (JObject)JsonConvert.DeserializeObject(json);


        //                //OrganisationsList
        //                var JSONDATA = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json);

        //                OrganisationsList= JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationList;


        //                OrganisationList.FilteredRecord = resultQB["OrganisationList"]["FilteredRecord"].Value<int>();
        //                OrganisationList.TotalNumberofRecord= resultQB["OrganisationList"]["TotalNumberofRecord"].Value<int>();
        //                OrganisationList.OrganisationList = resultQB["OrganisationList"]["OrganisationList"].Value<List<Organisation>>();

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
        //        return OrganisationsList;
        //    }

        //    public async Task<List<Organisation>> GetAllOrganisations(string logInUserId)
        //    {

        //        List<Organisation> OrganisationsList = new List<Organisation>();
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAvailableOrganisationsList/" + logInUserId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                OrganisationsList = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).lstOrganisation;
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
        //        return OrganisationsList;
        //    }
        //    #endregion

        //    //#region AddNewOffice
        //    //public async Task<string> AddOffice(Office Office)
        //    //{

        //    //    string result = "";
        //    //    try
        //    //    {
        //    //        var json = "";
        //    //        // Send request to server
        //    //        HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddOffice", new { Office }).Result;
        //    //        if (response.IsSuccessStatusCode)
        //    //        {
        //    //            // Parse the response body. Blocking!
        //    //            json = await response.Content.ReadAsStringAsync();
        //    //            result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
        //    //        }
        //    //        else
        //    //        {
        //    //            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        //ErrorLog.LogError(ex);
        //    //    }
        //    //    return result;
        //    //}
        //    //#endregion


        //    #region AddNewOrganisation
        //    public async Task<Organisation> AddOrganisation(Organisation Organisation)
        //    {

        //        //   string result = "";
        //        Organisation OrganisationDetails = new Organisation();
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddOrganisation", new { Organisation }).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                OrganisationDetails= JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationDetail;
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
        //        return OrganisationDetails;
        //    }
        //    #endregion

        //    #region GetOrganisationDetailByOrganisationId On Edit Button Click
        //    public async Task<Organisation> GetOrganisationDetailByOrganisationId(string OrganisationId)
        //    {

        //        Organisation OrganisationDetails = new Organisation();
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetOrganisationDetailByOrganisationId/" + OrganisationId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                OrganisationDetails = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationDetail;
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
        //        return OrganisationDetails;
        //    }

        //    #endregion

        //    #region EditOffice
        //    public async Task<string> EditOrganisation(Organisation Organisation)
        //    {

        //        string result = "";
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditOrganisation", new { Organisation }).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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
        //    #endregion

        //    #region DeleteOrganisation
        //    public async Task<string> DeleteOrganisationByOrganisationId(string OrganisationId, string UserId)
        //    {

        //        string result = "";
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteOrganisationByOrganisationId/" + OrganisationId + "/" + UserId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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
        //    #endregion

        //    #region AssignZipcodesToOrganisationByOrganisationId
        //    public async Task<string> AssignZipcodesToOrganisationByOrganisationId(Organisation Organisation)
        //    {

        //        string result = "";
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignZipcodesToOrganisationByOrganisationId/" + Organisation.OrganisationId + "/" + Organisation.AssignedZipcodes + "/" + Organisation.InsertUserId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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

        //    #endregion

        //    #region GetAssignZipcodesToOrganisationByOrganisationId
        //    public async Task<string> GetAssignZipcodesToOrganisationByOrganisationId(int OrganisationId)
        //    {

        //        string result = "";
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAssignZipcodesToOrganisationByOrganisationId/" + OrganisationId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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
        //    #endregion

        //    public async Task<List<Admin>> GetAllOrganisationAdminName()
        //    {

        //        List<Admin> AdminList = new List<Admin>();
        //        try
        //        {
        //            var json = "";
        //            // Send request to server
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOrganisationAdminName", this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Parse the response body. Blocking!
        //                json = await response.Content.ReadAsStringAsync();
        //                AdminList = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationAdminList;
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
        //        return AdminList;
        //    }


        //    public async Task<List<Organisation>> GetAllOrganisationExceptAssignedOrganisationByUserId(string UserId, string LoginUserId)
        //    {
        //        List<Organisation> OrganisationsList = new List<Organisation>();
        //        try
        //        {
        //            var json = "";
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOrganisationExceptAssignedOrganisationByUserId/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                json = await response.Content.ReadAsStringAsync();
        //                OrganisationsList = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationsList;
        //            }
        //            else
        //            {
        //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //            }
        //        }
        //        catch (Exception ex)
        //        { }
        //        return OrganisationsList;
        //    }


        //    public async Task<string> AssignOrganisationToUser(string OrganisationId, string UserId, string LoginUserId)
        //    {
        //        string result = "";
        //        try
        //        {
        //            var json = "";
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignOrganisationToUser/" + OrganisationId + "/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                json = await response.Content.ReadAsStringAsync();
        //                result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
        //            }
        //            else
        //            {
        //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //            }
        //        }
        //        catch (Exception ex)
        //        { }
        //        return result;
        //    }

        //    public async Task<List<Organisation>> GetAllAssignedOrganisationByUserId(string UserId)
        //    {
        //        List<Organisation> Organisations = new List<Organisation>();
        //        try
        //        {
        //            var json = "";
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAssignedOrganisationByUserId/" + UserId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                json = await response.Content.ReadAsStringAsync();
        //                Organisations = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationsList;
        //            }
        //            else
        //            {
        //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //            }
        //        }
        //        catch (Exception ex)
        //        { }
        //        return Organisations;
        //    }


        //    public async Task<string> RemoveSchedulerFromOrganisation(string OrganisationId, string UserId)
        //    {
        //        string result = "";
        //        try
        //        {
        //            var json = "";
        //            HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "RemoveSchedulerFromOrganisation/" + OrganisationId + "/" + UserId, this.cancellationToken).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                json = await response.Content.ReadAsStringAsync();
        //                result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
        //            }
        //            else
        //            {
        //                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //            }
        //        }
        //        catch (Exception ex)
        //        { }
        //        return result;
        //}

        #region GetOffice List
        public async Task<OrganisationsList> GetAllOrganisations(string Userid, int pageno, int recordperpage, string search, string sortfield, string sortOrder)
        {

            OrganisationsList OrganisationsList = new OrganisationsList();

            List<Organisation> lstorg = new List<Organisation>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOrganisationsList/" + Userid + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();

                    var resultQB = (JObject)JsonConvert.DeserializeObject(json);


                    //OrganisationsList
                    var JSONDATA = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json);

                    OrganisationsList = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationList;


                    OrganisationList.FilteredRecord = resultQB["OrganisationList"]["FilteredRecord"].Value<int>();
                    OrganisationList.TotalNumberofRecord = resultQB["OrganisationList"]["TotalNumberofRecord"].Value<int>();
                    OrganisationList.OrganisationList = resultQB["OrganisationList"]["OrganisationList"].Value<List<Organisation>>();

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
            return OrganisationsList;
        }

        public async Task<List<Organisation>> GetAllOrganisations(string logInUserId, string OrganisationId)
        {

            List<Organisation> OrganisationsList = new List<Organisation>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAvailableOrganisationsList/" + logInUserId +"/"+OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OrganisationsList = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).lstOrganisation;
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
            return OrganisationsList;
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
        //            result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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


        #region AddNewOrganisation
        public async Task<Organisation> AddOrganisation(Organisation Organisation)
        {

            //   string result = "";
            Organisation OrganisationDetails = new Organisation();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddOrganisation", new { Organisation }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OrganisationDetails = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationDetail;
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
            return OrganisationDetails;
        }
        #endregion

        #region GetOrganisationDetailByOrganisationId On Edit Button Click
        public async Task<Organisation> GetOrganisationDetailByOrganisationId(string OrganisationId)
        {

            Organisation OrganisationDetails = new Organisation();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetOrganisationDetailByOrganisationId/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    OrganisationDetails = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationDetail;
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
            return OrganisationDetails;
        }

        #endregion

        #region EditOffice
        public async Task<string> EditOrganisation(Organisation Organisation)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditOrganisation", new { Organisation }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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

        #region DeleteOrganisation
        public async Task<string> DeleteOrganisationByOrganisationId(string OrganisationId, string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteOrganisationByOrganisationId/" + OrganisationId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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

        #region AssignZipcodesToOrganisationByOrganisationId
        public async Task<string> AssignZipcodesToOrganisationByOrganisationId(Organisation Organisation)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignZipcodesToOrganisationByOrganisationId/" + Organisation.OrganisationId + "/" + Organisation.AssignedZipcodes + "/" + Organisation.InsertUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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

        #region GetAssignZipcodesToOrganisationByOrganisationId
        public async Task<string> GetAssignZipcodesToOrganisationByOrganisationId(int OrganisationId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAssignZipcodesToOrganisationByOrganisationId/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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

        public async Task<List<Admin>> GetAllOrganisationAdminName(int OrganisationId)
        {

            List<Admin> AdminList = new List<Admin>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOrganisationAdminName/" + OrganisationId.ToString(), this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AdminList = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationAdminList;
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


        public async Task<List<Organisation>> GetAllOrganisationExceptAssignedOrganisationByUserId(string UserId, string LoginUserId)
        {
            List<Organisation> OrganisationsList = new List<Organisation>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOrganisationExceptAssignedOrganisationByUserId/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    OrganisationsList = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return OrganisationsList;
        }


        public async Task<string> AssignOrganisationToUser(string OrganisationId, string UserId, string LoginUserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignOrganisationToUser/" + OrganisationId + "/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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

        public async Task<List<Organisation>> GetAllAssignedOrganisationByUserId(string UserId)
        {
            List<Organisation> Organisations = new List<Organisation>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAssignedOrganisationByUserId/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    Organisations = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).OrganisationsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return Organisations;
        }


        public async Task<string> RemoveSchedulerFromOrganisation(string OrganisationId, string UserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "RemoveSchedulerFromOrganisation/" + OrganisationId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrganisationServiceProxy>(json).Result;
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