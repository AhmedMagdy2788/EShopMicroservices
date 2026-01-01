namespace Ordering.Domain.Events;

public class OrderUpdatedEvent(Order order) : DomainEvent
{
    public Order Order { get; init; } = order;
}