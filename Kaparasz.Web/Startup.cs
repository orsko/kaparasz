using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IO;

namespace Kaparasz.Web
{
    public class Startup
    {
        // private AuthorizationSettings AuthorizationSettings { get; set; }
        private readonly string localStoragePath;
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                // .AddJsonFile("notificationMessages.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            localStoragePath = Path.Combine(env.ContentRootPath, "AppData");

            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .Enrich.FromLogContext()
              .WriteTo.ColoredConsole()
              .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Audience = "kaparasz-6742b";
                options.Authority = "https://securetoken.google.com/kaparasz-6742b";
            });

            // Add memory cache
            services.AddMemoryCache();
            
            services.AddCors();

            services.AddMvc().AddNewtonsoftJson();
            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddScoped<Firebase.Auth.IFirebaseAuthProvider, Firebase.Auth.FirebaseAuthProvider>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseRouting();
            app.UseCors("default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
