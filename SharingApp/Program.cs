using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharingApp.Areas.Admin.Services;
using SharingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //Run migrations => update-database
            using (var scope=host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                //var dbContext = provider.GetRequiredService<ApplicationDbContext>();
                //dbContext.Database.Migrate();

                //seed.
                var UserService = provider.GetRequiredService<IUserService>();
                await UserService.InitializeAsync();

            }
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
