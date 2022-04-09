using Storage.Core.Enums;

namespace Storage.Core.Services
{
    using System.Security.Cryptography;
    using System.Text;
    using Exceptions;
    using Helpers;
    using Microsoft.Extensions.Options;
    using Settings;

    public class StorageService : IStorageService
    {
        private readonly IOptions<FileHashingSettings> _fileHashingOptions;

        public StorageService(IOptions<FileHashingSettings> fileHashingOptions)
        {
            _fileHashingOptions = fileHashingOptions;
        }

        public async Task SaveMediaFile(byte[] fileBytes, FileCategory fileCategory, string fileExtension)
        {
            var fileName = GetFileName(fileBytes, fileExtension);
            var path = GetFilePath(fileName, fileCategory);
            await WriteFileAsync(fileBytes, path);
        }

        private string GetFileName(byte[] fileBytes, string fileExtension)
        {
            var secretBytes = Encoding.UTF8.GetBytes(_fileHashingOptions.Value.Secret);
            var base64String = Convert.ToBase64String(fileBytes);
            var base64StringBytes = Encoding.UTF8.GetBytes(base64String);

            var fileNameBuilder = new StringBuilder();
            using var provider = new HMACMD5(secretBytes);
            var base64StringHash = provider.ComputeHash(base64StringBytes);
            foreach (var b in base64StringHash)
            {
                fileNameBuilder.Append(b.ToString("x2").ToLower());
            }

            var fileName = $"{fileNameBuilder}{fileExtension}";

            return fileName;
        }

        private static string GetFilePath(string fileName, FileCategory fileCategory)
        {
            return fileCategory switch
            {
                FileCategory.Avatar => StoragePathsHelper.GetAvatarPath(fileName),
                FileCategory.VideoForChannel => StoragePathsHelper.GetChannelVideoPath(fileName),
                _ => throw new InvalidFileCategoryException("Unsupported file category has been provided.")
            };
        }

        private static async Task WriteFileAsync(byte[] fileBytes, string path)
        {
            await using var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            await stream.WriteAsync(fileBytes);
            stream.Close();
        }
    }
}
