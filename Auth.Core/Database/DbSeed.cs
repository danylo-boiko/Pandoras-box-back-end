namespace Auth.Core.Database
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

        public static ModelBuilder SeedUsers(this ModelBuilder builder)
        {
            var users = new List<ScamUser>
            {
                new()
                {
                    Id = 1,
                    Email = "jackie@mail.com",
                    EmailConfirmed = true,
                    UserRoles = new List<ScamUserRole>
                    {
                        new()
                        {
                            RoleId = AppConsts.UserRoles.Admin,
                            UserId = 1
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.User,
                            UserId = 1
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.NewUser,
                            UserId = 1
                        },
                    },
                },
                new()
                {
                    Id = 2,
                    Email = "bruce@mail.com",
                    EmailConfirmed = true,
                    UserRoles = new List<ScamUserRole>
                    {
                        new()
                        {
                            RoleId = AppConsts.UserRoles.Admin,
                            UserId = 2
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.User,
                            UserId = 2
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.NewUser,
                            UserId = 2
                        },
                    },
                },
                new()
                {
                    Id = 3,
                    Email = "vandamme@mail.com",
                    EmailConfirmed = true,
                    UserRoles = new List<ScamUserRole>
                    {
                        new()
                        {
                            RoleId = AppConsts.UserRoles.Admin,
                            UserId = 3
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.User,
                            UserId = 3
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.NewUser,
                            UserId = 3
                        },
                    },
                },
                new()
                {
                    Id = 4,
                    Email = "akuma@mail.com",
                    EmailConfirmed = true,
                    UserRoles = new List<ScamUserRole>
                    {
                        new()
                        {
                            RoleId = AppConsts.UserRoles.Admin,
                            UserId = 4
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.User,
                            UserId = 4
                        },
                        new()
                        {
                            RoleId = AppConsts.UserRoles.NewUser,
                            UserId = 4
                        },
                    },
                }
            };

            builder
                .Entity<ScamUser>()
                .HasData(users);

            return builder;
        }
    }
}
