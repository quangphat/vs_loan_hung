using MCreditService;
using System;
using Unity;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();



            // TODO: Register your type's mappings here.
            //business
            container.RegisterType<ICheckDupBusiness, CheckDupBusiness>();
            container.RegisterType<IMediaBusiness,MediaBusiness>();
            container.RegisterType<IRevokeDebtBusiness, RevokeDebtBusiness>();
            //repository
            container.RegisterSingleton<ICheckDupRepository, CheckDupRepository>();
            container.RegisterSingleton<IGroupRepository, GroupRepository>();
            container.RegisterSingleton<ICommonRepository, CommonRepository>();
            container.RegisterSingleton<ISystemconfigRepository, SystemconfigRepository>();
            container.RegisterSingleton<IRevokeDebtRepository, RevokeDebtRepository>();
            container.RegisterSingleton<ILogRepository, LogRepository>();
            container.RegisterSingleton<IHosoRepository, HosoRepository>();
            container.RegisterSingleton<IOcbRepository, OcbRepository>();
            container.RegisterSingleton<ICustomerRepository, CustomerRepository>();
            container.RegisterSingleton<IHosoCourrierRepository, HosoCourrierRepository>();
            container.RegisterSingleton<IMCreditRepositoryTest, MCreditRepositoryTest>();
            container.RegisterSingleton<IEmployeeRepository, EmployeeRepository>();
            container.RegisterSingleton<INoteRepository, NoteRepository>();
            container.RegisterSingleton<IMCeditRepository, MCreditRepository>();
            container.RegisterSingleton<IPartnerRepository, PartnerRepository>();
            container.RegisterSingleton<ITailieuRepository, TailieuRepository>();
            container.RegisterType<MCreditService.Interfaces.IMCreditService, MCreditLoanService>();

            container.RegisterType<MCreditService.Interfaces.IOdcService, OdbServiceService>();
            container.RegisterType<IOcbBusiness, OcbBusiness>();
            container.RegisterType<IMiraeRepository, MiraeRepository>();
            container.RegisterType<MCreditService.Interfaces.IMiraeService, MiraeService>();
            
            container.RegisterType<IMiraeDeferRepository, MiraeDeferRepository>();
            container.RegisterType<IMiraeMaratialRepository, MiraeMaratialRepository>();

            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}