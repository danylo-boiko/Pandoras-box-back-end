using Auth.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Core.CQRS.Commands.SignIn;

using Auth.Core.Models.Auth;
using Database.Entities.Identity;
using LS.Helpers.Hosting.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// SignInCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SignInCommand}" />
public class SignInCommandHandler : IRequestHandler<SignInCommand, ExecutionResult<SignedInUserDTO>>
{
    private readonly ILogger<SignInCommandHandler> _logger;
    private readonly BaseDbContext _dbContext;
    private readonly SignInManager<ScamUser> _signInManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignInCommandHandler" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="dbContext">The database context.</param>
    /// <param name="signInManager"></param>
    public SignInCommandHandler(
        ILogger<SignInCommandHandler> logger,
        BaseDbContext dbContext, 
        SignInManager<ScamUser> signInManager)
    {
        _logger = logger;
        _dbContext = dbContext;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request: SignInCommand</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>string</returns>
    public async Task<ExecutionResult<SignedInUserDTO>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var userInitial = await _dbContext
            .Users
            .Include(e => e.UserRoles)
            .ThenInclude(e => e.Role)
            .SingleOrDefaultAsync(e => e.Username == request.Username, cancellationToken);

        if (userInitial is null)
        {
            return new ExecutionResult<SignedInUserDTO>(new ErrorInfo("Invalid username/password."));
        }
        
        var signInResult = await _signInManager.CheckPasswordSignInAsync(userInitial, request.Password, false);

        if (!signInResult.Succeeded)
        {
            return new ExecutionResult<SignedInUserDTO>(new ErrorInfo("Invalid username/password."));
        }

        var signedInUserDto = new SignedInUserDTO
        {
            Id = userInitial.Id,
            Email = userInitial.Email,
            Roles = userInitial
                .UserRoles
                .Select(e => e.Role.Name)
                .ToList(),
            Username = userInitial.Username
        };

        return new ExecutionResult<SignedInUserDTO>(signedInUserDto);
    }
}