using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Clinic.Models;
using System.Web.Security;
using System.Data;
using System.IO;
using PagedList.Mvc;
using PagedList;
//using Mvc_Clinic.Models;

namespace Mvc_Clinic.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        // AdminDAL dal = new AdminDAL();
        HomeDAL hDAL = new HomeDAL();
        List<DoctorModel> list_doctors = new List<DoctorModel>();

        AdminDAL dal = new AdminDAL();
        //public ActionResult UnderConstruction()
        //{
        //    return View();
        //}
        public ActionResult Index()
        {
            try
            {
                List<FeedbackModel> lst = hDAL.showVisibleFeedback();
                lst = lst.OrderBy(x => x.feedbackMessage.Length).ToList();
                lst.Reverse();
                return View(lst);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }



        [HttpGet]
        public ActionResult AllTests(int id)
        {
            try
            {
                List<AddTestModel> list_allTests = new List<AddTestModel>();
                list_allTests = hDAL.GetAllTests(id);
                return View(list_allTests);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }



        [HttpGet]
        public ActionResult DoctorsList(int? page)
        {
            try
            {
                List<SelectListItem> lst_days = new List<SelectListItem>();
                lst_days.Add(new SelectListItem { Text = "Select by Day", Value = "" });
                lst_days.Add(new SelectListItem { Text = "Monday", Value = "Monday" });
                lst_days.Add(new SelectListItem { Text = "Tuesday", Value = "Tuesday" });
                lst_days.Add(new SelectListItem { Text = "Wednesday", Value = "Wednesday" });
                lst_days.Add(new SelectListItem { Text = "Thursday", Value = "Thursday" });
                lst_days.Add(new SelectListItem { Text = "Friday", Value = "Friday" });
                lst_days.Add(new SelectListItem { Text = "Saturday", Value = "Saturday" });
                lst_days.Add(new SelectListItem { Text = "Sunday", Value = "Sunday" });

                ViewBag.days = lst_days;

                ViewBag.SearchResult = TempData["SearchResult"];

                list_doctors = hDAL.SearchAllDoctors("", "", "");
                return View(list_doctors.ToPagedList(page ?? 1, 6));
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult DoctorsList(string SearchDoctor, string SearchSpeciality, string SearchDay, int? page)
        {
            try
            {
                List<SelectListItem> lst_days = new List<SelectListItem>();
                lst_days.Add(new SelectListItem { Text = "Select by Day", Value = "" });
                lst_days.Add(new SelectListItem { Text = "Monday", Value = "Monday" });
                lst_days.Add(new SelectListItem { Text = "Tuesday", Value = "Tuesday" });
                lst_days.Add(new SelectListItem { Text = "Wednesday", Value = "Wednesday" });
                lst_days.Add(new SelectListItem { Text = "Thursday", Value = "Thursday" });
                lst_days.Add(new SelectListItem { Text = "Friday", Value = "Friday" });
                lst_days.Add(new SelectListItem { Text = "Saturday", Value = "Saturday" });
                lst_days.Add(new SelectListItem { Text = "Sunday", Value = "Sunday" });

                ViewBag.days = lst_days;

                ModelState.Clear();

                list_doctors = hDAL.SearchAllDoctors(SearchDoctor, SearchSpeciality, SearchDay);
                if (list_doctors.Count == 0)
                {
                    if (SearchDoctor != "" && SearchSpeciality != "" && SearchDay != null)
                    {
                        ViewBag.noMatchFound = "No Match Found For Doctor's name '" + SearchDoctor + "' and Doctor's Speciality '" + SearchSpeciality + "'" + " on '" + SearchDay + "'";
                    }
                    else if (SearchDoctor != "" && SearchSpeciality != "")
                    {
                        ViewBag.noMatchFound = "No Match Found For Doctor's name '" + SearchDoctor + "'" + " and Doctor's Speciality '" + SearchSpeciality + "'";
                    }
                    else if (SearchDoctor != "" && SearchDay != "")
                    {
                        ViewBag.noMatchFound = "No Match Found For Doctor's name '" + SearchDoctor + "'" + " on '" + SearchDay + "'";
                    }
                    else if (SearchSpeciality != "" && SearchDay != "")
                    {
                        ViewBag.noMatchFound = "No Match Found For Doctor's Speciality '" + SearchSpeciality + "'" + " on '" + SearchDay + "'";
                    }
                    else if (SearchDoctor != "")
                    {
                        ViewBag.noMatchFound = "No Match Found For Doctor's name '" + SearchDoctor + "'";
                    }
                    else if (SearchSpeciality != "")
                    {
                        ViewBag.noMatchFound = "No Match Found For Doctor's Speciality '" + SearchSpeciality + "'";
                    }
                    else if (SearchDay != "")
                    {
                        ViewBag.noMatchFound = "No Match Found on '" + SearchDay + "'";
                    }
                }

                else
                {
                    if (SearchDoctor == "" && SearchSpeciality == "" && SearchDay == "")
                    {
                        ViewBag.SearchResult = "Showing search result of all available Doctors";
                    }

                    else if (SearchDoctor != "" && SearchSpeciality != "" && SearchDay != null)
                    {
                        ViewBag.SearchResult = "Showing search result of Doctor's name '" + SearchDoctor + "'" + " and Doctor's Speciality '" + SearchSpeciality + "'" + " on '" + SearchDay + "'";
                    }
                    else if (SearchDoctor != "" && SearchSpeciality != "")
                    {
                        ViewBag.SearchResult = "Showing search result of Doctor's name '" + SearchDoctor + "'" + " and Doctor's Speciality '" + SearchSpeciality + "'";
                    }
                    else if (SearchDoctor != "" && SearchDay != "")
                    {
                        ViewBag.SearchResult = "Showing search result of Doctor's name '" + SearchDoctor + "'" + " on '" + SearchDay + "'";
                    }
                    else if (SearchSpeciality != "" && SearchDay != "")
                    {
                        ViewBag.SearchResult = "Showing search result of Doctor's Speciality '" + SearchSpeciality + "'" + " on '" + SearchDay + "'";
                    }
                    else if (SearchDoctor != "")
                    {
                        ViewBag.SearchResult = "Showing search result of Doctor's name '" + SearchDoctor + "'";
                    }
                    else if (SearchSpeciality != "")
                    {
                        ViewBag.SearchResult = "Showing search result of Doctor's Speciality '" + SearchSpeciality + "'";
                    }
                    else if (SearchDay != "")
                    {
                        ViewBag.SearchResult = "Showing search result on '" + SearchDay + "'";
                    }
                }

                return View(list_doctors.ToPagedList(page ?? 1, 6));
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }


        //I have changed the return type
        public ActionResult GetDoctors(string searchTerm)
        {
            try
            {
                DataSet ds = hDAL.GetDoctors(searchTerm);
                List<DoctorModel> searchlist = new List<DoctorModel>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchlist.Add(new DoctorModel
                    {
                        doctorName = dr["doctorName"].ToString()
                    });
                }
                return Json(searchlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        //I have changed the return type
        public ActionResult GetDoctorsSpeciality(string searchTerm)
        {
            try
            {
                DataSet ds = hDAL.GetDoctorsSpeciality(searchTerm);
                List<DoctorModel> searchlist = new List<DoctorModel>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchlist.Add(new DoctorModel
                    {
                        doctorSpeciality = dr["doctorSpeciality"].ToString()
                    });
                }
                return Json(searchlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        //public JsonResult GetDoctors(string search)
        //{
        //    ClinicEntities ce_db = new ClinicEntities();
        //    List<DoctorIDNameModel> allSearch = ce_db.Doctors.Where(x => x.doctorName.StartsWith(search)).Select(x => new DoctorIDNameModel
        //    {
        //        doctorID = x.doctorID,
        //        doctorName = x.doctorName
        //    }).ToList();

        //    return new JsonResult { Data = allSearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}





        ////////////////////////////////////////// Souvik 20/10 ////////////////////////////////////
        [HttpGet]
        public ActionResult ShowDocSchedule(int doctorID)
        {
            try
            {
                DoctorModel doc = hDAL.showDocSchedule(doctorID);
                //string si = "/DoctorImages/" + doctorID + Path.GetExtension(img.FileName);
                //ViewBag.ImageAddress = si;
                ViewBag.Desc = doc.doctorName + " is leading " + doc.doctorSpeciality + " in " + doc.doctorDesignation + ". Book your Appointment today!";
                ViewBag.Title1 = doc.doctorName.ToString();
                ViewBag.Url = "http://www.mitalimemorial.in/Home/ShowDocSchedule?doctorID=" + doc.doctorID;
                //If Doctor iamge is not available default image will be selected at sharing
                if(doc.imageAddress=="")
                {
                    doc.imageAddress = "/DoctorImages/doctor.jpg";
                }
                ViewBag.image = doc.imageAddress;


                return View(doc);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }


        public ActionResult GetDevelopers()
        {
            return View();
        }










        /// <summary>
        /// /////////////////////////////////souvik 24-10/////////////////////////////////////////
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SendFeedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendFeedback(FeedbackModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (hDAL.sendFeedback(model))
                    {
                        ViewBag.msg = "Feedback sent";
                        ModelState.Clear();
                        //return Json(new { Success = true, Message = "" });
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.msg = "Feedback not sent";
                        ModelState.Clear();
                        return RedirectToAction("Error");
                    }
                }
                else
                {
                    ViewBag.msg = "Feedback not sent";
                    ModelState.Clear();
                    // return Json(new { Success = false, Message = "" }); 
                    return RedirectToAction("Error");
                }
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }


        //public JsonResult Success() { return Json(new { Success = true, Message = "" }); }
        //public JsonResult Error(string message) { return Json(new { Success = false, Message = message }); }



        public ActionResult FAQ()
        {
            return View();
        }

        [HttpGet]
        public ActionResult sendfeedbackNew()
        {
            return PartialView("SendFeedbackPartialView");
        }
        ////////////////////////////////////////subha 26/10////////////////////////////////////

        public ActionResult ShowAllFeedbackToAll()
        {
            try
            {
                List<FeedbackModel> lst = hDAL.showAllVisibleFeedback();
                return View(lst);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }


        /// <summary>
        /// subha 20/02
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TestDepartments()
        {
            try
            {
                AdminDAL dal = new AdminDAL();
                List<TestDepartmentsModel> obj = dal.getAllTestDepartments();
                return View(obj);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        public ActionResult OurStore()
        {
            return View();
        }


    }
}
