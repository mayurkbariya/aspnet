using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class CatalogProductImageConfiguration : IEntityTypeConfiguration<CatalogProductImage>
{
    public void Configure(EntityTypeBuilder<CatalogProductImage> builder)
    {
        builder.HasOne(p => p.CatalogProduct)
            .WithMany(p => p.CatalogProductImages)
            .HasForeignKey(p => p.CatalogProductId);
    }
}