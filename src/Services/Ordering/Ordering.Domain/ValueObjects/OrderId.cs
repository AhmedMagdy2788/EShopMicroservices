namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    private OrderId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static OrderId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value == Guid.Empty
            ? throw new DomainException("OrderId cannot be empty.")
            : new OrderId(value);
    }
}