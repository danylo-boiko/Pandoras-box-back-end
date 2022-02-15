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
            if (await _tagsRepository.GetAsync(request.Id) is null)
            {
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with id: {request.Id} is not exist."));
            }
            
            if (await _tagsRepository.GetAsync(request.Content) is not null)
            {
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with content: {request.Content} already exist."));
            }
            
            var tagEntity = _mapper.Map<Tag>(request);

            await _tagsRepository.UpdateAsync(tagEntity);

            var updatedTag = await _tagsRepository.GetAsync(request.Id);
            return new ExecutionResult<Tag>(updatedTag);
        }
        catch (Exception e)
        {
            return new ExecutionResult<Tag>(new ErrorInfo("Error while trying to update a tag.", e.Message));
        }
    }
}