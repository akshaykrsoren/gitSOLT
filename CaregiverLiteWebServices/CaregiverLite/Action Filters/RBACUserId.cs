using CaregiverLiteWCF.Class;
using CaregiverLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CareGiverSuperAdmin.Action_Filters
{
    public class RBACUserId
    {
        public string User_Id { get; set; }
        public bool IsSysAdmin { get; set; }
        public string Username { get; set; }
        private List<UserRole> UserRoles = new List<UserRole>();
        PermissionServiceProxy objPermissionServiceProxy = new PermissionServiceProxy();
        RoleServiceProxy objRoleServiceProxy = new RoleServiceProxy();
        public RBACUserId(string UserId)
        {
            this.User_Id = UserId;
           
            GetDatabaseUserRolesPermissions(UserId);
        }
        private void GetDatabaseUserRolesPermissions(string UserId)
        {
            MembershipUser _user = Membership.GetUser(new Guid(UserId));

            string[] roles = Roles.GetRolesForUser(_user.UserName);
            //   USER _user = _data.USERS.Where(u => u.Username == this.Username).FirstOrDefault();
            if (_user != null)
            {
                this.User_Id = _user.ProviderUserKey.ToString();

                List<Role> ListUserRoles = new List<Role>();
                ListUserRoles = objRoleServiceProxy.GetAllRolesForUser(UserId).Result;
                List<Permission> ListUserPermission = new List<Permission>();
                ListUserPermission = objPermissionServiceProxy.GetAllPermissionForUser(UserId).Result;
                foreach (Role _role in ListUserRoles)
                {
                    UserRole _userRole = new UserRole { RoleId = _role.RoleId, RoleName = _role.RoleName };
                    foreach (Permission _permission in ListUserPermission)
                    {
                        _userRole.Permissions.Add(new UserPermission { PermissionId = _permission.PermissionId, PermissionDescription = _permission.PermissionDescription });
                    }

                    this.UserRoles.Add(_userRole);
                }
            }
        }
        public bool HasPermission(string requiredPermission)
        {
            bool bFound = false;
            foreach (UserRole role in this.UserRoles)
            {
                bFound = (role.Permissions.Where(p => p.PermissionDescription.ToLower() == requiredPermission.ToLower()).ToList().Count > 0);
                if (bFound)
                    break;
            }
            return bFound;
        }
    }
}