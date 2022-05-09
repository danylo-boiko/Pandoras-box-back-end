using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;
using Users.Core.Database;
using Users.Core.Services.User;

namespace Users.Core.CQRS.Commands.Profile.SetProfileInfo;

public class SetProfileInfoCommandHandler : IRequestHandler<SetProfileInfoCommand, ExecutionResult>
{
    private readonly ILogger<SetProfileInfoCommandHandler> _logger;
    private readonly UsersDbContext _dbContext;
    private readonly IUserService _userService;

    public SetProfileInfoCommandHandler(
        ILogger<SetProfileInfoCommandHandler> logger,
        UsersDbContext dbContext,
        IUserService userService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<ExecutionResult> Handle(SetProfileInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();

            user.Bio = request.Bio;
            user.DisplayName = request.DisplayName;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Profile info for user with id: {Id} has been successfully updated", user.Id);
            return new ExecutionResult(new InfoMessage($"Profile info for user with id: {user.Id} has been successfully updated."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo($"Error while executing SetProfileInfoCommand.\n> {e.Message}"));
        }
    }
}