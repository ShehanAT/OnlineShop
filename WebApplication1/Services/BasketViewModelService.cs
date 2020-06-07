using Microsoft.WebApplication1.Entities;
using Microsoft.WebApplication1.Entities.BasketAggregate;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Pages.Basket;
using Microsoft.WebApplication1.Specifications;
using Microsoft.WebApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;
       /* private CatalogItemViewModel catalogItemModel { get; set; } = new CatalogItemViewModel();*/
        public BasketViewModelService(IAsyncRepository<Basket> basketRepository, IUriComposer uriComposer, IAsyncRepository<CatalogItem> itemRepository)
        { // IAsyncRepository items do not have to be instantiated to be used 
            this._basketRepository = basketRepository;
            this._itemRepository = itemRepository;
            this._uriComposer = uriComposer;
        }
        public async Task<BasketViewModel> GetOrCreateBasketForUser(string username)
        {   // fetchs basket corresponding to username then passes basket to CreateViewModelFromBasket(0
            var basketSpec = new BasketWithItemsSpecification(username);
            // basketRepo contains baskets not basketItems
            var basket = (await _basketRepository.FirstOrDefaultAsync(basketSpec));
            if (basket == null)
            {
                return (await CreateBasketForUser(username));
            }
       /*    S
            catalogItemModel = await _itemRepository.ListAsync()*/
            return await CreateViewModelFromBasket(basket);
        }

        public async Task<BasketViewModel> CreateBasketForUser(string userId)
        {
            var basket = new Basket(userId);
            await _basketRepository.AddAsync(basket);

            return new BasketViewModel()
            {
                Id = basket.id,
                BuyerId = basket.BuyerId
            };
        }

        public async Task<BasketViewModel> CreateViewModelFromBasket(Basket basket)
        {   // creating corresponding basket view model for basket and returning it
            var viewModel = new BasketViewModel
            {
                Id = basket.id,
                BuyerId = basket.BuyerId,
                Items = await GetBasketItems(basket.Items)
            };
            return viewModel;
         }


        public async Task<List<BasketItemViewModel>> GetBasketItems(IReadOnlyCollection<BasketItem> basketItems)
        {
            var catalogItemsSpecification = new CatalogItemsSpecification(basketItems.Select(b => b.CatalogItemId).ToArray());
            var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);
            var items = basketItems.Select(basketItem =>
            {
              
               var catalogItem = catalogItems.FirstOrDefault(c => c.id == basketItem.CatalogItemId);
               
                //if basketItem.CatalogItemId == catalogItem.id make a basketViewModel out of both and return it

                var basketItemViewModel = new BasketItemViewModel
                {
                    Id = basketItem.id,
                    CatalogItemId = basketItem.CatalogItemId,
                    ProductName = catalogItem.Name,
                    Quantity = basketItem.Quantity,
                    UnitPrice = basketItem.UnitPrice,
                    PictureUri = _uriComposer.ComposePictureUri(catalogItem.PictureUri)
                };
                return basketItemViewModel;
            }).ToList();

            return items;
        }
    }
}
