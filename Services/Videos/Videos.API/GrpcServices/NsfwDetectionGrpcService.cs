using Google.Protobuf;
using NsfwDetectionPb;

namespace Videos.API.GrpcServices;

public class NsfwDetectionGrpcService
{
    private readonly NsfwDetectionProtoService.NsfwDetectionProtoServiceClient _nsfwDetectionProtoService;

    public NsfwDetectionGrpcService(NsfwDetectionProtoService.NsfwDetectionProtoServiceClient nsfwDetectionProtoService)
    {
        _nsfwDetectionProtoService = nsfwDetectionProtoService;
    }

    public async Task<VideoDetectionResponse> DetectFromVideo(IFormFile videoFile)
    {
        await using var stream = videoFile.OpenReadStream();
        var request = new DetectFromVideoRequest
        {
            Video = await ByteString.FromStreamAsync(stream),
            VideoFormat = videoFile.ContentType
        };
            
        return await _nsfwDetectionProtoService.DetectFromVideoAsync(request);
    }
}