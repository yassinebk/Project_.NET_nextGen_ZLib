using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User>? _userManager;
        private readonly SignInManager<User>? _signInManager;
        private readonly CoreModelsDataContext _context;

        public UsersController(CoreModelsDataContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {

            if (ModelState.IsValid)
                //   hb*x!rV%@6Ky 
            {

                Console.WriteLine(model);
                var user = _context.Users.FirstOrDefault(u => u.Email==model.Email);
                if (user == null) return View(model);
                var result =
                    await _signInManager.CheckPasswordSignInAsync(user, model.Password,
                        false);
                Console.WriteLine(result);
                if (result.Succeeded)
                {

                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Email), new Claim(ClaimTypes.Role, user.Role??"USER") };
                    await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, claims);
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
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
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email, Role = "USER" };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect("~/Users/Login?redirect=Signup");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
    }
}