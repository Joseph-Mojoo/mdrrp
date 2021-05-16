using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class ProjectIndicator
    {
        public ProjectIndicator()
        {
            this.IntermediateIndicators = new HashSet<IntermediateIndicator>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        [Range(0,1000000000)]
        public int BaseLine { get; set; }

        [Required]
        [Range(0, 1000000000)]
        public int Target { get; set; }

        [DefaultValue(0)]
        public bool DeletedFlag { get; set; }

        public Project Project { get; set; }
        [Required]
        public int ProjectId { get; set; }

        public virtual ICollection<IntermediateIndicator> IntermediateIndicators { get; set; }
    }
}