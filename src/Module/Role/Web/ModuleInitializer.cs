using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YunHu.Lib.Module.Abstractions;
using YunHu.Module.Role.Infrastructure.Options;

namespace YunHu.Module.Role.Web
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RoleOptions>(configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }

        public void ConfigureMvc(MvcOptions mvcOptions)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
           
        }
    }
}
