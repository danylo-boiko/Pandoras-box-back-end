using EventBus.Messages.Events;
using LS.Helpers.Hosting.API;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Videos.Core.Database;
using ExecutionResult = LS.Helpers.Hosting.API.ExecutionResult;

namespace Videos.Core.CQRS.Commands.DeleteVideo;

public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand, ExecutionResult>
{
    private readonly ILogger<DeleteVideoCommandHandler> _logger;
    private readonly VideosDbContext _videosDbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteVideoCommandHandler(
        ILogger<DeleteVideoCommandHandler> logger, 
        VideosDbContext videosDbContext, 
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    public async Task<ExecutionResult> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _videosDbContext.Videos.FirstOrDefaultAsync(v => v.Id.Equals(request.Id)) is null)
            {
                _logger.LogError("Video with id: {Id} is not exist", request.Id);
                return new ExecutionResult(new ErrorInfo($"Video with id: {request.Id} is not exist."));
            }

            var existVideo = await _videosDbContext.Videos.FindAsync(request.Id);

            var eventMessage = new MediaFileDeleteEvent
            {
                AuthorId = existVideo.AuthorId,
                FileLocation = existVideo.VideoUrl
            };

            await _publishEndpoint.Publish<MediaFileDeleteEvent>(eventMessage);
            
            _videosDbContext.Videos.Remove(existVideo);
            await _videosDbContext.SaveChangesAsync();
            
            _logger.LogInformation("Tag with id: {Id} has been deleted successfully", existVideo.Id);
            return new ExecutionResult(new ErrorInfo($"Video with id: {request.Id} has been deleted successfully."));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while trying to delete a video.", e.Message));
        }
    }
}