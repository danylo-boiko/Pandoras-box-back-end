namespace Auth.Core.CQRS.Commands.SignUp;

using System.Threading;
using System.Threading.Tasks;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Users.Common;
using Microsoft.EntityFrameworkCore;
using Users.Core.Database;
using Users.Core.Database.Entities.Identity;
using Models.Auth;
using NodaTime;
using Services.Email;
using Services.TwoFactorDigitCodeProvider;
using SignIn;

/// <summary>
/// SignUpCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SignUpCommand}" />
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, ExecutionResult>
{
    private readonly BaseDbContext _dbContext;
    private readonly UserManager<ScamUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignUpCommandHandler" /> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public SignUpCommandHandler(
        BaseDbContext dbContext,
        UserManager<ScamUser> userManager,
        IEmailService emailService,
        IMediator mediator)
    {
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
                return new ExecutionResult(new ErrorInfo("Code and e-mail do not match."));
            }

            var isCodeValid = await totp.ValidateAsync("sign-up", request.EmailCode, _userManager, user);
            if (!isCodeValid)
            {
                return new ExecutionResult(new ErrorInfo("Code and e-mail do not match."));
            }

            user.EmailConfirmed = true;
            user.BirthDate = LocalDate.FromDateTime(request.BirthDate);
            user.DisplayName = request.DisplayName;
            user.UserRoles
                .Add(new ScamUserRole
                {
                    UserId = user.Id, 
                    RoleId = AppConsts.UserRoles.User
                });

            await _userManager.AddPasswordAsync(user, request.Password);

            //todo: add avatar_url and possibly bio

            await _dbContext.SaveChangesAsync(cancellationToken);

            var signInResult = await _mediator.Send(new SignInCommand { Email = user.Email, Password = request.Password }, cancellationToken);

            await _emailService.SendMimeMessageAsync(user.Email, "Welcome to Pandora's Box!", "You have successfully signed up.");

            return new ExecutionResult<SignedInUserDto>(signInResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ExecutionResult(new ErrorInfo(e.Message));
        }
    }
}