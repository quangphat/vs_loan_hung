using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VietBankApi.Infrastructures;
using VietBankApi.Business.Interfaces;
using VietBankApi.Business.Classes;
using Karambolo.Extensions.Logging.File;

namespace VietBankApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var appSettingsSection = Configuration.GetSection("ApiSetting");
            services.Configure<ApiSetting>(appSettingsSection);
            services.AddSingleton(new HttpClient(new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            { Timeout = TimeSpan.FromSeconds(20) });
            services.AddScoped<IAuthorizeBusiness, AuthorizeBusiness>();
            services.AddScoped<CurrentProcess>();
            services.AddScoped<ILogBusiness, LogBusiness>();
            services.AddScoped<IMediaBusiness, MediaBusiness>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            loggerFactory.AddLog4Net();
            app.UseHttpsRedirection();
            app.UseMiddleware<AuthorizeMiddleware>();
            app.UseMvc();
        }
    }
}
