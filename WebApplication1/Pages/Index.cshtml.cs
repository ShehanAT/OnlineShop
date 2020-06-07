using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.ViewModels;
using Microsoft.WebApplication1.Constants;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Microsoft.WebApplication1.Pages
{
    public class MainIndexModel : PageModel
    {
        private readonly ICatalogViewModelService _catalogViewModelService;

        public MainIndexModel(ICatalogViewModelService catalogViewModelService)
        {
            _catalogViewModelService = catalogViewModelService;
        }


        public CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();
        [HttpGet]
        public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId)
        {
            CatalogModel = await _catalogViewModelService.GetCatalogItems(pageId ?? 0, ApplicationConstants.ITEMS_PER_PAGE, catalogModel.brandFilterApplied, catalogModel.typeFilterApplied);
        }
    }
}
