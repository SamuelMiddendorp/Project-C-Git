using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public string ProductName { get; set; }

        public string ImageSrc { get; set; }

        [DisplayFormat(DataFormatString = "0:€ ", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [Required]
        public bool Mature{ get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string SubCategory { get; set; }

        public virtual List<Value> Values { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
