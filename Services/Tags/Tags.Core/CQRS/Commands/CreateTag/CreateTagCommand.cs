using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Database.Entities;

namespace Tags.Core.CQRS.Commands.CreateTag;

public record CreateTagCommand : IRequest<ExecutionResult<Tag>>
{
    public int AuthorId { get; set; }
    public string Content { get; set; }
}