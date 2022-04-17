using LS.Helpers.Hosting.API;
using MediatR;

namespace Users.Core.CQRS.Queries.GetProfileInfo;

public class GetProfileInfoQuery : IRequest<ExecutionResult<GetProfileInfoQueryResult>>
{
}