using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Models
{
    public class InputSearch
    {
        [MinLength(3)]
        [required]
        public string term { get; set; }
    }
}
