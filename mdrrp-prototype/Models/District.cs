using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class District
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "District Name")]
        public string Name { get; set; }

        [DefaultValue(0)]
        public bool DeletedFlag { get; set; }
        
    }
}