using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Controllers
{
    [Authorize]
    public class AdministratorController : Controller
    {
        private readonly DatabaseContext _context;

        public AdministratorController(DatabaseContext context)
        {
            _context = context;
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

        public async Task<IActionResult> EditUserPage(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        //Edit user information
        [HttpPost]
        public IActionResult EditUser(int id, string username, string password, 
            string role, string adress)
        {

            //Searches for a user id that mathch the given id
            User currentUser = _context.Users.Find(id);

            currentUser.Username = username;
            currentUser.Password = password.ComputeSha256Hash();
            currentUser.Role = role;
            currentUser.Adress = adress;
            _context.SaveChanges();
            return RedirectToAction("Users", "Admin");
        }

        //Delete user
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            //Check whether id is 0 and return a not found result if true
            if (id == 0)
            {
                return NotFound();
            }

            //await until Id of user is returned matching the specified Id of the user that will be deleted
            User user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        //Confirmation prompt for deleteting specified user
        public async Task<IActionResult> DeleteUserConfirmation(int id)
        {
            User user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            //Remove specified user from the DB
            _context.Users.Remove(user);

            //Await until DB has saved changes
            await _context.SaveChangesAsync();

            return RedirectToAction("Users", "Admin");
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> UserDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //TODO: Add product
        public  IActionResult AddProductPage()
        {
            return View();
        }

        //Adding a new product
        [HttpPost, ValidateAntiForgeryToken]
        public  IActionResult AddProduct(string productName, string shortDescription, string longDesccription,
            string imageSrc, decimal price, int stock, bool mature, string category, string subCategory)
        {
            Product newProduct = new Product
            {
                ProductName = productName,
                ShortDescription = shortDescription,
                LongDescription = longDesccription,
                ImageSrc = imageSrc,
                Price = price,
                Stock = stock,
                Mature = mature,
                Category = category,
                SubCategory = subCategory
            };
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Users", "Admin");
        }

        //Goes to the page for product details
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
        public async Task<IActionResult> EditProduct(int id, string productName, string shortDescription, string longdescription, 
            string imageSrc, decimal price, int stock, bool mature, string category, string subCategory)
        {
            Product currentProduct = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
            currentProduct.ProductName = productName;
            currentProduct.ShortDescription = shortDescription;
            currentProduct.LongDescription = longdescription;
            currentProduct.ImageSrc = imageSrc;
            currentProduct.Price = price;
            currentProduct.Stock = stock;
            currentProduct.Mature = mature;
            currentProduct.Category = category;
            currentProduct.SubCategory = subCategory;
            _context.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        //Delete product
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            return View();
        }

        //Confirm deletion product
        public async Task<IActionResult> DeleteProductConfirmation(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Products", "Admin");
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser([Bind("Id,Username,Password,Role,DateRegistered,Adress")] User user)
        {
            if (ModelState.IsValid)
            {
                user.DateRegistered = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Users", "Admin");
            }
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Role,DateRegistered,Adress")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
       

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
