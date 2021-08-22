using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class ForgetPasswordModel
    {
        [Display(Name = "Enter Admin ID")]
        [Required(ErrorMessage = "*")]
        public int adminID { get; set; }

        [Display(Name = "Select Secuity Question")]
        [Required(ErrorMessage = ("*"))]
        public string securityQuestion { get; set; }


        [Display(Name = "Enter Security Answer")]
        [Required(ErrorMessage = ("*"))]
        [StringLength(30, ErrorMessage = "Max 30 chars")]
        public string securityAnswer { get; set; }
    }
}