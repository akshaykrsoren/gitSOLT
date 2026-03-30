using CaregiverLiteWCF.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CaregiverLite.Models
{
    public class PermissionModel
    {
        public int PermissionId { get; set; }
        public string PermissionDescription { get; set; }
        public virtual ICollection<Role> ROLES { get; set; }
    }
    public class PermissionServiceProxy : CaregiverLiteBaseService
    {
        public List<Permission> PermissionList { get; set; }
        public string Result { get; set; }
        public List<Permission> UserPermissionList { get; set; }

        public PermissionServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }
        public async Task<List<Permission>> GetAllPermissionForUser(string UserId)
        {
            List<Permission> UserPermissionList = new List<Permission>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllPermissionForUser/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    UserPermissionList = JsonConvert.DeserializeObject<PermissionServiceProxy>(json).UserPermissionList;
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
            return UserPermissionList;
        }
    }
}