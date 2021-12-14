using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharingApp.Areas.Admin.Models;
using SharingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(ApplicationDbContext db,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this._db = db;
            this._userManger = userManager;
            this._roleManager = roleManager;
        }
        public async Task<OpertionResult> blockUserAsync(string userId)
        {
            var exsiteUser = await _db.Users.FindAsync(userId);
            var BlockState = exsiteUser.IsBlocked;
            if (exsiteUser == null)
                return OpertionResult.NotFound();
            else
            {
                exsiteUser.IsBlocked = !BlockState;
                _db.Update(exsiteUser);
                await _db.SaveChangesAsync();
                return OpertionResult.Succeeded();
            }
        }

        public List<AdminUserViewModel> GetAll()
        {
            List <ApplicationUser> Users = new List<ApplicationUser>();
            Users = _db.Users.OrderByDescending(x=>x.CreatedDate).ToList();
            List<AdminUserViewModel> AllUsers = new List<AdminUserViewModel>();
            foreach(var userInfo in Users)
            {
                AllUsers.Add(new AdminUserViewModel
                {
                    Email = userInfo.Email,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    IsBlocked = userInfo.IsBlocked,
                    CreatedDate = userInfo.CreatedDate,
                    UserId=userInfo.Id,
                });

            }
            var result = AllUsers;

            return (result);
        }

        public List<AdminUserViewModel> GetBlockedUsers()
        {
            List<ApplicationUser> Users = new List<ApplicationUser>();
            Users = _db.Users.Where(x=>x.IsBlocked==true).OrderByDescending(x=>x.CreatedDate).ToList();
            List<AdminUserViewModel> AllUsers = new List<AdminUserViewModel>();
            foreach (var userInfo in Users)
            {
                AllUsers.Add(new AdminUserViewModel
                {
                    Email = userInfo.Email,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    IsBlocked = userInfo.IsBlocked,
                    CreatedDate = userInfo.CreatedDate,
                    UserId = userInfo.Id,
                });

            }
            var result =  AllUsers.ToList();

            return (result);
        }


        public List<AdminUserViewModel> Search(string term)
        {
            List<ApplicationUser> Users = new List<ApplicationUser>();
            Users = _db.Users.Where(x => x.Email == term 
            || x.FirstName.Contains(term)
            || x.LastName.Contains(term)
            || (x.FirstName+" "+x.LastName).Contains(term)
            ).OrderByDescending(x=>x.CreatedDate).ToList();

            List<AdminUserViewModel> AllUsers = new List<AdminUserViewModel>();
            foreach (var userInfo in Users)
            {
                AllUsers.Add(new AdminUserViewModel
                {
                    Email = userInfo.Email,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    IsBlocked = userInfo.IsBlocked,
                    CreatedDate = userInfo.CreatedDate,
                });
            }
            var result = AllUsers;

            return (result);
        }

        public async Task<int> UserRegestrationCountAsync()
        {
            var count = await _db.Users.CountAsync();
            return (count);
        }

        public async Task<int> UserRegestrationCountAsync(int month)
        {
            var Year = DateTime.Now.Year;
            var count = await _db.Users
                .CountAsync(x=>x.CreatedDate.Month==month && x.CreatedDate.Year==Year);
            return (count);
        }

        public async Task InitializeAsync()
        {
            bool IsAdmin = await _roleManager.RoleExistsAsync(UserRoles.Admin);
           if (IsAdmin==false)
            {
               await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            var adminEmail = "Admin@a.com";
           var IsAdminExiste= await _userManger.FindByEmailAsync(adminEmail);
            if(IsAdminExiste == null)
            {
                var user = new ApplicationUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    PhoneNumber = "0000000000",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await _userManger.CreateAsync(user,"Admin123#");

                await _userManger.AddToRoleAsync(user,UserRoles.Admin);

            }

        }

    }
}
