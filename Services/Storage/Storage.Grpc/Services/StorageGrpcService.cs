using Storage.Core.Enums;
using Storage.Core.Repositories.StorageItem;

namespace Storage.Grpc.Services
{
    using Core.Services;
    using global::Grpc.Core;

    public class StorageGrpcService : Storage.StorageBase
    {
        private readonly IStorageService _storageService;
        private readonly IStorageItemRepository _storageItemRepository;

        public StorageGrpcService(
            IStorageService storageService,
            IStorageItemRepository storageItemRepository)
        {
            _storageService = storageService;
            _storageItemRepository = storageItemRepository;
        }

        public override async Task<SaveMediaFilesResponse> SaveMediaFiles(IAsyncStreamReader<SaveMediaFilesRequest> requestStream, ServerCallContext context)
        {
            try
            {
                await foreach (var current in requestStream.ReadAllAsync())
                {
                    var fileBytes = current
                        .FileBytes
                        .ToByteArray();

                    var fileCategory = current.CategoryId;

                    var storageItem = await _storageService.SaveMediaFile(fileBytes, (FileCategory)fileCategory, current.Extension);
                    await _storageItemRepository.Add(storageItem);
                }

                return new SaveMediaFilesResponse { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new SaveMediaFilesResponse { IsSuccess = false, Message = $"Error while saving media files to the storage. {e.Message}" };
            }
        }
    }
}
