using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{

    public class CartItem
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
