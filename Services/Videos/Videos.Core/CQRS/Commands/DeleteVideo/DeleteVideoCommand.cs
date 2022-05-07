using LS.Helpers.Hosting.API;
using MediatR;

namespace Videos.Core.CQRS.Commands.DeleteVideo;

public record DeleteVideoCommand : IRequest<ExecutionResult>
{
    public int Id { get; set; }
}