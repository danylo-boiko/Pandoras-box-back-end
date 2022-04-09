using Storage.Core.Enums;

namespace Storage.Core.Services
{
    public interface IStorageService
    {
        public Task SaveMediaFile(byte[] fileBytes, FileCategory fileCategory, string fileExtension);
    }
}
