using LS.Helpers.Hosting.API;
using MediatR;
using Users.Core.Services.User;

namespace Users.Core.CQRS.Queries.GetProfileInfo;

public class GetProfileInfoQueryHandler : IRequestHandler<GetProfileInfoQuery, ExecutionResult<GetProfileInfoQueryResult>>
{
    private readonly IUserService _userService;

    public GetProfileInfoQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ExecutionResult<GetProfileInfoQueryResult>> Handle(GetProfileInfoQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();
            var result = new GetProfileInfoQueryResult
            {
                Bio = user.Bio,
                DisplayName = user.DisplayName
            };

            return new ExecutionResult<GetProfileInfoQueryResult>(result);
        }
        catch (Exception e)
        {
            return new ExecutionResult<GetProfileInfoQueryResult>(new ErrorInfo($"Error while executing GetAllProfileInfoQuery.\n> {e.Message}"));
        }
    }
}