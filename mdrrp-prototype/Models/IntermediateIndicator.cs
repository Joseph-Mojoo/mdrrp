using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class IntermediateIndicator
    {
        public IntermediateIndicator()
        {
            this.Results = new HashSet<Result>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Unit { get; set; }
        [Required]
        public int BaseLine { get; set; }
        [Required]
        [Range(10,100000000)]
        public int Target { get; set; }
        [Required]
        [Display(Name = "Data source methodology")]
        public string DataSourceMethodology { get; set; }
        [Required]
        public string Frequency { get; set; }
        [Required]
        [Display(Name = "Data collector")]
        public string DataCollector { get; set; }

        [DefaultValue(0)]
        public bool DeletedFlag { get; set; }

        public ProjectIndicator ProjectIndicator { get; set; }
        [Display(Name = "Project Indicator Name")]
        public int ProjectIndicatorId { get; set; }

        public SubComponent SubComponent { get; set; }

        public int? SubComponentId { get; set; }

        public ICollection<Result> Results { get; set; }


    }
}