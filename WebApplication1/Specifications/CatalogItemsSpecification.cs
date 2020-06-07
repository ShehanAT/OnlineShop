using Microsoft.WebApplication1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Specifications
{
    public class CatalogItemsSpecification : BaseSpecification<CatalogItem>
    {
        // c refers to CatalogItem instance
        public CatalogItemsSpecification(params int[] ids) : base(c => ids.Contains(c.id))
        {

        }
    }
}
