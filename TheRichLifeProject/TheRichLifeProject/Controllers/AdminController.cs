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
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            //Search the user's username (that's been given as a parameter) 
            //in the DB and stores the specified user in
            //the variable admin
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
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                    IsPersistent = true,
                    AllowRefresh = true
                });

                return RedirectToAction("Dashboard");
            }

            else if (admin.Role != "Admin")
            {
                ViewBag.Allowence = "You're not authorized to proceed";
                return View();
            }

            else
            {
                ViewBag.Allowence = "Username and Password do not match";
                return View();
            }

        }

        //Admin Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        //Admin Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        //Admin page with users overview and management
        public IActionResult Users()
        {
            var allUsers = _context.Users.ToList();
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

        // GET: Admin
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date-desc" : "Date";
            var users = from u in _context.Users select u;
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Username);
                    break;

            }
            return View(await _context.Users.ToListAsync());
        }

        //Page where the admin can edit the info of a user
        public async Task<IActionResult> EditUserPage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        //Edit user information
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditUser(int id, string username, string password,
            string role, string adress)
        {

            //Searches for a user id that mathch the given id
            User currentUser = _context.Users.FirstOrDefault(u => u.Id == id);

            currentUser.Username = username;
            currentUser.Password = password.ComputeSha256Hash();
            currentUser.Role = role;
            currentUser.Adress = adress;

            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        //Delete user
        public IActionResult DeleteUser(int? id)
        {
            //Check whether id is 0 and return a not found result if id is 0
            if (id == null)
            {
                return NotFound();
            }

            //Get the specified user by checking if the given id matches a single Id in the DB
            User user = _context.Users.SingleOrDefault(u => u.Id == id);

            return View(user);
        }

        //Confirmation for deleteting specified user
        public IActionResult DeleteUserConfirmation(int id)
        {
            User user = _context.Users.SingleOrDefault(u => id == u.Id);

            //Remove specified user from the DB
            _context.Users.Remove(user);

            //Await until DB has saved changes
            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        // GET: Admin/Details/5
        public IActionResult UserDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = _context.Users.SingleOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //Admin page for adding products
        public IActionResult AddProductPage()
        {
            return View();
        }

        //Adding a new product
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product newProduct)
        {
            newProduct = new Product
            {
                ProductName = newProduct.ProductName,
                ShortDescription = newProduct.ShortDescription,
                LongDescription = newProduct.LongDescription,
                ImageSrc = newProduct.ImageSrc,
                Price = newProduct.Price,
                Stock = newProduct.Stock,
                Mature = newProduct.Mature,
                Category = newProduct.Category,
                SubCategory = newProduct.SubCategory
            };
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }

        //Goes to the page for product details
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Goes the page for editting products
        public async Task<IActionResult> EditProductPage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            return View(product);
        }

        //Editting the product
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product)
        {
            Product currentProduct = _context.Products.SingleOrDefault(p => p.Id == product.Id);

            currentProduct.ProductName = product.ProductName;
            currentProduct.ShortDescription = product.ShortDescription;
            currentProduct.LongDescription = product.LongDescription;
            currentProduct.ImageSrc = product.ImageSrc;
            currentProduct.Price = product.Price;
            currentProduct.Stock = product.Stock;
            currentProduct.Mature = product.Mature;
            currentProduct.Category = product.Category;
            currentProduct.SubCategory = product.SubCategory;

            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        //Admin page for deleting a specific product
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            return View(product);
        }

        //Confirm deletion product
        public async Task<IActionResult> DeleteProductConfirmation(int id)
        {
            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        public IActionResult AddUserPage()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddUser(User newUser)
        {
            newUser = new User
            {
                Username = newUser.Username,
                Name = newUser.Name,
                SurName = newUser.SurName,
                Password = newUser.Password.ComputeSha256Hash(),
                PhoneNumber = newUser.PhoneNumber,
                Birth = newUser.Birth,
                Email = newUser.Email,
                Role = newUser.Role,
                DateRegistered = DateTime.Now,
                Adress = newUser.Adress
            };

            _context.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        public Product GetProductId(int? id)
        {
            Product productId = _context.Products.SingleOrDefault(p => p.Id == id);

            return productId;
        }
    }
}
