using Ardalis.GuardClauses;
using Microsoft.WebApplication1.Entities.BasketAggregate;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Services
{
    public class BasketService : IBasketService 
    {
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAppLogger<BasketService> _logger;

        public BasketService(IAsyncRepository<Basket> basketRepository, IAppLogger<BasketService> logger)
        {
            this._basketRepository = basketRepository;
            this._logger = logger;
            
        }

        public async Task  AddItemToBasket(int basketId, int catalogItemId, decimal price, int quantity = 1)
        { // quantity param is set to 1 if not provided by caller
            var basket = await _basketRepository.GetByIdAsync(basketId);
            basket.AddItem(catalogItemId, price, quantity);

            await _basketRepository.UpdateAsync(basket);
        }

        public async Task DeleteBasketAsync(int basketId)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId);
            await _basketRepository.DeleteAsync(basket);
        }

        public async Task<int> GetBasketItemCountAsync(string userName)
        {
            Guard.Against.NullOrEmpty(userName, nameof(userName));
            var basketSpec = new BasketWithItemsSpecification(userName);
            var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
            if(basket == null)
            {
                _logger.LogInformation($"No basket found for ${userName}");
            }
            int count = basket.Items.Sum(i => i.Quantity); // get the sum of all basketItems(via Quantity attribute)
            return count;
        }

        public async Task SetQuantities(int basketId, Dictionary<string, int> quantities)
        {
            Guard.Against.Null(quantities, nameof(quantities));
            var basket = await _basketRepository.GetByIdAsync(basketId);
            Guard.Against.NullBasket(basketId, basket);
            foreach(var item in basket.Items)
            {
                // TryGetValue(<key>, out <value>);
                if(quantities.TryGetValue(item.CatalogItemId.ToString(), out var quantity)) // automatically adds new key value pair on false case
                {
                    if (_logger != null) _logger.LogInformation($"Updating quantity of item ID: {item.id} to {item.Quantity}");
                    item.SetNewQuantity(quantity);
                }
            }
            basket.RemoveEmptyItems();
            await _basketRepository.UpdateAsync(basket); // add new basketItem if current quantity is 0
        }

        public async Task TransferBasketAsync(string anonymousId, string userName)
        {
            Guard.Against.NullOrEmpty(anonymousId, nameof(anonymousId));
            Guard.Against.NullOrEmpty(userName, nameof(userName));
            var basketSpec = new BasketWithItemsSpecification(anonymousId);
            var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
            if(basket == null)
            {
                _logger.LogInformation($"Unable to transfer basket because basket for username: {userName} was not found.");
                return;
            }
            basket.SetNewBuyerId(anonymousId); // changing buyer id of basket
            await _basketRepository.UpdateAsync(basket);
        }
    }
}
