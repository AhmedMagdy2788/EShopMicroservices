namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCanceledEventHandler(ILogger<OrderCanceledEventHandler> logger)
    : INotificationHandler<OrderCancelledEvent>
{
    public Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}