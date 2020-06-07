using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.WebApplication1.Entities.BasketAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Models.config
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem> 
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.Property(basketItem => basketItem.UnitPrice)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();
        }
    }
}
