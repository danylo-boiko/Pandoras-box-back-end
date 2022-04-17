using Storage.Core.Enums;

namespace Storage.Core.Repositories.UserStorageItem
{
    public interface IUserStorageItemRepository
    {
        Task Add(Database.Entities.UserStorageItem item);

        Task<Database.Entities.UserStorageItem?> GetByUserId(int userId,
            FileCategory storageItemCategory = FileCategory.Avatar);
    }
}
