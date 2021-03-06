﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TM.Helper.Extension;

namespace TMShopsCore.Manager
{
    public class Startup
    {
        private static string _applicationPath = string.Empty;
        string sqlConnectionString = string.Empty;
        bool useInMemoryProvider = false;
        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add service for accessing current HttpContext AND ActionContext
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddSingleton<Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor, Microsoft.AspNetCore.Mvc.Infrastructure.ActionContextAccessor>();
            //Add service for accessing current HttpContext
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            // Add framework services.
            services.AddMvc();
            //Filters Auth
            services.AddScoped<MiddlewareFilters.Auth>();
            //Service Context
            var cs = Configuration.GetConnectionString("TMShopsContext");
            services.AddDbContext<TMShopsCore.Models.TMShopsContext>(options => options.UseSqlServer(cs, b => b.UseRowNumberForPaging()));
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Add service for accessing current HttpContext AND ActionContext
            TM.Helper.TMAppContext.Services = app.ApplicationServices;
            //CultureInfo Defalut
            TM.Format.Formating.CultureInfo();
            //var cultureInfo = new System.Globalization.CultureInfo("en-US");
            //cultureInfo.NumberFormat.CurrencySymbol = "€";
            //System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            //System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            //
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //
            app.UseStaticFiles();
            app.UseSession();
            //app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(30));

            // Add MVC to the request pipeline.
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            app.UseExceptionHandler(
              builder =>
              {
                  builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
              });

            app.UseMvc(routes =>
            {
                //Backend
                //routes.MapRoute(
                //    name: "areaRoute",
                //    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //Frontend
                //routes.MapRoute(
                //    name: "details",
                //    defaults: new { Controllers = "Details", Action = "Index" },
                //    template: "{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
