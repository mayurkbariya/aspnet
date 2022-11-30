using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.HasKey(p => new { p.TeamId, p.UserId });
            builder.HasOne(p => p.Team)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(p => p.TeamId);
            builder.HasOne(p => p.User)
                .WithOne(p => p.TeamMember)
                .HasForeignKey<TeamMember>(p => p.UserId);

        }
    }
}