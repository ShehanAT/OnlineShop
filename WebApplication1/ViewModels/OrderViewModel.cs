using Microsoft.WebApplication1.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.ViewModels
{
    
    public class OrderViewModel
    {
        private const string DEFAULT_STATUS = "Pending";
        public int OrderNumber { get; set; }
        public string buyerId { get; set; }
        public decimal Total { get; set; }
        public string Status => DEFAULT_STATUS;
        public Address ShippingAddress { get; set;}
        public DateTimeOffset OrderDate { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }  = new List<OrderItemViewModel>();
    }
}
