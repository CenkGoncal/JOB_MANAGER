using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JOB_MANAGER.WebApi.MessageHandler;

namespace JOB_MANAGER.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API yapılandırması ve hizmetleri
            config.MessageHandlers.Add(new AuthenticationHandler());

            // Web API yolları
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
