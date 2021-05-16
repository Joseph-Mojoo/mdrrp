using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class SubComponent
    {
        public SubComponent()
        {
            this.IntermediateIndicators = new HashSet<IntermediateIndicator>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Subcomponent Name")]
        public string Name { get; set; }
        [Required]
        [Range(1000000,10000000000)]
        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }


        public bool DeletedFlag { get; set; }

        //add a navigation property
        public ProjectComponent ProjectComponent { get; set; }
        //add a foreign key for a component id
        public int ProjectComponentId { get; set; }

        public ICollection<IntermediateIndicator> IntermediateIndicators { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}