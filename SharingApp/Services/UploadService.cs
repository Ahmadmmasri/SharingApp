using Microsoft.EntityFrameworkCore;
using SharingApp.Data;
using SharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Services
{
    public class UploadService : IUploadService
    {
        private readonly ApplicationDbContext _db;

        public UploadService(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task Create(InputUpload model)
        {
           await _db.Uploads.AddAsync(new Uploads
            {
                OriginalFileName = model.OriginalName,
                FileName =model.FileName,
                ContentType = model.CountentType,
                Size = model.Size,
                UserId =model.UserId,
            });
            await _db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var SelectedPhoto = await _db.Uploads.FirstOrDefaultAsync(x=>x.Id==id);
            if (SelectedPhoto != null)
            {
               _db.Uploads.Remove(SelectedPhoto);
               await _db.SaveChangesAsync();
            }
        }

        public async Task<UploadViewModel> Find(string id,string userId)
        {
            var SelectedUpload = await _db.Uploads.FirstOrDefaultAsync(x=>x.Id==id &&x.UserId==userId);
            if (SelectedUpload != null) 
            {
                //AutoMapper
                return new UploadViewModel
                {
                    Id = SelectedUpload.Id ,
                    OriginalName= SelectedUpload.OriginalFileName,
                    FileName = SelectedUpload.ContentType ,
                    Size = SelectedUpload.Size ,
                    CountentType = SelectedUpload.ContentType,
                    UploadDate = SelectedUpload.UploadDate,
                    DownloadCount = SelectedUpload.DownloadCount,
                };
            }
            return null;
        }

        public IQueryable<UploadViewModel> GetAll()
        {
            var result = _db.Uploads.OrderByDescending(x => x.UploadDate).Select(x => new UploadViewModel
            {
                FileName = x.FileName,
                OriginalName = x.OriginalFileName,
                CountentType = x.ContentType,
                Size = x.Size,
                UploadDate = x.UploadDate,
                DownloadCount = x.DownloadCount,
            });
            return result;
        }

        public IQueryable<UploadViewModel> GetBy(string userId)
        {
            var result= _db.Uploads.Where(uploads => uploads.UserId == userId).OrderByDescending(x => x.UploadDate)
               .Select(x => new UploadViewModel
               {
                   Id = x.Id,
                   FileName = x.FileName,
                   OriginalName = x.OriginalFileName,
                   CountentType = x.ContentType,
                   Size = x.Size,
                   UploadDate = x.UploadDate,
                   DownloadCount = x.DownloadCount,
               });
            return result;
        }

        public IQueryable<UploadViewModel> Search(string term)
        {
            var result = _db.Uploads.Where(x => x.OriginalFileName.Contains(term))
                                 .OrderBy(x => x.DownloadCount)
                                 .Select(x => new UploadViewModel
                                 {
                                     FileName = x.FileName,
                                     OriginalName = x.OriginalFileName,
                                     CountentType = x.ContentType,
                                     Size = x.Size,
                                     UploadDate = x.UploadDate,
                                 });
            return result;
        }
    }
}
