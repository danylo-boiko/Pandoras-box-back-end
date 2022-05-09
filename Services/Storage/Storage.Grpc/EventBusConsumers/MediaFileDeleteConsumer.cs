using EventBus.Messages.Events;
using MassTransit;

namespace Storage.Grpc.EventBusConsumers;

public class MediaFileDeleteConsumer : IConsumer<MediaFileDeleteEvent>
{
    private readonly ILogger<MediaFileDeleteConsumer> _logger;

    public MediaFileDeleteConsumer(ILogger<MediaFileDeleteConsumer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public Task Consume(ConsumeContext<MediaFileDeleteEvent> context)
    {
        return Task.Run(() =>
        {
            File.Delete(context.Message.FileLocation);
            _logger.LogInformation("{File} has been successfully deleted", context.Message.FileLocation);
        });
    }
}