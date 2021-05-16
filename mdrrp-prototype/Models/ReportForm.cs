using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;

namespace mdrrp_prototype.Models
{
    public class ReportForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Project Code")]
        public int ProjectId { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public int? Year { get; set; }

       


    }
}