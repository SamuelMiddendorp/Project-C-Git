using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheRichLifeProject.interfaces;
using TheRichLifeProject.Models;
using TheRichLifeProject.ViewModel;
using TheRichLifeProject.ViewModels;

namespace TheRichLifeProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IProductRepository _productRepository;

        public ProductController(DatabaseContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }
        public IActionResult Index(string searchvalue, string category)
        {
        
            var products = from p in _context.Products select p;
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(x => x.Category == (Category)Enum.Parse(typeof(Category), category));
            }
            if (!string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.ProductName.Contains(searchvalue)
                                        || x.ShortDescription.Contains(searchvalue));                   
            }
            return View(products);
        }

        //Voor de search
        [HttpPost]
        public string Index(string searchvalue, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchvalue;
        }
        [HttpPost]
        public string Lifestyle(string searchvalue, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchvalue;
        }
        [HttpPost]
        public string Exotic(string searchvalue, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchvalue;
        }
        [HttpPost]
        public string Fashion(string searchvalue, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchvalue;
        }


        public IActionResult Detail(int id)
        { 

            var product = _productRepository.GetProductById(id);
            var reviews = _context.Reviews.Where(x => x.Product == product).ToList();
            var model = new ProductViewModel
            {
                Reviews = reviews,
                Product = product
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Review(string title, string body, int productid)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            User CurrentUser = _context.Users.Find(Int32.Parse(userId));
            var product = _context.Products.Find(productid);
            var review = new Review
            {
                Title = title,
                Body = body,
                User = CurrentUser,
                Product = product,
                PublishDate = DateTime.Now
            };
            _context.Add(review);
            _context.SaveChanges();
            return RedirectToAction("Detail");
        }

        public IActionResult List()
        {
            throw new NotImplementedException();
        }
    }
}