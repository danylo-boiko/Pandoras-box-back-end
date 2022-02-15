using AutoMapper;
using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Models;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Commands.CreateTag;

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, ExecutionResult<Tag>>
{
    private readonly ITagsRepository _tagsRepository;
    private readonly IMapper _mapper;
    
    public CreateTagCommandHandler(ITagsRepository tagsRepository, IMapper mapper)
    {
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<ExecutionResult<Tag>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tagEntity = _mapper.Map<Tag>(request);
            //todo checking for the existence of a tag with the same content
            //todo checking for the existence of a user with id
            await _tagsRepository.CreateAsync(tagEntity);
            
            return new ExecutionResult<Tag>(tagEntity);
        }
        catch (Exception e)
        {
            return new ExecutionResult<Tag>(new ErrorInfo("Error while trying to create a tag.", e.Message));
        }
    }
}