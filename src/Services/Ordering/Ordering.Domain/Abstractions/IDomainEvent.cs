namespace Ordering.Domain.Abstractions;

public abstract class DomainEvent: INotification
{
    public Guid EventId { get; init; }
    public DateTime OccurredOn { get; init; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
    public string EventType
    {
        get
        {
            var assemblyQualifiedName = GetType().AssemblyQualifiedName;
            return assemblyQualifiedName ?? string.Empty;
        }
    }
}