using Microsoft.WebApplication1.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Interfaces
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        public Task<Order> GetByIdWithItemsAsync(int userId);
    }
}
