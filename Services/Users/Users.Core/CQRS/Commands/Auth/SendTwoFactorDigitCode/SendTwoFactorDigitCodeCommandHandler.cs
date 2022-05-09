using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Users.Core.Consts;
using Users.Core.Database;
using Users.Core.Database.Entities.Identity;
using Users.Core.Services.Email;
using Users.Core.Services.TwoFactorDigitCodeProvider;

namespace Users.Core.CQRS.Commands.Auth.SendTwoFactorDigitCode;

/// <summary>
/// SendTwoFactorDigitCode handler.
/// </summary>
/// <seealso cref="IRequestHandler{SendTwoFactorDigitCode}" />
public class SendTwoFactorDigitCodeCommandHandler : IRequestHandler<SendTwoFactorDigitCodeCommand, ExecutionResult>
{
    private readonly UsersDbContext _dbContext;
    private readonly UserManager<ScamUser> _userManager;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendTwoFactorDigitCodeCommandHandler" /> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public SendTwoFactorDigitCodeCommandHandler(
        UsersDbContext dbContext, 
        UserManager<ScamUser> userManager, 
        IEmailService emailService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _emailService = emailService;
    }

    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request: SendTwoFactorDigitCode</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>string</returns>
    public async Task<ExecutionResult> Handle(SendTwoFactorDigitCodeCommand request, CancellationToken cancellationToken)
    {
        var totp = new TwoFactorDigitCodeProvider();

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var newUser = new ScamUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            if (await _userManager.FindByEmailAsync(newUser.Email) is not null)
            {
                return new ExecutionResult(new ErrorInfo("Access denied."));
            }

            var userCreationResult = await _userManager.CreateAsync(newUser);
            if (!userCreationResult.Succeeded)
            {
                var errorsInfo = userCreationResult
                    .Errors
                    .Select(identityError => new ErrorInfo(identityError.Code, identityError.Description))
                    .ToList();

                return new ExecutionResult(errorsInfo);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            var code = await totp.GenerateAsync("sign-up", _userManager, newUser);

            if (code is null)
            {
                return new ExecutionResult(new ErrorInfo("Could not generate two factor digit code."));
            }

            newUser.UserRoles = new List<ScamUserRole>
            {
                new() {
                    UserId = newUser.Id,
                    RoleId = AppConsts.UserRoles.NewUser
                }
            };

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _emailService.SendMimeMessageAsync(newUser.Email, "E-mail verification code", code);

            await transaction.CommitAsync(cancellationToken);

            return new ExecutionResult(new InfoMessage("Confirmation code has been sent successfully."));
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new ExecutionResult(new ErrorInfo("Error while sending 6-digit code to e-mail.", e.Message));
        }
    }
}