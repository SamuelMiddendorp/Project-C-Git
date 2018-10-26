using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRichLifeProject.interfaces;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Repositories
{
    public class ProductRepository :IProductRepository
    {
        private readonly DatabaseContext _DbContext;

        public ProductRepository(DatabaseContext DbContext)
        {
            _DbContext = DbContext;
        }

        public IEnumerable<Product> Products => _DbContext.Products;

        public Product GetProductById(int productId) => _DbContext.Products.FirstOrDefault(p => p.Id == productId);
    }
}
