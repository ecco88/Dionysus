﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Dionysus.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Dionysus.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dionysus.Controllers
{
    public class AccountController : Controller
    {

//        private readonly UserManager<ApplicationUser> _userManager;
  //      private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(string returnUrl = "/Home")
        {

            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            Dionysus.Models.User user = await new DAL().LogIn(email, password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(14)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), props);

            string returnUrl = ViewBag.ReturnUrl ?? Url.Action("Index", "Home");
            return new RedirectResult(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = "/Home")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string firstName, string LastName)
        {
            Dionysus.Models.User user = await new DAL().Register(firstName, LastName, email);
           
            string returnUrl = ViewBag.ReturnUrl ?? Url.Action("Index", "Home");
            return new RedirectResult(returnUrl);
        }
    }
}
