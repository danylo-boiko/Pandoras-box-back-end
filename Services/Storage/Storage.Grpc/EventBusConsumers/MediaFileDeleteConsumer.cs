using EventBus.Messages.Events;
using MassTransit;

namespace Storage.Grpc.EventBusConsumers;

public class MediaFileDeleteConsumer : IConsumer<MediaFileDeleteEvent>
{
    public Task Consume(ConsumeContext<MediaFileDeleteEvent> context)
    {
        return Task.Run(() => File.Delete(context.Message.FileLocation));
    }
}