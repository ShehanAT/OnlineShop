using Microsoft.EntityFrameworkCore;
using Microsoft.WebApplication1.Entities.OrderAggregate;
using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Data
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(CatalogContext dbContext) : base(dbContext)
        { // ```: base(dbContext) ``` means the fields of EfRepository are accessible to OrderRepository
            // private fields cannot be accessed however
            // base keyword requires the constructor args of the base class to be passed in the devired class
        }

        public Task<Order> GetByIdWithItemsAsync(int userId)
        {
            return  _dbContext
                .Orders
                .Include(o => o.OrderItems) // adding order items to query 
                .Include($"{nameof(Order.OrderItems)}.{nameof(OrderItem.ItemOrdered)}")
                .FirstOrDefaultAsync(x => x.id == userId); 
        }
    }
}
