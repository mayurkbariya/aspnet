using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class InventoryProductImageConfiguration : IEntityTypeConfiguration<InventoryProductImage>
{
    public void Configure(EntityTypeBuilder<InventoryProductImage> builder)
    {
        builder.HasOne(p => p.InventoryProduct)
            .WithMany(p => p.InventoryProductImages)
            .HasForeignKey(p => p.InventoryProductId);
    }
}