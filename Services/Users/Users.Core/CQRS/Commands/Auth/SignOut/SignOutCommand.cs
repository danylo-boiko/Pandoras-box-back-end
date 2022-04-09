using LS.Helpers.Hosting.API;
using MediatR;

namespace Users.Core.CQRS.Commands.Auth.SignOut;

/// <summary>
/// SignOutCommand
/// </summary>
public sealed class SignOutCommand : IRequest<ExecutionResult>
{
}