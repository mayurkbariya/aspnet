using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations
{
    public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.HasOne(p => p.User)
                .WithMany(p => p.UserSubscriptions)
                .HasForeignKey(p => p.UserId);
            builder.HasOne(p => p.Subscription)
                .WithMany(p => p.UserSubscriptions)
                .HasForeignKey(p => p.SubscriptionId);

        }
    }
}