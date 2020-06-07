using Microsoft.AspNetCore.Mvc;
using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Controllers.api
{
    public class CatalogApiController : BaseApiController
    {
        private readonly ICatalogViewModelService _catalogViewModelService;

        public CatalogApiController(ICatalogViewModelService catalogViewModelService)
        {
            _catalogViewModelService = catalogViewModelService;
        }

        [HttpGet]
        public async Task<IActionResult> List(int? brandFilterApplied, int? typesFilterApplied, int? page)
        {
            var itemsPerPage = 10;
            var catalogModel = await _catalogViewModelService.GetCatalogItems(page ?? 0, itemsPerPage, brandFilterApplied, typesFilterApplied);
                // (page ?? 0): if page var not provided replace with 0
            return Ok(catalogModel);
        }
    }
}
