using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class LoginModel
    {

        [Display(Name = "LogIn ID")]
        [Required(ErrorMessage = "*")]
        public String LogInId { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Max 30 chars")]
        public String Password { get; set; }
    }
}