using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlinePharmacy.Data;
using OnlinePharmacy.Data.UnitOfWork;
using OnlinePharmacy.Domain.Auth;
using OnlinePharmacy.Services.Contract;
using OnlinePharmacy.Services.IdentityClasses;
using OnlinePharmacy.Services.Implementation;
using OnlinePharmacy.Web.Utility;
using System;

namespace OnlinePharmacy.Web
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
            SiteSetting siteSetting = new(Configuration);
            services.AddScoped<OnlinePharmacyDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<SiteSetting>(provider=> {
                return siteSetting;
            });
            services.AddScoped<IFileWorker, FileWorker>(provider =>
            {
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                return new FileWorker(env.ContentRootPath+"/wwwroot");
            });
            services.AddScoped<AppIdentityErrorDescriber>();
            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddIdentity<AppUser, AppRole>(
                op =>
                {
                    op.Password.RequireDigit = false;
                    op.Password.RequireLowercase = false;
                    op.Password.RequireUppercase = false;
                    op.Password.RequiredLength = 8;
                    op.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<OnlinePharmacyDbContext>()
                .AddUserManager<AppUserManager>()
                //.AddRoleManager<>
                .AddErrorDescriber<AppIdentityErrorDescriber>()
                .AddDefaultTokenProviders();
            services.AddScoped<SignInManager<AppUser>>();
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = siteSetting.GoogleOAuth.ClinetID;
                    options.ClientSecret = siteSetting.GoogleOAuth.ClientSecret;
                });
            services.AddMvc();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
            });
            services.AddSession(options=>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
            services.AddRazorPages();
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
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name:"adminArea",
                    areaName:"admin",
                    "admin/{controller}/{action}/{id?}",
                    defaults: new { controller = "Dashboard", action = "Index" }
                    );
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "defualt",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                    );
            });
        }
    }
}
