using FluentPattern.Models;

namespace FluentPattern.Builders;

/// <summary>
/// Builder fluent pour créer des instances d'Order avec configuration de l'adresse de livraison.
/// </summary>
public class OrderBuilder
{
    private int _number;
    private DateTime _createdAt;
    private readonly AddressBuilder _addressBuilder = AddressBuilder.Empty();

    private OrderBuilder() { }

    /// <summary>
    /// Crée une nouvelle instance vide du builder.
    /// </summary>
    public static OrderBuilder Empty() => new();

    /// <summary>
    /// Définit le numéro de commande.
    /// </summary>
    public OrderBuilder WithNumber(int number)
    {
        _number = number;
        return this;
    }

    /// <summary>
    /// Définit la date de création de la commande.
    /// </summary>
    public OrderBuilder CreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    /// <summary>
    /// Configure l'adresse de livraison via un delegate.
    /// </summary>
    public OrderBuilder ShippedTo(Action<AddressBuilder> action)
    {
        action(_addressBuilder);
        return this;
    }

    /// <summary>
    /// Construit l'instance finale d'Order.
    /// </summary>
    public Order Build()
    {
        return new Order
        {
            Number = _number,
            CreatedAt = _createdAt,
            ShippingAddress = _addressBuilder.Build()
        };
    }
}
