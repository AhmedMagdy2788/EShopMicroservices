namespace Ordering.Domain.ValueObjects;

public record ProductId
{
    private ProductId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ProductId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value == Guid.Empty
            ? throw new DomainException("ProductId cannot be empty.")
            : new ProductId(value);
    }
}