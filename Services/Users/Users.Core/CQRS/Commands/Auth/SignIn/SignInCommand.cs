namespace Users.Core.CQRS.Commands.SignIn;

using LS.Helpers.Hosting.API;
using MediatR;
using Models.Auth;

public sealed class SignInCommand : IRequest<ExecutionResult<SignedInUserDto>>
{
    public string Email { get; set; }

    public string Password { get; set; }
}