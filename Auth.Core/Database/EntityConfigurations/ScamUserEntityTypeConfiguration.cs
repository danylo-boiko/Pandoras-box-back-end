namespace Auth.Core.Database.EntityConfigurations
{
    using Entities.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ScamUserEntityTypeConfiguration : IEntityTypeConfiguration<ScamUser>
    {
        public void Configure(EntityTypeBuilder<ScamUser> builder)
        {
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
