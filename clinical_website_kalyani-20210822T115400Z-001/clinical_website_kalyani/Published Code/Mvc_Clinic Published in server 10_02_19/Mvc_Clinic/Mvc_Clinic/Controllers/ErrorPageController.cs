﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_Clinic.Controllers
{
    public class ErrorPageController : Controller
    {
        //
        // GET: /ErrorPage/

        public ActionResult ErrorMessage()
        {
            return View();
        }

    }
}
