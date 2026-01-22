# Design Patterns en C #

Ce dépôt contient des implémentations de divers design patterns en C#, avec des exemples pratiques et de la documentation.

## 📚 Patterns Implémentés

### 1. **Fluent Pattern (Builder Pattern)**

- **Emplacement**: [`src/FLuentPattern`](src/FLuentPattern)
- **Description**: Implémentation du pattern Builder avec une interface fluide pour construire des objets complexes
- **Namespace**: `FluentPattern.Models`, `FluentPattern.Builders`

### 2. **Unit of Work Pattern**

- **Emplacement**: [`src/UnitOfWork`](src/UnitOfWork)
- **Description**: Pattern pour gérer des opérations transactionnelles avec support de rollback automatique
- **Namespace**: `UnitOfWork.Core`, `UnitOfWork.App.Services`, `UnitOfWork.App.Operations`

## 🏗️ Structure du Projet

```text
DesignPatternCSharp/
├── DesignPatternCSharp.slnx     # Solution principale
└── src/
    ├── FLuentPattern/            # Pattern Builder Fluide
    │   ├── Models/              # Modèles de domaine
    │   ├── Builders/            # Builders fluents
    │   ├── Program.cs           # Démonstration
    │   └── README.md            # Documentation spécifique
    └── UnitOfWork/              # Pattern Unit of Work
        ├── UnitOfWork.Core/     # Interfaces et implémentation du pattern
        ├── UnitOfWork.App/      # Application de démonstration
        ├── UnitOfWork.Tests/    # Tests unitaires
        ├── PATTERN.md           # Documentation du pattern
        ├── README.md            # Guide d'utilisation
        └── TESTS.md             # Documentation des tests
```

## 🚀 Démarrage Rapide

### Prérequis

- .NET 9.0 ou supérieur
- Un IDE compatible (Visual Studio, VS Code, Rider)

### Exécution

#### Fluent Pattern

```bash
cd src/FLuentPattern
dotnet run
```

#### Unit of Work

```bash
cd src/UnitOfWork
dotnet run --project UnitOfWork.App
```

#### Tests du Unit of Work

```bash
cd src/UnitOfWork
dotnet test
```

## 📖 Documentation

Chaque pattern dispose de sa propre documentation :

- Fluent Pattern: [src/FLuentPattern/README.md](src/FLuentPattern/README.md)
- Unit of Work: [src/UnitOfWork/README.md](src/UnitOfWork/README.md)

## 🎯 Objectifs

Ce dépôt a pour but de :

1. Fournir des exemples concrets de design patterns
2. Démontrer les meilleures pratiques en C#
3. Servir de référence pour l'apprentissage et l'implémentation

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à :

- Ajouter de nouveaux patterns
- Améliorer la documentation
- Corriger des bugs ou optimiser le code

## 📄 Licence

Ce projet est fourni à des fins éducatives.
