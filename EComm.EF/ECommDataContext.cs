﻿using EComm.Data.Entities;
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

        public async Task<IEnumerable<Product>> GetAllProducts(bool includeSupplier = false)
        {
            return includeSupplier switch
            {
                true => await Products.Include(p => p.Supplier).ToListAsync(),
                false => await Products.ToListAsync()
            };
        }

        public async Task<Product> GetProduct(int id, bool includeSupplier = false)
        {
            return includeSupplier switch
            {
                true => await Products
                    .Include(p => p.Supplier)
                    .SingleOrDefaultAsync(p => p.Id == id),
                false => await Products.FindAsync(id)
            };
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            Products.Update(product);
            Entry(product).State = EntityState.Modified;
            await SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetProductRaw(int id)
            => await Products
                .FromSqlRaw("select * from Products where Id = {0}", id)    
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Supplier>> GetAllSuppliers()
            => await Suppliers.ToListAsync();
        
    }
}
