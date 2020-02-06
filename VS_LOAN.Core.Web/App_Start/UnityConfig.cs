using EasyCreditService.Classes;
using EasyCreditService.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.EasyCredit;
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
            container.RegisterSingleton<HttpClient>();
            container.RegisterType<IECLoanBusiness, ECLoanBusiness>();
            container.RegisterType<IHosoBusiness, HosoBusiness>();
            container.RegisterType<ILoanRequestService, LoanRequestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}