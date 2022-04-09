using LS.Helpers.Hosting.API;
using MediatR;
using Users.Core.Models.Auth;

namespace Users.Core.CQRS.Commands.Auth.SignIn;

public sealed class SignInCommand : IRequest<ExecutionResult<SignedInUserDto>>
{
    public string Email { get; set; }

    public string Password { get; set; }
}