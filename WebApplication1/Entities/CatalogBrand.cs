using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WebApplication1.Interfaces;
namespace Microsoft.WebApplication1.Entities
{
    public class CatalogBrand : BaseEntity, IAggregateRoot
    {
        public string brand { get; private set; }
        public CatalogBrand(string brand)
        {
            this.brand = brand;
        }
    }
}
