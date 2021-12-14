using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharingApp.Areas.Admin.Models;
using SharingApp.Areas.Admin.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace SharingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IXLWorkbook _workbook;

        public UserController(IUserService userService, IXLWorkbook workbook)
        {
            this._userService = userService;
            this._workbook = workbook;
        }
        public IActionResult Index()
        {
             
            var result = _userService.GetAll();
            //return PartialView("_NotificationView",result);
            return View(result);
        }

        public IActionResult BlockedList()
        {
            var result = _userService.GetBlockedUsers().ToList();
            return View(result);
        }                                                   

        [HttpPost]
        public IActionResult Search(InputSearch model)
        {
            if (model != null)
            {
                var result = _userService.Search(model.term);
                return View("Index",result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task <IActionResult> Block(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                var result = await _userService.blockUserAsync(userId);
                if (result.Success)
                    TempData["Success"] = result.Message;
                else
                    TempData["Error"] = result.Message;

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "User Id is Not Found";
                return RedirectToAction("Index");
            }



        }


        public async Task <IActionResult> UsersCount()
        {
            var totalUserCount = await _userService.UserRegestrationCountAsync();
            var month = DateTime.Now.Month;
            var monthUserCount = await _userService.UserRegestrationCountAsync(month);

            return Json(new { total= totalUserCount, month= monthUserCount });
        }


        public IActionResult ExportExcel() 
        {
            //excel file extension
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var result = _userService.GetAll().ToList();

            string fileName = "Users.xlsx";
            var worksheet = _workbook.Worksheets.Add("All Users");
            worksheet.Cell(1, 1).Value="First name";
            worksheet.Cell(1, 2).Value="Last name";
            worksheet.Cell(1, 3).Value="Email";      
            worksheet.Cell(1, 4).Value="Created Date";      

            for(int i = 1; i < result.Count; i++)
            {
                var row = i + 1;
                worksheet.Cell(row, 1).Value = result[i-1].FirstName;
                worksheet.Cell(row, 2).Value = result[i - 1].LastName;
                worksheet.Cell(row, 3).Value = result[i - 1].Email;
                worksheet.Cell(row, 4).Value =result[i-1].CreatedDate;
            }

            using (var ms= new MemoryStream())
            {
                _workbook.SaveAs(ms);
                return File(ms.ToArray(),contentType,fileName);

            }
        }



    }
}
