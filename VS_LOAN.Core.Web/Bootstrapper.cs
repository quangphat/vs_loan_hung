using System;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc4;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Interfaces;

namespace VS_LOAN.Core.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            Func<LifetimeManager> LM = () => { return new HierarchicalLifetimeManager(); };
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();   
            container.RegisterType<IHosoBusiness, HosoBusiness>();
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}