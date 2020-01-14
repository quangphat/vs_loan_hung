using System.Web.Http;
using Unity;
using Unity.WebApi;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Interfaces;

namespace VS_LOAN.Core.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IHosoBusiness, HosoBusiness>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}