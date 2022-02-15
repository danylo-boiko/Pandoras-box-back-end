using AutoMapper;
using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Models;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Commands.UpdateTag;

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, ExecutionResult<Tag>>
{
    private readonly ITagsRepository _tagsRepository;
    private readonly IMapper _mapper;

    public UpdateTagCommandHandler(ITagsRepository tagsRepository, IMapper mapper)
    {
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<ExecutionResult<Tag>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tagEntity = _mapper.Map<Tag>(request);
            //todo checking for the existence of a tag with the same content
            //todo checking for the existence of a user with id
            await _tagsRepository.UpdateAsync(tagEntity);
            
            return new ExecutionResult<Tag>(tagEntity);
        }
        catch (Exception e)
        {
            return new ExecutionResult<Tag>(new ErrorInfo("Error while trying to update a tag.", e.Message));
        }
    }
}