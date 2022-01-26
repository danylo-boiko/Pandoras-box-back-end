namespace Auth.Core.Database.Extensions
{
    using Entities.Identity;
    using EntityConfigurations;
    using Microsoft.EntityFrameworkCore;

    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyEntityTypesConfigurations(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ScamUserEntityTypeConfiguration());

            return builder;
        }

        public static ModelBuilder ApplyIdentityConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ScamUser>(e => e.ToTable("Users"));
            builder.Entity<ScamRole>(e => e.ToTable("Roles"));
            builder.Entity<ScamUserRole>(e => e.ToTable("UsersRoles"));
            builder.Entity<ScamUserClaim>(e => e.ToTable("UsersClaims"));
            builder.Entity<ScamUserLogin>(e => e.ToTable("UsersLogins"));
            builder.Entity<ScamUserToken>(e => e.ToTable("UsersTokens"));
            builder.Entity<ScamRoleClaim>(e => e.ToTable("RolesClaims"));

            return builder;
        }
    }
}
