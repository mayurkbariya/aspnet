using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class ProductListingImageConfiguration : IEntityTypeConfiguration<ProductListingImage>
{
    public void Configure(EntityTypeBuilder<ProductListingImage> builder)
    {
        builder.HasOne(p => p.ProductListing)
            .WithMany(p => p.ProductListingImages)
            .HasForeignKey(p => p.ProductListingId);
    }
}