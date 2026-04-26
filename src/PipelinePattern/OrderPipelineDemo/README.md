# Pipeline Design Pattern en .NET

Ce projet illustre l'implémentation du Pattern Pipeline pour le traitement séquentiel de données complexes.

## Pourquoi utiliser ce pattern ?

Le pattern Pipeline est particulièrement utile lorsque vous avez un processus métier qui doit être découpé en plusieurs étapes indépendantes.

Responsabilité Unique (SRP) : Chaque classe Step ne gère qu'une seule tâche.

Composabilité : Vous pouvez facilement ajouter, retirer ou réorganiser les étapes sans modifier la logique des autres étapes.

Maintenabilité : Le code est plus facile à tester de manière isolée.

## Fonctionnement

L'objet de contexte (Order) contient l'état initial et accumule les transformations.

L'interface IStep définit le contrat pour chaque maillon de la chaîne.

Le Pipeline itère sur les étapes. Si une étape échoue ou marque l'objet comme invalide, le processus peut être stoppé prématurément (Short-circuiting).

## Améliorations possibles

Injection de Dépendances : Utiliser IServiceProvider pour injecter automatiquement les étapes.

Gestion d'erreurs : Ajouter des blocs try-catch dans le pipeline pour logger précisément quelle étape a échoué.

Middleware : Ce pattern est très similaire au fonctionnement des Middlewares dans ASP.NET Core.