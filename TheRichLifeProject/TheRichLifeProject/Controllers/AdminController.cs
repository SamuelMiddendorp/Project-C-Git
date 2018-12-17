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
using TheRichLifeProject.ViewModel;

namespace TheRichLifeProject.Controllers
{
    [Authorize  ]
    public class AdminController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminController(DatabaseContext context)
        {
            _context = context;
        }

        //Admin Login

        [HttpGet]
        public List<StatisticsViewModel> GetStats(int dataselector) {

            List<StatisticsViewModel> statistics = new List<StatisticsViewModel>();
            switch (dataselector) {
                case 0:
                statistics.Add(new StatisticsViewModel() { Name = "Exotic", Count = _context.Products.Count(x => x.Category == Category.Exotic) });
                statistics.Add(new StatisticsViewModel() { Name = "Lifestyle", Count = _context.Products.Count(x => x.Category == Category.Lifestyle) });
                statistics.Add(new StatisticsViewModel() { Name = "Fashion", Count = _context.Products.Count(x => x.Category == Category.Fashion) });
                    break;
                case 1:
                statistics.Add(new StatisticsViewModel() { Name = "User", Count = _context.Users.Count(x => x.Role == Role.User) });
                statistics.Add(new StatisticsViewModel() { Name = "Admin", Count = _context.Users.Count(x => x.Role == Role.Admin) });
                    break;
                case 2:
                    statistics.Add(new StatisticsViewModel() { Name = "Orders", Count = _context.Orders.Count() });
                    break;
            }
            

            return (statistics);
        }
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            //Search the user's username (that's been given as a parameter) 
            //in the DB and stores the specified user in
            //the variable admin
            User admin = _context.Users.FirstOrDefault(u => u.Username == username);

            //Checks whether user (admin) is not null, password matches the one in the DB and the user has the role Admin
            if (admin != null && admin.Password == password.ComputeSha256Hash() && admin.Role == Role.Admin)
            {
                //List of claims that stores the user's name, id and role
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, username, ClaimValueTypes.String),
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString(), ClaimValueTypes.String),
                    new Claim(ClaimTypes.Role, admin.Role.ToString(), ClaimValueTypes.String)
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

            else if (admin.Role != Role.Admin)
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

        //Goes to the page for adding a user
        [ActionName("AddUserPage")]
        public IActionResult AddUser()
        {
            return View();
        }

        //Adding a new user
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
                Province = newUser.Province,
                City = newUser.City,
                Address = newUser.Address,
                Zip = newUser.Zip
            };

            _context.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        //Page where the admin can edit the info of a user
        [ActionName("EditUserPage")]
        public async Task<IActionResult> EditUser(int? id)
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
        public IActionResult EditUser(User user)
        {

            //Searches for a user id that mathch the given id
            User currentUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            currentUser.Username = user.Username;
            currentUser.Password = user.Password.ComputeSha256Hash();
            currentUser.Role = user.Role;
            currentUser.Address = user.Address;

            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        //Goes to the page for deleting a user
        [ActionName("DeleteUserPage")]
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
        public IActionResult DeleteUser(int id)
        {
            User user = _context.Users.SingleOrDefault(u => id == u.Id);

            //Remove specified user from the DB
            _context.Users.Remove(user);

            //Await until DB has saved changes
            _context.SaveChanges();

            return RedirectToAction("Users");
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

        //Goes to the page for adding products
        [ActionName("AddProductPage")]
        public IActionResult AddProduct()
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

        //Goes to the page for editting products
        [ActionName("EditProductPage")]
        public async Task<IActionResult> EditProduct(int? id)
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

        //Goes the page for deleting a specific product
        [ActionName("DeleteProductPage")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await GetProductId(id);

            //Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            return View(product);
        }

        //Confirm deletion product
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        public async Task<Product> GetProductId(int? id)
        {
            Product productId = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            return productId;
        }
    }
}
