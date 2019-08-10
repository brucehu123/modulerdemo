using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using YunHu.Lib.Module.Core;
using YunHu.Lib.Swagger;
using YunHu.Lib.WebHost.Core.Middlewares;
using YunHu.Lib.WebHost.Core.Options;

namespace YunHu.Lib.WebHost.Core
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用WebHost
        /// </summary>
        /// <param name="app"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebHost(this IApplicationBuilder app, HostOptions hostOptions, IHostingEnvironment env)
        {
            //异常处理
            app.UseExceptionHandle();

            //加载模块
            app.UseModules(env);

            //设置默认文档
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);

            //启用静态资源访问
            app.UseStaticFiles();

            //反向代理
            if (hostOptions.Proxy)
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            //身份认证
            app.UseAuthentication();

            //CORS
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
                
                //下载文件时，文件名称会保存在headers的Content-Disposition属性里面
                builder.WithExposedHeaders("Content-Disposition");
            });

            //开启Swagger
            if (hostOptions.Swagger || env.IsDevelopment())
            {
                app.UseCustomSwagger();
            }

            app.UseMvc();

            return app;
        }
    }
}
