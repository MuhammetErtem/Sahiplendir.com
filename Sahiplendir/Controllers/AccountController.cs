using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sahiplendir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
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

    }
}
