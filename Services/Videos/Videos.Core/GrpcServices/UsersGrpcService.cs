using Users.Grpc.Protos;

namespace Videos.Core.GrpcServices;

public class UsersGrpcService
{
    private readonly UsersProtoService.UsersProtoServiceClient _usersClient;

    public UsersGrpcService(UsersProtoService.UsersProtoServiceClient usersClient)
    {
        _usersClient = usersClient ?? throw new ArgumentNullException(nameof(usersClient));
    }

    public async Task<UserModel> GetUserAsync(int id)
    {
        return await _usersClient.GetUserAsync(new GetUserRequest
        {
            Id = id
        });
    }
}