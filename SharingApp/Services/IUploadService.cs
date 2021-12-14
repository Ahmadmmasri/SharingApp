using SharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Services
{
   public interface IUploadService
    {
        IQueryable<UploadViewModel> GetAll();
        IQueryable<UploadViewModel> GetBy(string userId);
        IQueryable<UploadViewModel> Search(string term);
        Task Create(InputUpload model);
        Task<UploadViewModel> Find(string id,string userId);
        Task Delete(string id);
    }
}
