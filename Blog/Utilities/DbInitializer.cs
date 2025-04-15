using System.Runtime.CompilerServices;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {

            if (!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAuthor)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Main"
                }, "Welcome@15").GetAwaiter().GetResult();

                var appUser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@gmail.com");
                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser, WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult();

                }

                var AboutPage = new Page()
                {
                    Title = "About Us",
                    Slug = "about"
                };
                var ContactPage = new Page()
                {
                    Title = "Contact Us",
                    Slug = "contact"
                };
                var PrivacyPolicyPage = new Page()
                {
                    Title = "Privacy Policy",
                    Slug = "privacy"
                };

                _context.Pages.Add(AboutPage);
                _context.Pages.Add(ContactPage);
                _context.Pages.Add(PrivacyPolicyPage);
                _context.SaveChanges();

            }
        }


    }
}

