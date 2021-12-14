using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Data
{
   // public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Uploads> Uploads { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        //check image size
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Uploads>().Property(x => x.Size).HasColumnType("decimal(18,4)");
            base.OnModelCreating(builder);
        }
    }
}
