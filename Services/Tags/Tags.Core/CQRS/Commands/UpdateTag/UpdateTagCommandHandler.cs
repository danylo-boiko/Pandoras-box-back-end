using AutoMapper;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;
using Tags.Core.Database.Entities;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Commands.UpdateTag;

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, ExecutionResult<Tag>>
{
    private readonly ILogger<UpdateTagCommandHandler> _logger;
    private readonly ITagsRepository _tagsRepository;
    private readonly IMapper _mapper;
    
    public UpdateTagCommandHandler(ILogger<UpdateTagCommandHandler> logger, ITagsRepository tagsRepository, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<ExecutionResult<Tag>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _tagsRepository.GetAsync(request.Id) is null)
            {
                _logger.LogError("Tag with id: {Id} is not exist", request.Id);
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with id: {request.Id} is not exist."));
            }
            
            var existTag = await _tagsRepository.GetAsync(request.Content);
            
            if (existTag is not null)
            {
                _logger.LogError("Tag with content: {Content} already exist, id: {Id}", request.Content, existTag.Id);
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with content: {request.Content} already exist, id: {existTag.Id}."));
            }
            
            var tagEntity = _mapper.Map<Tag>(request);
            await _tagsRepository.UpdateAsync(tagEntity);

            var updatedTag = await _tagsRepository.GetAsync(request.Id);
            
            _logger.LogInformation("Tag with id: {Id} has been successfully updated to {NewContent}", updatedTag.Id, updatedTag.Content);
            return new ExecutionResult<Tag>(updatedTag);
        }
        catch (Exception e)
        {
            return new ExecutionResult<Tag>(new ErrorInfo("Error while trying to update a tag.", e.Message));
        }
    }
}