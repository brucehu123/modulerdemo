﻿using Microsoft.AspNetCore.Hosting;
using YunHu.Lib.Logging.Serilog;
using YunHu.Lib.Utils.Core.Extensions;
using YunHu.Lib.Utils.Core.Helpers;
using YunHu.Lib.WebHost.Core.Options;

namespace YunHu.Lib.WebHost.Core
{
    /// <summary>
    /// WebHost构造器
    /// </summary>
    public class WebHostCreator
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args">启动参数</param>
        public static void Run<TStartup>(string[] args) where TStartup : StartupAbstract
        {
            CreateBuilder<TStartup>(args).Build().Run();
        }

        /// <summary>
        /// 创建主机生成器
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateBuilder<TStartup>(string[] args) where TStartup : StartupAbstract
        {
            var cfgHelper = new ConfigurationHelper();

            //加载主机配置项
            var hostOptions = cfgHelper.Get<HostOptions>("Host");

            if (hostOptions.Urls.IsNull())
                hostOptions.Urls = "http://*:5000";

            return Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                .UseStartup<TStartup>()
                .UseLogging()
                .UseUrls(hostOptions.Urls);
        }
    }
}
