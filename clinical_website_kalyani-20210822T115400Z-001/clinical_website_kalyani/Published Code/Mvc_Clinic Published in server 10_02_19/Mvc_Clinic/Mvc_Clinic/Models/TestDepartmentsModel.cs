using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class TestDepartmentsModel
    {
        [Display(Name = "Department ID")]
        [Required(ErrorMessage = "*")]
        public int testDeptID { get; set; }


        [Display(Name = "Department Name")]
        [Required(ErrorMessage = "*")]
        public string testDeptName { get; set; }

    }
}