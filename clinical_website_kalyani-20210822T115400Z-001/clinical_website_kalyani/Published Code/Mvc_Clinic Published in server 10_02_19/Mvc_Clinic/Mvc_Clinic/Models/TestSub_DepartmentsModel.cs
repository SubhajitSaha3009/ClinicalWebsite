using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class TestSub_DepartmentsModel
    {
        [Display(Name = "Sub Department ID")]
        [Required(ErrorMessage = "*")]
        public int testSub_DeptID { get; set; }


        [Display(Name = "Sub Department Name")]
        [Required(ErrorMessage = "*")]
        public string testSub_DeptName { get; set; }

        [Display(Name = "Corresponding Department ID")]
        [Required(ErrorMessage = "*")]
        public int testDeptID { get; set; }
    }
}