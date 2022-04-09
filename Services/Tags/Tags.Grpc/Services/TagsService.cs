using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Tags.Core.CQRS.Queries.GetTagById;
using Tags.Grpc.Protos;

namespace Tags.Grpc.Services;

public class TagsService : TagsProtoService.TagsProtoServiceBase
{
    private readonly IMediator _mediator;

    public TagsService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override async Task<TagModel> GetTag(GetTagRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetTagByIdQuery
        {
            Id = request.Id
        });

        if (!result.Success)
        {
            throw new RpcException(new Status(StatusCode.NotFound, String.Join(" ", result.Errors.Select(e => e.ErrorMessage))));
        }

        return new TagModel
        {
            Id = result.Value.Id,
            AuthorId = result.Value.AuthorId,
            Content = result.Value.Content,
            CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(result.Value.CreatedAt, DateTimeKind.Utc))
        };
    }
}