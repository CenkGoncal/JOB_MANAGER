using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using JOB_MANAGER.Business.Ninject;
using JOB_MANAGER.DataAcess.Helper;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers.Utilities;
using JOB_MANAGER.DATAACESS.Helper;

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

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(new BusinessModule()));
            //Depenciy inversion ile yazılan geçilen parametrelerde anlam kazanıp alt sınıfların üst sınıf cinsine tek
            //bir yerden ayarlamak 

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
