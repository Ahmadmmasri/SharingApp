using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Models
{
    public class InputModel
    {
        [Required]
        public IFormFile File  { get; set; }

    }

    public class InputUpload
    {
       public string OriginalName { get; set;}
       public string FileName { get; set; }

       public string CountentType { get; set;}
       public long Size { get; set;}
       public string UserId { get; set;}

    }

    public class UploadViewModel 
    {
        public string Id { get; set; }
        public string OriginalName { get; set; }
        public string FileName { get; set; }
        public decimal Size { get; set; }
        public string CountentType { get; set; }
        public DateTime UploadDate { get; set; }
        public long DownloadCount { get; set; } = 0;
    }
}
