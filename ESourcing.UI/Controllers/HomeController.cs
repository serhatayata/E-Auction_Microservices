using ESourcing.Core.Entities;
using ESourcing.UI.Models;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppUser _user;
        public UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }

        public HomeController(AppUser user, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _user = user;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //Cookie temizleme
                    await _signInManager.SignOutAsync();
                    //lockoutOnFailure hatalı girişlerde bekletme
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        //return RedirectToAction("Index", "Home");
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email or password address is not valid.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email or password address is not valid.");
                    return View(model);
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AppUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                _user.FirstName = model.FirstName;
                _user.Email = model.Email;
                _user.LastName = model.LastName;
                _user.PhoneNumber = model.PhoneNumber;
                _user.UserName = model.UserName;
                if (model.UserSelectTypeId=="1")
                {
                    _user.IsBuyer = true;
                    _user.IsSeller = false;
                }
                else
                {
                    _user.IsSeller = true;
                    _user.IsBuyer = false;
                }

                var result = await _userManager.CreateAsync(_user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
