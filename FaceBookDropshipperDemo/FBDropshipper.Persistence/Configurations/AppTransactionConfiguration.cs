using FBDropshipper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FBDropshipper.Persistence.Configurations
{
    public class AppTransactionConfiguration : IEntityTypeConfiguration<AppTransaction>
    {
        public void Configure(EntityTypeBuilder<AppTransaction> builder)
        {
            builder.HasOne(p => p.User)
                .WithMany(p => p.AppTransactions)
                .HasForeignKey(p => p.UserId);
        }
    }
}