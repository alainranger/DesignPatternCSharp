namespace FluentPattern.Models;

/// <summary>
/// Représente une adresse postale complète.
/// </summary>
public class Address
{
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }
}
