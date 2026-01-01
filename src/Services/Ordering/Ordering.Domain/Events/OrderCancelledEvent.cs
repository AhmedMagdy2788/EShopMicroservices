namespace Ordering.Domain.Events;

public class OrderCancelledEvent(Order order) : DomainEvent
{
    public Order Order { get; init; } = order;
}