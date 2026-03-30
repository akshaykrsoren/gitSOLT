using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CaregiverLite;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.WebPages.Html;


namespace CaregiverLite.Models
{
    public class LoginModel
    {
        //[Required(ErrorMessageResourceType = typeof(CareGiverSuperAdmin.Views.Resources.Account), ErrorMessageResourceName = "RequiredMsgUserName")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string UserName { get; set; }

        //[Required(ErrorMessageResourceType = typeof(CareGiverSuperAdmin.Views.Resources.Account), ErrorMessageResourceName = "RequiredMsgPassword")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgetPasswordServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }

        public ForgetPasswordServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> ForgotPassword(string Email)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "ForgotPassword/" + Email, this.cancellationToken).Result;
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


        public async Task<string> EmailtoSetPassword(string Email)
        {
            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EmailtoSetPassword/" + Email, this.cancellationToken).Result;
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
    }

    public class ForgotPassword
    {
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgPassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "RequiredMsgReTypePassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.CareGiver), ErrorMessageResourceName = "ValidationMsgPassword", MinimumLength = 6)]
        [DisplayName("Confirm Password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "retype password does not match")]
        public string ConfirmPassword { get; set; }
    }
}