using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;
        public LoginController(DatabaseContext context)
        {
            _context = context;
             //SetData();
        }
        // Een methode om wat testdate toe te voegen
        public void SetData()
        {
            Product Product = new Product
            {
                ProductName = "Phone",
                Description = "A small phone"                              
            };
            _context.Add(Product);
            _context.SaveChanges();

        }
        public IActionResult Index()
        {
            User Test = new User
            {
                Username = "Test2",
                Password = "123",
                Role = "User",
                Adress = "TestAdress 12",
                DateRegistered = DateTime.Now
            };
            HttpContext.Session.SetObjectAsJson("Cart", Test);
            return View();
        }
        // Deze methode get de gegevens van de form en checkt dan of er een record in de database zit met de ingevulde gebruikersnaam en password
        // ToDo, er moet nog een hashing helper functie worden geschreven.
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (User.Identity.Name != null)
            {
                ViewBag.Allowance = "You are already logged in";
            }
                // Gebruik linq om de user te krijgen
                User user = _context.Users.FirstOrDefault(u => u.Username == username);
                // Check het wachtwoord
                if (user != null && user.Password == password)
                {
                    // Maak de identity van de gebruiker aan
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username, ClaimValueTypes.String),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String),
                        new Claim(ClaimTypes.Role, user.Role, ClaimValueTypes.String)
                    };
                    var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                            IsPersistent = true,
                            AllowRefresh = false
                        });
                    ViewBag.Allowance = "Logged in !";
                    return RedirectToAction("Index", "Home");
                }
                else { ViewBag.Allowance = "username and password do not match"; return View(); }
        }
        public IActionResult Denied()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}