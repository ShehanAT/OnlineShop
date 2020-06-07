using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Discount => 0; // shorthand for OrderItemViewModel.Discount(get)
        

    }
}
