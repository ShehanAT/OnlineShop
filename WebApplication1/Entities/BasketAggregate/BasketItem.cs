using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Entities.BasketAggregate
{
    public class BasketItem : BaseEntity, IAggregateRoot
    {
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set;}
        public int CatalogItemId { get; private set; }
        public int BasketId { get; private set; }

        public BasketItem(int catalogItemId, int quantity, decimal unitPrice)
        {
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
            this.CatalogItemId = catalogItemId;
        }

        public void AddQuantity(int quantity)
        {
            this.Quantity += quantity;
        }

        public void SetNewQuantity(int quantity)
        {
            this.Quantity = quantity;
        }
    }
}
