namespace Ordering.Domain.ValueObjects;

public record OrderItemId
{
    private OrderItemId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static OrderItemId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value == Guid.Empty
            ? throw new DomainException("OrderItemId cannot be empty.")
            : new OrderItemId(value);
    }
}