# Fluent Pattern (Builder Pattern)

Ce projet démontre l'implémentation du pattern **Fluent Builder** en C#.

## 📁 Structure du projet

```text
FluentPattern/
├── Models/              # Modèles de données
│   ├── Order.cs        # Modèle de commande
│   └── Address.cs      # Modèle d'adresse
├── Builders/           # Builders fluents
│   ├── OrderBuilder.cs    # Builder pour créer des commandes
│   └── AddressBuilder.cs  # Builder pour créer des adresses
└── Program.cs          # Point d'entrée de l'application
```

## 🎯 Objectif

Le pattern Fluent Builder permet de :

- Construire des objets complexes de manière lisible et expressive
- Enchaîner les appels de méthodes (méthode chaînage)
- Séparer la construction d'un objet de sa représentation

## 💡 Utilisation

```csharp
var order = OrderBuilder.Empty()
    .WithNumber(10)
    .CreatedAt(DateTime.UtcNow)
    .ShippedTo(b =>
    {
        b.SetStreet("123 Main St")
         .SetCity("Anytown")
         .SetPostalCode("12345")
         .SetCountry("USA");
    })
    .Build();
```

## 🏃 Exécution

```bash
dotnet run
```
