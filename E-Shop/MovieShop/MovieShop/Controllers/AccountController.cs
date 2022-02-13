using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShop.DataAccess;
using MovieShop.Models.Models;
using MovieShop.Models.Static;
using MovieShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MovieDbContext _dbContext;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, MovieDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _dbContext.Users.ToListAsync();
            return View(users);

        }

        public IActionResult Login()
        {
            var reponse = new LoginVM();
            
            return View(reponse);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            if (!ModelState.IsValid) return View(loginVm);

            var user = await _userManager.FindByEmailAsync(loginVm.EmailAddress);
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVm.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }
                return View(loginVm);
            }
            TempData["Error"] = "Wrong credentials.Please try again!";
            return View(loginVm);
        }



        public IActionResult Register()
        {
            var reponse = new RegisterVM();

            return View(reponse);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (!ModelState.IsValid) return View(registerVm);

            var user = await _userManager.FindByEmailAsync(registerVm.EmailAddress);

            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVm);


            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVm.FullName,
                Email = registerVm.EmailAddress,
                UserName = registerVm.EmailAddress,

            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVm.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return View("RegisterCompleted");

        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movies");
        }
    }
}
