﻿namespace Storage.Core.Repositories.StorageItem
{
    public interface IStorageItemRepository
    {
        Task Add(Database.Entities.StorageItem item);
    }
}
