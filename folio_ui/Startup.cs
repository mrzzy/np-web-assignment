using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace folio_ui
{
    public class Startup
    {
        // configure services
        public void ConfigureServices(IServiceCollection services)
        {
            // enforce lowercase routing
            services.AddRouting(options => options.LowercaseUrls = true);

            // mvc routing service 
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // configure middleware
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // handing exceptions
            if (env.IsDevelopment())
            { app.UseDeveloperExceptionPage(); }
            else { app.UseExceptionHandler("/Home/Error"); }
        
            // serve static files in wwwroot
            app.UseStaticFiles();
        
            // MVC routing
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
