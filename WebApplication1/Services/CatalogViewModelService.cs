using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.WebApplication1.Entities;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Specifications;
using Microsoft.WebApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Services
{
    public class CatalogViewModelService : ICatalogViewModelService
    {
        private readonly ILogger<CatalogViewModelService> _logger;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;
        private readonly IAsyncRepository<CatalogBrand> _brandRepository;
        private readonly IAsyncRepository<CatalogType> _typeRepository;
        private readonly IUriComposer _uriComposer;

        public CatalogViewModelService(
            ILoggerFactory loggerFactory,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<CatalogBrand> brandRepository,
            IAsyncRepository<CatalogType> typeRepository,
            IUriComposer uriComposer
            )
        {
            this._logger = loggerFactory.CreateLogger<CatalogViewModelService>();
            this._itemRepository = itemRepository;
            this._brandRepository = brandRepository;
            this._typeRepository = typeRepository;
            this._uriComposer = uriComposer;
        }

        public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
        { // pageIndex is current page
            _logger.LogInformation("GetCatalogItems called");

            var filterSpecification = new CatalogFilterSpecification(brandId, typeId); // this contains all catalogitems with matching brand&type ids
            var filterPaginatedSpecifition = new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId); // this contains all catalogItem per page 
            var itemsPerPage = await _itemRepository.ListAsync(filterPaginatedSpecifition);
            var totalItems = await _itemRepository.CountAsync(filterSpecification);
            var vm = new CatalogIndexViewModel()
            {
                catalogItems = itemsPerPage.Select(i => new CatalogItemViewModel()
                {
                    id = i.id,
                    name = i.Name,
                    pictureUri = _uriComposer.ComposePictureUri(i.PictureUri),
                    price = i.Price
                }).ToList(),
                //for List<SelectListItem> brands
                brands = (List<SelectListItem>)await GetBrands(), // this is an explicit cast
                types = (List<SelectListItem>)await GetTypes(),
                brandFilterApplied = brandId ?? 0, // if brandId not null pass it, else pass 0
                typeFilterApplied = typeId ?? 0,

                paginationInfo = new PaginationInfoViewModel()
                {
                    totalItems = totalItems,
                    itemsPerPage = itemsPerPage.Count,
                    actualPage = pageIndex,
                    totalPages = int.Parse(Math.Ceiling((decimal)totalItems / itemsPerPage.Count).ToString())
                    // string next and previous are defined seperately below
                }
            };
            // "is-disabled" will be render into HTML that disables next/previous button
            vm.paginationInfo.next = (vm.paginationInfo.actualPage == vm.paginationInfo.totalPages - 1)  ?  "is-disabled" : "";
            vm.paginationInfo.previous = (vm.paginationInfo.actualPage == 0) ? "is-disabled" : "";

            return vm;
        }
        
        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {//since Enumerable is a non-generic type IEnumerable must be used as return type
            _logger.LogInformation("GetBrands() called");
            var brands = await _brandRepository.ListAllAsync();
            //create corresponding SelectListItem for every brandItem, append to list and return it 
            var items = brands
                .Select(i => new SelectListItem(i.brand, i.id.ToString()))
                .OrderBy(i => i.Text)
                .ToList();

            //add allItem SelectListView used for showing all brands in view, selected by default
            var allItem = new SelectListItem("ALL", "", true);
            items.Insert(0, allItem); // insert allItem to first position of items list 
            return items;

        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            _logger.LogInformation("GetBrands() called");
            var brands = await _typeRepository.ListAllAsync();
            //create corresponding SelectListItem for every catalogTypeItem, append to list and return it 
            var items = brands
                .Select(i => new SelectListItem(i.Type, i.id.ToString()))
                .OrderBy(i => i.Text)
                .ToList();

            //add allItem SelectListView used for showing all brands in view 
            var allItem = new SelectListItem("ALL", "", true);
            items.Insert(0, allItem); // insert allItem to first position of items list 
            return items;

        }
    }
}
