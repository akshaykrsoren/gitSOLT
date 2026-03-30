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
    public class AdministratorModel
    {
        public string UserId { get; set; }

        public string InsertUserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmail")]
        [Remote("IsEmailExists", "CareGiver", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmailExists")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsername")]
        [Remote("IsUserNameExists", "CareGiver", AdditionalFields = "UserId", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsernameExists")]
        [DisplayName("Username")]
        public string Username { get; set; }



        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        [DisplayName("Retype Password is required")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "retype password does not match")]      
        public string RetypePassword { get; set; }

       
    }
    public class AdministratorServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }

        public AdminsList AdminList { get; set; }
        public SuperAdmin SuperAdmin { get; set; }
        
        public AdministratorServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }
        public async Task<string> InsertUpdateSuperAdmin(SuperAdmin SuperAdmin)        
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertUpdateSuperAdmin", new { SuperAdmin }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AdministratorServiceProxy>(json).Result;
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

        public async Task<string> DeleteUserWithPermissions(string UserId)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteUserWithPermissions/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AdministratorServiceProxy>(json).Result;
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
        public async Task<string> InsertUserPerrmission(string UserId, string PermissionId)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertUserPermission/" + UserId + "/" + PermissionId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AdministratorServiceProxy>(json).Result;
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

        public async Task<string> DeleteAdmin(string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteAdmin/" + UserId , this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AdministratorServiceProxy>(json).Result;
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
        public async Task<SuperAdmin> GetAdminDetailsById(string NurseId)
        {

            SuperAdmin SuperAdminDetail = new SuperAdmin();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAdminDetailsById/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SuperAdminDetail = JsonConvert.DeserializeObject<AdministratorServiceProxy>(json).SuperAdmin;
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
            return SuperAdminDetail;
        }

        public async Task<AdminsList> GetAllAdmin()
        {

            AdminsList AdminLst = new AdminsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAdmin", this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AdminLst = JsonConvert.DeserializeObject<AdministratorServiceProxy>(json).AdminList;
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
            return AdminLst;
        }
        public async Task<AdminsList> GetAllAdmin(int pageno, int recordperpage, string search, string sortfield, string sortOrder)
        {

            AdminsList AdminsList = new AdminsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAdmin/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AdminsList = JsonConvert.DeserializeObject<AdministratorServiceProxy>(json).AdminList;
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
            return AdminsList;
        }
    }
}