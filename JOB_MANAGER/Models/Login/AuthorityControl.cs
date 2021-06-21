using JOB_MANAGER.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOB_MANAGER.Models.Login
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
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string Username = HttpContext.Current.User.Identity.Name;
                if(string.IsNullOrEmpty(Username))
                {
                    HttpContext.Current.Response.Redirect("/Login/Login");
                }

                BaseController Base = new BaseController();

                var MenuStr = (from E in Base.db.EMPLOYEES
                               join R in Base.db.ROLES on E.ROLE_ID equals R.ROLE_ID
                               join RM in Base.db.ROLE_MENU on R.ROLE_ID equals RM.ROLE_ID
                               where E.SYSTEM_USERNAME == Username
                               select new
                               {
                                   MENUS_STR = RM.MENUS_STR
                               }).FirstOrDefault();

                if (string.IsNullOrEmpty(MenuStr.MENUS_STR))
                {
                    HttpContext.Current.Response.Redirect("/Base/NotAuthority");
                }
                else
                {
                    List<MenuRolesDto> Menus = JsonConvert.DeserializeObject<List<MenuRolesDto>>(MenuStr.MENUS_STR);

                    foreach(MenuRolesDto Menu in Menus)
                    {
                        if(Menu.name == ActionName)
                        {
                            if(Menu.state)
                                base.OnActionExecuting(filterContext);
                            else
                                HttpContext.Current.Response.Redirect("/Base/NotAuthority");
                        }
                    }
                }
            }
        }
    }
}