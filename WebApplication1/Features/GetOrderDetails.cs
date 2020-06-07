using MediatR;
using Microsoft.WebApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Features
{
    public class GetOrderDetails : IRequest<OrderViewModel>
    {
        public string Username { get; set; }
        public int OrderId { get; set; }
        public GetOrderDetails(string username, int orderId)
        {
            this.Username = username;
            this.OrderId = orderId;
        }
    }

}
