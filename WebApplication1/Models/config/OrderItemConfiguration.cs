using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.WebApplication1.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Models.config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {

        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(i => i.ItemOrdered, itemOrdered =>
            {
                itemOrdered.WithOwner();

                itemOrdered.Property(CatalogItemId => CatalogItemId.ProductName)
                    .HasMaxLength(50)
                    .IsRequired();
                
            });

            builder.Property(orderItem => orderItem.UnitPrice)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();
        }
    }
}
