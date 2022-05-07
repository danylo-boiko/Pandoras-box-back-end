namespace Users.Core.Database
{
    using Entities.Identity;
    using Extensions;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class UsersDbContext : IdentityDbContext<
        ScamUser,
        ScamRole,
        int,
        ScamUserClaim,
        ScamUserRole,
        ScamUserLogin,
        ScamRoleClaim,
        ScamUserToken>
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyEntityTypesConfigurations()
                .ApplyIdentityConfiguration()
                .ApplyValueConversions()
                .ApplySeed();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder
                .Properties<string>()
                .HaveMaxLength(250);
        }
    }
}
