namespace Storage.Core.Repositories.UserStorageItem
{
    public interface IUserStorageItemRepository
    {
        Task Add(Database.Entities.UserStorageItem item);
    }
}
