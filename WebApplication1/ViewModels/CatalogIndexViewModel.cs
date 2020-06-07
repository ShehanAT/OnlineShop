using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.ViewModels
{
    public class CatalogIndexViewModel
    {
        public List<CatalogItemViewModel> catalogItems { get; set; }
        public List<SelectListItem> brands { get; set; } /* list of <option> tags containing the brand names*/
        public List<SelectListItem> types { get; set; } /* list of <option> tags containing the type names*/
        public int? brandFilterApplied { get; set; }
        public int? typeFilterApplied { get; set; }
        public PaginationInfoViewModel paginationInfo { get; set; }

        public override string ToString()
        {
            return "PaginationInforViewModel: " + paginationInfo;
        }
    }
}
