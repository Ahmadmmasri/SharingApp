using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedDate { get; set; }

        //you can add forign key her ex:
        //public int NationalityId { get; set; }
        //public virtual Nationality Nationality {get; set;}
    }
}
