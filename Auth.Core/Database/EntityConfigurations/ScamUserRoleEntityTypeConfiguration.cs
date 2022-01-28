namespace Auth.Core.Database.EntityConfigurations
{
    using Entities.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ScamUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<ScamUserRole>
    {
        public void Configure(EntityTypeBuilder<ScamUserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(x => x.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
