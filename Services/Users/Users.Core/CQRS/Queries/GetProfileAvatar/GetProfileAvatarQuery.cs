using LS.Helpers.Hosting.API;
using MediatR;

namespace Users.Core.CQRS.Queries.GetProfileAvatar;

public class GetProfileAvatarQuery : IRequest<ExecutionResult<GetProfileAvatarQueryResult>>
{
}