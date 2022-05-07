using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Database.Entities;

namespace Tags.Core.CQRS.Queries.GetTagById;

public record GetTagByIdQuery : IRequest<ExecutionResult<Tag>>
{
    public int Id { get; set; }
}