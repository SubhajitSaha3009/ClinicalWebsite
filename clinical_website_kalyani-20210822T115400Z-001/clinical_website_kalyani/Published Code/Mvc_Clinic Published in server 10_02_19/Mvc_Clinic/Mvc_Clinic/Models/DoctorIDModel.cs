using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Clinic.Models
{
    public class DoctorIDModel
    {
        [Display(Name = "Doctor ID")]
        [Required(ErrorMessage = "*")]
        public int doctorID { get; set; }
    }
}