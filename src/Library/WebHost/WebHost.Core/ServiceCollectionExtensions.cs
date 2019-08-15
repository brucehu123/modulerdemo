using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using YunHu.Lib.Auth.Jwt;
using YunHu.Lib.Cache.Integration;
using YunHu.Lib.Data.AspNetCore;
using YunHu.Lib.Mapper.AutoMapper;
using YunHu.Lib.Module.Core;
using YunHu.Lib.Swagger;
using YunHu.Lib.Swagger.Conventions;
using YunHu.Lib.Utils.Core;
using YunHu.Lib.Utils.Mvc;
using YunHu.Lib.Validation.FluentValidation;
using YunHu.Lib.WebHost.Core.Options;

namespace YunHu.Lib.WebHost.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env">环境</param>
        /// <returns></returns>
        public static IServiceCollection AddWebHost(this IServiceCollection services, HostOptions hostOptions, IHostingEnvironment env)
        {
            services.AddSingleton(hostOptions);

            services.AddUtils();

            services.AddUtilsMvc();

            //加载模块
            var modules = services.AddModules(env.EnvironmentName);

            System.Collections.Generic.List<System.Reflection.Assembly> assemblies = new System.Collections.Generic.List<System.Reflection.Assembly>();
            Module.Abstractions.ModuleInfo partModule = null;

            #region 添加未引用的模块

            var path = @"E:\Code\Net\Moduler\src\WebApiHost\bin\Debug\netcoreapp2.2\plugins";

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);

            foreach (var file in dir.GetFiles())
            {
                var name = file.FullName;
                if (name.EndsWith(".json"))
                {
                    partModule = Newtonsoft.Json.JsonConvert.DeserializeObject<Module.Abstractions.ModuleInfo>(File.ReadAllText(name));
                }
                else
                {
                    var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(name);
                    assemblies.Add(assembly);
                }

            }

            if (partModule != null)
            {
                //判断是否已存在
                if (!modules.Any(m => m.Name.Equals(partModule.Name)))
                {
                    partModule.AssembliesInfo = new Module.Abstractions.ModuleAssembliesInfo
                    {
                        Domain = assemblies.Where(s => s.FullName.Contains("Module." + partModule.Id + ".Domain")).FirstOrDefault(),
                        Infrastructure = assemblies.Where(s => s.FullName.Contains("Module." + partModule.Id + ".Infrastructure")).FirstOrDefault(),
                        Application = assemblies.Where(s => s.FullName.Contains("Module." + partModule.Id + ".Application")).FirstOrDefault(),
                        Web = assemblies.Where(s => s.FullName.Contains("Module." + partModule.Id + ".Web")).FirstOrDefault()
                    };

                    var moduleInitializerType = partModule.AssembliesInfo.Web.GetTypes().FirstOrDefault(t => typeof(Module.Abstractions.IModuleInitializer).IsAssignableFrom(t));
                    if (moduleInitializerType != null && (moduleInitializerType != typeof(Module.Abstractions.IModuleInitializer)))
                    {
                        partModule.Initializer = (Module.Abstractions.IModuleInitializer)System.Activator.CreateInstance(moduleInitializerType);
                    }
                    modules.Add(partModule);

                    services.AddApplicationServices(partModule);

                    if (partModule.Initializer != null)
                    {
                        partModule.Initializer.ConfigureServices(services);
                        var cfgHelper = new Utils.Core.Helpers.ConfigurationHelper();
                        var cfg = cfgHelper.Load("module", env.EnvironmentName, true);
                        partModule.Initializer.ConfigOptions(services, cfg.GetSection(partModule.Id));

                        services.AddSingleton(partModule);
                    }
                }
            }
            #endregion


            //添加对象映射
            services.AddMappers(modules);

            //添加缓存
            services.AddCache(env.EnvironmentName);

            //主动或者开发模式下开启Swagger
            if (hostOptions.Swagger || env.IsDevelopment())
            {
                services.AddSwagger(modules);
            }

            //Jwt身份认证
            services.AddJwtAuth(env.EnvironmentName);

            //添加MVC功能
            var mvcBuilder = services.AddMvc(c =>
              {
                  if (env.IsDevelopment())
                  {
                      //API分组约定
                      c.Conventions.Add(new ApiExplorerGroupConvention());
                  }

                  //模块中的MVC配置
                  foreach (var module in modules)
                  {
                      module.Initializer?.ConfigureMvc(c);
                  }

              })
              .AddJsonOptions(options =>
              {
                  //设置日期格式化格式
                  options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
              })
              .AddValidators(services)//添加验证器
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // 添加未引用的模块
            if (assemblies.Count > 0)
            {
                var assembly = assemblies.Where(s => s.FullName.Contains("Module." + partModule.Id + ".Web")).FirstOrDefault();
                mvcBuilder.AddApplicationPart(assembly);
            }


            //添加数据库
            services.AddDb(env.EnvironmentName, modules);

            //解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            return services;
        }
    }
}
