using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Models;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Queries.GetTagsByPattern;

public class GetTagsByPatternHandler : IRequestHandler<GetTagsByPatternQuery, ExecutionResult<IList<Tag>>>
{
    private readonly ITagsRepository _tagsRepository;

    public GetTagsByPatternHandler(ITagsRepository tagsRepository)
    {
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
    }
    
    public async Task<ExecutionResult<IList<Tag>>> Handle(GetTagsByPatternQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tags = await _tagsRepository.GetAsync(request.Pattern, request.PaginationFilter);
            
            return new ExecutionResult<IList<Tag>>(tags);
        }
        catch (Exception e)
        {
            return new ExecutionResult<IList<Tag>>(new ErrorInfo("Error while trying to get tags.", e.Message));
        }
    }
}