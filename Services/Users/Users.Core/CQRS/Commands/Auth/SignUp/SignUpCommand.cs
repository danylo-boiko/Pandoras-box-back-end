using System.ComponentModel.DataAnnotations;
using LS.Helpers.Hosting.API;
using MediatR;

namespace Users.Core.CQRS.Commands.Auth.SignUp;

/// <summary>
/// SignUpCommand
/// </summary>
/// <inheritdoc />
public sealed class SignUpCommand : IRequest<ExecutionResult>
{
    public DateTime BirthDate { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string RepeatPassword { get; set; }

    public string EmailCode { get; set; }

    public string DisplayName { get; set; }
}