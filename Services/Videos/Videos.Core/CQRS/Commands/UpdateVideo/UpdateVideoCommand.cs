using LS.Helpers.Hosting.API;
using MediatR;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Commands.UpdateVideo;

public record UpdateVideoCommand : IRequest<ExecutionResult<Video>>
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public ICollection<int>? TagsIds { get; set; }
}