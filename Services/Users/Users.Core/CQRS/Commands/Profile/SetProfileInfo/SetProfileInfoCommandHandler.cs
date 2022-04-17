using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.Core.Database;
using Users.Core.Services.User;

namespace Users.Core.CQRS.Commands.Profile.SetProfileInfo;

public class SetProfileInfoCommandHandler : IRequestHandler<SetProfileInfoCommand, ExecutionResult>
{
    private readonly UsersDbContext _dbContext;
    private readonly IUserService _userService;

    public SetProfileInfoCommandHandler(
        UsersDbContext dbContext,
        IUserService userService
    )
    {
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

            return new ExecutionResult(new InfoMessage("Profile info has been successfully updated."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(
                new ErrorInfo($"Error while executing SetProfileInfoCommand.\n> {e.Message}"));
        }
    }
}