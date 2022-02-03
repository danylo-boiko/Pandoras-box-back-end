using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Users.Core.Database;

namespace Users.Grpc.Services;

public class UsersService : UsersProtoService.UsersProtoServiceBase
{
    private readonly BaseDbContext _dbContext;

    public UsersService(BaseDbContext baseDbContext)
    {
        _dbContext = baseDbContext;
    }

    public override async Task<IsUserExistResponse> IsUserExist(IsUserExistRequest request, ServerCallContext context)
    {
        return new IsUserExistResponse
        {
            IsExist = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(request.Id)) != null
        };
    }
}