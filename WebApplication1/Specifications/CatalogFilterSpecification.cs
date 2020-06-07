using Microsoft.WebApplication1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Specifications
{
    public class CatalogFilterSpecification : BaseSpecification<CatalogItem>
    {

        public CatalogFilterSpecification(int? brandId, int? typeId)
            : base(i => (!brandId.HasValue || i.catalogBrandId == brandId) && 
            (!typeId.HasValue || i.catalogTypeId == typeId)) // i is of CatalogItem type
            {

            }
    }
}
