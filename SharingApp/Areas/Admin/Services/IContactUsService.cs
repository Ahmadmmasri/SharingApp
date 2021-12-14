using SharingApp.Areas.Admin.Models;
using SharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Services
{
   public interface IContactUsService
    {
       Task <IEnumerable<NotificationViewModel>> GetAll();

        Task ChangeStatusAsync(int id, bool status);

    }
}

