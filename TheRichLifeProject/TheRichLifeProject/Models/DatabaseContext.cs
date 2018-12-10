using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Value> Values { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
    }
}