﻿using JOB_MANAGER.Core.Helper;
using JOB_MANAGER.CrossCuttingConsers.Security.Web;
using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace JOB_MANAGER
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new AuthorizeAttribute());

            MemoryCacheManager.instance = new MemoryCacheManager();

            //First cache data creating operation
            GlobalCache global = new GlobalCache();
            global.CacheRemove();
            global.CreateCache();            
        }

        public override void Init()
        {
            PostAuthenticateRequest += MvcApplication_PostAuthRequest;
            base.Init();
        }

        private void MvcApplication_PostAuthRequest(object sender, EventArgs e)
        {
            try
            {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];                
                if (authCookie == null)
                    return;

                var encTicket = authCookie.Value;
                if (String.IsNullOrEmpty(encTicket))
                    return;

                var ticket = FormsAuthentication.Decrypt(encTicket);

                SecurityUtilities security = new SecurityUtilities();
                var identity = security.FormsAuthTicketToIdentity(ticket);
                var principal =  new GenericPrincipal(identity, identity.UserInfo.MenuRoles.ToArray());
                
                ThreadGlobals.UserAuthInfo.Value = identity.UserInfo;
                HttpContext.Current.User = principal;
                Thread.CurrentPrincipal = principal;                

            }
            catch (Exception)
            {                
            }
        }
    }
}
