using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mdrrp_prototype.Models
{
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole()
        {
            
        }
        public ApplicationRole(string name):base( name)
        {
            this.Name = name;
        }

        public ICollection<Permission> Permissions { get; set; }
    }
}