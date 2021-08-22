using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class FeedbackModel
    {
        [Display(Name = "Feedback ID")]
        [Required(ErrorMessage = "*")]
        public int feedbackID { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(20, ErrorMessage = "Max 20 characters")]
        public string name { get; set; }


        [Display(Name = "Contact Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Only 10 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "invalid format")]
        [Required(ErrorMessage = "Please enter your contact number")]
        public string contactNumber { get; set; }


        //[Display(Name = "Subject")]
        //[Required(ErrorMessage = "*")]
        //public string subject { get; set; }

        [Display(Name = "Feedback Message")]
        [Required(ErrorMessage = "Please enter feedback")]
        [StringLength(300, ErrorMessage = "Max 300 characters")]
        public string feedbackMessage { get; set; }


        [Display(Name = "Feedback Status")]
        //[Required(ErrorMessage = "*")]
        [StringLength(10, ErrorMessage = "Max 10 characters")]
        public string feedbackStatus { get; set; }
    }
}