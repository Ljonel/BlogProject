using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AuthenticationController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;

        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new Login());
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login viewLogin)
        {
            //only admin can go to see the Admin Panel
            var result = await _signInManager.PasswordSignInAsync(viewLogin.UserName, viewLogin.Password, false, false);
            if(!result.Succeeded)
            {
                return View(viewLogin);

            }

            var user = await _userManager.FindByNameAsync(viewLogin.UserName);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                return RedirectToAction("Index", "AdminPanel");
            }
            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        /*public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }*/


        [HttpGet]
        public IActionResult Register()
        {
            return View(new Register());
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register viewRegister)
        {
            if (!ModelState.IsValid)
            {
                return View(viewRegister);
            }

            var user = new IdentityUser
            {
                UserName = viewRegister.UserName,
                Email = viewRegister.Email,
            };

            var result = await _userManager.CreateAsync(user, viewRegister.Password);   //@Damian123

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            return View(viewRegister);
        }
    }
}
