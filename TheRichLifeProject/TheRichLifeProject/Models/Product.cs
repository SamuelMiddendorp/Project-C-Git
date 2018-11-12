using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ProductName { get; set; }
        public string ImageSrc { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Mature{ get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public virtual List<Value> Values { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
