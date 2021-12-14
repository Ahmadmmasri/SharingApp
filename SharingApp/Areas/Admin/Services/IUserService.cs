using SharingApp.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Services
{
    public interface IUserService
    {
        List<AdminUserViewModel> GetAll();
        List<AdminUserViewModel> GetBlockedUsers();
        List<AdminUserViewModel> Search(string term);

        Task<OpertionResult> blockUserAsync(string userId);

        Task<int> UserRegestrationCountAsync(int month);
        Task<int> UserRegestrationCountAsync();
        Task InitializeAsync();
    }
}
