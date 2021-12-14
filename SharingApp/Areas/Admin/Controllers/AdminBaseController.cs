using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {

        //[Area("Admin")]
        //[Authorize(Roles = UserRoles.Admin)]
        public IActionResult Index()
        {
            return View();
        }


    }
}
