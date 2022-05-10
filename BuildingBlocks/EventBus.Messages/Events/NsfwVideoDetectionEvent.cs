namespace EventBus.Messages.Events;

public class NsfwVideoDetectionEvent : IntegrationBaseEvent
{
    public int VideoId { get; set; }
    public int AuthorId { get; set; }
    public string VideoLocation { get; set; }
}