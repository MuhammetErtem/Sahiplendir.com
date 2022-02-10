using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NETCore.MailKit.Core;
using Sahiplendir.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IEmailService emailService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
            )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { IsPersistent = true, ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var result = await signInManager.PasswordSignInAsync(model.EMail, model.Password, model.IsPersistent, true);
            if (result.Succeeded)
                return Redirect(model.ReturnUrl ?? "/");
            ModelState.AddModelError("", "Geçersiz kullanıcı girişi");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            var user = new User
            {
                DateCreated = DateTime.Now,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                Gender = model.Gender,
                Name = model.Name,
                UserName = model.Email,

            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var userId = user.Id;
                var url = Url.Action("RegisterConfirmed", "Account", new { id = userId, token = token }, Request.Scheme);

                var templateFile = Path.Combine(webHostEnvironment.WebRootPath, "content", "templates", "emailconfirmation.html");
                var htmlBody = string.Format(
                    System.IO.File.ReadAllText(templateFile),
                    model.Name,
                    configuration.GetValue<string>("App:Title"),
                    url);



                await emailService.SendAsync(
                        model.Email,
                        "Sahiplendir E-Posta Doğrulama Mesajı",
                        htmlBody,
                        isHtml: true);

                return View("RegisterSucceded", user);
            }
            else
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError("", error.Description));
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RegisterConfirmed(string id, string token)
        {
            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                return View("RegisterFailed");
            }
        }
    }
}

