namespace Ordering.Domain.Events;

public class OrderCreatedEvent(Order order) : DomainEvent
{
    public Order Order { get; init; } = order;
}