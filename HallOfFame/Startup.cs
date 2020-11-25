using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HallOfFame.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HallOfFame
{
    public class Startup
    {
        private IConfiguration _config { get; }
        
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDatabaseContext>
                (x => x.UseSqlServer(_config.GetConnectionString("MainConnection")));

            var swaggerOptions = new SwaggerOptions();
            _config.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerOptions.Version,
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = swaggerOptions.Title,
                        Description = swaggerOptions.Description,
                        Version = swaggerOptions.Version
                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {           
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async c =>
                    {
                        var statusCode = c.Response.StatusCode;

                        c.Response.Headers["Content-Type"] = "text/plain; charset=utf-8";

                        await c.Response.WriteAsync(ErrorController.HttpStatusCodeHandler(statusCode));
                    });
                });
            }



            app.UseHttpsRedirection();

            app.UseMvc();

            var swaggerOptions = new SwaggerOptions();
            _config.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.Run(async c =>
            {
                c.Response.Headers["Content-Type"] = "text/plain; charset=utf-8";
                await c.Response.WriteAsync(ErrorController.HttpStatusCodeHandler(200));
            });
        }
    }
}
