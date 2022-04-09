using LS.Helpers.Hosting.API;
using MediatR;

namespace Users.Core.CQRS.Commands.Auth.SendTwoFactorDigitCode;

/// <summary>
/// SendTwoFactorDigitCode
/// </summary>
/// <inheritdoc />
public sealed class SendTwoFactorDigitCodeCommand : IRequest<ExecutionResult>
{
    public string Email { get; set; }
}