using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace VS_LOAN.Core.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
         name: "Api",
         routeTemplate: "api/{controller}/{action}/{id}",
         defaults: new { id = System.Web.Http.RouteParameter.Optional }
     );
            });
            //RouteConfig.Register();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());


        }
        void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        void Application_Error(object sender, EventArgs e)
        {
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
        }

    }
}