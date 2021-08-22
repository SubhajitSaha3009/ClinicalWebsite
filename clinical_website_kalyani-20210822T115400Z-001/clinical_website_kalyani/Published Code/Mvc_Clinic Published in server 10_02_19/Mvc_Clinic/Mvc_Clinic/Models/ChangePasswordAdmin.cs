using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Clinic.Models
{
    public class ChangePasswordAdmin
    {
        [Display(Name = "Enter Old Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ("*"))]
        [StringLength(30, ErrorMessage = "Max 30 chars")]
        public string oldPassword { get; set; }



        [Display(Name = "Enter New Password")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@!%*#?&])[A-Za-z\\d@$!%*#?&\\s]{6,}$", ErrorMessage = "Invalid Password Format! (Should be Alphanumeric and contain ateast one special character and minimum length of 6 characters)")]
        [Required(ErrorMessage = ("*"))]
        [StringLength(30, ErrorMessage = "Max 30 chars")]
        public string newPassword { get; set; }



        [Display(Name = "Enter Password Again")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "Password didn't match")]
        [Required(ErrorMessage = ("*"))]
        [StringLength(30, ErrorMessage = "Max 30 chars")]
        public string retypePassword { get; set; }



        //[Display(Name = "Enter Email ID")]
        //[Required(ErrorMessage = ("*"))]
        //[EmailAddress(ErrorMessage = "Invalid format")]
        //[StringLength(30, ErrorMessage = "Max 30 chars")]
        //public string emailID { get; set; }



        //[Display(Name = "Enter Secuity Question")]
        //[Required(ErrorMessage = ("*"))]
        //public string securityQuestion { get; set; }


        //[Display(Name = "Enter Security Answer")]
        //[Required(ErrorMessage = ("*"))]
        //[StringLength(30, ErrorMessage = "Max 30 chars")]
        //public string securityAnswer { get; set; }

    }
}