using Microsoft.EntityFrameworkCore;
using SharingApp.Areas.Admin.Models;
using SharingApp.Data;
using SharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly ApplicationDbContext _db;

        public ContactUsService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task ChangeStatusAsync(int id, bool status)
        {
            var selectedItem = await _db.Contacts.FindAsync(id);
            if (selectedItem != null)
            {
                selectedItem.State = status;
                _db.Update(selectedItem);
                await _db.SaveChangesAsync();
            }
        }

        public async Task <IEnumerable<NotificationViewModel>> GetAll()
        {
            List<NotificationViewModel> AllUsers = new List<NotificationViewModel>();

            var Users = await _db.Contacts.ToListAsync();
            foreach (var userInfo in Users)
            {
                AllUsers.Add(new NotificationViewModel
                {
                    Email = userInfo.Email,
                    Message = userInfo.Message,
                    Name = userInfo.Name,
                    Subject = userInfo.Subject,
                    SendDate = userInfo.SendDate,
                    State = userInfo.State,
                    Id = userInfo.Id,
                });
            }
            var result = AllUsers.ToList(); 

            return (result);
        }
    }
}
