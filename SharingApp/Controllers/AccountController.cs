using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SharingApp.Data;
using SharingApp.Mail;
using SharingApp.Models;
using SharingApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SharingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private IStringLocalizer<SharedResource> stringlocalizer;
        private readonly IMailHelper mailHelper;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,IStringLocalizer<SharedResource> stringlocalizer
            ,IMailHelper mailHelper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.stringlocalizer = stringlocalizer;
            this.mailHelper = mailHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Info()
        {  
            var CurrentUser = userManager.GetUserAsync(User);
            if (CurrentUser != null)
            {
                var info = new UserViewModel       
                {
                    Email = CurrentUser.Result.Email,
                    FirstName = CurrentUser.Result.FirstName,
                    LastName = CurrentUser.Result.LastName,
                    HasPassword=CurrentUser.Result.PasswordHash != null?true:false,
                    
                };
                return View(info);

            }
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Info(UserViewModel model)
        {
            var CurrenUser = await userManager.GetUserAsync(User);
            if(model != null && ModelState.IsValid)
            {
                CurrenUser.FirstName = model.FirstName;
                CurrenUser.LastName = model.LastName;
                model.HasPassword = model.HasPassword;
                var result= await userManager.UpdateAsync(CurrenUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = stringlocalizer["SuccessMessage"].Value;
                    return RedirectToAction("Info");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existedUser = await userManager.FindByEmailAsync(model.Email);
                if (existedUser == null)
                {
                    TempData["Error"] = "Invalid Email or Password";
                    return View(model);
                }
                if (existedUser.IsBlocked == false)
                {
                    // signInManager.PasswordSignInAsync(model.Email, model.Password, remmber me, stop account after 5 times password fails);
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, true);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Create", "Upload");
                    }
                    else if (result.IsNotAllowed)
                    {
                        TempData["Error"] = stringlocalizer["RequiredEmailConfirmation"].Value;
                    }
                }
                else
                    TempData["Error"] = "Account has been blocked";

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PasswordHash="Have password"
                };
                var result = await userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    // await signInManager.SignInAsync(user, remmber me);

                    //Create link
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail","Account",new {token= token,userId=user.Id },Request.Scheme);
                    //Send Email
                    StringBuilder body = new StringBuilder();
                    body.AppendLine("Sharing Application : Email Confirmation");
                    body.AppendFormat("to confirm your email, you should <a href='{0}'>click here </a>",url);
                    mailHelper.sendMail(new InputEmailMessage{
                        Body= body.ToString(),
                        Email=model.Email,
                        Subject="Email Confirmation",

                        });


                    return RedirectToAction("RequiredEmailConfirm");
                    //await signInManager.SignInAsync(user, true);
                    //return RedirectToAction("Index");
                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);

        }
        [HttpGet]
        public async Task <IActionResult> Logout()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ExternalLogin(string provider) //provider Facebook, Google
        {
            //FacebookDefaults.AuthenticationScheme      get name like FaceBook or Google ...
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, "/Account/ExternalResponse");
            return Challenge(properties,provider);
        }

        public async Task <IActionResult> ExternalResponse()
        {
            //get info
            var info =await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Message"] = "Login failed"; 
                return RedirectToAction("Login");
            }

            //check if user register before
            var loginResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,info.ProviderKey,false);
            //if not  register before
            if (loginResult.Succeeded == false)
            {
                //sign info details
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var LastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                //Create local account
                var UserToCreate = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = LastName,
                    EmailConfirmed=true
                };


                //sign in usermnage asp.net Users core
                var CreateResult = await userManager.CreateAsync(UserToCreate);

                if (CreateResult.Succeeded)
                {
                    //added to AspNetUserLogin database
                    var externalLoginResult = await userManager.AddLoginAsync(UserToCreate, info); //AspNetUserLogin Table
                    if (externalLoginResult.Succeeded)
                    {    
                        await signInManager.SignInAsync(UserToCreate, false, info.LoginProvider);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //remove account
                      await userManager.DeleteAsync(UserToCreate);
                    }
                }

            }

            var emailAddress = info.Principal.FindFirstValue(ClaimTypes.Email);
            var existedUser = await userManager.FindByEmailAsync(emailAddress);
            if (existedUser == null)
            {
                TempData["Error"] = "This account has been blocked";
                return RedirectToAction("Login");
            }
            if (existedUser.IsBlocked == true)
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var CurrenUser = await userManager.GetUserAsync(User);
            if (model != null && ModelState.IsValid)
            {
                var result = await userManager.ChangePasswordAsync(CurrenUser, model.CurrentPassword, model.NewPassword);
                var info = new UserViewModel
                {
                    Email = CurrenUser.Email,
                    FirstName = CurrenUser.FirstName,
                    LastName = CurrenUser.LastName,
                    HasPassword = CurrenUser.PasswordHash!=null ? true:false,
                }; 
                if (result.Succeeded)
                {
                    TempData["Success"] = stringlocalizer["ChangedPassMessage"].Value;
                    await signInManager.SignOutAsync();
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                //return View(model);
                return View("Info", info);
            }
            else
            {
                return NotFound();
            }

        }

        public async Task<IActionResult> AddPassword(AddPasswordViewModel model) 
        {
            var CurrenUser = await userManager.GetUserAsync(User);
            if (model != null && ModelState.IsValid)
            {
                var result = await userManager.AddPasswordAsync(CurrenUser, model.Password);
                if (result.Succeeded)
                {
                    TempData["Success"] = stringlocalizer["AddPassMessage"].Value;
                    return RedirectToAction("Info");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                return NotFound();
            }
            var info = new UserViewModel
            {
                Email = CurrenUser.Email,
                FirstName = CurrenUser.FirstName,
                LastName = CurrenUser.LastName,
                HasPassword = CurrenUser.PasswordHash != null ? true : false,
            };
            return View("Info", info);
        }

        public IActionResult RequiredEmailConfirm()
        {
            return View();
        }
    
        [HttpGet]
        public async Task <IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    if (user.EmailConfirmed==false)
                    {
                        var result = await userManager.ConfirmEmailAsync(user, model.Token);
                        if (result.Succeeded)
                        {
                            TempData["Sucess"] = "your email is confirmed Successfully";
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            foreach(var erroe in result.Errors)
                            {
                                ModelState.AddModelError("", erroe.Description);
                            }
                        }
                    }
                    else
                    {
                        TempData["Sucess"] = "your email is confirmed already";
                    }
                }
            }
            return View();
        }

        public IActionResult ForgetPassword() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exsiteUser = await userManager.FindByEmailAsync(model.Email);
                if(exsiteUser!= null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(exsiteUser);
                    var url = Url.Action("ResetPassword", "Account", new { token, model.Email }, Request.Scheme);
                    StringBuilder body = new StringBuilder();
                    body.Append("Sharing Application : Reset Password ");
                    body.Append("We are sending this email, because we have recieved a reset password request to your account");
                    body.AppendFormat("to reset your password <a href='{0}'>click here</a>", url);
                    mailHelper.sendMail(new InputEmailMessage
                    {
                        Email = model.Email,
                        Subject = "Reset Password",
                        Body = body.ToString()
                    });
                }
                TempData["Success"] = "Reset password Email has sent your email adress";
            }
            return View(model);
        }

        [HttpGet]
        public async Task <IActionResult> ResetPassword(VerifyPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existeUser = await userManager.FindByEmailAsync(model.Email);
                if(existeUser != null)
                {
                    var isValid =await userManager.VerifyUserTokenAsync(existeUser, TokenOptions.DefaultProvider,"ResetPassword",model.token);
                    if (isValid)
                    {
                        return View(new RestPasswordViewModel { Email=model.Email, Token=model.token });
                    }
                    else
                    {
                        TempData["Success"] = "Token is invalid";
                    }
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(RestPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existedUser = await userManager.FindByEmailAsync(model.Email);
                if(existedUser!= null)
                {
                    var result = await userManager.ResetPasswordAsync(existedUser,model.Token,model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData[""] = "Reset password completed successfully!";
                        return RedirectToAction("Login");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
