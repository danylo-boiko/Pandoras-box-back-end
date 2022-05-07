using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Videos.Core.Database;

namespace Videos.Core.CQRS.Commands.DeleteVideo;

public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand, ExecutionResult>
{
    private readonly VideosDbContext _videosDbContext;

    public DeleteVideoCommandHandler(VideosDbContext videosDbContext)
    {
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }
    
    public async Task<ExecutionResult> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _videosDbContext.Videos.FirstOrDefaultAsync(v=>v.Id.Equals(request.Id)) is null)
            {
                return new ExecutionResult(new ErrorInfo($"Video with id: {request.Id} is not exist."));
            }
            
            var existVideo = await _videosDbContext.Videos.FindAsync(request.Id);
            _videosDbContext.Videos.Remove(existVideo);
            await _videosDbContext.SaveChangesAsync();
            
            // todo delete file from storage via RabbitMQ
            
            return new ExecutionResult(new ErrorInfo($"Video with id: {request.Id} has been deleted successfully."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while trying to delete a video.", e.Message));
        }
    }
}