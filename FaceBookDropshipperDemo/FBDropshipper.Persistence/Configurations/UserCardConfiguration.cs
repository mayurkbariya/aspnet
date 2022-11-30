using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations
{
    public class UserCardConfiguration : IEntityTypeConfiguration<UserCard>
    {
        public void Configure(EntityTypeBuilder<UserCard> builder)
        {
            builder.HasKey(p => p.UserId);
            builder.HasOne(p => p.User)
                .WithOne(p => p.UserCard)
                .HasForeignKey<UserCard>(p => p.UserId);
        }
    }
}