using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models
{
    public class AddSuperAdminLoginDetails
    {
        public string UserId { get; set; }

        // public string FirstName { get; set; }

        //  public string LastName { get; set; }


        public string Name { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string RoleId { get; set; }

        public List<AddSuperAdminLoginDetails> AddSuperAdminLoginInfo { get; set; }

    }
}