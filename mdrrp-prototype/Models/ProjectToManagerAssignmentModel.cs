using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class ProjectToManagerAssignmentModel
    {
        [Key]
        public int ProjectToManagerModelId { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Project Code")]
        public int ProjectId { get; set; }
    }
}