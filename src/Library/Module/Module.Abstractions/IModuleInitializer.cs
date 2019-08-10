﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YunHu.Lib.Module.Abstractions
{
    /// <summary>
    /// 模块初始化器接口
    /// </summary>
    public interface IModuleInitializer
    {
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        void Configure(IApplicationBuilder app, IHostingEnvironment env);

        /// <summary>
        /// 配置MVC
        /// </summary>
        /// <param name="mvcOptions"></param>
        void ConfigureMvc(MvcOptions mvcOptions);

        /// <summary>
        /// 配置选项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        void ConfigOptions(IServiceCollection services, IConfiguration configuration);
    }
}
