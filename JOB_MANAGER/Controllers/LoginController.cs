using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JOB_MANAGER.Controllers
{
    public class LoginController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string username, string password,string remember)
        {
            LoginState loginstate = new LoginState();

            var emp = loginstate.chekLoginState(username, password, remember,true);
            if (emp != null)
            {
                Session["UserInfo"] = emp;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.LoginFaultMessage = "Username or Password is wrong";            
            
            return View(ViewBag);            
        }

        public ActionResult LogOut()
        {
            //clean user info
            Session["UserInfo"] = null;
            Session.Clear();
            Session.Abandon();
            //clean user info
            FormsAuthentication.SignOut();            
            return RedirectToAction("Login", "Login");
        }

        [AllowAnonymous]
        public ActionResult LockedUser()
        {
            string Username = GetUserName();
            if (string.IsNullOrEmpty(Username))
            {
                return RedirectToAction("Login", "Login");
            }
            var a = Session["UserInfo"] as LoginEmpoloyeeDto;


            LockedEmpInfo emp = (from q in db.EMPLOYEES
                         where q.SYSTEM_USERNAME == Username
                         select new LockedEmpInfo
                         {
                             EMP_ID = q.EMP_ID,
                             EMP_NAME = q.FIRST_NAME + " " + q.LAST_NAME,
                             EMP_IMG = q.EMP_IMG
                         }
                         ).FirstOrDefault();

            ViewBag.LockedUserName = emp;

            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        public ActionResult LockedUser(string password)
        {
            LoginState loginstate = new LoginState();

            string Username = GetUserName();
            var emp = loginstate.chekLoginState(Username, password, "false",false);
            if (emp != null)
            {
                Session["UserInfo"] = emp;

                return RedirectToAction("Index", "Home");
            }

            LockedEmpInfo emp1 = (from q in db.EMPLOYEES
                                 where q.SYSTEM_USERNAME == Username
                                 select new LockedEmpInfo
                                 {
                                     EMP_ID = q.EMP_ID,
                                     EMP_NAME = q.FIRST_NAME + " " + q.LAST_NAME,
                                     EMP_IMG = q.EMP_IMG
                                 }
                         ).FirstOrDefault();

            ViewBag.LockedUserName = emp1;
            ViewBag.LoginFaultMessage = "Username or Password is wrong";

            return View(ViewBag);
        }
        

        public JsonResult GetLoginUserInfo()
        {
            string Username = GetUserName();

            var data = (from e in db.EMPLOYEES
                        join c in db.CONTRACT_TYPES on e.CONTRACT_TYPE_ID equals c.CONTRACT_TYPE_ID
                        join d in db.DEPARTMENTS on e.DEPARTMENT_ID equals d.DEPARTMENT_ID
                        join s in db.STATUS on e.EMP_STATUS_ID equals s.STATUS_ID
                        join r in db.ROLES on e.ROLE_ID equals r.ROLE_ID
                        where e.IS_CANCELED == false && e.SYSTEM_USERNAME == Username
                        select new EmployeeDto
                        {
                            EMP_ID = e.EMP_ID,
                            EMP_IMG = e.EMP_IMG,
                            EMP_INITIALS = e.EMP_INITIALS,
                            EMP_NUMBER = e.EMP_NUMBER,
                            FIRST_NAME = e.FIRST_NAME,
                            LAST_NAME = e.LAST_NAME,
                            EMP_NAME = e.FIRST_NAME + " " + e.LAST_NAME,
                            TITLE_ID = e.TITLE_ID,
                            EMP_TITLE = e.TITLES.TITLE_NAME,
                            ROLE_ID = e.ROLE_ID,
                            EMP_ROLE = r.ROLE_NAME,
                            CONTRACT_TYPE_ID = e.CONTRACT_TYPE_ID,
                            EMP_CONTRACT = c.CONTRACT_TYPE_CODE,
                            DEPARTMENT_ID = e.DEPARTMENT_ID,
                            EMP_DEPARTMENT = d.DEPARTMENT_NAME,
                            EMP_EMAIL = e.EMAIL_USERNAME,
                            EMP_STATUS_ID = e.EMP_STATUS_ID,
                            SYSTEM_USERNAME = e.SYSTEM_USERNAME,
                            HIRED_DATE = e.HIRED_DATE,
                            EMP_STATUS = e.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = e.STATUS.DISPLAY_CLASS,
                            EMP_PHONE = e.EMP_PHONE,
                            PHONE_EXTENSION = e.PHONE_EXTENSION,
                            MOBILE_PHONE = e.MOBILE_PHONE,
                            IS_SUPERVISOR = e.IS_SUPERVISOR,
                            IS_DRIVER = e.IS_DRIVER,
                            EMP_COMPANY_ID = e.COMPANY_ID,
                            NAVBAR_CLASS = e.NAVBAR_CLASS

                        }
                         ).OrderBy(o => o.EMP_NAME).FirstOrDefault();

     
            return Json(new { GetUser = data }, JsonRequestBehavior.AllowGet);
        }
    }
}