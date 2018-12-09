using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
            User User = new User
            {
                DateRegistered = DateTime.Now,
                Adress = "Azaleastraat 15",
                Username = "Test",
                Password = "12345",
                Role = "User"
            };

            Product Product = new Product
            {
                ProductName = "Laptop2",
                ShortDescription = "A small Laptop for some heavy work",
                Price = (decimal)19.99
            };
            _context.Add(Product);
            //_context.Add(User);
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
            //HttpContext.Session.SetObjectAsJson("Cart", Test);
            return View();
        }
        // Deze methode get de gegevens van de form en checkt dan of er een record in de database zit met de ingevulde gebruikersnaam en password
        // ToDo, er moet nog een hashing helper functie worden geschreven.
        public IActionResult Registration()
        {
            if (HttpContext.Session.GetString("emptyfield") == "1")
            {
                ViewBag.emptyfield = "This is not a valid phonenumber";
            }
            else if(HttpContext.Session.GetString("emptyfield") == "2")
            {
                ViewBag.emptyfield = "This username already exists in our database";
            }
            return View();
        }
        public IActionResult Register(string username, string password, string name, string surname, string email, string phonenumber, DateTime birth, string adress)
        {
            HttpContext.Session.SetString("emptyfield", "0");
            int p = Int32.Parse(phonenumber);
            if (p < 0) { 
                HttpContext.Session.SetString("emptyfield", "1");
                return RedirectToAction("Registration");
            }
            else if(_context.Users.Any(x => x.Username == username)){
                HttpContext.Session.SetString("emptyfield", "2");
                return RedirectToAction("Registration");
            }

            User newuser = new User()
            {
                Username = username,
                Password = password.ComputeSha256Hash(),
                Name = name,
                SurName = surname,
                Email = email,
                PhoneNumber = phonenumber,
                Birth = birth,
                Adress = adress,
                Role = "Admin",
                DateRegistered = DateTime.Now           
            };
            _context.Add(newuser);
            _context.SaveChanges();
            return View();
        }
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
                if (user != null && user.Password == password.ComputeSha256Hash())
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
                else { string alowance = "username and password do not match"; return View("Index", alowance); }
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