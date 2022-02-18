using AutoMapper;
using Users.Core.Database.Entities.Identity;
using Users.Grpc.Protos;

namespace Users.Grpc.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ScamUser, UserModel>();
    }
}