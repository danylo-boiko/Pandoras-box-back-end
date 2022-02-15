using AutoMapper;
using Tags.Core.CQRS.Commands.CreateTag;
using Tags.Core.CQRS.Commands.UpdateTag;
using Tags.Core.Models;

namespace Tags.Core.Mapper;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, CreateTagCommand>().ReverseMap();
        CreateMap<Tag, UpdateTagCommand>().ReverseMap();
    }
}