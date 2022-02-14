namespace Users.Core.Database
{
    using Microsoft.EntityFrameworkCore;

    public static class DbSeedApplier
    {
        public static void ApplySeed(this ModelBuilder builder)
        {
            builder
                .SeedRoles();
        }
    }
}
