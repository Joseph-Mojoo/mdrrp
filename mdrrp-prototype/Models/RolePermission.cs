using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class RolePermission
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Roles")]
        public string RoleId { get; set; }

        [Required]
        [Display(Name = "Permissions")]
        public List<String> Permissions { get; set; }
    }
}