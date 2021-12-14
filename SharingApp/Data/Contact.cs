using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Data
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public bool State { get; set; }
        public DateTime SendDate { get; set; }

        [ForeignKey("User")]
        public string userId { get; set; }

        public virtual ApplicationUser User { get; set; }


    }
}
