using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class CatalogProductConfiguration : IEntityTypeConfiguration<CatalogProduct>
{
    public void Configure(EntityTypeBuilder<CatalogProduct> builder)
    {
        builder.HasOne(p => p.Catalog)
            .WithMany(p => p.CatalogProducts)
            .HasForeignKey(p => p.CatalogId);
    }
}