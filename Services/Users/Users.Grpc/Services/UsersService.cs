using AutoMapper;
using Grpc.Core;
using Users.Core.Repositories.Interfaces;
using Users.Grpc.Protos;

namespace Users.Grpc.Services;

public class UsersService : UsersProtoService.UsersProtoServiceBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public UsersService(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }
    
    public override async Task<UserModel> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var user = await _usersRepository.GetAsync(request.Id);
        
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with id: {request.Id} is not found."));
        }
        
        return _mapper.Map<UserModel>(user);
    }
}