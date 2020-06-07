using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.WebApplication1.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Models.config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.OwnsOne(o => o.Address, a =>
            { // o is a Order type, a is a Address type
                a.WithOwner(); // sets o as owner of a 

                a.Property(a => a.Street)
                    .HasMaxLength(180)
                    .IsRequired();
                a.Property(a => a.State)
                    .HasMaxLength(60)
                    .IsRequired();
                a.Property(a => a.City)
                    .HasMaxLength(100)
                    .IsRequired();
                a.Property(a => a.Country)
                    .HasMaxLength(100)
                    .IsRequired();
                a.Property(a => a.ZipCode)
                    .HasMaxLength(20)
                    .IsRequired();
            });


        }
        
    }
}
