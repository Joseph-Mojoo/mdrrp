using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class ProjectComponent
    {
        public ProjectComponent()
        {
            this.SubComponents = new HashSet<SubComponent>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1000000,10000000000)]
        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [DefaultValue(0)]
        public bool DeletedFlag { get; set; }

        public ICollection<SubComponent> SubComponents { get; set; }
    }
}