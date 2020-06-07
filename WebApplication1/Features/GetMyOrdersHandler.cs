using MediatR;
using Microsoft.WebApplication1.Data;
using Microsoft.WebApplication1.Entities.OrderAggregate;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Specifications;
using Microsoft.WebApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Features
{
    public class GetMyOrdersHandler : IRequestHandler<GetMyOrders, IEnumerable<OrderViewModel>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetMyOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<OrderViewModel>> Handle(GetMyOrders request, CancellationToken cancelToken)
        {
            var specification = new CustomerOrdersWithItemsSpecification(request.UserName);
            var orders = await _orderRepository.ListAsync(specification);

         

            return orders.Select(o => new OrderViewModel
            {
                OrderNumber = o.id,
                OrderDate = o.orderDate,
                OrderItems = o.OrderItems?.Select(oi => new OrderItemViewModel()
                {   // productId is catalogItemId
                    Id = oi.ItemOrdered.CatalogItemId,
                    ProductName = oi.ItemOrdered.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Units = oi.Units,
                    PictureUrl = oi.ItemOrdered.PictureURI
                }).ToList(),
                ShippingAddress = o.Address,
                Total = o.Total()
            });
        }
    }
}
