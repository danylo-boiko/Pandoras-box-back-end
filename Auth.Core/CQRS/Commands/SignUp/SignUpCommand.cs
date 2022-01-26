namespace Auth.Core.CQRS.Commands.SignUp;

using LS.Helpers.Hosting.API;
using MediatR;

/// <summary>
/// SignUpCommand
/// </summary>
/// <inheritdoc />
public sealed class SignUpCommand : IRequest<ExecutionResult>
{
    public DateOnly BirthDate { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string EmailCode { get; set; }
}