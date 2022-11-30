using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations
{
    public class MarketPlaceConfiguration : IEntityTypeConfiguration<MarketPlace>
    {
        public void Configure(EntityTypeBuilder<MarketPlace> builder)
        {
            builder.HasOne(p => p.Team)
                .WithMany(p => p.MarketPlaces)
                .HasForeignKey(p => p.TeamId);
        }
    }
}