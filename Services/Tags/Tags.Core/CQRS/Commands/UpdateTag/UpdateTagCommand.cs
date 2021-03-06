using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Database.Entities;

namespace Tags.Core.CQRS.Commands.UpdateTag;

public record UpdateTagCommand : IRequest<ExecutionResult<Tag>>
{
    public int Id { get; set; }
    public string Content { get; set; }
}