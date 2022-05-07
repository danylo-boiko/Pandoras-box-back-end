using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Videos.Core.Database.Entities;

namespace Videos.Core.Database.EntityConfigurations;

public class VideoTagEntityTypeConfiguration : IEntityTypeConfiguration<VideoTag>
{
    public void Configure(EntityTypeBuilder<VideoTag> builder)
    {
        builder.HasOne(vt => vt.Video)
            .WithMany(v => v.VideoTags)
            .HasForeignKey(vt => vt.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(vt => new {vt.VideoId, vt.TagId});
    }
}