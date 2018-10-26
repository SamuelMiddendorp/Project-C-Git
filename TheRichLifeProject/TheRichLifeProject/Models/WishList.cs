using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class WishList
    {
        private readonly DatabaseContext _databaseContext;

        public WishList(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public static WishList GetList(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<DatabaseContext>();
            string listId = session.GetString("ListId") ?? Guid.NewGuid().ToString();

            session.SetString("ListId", listId);

            return new WishList(context) { WishListId = listId };
        }

        public void AddToList(Product product)
        {
            var wishListItem = _databaseContext.WishListItems.SingleOrDefault(
                s => s.Product.Id == product.Id && s.WishListId == WishListId);

            if (wishListItem == null)
            {
                wishListItem = new WishListItem
                {
                    WishListId = WishListId,
                    Product = product,
                    
                };

                _databaseContext.WishListItems.Add(wishListItem);
            }
            
            _databaseContext.SaveChanges();
        }

        public void RemoveFromList(Product product)
        {
            var wishListItem =
                _databaseContext.WishListItems.SingleOrDefault(
                    s => s.Product.Id == product.Id && s.WishListId == WishListId);

            

            if (wishListItem != null)
            {
                
                 _databaseContext.WishListItems.Remove(wishListItem);
              
            }

            _databaseContext.SaveChanges();

            
        }

        public List<WishListItem> GetWishListItems()
        {
            return WishListItems ??
                (WishListItems =
                _databaseContext.WishListItems.Where(c => c.WishListId == WishListId)
                .Include(s => s.Product).ToList()
                );
        }

        public void ClearList()
        {
            var listItems = _databaseContext
                .WishListItems.Where(cart => cart.WishListId == WishListId);

            _databaseContext.WishListItems.RemoveRange(listItems);

            _databaseContext.SaveChanges();
        }





        public string WishListId { get; set; }

        public List<WishListItem> WishListItems { get; set; }
    }
}
