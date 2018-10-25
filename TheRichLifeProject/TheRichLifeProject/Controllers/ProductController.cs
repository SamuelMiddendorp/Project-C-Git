using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly DatabaseContext _context;
        public ProductController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchValue)
        {
            var products = from p in _context.Products select p;
            if (!string.IsNullOrEmpty(searchValue))
            {
                products = products.Where(x => x.ProductName.Contains(searchValue)); 
            }
            return View(products);
        }
    }
}