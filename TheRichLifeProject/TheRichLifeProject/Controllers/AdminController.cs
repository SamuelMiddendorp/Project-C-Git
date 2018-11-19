using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Controllers
{
    [Authorize]   
    public class AdminController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminController(DatabaseContext context)
        {
            _context = context;
        }

        //Admin Login
        [AllowAnonymous, ValidateAntiForgeryToken]
        public IActionResult AdminLogin(string username, string password)
        {
            User admin = _context.Users.FirstOrDefault(u => u.Username == username);

            //Checks whether user (admin) is not null, password matches the one in the DB and the user has the role Admin
            if (admin != null && admin.Password == password.ComputeSha256Hash() && admin.Role == "Admin")
            {
                //List of claims that stores the user's name, id and role
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, username, ClaimValueTypes.String),
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString(), ClaimValueTypes.String),
                    new Claim(ClaimTypes.Role, admin.Role, ClaimValueTypes.String)
                };
                var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                //Sign in with cookie based authentication and additional authentication properties
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(15),
                    IsPersistent = true,
                    AllowRefresh = true
                });

                return RedirectToAction("Dashboard", "Administrator");
            }

            else
            {
                ViewBag.Allowence = "Username and Password do not match";
                return View();
            }

        }

        //Admin Logout
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AdminLogout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(AdminLogin));
        }

        //Admin Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        //Admin page users overview and management
        public async Task<IActionResult> Users()
        {
            var allUsers = await _context.Users.ToListAsync();
            return View(allUsers);
        }

        //Admin page products overview and management
        public async Task<IActionResult> Products()
        {
            var allProducts = await _context.Products.ToListAsync();
            return View(allProducts);
        }

        //TODO: Admin page with statistics about the website
        public IActionResult Statistics()
        {
            return View();
        }

      
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
