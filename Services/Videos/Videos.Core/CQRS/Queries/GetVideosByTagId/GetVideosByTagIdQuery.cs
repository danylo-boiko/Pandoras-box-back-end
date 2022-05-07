using LS.Helpers.Hosting.API;
using MediatR;
using Videos.Core.Database;
using Videos.Core.Database.Entities;

namespace Videos.Core.CQRS.Queries.GetVideosByTagId;

public record GetVideosByTagIdQuery : IRequest<ExecutionResult<IList<Video>>>
{
    public int TagId { get; set; }
    public PaginationFilter PaginationFilter { get; set; }
}