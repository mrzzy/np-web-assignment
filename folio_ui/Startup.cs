using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using folio.Services.API;

namespace folio_ui
{
    public class Startup
    {
        private const string APIHostPolicy = "AllowAPIHostPolicy";
        // configure services
        public void ConfigureServices(IServiceCollection services)
        {
            // enforce lowercase routing
            services.AddRouting(options => options.LowercaseUrls = true);

            // mvc routing service 
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            // enable CORS for talking to api server
            string apiHost = "http://" + Environment.GetEnvironmentVariable(
                    "API_SERVICE");
            string apiIngress = "http://" + Environment.GetEnvironmentVariable(
                    "API_ENDPOINT");
            services.AddCors(options =>
            {  
                options.AddPolicy(Startup.APIHostPolicy, builder => 
                {
                    builder.WithOrigins(apiHost, apiIngress)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // configure middleware
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            // handing exceptions
            if (env.IsDevelopment())
            { app.UseDeveloperExceptionPage(); }
            else { app.UseExceptionHandler("/Home/Error"); }

            // set CORS headers
            app.UseCors(Startup.APIHostPolicy); 
        
            // serve static files in wwwroot
            app.UseStaticFiles();

            // middleware to inject userinfo into http context
            app.Use(async (context, next) =>
            {
                // pull user info form api and pass as http context items
                APIClient api = new APIClient(context);
                context.Items["UserInfo"] = api.GetUserInfo();

                await next.Invoke();
            });
        
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
