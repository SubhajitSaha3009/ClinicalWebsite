using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Clinic.Models;
using System.Web.Security;
using System.Web.UI;


namespace Mvc_Clinic.Controllers
{
    public class HeadOfClinicController : Controller
    {
        HomeDAL hDAL = new HomeDAL();
        AdminDAL dal = new AdminDAL();
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        
        public ActionResult Login(LoginModel model)
        {
            try
            {
               
                    if (model.LogInId.ToString().Length == 5)
                    {
                        CheckPasswordStatusEnum status = (CheckPasswordStatusEnum)dal.login(model);
                        if (status == CheckPasswordStatusEnum.WrongPassword)
                        {
                            ViewBag.errmsg = "Invalid login ID or password!";
                            return View();
                        }
                        else if (status == CheckPasswordStatusEnum.NewUser)
                        {
                            FormsAuthentication.SetAuthCookie(model.LogInId.ToString(), false);
                            ////Change Atanu 16/09
                            LoginLogModel mod = new LoginLogModel(0,Convert.ToInt32(model.LogInId));
                            if(dal.LogLoginDetails(mod))
                            {
                                return RedirectToAction("UpdatePasswordAdmin", "Admin");
                            }
                            else
                            {
                                return View("Fail");
                            }
                            
                        }
                        else if (status == CheckPasswordStatusEnum.Updated)
                        {
                            FormsAuthentication.SetAuthCookie(model.LogInId.ToString(), false);

                            ////Change Atanu 16/09
                            LoginLogModel mod = new LoginLogModel(0,Convert.ToInt32(model.LogInId));
                            if (dal.LogLoginDetails(mod))
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                return View("Fail");
                            }

                            
                        }
                    
                }
                
                ViewBag.errmsg = "Enter Valid Login ID";
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

        /// <summary>
        /// //////////////////////////Forget password
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult ForgetPassword()
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
        public ActionResult ForgetPassword(ForgetPasswordModel model)
        {
            try
            {
                string ps = dal.forgetPasswordBySecurityAnswer(model);
                if (ps == null)
                {

                    List<SelectListItem> lst_securityQuestion = new List<SelectListItem>();
                    lst_securityQuestion.Add(new SelectListItem { Text = "Select", Value = "" });
                    lst_securityQuestion.Add(new SelectListItem { Text = "What is your's first school name?", Value = "What is your's first school name?" });
                    lst_securityQuestion.Add(new SelectListItem { Text = "What is your pet’s name?", Value = "What is your pet’s name?" });
                    lst_securityQuestion.Add(new SelectListItem { Text = "In what city or town does your nearest sibling live?", Value = "In what city or town does your nearest sibling live?" });
                    lst_securityQuestion.Add(new SelectListItem { Text = "What is your favorite food?", Value = "What is your favorite food?" });

                    ViewBag.securityQuestion1 = lst_securityQuestion;



                    ViewBag.err = "Wrong Security answer";
                    return View();
                }
                else
                {
                    ViewBag.passwrd = ps;
                    return View("PasswordUpdated");
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
        public ActionResult ForgetPasswordToEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPasswordToEmail(ForgetPasswordToEmailModel model)
        {
            try
            {
                if (dal.forgetPasswordToEmail(model))
                {
                    return View("LoginHere");
                }
                else
                {
                    //roolback
                    ViewBag.msg = "Invalid Admin ID";
                    return View();
                }

                //string[] arr = str.Split(' ');


                //string emailID = arr[0];
                //string password = arr[1];

                // dal.mailConnection(model,emailID);
                // return View();
            }
            catch (Exception e)
            {
                FailedEventLogModel failedmodel = new FailedEventLogModel(2, Convert.ToInt32(User.Identity.Name));
                failedmodel.ExceptionStacktrace = e.ToString();
                dal.LogException(failedmodel);
                return View("Error");
            }
        }

    }
}
