using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mdrrp_prototype.Models
{
    public class Permission
    {
        public Permission()
        {
            this.ApplicationRoles = new HashSet<ApplicationRole>();
        }
        public int Id { get; set; }


        [Required]
        [Display(Name = "Permission")]
        public string Description { get; set; }
        [DefaultValue(0)]
        public bool DeletedFlag { get; set; }

        public ICollection<ApplicationRole> ApplicationRoles { get; set; }
    }
}