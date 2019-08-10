using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YunHu.Lib.WebHost.Core;

namespace YunHu.WebApiHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHostCreator.Run<Startup>(args);
        }
    }
}
