﻿using Microsoft.AspNetCore.Hosting;
using YunHu.Lib.Utils.Core.Helpers;
using Serilog;

namespace YunHu.Lib.Logging.Serilog
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseLogging(this IWebHostBuilder builder)
        {
            builder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var cfgHelper = new ConfigurationHelper();
                var cfg = cfgHelper.Load("logging", hostingContext.HostingEnvironment.EnvironmentName);

                loggerConfiguration
                    .ReadFrom.Configuration(cfg)
                    .Enrich.FromLogContext();
            });

            return builder;
        }
    }
}
