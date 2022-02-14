namespace Users.Core.CQRS.Commands.SignOut;

using System.Threading;
using System.Threading.Tasks;
using Database.Entities.Identity;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.User;

/// <summary>
/// SignOutCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SignOutCommand}" />
public class SignOutCommandHandler : IRequestHandler<SignOutCommand, ExecutionResult>
{
    private readonly SignInManager<ScamUser> _signInManager;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignOutCommandHandler" /> class.
    /// </summary>
    public SignOutCommandHandler(SignInManager<ScamUser> signInManager,
        IUserService userService)
    {
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
                return new ExecutionResult(new ErrorInfo("No such user found."));
            }

            await _signInManager.SignOutAsync();

            return new ExecutionResult(new InfoMessage("You have successfully signed out."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while signing out.", e.Message));
        }
    }
}