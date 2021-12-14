using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharingApp.Data;
using SharingApp.Models;
using SharingApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharingApp.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment env;
        private readonly IUploadService _uploadService;

        public UploadController(ApplicationDbContext context, IWebHostEnvironment env,IUploadService uploadService)
        {
            this._db = context;
            this.env = env;
            this._uploadService = uploadService;
        }

        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        public IActionResult Index()
        {
            var result = _uploadService.GetBy(UserId);
            return View(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task <IActionResult> Browse(int RequiredPage=1)
        {
            var result = _uploadService.GetAll();
           var model=  await GetPageData(result,RequiredPage);

            return View(model);


        }


        private async Task<List<UploadViewModel>> GetPageData(IQueryable<UploadViewModel> result,int RequiredPage)
        {

            const int pageSize = 5;
            RequiredPage = RequiredPage <= 0 ? 1 : RequiredPage;
            decimal CountRows = await _db.Uploads.CountAsync();
            decimal pageCount = Math.Ceiling(CountRows / pageSize);
            if (RequiredPage > pageCount)
            {
                RequiredPage = 1;
            }
            int skipCount = (RequiredPage - 1) * pageSize;

            var PageData = await result
              .Skip(skipCount)
              .Take(pageSize)
              .ToListAsync();


            ViewBag.CurrentPage = RequiredPage;
            ViewBag.PagesCount = pageCount;

            return (PageData);

        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Results(string term,int RequiredPage=1)
        {
            var result = _uploadService.Search(term);
            var model = await GetPageData(result, RequiredPage);

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Create photo and save it into database
        [HttpPost]
        public async Task<IActionResult> Create(InputModel model)
        {
            if (ModelState.IsValid)
            {
                var newName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(model.File.FileName);
                // Concat=  newName + extension
                var fileName = string.Concat(newName, extension);
                var root = env.WebRootPath;
                var path = Path.Combine(root, "Uploads", fileName);

                using (var file = System.IO.File.Create(path))
                {
                    await model.File.CopyToAsync(file);
                }
                await _uploadService.Create(new InputUpload
                {
                    OriginalName = model.File.FileName,
                    FileName = fileName,
                    CountentType = model.File.ContentType,
                    Size = model.File.Length,
                    UserId = UserId,
                });
                return RedirectToAction("Index");

            }
            return View();
        }
    
        [HttpGet]
        public async Task<IActionResult> Delete(string id, string userId)
        {
            userId = UserId;
            var SelectedPhoto =await _uploadService.Find(id,userId);
            if (SelectedPhoto != null)
            {
                try
                {
                    await _uploadService.Delete(id);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return NotFound();
                }
            }
            else
                return NotFound();

        }

        [HttpGet]
        public async Task <IActionResult> Download(string fileName)
        {
            var SelectedFile = await _db.Uploads.FirstOrDefaultAsync(x=>x.FileName== fileName);
            SelectedFile.DownloadCount++;
            if (SelectedFile == null)
            {
                return NotFound();
            }
            var path = "/Uploads/" + SelectedFile.FileName;
            _db.Update(SelectedFile);
            await _db.SaveChangesAsync();
            //clean cache to show dounload number withour reload
            Response.Headers.Add("Expires", DateTime.Now.AddDays(-3).ToLongDateString());
            Response.Headers.Add("Cache-Control","no-cache");
            //end clean cach
            return File(path,SelectedFile.ContentType,SelectedFile.OriginalFileName);
        }
    }
}
