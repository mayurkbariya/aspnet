using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder.HasOne(p => p.MarketPlace)
            .WithMany(p => p.Catalogs)
            .HasForeignKey(p => p.MarketPlaceId);
        builder.HasOne(p => p.User)
            .WithMany(p => p.Catalogs)
            .HasForeignKey(p => p.UserId);

    }
}