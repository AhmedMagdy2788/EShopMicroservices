namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;

    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value == Guid.Empty
            ? throw new DomainException("CustomerId cannot be empty.")
            : new CustomerId(value);
    }
};