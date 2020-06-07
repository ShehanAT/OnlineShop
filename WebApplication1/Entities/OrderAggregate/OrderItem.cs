using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity, IAggregateRoot
    {
        public CatalogItemOrdered ItemOrdered { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Units { get; private set; }
        private OrderItem()
        {
            // this private constructor is required by entityframework
        }
        public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
        {
            this.ItemOrdered = itemOrdered;
            this.UnitPrice = unitPrice;
            this.Units = units;
        }
    }
}
