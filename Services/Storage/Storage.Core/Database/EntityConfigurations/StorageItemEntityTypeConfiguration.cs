using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Storage.Core.Database.Entities;
using Storage.Core.Enums;

namespace Storage.Core.Database.EntityConfigurations
{
    public class StorageItemEntityTypeConfiguration : IEntityTypeConfiguration<StorageItem>
    {
        public void Configure(EntityTypeBuilder<StorageItem> builder)
        {
            builder.Property(e => e.Category)
                .HasConversion(e => e.ToString(), e => Enum.Parse<FileCategory>(e))
                .HasMaxLength(250);
        }
    }
}
