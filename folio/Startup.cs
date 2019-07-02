using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using folio.Models;


namespace folio
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
     
            // support for MVC
            services.AddMvc();
            // force routes are all lowercase 
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // shown exception page if development uild
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            // serve static files in wwwroot directory
            app.UseStaticFiles();
            // define default MVC route: controller/action/id
            app.UseMvc((routes) =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
