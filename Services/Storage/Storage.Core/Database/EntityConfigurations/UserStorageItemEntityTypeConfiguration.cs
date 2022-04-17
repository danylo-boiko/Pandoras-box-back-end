using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Storage.Core.Database.Entities;

namespace Storage.Core.Database.EntityConfigurations
{
    public class UserStorageItemEntityTypeConfiguration : IEntityTypeConfiguration<UserStorageItem>
    {
        public void Configure(EntityTypeBuilder<UserStorageItem> builder)
        {
            builder.HasOne(e => e.StorageItem)
                .WithMany()
                .HasForeignKey(e => e.StorageItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(e => new { e.UserId, e.StorageItemId });
        }
    }
}
