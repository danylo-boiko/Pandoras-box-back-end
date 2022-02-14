namespace Users.Core.CQRS.Commands.SignOut;

using LS.Helpers.Hosting.API;
using MediatR;

/// <summary>
/// SignOutCommand
/// </summary>
public sealed class SignOutCommand : IRequest<ExecutionResult>
{
}