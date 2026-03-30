using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CaregiverLiteWCF;

namespace CaregiverLite.Models
{
    public class ServiceModel
    {
        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Services), ErrorMessageResourceName = "RequiredMsgServiceName")]
        [DisplayName("ServiceName")]
        public string ServiceName { get; set; }

        public int ServiceId { get; set; }

        public int OrganisationId { get; set; }
        public string Description { get; set; }
    }



    public class VisitTypeModel
    {
      // [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Services), ErrorMessageResourceName = "RequiredMsgVisitTypeName")]

        [DisplayName("VisitTypeName")]
        public string VisitTypeName { get; set; }

        public int VisitTypeId { get; set; }

        public int OrganisationId { get; set; }
    }


    public class ServicesServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public List<Services> ServicesList { get; set; }

        public ServicesServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> InsertUpdateService(Services Service)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertUpdateService", new { Service }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ServicesServiceProxy>(json).Result;
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

        public async Task<List<Services>> GetAllServices(string OrganisationId)
        {

            List<Services> ServicesList = new List<Services>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllServices/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ServicesList = JsonConvert.DeserializeObject<ServicesServiceProxy>(json).ServicesList;
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
            return ServicesList;
        }

        public async Task<string> DeleteService(string ServiceId, string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteService/" + ServiceId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ServicesServiceProxy>(json).Result;
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
    public class ServiceVisitModel
    {
        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Services), ErrorMessageResourceName = "RequiredMsgServiceName")]
        [DisplayName("ServiceName")]
        public string ServiceName { get; set; }

        public int ServiceId { get; set; }
        [DisplayName("VisitTypeName")]
        public string VisitTypeName { get; set; }

        public int VisitTypeId { get; set; }

        public int OrganisationId { get; set; }
        public string Description { get; set; }


       
    }
}