using System.Collections.Specialized;
using AspNetCoreHero.ToastNotification.Abstractions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotyfService _notification;
       

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INotyfService notification)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notification = notification;
        }
        public IActionResult Index()
        {
            return View();
        }



        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existingUser = await _userManager.Users.FirstOrDefaultAsync( x => x.UserName == vm.Username);
                if (existingUser == null)
                {
                    _notification.Error("Username does not exist");
                    return View(vm);
                }

            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);

                if (!verifyPassword)
            {
                _notification.Error("Password does not match");
                return View(vm);
            }
            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);
            _notification.Success("Login Successful");
            return RedirectToAction("Index", "User", new { area = "Admin" });
               

            }

        [HttpPost]
        public IActionResult Logout()
        {
            _notification.Success("You are successfully logged out");
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new {area = ""});

        }

    }
 }

