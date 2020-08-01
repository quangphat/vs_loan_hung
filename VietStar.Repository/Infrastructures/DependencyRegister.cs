using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Repository.Interfaces;

namespace VietStar.Repository.Infrastructures
{
    public static class DependencyRegister
    {
        public static void RegisterRepository(this IServiceCollection services)
        {
            services.AddSingleton<ICommonRepository, CommonRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IGroupRepository, GroupRepository>();
            services.AddSingleton<ILogRepository, LogRepository>();
            services.AddSingleton<IProfileRepository, ProfileRepository>();
        }
    }
}
