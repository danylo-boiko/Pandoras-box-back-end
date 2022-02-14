using AutoMapper;
using Users.Core.Database.Entities.Identity;

namespace Users.Grpc.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ScamUser, UserModel>();
    }
}