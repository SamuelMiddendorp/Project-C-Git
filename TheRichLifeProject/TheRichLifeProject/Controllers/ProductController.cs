using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Fashion(string subcat, string searchvalue)
        {
            //Filtering the products
            var products = from p in _context.Products where p.Category == "Fashion" select p;
            if (!string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.ProductName.Contains(searchvalue)
                                        || x.ShortDescription.Contains(searchvalue) && x.Category.Contains("Fashion"));
            }

            if (!string.IsNullOrEmpty(subcat) && string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.SubCategory == (subcat));

            }

            //Creating the filter buttons
            CategoryViewModel subprod = new CategoryViewModel();
            List<string> categories = new List<string>();
            foreach (var item in _context.Products.Where(x => x.Category == "Fashion"))
            {
                if (!categories.Contains(item.SubCategory))
                {
                    categories.Add(item.SubCategory);
                }
                else
                {

                }
            }
            subprod.Products = products.ToList();
            subprod.Categories = categories;

            return View(subprod);
        }
        public IActionResult Exotic(string subcat, string searchvalue)
        {
            //Filter the products
            var products = from p in _context.Products where p.Category == "Exotic" select p;
            if (!string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.ProductName.Contains(searchvalue)
                                        || x.ShortDescription.Contains(searchvalue) && x.Category.Contains("Exotic"));
            }

            if (!string.IsNullOrEmpty(subcat) && !string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.SubCategory == (subcat) && 
                                               (x.ProductName.Contains(searchvalue) || x.ShortDescription.Contains(searchvalue)));

            }

            if (!string.IsNullOrEmpty(subcat))
            {
                products = products.Where(x => x.SubCategory == (subcat));

            }

            //Creating the filter buttons
            CategoryViewModel subprod = new CategoryViewModel();
            List<string> categories = new List<string>();
            foreach (var item in _context.Products.Where(x => x.Category == "Exotic"))
            {
                if(!categories.Contains(item.SubCategory))
                {
                    categories.Add(item.SubCategory);
                }
                else
                {

                }
            }
            subprod.Products = products.ToList();
            subprod.Categories = categories;

            return View(subprod);
        }
        public IActionResult Lifestyle(string subcat, string searchvalue)
        {
            //Filtering the products
            var products = from p in _context.Products where p.Category == "Lifestyle" select p;
            if (!string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.ProductName.Contains(searchvalue)
                                        || x.ShortDescription.Contains(searchvalue) && x.Category.Contains("Lifestyle"));
            }

            if (!string.IsNullOrEmpty(subcat) && string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.SubCategory == (subcat));

            }
            //Creating the filter buttons
            CategoryViewModel subprod = new CategoryViewModel();
            List<string> categories = new List<string>();
            foreach (var item in _context.Products.Where(x => x.Category == "Lifestyle"))
            {
                if (!categories.Contains(item.SubCategory))
                {
                    categories.Add(item.SubCategory);
                }
                else
                {

                }
            }
            subprod.Products = products.ToList();
            subprod.Categories = categories;

            return View(subprod);
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


        public IActionResult List()
        {
            throw new NotImplementedException();
        }
    }
}