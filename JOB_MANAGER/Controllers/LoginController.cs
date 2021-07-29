using JOB_MANAGER.Bussiness.Concrete;
using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            
            if (employee.CheckLogin(username, password, remember == "true", true))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.LoginFaultMessage = "Username or Password is wrong";            
            
            return View(ViewBag);            
        }

        public ActionResult LogOut()
        {
            return base.LoginPage();
        }

        [AllowAnonymous]
        public ActionResult LockedUser()
        {            
            if (string.IsNullOrEmpty(ThreadGlobals.UserAuthInfo.Value.UserName))
            {
                return RedirectToAction("Login", "Login");
            }

            EmployeeManager employee = new EmployeeManager(new EmployeeDal());


            var emp = employee.Get(ThreadGlobals.UserAuthInfo.Value.UserName);
            ViewBag.LockedUserName = new LockedEmpInfo() {  EMP_ID = emp.EMP_ID,EMP_IMG=emp.EMP_IMG,EMP_NAME = emp.EMP_NAME};

            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        public ActionResult LockedUser(string password)
        {

            if (string.IsNullOrEmpty(ThreadGlobals.UserAuthInfo.Value.UserName))
            {
                return RedirectToAction("Login", "Login");
            }

            EmployeeManager employee = new EmployeeManager(new EmployeeDal());

            if (employee.CheckLogin(ThreadGlobals.UserAuthInfo.Value.UserName, password, false, true))
            {
                return RedirectToAction("Index", "Home");
            }

            var emp = employee.Get(ThreadGlobals.UserAuthInfo.Value.UserName);
            ViewBag.LockedUserName = new LockedEmpInfo() { EMP_ID = emp.EMP_ID, EMP_IMG = emp.EMP_IMG, EMP_NAME = emp.EMP_NAME };
            ViewBag.LoginFaultMessage = "Username or Password is wrong";

            return View(ViewBag);
        }
        

        public JsonResult GetLoginUserInfo()
        {

            EmployeeManager employee = new EmployeeManager(new EmployeeDal());            
            var data = employee.Get(ThreadGlobals.UserAuthInfo.Value.UserName);

            return Json(new { GetUser = data }, JsonRequestBehavior.AllowGet);
        }
    }
}
