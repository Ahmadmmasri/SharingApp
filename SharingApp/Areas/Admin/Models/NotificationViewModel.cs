using SharingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Models
{
    public class NotificationViewModel : Contact
    {
        public int notifictionNumber { get; set; }
    }
}
