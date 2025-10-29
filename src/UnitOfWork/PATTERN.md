# Pattern Unit of Work avec Opérations Réversibles

## 📚 Vue d'ensemble

Le **Unit of Work** est un pattern architectural qui maintient une liste d'objets affectés par une transaction métier et coordonne l'écriture des changements. Il garantit que soit **toutes les opérations réussissent**, soit **aucune ne s'applique** (principe ACID).

Dans cette implémentation, nous combinons le Unit of Work avec le **pattern Command** et des **opérations réversibles** (Do/Undo) pour permettre un rollback automatique en cas d'échec.

## 🎯 Problème résolu

### Sans Unit of Work

```
❌ Opération 1 : Débiter le portefeuille → ✅ Réussit
❌ Opération 2 : Réserver le stock → ❌ ÉCHEC (stock insuffisant)

Résultat : Le portefeuille est débité mais le stock n'est pas réservé
          → État incohérent !
```

### Avec Unit of Work

```
✅ Opération 1 : Débiter le portefeuille → ✅ Réussit
✅ Opération 2 : Réserver le stock → ❌ ÉCHEC
   → Rollback automatique
   → Undo Opération 1 : Créditer le portefeuille

Résultat : Toutes les modifications sont annulées
          → État cohérent !
```

## 🏗️ Architecture

### Diagramme de classes

```mermaid
classDiagram
    class IReversibleOperation {
        <<interface>>
        +Do() void
        +Undo() void
    }
    
    class IUnitOfWork {
        <<interface>>
        +Register(operation) void
        +Commit() void
        +Rollback() void
    }
    
    class UnitOfWorkManager {
        -Stack~IReversibleOperation~ executedOperations
        -Action~string~ logger
        +Register(operation) void
        +Commit() void
        +Rollback() void
    }
    
    class DebitWalletOperation {
        -WalletService wallet
        -string userId
        -decimal amount
        +Do() void
        +Undo() void
    }
    
    class ReserveInventoryOperation {
        -InventoryService inventory
        -string sku
        -int quantity
        +Do() void
        +Undo() void
    }
    
    class PurchaseService {
        -WalletService wallet
        -InventoryService inventory
        +Purchase(userId, sku, quantity, price)
    }
    
    IReversibleOperation <|.. DebitWalletOperation
    IReversibleOperation <|.. ReserveInventoryOperation
    IUnitOfWork <|.. UnitOfWorkManager
    UnitOfWorkManager o-- IReversibleOperation
    PurchaseService --> UnitOfWorkManager
    PurchaseService --> DebitWalletOperation
    PurchaseService --> ReserveInventoryOperation
```

## 🔄 Flux d'exécution

### Scénario de succès

```mermaid
sequenceDiagram
    participant Client
    participant PurchaseService
    participant UnitOfWork
    participant Operation1 as DebitWallet
    participant Operation2 as ReserveInventory
    participant WalletService
    participant InventoryService

    Client->>PurchaseService: Purchase(user, sku, qty, price)
    PurchaseService->>UnitOfWork: new UnitOfWorkManager()
    
    PurchaseService->>Operation1: new DebitWalletOperation(...)
    PurchaseService->>UnitOfWork: Register(Operation1)
    
    PurchaseService->>Operation2: new ReserveInventoryOperation(...)
    PurchaseService->>UnitOfWork: Register(Operation2)
    
    PurchaseService->>UnitOfWork: Commit()
    
    UnitOfWork->>Operation1: Do()
    Operation1->>WalletService: Debit(user, amount)
    WalletService-->>Operation1: ✅ Success
    Operation1-->>UnitOfWork: ✅ Done
    Note over UnitOfWork: Push to executedOperations
    
    UnitOfWork->>Operation2: Do()
    Operation2->>InventoryService: Reserve(sku, qty)
    InventoryService-->>Operation2: ✅ Success
    Operation2-->>UnitOfWork: ✅ Done
    Note over UnitOfWork: Push to executedOperations
    
    UnitOfWork-->>PurchaseService: ✅ Commit successful
    PurchaseService-->>Client: ✅ Purchase completed
```

### Scénario avec rollback

