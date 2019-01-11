using Microsoft.AspNetCore.Http;
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

        [DataType(DataType.Currency)]
        [Range(0.1, 99999999.99)]
        public decimal Price { get; set; }

        [Range(0, 9999999)]
        public int Stock { get; set; }

        [Required]
        public bool Mature{ get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Subcategory can only contains letters")]
        public string SubCategory { get; set; }

        public virtual List<Value> Values { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
