public class AddressBuilder
{
    private string _street;
    private string _city;
    private string _postalCode;
    private string _country;

    private AddressBuilder() { }

    public static AddressBuilder Empty() => new();

    public AddressBuilder SetStreet(string street)
    {
        _street = street;
        return this;
    }

    public AddressBuilder SetCity(string city)
    {
        _city = city;
        return this;
    }

    public AddressBuilder SetPostalCode(string postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    public AddressBuilder SetCountry(string country)
    {
        _country = country;
        return this;
    }

    public Address Build()
    {
        return new Address
        {
            Street = _street,
            City = _city,
            PostalCode = _postalCode,
            Country = _country
        };
    }
}