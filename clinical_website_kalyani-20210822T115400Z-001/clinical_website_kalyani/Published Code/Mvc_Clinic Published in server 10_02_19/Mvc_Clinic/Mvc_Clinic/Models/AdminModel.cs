using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class AdminModel
    {
        [Display(Name = "Admin ID")]
        [Required(ErrorMessage = "*")]
        public int adminID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "*")]
        [RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(60, ErrorMessage = "Max 60 characters")]
        public string adminName { get; set; }


        [Display(Name = "Gender")]
        [Required(ErrorMessage = "*")]
        [RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(10, ErrorMessage = "Max 10 characters")]
        public string adminGender { get; set; }


        [Display(Name = "DOB")]
        [Required(ErrorMessage = "*")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[RegularExpression("(0?\d|1[012])\/([012]?\d|3[01])\/\d{4}", ErrorMessage = "Invalid Date")]
      //  [DisplayFormat(DataFormatString="{0:d}",ApplyFormatInEditMode=true)]
        public string adminDoB { get; set; }


        [Display(Name = "Admin Email ID")]
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid format")]
        [StringLength(30, ErrorMessage = "Max 30 chars")]
        public string adminEmailID { get; set; }




        //[Required(ErrorMessage = "*")]
        [Display(Name = "Upload Admin Image")]
        [StringLength(100, ErrorMessage = "Max 100 characters")]
        public string imageAddress { get; set; }
    }
}