using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class CustomDeptAddModel
    {
        public List<TestDepartmentsModel> test_Deptlist { get; set; }

        public TestDepartmentsModel test_dept { get; set; }

        public TestSub_DepartmentsModel test_subdept { get; set; }

        public List<TestSub_DepartmentsModel> test_subdeptList { get; set; }

    }
}