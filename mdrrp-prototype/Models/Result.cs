using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mdrrp_prototype.Models
{
    public class Result
    {
        public int Id { get; set; }

        public IntermediateIndicator IntermediateIndicator { get; set; }

        [Required]
        [Display(Name ="Indicator Name")]
        public int IntermediateIndicatorId { get; set; }

        [Required]
        public decimal Value { get; set; }


        [DefaultValue(0)]
        public bool DeletedFlag { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        

    }
}