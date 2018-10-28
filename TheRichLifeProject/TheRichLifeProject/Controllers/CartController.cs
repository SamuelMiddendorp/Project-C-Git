using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRichLifeProject.Models;
using System.Security.Claims;
namespace TheRichLifeProject.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseContext _context;
        public CartController(DatabaseContext context)
        {
            _context = context;

        }
        public IActionResult Add(int productId)
        {
            
            var cart = GetCart();
            var cartItem = cart.SingleOrDefault(c => c.ProductId == productId);
            if (cartItem == null)
            {
                var newcartItem = new CartItem
                {
                    Quantity = 1,
                    ProductId = productId,
                    Product = _context.Products.SingleOrDefault(x => x.Id == productId)
                    
                };
                cart.Add(newcartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");

        }
        public IActionResult Delete(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.Single(x => x.ProductId == productId);
                cart.Remove(cartItem);
            
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            return View(GetCart());
        }
        public List<CartItem> GetCart()
        {
            List<CartItem> Cart;
            if (HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") == null)
            {
                Cart = new List<CartItem>();
            }
            else
            {
                Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            }
            return Cart;
        }
        public IActionResult Plus(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.Single(x => x.ProductId == productId);
            cartItem.Quantity++;
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
        public IActionResult Min(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.Single(x => x.ProductId == productId);
            cartItem.Quantity--;
            if(cartItem.Quantity <= 0)
            {
                cart.Remove(cartItem);
            }
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Buy()
        {
            var cart = GetCart();
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            User CurrentUser = _context.Users.Find(Int32.Parse(userId));
            decimal CartTotal = 0;
            foreach (var item in cart)
            {
                CartTotal += (item.Product.Price * item.Quantity);
            }
            Order newOrder = new Order
            {
                User = CurrentUser,
                OrderPrice = CartTotal
            };
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in cart)
            {
                Product product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
                _context.Add(new OrderDetail
                {
                    Order = newOrder,
                    Quantity = item.Quantity,
                    Product = product,
                    SubTotal = product.Price * item.Quantity
                });
            }
            _context.Add(newOrder);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}