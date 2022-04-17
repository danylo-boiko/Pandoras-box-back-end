using LS.Helpers.Hosting.API;
using MediatR;

namespace Users.Core.CQRS.Commands.Profile.SetProfileInfo;

public class SetProfileInfoCommand : IRequest<ExecutionResult>
{
    public string DisplayName { get; init; }

    public string Bio { get; init; }
}