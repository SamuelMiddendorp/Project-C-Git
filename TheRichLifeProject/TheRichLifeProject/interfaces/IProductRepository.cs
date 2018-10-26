using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }


        Product GetProductById(int productId);
    }
}
