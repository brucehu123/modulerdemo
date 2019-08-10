﻿using System;
using System.Linq;
using ElectronNET.API;
using Microsoft.AspNetCore.Hosting;
using YunHu.Lib.Logging.Serilog;
using YunHu.Lib.WebHost.Core;

namespace YunHu.Lib.WebHost.Electron
{
    public class ElectronWebHostCreator
    {
        public static void Run<TStartup>(string[] args) where TStartup : StartupAbstract
        {
            if (args != null && args.Any(arg => arg.Contains("/electron", StringComparison.OrdinalIgnoreCase)))
            {
                Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                    .UseStartup<TStartup>()
                    .UseLogging()
                    .UseElectron(args)
                    .Build()
                    .Run();
            }
            else
            {
                WebHostCreator.Run<TStartup>(args);
            }
        }
    }
}
