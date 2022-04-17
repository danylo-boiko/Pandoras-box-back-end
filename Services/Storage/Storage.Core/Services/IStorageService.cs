using Storage.Core.Database.Entities;
using Storage.Core.Enums;

namespace Storage.Core.Services
{
    public interface IStorageService
    {
        public Task<StorageItem> SaveMediaFile(byte[] fileBytes, FileCategory fileCategory, string fileExtension);
    }
}
