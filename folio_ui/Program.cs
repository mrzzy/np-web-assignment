using System;
using System.IO;
using DotNetEnv;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace folio_ui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // load environment variables from .env
            
        
            // configure host to listen on all interfaces
            // required to be able to reach the service from
            // outside container
            var host = WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:5001/")
                .UseStartup<Startup>()
                .Build();
            
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
