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
        public ProductController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult index(string searchvalue)
        {
            var products = from p in _context.Products select p;
            if (!string.IsNullOrEmpty(searchvalue))
            {
                products = products.Where(x => x.ProductName.Contains(searchvalue)); 
            }
            return View(products);
        }

        
        //private readonly IProductRepository _productRepository;


        //public ProductController(IProductRepository productRepository)
        //{
            
        //    _productRepository = productRepository;
        //}

        //public ViewResult List()
        //{
        //    ProductListViewModel vm = new ProductListViewModel();
        //    vm.Products = _productRepository.Products;
        //    return View(vm);
        //}
        

    }
}