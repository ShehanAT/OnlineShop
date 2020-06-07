using Ardalis.GuardClauses;
using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Entities.OrderAggregate
{
    // Represents a snapshot of the item that was ordered. If catalog item details change, details of 
    // the item that was part of a completed order should not change.
    public class CatalogItemOrdered : BaseEntity, IAggregateRoot
    {
        public int CatalogItemId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureURI { get; private set; }
        public CatalogItemOrdered(int catalogItemId, string productName, string pictureURI)
        {
            Guard.Against.OutOfRange(catalogItemId, nameof(catalogItemId), 1, int.MaxValue); //make sure catalogItemId is > 1 and < int.MaxValue
            Guard.Against.NullOrEmpty(productName, nameof(productName));
            Guard.Against.NullOrEmpty(pictureURI, nameof(pictureURI));

            this.CatalogItemId = catalogItemId;
            this.ProductName = productName;
            this.PictureURI = pictureURI;
        }

    }
}
