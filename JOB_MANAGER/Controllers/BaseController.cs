﻿using JOB_MANAGER.Models;
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
        public GlobalCache _globalCache;        

        public BaseController()
        {
            db = new JOB_MANAGER_DBEntities();

            if(_globalCache == null)
                _globalCache = new GlobalCache();            
        }

        public UserInfo GetUserInfo()
        {
            var tool = _globalCache.GetUserInfo();
            if (tool == null)
            {
                LoginPage();
                throw new Exception("Sesion not start or expired");
            }

            return tool;
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
                var userinfo =  GetUserInfo();
                if (userinfo == null)
                    return -1;

                return userinfo.UserId;
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
                var userinfo = GetUserInfo();
                if (userinfo == null)
                    return -1;
          
                return userinfo.CompanyId;
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

        public ActionResult LoginPage()
        {  
            //clean user info
            //Session["UserInfo"] = null;
            Session.Clear();
            Session.Abandon();
            //clean user info
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");            
        }

    }
}