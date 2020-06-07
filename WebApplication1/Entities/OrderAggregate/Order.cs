using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using System.Collections;

namespace Microsoft.WebApplication1.Entities.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        private Order()
        {
            // required by EF
        }
        public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
        {
            //validations 
            Guard.Against.NullOrEmpty(buyerId, nameof(buyerId));
            Guard.Against.Null(shipToAddress, nameof(shipToAddress));
            Guard.Against.Null(items, nameof(items));

            this.buyerId = buyerId;
            this.Address = shipToAddress;
            this._orderItems = items;

        }
        public string buyerId { get; private set; }
        public DateTimeOffset orderDate { get; private set; } = DateTimeOffset.Now;
        public Address Address { get; private set; }
        public List<OrderItem> _orderItems = new List<OrderItem>();

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    


        public decimal Total()
        {
            var total = 0m;
            foreach(var item in _orderItems)
            {
                total += item.UnitPrice * item.Units;
            }
            return total;
        }
    }
}
