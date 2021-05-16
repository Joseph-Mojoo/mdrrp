using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class SubcomponentToClerkModel
    {
        [Key]
        public int SubcomponentToClerkModelId { get; set; }

        [Required]
        public string UserId  { get; set; }

        [Required]
        [Display(Name = "Subcomponent Name")]
        public int SubcomponentId { get; set; }
    }
}