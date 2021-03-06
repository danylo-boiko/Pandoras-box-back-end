using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Database;
using Tags.Core.Database.Entities;

namespace Tags.Core.CQRS.Queries.GetTagsByPattern;

public record GetTagsByPatternQuery : IRequest<ExecutionResult<IList<Tag>>>
{
    public string Pattern { get; set; }
    public PaginationFilter PaginationFilter { get; set; }
}