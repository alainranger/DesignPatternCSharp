using FluentPattern.Models;

namespace FluentPattern.Builders;

/// <summary>
/// Builder fluent pour créer des instances d'Address.
/// </summary>
public class AddressBuilder
{
    private string? _street;
    private string? _city;
    private string? _postalCode;
    private string? _country;

    private AddressBuilder() { }

    /// <summary>
    /// Crée une nouvelle instance vide du builder.
    /// </summary>
    public static AddressBuilder Empty() => new();

    /// <summary>
    /// Définit la rue de l'adresse.
    /// </summary>
    public AddressBuilder SetStreet(string street)
    {
        _street = street;
        return this;
    }

    /// <summary>
    /// Définit la ville de l'adresse.
    /// </summary>
    public AddressBuilder SetCity(string city)
    {
        _city = city;
        return this;
    }

    /// <summary>
    /// Définit le code postal de l'adresse.
    /// </summary>
    public AddressBuilder SetPostalCode(string postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    /// <summary>
    /// Définit le pays de l'adresse.
    /// </summary>
    public AddressBuilder SetCountry(string country)
    {
        _country = country;
        return this;
    }

    /// <summary>
    /// Construit l'instance finale d'Address.
    /// </summary>
    public Address Build()
    {
        return new Address
        {
            Street = _street!,
            City = _city!,
            PostalCode = _postalCode!,
            Country = _country!
        };
    }
}
