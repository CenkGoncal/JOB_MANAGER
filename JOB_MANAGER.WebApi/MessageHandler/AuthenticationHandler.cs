using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Dispatcher;
using System.Web.Security;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.Business.Ninject;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers;

namespace JOB_MANAGER.WebApi.MessageHandler
{
    public class AuthenticationHandler:DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {                
                var config =request.GetConfiguration();
                IHttpControllerSelector controllerSelector = config.Services.GetService(typeof(IHttpControllerSelector)) as IHttpControllerSelector;

                var abc =controllerSelector.SelectController(request);
                

                var token = request.Headers.GetValues("Authorization").FirstOrDefault();
                if(token  !=  null)
                {
                    byte[] data = Convert.FromBase64String(token);
                    string stringdecoded = Encoding.UTF8.GetString(data);
                    var tokenValues = stringdecoded.Split(':');

                    IEmployeeService employeeService = InstanceFactory.GetInstance<IEmployeeService>();

                    //if (tokenValues[0] == "Cenk" && tokenValues[1] == "12345")
                    if(employeeService.CheckLogin(tokenValues[0], tokenValues[1],false,true))
                    {

                        var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                        if (authCookie != null && !String.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);

                            SecurityUtilities security = new SecurityUtilities();
                            var identity = security.FormsAuthTicketToIdentity(ticket);

                            IPrincipal principal = new GenericPrincipal(identity, identity.UserInfo.MenuRoles.ToArray());
                            Thread.CurrentPrincipal = principal;
                            HttpContext.Current.User = principal;

                        }
                        else
                        {
                            return Task<HttpResponseMessage>.Factory.StartNew(
                                           () => request.CreateResponse(HttpStatusCode.Unauthorized, "Token not valideted"));
                        }
                    }
                    else
                    {
                        return Task<HttpResponseMessage>.Factory.StartNew(
                                       () => request.CreateResponse(HttpStatusCode.Unauthorized, "Token not valideted"));
                    }
                }
                else
                {
                    return Task<HttpResponseMessage>.Factory.StartNew(
                                   () => request.CreateResponse(HttpStatusCode.Unauthorized,"Token not valideted"));
                }
            }
            catch (Exception)
            {

                return Task<HttpResponseMessage>.Factory.StartNew(
                               () => request.CreateResponse(HttpStatusCode.Unauthorized));
            }
            
            return base.SendAsync(request, cancellationToken);
        }
    }
}
