using Microsoft.WebApplication1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Specifications
{
    public class CatalogFilterPaginatedSpecification : BaseSpecification<CatalogItem>
    {
        // int skip = items per page * page number, int take = items per page
        public CatalogFilterPaginatedSpecification(int skip, int take, int? brandId, int? typeId)
            : base(i => (!brandId.HasValue || i.catalogBrandId == brandId) && 
            (!typeId.HasValue || i.catalogTypeId == typeId))
        {
            ApplyPaging(skip, take);  // filter catalogItems by matching their catalogBrandIds and catalogTypeIds with arg brandId or typeId  
        }
    }
}
