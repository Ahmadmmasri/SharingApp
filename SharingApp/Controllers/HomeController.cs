using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SharingApp.Areas.Admin.Models;
using SharingApp.Areas.Admin.Services;
using SharingApp.Data;
using SharingApp.Hubs;
using SharingApp.Mail;
using SharingApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMailHelper _Mail;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly UserManager<ApplicationUser> userManager;
        static int counter=0;


        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db,
            IMailHelper Mail,
            IHubContext<NotificationHub> notificationHub,
            UserManager<ApplicationUser> userManager
            )
        {
            _logger = logger;
            this._db = db;
            this._Mail = Mail;
            this._notificationHub = notificationHub;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
          var  highestDownload = _db.Uploads.OrderByDescending(x => x.DownloadCount).Take(4)
                .Select(x => new UploadViewModel
                  {
                      FileName = x.FileName,
                      OriginalName = x.OriginalFileName,
                      CountentType = x.ContentType,
                      Size = x.Size,
                      UploadDate = x.UploadDate,
                      DownloadCount = x.DownloadCount,
                  }); ;
            ViewBag.Popular = highestDownload;
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.Contacts.Add(new Contact
                {
                   Email=model.Email,
                   Message=model.Message,
                   Name=model.Name,
                   Subject=model.Subject,
                   userId= UserId,
                   SendDate= Convert.ToDateTime( DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") )
                });
               await _db.SaveChangesAsync();
                //send email
                //format email format text
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<h1>Sharing App -Unread Message</h1>");
                builder.AppendFormat("Email : {0} \n", model.Email);
                builder.AppendFormat("Name : {0} \n", model.Name);
                //new line
                builder.AppendLine();
                builder.AppendFormat("Subject : {0} \n", model.Subject);
                builder.AppendFormat("Message : {0} \n", model.Message);
                _Mail.sendMail(new InputEmailMessage
                {
                    Email = model.Email,
                    Subject = model.Subject,
                    Body = builder.ToString(),
                });

                //send notification
                var adminUsers =await userManager.GetUsersInRoleAsync(UserRoles.Admin);
                var adminIds = adminUsers.Select(user => user.Id);
                if (adminIds.Any())
                {
                    await _notificationHub.Clients.Users(adminUsers.Select(user => user.Id)).SendAsync("RecievedNotification", "you have unraed message",++counter);
                }
                //end of notification send
                TempData["Message"] = "Message has been sent succefully!";
                return RedirectToAction("Contact");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        //set language
        [HttpGet]
        public IActionResult SetCulture(string lang,string ReturnUrl=null)
        {
            if ( !string.IsNullOrEmpty(lang) )
            {
                Response.Cookies.Append
                    (
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                    );
            }
            if (ReturnUrl != null)
            {
                return LocalRedirect(ReturnUrl);
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
