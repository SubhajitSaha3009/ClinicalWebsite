using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class DoctorModel
    {
        [Display(Name = "Doctor ID")]
        [Required(ErrorMessage = "*")]
        public int doctorID { get; set; }

        [Display(Name = "Doctor Name")]
        [Required(ErrorMessage = "*")]
        [RegularExpression("([A-Za-z .()]*)", ErrorMessage = "invalid format")]
        [StringLength(60, ErrorMessage = "Max 60 characters")]
        public string doctorName { get; set; }


        [Display(Name = "Doctor Designation")]
       // [Required(ErrorMessage = "*")]
       //[RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        public string doctorDesignation { get; set; }


        [Display(Name = "Doctor Speciality")]
        [Required(ErrorMessage = "*")]
        [RegularExpression("([A-Za-z .();,/]*)", ErrorMessage = "invalid format")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        public string doctorSpeciality { get; set; }



        //[Display(Name = "Doctor Experience")]
        //[Required(ErrorMessage = "*")]
        ////[RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        //[StringLength(200, ErrorMessage = "Max 200 characters")]
        //public string doctorExperience { get; set; }



        [Display(Name = "Doctor Degree")]
        [Required(ErrorMessage = "*")]
        //[RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        public string doctorDegree { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Upload Doctor Image")]
        [StringLength(100, ErrorMessage = "Max 100 characters")]
        public string imageAddress { get; set; }

        //[Required(ErrorMessage = "*")]
        //public string doctorStatus { get; set; }

        [Display(Name = "Doctor Working Schedule")]
        [Required(ErrorMessage = "*")]
        // [RegularExpression("([A-Za-z ]*)", ErrorMessage = "invalid format")]
        [StringLength(1000, ErrorMessage = "Max 1000 characters")]
        public string workingTiming { get; set; }


        [Display(Name = "Phone Number For Appointment")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Only 10 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "invalid format")]
        [Required(ErrorMessage = "*")]
        public string numberForAppointment { get; set; }


        [Display(Name = "Alternate Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Only 10 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "invalid format")]
       // [Required(ErrorMessage = "*")]
        public string alternateNumberForAppointment { get; set; }


        [Display(Name = "Land Line Number")]
        //[Required(ErrorMessage = "*")]
        [RegularExpression("([0-9-()]*)", ErrorMessage = "invalid format")]
        [StringLength(12, ErrorMessage = "Max 15 characters")]
        public string LnumberForAppointment { get; set; }


        [Display(Name = "Last Modified Date")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "Modifier ID")]
        public int ModifierID { get; set; }


        //[Display(Name = "Select Working Day ")]
        //public string WorkingDay { get; set; }



    }
}