```mermaid
sequenceDiagram
    participant Client
    participant PurchaseService
    participant UnitOfWork
    participant Operation1 as DebitWallet
    participant Operation2 as ReserveInventory
    participant WalletService
    participant InventoryService

    Client->>PurchaseService: Purchase(user, sku, qty, price)
    PurchaseService->>UnitOfWork: new UnitOfWorkManager()
    
    PurchaseService->>Operation1: new DebitWalletOperation(...)
    PurchaseService->>UnitOfWork: Register(Operation1)
    
    PurchaseService->>Operation2: new ReserveInventoryOperation(...)
    PurchaseService->>UnitOfWork: Register(Operation2)
    
    PurchaseService->>UnitOfWork: Commit()
    
    UnitOfWork->>Operation1: Do()
    Operation1->>WalletService: Debit(user, amount)
    WalletService-->>Operation1: ✅ Success
    Operation1-->>UnitOfWork: ✅ Done
    Note over UnitOfWork: Push to Stack
    
    UnitOfWork->>Operation2: Do()
    Operation2->>InventoryService: Reserve(sku, qty)
    InventoryService-->>Operation2: ❌ InsufficientStockException
    Operation2-->>UnitOfWork: ❌ Exception
    
    Note over UnitOfWork: ROLLBACK STARTED
    
    UnitOfWork->>Operation1: Undo()
    Operation1->>WalletService: Credit(user, amount)
    WalletService-->>Operation1: ✅ Restored
    Operation1-->>UnitOfWork: ✅ Undone
    Note over UnitOfWork: Pop from Stack
    
    UnitOfWork-->>PurchaseService: ❌ Exception thrown
    PurchaseService-->>Client: ❌ Purchase failed (rollback done)
```

## 🔧 Composants clés

### 1. Interface IReversibleOperation

Définit le contrat pour les opérations réversibles :

```csharp
public interface IReversibleOperation
{
    void Do();    // Exécute l'opération
    void Undo();  // Annule l'opération
}
```

**Principe** : Chaque opération doit savoir comment s'exécuter ET comment s'annuler.

### 2. Interface IUnitOfWork

Définit le contrat pour le gestionnaire de transactions :

```csharp
public interface IUnitOfWork
{
    void Register(IReversibleOperation operation);  // Ajoute une opération
    void Commit();                                  // Exécute tout
    void Rollback();                                // Annule tout
}
```

### 3. UnitOfWorkManager

Implémentation concrète qui :

- **Enregistre** les opérations dans l'ordre
- **Exécute** toutes les opérations séquentiellement
- **Empile** les opérations réussies dans une Stack
- **Rollback** en dépilant et en appelant Undo() en ordre inverse

```mermaid
graph TD
    A[Commit appelé] --> B{Exécuter Op1}
    B -->|✅ Success| C[Push Op1 sur Stack]
    C --> D{Exécuter Op2}
    D -->|✅ Success| E[Push Op2 sur Stack]
    E --> F{Exécuter Op3}
    F -->|❌ Échec| G[ROLLBACK]
    G --> H[Pop Op2 → Undo]
    H --> I[Pop Op1 → Undo]
    I --> J[Throw Exception]
    
    F -->|✅ Success| K[Transaction complète ✅]
    
    style G fill:#ff6b6b
    style J fill:#ff6b6b
    style K fill:#51cf66
```

### 4. Opérations concrètes

#### DebitWalletOperation

```csharp
public class DebitWalletOperation : IReversibleOperation
{
    public void Do()   => wallet.Debit(userId, amount);   // Retire l'argent
    public void Undo() => wallet.Credit(userId, amount);  // Remet l'argent
}
```

#### ReserveInventoryOperation

```csharp
public class ReserveInventoryOperation : IReversibleOperation
{
    public void Do()   => inventory.Reserve(sku, quantity);  // Réserve le stock
    public void Undo() => inventory.Release(sku, quantity);  // Libère le stock
}
```

## 🎨 État des données pendant le cycle

```mermaid
stateDiagram-v2
    [*] --> Initial: État initial
    Initial --> Op1_Done: Do Operation 1
    Op1_Done --> Op2_Done: Do Operation 2
    Op2_Done --> Committed: Commit réussi ✅
    
    Op1_Done --> Op1_Undone: Échec Op2 → Undo Op1
    Op2_Done --> Op2_Undone: Échec Op3 → Undo Op2
    Op2_Undone --> Op1_Undone: Undo Op1
    
    Op1_Undone --> Initial: Rollback complet
    Op1_Undone --> [*]: Exception propagée
    Committed --> [*]: Transaction terminée
    
    note right of Op1_Done
        Stack: [Op1]
    end note
    
    note right of Op2_Done
        Stack: [Op1, Op2]
    end note
    
    note right of Op1_Undone
        Stack: []
        État restauré
    end note
```

