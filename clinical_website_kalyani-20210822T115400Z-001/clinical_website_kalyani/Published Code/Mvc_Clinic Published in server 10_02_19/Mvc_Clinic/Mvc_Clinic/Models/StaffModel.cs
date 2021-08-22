using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class StaffModel
    {
        public string Staffid { get; set; }
        
        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        public string StaffName { get; set; }

        [Display(Name = "Staff Gender")]
        [Required(ErrorMessage = "*")]
        public string StaffGender { get; set; }


        [Display(Name = "Staff DoB")]
        [Required(ErrorMessage = "*")]
        public string dob { get; set; }


        [Display(Name = "Staff Address")]
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "Max 100 chars")]
        public string StaffAddress { get; set; }

        [Display(Name = "Staff Mobile No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Only 10 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "invalid format")]
        [Required(ErrorMessage = "*")]
        public string StaffMobileNo { get; set; }

        [Display(Name = "Staff Email ID")]
        [Required(ErrorMessage = "*")]
        //[EmailAddress(ErrorMessage = "Invalid format")]
        [StringLength(30, ErrorMessage = "Max 30 chars")]
        
        public string StaffEmailID { get; set; }

        public string StaffStatus { get; set; }

        [Display(Name = "Staff Joining Date")]
        [Required(ErrorMessage = "*")]
        public string StJoiDate { get; set; }

        [Display(Name = "Staff Leaving Date")]
        //[Required(ErrorMessage = "*")]
        public string StLeaDate { get; set; }

    }
}