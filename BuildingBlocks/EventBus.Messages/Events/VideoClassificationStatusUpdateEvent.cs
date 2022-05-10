namespace EventBus.Messages.Events;

public class VideoClassificationStatusUpdateEvent : IntegrationBaseEvent
{
    public int VideoId { get; set; }
    public int AuthorId { get; set; }
    public int ClassificationStatusCode { get; set; }
}