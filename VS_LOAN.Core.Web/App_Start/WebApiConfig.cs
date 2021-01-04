using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VS_LOAN.Core.Web.Controllers;

namespace VS_LOAN.Core.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

          
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
              name: "RpcApi",
              routeTemplate: "api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional }
         );



            config.Routes.MapHttpRoute(
            name: "uploadStatus1ds",
            routeTemplate: "api/{action}",
            defaults: new { controller = "MiraeApi", Action = RouteParameter.Optional }
            );



            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Filters.Add(new BasicAuthenticationAttribute());
        }
    }
}
