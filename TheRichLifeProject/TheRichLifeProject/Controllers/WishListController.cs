using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRichLifeProject.Models;
using TheRichLifeProject.ViewModel;

namespace TheRichLifeProject.Controllers
{
    public class WishListController : Controller
    {
        private readonly WishList _wishList;
        private readonly DatabaseContext _context;
        
        public WishListController(DatabaseContext context, WishList wishList)
        {
            _context = context;
            _wishList = wishList;

        }

        public ViewResult Index()
        {
            var items = _wishList.GetWishListItems();
            _wishList.WishListItems = items;

            var sCVM = new WishListViewModel
            {
                WishList = _wishList,
                
            };

            return View(sCVM);
        }

        public RedirectToActionResult AddToWishList(int productId)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                _wishList.AddToList(selectedProduct);

            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromWishList(int productId)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                _wishList.RemoveFromList(selectedProduct);

            }
            return RedirectToAction("Index");
        }
    }
}