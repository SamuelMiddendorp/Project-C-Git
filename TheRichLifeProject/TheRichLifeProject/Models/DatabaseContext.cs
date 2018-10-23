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
        public DbSet<Atribute> Atributes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Value> Values { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
//Welp, as you can see branching works :D...