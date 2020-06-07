using Microsoft.WebApplication1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WebApplication1.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microsoft.WebApplication1.Interfaces
{
    public interface ICatalogViewModelService
    {
        public Task<CatalogIndexViewModel> GetCatalogItems(int page, int itemsPerPage, int? brandId, int? typeId);
        public Task<IEnumerable<SelectListItem>> GetBrands(); /* get the <option>brands</option> of each CatalogIndexViewModel object*/
        public Task<IEnumerable<SelectListItem>> GetTypes();/* get the <option>types</option> of each CatalogIndexViewModel object*/
    }
}
