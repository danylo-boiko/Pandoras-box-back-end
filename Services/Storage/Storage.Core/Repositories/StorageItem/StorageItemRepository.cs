using Microsoft.EntityFrameworkCore;
using Storage.Core.Database;

namespace Storage.Core.Repositories.StorageItem
{
    public class StorageItemRepository : IStorageItemRepository
    {
        private readonly StorageDbContext _dbContext;

        public StorageItemRepository(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Database.Entities.StorageItem item)
        {
            _dbContext.StorageItems.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Database.Entities.StorageItem?> GetById(int id)
        {
            var storageItem = await _dbContext.StorageItems.SingleOrDefaultAsync(e => e.Id == id);
            return storageItem;
        }
    }
}
