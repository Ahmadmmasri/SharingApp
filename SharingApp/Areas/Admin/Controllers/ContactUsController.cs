using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingApp.Areas.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public class ContactUsController : Controller
    {
        private readonly IContactUsService _contactUsService;

        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _contactUsService.GetAll();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id,bool status)
        {
            await _contactUsService.ChangeStatusAsync(id,status);
            return RedirectToAction("Index");
        }

    }
}
