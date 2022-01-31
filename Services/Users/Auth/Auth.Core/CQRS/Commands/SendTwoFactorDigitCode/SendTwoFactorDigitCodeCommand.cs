namespace Auth.Core.CQRS.Commands.SendTwoFactorDigitCode;

using LS.Helpers.Hosting.API;
using MediatR;

/// <summary>
/// SendTwoFactorDigitCode
/// </summary>
/// <inheritdoc />
public sealed class SendTwoFactorDigitCodeCommand : IRequest<ExecutionResult>
{
    public string Email { get; set; }
}