﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using TestWebApplication.Conventions;
using WeihanLi.AspNetCore.Authentication;

namespace TestWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(HeaderAuthenticationDefaults.HeaderAuthenticationSchema)
                .AddCustomHeader(options => { options.AdditionalHeaderToClaims.Add("UserEmail", ClaimTypes.Email); });

            services.AddMvc(options =>
                {
                    options.Conventions.Add(new ApiControllerVersionConvention());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}