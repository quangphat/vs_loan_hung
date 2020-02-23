using LoanRepository.Classes;
using LoanRepository.Interfaces;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Classes;
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

            
            container.RegisterSingleton<HttpClient>();
            container.RegisterSingleton<CurrentProcess>();
            container.RegisterRepository();
            container.RegisterBusiness();
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
        private static void RegisterRepository(this UnityContainer container)
        {
            container.RegisterType<IEcProductRepository, EcProductRepository>();
            container.RegisterType<ITailieuRepository, TailieuRepository>();
            container.RegisterType<IEcEmploymentRepository, EcEmploymentRepository>();
            container.RegisterType<IEcLocationRepository, EcLocationRepository>();
        }
        private static void RegisterBusiness(this UnityContainer container)
        {
            container.RegisterType<IEcProductBusiness, EcProductBusiness>();
            container.RegisterType<IEcLocationBusiness, EcLocationBusiness>();
            container.RegisterType<IEcEmploymentBusiness, EcEmploymentBusiness>();
            container.RegisterType<IECLoanBusiness, ECLoanBusiness>();
            container.RegisterType<IHosoBusiness, HosoBusiness>();
            container.RegisterType<ITailieuBusniness, TailieuBusiness>();
        }
    }
}