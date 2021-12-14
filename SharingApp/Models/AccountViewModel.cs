using SharingApp.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Models
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessageResourceName ="Email",ErrorMessageResourceType =typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name ="EmailLable",ResourceType = typeof(SharedResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "PassLable", ResourceType = typeof(SharedResource))]

        public string Password { get; set; }
    }

    public class RegisterViewModel {
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "EmailLable", ResourceType = typeof(SharedResource))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "PassLable", ResourceType = typeof(SharedResource))]

        public string Password { get; set; }
        [Compare("Password")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "ConfirmPassLable", ResourceType = typeof(SharedResource))]

        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "FirstName", ResourceType = typeof(SharedResource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "LastName", ResourceType = typeof(SharedResource))]
        public string FirstName { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.Password)]
        [Display(Name = "PassLable", ResourceType = typeof(SharedResource))]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.Password)]
        [Display(Name = "PassLable", ResourceType = typeof(SharedResource))]
        public string NewPassword { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "ConfirmPassLable", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "CompairePass", ErrorMessageResourceType = typeof(SharedResource))]
        public string ConfirmPassword { get; set; }
    }

    public class AddPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.Password)]
        [Display(Name = "PassLable", ResourceType = typeof(SharedResource))]
        public string Password { get; set; }
    }
    
    public class ConfirmEmailViewModel
    {   
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
    }

    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Email { get; set; }
    }

    public class VerifyPasswordViewModel
    {
        [Required]
        public string token { get; set; }

        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Email { get; set; }
    }

    public class RestPasswordViewModel
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.Password)]
        [Display(Name = "PassLable", ResourceType = typeof(SharedResource))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "ConfirmPassLable", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "CompairePass", ErrorMessageResourceType = typeof(SharedResource))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string Email { get; set; }
    }

}
