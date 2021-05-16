using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class ProjectReportData
    {   [Key]
        public int Id { get; set; }

        [Display(Name = "Indicator Name")]
        public string IndicatorName { get; set; }
        
        public int Target { get; set; }

        public int BaseLine { get; set; }

        public int Remaining { get; set; }

        public string ProjectName { get; set; }

        public IEnumerable<IntermediateIndicator> IntermediateIndicators { get; set; }
    }
}