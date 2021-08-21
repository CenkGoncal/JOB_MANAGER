using System;
using System.Web;
using System.Web.Security;
using JOB_MANAGER.DATAACESS.Models;
using Newtonsoft.Json;

namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers
{
    public static class AuthenticationHelper
    {
        public static void  CreateAuthCookie(int userId, string username, UserInfoDto userInfoDto, DateTime expreation, bool rememberMe)
        {
            var auhtTicket = new FormsAuthenticationTicket(1, username, DateTime.Now, expreation, rememberMe, JsonConvert.SerializeObject(userInfoDto));
            string encTicket = FormsAuthentication.Encrypt(auhtTicket);

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            authCookie.Expires = DateTime.Now.AddDays(10);

            //HttpContext.Current.Response.SetCookie(authCookie);
            HttpContext.Current.Response.Cookies.Add(authCookie);

            //FormsAuthentication.SetAuthCookie(FormsAuthentication.FormsCookieName, false)


        }
    }
}
