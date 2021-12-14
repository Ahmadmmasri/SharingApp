using SharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Models
{
    public class ContactUsViewModel : ContactViewModel
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public DateTime SendDate { get; set; }
    }
}
