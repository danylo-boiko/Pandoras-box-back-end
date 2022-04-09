using LS.Helpers.Hosting.API;
using MediatR;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.CQRS.Commands.DeleteTag;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, ExecutionResult>
{
    private readonly ITagsRepository _tagsRepository;

    public DeleteTagCommandHandler(ITagsRepository tagsRepository)
    {
        _tagsRepository = tagsRepository ?? throw new ArgumentNullException(nameof(tagsRepository));
    }
    
    public async Task<ExecutionResult> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _tagsRepository.DeleteAsync(request.Id);
            
            return new ExecutionResult(new ErrorInfo($"Tag with id: {request.Id} deleted successfully."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while trying to delete a tag.", e.Message));
        }
    }
}