## 📊 Exemple concret

### Configuration initiale

```
Portefeuille de Alice : 100€
Stock de "Laptop" : 5 unités
```

### Achat réussi

```csharp
purchaseService.Purchase("Alice", "Laptop", 2, 50.00m);
```

**Étapes** :

1. ✅ Débiter 100€ du portefeuille d'Alice → Solde : 0€
2. ✅ Réserver 2 "Laptop" → Stock : 3 unités
3. ✅ **Commit réussi**

**Résultat final** :

```
Portefeuille de Alice : 0€      (100 - 100 = 0)
Stock de "Laptop" : 3 unités    (5 - 2 = 3)
```

### Achat avec rollback

```csharp
purchaseService.Purchase("Alice", "Laptop", 10, 50.00m);
```

**Étapes** :

1. ✅ Débiter 500€ → Temporairement : -400€
2. ❌ Réserver 10 "Laptop" → **ÉCHEC** (stock = 3 < 10)
3. 🔄 **Rollback automatique**
   - Undo : Créditer 500€ → Solde restauré : 100€

**Résultat final** :

```
Portefeuille de Alice : 100€    (Restauré à l'état initial)
Stock de "Laptop" : 3 unités    (Aucun changement)
Exception : InsufficientStockException
```

## ✅ Avantages du pattern

| Avantage | Description |
|----------|-------------|
| **🔒 Cohérence** | Garantit que les données restent cohérentes (tout ou rien) |
| **🔄 Réversibilité** | Rollback automatique sans code conditionnel complexe |
| **📦 Encapsulation** | Chaque opération est isolée et testable individuellement |
| **🧩 Extensibilité** | Facile d'ajouter de nouvelles opérations réversibles |
| **🧪 Testabilité** | Chaque composant peut être testé en isolation |
| **📊 Traçabilité** | Logger optionnel pour suivre l'exécution et le rollback |

## 🎯 Cas d'usage

- **Transactions bancaires** : Transfert entre comptes
- **E-commerce** : Achat avec paiement + réservation de stock
- **Réservations** : Hôtel/vol avec paiement
- **Systèmes distribués** : Opérations multi-services (Saga pattern)
- **Gestion de ressources** : Allocation/libération atomique
- **Workflows métier** : Processus en plusieurs étapes

## 🚀 Extensions possibles

### 1. Opérations asynchrones

```csharp
public interface IReversibleOperationAsync
{
    Task DoAsync();
    Task UndoAsync();
}
```

### 2. Gestion des compensations partielles

Certaines opérations ne peuvent pas être complètement annulées (ex: email envoyé). On peut alors :

- Logger les actions pour audit
- Envoyer une notification de compensation
- Marquer l'opération comme "compensée partiellement"

### 3. Retry automatique

```csharp
public class RetryableUnitOfWork : IUnitOfWork
{
    private readonly int maxRetries = 3;
    
    public void Commit()
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                CommitInternal();
                return;
            }
            catch (TransientException ex)
            {
                if (i == maxRetries - 1) throw;
                Rollback();
            }
        }
    }
}
```

### 4. Saga pattern distribué

Pour des opérations sur plusieurs microservices :

```mermaid
graph LR
    A[Service 1] -->|Success| B[Service 2]
    B -->|Success| C[Service 3]
    C -->|Failure| D[Compensate 2]
    D --> E[Compensate 1]
    
    style C fill:#ff6b6b
    style D fill:#ffd43b
    style E fill:#ffd43b
```

## 📚 Références

- **Martin Fowler** - Patterns of Enterprise Application Architecture
- **Domain-Driven Design** - Eric Evans
- **Saga Pattern** - Microservices patterns
- **ACID Transactions** - Database theory

## 🎓 Concepts liés

- **Command Pattern** : Encapsulation d'une requête comme objet
- **Memento Pattern** : Sauvegarde et restauration d'état
- **Transaction Script** : Logique métier organisée par transaction
- **Repository Pattern** : Abstraction de la couche de persistance
- **Two-Phase Commit** : Protocole de transaction distribuée

---

**💡 Principe clé** : "Une transaction métier doit réussir complètement ou échouer complètement, sans laisser le système dans un état incohérent."
