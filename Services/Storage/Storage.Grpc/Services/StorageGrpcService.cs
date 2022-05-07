using System.Text;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Storage.Core.Database;
using Storage.Core.Database.Entities;
using Storage.Core.Enums;
using Storage.Core.Repositories.StorageItem;
using Storage.Core.Repositories.UserStorageItem;

namespace Storage.Grpc.Services
{
    using Core.Services;
    using global::Grpc.Core;

    public class StorageGrpcService : Storage.StorageBase
    {
        private readonly StorageDbContext _dbContext;
        private readonly IStorageService _storageService;
        private readonly IStorageItemRepository _storageItemRepository;
        private readonly IUserStorageItemRepository _userStorageItemRepository;

        public StorageGrpcService(
            IStorageService storageService,
            IStorageItemRepository storageItemRepository,
            IUserStorageItemRepository userStorageItemRepository,
            StorageDbContext dbContext)
        {
            _storageService = storageService;
            _storageItemRepository = storageItemRepository;
            _userStorageItemRepository = userStorageItemRepository;
            _dbContext = dbContext;
        }

        public override async Task<SaveMediaFilesResponse> SaveMediaFiles(IAsyncStreamReader<SaveMediaFilesRequest> requestStream, ServerCallContext context)
        {
            var faultyFilesNumbers = new List<int>();
            var uploadedFiles = new RepeatedField<string>();
            var fileCounter = 0;
            var currentFilePath = string.Empty;

            await foreach (var current in requestStream.ReadAllAsync())
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    fileCounter++;

                    var fileBytes = current
                        .FileBytes
                        .ToByteArray();

                    var fileCategory = current.CategoryId;

                    var storageItem = await _storageService.SaveMediaFile(fileBytes, (FileCategory)fileCategory, current.Extension);
                    currentFilePath = storageItem.Location;
                    uploadedFiles.Add(currentFilePath);
                    
                    await _storageItemRepository.Add(storageItem);

                    var userStorageItem = new UserStorageItem
                    {
                        StorageItemId = storageItem.Id,
                        UserId = current.UserId
                    };
                    await _userStorageItemRepository.Add(userStorageItem);
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    File.Delete(currentFilePath);
                    uploadedFiles.Remove(currentFilePath);
                    faultyFilesNumbers.Add(fileCounter);
                }
            }

            var isSuccess = !faultyFilesNumbers.Any();
            var builder = new StringBuilder();
            for (var i = 0; i < faultyFilesNumbers.Count; i++)
            {
                if (i + 1 == faultyFilesNumbers.Count)
                {
                    builder.Append($"{faultyFilesNumbers[i]}");
                }
                else
                {
                    builder.Append($"{faultyFilesNumbers[i]}, ");
                }
            }
            var message = isSuccess ? string.Empty : $"Could not upload files: {builder}";
            return new SaveMediaFilesResponse { IsSuccess = isSuccess, Locations = {uploadedFiles}, Message = message};
        }

        public override async Task<GetUserCurrentAvatarDataResponse> GetUserCurrentAvatarData(GetUserCurrentAvatarDataRequest request, ServerCallContext context)
        {
            var userStorageItem = await _userStorageItemRepository.GetByUserId(request.UserId);

            if (userStorageItem is null)
            {
                return new GetUserCurrentAvatarDataResponse
                {
                    AvatarBytes = ByteString.Empty
                };
            }

            var bytes = await File.ReadAllBytesAsync(userStorageItem.StorageItem.Location);
            return new GetUserCurrentAvatarDataResponse
            {
                AvatarBytes = ByteString.CopyFrom(bytes),
                Extension = userStorageItem.StorageItem.Extension
            };
        }
    }
}
