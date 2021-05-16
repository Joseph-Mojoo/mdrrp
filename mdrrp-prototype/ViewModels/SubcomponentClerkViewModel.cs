using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.ViewModels
{
    public class SubcomponentClerkViewModel
    {
       
       
        public string SubcomponentClerkViewModelId { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Component Name")]
        public int ComponentId { get; set; }

        [Required]
        [Display(Name = "Sucomponent Name")]
        public int SubcomponentId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}