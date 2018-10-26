using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class WishListItem
    {
        public int WishListItemId { get; set; }

        public Product Product { get; set; }

        public string WishListId { get; set; }
    }
}
