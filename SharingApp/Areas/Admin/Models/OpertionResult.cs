using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Areas.Admin.Models
{
    public class OpertionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

       public static OpertionResult NotFound(string message="User Not Found")
        {
            return new OpertionResult
            {
                Success = false,
                Message = message
            };
        }

        public static OpertionResult Succeeded(string message = "Operation Compliated Successfully")
        {
            return new OpertionResult
            {
                Success = true,
                Message = message
            };
        }
    }
}
