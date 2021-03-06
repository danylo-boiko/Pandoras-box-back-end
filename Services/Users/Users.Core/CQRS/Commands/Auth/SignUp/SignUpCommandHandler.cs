using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NodaTime;
using Users.Core.Consts;
using Users.Core.CQRS.Commands.Auth.SignIn;
using Users.Core.Database;
using Users.Core.Database.Entities.Identity;
using Users.Core.Models.Auth;
using Users.Core.Services.Email;
using Users.Core.Services.TwoFactorDigitCodeProvider;

namespace Users.Core.CQRS.Commands.Auth.SignUp;

/// <summary>
/// SignUpCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SignUpCommand}" />
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, ExecutionResult>
{
    private readonly ILogger<SignUpCommandHandler> _logger;
    private readonly UsersDbContext _dbContext;
    private readonly UserManager<ScamUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignUpCommandHandler" /> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public SignUpCommandHandler(
        ILogger<SignUpCommandHandler> logger,
        UsersDbContext dbContext,
        UserManager<ScamUser> userManager,
        IEmailService emailService,
        IMediator mediator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
        _emailService = emailService;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request: SignUpCommand</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>string</returns>
    public async Task<ExecutionResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var totp = new TwoFactorDigitCodeProvider();

            var user = await _dbContext
                .Users
                .Include(e => e.UserRoles)
                .SingleOrDefaultAsync(e => e.Email == request.Email, cancellationToken);

            if (user is null)
            {
                _logger.LogError("User with email: {Email} is not exist", request.Email);
                return new ExecutionResult(new ErrorInfo("Code and e-mail do not match."));
            }

            var isCodeValid = await totp.ValidateAsync("sign-up", request.EmailCode, _userManager, user);
            if (!isCodeValid)
            {
                _logger.LogError("6-digit code is not valid");
                return new ExecutionResult(new ErrorInfo("Code and e-mail do not match."));
            }

            user.EmailConfirmed = true;
            user.BirthDate = LocalDate.FromDateTime(request.BirthDate);
            user.DisplayName = request.DisplayName;
            user.UserRoles.Add(new ScamUserRole
            {
                UserId = user.Id,
                RoleId = AppConsts.UserRoles.User
            });

            await _userManager.AddPasswordAsync(user, request.Password);

            //todo: add avatar_url and possibly bio

            await _dbContext.SaveChangesAsync(cancellationToken);

            var signInResult = await _mediator.Send(new SignInCommand { Email = user.Email, Password = request.Password }, cancellationToken);

            await _emailService.SendMimeMessageAsync(user.Email, "Welcome to Pandora's Box!", "You have successfully signed up.");

            _logger.LogInformation("{Email} has been successfully signed up", request.Email);
            return new ExecutionResult<SignedInUserDto>(signInResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ExecutionResult(new ErrorInfo(e.Message));
        }
    }
}