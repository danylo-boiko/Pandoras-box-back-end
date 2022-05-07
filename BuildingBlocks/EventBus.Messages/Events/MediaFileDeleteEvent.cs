namespace EventBus.Messages.Events;

public class MediaFileDeleteEvent : IntegrationBaseEvent
{
    public int AuthorId { get; set; }
    public string FileLocation { get; set; }
}