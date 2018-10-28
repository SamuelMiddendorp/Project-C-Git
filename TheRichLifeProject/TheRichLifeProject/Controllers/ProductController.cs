using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRichLifeProject.interfaces;
using TheRichLifeProject.Models;
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
        public IActionResult index(string searchvalue)
        {
            var products = from p in _context.Products select p;
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


        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetProductById(id);

            var model = new Product
            {
                Id = id,
                ProductName = product.ProductName,
                Price = product.Price,
                ImageSrc = product.ImageSrc,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Mature = product.Mature,
                Stock = product.Stock

            };

            return View(model);
        }


    }
}