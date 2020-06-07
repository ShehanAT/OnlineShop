using Microsoft.WebApplication1.Entities.OrderAggregate;
using Microsoft.WebApplication1.Helpers.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Specifications
{
    public class CustomerOrdersWithItemsSpecification : BaseSpecification<Order>
    {
        public CustomerOrdersWithItemsSpecification(string buyerId) : base(o => o.buyerId == buyerId)
        {
            AddIncludes(query => query.Include(o => o.OrderItems).ThenInclude(i => i.ItemOrdered));
        }
    }
}
