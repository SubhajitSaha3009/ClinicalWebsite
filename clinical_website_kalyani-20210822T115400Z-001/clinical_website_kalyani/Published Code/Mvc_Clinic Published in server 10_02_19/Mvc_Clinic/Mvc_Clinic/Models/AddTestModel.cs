using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class AddTestModel
    {
        public int TestId { get; set; }

        [Display(Name = "Test Name")]
        [Required(ErrorMessage = "*")]
        //[RegularExpression("(/^[ A-Za-z0-9_@./#&+-]*$/)", ErrorMessage = "invalid format")]
        [StringLength(80, ErrorMessage = "Max 80 characters")]
        public String TestName { get; set; }

        [Display(Name = "Test Description")]
        [Required(ErrorMessage = "*")]
        [StringLength(5000, ErrorMessage = "Max 5000 characters")]
        public String TestDesc { get; set; }

        [Display(Name = "Upload Equipment Image")]
        //[Required(ErrorMessage = "*")]
        public HttpPostedFileBase file { get; set; }

        [Display(Name = "Enter the timing for the test")]
        [Required(ErrorMessage = "*")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        public String TestSchedule { get; set; }



        [Display(Name = "Department Name")]
        [Required(ErrorMessage = "*")]
        public int testDeptID { get; set; }


        [Display(Name = "Sub-department Name")]
        [Required(ErrorMessage = "*")]
        public string testSub_DeptName { get; set; }

        public string testSub_DeptName1 { get; set; }

        public string imageAddress { get; set; }

    }
}