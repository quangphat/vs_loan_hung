using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Web.Controllers;

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
            //DependencyResolver.SetResolver(new Unity..UnityDependencyResolver(container));
            RegisterTypes(container);
           GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
        private static void RegisterTypes(IUnityContainer container)
        {
            //container.RegisterType<IController, QuanLyHoSoController>("QuanLyHoSo");
            container.RegisterType<IController,BaseController>();
            container.RegisterType<IHosoBusiness, HosoBusiness>();
            container.RegisterType<ICurrentProcess, CurrentProcess>();
        }
    }
}