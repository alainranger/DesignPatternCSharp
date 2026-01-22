namespace FleuntPattern;

public class Address
{
    public required string Street { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
}

public class AddressBuilder
{
    private string _street;
    private string _postalCode;
    private string _country;

    public AddressBuilder SetStreet(string? street)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(street));
        _street = street;
        return this;
    }

    public AddressBuilder SetPostalCode(string? postalCode)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(postalCode));
        _postalCode = postalCode;
        return this;
    }

    public AddressBuilder SetCountry(string country)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(country));
        _country = country;
        return this;
    }

    public Address Build()
    {
        // Implementation here
        return new Address
        {
            Street = _street,
            PostalCode = _postalCode,
            Country = _country
        };
    }
}