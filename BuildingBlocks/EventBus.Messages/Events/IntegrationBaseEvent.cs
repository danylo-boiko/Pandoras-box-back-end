namespace EventBus.Messages.Events;

public class IntegrationBaseEvent
{
    public Guid Id { get; protected set; }
    public DateTime CreationDate { get; protected set; }

    public IntegrationBaseEvent() : this(Guid.NewGuid(), DateTime.Now)
    {
    }

    public IntegrationBaseEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}