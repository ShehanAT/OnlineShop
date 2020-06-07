using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.WebApplication1.Entities.BasketAggregate;
using Microsoft.WebApplication1.Entities;
using Microsoft.WebApplication1.Entities.OrderAggregate;
using System.Reflection;

namespace Microsoft.WebApplication1.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItem { get; set; }
        public DbSet<CatalogBrand> CatalogBrand { get; set; }
        public DbSet<CatalogItem> CatalogItem { get; set; }
        public DbSet<CatalogType> CatalogType { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}
