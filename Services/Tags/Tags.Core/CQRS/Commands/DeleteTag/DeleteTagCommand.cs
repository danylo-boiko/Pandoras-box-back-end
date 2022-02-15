using LS.Helpers.Hosting.API;
using MediatR;

namespace Tags.Core.CQRS.Commands.DeleteTag;

public record DeleteTagCommand : IRequest<ExecutionResult>
{
    public int Id { get; set; }
}