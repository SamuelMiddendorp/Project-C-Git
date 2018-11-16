using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class CategoryViewModel
    {
        public List<Product> Products { get; set; }
        public List<string> Categories { get; set; }
    }
}
