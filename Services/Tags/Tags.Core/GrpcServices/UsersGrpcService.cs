using Tags.Core.Protos;

namespace Tags.Core.GrpcServices;

public class UsersGrpcService
{
    private readonly UsersProtoService.UsersProtoServiceClient _usersProtoService;

    public UsersGrpcService(UsersProtoService.UsersProtoServiceClient usersProtoService)
    {
        _usersProtoService = usersProtoService ?? throw new ArgumentNullException(nameof(usersProtoService));
    }

    public async Task<UserModel> GetUserAsync(int id)
    {
        return await _usersProtoService.GetUserAsync(new GetUserRequest
        {
            Id = id
        });
    }
}