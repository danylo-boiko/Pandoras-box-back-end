using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;
using Tags.Core.Database.Entities;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Commands.DeleteTag;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, ExecutionResult>
{
    private readonly ILogger<DeleteTagCommandHandler> _logger;
    private readonly ITagsRepository _tagsRepository;

    public DeleteTagCommandHandler(ILogger<DeleteTagCommandHandler> logger, ITagsRepository tagsRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
    }
    
    public async Task<ExecutionResult> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existTag = await _tagsRepository.GetAsync(request.Id);
            
            if (existTag is null)
            {
                _logger.LogError("Tag with id: {Id} is not exist", request.Id);
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with id: {request.Id} is not exist."));
            }
            
            await _tagsRepository.DeleteAsync(request.Id);
            
            _logger.LogInformation("Tag {Tag} (id:{Id}) has been successfully deleted", existTag.Content, existTag.Id);
            return new ExecutionResult(new ErrorInfo($"Tag {existTag.Content} (id:{existTag.Id}) has been successfully deleted."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while trying to delete a tag.", e.Message));
        }
    }
}