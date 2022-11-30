using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class InventoryProductConfiguration : IEntityTypeConfiguration<InventoryProduct>
{
    public void Configure(EntityTypeBuilder<InventoryProduct> builder)
    {
        builder.HasOne(p => p.CatalogProduct)
            .WithMany(p => p.InventoryProducts)
            .HasForeignKey(p => p.CatalogProductId);
        builder.HasOne(p => p.MarketPlace)
            .WithMany(p => p.InventoryProducts)
            .HasForeignKey(p => p.MarketPlaceId);
    }
}