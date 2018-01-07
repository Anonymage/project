using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using xApplication.Security;

namespace xApplication
{
    public class Startup
    {
        public static IHostingEnvironment HostingEnvironment;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options => {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = "volatile",
                           ValidAudience = "volatile.s.children",
                           IssuerSigningKey = JWTSecurityKey.Create("mycoolsecretkey")
                       };

                       options.Events = new JwtBearerEvents
                       {
                           OnAuthenticationFailed = context =>
                           {
                               Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                               return Task.CompletedTask;
                           },
                           OnTokenValidated = context =>
                           {
                               Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                               return Task.CompletedTask;
                           }
                       };
                   });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Role",
                    policy => policy.RequireRole("User"));
            });


            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            HostingEnvironment = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }

    }
}
