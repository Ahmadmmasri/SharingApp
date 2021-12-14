using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharingApp.Areas.Admin;
using SharingApp.Data;
using SharingApp.Hubs;
using SharingApp.Mail;
using SharingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp
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
            services.AddControllersWithViews().AddViewLocalization(options => options.ResourcesPath = "Resources");
            services.AddTransient<IMailHelper, MailHelper>();

            /*can add service for admin here direct
            //services.AddTransient<IUserService, UserService>();
            
            or can insert startup class in area and add it here
            */
            services.AddAdminServices();


            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.SignIn.RequireConfirmedEmail = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(optios =>
            {
                optios.TokenLifespan = TimeSpan.FromHours(4);
            });

            //For multi Languages
            services.AddLocalization();
            services.AddTransient<IUploadService, UploadService>();
            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = Configuration["Authentication:Facebook:AppId"];
                    options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(options=> {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });

            services.AddSignalR();


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();
                                 
            app.UseAuthorization();

            //add languages
            var supportCulture = new[] { "ar-SA", "en-US" };
            app.UseRequestLocalization(x=> {
                x.AddSupportedUICultures(supportCulture);
                x.AddSupportedCultures(supportCulture);
                x.SetDefaultCulture("en-Us");
            }); //ar-SA en-US

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                endpoints.MapHub<NotificationHub>("/notify");
            });

            Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath);
        }
    }
}
