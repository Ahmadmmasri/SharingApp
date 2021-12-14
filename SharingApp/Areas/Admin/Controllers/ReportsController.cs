using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SharingApp.Areas.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public class ReportsController : Controller
    {
        private readonly IUserService userService;

        public ReportsController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Users()
        {
            var model = userService.GetAll().ToList();
            return new ViewAsPdf(model);
        }
    }
}
