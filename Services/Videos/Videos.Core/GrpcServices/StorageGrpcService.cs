using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Storage.Grpc;
using Users.Core.Enums;
using Users.Core.Extensions;

namespace Videos.Core.GrpcServices;

public class StorageGrpcService
{
    private readonly Storage.Grpc.Storage.StorageClient _storageClient;

    public StorageGrpcService(Storage.Grpc.Storage.StorageClient storageClient)
    {
        _storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
    }
    
    public async Task<SaveMediaFilesResponse> SaveVideo(int userId, IFormFile videoFile)
    {
        var videoBytes = await videoFile.GetBytesAsync();
        var fileExtension = Path.GetExtension(videoFile.FileName);
        
        var grpcRequest = new SaveMediaFilesRequest
        {
            FileBytes = ByteString.CopyFrom(videoBytes),
            CategoryId = (int)FileCategory.VideoForChannel,
            Extension = fileExtension,
            UserId = userId
        };
        
        using var call = _storageClient.SaveMediaFiles();
        await call.RequestStream.WriteAsync(grpcRequest);
        await call.RequestStream.CompleteAsync();

        return await call.ResponseAsync;
    }
}