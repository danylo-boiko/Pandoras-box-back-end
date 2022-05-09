using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;
using Tags.Core.Database.Entities;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Queries.GetTagById;

public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, ExecutionResult<Tag>>
{
    private readonly ILogger<GetTagByIdQueryHandler> _logger;
    private readonly ITagsRepository _tagsRepository;

    public GetTagByIdQueryHandler(ILogger<GetTagByIdQueryHandler> logger, ITagsRepository tagsRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
    }
    
    public async Task<ExecutionResult<Tag>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tag = await _tagsRepository.GetAsync(request.Id);

            if (tag is null)
            {
                _logger.LogError("Tag with id: {Id} is not exist", request.Id);
                return new ExecutionResult<Tag>(new ErrorInfo($"Tag with id: {request.Id} is not exist."));
            }
            
            return new ExecutionResult<Tag>(tag);
        }
        catch (Exception e)
        {
            return new ExecutionResult<Tag>(new ErrorInfo("Error while trying to get a tag.", e.Message));
        }
    }
}