using Tags.Grpc.Protos;

namespace Videos.Core.GrpcServices;

public class TagsGrpcService
{
    private readonly TagsProtoService.TagsProtoServiceClient _tagsClient;

    public TagsGrpcService(TagsProtoService.TagsProtoServiceClient tagsClient)
    {
        _tagsClient = tagsClient ?? throw new ArgumentNullException(nameof(tagsClient));
    }

    public async Task<TagModel> GetTagAsync(int id)
    {
        return await _tagsClient.GetTagAsync(new GetTagRequest
        {
            Id = id
        });
    }
}