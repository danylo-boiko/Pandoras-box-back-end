using Storage.Core.Database;

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
    }
}
