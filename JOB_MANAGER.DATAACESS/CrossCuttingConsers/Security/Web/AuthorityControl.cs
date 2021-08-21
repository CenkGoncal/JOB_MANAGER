using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers
{
    public class AuthorityControl:ActionFilterAttribute
    {
        public AuthorityControl(string ActionName)
        {
            this.ActionName = ActionName;
        }

        private string ActionName;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if(System.Threading.Thread.CurrentPrincipal == null)
            {
                HttpContext.Current.Response.Redirect("/Login/Login");
            }

            if (System.Threading.Thread.CurrentPrincipal.IsInRole(ActionName))
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                HttpContext.Current.Response.Redirect("/Base/NotAuthority");
            }            
        }
    }
}
