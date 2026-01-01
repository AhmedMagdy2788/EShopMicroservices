namespace Ordering.Domain.ValueObjects;

public record OrderItemId
{
    public Guid Value { get; }
    private OrderItemId(Guid value) => Value = value;

    public static OrderItemId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value == Guid.Empty
            ? throw new DomainException("OrderItemId cannot be empty.")
            : new OrderItemId(value);
    }
}