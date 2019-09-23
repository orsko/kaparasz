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
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Kaparasz.Web.Middlewares;

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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.EventsType = typeof(CookieValidationMiddleware);
            });

            // Add memory cache
            services.AddMemoryCache();
            
            services.AddCors();

            services.AddMvc().AddNewtonsoftJson();
            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddScoped<IFirebaseAuthProvider>(s =>
            {
                return new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAcyaI48Vo7P3r8thKttsdUYl_bsfpG-g4"));
            });

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("Properties/kaparasz-6742b-firebase-adminsdk-dpgvn-55702ca3fa.json"),
            });

            services.AddScoped<CookieValidationMiddleware>();

            //services.AddScoped(s =>
            //{
            //    FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance
            //    return FirebaseApp.Create(new AppOptions()
            //    {
            //        Credential = GoogleCredential.FromFile("Properties/kaparasz-6742b-firebase-adminsdk-dpgvn-55702ca3fa.json"),
            //    });
            //});


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
