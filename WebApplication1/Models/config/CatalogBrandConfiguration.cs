using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WebApplication1.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.WebApplication1.Models.config
{
    public class CatalogBrandConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> builder)
        {
            //ci is a CatalogBrand type object
            builder.HasKey(ci => ci.id);

            builder.Property(ci => ci.id)
                .UseHiLo("catalog_brand_hilo")
                .IsRequired();

            builder.Property(cb => cb.brand)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
