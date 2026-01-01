namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; }
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public bool IsEditable() => Status == OrderStatus.Draft;
    public bool CanBeCancelled() => Status is OrderStatus.Pending or OrderStatus.Draft;

    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }
    // In Order class


    public static Order Create(
        OrderId id,
        CustomerId customerId,
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment
    )
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending,
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Update(
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment,
        OrderStatus status
    )
    {
        // Can't update cancelled orders
        if (Status == OrderStatus.Cancelled)
            throw new DomainException("Cannot update a cancelled order");

        // Can't go from Completed back to Pending
        if (Status == OrderStatus.Completed && status == OrderStatus.Pending)
            throw new DomainException("Cannot move completed order back to pending");
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = status;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void AddItem(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var orderItem = new OrderItem(Id, productId, quantity, price);
        _orderItems.Add(orderItem);
    }

    public void Remove(ProductId productId)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (orderItem is not null)
        {
            _orderItems.Remove(orderItem);
        }
    }

    public void UpdateOrderItemQuantity(ProductId productId, int newQuantity)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (orderItem is null)
            throw new DomainException("Order item not found");

        orderItem.UpdateQuantity(newQuantity);
    }

    public void Cancel()
    {
        if (!CanBeCancelled())
            throw new DomainException($"Cannot cancel order in {Status} status");

        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelledEvent(this));
    }
}