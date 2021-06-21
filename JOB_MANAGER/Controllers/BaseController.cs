using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Web.Security;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Controllers
{

    public class BaseController : Controller
    {
        public JOB_MANAGER_DBEntities db;
        public GlobalTools.UserInfo UserInfo;
        public BaseController()
        {
            db = new JOB_MANAGER_DBEntities();
            
            GlobalTools globalTools = new GlobalTools();
            UserInfo = new GlobalTools.UserInfo("cenkgoncal");
        }

        public string GetUserName()
        {
            try
            {
                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                return UserName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetUserID()
        {
            try
            {
                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                var empID = db.EMPLOYEES.Where(w => w.SYSTEM_USERNAME == UserName).Select(s => s.EMP_ID).FirstOrDefault();

                return empID == null ? -1 : empID;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int GetCompanyID()
        {
            try
            {
                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                var CompanyID = db.EMPLOYEES.Where(w => w.SYSTEM_USERNAME == UserName).Select(s => s.COMPANY_ID).FirstOrDefault();

                return CompanyID == null ? -1 : CompanyID;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public ActionResult NotAuthority()
        {
            return View();
        }

    }
}