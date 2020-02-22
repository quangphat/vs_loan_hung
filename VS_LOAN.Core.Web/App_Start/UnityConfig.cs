using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.EasyCredit;
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

            //// register all your components with the container here
            //// it is NOT necessary to register your controllers
            //// e.g. container.RegisterType<ITestService, TestService>();
            //container.RegisterType<QuanLyHoSoController> (new InjectionConstructor());
            //container.RegisterType<IECLoanBusiness, ECLoanBusiness>();
            //DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            //GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            container.RegisterSingleton<HttpClient>();
            container.RegisterSingleton<CurrentProcess>();
            container.RegisterType<IECLoanBusiness, ECLoanBusiness>();
            container.RegisterType<IHosoBusiness, HosoBusiness>();
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}