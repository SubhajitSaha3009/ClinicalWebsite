using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Clinic.Models;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Text;
using PagedList.Mvc;
using PagedList;
using System.Diagnostics;

namespace Mvc_Clinic.Controllers
{
    [Authorize(Roles = "Admin")]
    //ghoto test
    public class AdminController : Controller
    {
        

        //
        // GET: /Admin/

        IAdminDAL dal = new AdminDAL();
        IHomeDAL hDAL = new HomeDAL();
        //static HttpPostedFileBase image;
        private static int flagforerror = 0;
        private static int flagforerrorDoc = 0;

        public ActionResult Index()
        {
            try
            {
                AdminModel model = dal.searchAdmin(Convert.ToInt32(User.Identity.Name));

                ViewBag.id = model.adminID;
                ViewBag.name = model.adminName;
                ViewBag.adminGender = model.adminGender;
                ViewBag.adminDoB = model.adminDoB;
                ViewBag.email = model.adminEmailID;


                //string si = "/AdminImages/" + model.adminID + ".jpg";
                ViewBag.ImageAddress = model.imageAddress;
                ViewBag.updmsg = TempData["updatedmsg"];
                return View();
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace=e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }





        private bool isValidContentProfilePic(String ContentType)
        {
            return ContentType.Equals("image/jpeg") || ContentType.Equals("image/png") || ContentType.Equals("image/jpg");
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(AdminModel model, HttpPostedFileBase imageAddress)
        {
            try
            {

                if (imageAddress != null)
                {
                    if (!isValidContentProfilePic(imageAddress.ContentType))
                    {
                        ViewBag.Error = "Only jpg and png are allowed";
                        return View();
                    }

                    if (imageAddress.ContentLength > 2000000)
                    {
                        ViewBag.Error = "File Size Must Be Less Than 2mb";
                        return View();
                    }
                }

                if (ModelState.IsValid)
                {

                    string idpassword = dal.addAdmin(model, imageAddress);
                    {
                        if (idpassword != null)
                        {
                            if (idpassword[0] != Convert.ToChar("x"))
                            {
                                imageAddress.SaveAs(Server.MapPath("/AdminImages/" + model.adminID + Path.GetExtension(imageAddress.FileName)));

                            }
                            else
                            {
                                idpassword = idpassword.Remove(0, 1);
                            }
                            string[] arr = idpassword.Split(' ');


                            ViewBag.id = arr[0];
                            ViewBag.password = arr[1];

                            return View("AdminAdded");
                        }
                        else
                        {
                            ViewBag.Redirectionmsg = "Admin was not added Successfully.";
                            ViewBag.Btnlbl = "Add Admin";
                            ViewBag.BtnAction = "AddAdmin";
                            return View("Fail");
                        }
                    }

                }
                else
                {
                    ViewBag.Redirectionmsg = "Admin was not added Successfully.";
                    ViewBag.Btnlbl = "Add Admin";
                    ViewBag.BtnAction = "AddAdmin";
                    return View("Fail");
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


        [HttpGet]
        public ActionResult AddDoctor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDoctor(DoctorModel model, HttpPostedFileBase imageAddress)
        {
            try
            {
                Random random = new Random();
                int c = random.Next(11111, 99999);

                if (imageAddress != null)
                {
                    if (!isValidContentProfilePic(imageAddress.ContentType))
                    {
                        ViewBag.Error = "Only jpg and png are allowed";
                        return View();
                    }
                    if (imageAddress.ContentLength > 2000000)
                    {
                        ViewBag.Error = "File Size Must Be Less Than 2mb";
                        return View();
                    }


                    model.imageAddress = "/DoctorImages/" + model.doctorName + "" + c + Path.GetExtension(imageAddress.FileName);
                    imageAddress.SaveAs(Server.MapPath(model.imageAddress));

                    //imageAddress.SaveAs(Server.MapPath("/DoctorImages/" + model.doctorID + Path.GetExtension(imageAddress.FileName)));
                }

                ViewBag.dID = model.doctorID;
                ViewBag.dName = model.doctorName;
                ViewBag.dDesignation = model.doctorDesignation;
                ViewBag.dSpeciality = model.doctorSpeciality;
                ViewBag.degree = model.doctorDegree;
                if (model.imageAddress != null)
                {
                    ViewBag.img = "/DoctorImages/" + model.doctorName + "" + c + Path.GetExtension(imageAddress.FileName);
                }
                else
                {
                    ViewBag.img = "";
                }

                ViewBag.workingTime = model.workingTiming;
                if (model.alternateNumberForAppointment != null)
                    ViewBag.numAppoin = model.numberForAppointment + "," + model.alternateNumberForAppointment;
                else
                    ViewBag.numAppoin = model.numberForAppointment;

                ViewBag.Lnum = model.LnumberForAppointment;
                ViewBag.StatusDoc = "Add";

                /// <summary>
                /// Substring the doc designation and degree to preview
                /// </summary>
                /// <developedBy>
                /// Subha 26-08-2018
                /// </developedBy>
                /// Begin Logic

                string intSpacedString = model.doctorSpeciality;
                string tempDoctorSpeciality = "";
                char[] spaceSeparator = new char[] { ' ' };
                string[] result;
                result = intSpacedString.Split(spaceSeparator, StringSplitOptions.None);
                int len = 0;
                if (result.Length > 4)
                {
                    foreach (string str in result)
                    {
                        if (len < 4)
                        {
                            tempDoctorSpeciality += result[len] + " ";
                            len++;
                        }
                        else { break; }
                    }
                    tempDoctorSpeciality += "...";
                }
                else
                    tempDoctorSpeciality = model.doctorSpeciality;

                string intSpacedStringDeg = model.doctorDegree;
                string tempDoctorDegree = "";
                char[] spaceSeparatorDeg = new char[] { ' ' };
                string[] resultDeg;
                resultDeg = intSpacedStringDeg.Split(spaceSeparatorDeg, StringSplitOptions.None);
                int lenDeg = 0;
                if (resultDeg.Length > 4)
                {
                    foreach (string str in resultDeg)
                    {
                        if (lenDeg < 4)
                        {
                            tempDoctorDegree += resultDeg[lenDeg] + " ";
                            lenDeg++;
                        }
                        else { break; }
                    }
                    tempDoctorDegree += "...";
                }
                else
                    tempDoctorDegree = model.doctorDegree;

                ViewBag.degreeSorted = tempDoctorDegree;
                ViewBag.dSpecialitySorted = tempDoctorSpeciality;

                TempData["AddDID"] = model.doctorID;
                TempData["AddDName"] = model.doctorName;
                TempData["AddDesg"] = model.doctorDesignation;
                TempData["AddDSpec"] = model.doctorSpeciality;
                TempData["AddDDegree"] = model.doctorDegree;
                TempData["AddDImg"] = model.imageAddress;
                TempData["AddDWorT"] = model.workingTiming;
                TempData["AddDAppNum"] = ViewBag.numAppoin;
                TempData["AddDLNum"] = model.LnumberForAppointment;

                return View("IntermediateDoctor");


                //if (ModelState.IsValid)
                //{
                //    int count = dal.addDoctors(model, imageAddress);

                //    if (count == 1)
                //    {
                //        imageAddress.SaveAs(Server.MapPath("/DoctorImages/" + model.doctorID + Path.GetExtension(imageAddress.FileName)));
                //        ViewBag.Redirectionmsg = "Doctor Added Successfully.";
                //        ViewBag.Btnlbl = "Add Another Doctor";
                //        ViewBag.BtnAction = "AddDoctor";
                //        return View("Success");
                //    }

                //    else if (count == 0)
                //    {
                //        ViewBag.Redirectionmsg = "Doctor not Added.";
                //        ViewBag.Btnlbl = "Add Doctor";
                //        ViewBag.BtnAction = "AddDoctor";
                //        return View("Fail");
                //    }
                //    else if (count == 2)
                //    {
                //        ViewBag.Redirectionmsg = "Doctor Added Successfully.";
                //        ViewBag.Btnlbl = "Add Another Doctor";
                //        ViewBag.BtnAction = "AddDoctor";
                //        return View("Success");
                //    }
                //    else
                //    {
                //        ViewBag.Redirectionmsg = "Doctor not Added.";
                //        ViewBag.Btnlbl = "Add Doctor";
                //        ViewBag.BtnAction = "AddDoctor";
                //        return View("Fail");
                //    }
                //}
                //else
                //{
                //    ViewBag.Redirectionmsg = "Doctor not Added.";
                //    ViewBag.Btnlbl = "Add Doctor";
                //    ViewBag.BtnAction = "AddDoctor";
                //    return View("Fail");
                //}
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }

        }

        public ActionResult FinalAddDocSubmission()
        {
            DoctorModel d_model = new DoctorModel();

            //d_model.doctorID = Convert.Toin(TempData["AddDID"]);
            d_model.doctorName = Convert.ToString(TempData["AddDName"]);
            d_model.doctorDesignation = Convert.ToString(TempData["AddDesg"]);
            d_model.doctorSpeciality = Convert.ToString(TempData["AddDSpec"]);
            d_model.doctorDegree = Convert.ToString(TempData["AddDDegree"]);
            d_model.imageAddress = Convert.ToString(TempData["AddDImg"]);
            d_model.workingTiming = Convert.ToString(TempData["AddDWorT"]);
            d_model.numberForAppointment = Convert.ToString(TempData["AddDAppNum"]);
            d_model.LnumberForAppointment = Convert.ToString(TempData["AddDLNum"]);

            if (ModelState.IsValid)
            {
                d_model.ModifierID = Convert.ToInt32(User.Identity.Name);

                //souvik 16/9/18
                string docinfo = dal.addDoctors(d_model);
                String[] arrDoc = docinfo.Split('_');
                int count = Convert.ToInt32(arrDoc[0]);
                d_model.doctorID = Convert.ToInt32( arrDoc[1]);

                if (count == 1)
                {
                    //ViewBag.Redirectionmsg = "Doctor Added Successfully.";
                    //ViewBag.Btnlbl = "Add Another Doctor";
                    //ViewBag.BtnAction = "AddDoctor";
                    //return View("Success");

                    // souvik 26-8-18
                    return RedirectToAction("AddWorkingDayofDoctor", new { doctorid = d_model.doctorID });
                    
                }

                else if (count == 0)
                {
                    System.IO.File.Delete(Server.MapPath(d_model.imageAddress));
                    ViewBag.Redirectionmsg = "Doctor not Added.";
                    ViewBag.Btnlbl = "Try Again";
                    ViewBag.BtnAction = "AddDoctor";
                    return View("Fail");
                }
                else if (count == 2)
                {
                    //ViewBag.Redirectionmsg = "Doctor Added Successfully.";
                    //ViewBag.Btnlbl = "Add Another Doctor";
                    //ViewBag.BtnAction = "AddDoctor";
                    //return View("Success");

                    // souvik 26-8-18
                    return RedirectToAction("AddWorkingDayofDoctor", new { doctorid = d_model.doctorID });
                }
                else
                {
                    System.IO.File.Delete(Server.MapPath(d_model.imageAddress));
                    ViewBag.Redirectionmsg = "Doctor not Added.";
                    ViewBag.Btnlbl = "Try Again";
                    ViewBag.BtnAction = "AddDoctor";
                    return View("Fail");
                }
            }
            else
            {
                System.IO.File.Delete(Server.MapPath(d_model.imageAddress));
                ViewBag.Redirectionmsg = "Doctor not Added.";
                ViewBag.Btnlbl = "Try Again";
                ViewBag.BtnAction = "AddDoctor";
                return View("Fail");
            }
        }




        [HttpGet]
        public ActionResult ViewAllDoctors(int? page)
        {
            try
            {
                List<SearchDoctorModel> list_doctors = new List<SearchDoctorModel>();

                list_doctors = dal.SearchAllDoctors(null);
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
        public ActionResult ViewAllDoctors(string SearchDoctor, int? page)
        {
            try
            {
                List<SearchDoctorModel> list_doctors = new List<SearchDoctorModel>();
                list_doctors = dal.SearchAllDoctors(SearchDoctor);
                if (list_doctors.Count == 0)
                {
                    ViewBag.noMatchFound = "No match found for '" + SearchDoctor + "'";
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


        [HttpGet]
        public ActionResult UpdateDoctor(int doctorID)
        {
            try
            {
                //int docID = Convert.ToInt32(TempData["doctorID"]);
                if (doctorID == 0 || doctorID.ToString().Length < 4 || doctorID.ToString().Length == 0)
                {
                    return RedirectToAction("ViewAllDoctors");
                }

                //string si = "/DoctorImages/" + docID + ".jpg";


                //if(File.Exists(Server.MapPath(si)))
                //{

                //}

                SearchDoctorModel doc = dal.searchDoctor(doctorID);
                ViewBag.id = doc.doctorID;
                ViewBag.name = doc.doctorName;
                ViewBag.desig = doc.doctorDesignation;
                ViewBag.specialty = doc.doctorSpeciality;
                //ViewBag.Exp = doc.doctorExperience;
                ViewBag.deg = doc.doctorDegree;
                string s = doc.doctorStatus;
                ViewBag.wtiming = doc.workingTiming;
                ViewBag.no = doc.numberForAppointment;
                ViewBag.ano = doc.alternateNumberForAppointment;
                ViewBag.lno = doc.LnumberForAppointment;
                ViewBag.ImageAddress = doc.imageAddress;
                ViewBag.LastModifieddate = doc.LastModifiedDate;
                ViewBag.ModifierID = doc.ModifierID;
                TempData["notChangedImagedoc"] = doc.imageAddress;

                if (s == "Present")
                {
                    List<SelectListItem> list_DocStatus = new List<SelectListItem>();
                    //list_DocStatus.Add(new SelectListItem { Text = "Select", Value = "" });
                    list_DocStatus.Add(new SelectListItem { Text = "Present", Value = "Present" });
                    list_DocStatus.Add(new SelectListItem { Text = "Left", Value = "Left" });


                    ViewBag.stat = list_DocStatus;
                }
                else
                {
                    List<SelectListItem> list_DocStatus = new List<SelectListItem>();
                    //list_DocStatus.Add(new SelectListItem { Text = "Select", Value = "" });
                    list_DocStatus.Add(new SelectListItem { Text = "Left", Value = "Left" });
                    list_DocStatus.Add(new SelectListItem { Text = "Present", Value = "Present" });


                    ViewBag.stat = list_DocStatus;

                }

                ViewBag.Err = TempData["Errormsg"];
                TempData["doctorID"] = doctorID;
                return View();
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
        public ActionResult UpdateDoctor(SearchDoctorModel model, HttpPostedFileBase imageAddress)
        {
            try
            {
                if (imageAddress != null)
                {
                    if (imageAddress.ContentLength > 2000000)
                    {
                        flagforerrorDoc = 5;
                        ViewBag.Error = "File Size Must Be Less Than 2mb";
                        int doctorID = Convert.ToInt32(TempData["doctorID"]);
                        if (doctorID == 0)
                        {
                            return RedirectToAction("ViewAllDoctors");
                        }

                        SearchDoctorModel doc = dal.searchDoctor(doctorID);
                        ViewBag.id = doc.doctorID;
                        ViewBag.name = doc.doctorName;
                        ViewBag.desig = doc.doctorDesignation;
                        ViewBag.specialty = doc.doctorSpeciality;
                        //ViewBag.Exp = doc.doctorExperience;
                        ViewBag.deg = doc.doctorDegree;
                        string s = doc.doctorStatus;
                        ViewBag.wtiming = doc.workingTiming;
                        ViewBag.no = doc.numberForAppointment;
                        ViewBag.ano = doc.alternateNumberForAppointment;
                        ViewBag.lno = doc.LnumberForAppointment;
                        ViewBag.ImageAddress = doc.imageAddress;
                        ViewBag.LastModifieddate = doc.LastModifiedDate;
                        ViewBag.ModifierID = doc.ModifierID;


                        List<SelectListItem> list_DocStatus = new List<SelectListItem>();
                        if (s == "Present")
                        {

                            //list_DocStatus.Add(new SelectListItem { Text = "Select", Value = "" });
                            list_DocStatus.Add(new SelectListItem { Text = "Present", Value = "Present" });
                            list_DocStatus.Add(new SelectListItem { Text = "Left", Value = "Left" });


                            ViewBag.stat = list_DocStatus;
                        }
                        else
                        {
                            //List<SelectListItem> list_DocStatus = new List<SelectListItem>();
                            //list_DocStatus.Add(new SelectListItem { Text = "Select", Value = "" });
                            list_DocStatus.Add(new SelectListItem { Text = "Left", Value = "Left" });
                            list_DocStatus.Add(new SelectListItem { Text = "Present", Value = "Present" });


                            ViewBag.stat = list_DocStatus;

                        }

                        ViewBag.Err = TempData["Errormsg"];
                        TempData["doctorID"] = doctorID;
                        return View();
                    }
                    if (!isValidContentProfilePic(imageAddress.ContentType))
                    {
                        ViewBag.Err = "Only jpg and png are allowed";


                        int doctorID = Convert.ToInt32(TempData["doctorID"]);

                        SearchDoctorModel doc = dal.searchDoctor(doctorID);
                        ViewBag.id = doc.doctorID;
                        ViewBag.name = doc.doctorName;
                        ViewBag.desig = doc.doctorDesignation;
                        ViewBag.specialty = doc.doctorSpeciality;
                        //ViewBag.Exp = doc.doctorExperience;
                        ViewBag.deg = doc.doctorDegree;
                        string s = doc.doctorStatus;
                        ViewBag.wtiming = doc.workingTiming;
                        ViewBag.no = doc.numberForAppointment;
                        ViewBag.ano = doc.alternateNumberForAppointment;
                        ViewBag.lno = doc.LnumberForAppointment;
                        ViewBag.ImageAddress = doc.imageAddress;
                        ViewBag.LastModifieddate = doc.LastModifiedDate;
                        ViewBag.ModifierID = doc.ModifierID;

                        List<SelectListItem> list_DocStatus = new List<SelectListItem>();
                        if (s == "Present")
                        {

                            //list_DocStatus.Add(new SelectListItem { Text = "Select", Value = "" });
                            list_DocStatus.Add(new SelectListItem { Text = "Present", Value = "Present" });
                            list_DocStatus.Add(new SelectListItem { Text = "Left", Value = "Left" });


                            ViewBag.stat = list_DocStatus;
                        }
                        else
                        {
                            //List<SelectListItem> list_DocStatus = new List<SelectListItem>();
                            //list_DocStatus.Add(new SelectListItem { Text = "Select", Value = "" });
                            list_DocStatus.Add(new SelectListItem { Text = "Left", Value = "Left" });
                            list_DocStatus.Add(new SelectListItem { Text = "Present", Value = "Present" });


                            ViewBag.stat = list_DocStatus;

                        }


                        return View();
                    }
                    else
                    {

                        imageAddress.SaveAs(Server.MapPath("~/DoctorImages/" + model.doctorID + Path.GetExtension(imageAddress.FileName)));

                        ViewBag.dID = model.doctorID;
                        ViewBag.dName = model.doctorName;
                        ViewBag.dDesignation = model.doctorDesignation;
                        ViewBag.dSpeciality = model.doctorSpeciality;
                        ViewBag.degree = model.doctorDegree;
                        if (model.imageAddress != null)
                        {
                            ViewBag.img = "/DoctorImages/" + model.doctorID + Path.GetExtension(imageAddress.FileName);
                            model.imageAddress = ViewBag.img;
                        }
                        else
                        {
                            ViewBag.img = "";
                        }

                        ViewBag.workingTime = model.workingTiming;
                        ViewBag.numAppoin = model.numberForAppointment + "," + model.alternateNumberForAppointment;
                        ViewBag.Lnum = model.LnumberForAppointment;
                        ViewBag.StatusDoc = "Update";

                        /// <summary>
                        /// Substring the doc designation and degree to preview
                        /// </summary>
                        /// <developedBy>
                        /// Subha 26-08-2018
                        /// </developedBy>
                        /// Begin Logic

                        string intSpacedString = model.doctorSpeciality;
                        string tempDoctorSpeciality = "";
                        char[] spaceSeparator = new char[] { ' ' };
                        string[] result;
                        result = intSpacedString.Split(spaceSeparator, StringSplitOptions.None);
                        int len = 0;
                        if (result.Length > 4)
                        {
                            foreach (string str in result)
                            {
                                if (len < 4)
                                {
                                    tempDoctorSpeciality += result[len] + " ";
                                    len++;
                                }
                                else { break; }
                            }
                            tempDoctorSpeciality += "...";
                        }
                        else
                            tempDoctorSpeciality = model.doctorSpeciality;

                        string intSpacedStringDeg = model.doctorDegree;
                        string tempDoctorDegree = "";
                        char[] spaceSeparatorDeg = new char[] { ' ' };
                        string[] resultDeg;
                        resultDeg = intSpacedStringDeg.Split(spaceSeparatorDeg, StringSplitOptions.None);
                        int lenDeg = 0;
                        if (resultDeg.Length > 4)
                        {
                            foreach (string str in resultDeg)
                            {
                                if (lenDeg < 4)
                                {
                                    tempDoctorDegree += resultDeg[lenDeg] + " ";
                                    lenDeg++;
                                }
                                else { break; }
                            }
                            tempDoctorDegree += "...";
                        }
                        else
                            tempDoctorDegree = model.doctorDegree;

                        ViewBag.degreeSorted = tempDoctorDegree;
                        ViewBag.dSpecialitySorted = tempDoctorSpeciality;

                        TempData["UpdateDID"] = model.doctorID;
                        TempData["UpdateDName"] = model.doctorName;
                        TempData["UpdateDesg"] = model.doctorDesignation;
                        TempData["UpdateDSpec"] = model.doctorSpeciality;
                        TempData["UpdateDDegree"] = model.doctorDegree;
                        TempData["UpdateDImg"] = model.imageAddress;
                        TempData["UpdateDWorT"] = model.workingTiming;
                        TempData["UpdateDAppNum"] = model.numberForAppointment + "," + model.alternateNumberForAppointment;
                        TempData["UpdateDLNum"] = model.LnumberForAppointment;
                        TempData["UpdateDocStatus"] = model.doctorStatus;

                        if (model.doctorStatus == "Left")
                        {
                            return RedirectToAction("FinalUpdateDocSubmission");
                        }
                        else
                        {
                            return View("IntermediateDoctor");
                        }
                    }
                }
                else
                {
                    ViewBag.dID = model.doctorID;
                    ViewBag.dName = model.doctorName;
                    ViewBag.dDesignation = model.doctorDesignation;
                    ViewBag.dSpeciality = model.doctorSpeciality;
                    ViewBag.degree = model.doctorDegree;
                    if (flagforerrorDoc == 5)
                    {
                        TempData["UpdateDImg"] = "";
                        ViewBag.img = "";
                    }
                    else
                    {
                        ViewBag.img = TempData["notChangedImagedoc"];
                        TempData["UpdateDImg"] = ViewBag.img;
                    }

                    ViewBag.workingTime = model.workingTiming;
                    ViewBag.numAppoin = model.numberForAppointment + "," + model.alternateNumberForAppointment;
                    ViewBag.Lnum = model.LnumberForAppointment;

                    ViewBag.StatusDoc = "Update";

                    /// <summary>
                    /// Substring the doc designation and degree to preview
                    /// </summary>
                    /// <developedBy>
                    /// Subha 26-08-2018
                    /// </developedBy>
                    /// Begin Logic

                    string intSpacedString = model.doctorSpeciality;
                    string tempDoctorSpeciality = "";
                    char[] spaceSeparator = new char[] { ' ' };
                    string[] result;
                    result = intSpacedString.Split(spaceSeparator, StringSplitOptions.None);
                    int len = 0;
                    if (result.Length > 4)
                    {
                        foreach (string str in result)
                        {
                            if (len < 4)
                            {
                                tempDoctorSpeciality += result[len] + " ";
                                len++;
                            }
                            else { break; }
                        }
                        tempDoctorSpeciality += "...";
                    }
                    else
                        tempDoctorSpeciality = model.doctorSpeciality;

                    string intSpacedStringDeg = model.doctorDegree;
                    string tempDoctorDegree = "";
                    char[] spaceSeparatorDeg = new char[] { ' ' };
                    string[] resultDeg;
                    resultDeg = intSpacedStringDeg.Split(spaceSeparatorDeg, StringSplitOptions.None);
                    int lenDeg = 0;
                    if (resultDeg.Length > 4)
                    {
                        foreach (string str in resultDeg)
                        {
                            if (lenDeg < 4)
                            {
                                tempDoctorDegree += resultDeg[lenDeg] + " ";
                                lenDeg++;
                            }
                            else { break; }
                        }
                        tempDoctorDegree += "...";
                    }
                    else
                        tempDoctorDegree = model.doctorDegree;

                    ViewBag.degreeSorted = tempDoctorDegree;
                    ViewBag.dSpecialitySorted = tempDoctorSpeciality;

                    TempData["UpdateDID"] = model.doctorID;
                    TempData["UpdateDName"] = model.doctorName;
                    TempData["UpdateDesg"] = model.doctorDesignation;
                    TempData["UpdateDSpec"] = model.doctorSpeciality;
                    TempData["UpdateDDegree"] = model.doctorDegree;

                    TempData["UpdateDWorT"] = model.workingTiming;
                    TempData["UpdateDAppNum"] = model.numberForAppointment + "," + model.alternateNumberForAppointment;
                    TempData["UpdateDLNum"] = model.LnumberForAppointment;
                    TempData["UpdateDocStatus"] = model.doctorStatus;

                    if (model.doctorStatus == "Left")
                    {
                        return RedirectToAction("FinalUpdateDocSubmission");
                    }
                    else
                    {
                        return View("IntermediateDoctor");
                    }

                    //if (dal.EditDoctor(model))
                    //{
                    //    ViewBag.Redirectionmsg = "Doctor Updated Successfully.";
                    //    ViewBag.Btnlbl = "Update Another Doctor";
                    //    ViewBag.BtnAction = "UpdateDoctor";
                    //    return View("Success");
                    //}
                    //else
                    //{
                    //    ViewBag.Redirectionmsg = "Doctor Not Updated Successfully.";
                    //    ViewBag.Btnlbl = "Try Again";
                    //    ViewBag.BtnAction = "UpdateDoctor";
                    //    return View("Fail");
                    //}
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


        public ActionResult FinalUpdateDocSubmission()
        {
            SearchDoctorModel d_model = new SearchDoctorModel();
            d_model.doctorID = Convert.ToInt32(TempData["UpdateDID"]);
            d_model.doctorName = Convert.ToString(TempData["UpdateDName"]);
            d_model.doctorDesignation = Convert.ToString(TempData["UpdateDesg"]);
            d_model.doctorSpeciality = Convert.ToString(TempData["UpdateDSpec"]);
            d_model.doctorDegree = Convert.ToString(TempData["UpdateDDegree"]);
            d_model.imageAddress = Convert.ToString(TempData["UpdateDImg"]);
            d_model.workingTiming = Convert.ToString(TempData["UpdateDWorT"]);
            d_model.numberForAppointment = Convert.ToString(TempData["UpdateDAppNum"]);
            d_model.LnumberForAppointment = Convert.ToString(TempData["UpdateDLNum"]);
            d_model.doctorStatus = Convert.ToString(TempData["UpdateDocStatus"]);
            d_model.ModifierID = Convert.ToInt32(User.Identity.Name);

            if (dal.EditDoctor(d_model))
            {

                ViewBag.Redirectionmsg = "Doctor Updated Successfully.";
                ViewBag.Btnlbl = "Update Another Doctor";
                ViewBag.BtnAction = "ViewAllDoctors";
                return View("Success");
            }
            else
            {
                ViewBag.Redirectionmsg = "Doctor Not Updated Successfully.";
                System.IO.File.Delete(Server.MapPath(d_model.imageAddress));
                ViewBag.Btnlbl = "Try Again";
                ViewBag.BtnAction = "ViewAllDoctors";
                return View("Fail");
            }

        }




        [HttpGet]
        public ActionResult UpdateAdminProfile()
        {
            try
            {
                int adminID = Convert.ToInt32(User.Identity.Name);
                //int adminID = 7005;
                AdminModel model = dal.searchAdmin(adminID);

                ViewBag.id = model.adminID;
                ViewBag.name = model.adminName;
                ViewBag.adminGender = model.adminGender;
                ViewBag.adminDoB = model.adminDoB;
                ViewBag.email = model.adminEmailID;
                ViewBag.ImageAddress = model.imageAddress;


                return View();
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
        public ActionResult UpdateAdminProfile(AdminModel model)
        {
            try
            {
                model.adminID = Convert.ToInt32(User.Identity.Name);
                if (dal.updateAdminProfile(model))
                {
                    TempData["updatedmsg"] = "Your profile is updated";
                    return RedirectToAction("Index");
                }
                return View("NotUpdated");
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult UpdatePasswordAdmin()
        {

            List<SelectListItem> lst_securityQuestion = new List<SelectListItem>();
            lst_securityQuestion.Add(new SelectListItem { Text = "Select", Value = "" });
            lst_securityQuestion.Add(new SelectListItem { Text = "What is your's first school name?", Value = "What is your's first school name?" });
            lst_securityQuestion.Add(new SelectListItem { Text = "What is your pet’s name?", Value = "What is your pet’s name?" });
            lst_securityQuestion.Add(new SelectListItem { Text = "In what city or town does your nearest sibling live?", Value = "In what city or town does your nearest sibling live?" });
            lst_securityQuestion.Add(new SelectListItem { Text = "What is your favorite food?", Value = "What is your favorite food?" });

            ViewBag.securityQuestion1 = lst_securityQuestion;

            return View();
        }
        [HttpPost]
        public ActionResult UpdatePasswordAdmin(ChangePasswordAdmin model)
        {
            try
            {
                List<SelectListItem> lst_securityQuestion = new List<SelectListItem>();
                lst_securityQuestion.Add(new SelectListItem { Text = "Select", Value = "" });
                lst_securityQuestion.Add(new SelectListItem { Text = "What is your's first school name?", Value = "What is your's first school name?" });
                lst_securityQuestion.Add(new SelectListItem { Text = "What is your pet’s name?", Value = "What is your pet’s name?" });
                lst_securityQuestion.Add(new SelectListItem { Text = "In what city or town does your nearest sibling live?", Value = "In what city or town does your nearest sibling live?" });
                lst_securityQuestion.Add(new SelectListItem { Text = "What is your favorite food?", Value = "What is your favorite food?" });

                ViewBag.securityQuestion1 = lst_securityQuestion;


                int adminID = Convert.ToInt32(User.Identity.Name);
                //LoginDAL dal = new LoginDAL();
                if (dal.updatePassword(adminID, model))
                {
                    TempData["updatedmsg"] = "Password is updated";
                    return RedirectToAction("Index");
                }
                

                return View("NotUpdated");
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        public ActionResult Logout()
        {
            ////Change Atanu 16/09
            LoginLogModel mod = new LoginLogModel(1,Convert.ToInt32(User.Identity.Name));
            if(dal.LogLoginDetails(mod))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "HeadOfClinic");
            }
            else
            {
                return View("Fail");
            }
            
            
        }






        [HttpGet]
        public ActionResult AddStaff()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddStaff(StaffModel Smod)
        {
            try
            {
                String Flag = dal.AddStaff(Smod, Convert.ToInt32(User.Identity.Name));
                if (Flag.Equals("Fail"))
                {
                    ViewBag.Redirectionmsg = "Staff was not added Successfully.";
                    ViewBag.Btnlbl = "Try Again";
                    ViewBag.BtnAction = "AddStaff";
                    return View("Fail");
                }
                else
                {
                    ViewBag.Redirectionmsg = "Staff added Successfully.";
                    ViewBag.Btnlbl = "Add  Staff";
                    ViewBag.BtnAction = "AddStaff";
                    return View("Success");
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

        [HttpGet]
        public ActionResult GetActiveStaff()
        {
            try
            {
                List<StaffModel> StM = dal.GetAllActiveStaff();
                return View(StM);
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
        public ActionResult UpdateStaffById(int id)
        {
            try
            {
                if (id != 0)
                {
                    StaffModel sm = dal.SearchStaff(id);
                    ViewBag.Stid = sm.Staffid;
                    ViewBag.StName = sm.StaffName;
                    ViewBag.Stdob = sm.dob;
                    ViewBag.StaffGender = sm.StaffGender;
                    ViewBag.StAddress = sm.StaffAddress;
                    ViewBag.StMobNo = sm.StaffMobileNo;
                    ViewBag.StEmail = sm.StaffEmailID;
                    ViewBag.StStatus = sm.StaffStatus;
                    ViewBag.StJoiDt = sm.StJoiDate;
                    ViewBag.StLeaDt = sm.StLeaDate;

                    List<SelectListItem> list_Status = new List<SelectListItem>();
                    if (sm.StaffStatus == "Active")
                    {
                        list_Status.Add(new SelectListItem { Text = "Active", Value = "Active" });
                        list_Status.Add(new SelectListItem { Text = "Inactive", Value = "Inactive" });
                    }
                    else
                    {
                        list_Status.Add(new SelectListItem { Text = "Inactive", Value = "Inactive" });
                        list_Status.Add(new SelectListItem { Text = "Active", Value = "Active" });
                    }
                    //list_Status.Add(new SelectListItem { Text = "Select", Value = "" });

                    ViewBag.Status = list_Status;
                    TempData["status"] = sm.StaffStatus;
                    return View();
                }
                else
                {
                    return RedirectToAction("GetActiveStaff");
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

        [HttpPost]
        public ActionResult UpdateStaffById(StaffModel sm)
        {
            try
            {
                if (sm.StaffStatus == "Inactive" && sm.StLeaDate == null)
                {


                    //   StaffModel sm = dal.SearchStaff(id);
                    ViewBag.Stid = sm.Staffid;
                    ViewBag.StName = sm.StaffName;
                    ViewBag.Stdob = sm.dob;
                    ViewBag.StaffGender = sm.StaffGender;
                    ViewBag.StAddress = sm.StaffAddress;
                    ViewBag.StMobNo = sm.StaffMobileNo;
                    ViewBag.StEmail = sm.StaffEmailID;
                    ViewBag.StStatus = sm.StaffStatus;
                    ViewBag.StJoiDt = sm.StJoiDate;
                    ViewBag.StLeaDt = sm.StLeaDate;

                    List<SelectListItem> list_Status = new List<SelectListItem>();
                    if (sm.StaffStatus == "Active")
                    {
                        list_Status.Add(new SelectListItem { Text = "Active", Value = "Active" });
                        list_Status.Add(new SelectListItem { Text = "Inactive", Value = "Inactive" });
                    }
                    else
                    {
                        list_Status.Add(new SelectListItem { Text = "Inactive", Value = "Inactive" });
                        list_Status.Add(new SelectListItem { Text = "Active", Value = "Active" });
                    }
                    //list_Status.Add(new SelectListItem { Text = "Select", Value = "" });

                    ViewBag.Status = list_Status;
                    ViewBag.Error = "Leaving date has to be specified if status is inactive";
                    return View();
                }
                string flag = dal.UpdateStaff(sm, Convert.ToInt32(User.Identity.Name));
                if (flag.Equals("Fail"))
                {
                    ViewBag.Redirectionmsg = "Staff not updated, Try Again";

                    if (TempData["status"].ToString() == "Active")
                    {
                        ViewBag.Btnlbl = "Update another Active Staff";
                        ViewBag.BtnAction = "GetActiveStaff";
                    }
                    else
                    {
                        ViewBag.Btnlbl = "Update another Inctive Staff";
                        ViewBag.BtnAction = "GetInactiveStaff";
                    }

                    return View("Fail");
                }
                else
                {
                    ViewBag.Redirectionmsg = "Staff updated Successfully.";

                    if (TempData["status"].ToString() == "Active")
                    {
                        ViewBag.Btnlbl = "Update another Active Staff";
                        ViewBag.BtnAction = "GetActiveStaff";
                    }
                    else
                    {
                        ViewBag.Btnlbl = "Update another Inctive Staff";
                        ViewBag.BtnAction = "GetInactiveStaff";
                    }

                    return View("Success");
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

        [HttpGet]

        public ActionResult GetInactiveStaff()
        {
            try
            {
                List<StaffModel> StM = dal.GetAllInActiveStaff();
                return View(StM);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        // public ActionResult SearchStaff()
        // {
        //     return View();
        // }

        //public JsonResult GetStaff(String prefix)
        // {
        //     DataSet ds = dal.GetStaff(prefix);
        //     List<StaffName> searchlist = new List<StaffName>();
        //     foreach (DataRow dr in ds.Tables[0].Rows)
        //     {
        //         searchlist.Add(new StaffName
        //         {
        //             StaffNames=dr["staffName"].ToString()
        //         });
        //     }
        //     return Json(searchlist, JsonRequestBehavior.AllowGet);
        // }



        [HttpGet]
        public ActionResult AddTest()
        {
            List<TestDepartmentsModel> lst_dept = dal.getAllTestDepartments();
            List<SelectListItem> list_dep = new List<SelectListItem>();
            list_dep.Add(new SelectListItem { Text = "Select", Value = "" });

            foreach (var item in lst_dept)
            {
                list_dep.Add(new SelectListItem { Text = item.testDeptName, Value = item.testDeptID.ToString() });
            }
            ViewBag.dpt = list_dep;
            List<SelectListItem> list_dep1 = new List<SelectListItem>();
            list_dep1.Add(new SelectListItem { Text = "Select", Value = "" });
            //list_dep.Add(new SelectListItem { Text = "sub", Value = "sub" });
            return View();
        }
        public ActionResult GetSubDeptAjax(int deptID)
        {
            try
            {
                //TeacherDAL dal = new TeacherDAL();
                List<TestSub_DepartmentsModel> lst_sub = dal.getAllSubDept(deptID);
                //ViewBag.dept = lst_sub;
                return Json(lst_sub, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        private bool isValidContentTestPic(String ContentType)
        {
            return ContentType.Equals("image/jpeg") || ContentType.Equals("image/jpg") || ContentType.Equals("image/png");
        }

        [HttpPost]
        public ActionResult AddTest(AddTestModel Add)
        {
            //try
            //{ 
            Random random = new Random();
            int c = random.Next(11111, 99999);

            if (Add.file != null)
            {
                if (!isValidContentProfilePic(Add.file.ContentType))
                {
                    ViewBag.Error = "Only jpg and png are allowed";
                    return View();
                }
                if (Add.file.ContentLength > 2000000)
                {
                    ViewBag.Error = "File Size Must Be Less Than 2mb";
                    return View();
                }


                Add.imageAddress = "/TestImages/" + Add.TestName + "" + c + Path.GetExtension(Add.file.FileName);
                Add.file.SaveAs(Server.MapPath("~/TestImages/" + Add.TestName + "" + c + Path.GetExtension(Add.file.FileName)));
            }


            ViewBag.tId = Add.TestId;
            ViewBag.tName = Add.TestName;
            ViewBag.tDescription = Add.TestDesc;
            ViewBag.testSchd = Add.TestSchedule;
            if (Add.file != null)
            {
                ViewBag.img = "/TestImages/" + Add.TestName + "" + c + Path.GetExtension(Add.file.FileName);
            }
            else
            {
                ViewBag.img = "";
            }
            string p = ViewBag.img;
            ViewBag.Status = "Add";


            TempData["abc"] = Add;
            //  image = Add.file;


            return View("IntermediateUpdateTest");

        }


        //////////////////////////////////////////////////vovo///////////////////////////////////////


        [HttpGet]
        public ActionResult UpdateTestByID()
        {
            try
            {
                //StaffDal sDAL = new StaffDal();
                List<AddTestModel> lst_allTests = dal.AllTests();
                ViewBag.delmsg = TempData["TestMsg"];
                ViewBag.delerrmsg = TempData["TestErrMsg"];
                return View(lst_allTests);
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
        public ActionResult UpdateTest(int id)
        {
            //try
            //{ 
            if (id.ToString() == null || id.ToString().Length < 4)
            {
                return RedirectToAction("UpdateTestByID");
            }
            else
            {
                List<TestDepartmentsModel> lst_dept = dal.getAllTestDepartments();
                List<SelectListItem> list_dep = new List<SelectListItem>();
                list_dep.Add(new SelectListItem { Text = "Select", Value = "" });

                foreach (var item in lst_dept)
                {
                    list_dep.Add(new SelectListItem { Text = item.testDeptName, Value = item.testDeptID.ToString() });
                }
                ViewBag.testDep = list_dep;
                List<SelectListItem> list_dep1 = new List<SelectListItem>();
                list_dep1.Add(new SelectListItem { Text = "Select", Value = "" });
                ////



                TempData["id"] = id;
                // StaffDal sDAL = new StaffDal();
                AddTestModel details = dal.GetTestDetailsByID(id);

                ViewBag.tId = details.TestId;
                ViewBag.tName = details.TestName;
                ViewBag.tDescription = details.TestDesc;
                ViewBag.testSchd = details.TestSchedule;
                ViewBag.img = details.imageAddress;
                ViewBag.testDeptID = details.testDeptID;
                ViewBag.testSub_Dep = details.testSub_DeptName;
                // a unique img is present for a test and that is not updated on UpdateTest view so
                //a_model.imageAddress is null but the unchnaged img needed to be shown on IntermsdiateUpdateTest
                // view, thats why this TempData is used
                TempData["notChangedImage"] = details.imageAddress;

                //TempData["img"] = "/TestImages/" + details.TestId + ".jpg";

                return View();
            }
            //}
            //catch (Exception e)
            //{
            //    FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
            //    failedmodel.ExceptionStacktrace=e.ToString();
            //    dal.LogException(failedmodel);
            //    return View("Error");
            //}
        }

        [HttpPost]
        public ActionResult UpdateTest(AddTestModel a_model, HttpPostedFileBase imageAddress)
        {
            //try
            //{ 
            if (a_model.testSub_DeptName1 != null)
            {
                a_model.testSub_DeptName = a_model.testSub_DeptName1;
            }

            if (imageAddress != null)
            {

                if (imageAddress.ContentLength > 2000000)
                {
                    int id = Convert.ToInt32(TempData["id"]);
                    ViewBag.Error = "File Size Must Be Less Than 2mb";
                    ViewBag.tId = a_model.TestId;
                    ViewBag.tName = a_model.TestName;
                    ViewBag.tDescription = a_model.TestDesc;
                    ViewBag.testSchd = a_model.TestSchedule;
                    ViewBag.img = a_model.imageAddress;
                    flagforerror = 5;



                    //AddTestModel details = dal.GetTestDetailsByID(id);

                    //ViewBag.tId = details.TestId;
                    //ViewBag.tName = details.TestName;
                    //ViewBag.tDescription = details.TestDesc;
                    //ViewBag.testSchd = details.TestSchedule;
                    //ViewBag.img = details.imageAddress;
                    //////////TempData["img"] = "/TestImages/" + details.TestId + ".jpg";

                    return View();

                }


                bool flag = isValidContentTestPic(imageAddress.ContentType);
                if (!flag)
                {
                    ViewBag.errmsg = "Only jpg,jpeg,png are allowed";
                    int id = Convert.ToInt32(TempData["id"]);
                    //AddTestModel details = dal.GetTestDetailsByID(id);

                    ViewBag.tId = a_model.TestId;
                    ViewBag.tName = a_model.TestName;
                    ViewBag.tDescription = a_model.TestDesc;
                    ViewBag.testSchd = a_model.TestSchedule;
                    ViewBag.img = "";
                    return View();
                }
                else
                {
                    // StaffDal sDAL = new StaffDal();

                    imageAddress.SaveAs(Server.MapPath("~/TestImages/" + a_model.TestId + Path.GetExtension(imageAddress.FileName)));

                    ViewBag.tId = a_model.TestId;
                    ViewBag.tName = a_model.TestName;
                    ViewBag.tDescription = a_model.TestDesc;
                    ViewBag.testSchd = a_model.TestSchedule;
                    ViewBag.img = "/TestImages/" + a_model.TestId + Path.GetExtension(imageAddress.FileName);
                    string p = ViewBag.img;
                    ViewBag.Status = "Update";

                    TempData["tId"] = a_model.TestId;
                    TempData["tName"] = a_model.TestName;
                    TempData["tDescription"] = a_model.TestDesc;
                    TempData["testSchd"] = a_model.TestSchedule;
                    string s = "/TestImages/" + a_model.TestId + Path.GetExtension(imageAddress.FileName);
                    TempData["img"] = s;
                    TempData["dept"] = a_model.testDeptID;
                    TempData["sdept"] = a_model.testSub_DeptName;

                    return View("IntermediateUpdateTest");

                }
            }


            else
            {
                ViewBag.tId = a_model.TestId;
                ViewBag.tName = a_model.TestName;
                ViewBag.tDescription = a_model.TestDesc;
                ViewBag.testSchd = a_model.TestSchedule;
                if (flagforerror == 5)
                {
                    ViewBag.img = "";
                    TempData["img"] = "";
                }
                else
                {
                    ViewBag.img = TempData["notChangedImage"];
                    TempData["img"] = TempData["notChangedImage"];
                }

                string s = Convert.ToString(TempData["notChangedImage"]);
                ViewBag.Status = "Update";

                TempData["tId"] = a_model.TestId;
                TempData["tName"] = a_model.TestName;
                TempData["tDescription"] = a_model.TestDesc;
                TempData["testSchd"] = a_model.TestSchedule;
                TempData["dept"] = a_model.testDeptID;
                TempData["sdept"] = a_model.testSub_DeptName;

                return View("IntermediateUpdateTest");


            }
            //}
            /// catch (Exception e)
            //{
            //    FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
            //    failedmodel.ExceptionStacktrace=e.ToString();
            //    dal.LogException(failedmodel);
            //    return View("Error");
            //}
        }

        public ActionResult FinalUpdateTestSubmission()
        {
            AddTestModel a_model = new AddTestModel();

            a_model.TestId = Convert.ToInt32(TempData["tId"]);
            a_model.TestName = Convert.ToString(TempData["tName"]);
            a_model.TestDesc = Convert.ToString(TempData["tDescription"]);
            a_model.TestSchedule = Convert.ToString(TempData["testSchd"]);
            a_model.imageAddress = Convert.ToString(TempData["img"]);
            a_model.testDeptID = Convert.ToInt32(TempData["dept"]);
            a_model.testSub_DeptName = TempData["sdept"].ToString();
            //string s = a_model.imageAddress;


            if (dal.UpdateTest(a_model, Convert.ToInt32(User.Identity.Name)))
            {
                ViewBag.Redirectionmsg = "Test Updated Successfully.";
                ViewBag.Btnlbl = "Update Another Test";
                ViewBag.BtnAction = "UpdateTestByID";
                return View("Success");
            }
            else
            {
                string imgToBeDeleted = Convert.ToString(TempData["img"]);
                System.IO.File.Delete(Server.MapPath(imgToBeDeleted));
                ViewBag.Redirectionmsg = "Test Not Updated.";
                ViewBag.Btnlbl = "Try Again";
                ViewBag.BtnAction = "UpdateTestByID";
                return View("Fail");
            }
        }

        public ActionResult FinalAddTestSubmission()
        {
            AddTestModel Add = new AddTestModel();

            Add = (AddTestModel)TempData["abc"];
            //Add.TestId = Convert.ToInt32(TempData["AddtId"]);
            //Add.TestName = Convert.ToString(TempData["AddtName"]);
            //Add.TestDesc = Convert.ToString(TempData["AddtDescription"]);
            //Add.TestSchedule = Convert.ToString(TempData["AddtestSchd"]);
            //Add.imageAddress = Convert.ToString(TempData["Addimg"]);


            String FileName = dal.AddTest(Add, Convert.ToInt32(User.Identity.Name));
            if (FileName == "1")
            {
                //  var path = Path.Combine(Server.MapPath("/TestImages/"), FileName);
                // Add.file.SaveAs(path);
                ViewBag.Redirectionmsg = "Test Added Successfully.";
                ViewBag.Btnlbl = "Add Another Test";
                ViewBag.BtnAction = "AddTest";
                return View("Success");
            }
            else
            {
                String[] arr = FileName.Split(' ');
                if (arr.Length == 1)
                {
                    string imgToBeDeleted = Convert.ToString(TempData["Addimg"]);
                    System.IO.File.Delete(Server.MapPath(imgToBeDeleted));
                    ViewBag.Redirectionmsg = "Test Not Added.";
                    ViewBag.Btnlbl = "Try Again";
                    ViewBag.BtnAction = "AddTest";
                    return View("Fail");
                }
                else
                {
                    //var path1 = Path.Combine(Server.MapPath("~/Counter/"), "xyz.jpg");
                    // string path = Path.Combine(Server.MapPath("/TestImages/")+ arr[2]);
                    //Add.file.SaveAs(Server.MapPath("/TestImages/" + "xyz2" + Path.GetExtension(Add.file.FileName)));
                    // image.SaveAs(Server.MapPath("/TestImages/" + "xyz2" + Path.GetExtension(Add.file.FileName)));
                    //Add.file.SaveAs(path);
                    ViewBag.Redirectionmsg = "Test Added Successfully.";
                    ViewBag.Btnlbl = "Add Another Test";
                    ViewBag.BtnAction = "AddTest";
                    return View("Success");
                }
            }



        }





        ////////////////////////////////souvik 24/10/////////////////////////////////////////////
        [HttpGet]
        public ActionResult ShowAllFeedback()
        {
            try
            {
                List<FeedbackModel> list_fdbk = dal.showAllFeedback();
                ViewBag.delerrmsg = TempData["FeedErrMsg"];
                ViewBag.delmsg = TempData["FeedMsg"];
                return View(list_fdbk);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        public ActionResult InvisibleFeedback(int id)
        {
            try
            {
                if (dal.invisibleFeedback(id))
                {
                    return RedirectToAction("ShowAllFeedback");
                }
                else
                {
                    ViewBag.Redirectionmsg = "Feedback Not Updated.";
                    ViewBag.Btnlbl = "Try Again";
                    ViewBag.BtnAction = "ShowAllFeedback";
                    return View("Fail");
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

        public ActionResult VisibleFeedback(int id)
        {
            try
            {
                if (dal.visibleFeedback(id))
                {
                    return RedirectToAction("ShowAllFeedback");
                }
                else
                {
                    ViewBag.Redirectionmsg = "Feedback Not Updated.";
                    ViewBag.Btnlbl = "Try Again";
                    ViewBag.BtnAction = "ShowAllFeedback";
                    return View("Fail");
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


        /*------------------------------1/11/17--------------------------*/
        public ActionResult DeleteTest(int id)
        {
            try
            {
                if (dal.DeleteTestById(id))
                {
                    TempData["TestMsg"] = "Test Deleted Successfully.";
                    return RedirectToAction("UpdateTestByID");
                }
                else
                {
                    TempData["TestErrMsg"] = "Test not Deleted.";
                    return RedirectToAction("UpdateTestByID");
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

        public ActionResult DeleteFeedback(int id)
        {
            try
            {

                if (dal.DeleteFeedbackById(id))
                {
                    TempData["FeedMsg"] = "Feedback Deleted Successfully.";
                    return RedirectToAction("ShowAllFeedback");
                }
                else
                {
                    TempData["FeedErrMsg"] = "Feedback not Deleted.";
                    return RedirectToAction("ShowAllFeedback");
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


        //souvik

        [HttpGet]
        public ActionResult GetAllStaffDuringAPeriod()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAllStaffDuringAPeriod(StaffPeriodModel model)
        {
            try
            {
                List<StaffModel> lst_staff = dal.getAllStaffDuringAPeriod(model);
                TempData["lst_staff"] = lst_staff;



                return RedirectToAction("ShowStaffByPeriod");
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        public ActionResult ShowStaffByPeriod()
        {
            try
            {
                List<StaffModel> lst_Allstaff = (List<StaffModel>)TempData["lst_staff"];


                if (lst_Allstaff == null)
                {
                    return View("Error");
                }
                if (lst_Allstaff.Count == 0)
                {
                    return View("NoStaff");
                }
                return View(lst_Allstaff);
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
        ///  Atanu Department add code 5/02
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewAllDepts()
        {
            try
            {
                ViewBag.Smsg = TempData["Delmsg"];
                ViewBag.Fmsg = TempData["fmsg"];
                CustomDeptAddModel model = new CustomDeptAddModel();
                model.test_Deptlist = dal.getAllTestDepartments();
                return View(model);
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
        public ActionResult ViewAllDepts(CustomDeptAddModel model)
        {
            try
            {
                ModelState.Clear();
                if (dal.checkDuplicateDepartment(model.test_dept))
                {
                    ViewBag.Fmsg = "Do not insert duplicate Department Name/Reload the page.";
                    CustomDeptAddModel model1 = new CustomDeptAddModel();
                    model1.test_Deptlist = dal.getAllTestDepartments();
                    return View(model1);
                }
                if (dal.AddDepartment(model.test_dept, Convert.ToInt32(User.Identity.Name)))
                {
                    ViewBag.Smsg = "Department added Successfully.";
                    CustomDeptAddModel model1 = new CustomDeptAddModel();
                    model1.test_Deptlist = dal.getAllTestDepartments();
                    return View(model1);
                }
                ViewBag.Fmsg = "Department was not added, Try Again.";
                CustomDeptAddModel model2 = new CustomDeptAddModel();
                model2.test_Deptlist = dal.getAllTestDepartments();
                return View(model2);
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }

        }

        public ActionResult DeleteDepartment(int id)
        {
            try
            {
                if (dal.DelDept(id))
                {
                    ///Success msg
                    TempData["Delmsg"] = "Department deleted Successfully.";
                    return RedirectToAction("ViewAllDepts");
                }
                //fail msg
                TempData["fmsg"] = "Department was not deleted, Try again.";
                return RedirectToAction("ViewAllDepts");

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
        public ActionResult ViewallSubDept(int? id)
        {
            try
            {
                int did;
                if (id == null)
                {
                    if (TempData["id"] == null)
                        did = Convert.ToInt32(TempData["did"]);
                    else
                        did = Convert.ToInt32(TempData["id"]);
                }

                else
                    did = (int)id;
                if (id == 0)
                    return RedirectToAction("ViewAllDepts");

                ViewBag.Smsg = TempData["Delmsg"];
                ViewBag.Fmsg = TempData["fmsg"];
                CustomDeptAddModel model = new CustomDeptAddModel();
                model.test_subdeptList = dal.getAllSubDept(did);
                //if (model.test_subdeptList.Count == 0)
                //{
                //    TempData["fmsg"] = "No Sub-department found.";
                //    return RedirectToAction("ViewAllDepts");
                //}
                TestDepartmentsModel m = new TestDepartmentsModel();
                m.testDeptName = dal.getDeptNameById(did);
                m.testDeptID = did;
                model.test_dept = m;
                TempData["did"] = did;
                return View(model);
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
        public ActionResult ViewallSubDept(CustomDeptAddModel model)
        {
            try
            {
                ModelState.Clear();
                int id = Convert.ToInt32(TempData["did"]);
                if (dal.checkDuplicateSubDepartment(model.test_subdept))
                {
                    ViewBag.Fmsg = "Do not insert Duplicate Department Name/Reload the page.";
                    TempData["did"] = id;
                    TestDepartmentsModel m3 = new TestDepartmentsModel();
                    m3.testDeptName = dal.getDeptNameById(id);
                    m3.testDeptID = id;
                    CustomDeptAddModel model2 = new CustomDeptAddModel();
                    model2.test_subdeptList = dal.getAllSubDept(id);
                    model2.test_dept = m3;
                    return View(model2);
                }

                if (dal.AddSubDept(model.test_subdept.testSub_DeptName, id, Convert.ToInt32(User.Identity.Name)))
                {
                    ViewBag.Smsg = "Sub-department added Successfully.";
                    TempData["did"] = id;
                    TestDepartmentsModel m = new TestDepartmentsModel();
                    m.testDeptName = dal.getDeptNameById(id);
                    m.testDeptID = id;
                    CustomDeptAddModel model1 = new CustomDeptAddModel();
                    model1.test_subdeptList = dal.getAllSubDept(id);
                    model1.test_dept = m;
                    return View(model1);
                }
                ViewBag.Fmsg = "Sub-department was not added, Try Again.";
                TempData["did"] = id;
                TestDepartmentsModel m1 = new TestDepartmentsModel();
                m1.testDeptName = dal.getDeptNameById(id);
                m1.testDeptID = id;
                CustomDeptAddModel model3 = new CustomDeptAddModel();
                model3.test_subdeptList = dal.getAllSubDept(id);
                model3.test_dept = m1;
                return View(model3);
            }

            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

        public ActionResult DeleteSubDepartment(int id)
        {
            if (dal.DelSubDept(id))
            {
                ///Success msg
                TempData["Delmsg"] = "Department deleted Successfully.";
                TempData["id"] = TempData["did"];
                return RedirectToAction("ViewallSubDept");
            }
            //fail msg
            TempData["id"] = TempData["did"];
            TempData["fmsg"] = "Department was not deleted, Try again.";
            return RedirectToAction("ViewallSubDept");
        }

        [HttpGet]
        public ActionResult AddWorkingDayofDoctor(int doctorID)
        {
            try
            {
                //if (doctorID==0)
                //{
                //    doctorID = Convert.ToInt32(TempData["doc_id_wd"]);
                //}


                List<String> lst_WD = new List<string>();
                lst_WD = dal.getWorkingDays(doctorID);
                ViewBag.wd = lst_WD;

                List<WorkingDays> lst = new List<WorkingDays>();
                // lst.Add(new WorkingDays() { WorkingDay = "Everyday", isChecked=false });
                lst.Add(new WorkingDays() { WorkingDay = "Monday", isChecked = false });
                lst.Add(new WorkingDays() { WorkingDay = "Tuesday", isChecked = false });
                lst.Add(new WorkingDays() { WorkingDay = "Wednesday", isChecked = false });
                lst.Add(new WorkingDays() { WorkingDay = "Thursday", isChecked = false });
                lst.Add(new WorkingDays() { WorkingDay = "Friday", isChecked = false });
                lst.Add(new WorkingDays() { WorkingDay = "Saturday", isChecked = false });
                lst.Add(new WorkingDays() { WorkingDay = "Sunday", isChecked = false });

                WorkingDaysList day_lst = new WorkingDaysList();
                day_lst.lst_days = lst;
                TempData["did"] = doctorID;
                return View(day_lst);
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
        public ActionResult AddWorkingDayofDoctor(WorkingDaysList lst)
        {
            try
            {

                int docID = Convert.ToInt32(TempData["did"]);
                StringBuilder sb = new StringBuilder();
                foreach (var item in lst.lst_days)
                {
                    if (item.isChecked)
                    {
                        sb.Append(item.WorkingDay + ",");
                    }

                }
                string str = sb.ToString();
                String str1;
                if (str == "")
                {
                    List<String> lst_WD = new List<string>();
                    lst_WD = dal.getWorkingDays(docID);
                    ViewBag.wd = lst_WD;

                    List<WorkingDays> lst1 = new List<WorkingDays>();
                    // lst.Add(new WorkingDays() { WorkingDay = "Everyday", isChecked=false });
                    lst1.Add(new WorkingDays() { WorkingDay = "Monday", isChecked = false });
                    lst1.Add(new WorkingDays() { WorkingDay = "Tuesday", isChecked = false });
                    lst1.Add(new WorkingDays() { WorkingDay = "Wednesday", isChecked = false });
                    lst1.Add(new WorkingDays() { WorkingDay = "Thursday", isChecked = false });
                    lst1.Add(new WorkingDays() { WorkingDay = "Friday", isChecked = false });
                    lst1.Add(new WorkingDays() { WorkingDay = "Saturday", isChecked = false });
                    lst1.Add(new WorkingDays() { WorkingDay = "Sunday", isChecked = false });

                    WorkingDaysList day_lst = new WorkingDaysList();
                    day_lst.lst_days = lst1;
                    TempData["did"] = docID;
                    ViewBag.bc = "Select any day";
                    return View(day_lst);
                }
                else
                {
                    str1 = str.Remove(str.Length - 1);
                }
                if (dal.addWorkingDayofDoctor(str1, docID, Convert.ToInt32(User.Identity.Name)))
                {
                    ViewBag.Redirectionmsg = "Working Day Added Successfully.";
                    ViewBag.Btnlbl = "Go to doctors list";
                    ViewBag.BtnAction = "ViewAllDoctors";
                    return View("Success");
                }
                else
                {
                    ViewBag.Redirectionmsg = "Working Day Added Successfully.";
                    ViewBag.Btnlbl = "Go to doctors list";
                    ViewBag.BtnAction = "ViewAllDoctors";
                    return View("Fail");
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

        public JsonResult CheckPwd(string Oldpwd)
        {   
            var flag="";
            if( dal.checkOldPassword(User.Identity.Name, Oldpwd))
            {
                 flag= "success";
            }
            else
            {
                flag = "fail";
            }
            
            return Json(flag, JsonRequestBehavior.AllowGet);

        }

    }
}
