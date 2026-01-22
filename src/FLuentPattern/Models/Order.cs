namespace FluentPattern.Models;

/// <summary>
/// Représente une commande avec ses informations de base et son adresse de livraison.
/// </summary>
public class Order
{
    public int Number { get; init; }
    public DateTime CreatedAt { get; init; }
    public required Address ShippingAddress { get; set; }
}
