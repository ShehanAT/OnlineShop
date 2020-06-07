using Ardalis.GuardClauses;
using Microsoft.WebApplication1.Entities;
using Microsoft.WebApplication1.Entities.BasketAggregate;
using Microsoft.WebApplication1.Entities.OrderAggregate;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;

        public OrderService(IAsyncRepository<Order> orderRepository,
            IUriComposer uriComposer, 
            IAsyncRepository<Basket> basketRepository,
            IAsyncRepository<CatalogItem> itemRepository)
        {
            this._orderRepository = orderRepository;
            this._uriComposer = uriComposer;
            this._basketRepository = basketRepository;
            this._itemRepository = itemRepository;
        }

        public async Task CreateOrderAsync(int basketId, Address shippingAddress)
        {
            var basketSpec = new BasketWithItemsSpecification(basketId);
            var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

            Guard.Against.NullBasket(basket.id, basket);

            // make an array of catalogItem ids in all basketItem objects and send to CatalogItemsSpecification()
            var catalogItemsSpecification = new CatalogItemsSpecification(basket.Items.Select(item => item.CatalogItemId).ToArray());
            var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification); // list of catalogItems with ids in basket items
            var items = basket.Items.Select(basketItem =>
            {
                // if basketItem's catalog item id matches repo catalogItem id set var 
                var catalogItem = catalogItems.First(c => c.id == basketItem.CatalogItemId);
                var itemOrdered = new CatalogItemOrdered(catalogItem.id, catalogItem.Name, catalogItem.PictureUri);
                // make new orderItem by using selected basketItem and catalogItem
                var orderItem = new OrderItem(itemOrdered, catalogItem.Price, basketItem.Quantity);
                return orderItem; // return the new orderItem and append to items list
            }).ToList();

            var order = new Order(basket.BuyerId, shippingAddress, items);
            await _orderRepository.AddAsync(order); // add the new Order obj to orderRepository
        }

    }
}
