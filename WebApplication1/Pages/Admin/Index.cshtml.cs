using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.WebApplication1.Constants;
using Microsoft.WebApplication1.Extentions;
using Microsoft.WebApplication1.Services;
using Microsoft.WebApplication1.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.WebApplication1.Interfaces;

namespace Microsoft.WebApplication1.Page.Admin
{
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    public class AdminIndexModel : PageModel
    {
        private readonly ICatalogViewModelService _catalogViewModelService;
        private readonly IMemoryCache _cache;

        public AdminIndexModel(ICatalogViewModelService catalogViewModelService, IMemoryCache cache)
        {
            _catalogViewModelService = catalogViewModelService;
            _cache = cache;
        }

        public CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

        public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId)
        {
            var cacheKey = CacheHelpers.GenerateCatalogItemCacheKey(pageId.GetValueOrDefault(), ApplicationConstants.ITEMS_PER_PAGE, catalogModel.brandFilterApplied, catalogModel.typeFilterApplied);
            _cache.Remove(cacheKey);
            CatalogModel = await _catalogViewModelService.GetCatalogItems(pageId.GetValueOrDefault(), ApplicationConstants.ITEMS_PER_PAGE, catalogModel.brandFilterApplied, catalogModel.typeFilterApplied);
        }
    }
}