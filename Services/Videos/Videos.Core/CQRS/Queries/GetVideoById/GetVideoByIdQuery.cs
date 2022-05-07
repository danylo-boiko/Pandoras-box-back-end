using LS.Helpers.Hosting.API;
using MediatR;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Queries.GetVideoById;

public record GetVideoByIdQuery : IRequest<ExecutionResult<Video>>
{
    public int Id { get; set; }
}