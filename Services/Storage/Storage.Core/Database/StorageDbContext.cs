using Microsoft.EntityFrameworkCore;
using Storage.Core.Database.Entities;

namespace Storage.Core.Database
{
    public class StorageDbContext : DbContext
    {
        public DbSet<StorageItem> StorageItems { get; set; }
    }
}
