using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Users.Core.Database.Entities.Identity;
using Users.Core.Services.User;

namespace Users.Core.CQRS.Commands.Auth.SignOut;

/// <summary>
/// SignOutCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SignOutCommand}" />
public class SignOutCommandHandler : IRequestHandler<SignOutCommand, ExecutionResult>
{
    private readonly ILogger<SignOutCommandHandler> _logger;
    private readonly SignInManager<ScamUser> _signInManager;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignOutCommandHandler" /> class.
    /// </summary>
    public SignOutCommandHandler(ILogger<SignOutCommandHandler> logger, SignInManager<ScamUser> signInManager, IUserService userService)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userService = userService;
    }

    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request: SignOutCommand</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>string</returns>
    public async Task<ExecutionResult> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user is null)
            {
                _logger.LogError("User with id: {id} is not exist", _userService.UserId);
                return new ExecutionResult(new ErrorInfo("No such user found."));
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("{Email} has been successfully signed out", user.Email);
            return new ExecutionResult(new InfoMessage("You have successfully signed out."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while signing out.", e.Message));
        }
    }
}