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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TheRichLifeProject.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        public AdminController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            hostingEnvironment = environment;
        }

        //Admin Login

        [HttpGet]
        public List<StatisticsViewModel> GetStats(int dataselector)
        {

            List<StatisticsViewModel> statistics = new List<StatisticsViewModel>();
            switch (dataselector)
            {
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
                case 3:
                    foreach (var province in Enum.GetNames(typeof(Province)))
                    {
                        Province selected = (Province)Enum.Parse(typeof(Province), province);
                        statistics.Add(new StatisticsViewModel() { Name = province, Count = _context.Users.Where(x => x.Province == selected).Count() });
                    }
                    break;
                case 4:
                    List<Product> Products = _context.Products.ToList();
                    List<StatisticsViewModel> tade = new List<StatisticsViewModel>();
                    foreach (var product in Products)
                    {
                        tade.Add(new StatisticsViewModel() { Name = product.ProductName, Count = _context.OrderDetails.Where(x => x.Product.Id == product.Id).Count() });
                    }
                    tade = tade.OrderByDescending(x => x.Count).ToList();
                    int y = 0;
                    while (y < 5)
                    {
                        statistics.Add(tade[y]);
                        y++;
                    }

                    break;
            }


            return (statistics);
        }
        /*[AllowAnonymous]
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

        //Admin Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }*/

        //Admin page with users overview and management
        public IActionResult Users()
        {
            var allUsers = _context.Users.ToList();
            return View("~/Views/Admin/Users/Users.cshtml", allUsers);
        }

        //Admin page products overview and management
        public async Task<IActionResult> Products()
        {
            var allProducts = await _context.Products.ToListAsync();
            return View("~/Views/Admin/Products/Products.cshtml", allProducts);
        }

        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        // GET: User details
        public async Task<IActionResult> UserDetails(int? id)
        {

            var user = await GetUserId(id);

            if (id == null && user == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Users/UserDetails.cshtml", user);
        }

        //User page for adding new a new user
        [ActionName("AddUserPage")]
        public IActionResult AddUser()
        {
            return View("~/Views/Admin/Users/AddUserPage.cshtml");
        }

        //Adding a new user
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddUser(User newUser)
        {
            string viewPath = "~/Views/Admin/Users/AddUserPage.cshtml";
            if (ModelState.IsValid)
            {
                var usernameExist = _context.Users.Where(x => x.Username == newUser.Username).FirstOrDefault();
                var emailExist = _context.Users.Where(x => x.Email == newUser.Email).FirstOrDefault();

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

                if (usernameExist == null && emailExist == null)
                {

                    _context.Add(newUser);
                    _context.SaveChanges();

                    return RedirectToAction("~/Views/Admin/Users/Users.cshtml");
                }
                else
                {
                    return View(viewPath, newUser);
                }
            }
            return View(viewPath, newUser);
        }

        //Page where the admin can edit the info of a user
        [ActionName("EditUserPage")]
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await GetUserId(id);

            return View("~/Views/Admin/Users/EditUserPage.cshtml", user);
        }

        //Edit user information
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                User currentUser = await GetUserId(user.Id);
                currentUser.Username = user.Username;
                if (currentUser.Password == null)
                {
                    currentUser.Password = currentUser.Password;
                }
                else
                {
                    currentUser.Password = user.Password.ComputeSha256Hash();
                }
                currentUser.Name = user.Name;
                currentUser.SurName = user.SurName;
                currentUser.Role = user.Role;
                currentUser.Address = user.Address;
                currentUser.City = user.City;
                currentUser.Zip = user.Zip;
                currentUser.Province = user.Province;
                currentUser.Email = user.Email;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Birth = user.Birth;

                _context.SaveChanges();

                return RedirectToAction("Users");
            }

            return View("~/Views/Admin/Users/EditUserPage.cshtml", user);
        }

        //Goes to the page for deleting a user
        [ActionName("DeleteUserPage")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            //Check whether id is 0 and return a not found result if id is 0
            if (id == null)
            {
                return NotFound();
            }

            //Get the specified user by checking if the given id matches a single Id in the DB
            User user = await GetUserId(id);

            return View("~/Views/Admin/Users/DeleteUserPage.cshtml", user);
        }

        //Confirmation for deleteting specified user
        public async Task<IActionResult> DeleteUser(int id)
        {
            User user = await GetUserId(id);

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
            Product product = await GetProductId(id);

            if (id == null || product == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/Products/ProductDetails.cshtml", product);
        }

        //Goes to the page for adding products
        [ActionName("AddProductPage")]
        public IActionResult AddProduct()
        {
            return View("~/Views/Admin/Products/AddProductPage.cshtml");
        }

        //Adding a new product
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product newProduct)
        {
            if (ModelState.IsValid)
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
            return View("~/Views/Admin/Products/AddProductPage.cshtml", newProduct);

        }

        //Page for editting products
        [ActionName("EditProductPage")]
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await GetProductId(id);

            return View("~/Views/Admin/Products/EditProductPage.cshtml", product);
        }

        //Editting the product
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                Product currentProduct = await GetProductId(product.Id);

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
            return View("~/Views/Admin/Products/AddProductPage.cshtml", product);
            
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

            return View("~/Views/Admin/Products/DeleteProductPage.cshtml", product);
        }

        //Confirm deletion product
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await GetProductId(id);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        public IActionResult UserOrders(int id)
        {
            var orders = _context.Orders.ToList();
            var orderdetails = _context.OrderDetails.ToList();
            var products = _context.Products.ToList();
            var order = new List<Order>();
            User user = _context.Users.SingleOrDefault(u => u.Id == id);
            foreach (var item in orders.Where(x => x.User == user))
            {
                item.OrderDetails = new List<OrderDetail>();
                foreach (var item2 in orderdetails.Where(x => x.Order == item))
                {
                    item.OrderDetails.Add(item2);
                }
                order.Add(item);
            }
            return View("~/Views/Admin/Users/UserOrders.cshtml", order);
        }

        public IActionResult ChangeStatus()
        {
            return View();
        }

        //Get product id
        public async Task<Product> GetProductId(int? id)
        {
            //Get product by searching the DB for UserId matching the Id given as argument
            Product productId = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
            return productId;
        }

        //Get user id
        public async Task<User> GetUserId(int? id)
        {
            //Get user by searching the DB for UserId matching the Id given as argument
            User userId = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            return userId;
        }
    }
}
