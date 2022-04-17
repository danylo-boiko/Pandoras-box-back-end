using Microsoft.EntityFrameworkCore;
using Storage.Core.Database.Entities;

namespace Storage.Core.Database
{
    public class StorageDbContext : DbContext
    {
        public virtual DbSet<StorageItem> StorageItems { get; set; }

        public virtual DbSet<UserStorageItem> UsersStorageItems { get; set; }

        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StorageDbContext).Assembly);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<string>()
                .HaveMaxLength(250);
        }
    }
}
