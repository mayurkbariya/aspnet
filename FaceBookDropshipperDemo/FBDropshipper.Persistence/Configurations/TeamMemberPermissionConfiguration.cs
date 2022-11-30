using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations;

public class TeamMemberPermissionConfiguration : IEntityTypeConfiguration<TeamMemberPermission>
{
    public void Configure(EntityTypeBuilder<TeamMemberPermission> builder)
    {
        builder.HasKey(p => new { p.UserId, p.MarketPlaceId });
        builder.HasOne(p => p.User)
            .WithMany(p => p.TeamMemberPermissions)
            .HasForeignKey(p => p.UserId);
        builder.HasOne(p => p.MarketPlace)
            .WithMany(p => p.TeamMemberPermissions)
            .HasForeignKey(p => p.MarketPlaceId);
    }
}