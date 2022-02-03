namespace Users.Core.Database
{
    using Common;
    using Entities.Identity;
    using Microsoft.EntityFrameworkCore;

    public static class DbSeed
    {
        public static ModelBuilder SeedRoles(this ModelBuilder builder)
        {
            var roles = new List<ScamRole>
            {
                new()
                {
                    Id = 1,
                    Name = nameof(AppConsts.UserRoles.Admin),
                    NormalizedName = nameof(AppConsts.UserRoles.Admin).ToUpper()
                },
                new()
                {
                    Id = 2,
                    Name = nameof(AppConsts.UserRoles.User),
                    NormalizedName = nameof(AppConsts.UserRoles.User).ToUpper()
                },
                new()
                {
                    Id = 3,
                    Name = nameof(AppConsts.UserRoles.NewUser),
                    NormalizedName = nameof(AppConsts.UserRoles.NewUser).ToUpper()
                }
            };

            builder
                .Entity<ScamRole>()
                .HasData(roles);

            return builder;
        }
    }
}
