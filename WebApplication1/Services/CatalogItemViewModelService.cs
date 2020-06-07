using Microsoft.WebApplication1.Entities;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Services
{
    public class CatalogItemViewModelService : ICatalogItemViewModelService
    {
        private readonly IAsyncRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemViewModelService(IAsyncRepository<CatalogItem> catalogItemRepository)
        {
            _catalogItemRepository = catalogItemRepository;
        }

        public async Task UpdateCatalogItem(CatalogItemViewModel viewModel)
        {
            var existingCatalogItem = await _catalogItemRepository.GetByIdAsync(viewModel.id);
            existingCatalogItem.Update(viewModel.name, viewModel.price); // only name and price of catalogItem can be updated 
            await _catalogItemRepository.UpdateAsync(existingCatalogItem);
        }
    }
}
