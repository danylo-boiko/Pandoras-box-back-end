namespace Auth.Core.CQRS.Commands.SignIn;

using Auth.Core.Models.Auth;
using LS.Helpers.Hosting.API;
using MediatR;

public sealed class SignInCommand : IRequest<ExecutionResult<SignedInUserDTO>>
{
    public string Username { get; set; }

    public string Password { get; set; }
}