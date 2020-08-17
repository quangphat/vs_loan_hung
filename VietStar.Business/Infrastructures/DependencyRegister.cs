﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Business.Interfaces;

namespace VietStar.Business.Infrastructures
{
    public static class DependencyRegister
    {
        public static void RegisterBusiness(this IServiceCollection services)
        {
            services.AddScoped<IMediaBusiness, MediaBusiness>();
            services.AddScoped<ICompanyBusiness, CompanyBusiness>();
            services.AddScoped<ICheckDupBusiness, CheckDupBusiness>();
            services.AddScoped<INoteBusiness, NoteBusiness>();
            services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();
            services.AddScoped<ICommonBusiness, CommonBusiness>();
            services.AddScoped<IGroupBusiness, GroupBusiness>();
            services.AddScoped<IProfileBusiness, ProfileBusiness>();
        }
    }
}
