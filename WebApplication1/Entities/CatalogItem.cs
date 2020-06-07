using Ardalis.GuardClauses;
using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Entities
{
    public class CatalogItem : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        //uri can be a name, locator or both whereas url is only a locator
        public string PictureUri { get; private set; }
        public int catalogTypeId { get; private set; }
        public CatalogType catalogType { get; private set; }
        public int catalogBrandId { get; private set; }
        public CatalogBrand catalogBrand { get; private set; }
        public CatalogItem(int catalogTypeId, int catalogBrandId, string description, string name, decimal price, string pictureUri)
        {
            this.catalogTypeId = catalogTypeId;
            this.catalogBrandId = catalogBrandId;
            this.Description = description;
            this.Name = name;
            this.Price = price;
            this.PictureUri = pictureUri;
        }

        public void Update(string name, decimal price)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Name = name;
            Price = price;
        }
    }
}
