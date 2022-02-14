namespace Storage.Core.Services
{
    using Consts;

    public interface IStorageService
    {
        public Task SaveMediaFile(byte[] fileBytes, FileCategories fileCategory, string fileExtension);
    }
}
