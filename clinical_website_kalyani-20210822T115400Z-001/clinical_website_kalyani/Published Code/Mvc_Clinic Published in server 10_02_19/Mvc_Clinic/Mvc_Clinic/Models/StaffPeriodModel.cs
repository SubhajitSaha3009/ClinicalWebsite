using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class StaffPeriodModel
    {
        [Display(Name = "From Date")]
        [Required(ErrorMessage = "*")]
        public string fromDate { get; set; }

        [Display(Name = "To Date")]
        [Required(ErrorMessage = "*")]
        public string toDate { get; set; }
    }
}