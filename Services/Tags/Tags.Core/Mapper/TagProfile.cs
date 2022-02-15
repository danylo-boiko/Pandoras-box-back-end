using AutoMapper;
using Tags.Core.CQRS.Commands.CreateTag;
using Tags.Core.CQRS.Commands.UpdateTag;
using Tags.Core.Models;

namespace Tags.Core.Mapper;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<CreateTagCommand, Tag>()
            .ForMember(t => t.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ReverseMap();
        
        CreateMap<UpdateTagCommand, Tag>().ReverseMap();
    }
}