using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class ProductListingConfiguration : IEntityTypeConfiguration<ProductListing>
{
    public void Configure(EntityTypeBuilder<ProductListing> builder)
    {
        builder.HasOne(p => p.Category)
            .WithMany(p => p.ProductLists)
            .HasForeignKey(p => p.CategoryId);
        builder.HasOne(p => p.MarketPlace)
            .WithMany(p => p.ProductLists)
            .HasForeignKey(p => p.MarketPlaceId);
        builder.HasOne(p => p.InventoryProduct)
            .WithMany(p => p.ProductLists)
            .HasForeignKey(p => p.InventoryProductId);
        builder.HasOne(p => p.ListingTemplate)
            .WithMany(p => p.ProductLists)
            .HasForeignKey(p => p.ListingTemplateId);
        
    }
}