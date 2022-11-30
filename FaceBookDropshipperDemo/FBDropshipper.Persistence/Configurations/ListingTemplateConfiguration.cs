using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class ListingTemplateConfiguration : IEntityTypeConfiguration<ListingTemplate>
{
    public void Configure(EntityTypeBuilder<ListingTemplate> builder)
    {
        builder.HasOne(p => p.MarketPlace)
            .WithMany(p => p.ListingTemplates)
            .HasForeignKey(p => p.MarketPlaceId);
    }
}