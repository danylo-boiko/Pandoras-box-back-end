using Microsoft.EntityFrameworkCore;
using Storage.Core.Database;
using Storage.Core.Enums;

namespace Storage.Core.Repositories.UserStorageItem
{
    public class UserStorageItemRepository : IUserStorageItemRepository
    {
        private readonly StorageDbContext _dbContext;

        public UserStorageItemRepository(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Database.Entities.UserStorageItem item)
        {
            _dbContext.UsersStorageItems.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Database.Entities.UserStorageItem?> GetByUserId(int userId, FileCategory storageItemCategory = FileCategory.Avatar)
        {
            var record = await _dbContext
                .UsersStorageItems
                .Include(e => e.StorageItem)
                .SingleOrDefaultAsync(e => e.UserId == userId &&
                                           e.StorageItem.Category == storageItemCategory);

            return record;
        }
    }
}
