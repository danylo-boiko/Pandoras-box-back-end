namespace Auth.Core.CQRS.Commands.SendTwoFactorDigitCode;

using System.Threading;
using System.Threading.Tasks;
using Database;
using Database.Entities.Identity;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services;
using Services.Interfaces;

/// <summary>
/// SendTwoFactorDigitCode handler.
/// </summary>
/// <seealso cref="MediatR.IRequestHandler{SendTwoFactorDigitCode}" />
public class SendTwoFactorDigitCodeCommandHandler : IRequestHandler<SendTwoFactorDigitCodeCommand, ExecutionResult>
{
    private readonly ILogger<SendTwoFactorDigitCodeCommandHandler> _logger;
    private readonly BaseDbContext _dbContext;
    private readonly UserManager<ScamUser> _userManager;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendTwoFactorDigitCodeCommandHandler" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="dbContext">The database context.</param>
    public SendTwoFactorDigitCodeCommandHandler(
        ILogger<SendTwoFactorDigitCodeCommandHandler> logger,
        BaseDbContext dbContext,
        UserManager<ScamUser> userManager, 
        IEmailService emailService)
    {
        _logger = logger;
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
    public async Task<ExecutionResult> Handle(SendTwoFactorDigitCodeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var totp = new TwoFactorDigitCodeProvider();

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

            var code = await totp.GenerateAsync("sign-up", _userManager, newUser);

            if (code is null)
            {
                return new ExecutionResult(new ErrorInfo("Could not generate two factor digit code."));
            }

            await _emailService.SendMimeMessageAsync(newUser.Email, "E-mail verification code", code);

            return new ExecutionResult(new InfoMessage("Code has been sent successfully."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while sending code.", e.Message));
        }
    }
}