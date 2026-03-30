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
using System.Web.Mvc;
namespace CaregiverLite.Models
{
    public class AdminModel
    {
        public int AdminId { get; set; }

        public string UserId { get; set; }

        public string InsertUserId { get; set; }

        public string QuickBloxId { get; set; }

        public int OrganisationId { get; set; }

        public string IsActive { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        //[DisplayName("Name")]
        //public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgName")]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmail")]
        //[Remote("IsEmailExists", "CareGiver", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgEmailExists")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsername")]
        // [Remote("IsUserNameExists","CareGiver",AdditionalFields="UserId", ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgUsernameExists")]
        [DisplayName("Username")]
        public string Username { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        //[StringLength(15, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        //[DisplayName("Password")]
        public string Password { get; set; }

        public string OldEmail { get; set; }

        public string OfficeIds { get; set; }


    }


    public class AdminServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public AdminsList AdminsList { get; set; }
        public Admin AdminDetail { get; set; }
        public List<Admin> AdminList { get; set; }



        public AdminServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }


        public async Task<AdminsList> GetAllAdmins(int pageno, int recordperpage, string search, string sortfield, string sortOrder, int officeId,int OrganisationId,string IsActiveStatus)
        {

            AdminsList AdminsList = new AdminsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllOfficeAdmin/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + officeId.ToString() + "/"  + search + "/" + OrganisationId.ToString()+"/"+ IsActiveStatus, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AdminsList = JsonConvert.DeserializeObject<AdminServiceProxy>(json).AdminsList;
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


        public async Task<string> AddAdmin(Admin Admin)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddAdmin", new { Admin }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AdminServiceProxy>(json).Result;
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

        public async Task<Admin> GetAdminDetailById(string SchedulerId)
        {

            Admin AdminDetails = new Admin();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAdminDetailById/" + SchedulerId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AdminDetails = JsonConvert.DeserializeObject<AdminServiceProxy>(json).AdminDetail;
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
            return AdminDetails;
        }


        public async Task<string> EditAdmin(Admin Admin)
        {

            string result = "";
            try
            {
                var json = "";

                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditAdmin", new { Admin }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AdminServiceProxy>(json).Result;
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

        public async Task<string> DeleteAdmin(string AdminId, string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteAdmin/" + AdminId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AdminServiceProxy>(json).Result;
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


        public async Task<Admin> GetAdminDetailByUserId(string AdminUserId)
        {

            Admin AdminDetails = new Admin();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAdminDetailByUserId/" + AdminUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    AdminDetails = JsonConvert.DeserializeObject<AdminServiceProxy>(json).AdminDetail;
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
            return AdminDetails;
        }
    }
}