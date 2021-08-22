using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Clinic.Models
{
    public class ForgetPasswordToEmailModel
    {

        [Display(Name = "Admin ID")]
        [Required(ErrorMessage = "*")]
        public int adminID { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "*")]
        public string name { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "*")]
        public string subject { get; set; }

        [Display(Name = "Body")]
        [Required(ErrorMessage = "*")]
        public string body { get; set; }
    }
}