using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(p => p.MarketPlace)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.MarketPlaceId);
        builder.HasOne(p => p.ProductListing)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.ProductListingId);
    }
}