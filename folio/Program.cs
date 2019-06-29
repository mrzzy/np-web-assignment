using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DotNetEnv;

namespace folio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // load environment variables from .env
            DotNetEnv.Env.Load();
        
            // configure host to listen on all interfaces
            // required to be able to reach the service from
            // outside container
            var host = WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:5000/")
                .UseStartup<Startup>()
                .Build();
            
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
