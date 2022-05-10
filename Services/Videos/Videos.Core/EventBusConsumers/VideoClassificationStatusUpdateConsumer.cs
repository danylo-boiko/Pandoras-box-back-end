using EventBus.Messages.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Videos.Core.Database;
using Videos.Core.Enums;

namespace Videos.Core.EventBusConsumers;

public class VideoClassificationStatusUpdateConsumer : IConsumer<VideoClassificationStatusUpdateEvent>
{
    private readonly ILogger<VideoClassificationStatusUpdateConsumer> _logger;
    private readonly VideosDbContext _videosDbContext;

    public VideoClassificationStatusUpdateConsumer(
        ILogger<VideoClassificationStatusUpdateConsumer> logger,
        VideosDbContext videosDbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _videosDbContext = videosDbContext ?? throw new ArgumentNullException(nameof(videosDbContext));
    }
    
    public async Task Consume(ConsumeContext<VideoClassificationStatusUpdateEvent> context)
    {
        var video = await _videosDbContext.Videos.FirstOrDefaultAsync(v => v.Id.Equals(context.Message.VideoId));
        video.ClassificationStatus = (ClassificationStatus)context.Message.ClassificationStatusCode;
        await _videosDbContext.SaveChangesAsync();
        _logger.LogInformation("Classification status for video with id {Id} has been successfully updated", context.Message.VideoId);
    }
}