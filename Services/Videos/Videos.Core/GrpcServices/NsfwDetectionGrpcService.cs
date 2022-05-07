using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using NsfwDetectionPb;

namespace Videos.Core.GrpcServices;

public class NsfwDetectionGrpcService
{
    private readonly NsfwDetectionProtoService.NsfwDetectionProtoServiceClient _nsfwDetectionProtoService;

    public NsfwDetectionGrpcService(NsfwDetectionProtoService.NsfwDetectionProtoServiceClient nsfwDetectionProtoService)
    {
        _nsfwDetectionProtoService = nsfwDetectionProtoService ?? throw new ArgumentNullException(nameof(nsfwDetectionProtoService));
    }

    public async Task<VideoDetectionResponse> DetectFromVideo(IFormFile videoFile)
    {
        //todo nsft gRPC client
        await using var stream = videoFile.OpenReadStream();
        var request = new DetectFromVideoRequest
        {
            Video = await ByteString.FromStreamAsync(stream),
            VideoFormat = videoFile.ContentType
        };
            
        return await _nsfwDetectionProtoService.DetectFromVideoAsync(request);
    }
}