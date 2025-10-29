# Unit of Work Pattern - Implémentation C #

Ce projet démontre l'implémentation du **pattern Unit of Work** avec des opérations réversibles en C#.

## 🏗️ Architecture

```
UnitOfWork/
├── UnitOfWork.Core/              # 📦 Bibliothèque réutilisable
│   ├── IReversibleOperation.cs   # Interface des opérations réversibles
│   ├── IUnitOfWork.cs            # Interface du pattern
│   └── UnitOfWorkManager.cs      # Implémentation du pattern
│
├── UnitOfWork.App/               # 🚀 Application de démonstration
│   ├── Program.cs                # Point d'entrée
│   ├── Services/                 # Services métier
│   │   ├── WalletService.cs      # Gestion des portefeuilles
│   │   ├── InventoryService.cs   # Gestion des stocks
│   │   └── PurchaseService.cs    # Coordination des achats
│   └── Operations/               # Opérations réversibles métier
│       ├── DebitWalletOperation.cs
│       └── ReserveInventoryOperation.cs
│
└── UnitOfWork.Tests/             # 🧪 Tests unitaires
    ├── PurchaseServiceTests.cs
    └── UnitOfWorkManagerTests.cs
```

## 📚 Composants

### **UnitOfWork.Core** (Bibliothèque réutilisable)

Framework générique pour gérer des transactions avec rollback automatique :

- **IReversibleOperation** : Interface pour les opérations Do/Undo
- **IUnitOfWork** : Interface du pattern Unit of Work
- **UnitOfWorkManager** : Implémentation avec support du logging optionnel

### **UnitOfWork.App** (Application métier)

Exemple concret d'utilisation avec un système d'achat :

- **Services** : Logique métier (Wallet, Inventory, Purchase)
- **Operations** : Opérations métier réversibles

### **UnitOfWork.Tests**

Tests unitaires xUnit couvrant :

- ✅ Transactions réussies
- ✅ Rollback automatique en cas d'erreur
- ✅ Opérations multiples
- ✅ Gestion des erreurs

## 🚀 Utilisation

### Exécuter l'application

```bash
dotnet run --project UnitOfWork.App
```

### Exécuter les tests

```bash
dotnet test
```

### Compiler la solution

```bash
dotnet build
```

## 💡 Exemple d'utilisation

```csharp
using UnitOfWork.Core;

// Créer une unité de travail avec logging
var unitOfWork = new UnitOfWorkManager(Console.WriteLine);

// Enregistrer les opérations
unitOfWork.Register(new DebitWalletOperation(wallet, "user-1", 30m));
unitOfWork.Register(new ReserveInventoryOperation(inventory, "sku-ABC", 1));

// Exécuter (rollback automatique si erreur)
unitOfWork.Commit();
```

## 🎯 Avantages

✅ **Atomicité** : Toutes les opérations réussissent ou échouent ensemble  
✅ **Cohérence** : Pas d'état incohérent entre les services  
✅ **Réutilisabilité** : Framework générique dans UnitOfWork.Core  
✅ **Extensibilité** : Facile d'ajouter de nouvelles opérations  
✅ **Testabilité** : Code bien structuré et testable  

## 📦 Dépendances

- **.NET 9.0**
- **xUnit** (tests)

## 🧪 Tests

Le projet inclut 11 tests unitaires couvrant :

- Cas de succès
- Rollback sur erreur
- Opérations multiples
- Validation des paramètres
