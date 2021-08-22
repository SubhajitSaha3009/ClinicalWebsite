using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Clinic.Models
{
    public class DocWorkingTimingModel
    {
        [Display(Name = "Doctor ID")]
        [Required(ErrorMessage = "*")]
        public int doctorID { get; set; }


        [Display(Name = "Doctor Name")]
        [Required(ErrorMessage = "*")]
     //   [RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
     //   [StringLength(60, ErrorMessage = "Max 60 characters")]
        public string doctorName { get; set; }



        [Display(Name = "Doctor Designation")]
        [Required(ErrorMessage = "*")]
        //[RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        public string doctorDesignation { get; set; }


        [Display(Name = "Doctor Working Day")]
        [Required(ErrorMessage = "*")]
        [RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(15, ErrorMessage = "Max 15 characters")]
        public string workingDay { get; set; }



        [Display(Name = "Doctor Working Time")]
        [Required(ErrorMessage = "*")]
       // [RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        public string workingTime { get; set; }
    }
}