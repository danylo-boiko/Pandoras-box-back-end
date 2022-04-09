using Storage.Core.Enums;

namespace Storage.Grpc.Services
{
    using Core.Services;
    using global::Grpc.Core;

    public class StorageGrpcService : Storage.StorageBase
    {
        private readonly IStorageService _storageService;

        public StorageGrpcService(IStorageService storageService)
        {
            _storageService = storageService;
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

                    await _storageService.SaveMediaFile(fileBytes, (FileCategory)fileCategory, current.Extension);
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
