using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingApp.Areas.Admin.Models;
using SharingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
           this._db = db;
        }

        public new IActionResult Index()
        {
            var Lastfications = _db.Contacts.ToList().OrderByDescending(x => x.SendDate).Take(5);
            List<Contact> contacts = Lastfications.ToList();
            List<NotificationViewModel> notification = new List<NotificationViewModel>();
            foreach (var contact in contacts)
            {
                 notification.Add( new NotificationViewModel()
                    {
                        Id = contact.Id,
                        Email = contact.Email,
                        Message = contact.Message,
                        SendDate = contact.SendDate,
                        notifictionNumber = Lastfications.Count()
                    });
            }
            return View(notification.ToList());
        }    
    }
}
