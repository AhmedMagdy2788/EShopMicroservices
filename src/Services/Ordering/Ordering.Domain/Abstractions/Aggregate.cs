namespace Ordering.Domain.Abstractions;

public abstract class Aggregate<T>: Entity<T>, IAggregate<T>
{
    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public DomainEvent[] ClearDomainEvents()
    {
        var dequeuedEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeuedEvents;
    }
}