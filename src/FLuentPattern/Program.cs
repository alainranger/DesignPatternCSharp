using FluentPattern.Builders;
using FluentPattern.Models;

// ====================================
// Point d'entrée de l'application
// ====================================

// Exemple de création d'une commande simple
var order = OrderBuilder.Empty()
    .WithNumber(10)
    .CreatedAt(DateTime.UtcNow)
    .ShippedTo(b =>
    {
        b
            .SetStreet("123 Main St")
            .SetCity("Anytown")
            .SetPostalCode("12345")
            .SetCountry("USA");
    })
    .Build();

// Exemple de création de plusieurs commandes par lot
List<Order[]> orders = Enumerable.Range(1, 18)
    .Select(number => OrderBuilder.Empty()
        .WithNumber(number)
        .CreatedAt(DateTime.UtcNow)
        .ShippedTo(b =>
        {
            b
                .SetStreet($"{number} Elm St")
                .SetCity("Othertown")
                .SetPostalCode($"6789{number}")
                .SetCountry("USA");
        })
        .Build())
    .Chunk(2)
    .ToList();

Console.WriteLine($"Commande #{order.Number} créée pour {order.ShippingAddress.City}, {order.ShippingAddress.Country}");
Console.WriteLine($"Total de lots de commandes créés: {orders.Count}");
