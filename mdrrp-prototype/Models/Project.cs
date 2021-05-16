using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mdrrp_prototype.Models
{
    public class Project
    {
        public Project()
        {
            this.ProjectComponents = new HashSet<ProjectComponent>();
            this.ProjectIndicators = new HashSet<ProjectIndicator>();
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Project Code")]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Approval Date")]
        public DateTime ApprovalDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }

        [Required]
        public string Objective { get; set; }

        [Required]
        [Display(Name = "Is Regionally Tagged")]
        public bool IsRegionallyTagged { get; set; }

        [Required]
        [Display(Name = "Bank/IF Collaboration")]
        public bool BankIFCCollaboration { get; set; }

        [DefaultValue(0)]
        public bool DeletedFlag { get; set; }
        [Required]
        [Range(1000000,10000000000)]
        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        public  ICollection<ProjectComponent> ProjectComponents { get; set; }

        public  ICollection<ProjectIndicator> ProjectIndicators { get; set; }

        public ICollection<ApplicationUser> User { get; set; }
    }
}