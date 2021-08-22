using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class FailedEventLogModel
    {
        public int Choice { get; set; }

        public int AdminID { get; set; }

        public string ExceptionStacktrace { get; set; }

        public FailedEventLogModel(int _Choice,int _AdminID)
        {
            this.Choice = _Choice;
            this.AdminID = _AdminID;
        }
    }
}