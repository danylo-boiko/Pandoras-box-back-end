using LS.Helpers.Hosting.API;
using MediatR;
using Videos.Core.Database;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Queries.GetVideosByUserId;

public record GetVideosByUserIdQuery : IRequest<ExecutionResult<IList<Video>>>
{
    public int UserId { get; set; }
    public PaginationFilter PaginationFilter { get; set; }
}