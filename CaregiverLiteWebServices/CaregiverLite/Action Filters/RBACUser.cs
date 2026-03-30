using CaregiverLiteWCF.Class;
using CaregiverLite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

public class RBACUser
{
    public string User_Id { get; set; }
    public bool IsSysAdmin { get; set; }
    public string Username { get; set; }
    private List<UserRole> UserRoles = new List<UserRole>();
   
    PermissionServiceProxy objPermissionServiceProxy = new PermissionServiceProxy();
    RoleServiceProxy objRoleServiceProxy = new RoleServiceProxy();

    public RBACUser(string _username)
    {
        this.Username = _username;
        MembershipUser _user = Membership.GetUser(this.Username);
        if (_user != null)
        {
            string[] roles = Roles.GetRolesForUser(_user.UserName);
            if (roles.Length != 0)
            {
                foreach (string role in roles)
                {
                    if (role == "SuperAdmin")
                    {
                        this.IsSysAdmin = true;
                    }
                    else
                    {
                        this.IsSysAdmin = false;
                    }
                }
            }

            GetDatabaseUserRolesPermissions();
        }
        else
        {
            this.IsSysAdmin = true;
        }
    }

    
    private void GetDatabaseUserRolesPermissions()
    {
        MembershipUser _user = Membership.GetUser(this.Username);

        string[] roles = Roles.GetRolesForUser(this.Username);
         //   USER _user = _data.USERS.Where(u => u.Username == this.Username).FirstOrDefault();
            if (_user != null)
            {
                this.User_Id = _user.ProviderUserKey.ToString();

                List<Role> ListUserRoles = new List<Role>();
                ListUserRoles = objRoleServiceProxy.GetAllRolesForUser(this.User_Id).Result;
                List<Permission> ListUserPermission = new List<Permission>();
                ListUserPermission = objPermissionServiceProxy.GetAllPermissionForUser(this.User_Id).Result;
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


    public bool HasRole(string role)
    {
        return (UserRoles.Where(p => p.RoleName == role).ToList().Count > 0);
    }


    public bool HasRoles(string roles)
    {
        bool bFound = false;
        string[] _roles = roles.ToLower().Split(';');
        foreach (UserRole role in this.UserRoles)
        {
            try
            {
                bFound = _roles.Contains(role.RoleName.ToLower());
                if (bFound)
                    return bFound;
            }
            catch (Exception)
            {
            }
        }
        return bFound;
    }
}


public class UserRole
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public List<UserPermission> Permissions = new List<UserPermission>();
}
 

public class UserPermission
{
    public string PermissionId { get; set; }
    public string PermissionDescription { get; set; }
}
