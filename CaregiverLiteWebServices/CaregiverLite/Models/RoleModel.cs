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
    public class RoleModel
    {
    }
    public class RoleServiceProxy : CaregiverLiteBaseService
    {
        public List<Role> UserRoleList { get; set; }
        public string Result { get; set; }
        public List<Permission> UserPermissionList { get; set; }

        public RoleServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }
        public async Task<List<Role>> GetAllRolesForUser(string UserId)
        {
            List<Role> UserRoleList = new List<Role>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllRolesForUser/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    UserRoleList = JsonConvert.DeserializeObject<RoleServiceProxy>(json).UserRoleList;
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
            return UserRoleList;
        }
    }
}