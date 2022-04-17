namespace Users.Core.CQRS.Commands.Profile.SetAvatar;

using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Http;

/// <summary>
/// SetAvatarCommand
/// </summary>
public sealed class SetAvatarCommand : IRequest<ExecutionResult>
{
    public IFormFile Avatar { get; init; }
}