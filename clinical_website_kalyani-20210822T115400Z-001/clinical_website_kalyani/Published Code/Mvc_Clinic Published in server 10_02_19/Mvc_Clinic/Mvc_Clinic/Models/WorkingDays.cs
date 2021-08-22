using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mvc_Clinic.Models
{
    public class WorkingDays
    {
        [Display(Name = "Select Working Day ")]
        public string WorkingDay { get; set; }
        public bool isChecked { get; set; }
    }

    public class WorkingDaysList
    {
        public List<WorkingDays> lst_days { get; set; }
    }
}