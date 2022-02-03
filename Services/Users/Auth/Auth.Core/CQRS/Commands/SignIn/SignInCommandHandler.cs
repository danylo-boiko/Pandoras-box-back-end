using MediatR;
using Users.Core.Database;
using Users.Core.Database.Entities.Identity;
using Auth.Core.Models.Auth;
using LS.Helpers.Hosting.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Core.CQRS.Commands.SignIn;

/// <summary>
/// SignInCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SignInCommand}" />
public class SignInCommandHandler : IRequestHandler<SignInCommand, ExecutionResult<SignedInUserDto>>
{
    private readonly BaseDbContext _dbContext;
    private readonly SignInManager<ScamUser> _signInManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignInCommandHandler" /> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="signInManager"></param>
    public SignInCommandHandler(BaseDbContext dbContext, 
        SignInManager<ScamUser> signInManager)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request: SignInCommand</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>string</returns>
    public async Task<ExecutionResult<SignedInUserDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _dbContext
                .Users
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .SingleOrDefaultAsync(e => e.Email == request.Email, cancellationToken);

            if (user is null)
            {
                return new ExecutionResult<SignedInUserDto>(new ErrorInfo("Invalid username/password."));
            }
        
            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

            if (!signInResult.Succeeded)
            {
                return new ExecutionResult<SignedInUserDto>(new ErrorInfo("Invalid username/password."));
            }

            var signedInUserDto = new SignedInUserDto
            {
                Id = user.Id,
                Email = user.Email,
                Roles = user
                    .UserRoles
                    .Select(e => e.Role.Name)
                    .ToList(),
                DisplayName = user.DisplayName
            };

            return new ExecutionResult<SignedInUserDto>(signedInUserDto);
        }
        catch (Exception e)
        {
            return new ExecutionResult<SignedInUserDto>(new ErrorInfo("Error while trying to sign in.", e.Message));
        }
    }
}