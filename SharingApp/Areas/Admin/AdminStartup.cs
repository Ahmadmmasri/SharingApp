using ClosedXML.Excel;
using Microsoft.Extensions.DependencyInjection;
using SharingApp.Areas.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin
{
    public static class AdminStartup
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IXLWorkbook, XLWorkbook>();
            services.AddTransient<IContactUsService, ContactUsService>();

            return services;

        }
    }
}


