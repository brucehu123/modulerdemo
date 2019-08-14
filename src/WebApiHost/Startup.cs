﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YunHu.Lib.WebHost.Core;

namespace YunHu.WebApiHost
{
    public class Startup : StartupAbstract
    {
        public Startup(IHostingEnvironment env) : base(env)
        {
        }
    }
}