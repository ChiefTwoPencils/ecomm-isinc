using EComm.Data.Entities;
using EComm.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EComm.EF
{
    public class ECommDataContext : DbContext, IRepository
    {
        public ECommDataContext(DbContextOptions options)
        : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public async Task<IEnumerable<Product>> GetAllProducts()
            => await Products.ToListAsync();

        public async Task<Product> GetProduct(int id)
            => await Products.FindAsync(id);

        public async Task<Product> GetProductRaw(int id)
            => await Products
                .FromSqlRaw("select * from Products where Id = {0}", id)    
                .SingleOrDefaultAsync();
    }
}
