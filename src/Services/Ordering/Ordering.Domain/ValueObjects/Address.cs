namespace Ordering.Domain.ValueObjects;

public sealed record Address
{
    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public string? EmailAddress { get; } = null!;
    public string AddressLine { get; } = null!;
    public string Country { get; } = null!;
    public string State { get; } = null!;
    public string ZipCode { get; } = null!;
    
    private Address(
        string firstName,
        string lastName,
        string emailAddress,
        string addressLine,
        string country,
        string state,
        string zipCode
    )
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }

    public static Address Of(
        string firstName,
        string lastName,
        string emailAddress,
        string addressLine,
        string country,
        string state,
        string zipCode
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName, EmailAddress, AddressLine, Country, State, ZipCode);
    }
}