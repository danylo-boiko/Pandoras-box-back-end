namespace Storage.Core.Database.Entities
{
    public class UserStorageItem
    {
        public int UserId { get; set; }

        public int StorageItemId { get; set; }

        public StorageItem StorageItem { get; set; }
    }
}
